using Base.DAL;
using Base.Enums;
using Base.Events;
using Base.Events.Auth;
using Base.Identity.Entities;
using Base.Security;
using Base.Settings;
using Base.Utils.Common.Caching;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Base.Entities;
using Base.Events.Registration;

namespace Base.Identity.Core
{
    public class AccountManager : IAccountManager, IEventSource
    {
        #region Prepared Answers
        const string NotFoundMessage = "Пользователь не найден";
        const string BadTokenMessage = "Токен имеет не верный формат или устарел";
        const string OperationNotAllowedMessage = "Операция запрещена";

        private static readonly string[] NeedConfirmMessage = { "Вход без подтверждение регистрации запрещен" };

        private static readonly AuthResult BadTokenResult = AuthResult.Failed(BadTokenMessage);

        private static readonly AuthResult LockedOutResult = new AuthResult(null, null, AuthStatus.LockedOut, "Превышено количество попыток входа");

        private static readonly AuthResult OperationNotAllowedAuthResult = AuthResult.Failed(OperationNotAllowedMessage);

        private static readonly IdentityResult UserNotFoundResult = IdentityResult.Failed(NotFoundMessage);

        private static readonly IdentityResult SendLockedOutResult = IdentityResult.Failed("Превышено количество попыток. Попробуйте позже");

        private static readonly IdentityResult OperationNotAllowedResult = IdentityResult.Failed(OperationNotAllowedMessage);
        #endregion

        //Кэш сбрасываемый раз в час 
        private static readonly CacheAccessor<ConcurrentDictionary<string, LockoutInfo>> LockoutAccessor =
            new CacheAccessor<ConcurrentDictionary<string, LockoutInfo>>(TimeSpan.FromHours(1), false);

        private static readonly CacheAccessor<ConcurrentDictionary<string, LockoutInfo>> SendMessageLockoutAccessor =
            new CacheAccessor<ConcurrentDictionary<string, LockoutInfo>>(TimeSpan.FromHours(1), false);

        private readonly ISimpleCacheWrapper _cache;
        private readonly IUnitOfWorkFactory _factory;
        private readonly IUserManagerOptions _options;
        private readonly ISettingService<AuthSettings> _settings;

        public AccountManager(IUnitOfWorkFactory factory, IUserManagerOptions options, ISettingService<AuthSettings> settings, ISimpleCacheWrapper cache, IEventBus eventBus)
        {
            _factory = factory;
            _options = options;
            _settings = settings;
            _cache = cache;

            InitAuthEvents(eventBus);
        }

        #region Auth Events
        public bool DisableEvents { get; protected set; }
        protected IEventTrigger<OnAccountRegistered<IRegisterResult>> TriggerAccountRegistered { get; set; }
        protected IEventTrigger<OnLogOn<IAuthResult>> TriggerLogOn { get; set; } 
        protected IEventTrigger<OnLogOff<IAuthResult>> TriggerLogOff { get; set; }
        protected IEventTrigger<OnLogOnError<IAuthResult>> TriggerLogOnError { get; set; }

        private void TriggerAuthEvent(IUnitOfWork uow, AuthResult authResult)
        {
            if (!DisableEvents)
            {
                switch (authResult.Status)
                {
                    case AuthStatus.Success:
                    {
                        TriggerLogOn.Raise(() => new OnLogOn<IAuthResult>(uow, authResult));
                        break;
                    }
                    case AuthStatus.SignOut:
                    {
                        TriggerLogOff.Raise(() => new OnLogOff<IAuthResult>(uow, authResult));
                        break;
                    }
                    case AuthStatus.NotFound:
                    case AuthStatus.NeedConfirm:
                    case AuthStatus.LockedOut:
                    case AuthStatus.Failure:
                    case AuthStatus.FailureNotInRole:
                    {
                        TriggerLogOnError.Raise(() => new OnLogOnError<IAuthResult>(uow, authResult));
                        break;
                    }
                    default:
                    {
                        // Unknown event
                        break;
                    }
                }
            }
        }

        private void TriggerRegisterEvent(IUnitOfWork uow, IRegisterResult regResult)
        {
            if (!DisableEvents)
            {
                switch (regResult.Status)
                {
                    case RegisterStatus.AccountRegistered:
                    {
                        TriggerAccountRegistered.Raise(() => new OnAccountRegistered<IRegisterResult>(uow, regResult));
                        break;
                    }
                    default:
                    {
                        // Unknown event
                        break;
                    }
                }
            }
        }

        private void InitAuthEvents(IEventBus eventBus)
        {
            TriggerAccountRegistered = eventBus.GetTrigger<OnAccountRegistered<IRegisterResult>>(this);
            TriggerLogOn = eventBus.GetTrigger<OnLogOn<IAuthResult>>(this);
            TriggerLogOff = eventBus.GetTrigger<OnLogOff<IAuthResult>>(this);
            TriggerLogOnError = eventBus.GetTrigger<OnLogOnError<IAuthResult>>(this);
        }
        #endregion

        public IAuthSettings Settings => _settings.Get();

        public Task<AccountInfo> GetAccountInfoAsync(int user_id)
        {
            return UsingManagerAsync(async x => await x.GetAccountInfoAsync(await x.FindByUserIdAsync(user_id)));
        }

        public async Task<AuthResult> AuthenticateByLoginAsync(UserLoginInfo login_info)
        {

            if (!Settings.ExternalLoginAllowed)
                return OperationNotAllowedAuthResult;

            return await UsingManagerAsync(async x => await GetAuthentificationAsync(x, await x.FindAsync(login_info)));
        }

        public async Task<AuthResult> AuthenticateByLoginAsync(string login)
        {
            return await UsingManagerAsync(async x =>
                await GetAuthentificationAsync(x, await x.FindByNameAsync(login), new AccountInfo() { Login = login})
            );
        }

        public Task<AuthResult> AuthenticateByPasswordAsync(string login, string password, bool check_lockout)
        {

            LockoutInfo lockout_info = null;

            ConcurrentDictionary<string, LockoutInfo> lockouts = _cache.GetOrAdd(LockoutAccessor, null, () => new ConcurrentDictionary<string, LockoutInfo>());

            if (check_lockout && lockouts.TryGetValue(login, out lockout_info))
            {
                if (lockout_info.AccessFailedCount > 3)
                    return Task.FromResult(LockedOutResult);
            }

            return UsingManagerAsync(async x =>
            {

                var account = await x.FindAsync(login, password);

                if (account != null)
                    lockouts.TryRemove(login, out lockout_info);
                else
                {
                    lockouts.AddOrUpdate(login, n => new LockoutInfo(), (n, y) =>
                    {
                        y.AccessFailedCount++;
                        return y;
                    });
                }

                return await GetAuthentificationAsync(x, account, new AccountInfo() { Login = login });
            });
        }

        public async Task<AuthResult> RegisterByLoginAsync(UserInfo info, UserLoginInfo login_info, bool createUser = false)
        {
            if (!Settings.ExternalLoginAllowed)
                return OperationNotAllowedAuthResult;

            if (!Settings.RegistrationAllowed)
                return OperationNotAllowedAuthResult;

            return await UsingManagerAsync(async x =>
            {
                var account = x.GetAccount(info);

                var create_result = await x.CreateAsync(account, login_info);
                if (!create_result.Succeeded)
                    return AuthResult.Failed(create_result.Errors.ToArray());

                if (createUser)
                {
                    await x.CreateUserIdAsync(account);

                    await x.UpdateAsync(account);

                    return new AuthResult(account.UserId, account.UserName, AuthStatus.Success);
                }

                return await GetAuthentificationAsync(x, account);

            });
        }

        public async Task<RegisterResult> RegisterByUserIdAsync(int userId, UserInfo userInfo)
        {
            if (userId < 1)
            {
                throw new ArgumentException("User ID must be valid.", nameof(userId));
            }

            return await UsingManagerAsync(async x =>
            {
                // TODO
                //if (!Settings.RegistrationAllowed)
                //    return RegisterResult.NotAllowed();
                var account = x.GetAccount(userId, userInfo);
                var result = await x.CreateAsync(account);
                if (result.Succeeded)
                {
                    return RegisterResult.AccountRegistered(account);
                }
                return RegisterResult.Failure(account, result.Errors);
            });
        }

        public async Task<AuthResult> RegisterByPasswordAsync(UserInfo info, string password, bool createUser = false)
        {
            if (!Settings.RegistrationAllowed)
                return OperationNotAllowedAuthResult;


            return await UsingManagerAsync(async x =>
            {

                var account = x.GetAccount(info);

                var create_result = await x.CreateAsync(account, password);
                if (!create_result.Succeeded)
                    return AuthResult.Failed(create_result.Errors.ToArray());

                if (createUser)
                {
                    await x.CreateUserIdAsync(account);

                    await x.UpdateAsync(account);

                    return new AuthResult(account.UserId, account.UserName, AuthStatus.Success);
                }

                return await GetAuthentificationAsync(x, account);
            });

        }

        public Task<IdentityResult> AddLoginAsync(int user_id, UserLoginInfo login_info)
        {
            return UsingManagerAsync(async x =>
            {
                var account = await x.FindByUserIdAsync(user_id);
                if (account == null)
                    return UserNotFoundResult;

                return await x.AddLoginAsync(account.ID, login_info);
            });
        }

        public Task<IdentityResult> RemoveLoginAsync(int user_id, UserLoginInfo login_info)
        {
            return UsingManagerAsync(async x =>
            {
                var account = await x.FindByUserIdAsync(user_id);
                if (account == null)
                    return UserNotFoundResult;

                return await x.RemoveLoginAsync(account.ID, login_info);

            });
        }

        public Task<IdentityResult> AddPasswordAsync(int user_id, string password)
        {
            return UsingManagerAsync(async x =>
            {
                var account = await x.FindByUserIdAsync(user_id);
                if (account == null)
                    return UserNotFoundResult;

                return await x.AddPasswordAsync(account.ID, password);
            });
        }

        public Task<IdentityResult> RemovePasswordAsync(int user_id)
        {
            return UsingManagerAsync(async x =>
            {
                var account = await x.FindByUserIdAsync(user_id);
                if (account == null)
                    return UserNotFoundResult;

                return await x.RemovePasswordAsync(account.ID);
            });
        }

        public Task<IdentityResult> ChangePasswordAsync(int user_id, string old_password, string new_password)
        {
            return UsingManagerAsync(async x =>
            {
                var account = await x.FindByUserIdAsync(user_id);
                if (account == null)
                    return UserNotFoundResult;

                return await x.ChangePasswordAsync(account.ID, old_password, new_password);
            });
        }

        public Task<IdentityResult> SetPasswordAsync(int user_id, string new_password)
        {

            return UsingManagerAsync(async x =>
            {
                var account = await x.FindByUserIdAsync(user_id);
                if (account == null)
                    return UserNotFoundResult;

                await x.RemovePasswordAsync(account.ID);
                return await x.AddPasswordAsync(account.ID, new_password);

            });

        }

        public async Task<IdentityResult> SendConfirmEmailCodeAsync(string login, string title, Func<string, string> body_func)
        {
            if (!Settings.ConfirmAllowed)
                return OperationNotAllowedResult;

            if (CheckSendLockouts(login))
                return SendLockedOutResult;

            return await UsingManagerAsync(async x =>
            {
                var account = await x.FindByNameAsync(login);
                if (account == null)
                    return UserNotFoundResult;

                var code = CombineTokenString(account.ID, await x.GenerateEmailConfirmationTokenAsync(account.ID));

                return await SendEmailAsync(x, account, title, body_func(code));
            });

        }

        public async Task<IdentityResult> SendResetPasswordCodeAsync(string login, string title, Func<string, string> body_func)
        {
            if (!Settings.ResetPasswordByTokenAllowed)
                return OperationNotAllowedResult;

            if (CheckSendLockouts(login))
                return SendLockedOutResult;

            return await UsingManagerAsync(async x =>
            {
                var account = await x.FindByNameAsync(login);

                if (account == null)
                    return UserNotFoundResult;


                var code = CombineTokenString(account.ID, await x.GeneratePasswordResetTokenAsync(account.ID));

                return await SendEmailAsync(x, account, title, body_func(code));

            });


        }

        public async Task<AuthResult> ResetPasswordAsync(string code, string new_password)
        {
            if (!Settings.ResetPasswordByTokenAllowed)
                return OperationNotAllowedAuthResult;

            return await UsingManagerAsync(async x =>
            {

                int id;
                string token;
                if (!SplitTokenString(code, out id, out token))
                {
                    return BadTokenResult;
                }


                var account = await x.FindByIdAsync(id);

                if (account == null)
                    return BadTokenResult;


                var result = await x.ResetPasswordAsync(account.ID, token, new_password);


                if (!result.Succeeded)
                    return BadTokenResult;


                await x.SetEmailConfirmedAsync(account.ID);


                return await GetAuthentificationAsync(x, account, new AccountInfo() { UserId = id });
            });
        }

        public async Task<AuthResult> SignOutByLogin(string login)
        {
            return await UsingManagerAsync(async x =>
            {
                var account = await x.FindByNameAsync(login);
                return GetSignOutResultAsync(account, new AccountInfo() { Login = login });
            });
        }

        public async Task<AuthResult> ConfirmEmailAsync(string code)
        {
            if (!Settings.ConfirmAllowed)
                return OperationNotAllowedAuthResult;

            return await UsingManagerAsync(async x =>
            {
                int id;
                string token;

                if (!SplitTokenString(code, out id, out token))
                {
                    return BadTokenResult;
                }


                var account = await x.FindByIdAsync(id);


                if (account == null)
                    return BadTokenResult;

                var result = await x.ConfirmEmailAsync(account.ID, token);

                if (!result.Succeeded)
                    return BadTokenResult;


                return await GetAuthentificationAsync(x, account, new AccountInfo() { UserId = id });
            });
        }

        private bool CheckSendLockouts(string email)
        {
            var lockouts = _cache.GetOrAdd(SendMessageLockoutAccessor, null,
                () => new ConcurrentDictionary<string, LockoutInfo>());

            var info = lockouts.GetOrAdd(email, x => new LockoutInfo());

            return info.AccessFailedCount++ > 3;
        }

        private static string CombineTokenString(int id, string token)
        {
            return id + "_" + token;
        }

        private static bool SplitTokenString(string code, out int id, out string token)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var strings = code.Split(new char[] { '_' }, 2);
                if (strings.Length > 1 && int.TryParse(strings[0], out id))
                {

                    token = strings[1];

                    return true;
                }
            }

            id = 0;
            token = null;
            return false;
        }


        private async Task<IdentityResult> SendEmailAsync(UserManager manager, AccountEntry account, string header, string body)
        {

            try
            {
                await manager.SendEmailAsync(account.ID, header, body);
                return IdentityResult.Success;
            }
            catch (MailServiceException ex)
            {
                return IdentityResult.Failed(ex.Message);

            }
        }

        protected async Task<AuthResult> GetAuthentificationAsync(UserManager manager, AccountEntry account, AccountInfo accountInfo = null)
        {

            if (account == null)
                return AuthResult.NotFound(accountInfo, NotFoundMessage);

            if (!Settings.NotConfirmedLoginAllowed && !manager.IsSystem(account) &&
                !await manager.IsEmailConfirmedAsync(account.ID))
                return new AuthResult(null, account.UserName, AuthStatus.NeedConfirm, NeedConfirmMessage);

            if (manager.IsUser(account))
                return new AuthResult(account.UserId, account.UserName, AuthStatus.Success);

            
            await manager.CreateUserIdAsync(account);

            await manager.UpdateAsync(account);

            return new AuthResult(account.UserId, account.UserName, AuthStatus.Success);

        }

        protected AuthResult GetSignOutResultAsync(AccountEntry account, AccountInfo accountInfo = null)
        {
            if (account == null)
                return AuthResult.NotFound(accountInfo, NotFoundMessage);

            return new AuthResult(account.UserId, account.UserName, AuthStatus.SignOut);
        }

        protected async Task<TResult> UsingManagerAsync<TResult>(Func<UserManager, Task<TResult>> result_func)
        {
            TResult result;
            using (var unitOfWork = _factory.CreateSystemTransaction())
            {
                using (var manager = new UserManager(unitOfWork, _options))
                {
                    result = await result_func(manager);
                    if (result is AuthResult)
                    {
                        TriggerAuthEvent(unitOfWork, result as AuthResult);
                    }
                    if (result is IRegisterResult)
                    {
                        TriggerRegisterEvent(unitOfWork, result as IRegisterResult);
                    }
                }
                unitOfWork.Commit();
            }
            return result;
        }

        public void RegisterAuthEvent(AuthResult authResult)
        {
            using (var unitOfWork = _factory.CreateSystemTransaction())
            {
                TriggerAuthEvent(unitOfWork, authResult);
                unitOfWork.Commit();
            }
        }

        private class LockoutInfo
        {
            public int AccessFailedCount { get; set; }
        }
    }

}