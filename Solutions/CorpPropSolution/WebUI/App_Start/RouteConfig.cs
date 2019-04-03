using System;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using WebUI.Concrete;
using WebUI.Controllers;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DashboardWidgets",
                url: "Dashboard/GetWidgets",
                defaults: new {controller = "Dashboard", action = "GetWidgets", area = ""},
                namespaces: new[] {"WebUI.Controllers"}
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{module}",
                defaults: new { controller = "Dashboard", action = "Index", area = "", module = UrlParameter.Optional },
                namespaces: new[] { "WebUI.Controllers" }
            );

            routes.MapRoute(
                name: "DashboardAPI",
                url: "DashboardAPI/{action}",
                defaults: new { controller = "Dashboard", area = "" }
            );

            routes.MapRoute(
                 name: "Preset",
                 url: "Preset/{action}/{GenericControllerVariable}/{id}",
                 defaults: new { controller = "Preset", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "WebUI.Controllers" }
             );

            routes.MapRoute(
                name: "View",
                url: "Entities/{mnemonic}",
                defaults: new { controller = "View", action = "Index", mnemonic = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ViewModel",
                url: "EntityType/{mnemonic}-{typeDialog}-{id}",
                defaults: new { controller = "View", action = "GetViewModel", mnemonic = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Files",
                url: "files/{folder}/{fileName}.xml",
                defaults: new { controller = "Files", action = "GetXml" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WebUI.Controllers" }
            );
        }
    }
}