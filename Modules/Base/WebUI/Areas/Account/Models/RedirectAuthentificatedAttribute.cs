using System;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Account.Models
{



    public class SkipRedirectAuthentificatedAttribute : Attribute
    {

    }

    public class RedirectAuthentificatedAttribute : FilterAttribute, IAuthorizationFilter
    {

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(SkipRedirectAuthentificatedAttribute), false)
                && filterContext.HttpContext.GetOwinContext().Authentication.User?.Identity.IsAuthenticated == true)
            {
                var values = filterContext.RequestContext.GetRouteValueFromCurrent(new
                {
                    Controller,
                    Action,
                    Area
                });

                filterContext.Result = new RedirectToRouteResult(values);
            }

        }
    }
}