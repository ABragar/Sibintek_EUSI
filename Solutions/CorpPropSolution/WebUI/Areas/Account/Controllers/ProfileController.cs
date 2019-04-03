using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Security.Service;
using Microsoft.AspNet.Identity;
using WebUI.Auth;

namespace WebUI.Areas.Account.Controllers
{
    public class ProfileController : Controller
    {

        private readonly ISecurityUserService _user_service;

        public ProfileController(ISecurityUserService user_service)
        {
            _user_service = user_service;
        }


        public async Task<ActionResult> UserInfo()
        {
            var x = await HttpContext.GetAuthentification().AuthenticateAsync(AuthExtensions.LocalBearer);
            if (x == null)
            {
                HttpContext.GetAuthentification().Challenge(AuthExtensions.LocalBearer);
                return new HttpUnauthorizedResult();
            }

            var user = _user_service.FindSecurityUser(x.Identity.GetUserId<int>(), x.Identity.Name);

            if (user == null)
            {
                HttpContext.GetAuthentification().Challenge(AuthExtensions.LocalBearer);
                return new HttpUnauthorizedResult();
            }


            var client_id = x.Properties.Dictionary["client_id"];

            var scopes = x.Properties.Dictionary["scope"].Split(' ');

            var data = new Dictionary<string, object>();

            data.Add("name", user.ProfileInfo.FullName);

            //TODO add crypt
            data.Add("user_key", client_id + user.ID);

            foreach (var scope in scopes)
            {
                switch (scope)
                {
                    case "user_id":
                        data.Add("user_id", user.ID);
                        break;
                    case "email":
                        data.Add("email", user.ProfileInfo.Email);
                        break;
                    default:
                        break;
                }

            }


            return new JsonResult()
            {
                Data = data
            };


        }
    }
}