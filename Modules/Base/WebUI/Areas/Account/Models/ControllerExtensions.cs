using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Base.Extensions;
using Microsoft.Owin.Security;
using WebUI.Auth;

namespace WebUI.Areas.Account.Models
{
    public static class ControllerExtensions
    {
        public static void AddToModelState(this Controller controller, IEnumerable<string> errors)
        {
            errors.ForEach(x => controller.ModelState.AddModelError("", x));
        }

        public static IEnumerable<ProviderInfo> GetExternalProviders(this Controller controller)
        {
            return controller.HttpContext.GetAuthentification()
                .GetExternalAuthenticationTypes()
                .Select(x => new ProviderInfo { LoginProvider = x.AuthenticationType, Caption = x.Caption });
        }


    }
}