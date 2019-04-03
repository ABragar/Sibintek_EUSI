using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Identity.Core;
using Microsoft.AspNet.Identity;
using WebUI.Areas.Account.Models;
using WebUI.Areas.Account.Models.Admin;
using WebUI.Areas.Account.Models.Shared;
using WebUI.Auth;
using WebUI.Authorize;

namespace WebUI.Areas.Account.Controllers
{
    public class AdminControllerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            var user_id = filterContext.RouteData.Values["id"] as string;

            if (user_id == filterContext.HttpContext.GetUserId()?.ToString())
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                        filterContext.RequestContext.GetRouteValueFromCurrent(
                            new { Action = "Index", Controller = "Manage", Id = (string)null, }));
                return;
            }

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

    [AdminControllerFilter]
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly IAccountManager _account_manager;

        public AdminController(IAccountManager account_manager)
        {
            _account_manager = account_manager;
        }

        public ActionResult Error(int id)
        {
            return ResultView(ActionResultModel.Error(GetIndexUrl()));
        }

        public async Task<ActionResult> Index(int id)
        {
            var account_info = await _account_manager.GetAccountInfoAsync(id);


            if (account_info == null)
                return HttpNotFound();

            var model = new AdminViewModel
            {
                UserId = id,
                Login = account_info.Login,
                HasPassword = account_info.HasPassword,
                UserProviders = this.GetExternalProviders()
                    .Join(account_info.ExternalLogins,
                            x => x.LoginProvider, x => x.LoginProvider,
                            (x, y) => new UserProviderInfo { LoginProvider = y.LoginProvider, ProviderKey = y.ProviderKey, Caption = x.Caption })
                    .ToArray(),
                CanDatachProviders = true,
                CanDetachPassword = account_info.HasPassword,
                CanAttachPassword = !account_info.HasPassword,

            };

            return View(model);
        }

        #region SET PASSWORD

        [HttpGet]
        public ActionResult SetPassword(int id)
        {
            return View(new SetPasswordModel() { UserId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(int id, SetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _account_manager.SetPasswordAsync(model.UserId, model.NewPassword);

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
        public ActionResult AttachPassword(int id)
        {
            return View(new AttachPasswordModel { UserId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AttachPassword(int id, AttachPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _account_manager.AddPasswordAsync(model.UserId, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToIndex();
                }

                this.AddToModelState(result.Errors);
            }

            return View(model);
        }

        #endregion

        #region DETACH PASSWORD/EXTERNAL LOGIN

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DetachPassword(int id, UserActionModel model)
        {
            if (!ModelState.IsValid)
                return Error(id);

            //TODO CHECK

            var result = await _account_manager.RemovePasswordAsync(model.UserId);

            if (result.Succeeded)
            {
                return RedirectToIndex();
            }

            return ResultView(result.ToActionResult(GetIndexUrl()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DetachExternalLogin(int id, DetachExternalLoginModel model)
        {
            if (!ModelState.IsValid)
                return Error(id);

            //TODO CHECK

            var result = await _account_manager.RemoveLoginAsync(model.UserId, new UserLoginInfo(model.LoginProvider, model.ProviderKey));

            if (result.Succeeded)
                return RedirectToIndex();

            return ResultView(result.ToActionResult(GetIndexUrl()));
        }

        #endregion

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