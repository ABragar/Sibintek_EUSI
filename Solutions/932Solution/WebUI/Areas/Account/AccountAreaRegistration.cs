using System.Web.Mvc;

namespace WebUI.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName { get; } = "Account";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Account_manage",
                "Account/Manage/{action}",
                new { controller = "Manage", action = "Index" },
                namespaces: new[] { "WebUI.Areas.Account.Controllers" });

            context.MapRoute(
                "Account_IndexAdmin",
                "Account/Admin/{id}",
                new { controller = "Admin", action = "Index" },
                namespaces: new[] { "WebUI.Areas.Account.Controllers" });

            context.MapRoute(
                "Account_Admin",
                "Account/Admin/{action}/{id}",
                new { controller = "Admin", action = "Index" },
                namespaces: new[] { "WebUI.Areas.Account.Controllers" });

            context.MapRoute(
                "Account_Authorize",
                "Account/Authorize/{action}",
                new { controller = "Authorize", action = "Index" },
                namespaces: new[] { "WebUI.Areas.Account.Controllers" });

            context.MapRoute(
                "Account_Profile",
                "Account/Profile/{action}",
                    new { controller = "Profile", action = "Index" },
                    namespaces: new[] { "WebUI.Areas.Account.Controllers" });


            context.MapRoute(
                "Account",
                "Account/{action}/",
                new { controller = "Account" },
                namespaces: new[] { "WebUI.Areas.Account.Controllers" });
        }
    }
}