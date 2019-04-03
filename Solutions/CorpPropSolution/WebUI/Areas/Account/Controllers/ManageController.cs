using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Extensions;
using Base.Identity.Core;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebUI.Areas.Account.Models;
using WebUI.Areas.Account.Models.Account;
using WebUI.Areas.Account.Models.Manage;
using WebUI.Areas.Account.Models.Shared;
using WebUI.Auth;
using WebUI.Authorize;
using WebUI.Extensions;

namespace WebUI.Areas.Account.Controllers
{
    public class ManageControllerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            var user_id = filterContext.HttpContext.GetUserId()?.ToString();


            var form_id = request.Form["UserId"];

            if (form_id != null && form_id != user_id)
            {
                filterContext.Result = new RedirectToRouteResult(filterContext.RequestContext.GetRouteValueFromCurrent(new
                {
                    Action = "Error"
                }));
            }
        }
    }

    [ManageControllerFilter]
    [AuthorizeCustom]
    public class ManageController : Controller
    {
        private const string XsrfKey = "XsrfKey";

        private readonly IAccountManager _account_manager;

        public ManageController(IAccountManager account_manager)
        {
            _account_manager = account_manager;
        }

        public async Task<ActionResult> Index()
        {
            var user_id = HttpContext.GetUserId().Value;

            var account_info = await _account_manager.GetAccountInfoAsync(user_id);


            var external_allowed = _account_manager.Settings.ExternalLoginAllowed;

            var user_providers = new List<UserProviderInfo>();

            var can_attach_providers = new List<ProviderInfo>();

            this.GetExternalProviders().ForEach(x =>
            {
                var info = account_info.ExternalLogins.SingleOrDefault(e => e.LoginProvider == x.LoginProvider);

                if (info != null)
                {
                    user_providers.Add(new UserProviderInfo { LoginProvider = info.LoginProvider, ProviderKey = info.ProviderKey, Caption = x.Caption });
                }
                else if (external_allowed)
                {
                    can_attach_providers.Add(x);
                }
            });

            var can_dettach_providers_count = account_info.HasPassword ? 1 : 2;

            var model = new ManageViewModel
            {
                UserId = user_id,
                Login = account_info.Login,
                HasPassword = account_info.HasPassword,
                UserProviders = user_providers,
                CanDetachPassword = account_info.HasPassword && external_allowed && user_providers.Count > 0,
                CanConfirmEmail = account_info.HasEmail && !account_info.EmailConfirmed && _account_manager.Settings.ConfirmAllowed,
                CanDetachProvider = user_providers.Count >= can_dettach_providers_count,
                CanAttachProviders = can_attach_providers,
            };

            return View(model);
        }

        #region CHANGE PASSWORD

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel { UserId = HttpContext.GetUserId().Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _account_manager.ChangePasswordAsync(HttpContext.GetUserId().Value, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToIndex();
                }

                this.AddToModelState(result.Errors);
            }
            return View(model);
        }

        #endregion

        #region ATTACH PASSWORD

        [HttpGet]
        public ActionResult AttachPassword()
        {
            return View(new AttachPasswordModel { UserId = HttpContext.GetUserId().Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AttachPassword(AttachPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _account_manager.AddPasswordAsync(HttpContext.GetUserId().Value, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToIndex();
                }

                this.AddToModelState(result.Errors);
            }

            return View(model);
        }

        #endregion

        #region DETACH PASSWORD

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DetachPassword(UserActionModel model)
        {
            if (!ModelState.IsValid)
                return ResultView(ActionResultModel.Error(GetIndexUrl()));

            var result = await _account_manager.RemovePasswordAsync(HttpContext.GetUserId().Value);

            if (result.Succeeded)
            {
                return RedirectToIndex();
            }

            return ResultView(result.ToActionResult(GetIndexUrl()));
        }

        #endregion

        #region EXTERNAL

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AttachExternalLogin(ExternalLoginActionModel model)
        {
            if (!ModelState.IsValid)
                return ResultView(ActionResultModel.Error(GetIndexUrl()));

            return new ChallengeResult(model.LoginProvider, Url.Action("AttachExternalLoginResult", Url.FromCurrent()), XsrfKey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DetachExternalLogin(DetachExternalLoginModel model)
        {
            if (!ModelState.IsValid)
                return Error();

            //TODO CHECK

            var result = await _account_manager.RemoveLoginAsync(HttpContext.GetUserId().Value, new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            if (result.Succeeded)
                return RedirectToIndex();

            return ResultView(result.ToActionResult(GetIndexUrl()));
        }

        #endregion

        public ActionResult Error()
        {
            return ResultView(ActionResultModel.Error(GetIndexUrl()));
        }

        public async Task<ActionResult> AttachExternalLoginResult()
        {
            var user_id = HttpContext.GetUserId().Value;

            var info = await HttpContext.GetAuthentification()
                .GetExternalLoginInfoAsync(XsrfKey, user_id.ToString());

            if (info == null)
                return ResultView(ActionResultModel.ExternalInfoNotFoundResult(GetIndexUrl()));

            var result = await _account_manager.AddLoginAsync(user_id, info.Login);

            if (result.Succeeded)
            {
                return RedirectToIndex();
            }

            return ResultView(result.ToActionResult(GetIndexUrl()));
        }


        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index", Url.FromCurrent());
        }


        private ActionResult ResultView(ActionResultModel model)
        {
            return View("Result", model);
        }

        private string GetIndexUrl()
        {
            return Url.Action("Index", Url.FromCurrent());
        }
    }
}