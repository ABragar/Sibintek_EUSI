using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Identity;
using Base.Identity.Core;
using Base.Security;
using Microsoft.Owin.Security;
using WebUI.Areas.Account.Helpers;
using WebUI.Areas.Account.Models;
using WebUI.Areas.Account.Models.Account;
using WebUI.Areas.Account.Models.Shared;
using WebUI.Auth;
using WebUI.Extensions;

namespace WebUI.Areas.Account.Controllers
{
    [RedirectAuthentificated(Action = "Index", Controller = "Manage")]
    public class AccountController : Controller
    {

        private readonly IAccountManager _account_manager;

        public AccountController(IAccountManager account_manager)
        {
            _account_manager = account_manager;
        }

        #region LOGOUT

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SkipRedirectAuthentificated]
        public ActionResult Logout()
        {
            HttpContext.SignOut();

            return RedirectToLogin();
        }

        #endregion

        [ChildActionOnly]
        public ActionResult Socials(string action, string controller = "Account")
        {
            if (!_account_manager.Settings.ExternalLoginAllowed)
            {
                return null;
            }

            return PartialView(this.GetExternalProviders().ToArray());

        }


        #region LOGIN

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(PasswordLoginModel model)
        {
            var validated = model.ValidateCaptcha == true && this.ValidateCaptcha(model.CaptchaName);

            if (ModelState.IsValid)
            {

                var result = await _account_manager.AuthenticateByPasswordAsync(model.Login, model.Password, !validated);

                switch (result.Status)
                {
                    case AuthStatus.Success:
                        HttpContext.SignIn(result.UserId, result.Login, model.RememberMe ?? false);
                        return RedirectToReturnUrl();

                    case AuthStatus.LockedOut:
                        model.ValidateCaptcha = true;
                        break;
                    case AuthStatus.NeedConfirm:



                        return View("SendConfirmEmail", new SendConfirmEmailModel() { Login = result.Login, Message = "Проверьте почту" });
                    default:
                        break;
                }
                this.AddToModelState(result.Messages);
            }

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult LoginActions()
        {

            var model = new LoginActionsViewModel
            {
                RegistrationAllowed = _account_manager.Settings.RegistrationAllowed,
                ResetPasswordAllowed = _account_manager.Settings.ResetPasswordByTokenAllowed,
            };

            return PartialView(model);
        }

        #region EXTERNAL

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string LoginProvider)
        {
            return new ChallengeResult(LoginProvider, Url.Action("ExternalLoginResult", Url.FromCurrent()));
        }

        public async Task<ActionResult> ExternalLoginResult()
        {
            var info = await HttpContext.GetAuthentification().GetExternalLoginInfoAsync();

            if (info == null)
                return ResultView(ActionResultModel.ExternalInfoNotFoundResult(GetLoginUrl()));



            var result = await _account_manager.AuthenticateByLoginAsync(info.Login);

            switch (result.Status)
            {
                case AuthStatus.Success:
                    HttpContext.SignIn(result.UserId, result.Login);
                    return RedirectToReturnUrl();


                case AuthStatus.NotFound:
                    if (_account_manager.Settings.RegistrationAllowed)
                        return RedirectToAction("ExternalRegister", Url.FromCurrent());

                    break;
                case AuthStatus.NeedConfirm:

                    return View("SendConfirmEmail", new SendConfirmEmailModel() { Login = result.Login, Message = "Проверьте почту" });

                default:
                    break;
            }

            return ErrorResultView(result.Messages);
        }



        #endregion

        #endregion

        #region REGISTER

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // TODO: REDIRECT TO ConfirmEmail IF NEEDED (SET EmailToRegister TO COOKIES/SESSION)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(PasswordRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _account_manager.RegisterByPasswordAsync(new UserInfo
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                }, model.Password);

                switch (result.Status)
                {
                    case AuthStatus.Success:
                        HttpContext.SignIn(result.UserId, result.Login);
                        return RedirectToManage();

                    case AuthStatus.NeedConfirm:

                        var confirm_res = await
                            _account_manager.SendConfirmEmailCodeAsync(result.Login, "Подтверждение почты",
                                x => this.RenderPartialViewToString("SendConfirmEmailBody", x));

                        var message = confirm_res.Succeeded ? "Проверьте почту" : string.Join(". ", confirm_res.Errors);


                        return View("SendConfirmEmail", new SendConfirmEmailModel() { Login = result.Login, Message = message });

                    default:
                        break;
                }
                this.AddToModelState(result.Messages);
            }

            return View(model);
        }

        #region EXTERNAL

        [HttpGet]
        public async Task<ActionResult> ExternalRegister()
        {
            var info = await HttpContext.GetAuthentification().GetExternalLoginInfoAsync();

            if (info == null)
                return ResultView(ActionResultModel.ExternalInfoNotFoundResult(GetLoginUrl()));

            return View(new ExternalRegisterModel
            {
                LoginProvider = info.Login.LoginProvider,
                ProviderKey = info.Login.ProviderKey,
                Email = info.Email,
                FirstName = info.DefaultUserName,
                LastName = info.ExternalIdentity.FindFirst(ClaimTypes.Surname)?.Value
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalRegister(ExternalRegisterModel model)
        {
            if (ModelState.IsValid)
            {

                var info = await HttpContext.GetAuthentification().GetExternalLoginInfoAsync();

                if (info == null || info.Login.LoginProvider != model.LoginProvider || info.Login.ProviderKey != model.ProviderKey)
                    return ResultView(ActionResultModel.ExternalInfoNotFoundResult(GetLoginUrl()));


                var result = await _account_manager.RegisterByLoginAsync(new UserInfo
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                }, info.Login);

                switch (result.Status)
                {
                    case AuthStatus.Success:
                        HttpContext.SignIn(result.UserId, result.Login);
                        return RedirectToManage();

                    case AuthStatus.NeedConfirm:

                        var confirm_res = await
                            _account_manager.SendConfirmEmailCodeAsync(result.Login, "Подтверждение почты",
                                x => this.RenderPartialViewToString("SendConfirmEmailBody", x));

                        var message = confirm_res.Succeeded ? "Проверьте почту" : string.Join(". ", confirm_res.Errors);

                        return View("SendConfirmEmail", new SendConfirmEmailModel() { Login = result.Login, Message = message });
                    default:

                        break;
                }
                this.AddToModelState(result.Messages);
            }
            return View(model);
        }


        #endregion

        #endregion

        #region RESET PASSWORD

        [HttpGet]
        [SkipRedirectAuthentificated]
        public ActionResult SendResetPassword()
        {
            return View(new SendResetPasswordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendResetPassword(SendResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _account_manager.SendResetPasswordCodeAsync(model.Login, "Сброс пароля",
                    x => this.RenderPartialViewToString("SendResetPasswordBody", x));

                if (result.Succeeded)
                {
                    return ResultView(ActionResultModel.MessageSendResult(GetLoginUrl()));
                }

                this.AddToModelState(result.Errors);

            }
            return View(model);
        }

        [HttpGet]
        [SkipRedirectAuthentificated]
        public ActionResult ResetPassword(string code)
        {
            return View(new ResetPasswordModel() { Code = code });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SkipRedirectAuthentificated]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _account_manager.ResetPasswordAsync(model.Code, model.NewPassword);

                if (result.Status == AuthStatus.Success)
                {
                    HttpContext.SignIn(result.UserId, result.Login);
                    return RedirectToManage();
                }

                this.AddToModelState(result.Messages);
            }

            return View(model);
        }

        #endregion

        #region CONFIRM EMAIL


        [HttpPost]
        [ValidateAntiForgeryToken]
        [SkipRedirectAuthentificated]
        public async Task<ActionResult> SendConfirmEmail(SendConfirmEmailModel model)
        {

            this.ValidateCaptcha(model.CaptchaName);

            if (ModelState.IsValid)
            {



                var result = await
                    _account_manager.SendConfirmEmailCodeAsync(model.Login, "Подтверждение почты",
                        x => this.RenderPartialViewToString("SendConfirmEmailBody", x));

                if (result.Succeeded)
                {
                    return ResultView(ActionResultModel.MessageSendResult(GetLoginUrl()));
                }

                this.AddToModelState(result.Errors);
            }
            return View(model);
        }




        [HttpGet]
        [SkipRedirectAuthentificated]
        public async Task<ActionResult> ConfirmEmail(string code)
        {

            var result = await _account_manager.ConfirmEmailAsync(code);

            switch (result.Status)
            {
                case AuthStatus.Success:
                    HttpContext.SignIn(result.UserId, result.Login);
                    return RedirectToManage();

                default:
                    return ErrorResultView(result.Messages);
            }

        }


        #endregion

        #region HELPERS

        private ActionResult RedirectToReturnUrl()
        {
            return Redirect(Url.GetReturnUrl());
        }

        private ActionResult ResultView(ActionResultModel model)
        {
            return View("Result", model);
        }

        private ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", Url.FromCurrent());
        }

        private ActionResult RedirectToManage()
        {
            return RedirectToAction("Index", "Manage", Url.FromCurrent());
        }

        private string GetLoginUrl()
        {
            return Url.Action("Login", Url.FromCurrent());
        }

        private ActionResult ErrorResultView(IReadOnlyCollection<string> errors)
        {
            return ResultView(new ActionResultModel()
            {
                Messages = errors,
                GoBackUrl = GetLoginUrl(),
                Success = false
            });
        }

        #endregion
    }
}