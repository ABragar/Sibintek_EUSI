using System;
using System.Threading.Tasks;
using Base.Ambient;
using Base.Security;
using Base.Security.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;

namespace WebUI
{
    public class OwinSecurityUserMiddleware : OwinMiddleware
    {

        private readonly ISecurityUserService _security_user_service;
        private readonly IAppContextBootstrapper _app_context_bootstrapper;

        public OwinSecurityUserMiddleware(OwinMiddleware next,

            ISecurityUserService security_user_service,
            IAppContextBootstrapper app_context_bootstrapper) : base(next)
        {
            _app_context_bootstrapper = app_context_bootstrapper;
            _security_user_service = security_user_service;
        }

        public override async Task Invoke(IOwinContext context)
        {

            IDisposable user_context = null;

            try
            {
                var identity = context.Authentication.User?.Identity;
                
                if (identity!= null && identity.IsAuthenticated)
                {

                    
                    var user_id = identity.GetUserId<int>();
                    var login = identity.Name;

                    user_context = _app_context_bootstrapper.LocalContextSecurity(() => _security_user_service.FindSecurityUser(user_id, login));
                }

                await Next.Invoke(context);
            }
            finally
            {
                user_context?.Dispose();
            }
        }

    }
}