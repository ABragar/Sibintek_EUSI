using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Identity;
using WebUI.Auth;
using WebUI.Authorize;
using WebUI.Helpers;
using AppContext = Base.Ambient.AppContext;

namespace WebUI.Areas.Account.Controllers
{
    public class AuthorizeController : Controller
    {

        private readonly TokenService _service;

        public AuthorizeController(TokenService service)
        {
            _service = service;
        }


        [AuthorizeCustom]
        public ActionResult Token()
        {
            return new JsonNetResult(new
            {
                Token = _service.GetToken(AppContext.SecurityUser)
            });

        }




        [AuthorizeCustom]
        public ActionResult Index()
        {


            HttpContext.GetAuthentification().SignIn(IdentityHelper.CreateIdentity(AppContext.SecurityUser,"Bearer"));


            return View();
        }





    }
}