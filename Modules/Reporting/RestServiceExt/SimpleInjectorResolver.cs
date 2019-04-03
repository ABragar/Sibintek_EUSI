using System.Web.Http;
using ReportStorage.Service;
using RestService.Identity;
using RestService.Service;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace RestService
{
    public static class SimpleInjectorResolver
    {
        private static readonly Container container = new Container();
        public static void Init(HttpConfiguration config)
        { 
            container.Options.DefaultScopedLifestyle = new SimpleInjector.Lifestyles.AsyncScopedLifestyle();

            container.Register<IReportStorageService, ReportStorageServiceWithHistory>();
            container.Register<IFileService, FileService>();

            container.Register<IAuthStrategyFactory, AuthStrategyFactory>();

            container.Register<IPathHelper, PathHelper>(Lifestyle.Singleton);
            container.Register<IResourceService, ResourceService>(Lifestyle.Singleton);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        public static Container Container
        {
            get { return container; }
        }
    }
}