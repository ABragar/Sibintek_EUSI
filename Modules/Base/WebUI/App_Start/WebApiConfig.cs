using System.Web.Http;
using System.Web.Http.Dispatcher;
using Base.WebApi;

namespace WebUI
{
    public static class WebApiConfig
    {
        public static void Register(System.Web.Http.HttpConfiguration config)
        {
            
            
            config.UseCustomControllerTypeResolver(true);

            config.UseCustomAssembliesResolver(typeof(WebApiConfig).Assembly);

            //TODO: проверить
            //config.Routes.MapHttpRoute(
            //name: "apigrid",
            //routeTemplate: "api/{controller}/{action}/{id}",
            //defaults: new { id = RouteParameter.Optional } 
            //);
        }
    }
}
