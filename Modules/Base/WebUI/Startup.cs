using System;
using Base.BusinessProcesses.Services.Abstract;
using Base.Event.Entities;
using Base.Event.Service;
using Base.Hangfire;
using Base.Identity.OAuth2;
using Hangfire;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using WebUI.Auth;
using WebUI.Helpers;
using Base.Service;
using Base.WebApi.Services;
using Microsoft.AspNet.SignalR.Hubs;
using WebApi;
using WebUI.SimpleInjector;
using CorpProp.Services.ProjectActivity;
using CorpProp.Services.Settings;

[assembly: OwinStartup(typeof(WebUI.Startup))]

namespace WebUI
{

    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {

            var container = Bindings.Bindings.GetContainer();
            var locator = container.GetInstance<IServiceLocator>();

            BoundsRegister.BoundsRegister.Init();

            app.Use<HttpContextScopeMiddleware>(container);

            app.UseExceptionLogger(locator);


            app.UseAuth(locator.GetService<OAuthService>(),locator.GetService<TicketFormatService>(), AuthSettings.Config);

            app.UseSecurityUserBootstrapper(locator);



            app.UseStandartApi(locator);

            var hangfireService = locator.GetService<IHangFireService>();


            hangfireService.Run("DataContext");

            hangfireService.MapDashboard(app);

#if (Release)
            hangfireService.Register<IWorkflowService>(x => x.AutoInvokeStage(), "0 2 * * *");
            hangfireService.Register<IMailQueueService>(x => x.SendQueue(), Cron.Hourly());
            hangfireService.Register<IProductionCalendarService>(x => x.CreateCalendar(DateTime.Now.Year), Cron.Yearly());
            hangfireService.Register<IFileManager>(x => x.DeleteFileData(), "0 1 * * *");
            hangfireService.Register<IFileManager>(x => x.DeleteFiles(), "0 1 * * *");
            hangfireService.Register<IEventService<Event>>(x => x.GetEventsToRemind(), Cron.Hourly());
            //hangfireService.Register<ISibNotificationService>(x => x.GetNotificationToRemind(), Cron.Hourly());
#endif
            hangfireService.Register<ISibNotificationService>(x => x.GetNotificationToRemind(), Cron.Daily());


            var resolver = GlobalHost.DependencyResolver;

#region SignalR Serializers


            var settings = new JsonSerializerSettings();

            locator.GetService<IJsonSettingsProvider>().ApplySettings(settings);

            settings.Converters.Add(new CustomDateTimeConvertor());
            

            resolver.Register(typeof(JsonSerializer), () => JsonSerializer.Create(settings));


#endregion

            resolver.Register(typeof(IHubActivator), () => new SignalRHubActivator(container));
            resolver.Resolve<IHubPipeline>().AddModule(locator.GetService<HubScopeModule>());



            app.MapSignalR("/signalr", new HubConfiguration()
            {
                Resolver = resolver,
                EnableJSONP = true,
                EnableDetailedErrors = true
            });

        }


    }
}