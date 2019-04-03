using System.Net;
using System.Web;
using System.Web.Mvc;
using Base.Ambient;

namespace WebUI.Authorize
{
    public class AdminAuthorize: AuthorizeCustomAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return AppContext.SecurityUser?.IsAdmin == true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden,"Только администратор");
        }
    }
}