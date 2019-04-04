using System;
using System.Data.Entity;
using Base.Entities;
using Base.UI.Service;
using System.Data.Entity.Spatial;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Base;
using Base.Service;
using Base.WebApi.Services;
using Common.Data.EF;
using Data.EF;
using SimpleInjector;
using WebUI.Binders;
using WebUI.Concrete;
using Initializer = WebUI.Concrete.Initializer;


namespace WebUI
{
    public class MvcApplication : HttpApplication
    {
        readonly Container _container = Bindings.Bindings.GetContainer();

        protected void Application_Start()
        {
            #region Temporary global hack for untrusted ssl certificate
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            ServicePointManager.Expect100Continue = false;
            #endregion

            Database.SetInitializer(new MigrateDatabaseToLatestVersionInitializer());

            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            if (System.Configuration.ConfigurationManager.AppSettings["SuppressIdentityHeuristicChecks"] != null)
            {
                bool suppressIdentity = false;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["SuppressIdentityHeuristicChecks"], out suppressIdentity);
                AntiForgeryConfig.SuppressIdentityHeuristicChecks = suppressIdentity;
            }

            var locator = _container.GetInstance<IServiceLocator>();

            DependencyResolver.SetResolver(locator.GetService<IDependencyResolver>());

            var scopeManager = locator.GetService<IExecutionContextScopeManager>();
            
            using (scopeManager.BeginScope())
            {
                Initializer.Run(locator, Bindings.Bindings.GetModuleInitializers(_container));
                _container.GetInstance<IUiEnumService>().Sync();

            }
            //EUSI.Export.AccObjectExport.GetObjects(locator);

            _container.GetInstance<IJsonSettingsProvider>().ApplySettings(GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
            
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            // TODO: проверить GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(_container);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Areas.Public.BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(typeof(CustomControllerFactory));
            BinderConfig.RegisterBinders(ModelBinders.Binders);
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif            
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication context = (HttpApplication)sender;
            context.Response.SuppressFormsAuthenticationRedirect = true;
        }
    }


    public static class BinderConfig
    {
        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            //User controller changemyprofile
            //wizard getsymmary

            binders.Add(typeof(BaseObject), new BaseObjectModelBinder());
            binders.Add(typeof(HCategory), new BaseObjectModelBinder());
            binders.Add(typeof(Base.UI.ViewModal.ListView), new ListViewBinder());
            binders.Add(typeof(Base.UI.ViewModal.DetailView), new DetailViewBinder());
            binders.Add(typeof(Base.UI.Preset), new PresetBinder());
            var provider = new InheritanceModelBinderProvider(binders);

            provider.Add(x => typeof(IRuntimeBindingType).IsAssignableFrom(x), new RuntimeTypeModelBinder());

            ModelBinderProviders.BinderProviders.Add(provider);
        }

      

    }
}