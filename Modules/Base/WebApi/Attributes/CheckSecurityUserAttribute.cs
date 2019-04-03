using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Attributes
{
    public class CheckSecurityUserAttribute: AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return Base.Ambient.AppContext.SecurityUser != null;
        }
    }
}