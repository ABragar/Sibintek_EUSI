using Base.Ambient;
using Base.DAL;
using Base.Entities;
using Base.Enums;
using Base.Identity;
using Base.Identity.Core;
using Base.Identity.Entities;
using Base.Security;
using Base.Security.Service;
using Base.Service.Log;
using CorpProp.WindowsAuth.Extensions;
using CorpProp.WindowsAuth.Helpers;
using CorpProp.WindowsAuth.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Middlewares
{
    public class ADInternalAuthHandler : AuthenticationHandler<ADInternalAuthOptions>
    {
        private const string DEFAULT_AUTH_ERROR_MSG = "Доступ запрещен.";
        private const string USER_NOT_IMPORTED = "Доступ запрещен. Пользователь с логином {0} не был импортирован в систему.";
        private const string USER_NOT_IN_ROLE = "Доступ запрещен. Пользователь с логином {0} не состоит ни в одной разрешенной группе Active Directory.";
        private const string AUTO_REGISTER_ADMIN_FAILS = "Доступ запрещен. Не удалось зарегистрировать в системе нового админа с логином {0}.";

        private readonly IAppContextBootstrapper _appContextBootstrapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IAccountManager _accountManager;
        private readonly IADUserService _adUserService;
        private readonly IAccessUserService _accessUserService;
        private bool IsAuthenticated { get; set; }
        private AuthResult AuthResult { get; set; }
        private string _currentLogin = string.Empty;
        protected string CurrentLogin
        {
            get
            {
                if (string.IsNullOrEmpty(_currentLogin))
                {
                    var fullName = Context.Authentication.User?.Identity?.Name ?? string.Empty;
                    var indexOfBackslash = fullName.LastIndexOf("\\");
                    if (indexOfBackslash > 0)
                    {
                        return fullName.Substring(indexOfBackslash + 1);
                    }
                    _currentLogin = fullName;
                }
                return _currentLogin;
            }
        }

        public ADInternalAuthHandler(IUnitOfWorkFactory unitOfWorkFactory, IAccountManager accountManager,
            IADUserService adUserService, IAccessUserService accessUserService,
            IAppContextBootstrapper appContextBootstrapper)
        {
            _appContextBootstrapper = appContextBootstrapper;
            _unitOfWorkFactory = unitOfWorkFactory;
            _accountManager = accountManager;
            _adUserService = adUserService;
            _accessUserService = accessUserService;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthResult = await AuthentificateInternalAsync();
            if (AuthResult.IsSuccess)
            {
                var identity = new ClaimsIdentity(Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, AuthResult.UserIdValue.ToString()));

                var utcNow = DateTimeOffset.UtcNow;
                var props = new AuthenticationProperties();
                props.IssuedUtc = utcNow;
                props.ExpiresUtc = utcNow.Add(Options.ExpireTimeSpan);
                return new AuthenticationTicket(identity, props);
            }
            return new AuthenticationTicket(null, null);
        }

        public override async Task<bool> InvokeAsync()
        {
            AuthenticationTicket ticket = null;
            try
            {
                if (Options.LoginPath == Request.Path)
                {
                    // реальная авторизация в редиректе
                    ticket = await AuthenticateAsync();
                    if (ValidateTicket(ticket))
                    {
                        WriteTicketToCookie(ticket);
                        ApplyReturnRedirect();
                        return true;
                    }
                }
                else
                {
                    ticket = ReadTicketFromCookie();
                    if (!ValidateTicket(ticket))
                    {
                        // редирект для авторизации по пассивной схеме
                        ApplyRedirect(Options.LoginPath, true);
                    }
                }
                return !IsAuthenticated;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private void ApplyReturnRedirect()
        {
            var redirectPath = new PathString(Request.Query.Get(Options.ReturnUrlParameter) ?? "/");
            ApplyRedirect(redirectPath, false);
        }

        private void ApplyRedirect(PathString redirectPath, bool includeReturnUrl)
        {
            var redirectUri = string.Concat(Request.Scheme, Uri.SchemeDelimiter, Request.Host, Request.PathBase, redirectPath);

            if (includeReturnUrl)
            {
                var currentUri = string.Concat(Request.PathBase, Request.Path, Request.QueryString);
                redirectUri += new QueryString(Options.ReturnUrlParameter, currentUri);
            }

            Response.Redirect(redirectUri);
        }

        protected override Task ApplyResponseGrantAsync()
        {
            if (!IsAuthenticated)
            {
                var errorMsg = DEFAULT_AUTH_ERROR_MSG;
                if (AuthResult != null)
                {
                    if (AuthResult.Messages.Any())
                    {
                        errorMsg = string.Join("\n", AuthResult.Messages);
                    }
                    else
                    {
                        switch (AuthResult.Status)
                        {
                            case AuthStatus.NotFound:
                                {
                                    errorMsg = string.Format(USER_NOT_IMPORTED, CurrentLogin);
                                    break;
                                }
                            case AuthStatus.FailureNotInRole:
                                {
                                    errorMsg = string.Format(USER_NOT_IN_ROLE, CurrentLogin);
                                    break;
                                }
                            case AuthStatus.Failure:
                            default:
                                {
                                    errorMsg = DEFAULT_AUTH_ERROR_MSG;
                                    break;
                                }
                        }
                    }
                }
                DenyAccess(Context, errorMsg);
            }
            return Task.FromResult<object>(null);
        }

        private AuthenticationTicket ReadTicketFromCookie()
        {
            string cookie = Options.CookieManager.GetRequestCookie(Context, Options.CookieName);
            return Options.TicketDataFormat.Unprotect(cookie);
        }

        private void WriteTicketToCookie(AuthenticationTicket ticket)
        {
            string cookieValue = Options.TicketDataFormat.Protect(ticket);
            Options.CookieManager.DeleteCookie(Context, Options.CookieName, new CookieOptions());
            Options.CookieManager.AppendResponseCookie(Context, Options.CookieName, cookieValue, new CookieOptions());
        }

        private bool ValidateTicket(AuthenticationTicket ticket)
        {
            if (ticket != null && ticket.Identity != null && ticket.Properties?.ExpiresUtc != null)
            {
                var currentUtc = DateTimeOffset.UtcNow;
                var expiresUtc = ticket.Properties?.ExpiresUtc.Value;

                if (expiresUtc > currentUtc)
                {
                    // Это нужно для OwinSecurityUserMiddleware
                    // TODO refactor OwinSecurityUserMiddleware, чтобы мог работать с
                    // Helper.AddUserIdentity(ticket.Identity);
                    foreach (var claim in ticket.Identity.Claims)
                    {
                        ((ClaimsIdentity)Context.Authentication.User.Identity).AddClaim(claim);
                    }

                    IsAuthenticated = true;
                    return true;
                }
            }
            return false;
        }

        private async Task<AuthResult> AuthentificateInternalAsync()
        {
            AuthResult authResult;

            using (_appContextBootstrapper.LocalContextSecurity(new SystemUser()))
            {
                if (IsValidUserGroup())
                {
                    authResult = await _accountManager.AuthenticateByLoginAsync(CurrentLogin);
                    if (authResult.IsNotFound && IsAdminPrincipal())
                    {
                        var regResult = await RegisterNewAdmin();
                        if (regResult.IsSucceeded)
                        {
                            authResult = await _accountManager.AuthenticateByLoginAsync(CurrentLogin);
                        }
                    }
                }
                else
                {
                    authResult = AuthResult.FailedNotInRole(null, CurrentLogin, string.Format(USER_NOT_IN_ROLE, CurrentLogin));
                    _accountManager.RegisterAuthEvent(authResult);
                }
            }

            return authResult;
        }

        private async Task<RegisterResult> RegisterNewAdmin()
        {
            try
            {
                using (IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateSystem())
                {
                    User user = await _adUserService.CreateUserFromADUserAsync(unitOfWork, CurrentLogin);
                    if (user == null)
                    {
                        throw new Exception("Не удалось получить пользователя из Active Directory.");
                    }

                    var adminCategory = unitOfWork.GetRepository<UserCategory>().Find(uc => uc.SysName == "admins");
                    user.CategoryID = adminCategory.ID;
                    _accessUserService.Create(unitOfWork, user);
                    var userInfo = new UserInfo()
                    {
                        Login = user.SysName,
                        FirstName = user.Profile?.FirstName,
                        LastName = user.Profile?.LastName,
                        Email = user.Profile?.GetPrimaryEmail()
                    };
                    return await _accountManager.RegisterByUserIdAsync(user.ID, userInfo);
                }
            }
            catch (Exception e)
            {
                throw new AuthenticationException(string.Format(AUTO_REGISTER_ADMIN_FAILS, CurrentLogin), e);
            }
        }

        private static void DenyAccess(IOwinContext context, string errorMessage = "")
        {
            // TODO refactor better route on generic view for erros
            context.Response.ContentType = "text/html; charset=utf-8 ";
            context.Response.Write(errorMessage);
        }

        #region Help Methods
        private static void AddIDClaim(ClaimsPrincipal user, string value)
        {
            ((ClaimsIdentity)user.Identity).AddClaim(new Claim(ClaimTypes.NameIdentifier, value));
        }
        #endregion

        #region AD Groups Checks
        private static bool IsExist(ClaimsPrincipal user)
        {
            return user != null && user.Identity != null &&
                user.Identity.IsAuthenticated;
        }

        private bool IsValidUserGroup()
        {
            var user = Context.Authentication.User;
            return IsExist(user)
                && (ADSettingsHelper.GetADUsersGroups().Any(x => user.IsInRole(x))
                || ADSettingsHelper.GetADAdminsGroups().Any(x => user.IsInRole(x)));
        }

        private bool IsUserPrincipal()
        {
            var user = Context.Authentication.User;
            return IsExist(user) && ADSettingsHelper.GetADUsersGroups().Any(x => user.IsInRole(x));
        }

        private bool IsAdminPrincipal()
        {
            var user = Context.Authentication.User;
            return IsExist(user) && ADSettingsHelper.GetADAdminsGroups().Any(x => user.IsInRole(x));
        }
        #endregion


    }
}
