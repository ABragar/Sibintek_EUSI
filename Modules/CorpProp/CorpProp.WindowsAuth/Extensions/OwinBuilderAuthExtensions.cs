using Base.Service;
using Base.Service.Log;
using Base.Identity.Core;
using Microsoft.Owin.Extensions;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using CorpProp.WindowsAuth.Services;
using CorpProp.WindowsAuth.Middlewares;
using Base.Security.Service;
using Base.Ambient;

namespace CorpProp.WindowsAuth.Extentions
{
    public static class OwinBuilderAuthExtensions
    {
        public static void UseWindowsAuth(this IAppBuilder app, IServiceLocator locator)
        {
            app.Use<ADAuthInSystem>(locator.GetService<AccountManager>(),
                locator.GetService<IUnitOfWorkFactory>(), locator.GetService<IADUserService>(), 
                locator.GetService<IAccessUserService>(), locator.GetService<IAppContextBootstrapper>(),
                app, new ADInternalAuthOptions());
            app.UseStageMarker(PipelineStage.PostAuthenticate);
        }
    }
}
