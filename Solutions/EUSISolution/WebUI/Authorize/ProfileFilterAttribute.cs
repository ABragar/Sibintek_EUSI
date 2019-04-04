using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Base.Ambient;
using Base.Entities.Complex;
using WebUI.Controllers;


namespace WebUI.Authorize
{
    public class ProfileFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AppContext.SecurityUser != null && ( AppContext.SecurityUser.ID == 0 || AppContext.SecurityUser.ProfileInfo.IsEmpty))
            {
                filterContext.Result = new RedirectResult("~/Users/ChooseProfile");
            }
        }

    }

}