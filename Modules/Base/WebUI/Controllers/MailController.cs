
using Base.Settings;
using System;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using AppContext = Base.Ambient.AppContext;

namespace WebUI.Controllers
{
    public class MailController : BaseController
    {


        public MailController(IBaseControllerServiceFacade serviceFacade)
            : base(serviceFacade)
        {

        }

        public ActionResult Index()
        {
            return View(new BaseViewModel(this));
        }

        public ActionResult GetSettings()
        {

            return new JsonNetResult(new
            {

            });
        }

        private string ReplacePlaceholders(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var currentUser = AppContext.SecurityUser;

            value = value
                .Replace("%FromName", currentUser.ProfileInfo.FullName)
                .Replace("%FromAddress", GetUserEmailFromLogin(currentUser.Login));

            return value;
        }

        private string GetUserEmailFromLogin(string login)
        {
            if (login.IndexOf("@", StringComparison.Ordinal) != -1)
            {
                return login;
            }

            var domain = "";
            return $"{login}@{domain}";
        }
    }
}