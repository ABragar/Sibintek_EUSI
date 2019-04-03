using System.Web.Mvc;
using WebUI.Areas.Public.Models;

namespace WebUI.Areas.Public
{
    public class PublicAreaRegistration : AreaRegistration
    {
        public override string AreaName { get; } = "Public";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Public_standart_map",
                "Public/Map",
                new { controller = "Map", action = "Index", area = "Public", id = MapViewType.Standart },
                namespaces: new[] { "WebUI.Areas.Public.Controllers" }
            );

            context.MapRoute(
                "Public_simple_map",
                "Public/SimpleMap",
                new { controller = "Map", action = "Index", area = "Public", id = MapViewType.Simple },
                namespaces: new[] { "WebUI.Areas.Public.Controllers" }
            );

            context.MapRoute(
                "Public_default",
                "Public/{controller}/{action}/{id}",
                new { action = "Index", area = "Public", id = UrlParameter.Optional },
                namespaces: new[] { "WebUI.Areas.Public.Controllers" }
            );
        }
    }
}