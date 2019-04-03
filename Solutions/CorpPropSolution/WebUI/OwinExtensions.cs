using Base.Ambient;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using Microsoft.Owin.Extensions;
using Owin;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace WebUI
{
    public static class OwinExtensions
    {

        public static void UseSecurityUserBootstrapper(this IAppBuilder app, IServiceLocator locator)
        {

            app.Use<OwinSecurityUserMiddleware>(locator.GetService<ISecurityUserService>(), locator.GetService<IAppContextBootstrapper>());
            app.UseStageMarker(PipelineStage.PreHandlerExecute);

        }

        public static void UseExceptionLogger(this IAppBuilder app, IServiceLocator locator)
        {
            
            app.Use<OwinExceptionHandler>(locator.GetService<ILogService>());
        }

    }
}