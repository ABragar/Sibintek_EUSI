using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Base.Excel;
using Base.WebApi.Services;
using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using WebUI.Concrete;
using WebUI.Controllers;
using WebUI.Helpers;
using WebUI.Service;
using WebUI.SimpleInjector;

namespace WebUI.Bindings
{
    public class WebUIBindings
    {
        public static void Bind(Container container)
        {
            container.Register<WebUIInitilizer>();
            container.Register<IBaseControllerServiceFacade, BaseControllerServiceFacade>();
            container.Register<IWebUIConfigService, WebUIConfigService>();
            container.Register<IEventNotificationService, EventNotificationService>();
            container.Register<IColumnBoundRegisterService, ColumnBoundRegisterService>(Lifestyle.Singleton);
            container.Register<IBoundManagerService, BoundManagerService>(Lifestyle.Singleton);
            container.Register<IAggregateColumnsByConfig, AggregateColumnsByConfig>(Lifestyle.Singleton);

            //Rtc Channels
            container.Register<VideoChannelService>(Lifestyle.Singleton);

            //SignalR
            container.Register<ConferenceHubFactory>();

            container.Register<BroadcasterHandler>(Lifestyle.Singleton);

            RegisterAllTypes<IHub>(container, Assembly.GetExecutingAssembly());

            //Mvc
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());            

            container.RegisterSingleton<IDependencyResolver>(new SimpleInjectorDependencyResolver(container));

            container.RegisterSingleton<IJsonSettingsProvider,JsonSettingsProvider>();


            container.RegisterSingleton<IExcelExportService,ExcelExportService>();
        }



        

        private static void RegisterAllTypes<T>(Container container, params Assembly[] assemblies)
        {
            var type = typeof(T);

            var types =
                assemblies.SelectMany(a => a.GetTypes()
                    .Where(x => type.IsAssignableFrom(x) && !x.IsAbstract && x.IsGenericTypeDefinition));

            foreach (var t in types)
            {
                container.Register(t);
            }
        }
    }
}