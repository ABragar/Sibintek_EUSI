using System.Linq;
using System.Web.Mvc;
using WebUI.Auth;

namespace WebUI.Areas.Account.Controllers
{
    public class AuthorizeLocalBearerAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        public AuthorizeLocalBearerAttribute(string scope)
        {
            Scope = scope;
        }


        public string Scope { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {

            var user = filterContext.HttpContext.GetAuthentification().User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var user_scopes =
                    user.FindAll(AuthExtensions.ScopeClaimType)
                        .Select(x => x.Value)
                        .ToArray();

                var current_scope = AuthExtensions.GetScopes(Scope);

                var all = current_scope.All(x => user_scopes.Contains(x));


                if (all)
                    return;
            }


            filterContext.HttpContext.GetAuthentification().Challenge(AuthExtensions.LocalBearer);
            filterContext.Result = new HttpUnauthorizedResult();

        }

    }
}