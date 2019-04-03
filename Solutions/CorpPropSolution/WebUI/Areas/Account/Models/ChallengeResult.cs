using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using WebUI.Auth;

namespace WebUI.Areas.Account.Models
{

    public class ChallengeResult : HttpUnauthorizedResult
    {

        public ChallengeResult(string provider, string redirectUri, string xsrf_key = null)
        {
            XsrfKey = xsrf_key;
            LoginProvider = provider;
            RedirectUri = redirectUri;

        }

        public string XsrfKey { get; }
        public string LoginProvider { get; }
        public string RedirectUri { get; }


        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

            if (XsrfKey != null)
            {
                var user_id = context.HttpContext.GetUserId();

                if (user_id != null)
                {
                    properties.Dictionary[XsrfKey] = user_id.ToString();
                }
            }

            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }

    }
}