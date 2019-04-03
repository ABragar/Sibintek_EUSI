using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebUI.Auth;
using AppContext = Base.Ambient.AppContext;

namespace WebUI.Authorize
{
    public class AuthorizeCustomAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return AppContext.SecurityUser != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.SignOut();



            //base.HandleUnauthorizedRequest(filterContext);
            if (filterContext == null)
            {
                throw new System.ArgumentNullException(nameof(filterContext));
            }

            if (filterContext.HttpContext.User.Identity.IsAuthenticated) return;
            const string loginUrl = "/Account/Login"; // Default Login Url 
            filterContext.Result = new RedirectResult(loginUrl);

        }
    }
}