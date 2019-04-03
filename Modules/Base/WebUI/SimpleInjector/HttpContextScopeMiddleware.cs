using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using SimpleInjector;

namespace WebUI.SimpleInjector
{
    public class HttpContextScopeMiddleware : OwinMiddleware
    {
        private readonly Func<IDisposable> _provider;

        public HttpContextScopeMiddleware(OwinMiddleware next, Container container) : base(next)
        {
            _provider = container.GetBeginHttpContextScopeProvider();
        }

        public override async Task Invoke(IOwinContext context)
        {
            using (_provider())
            {
                await Next.Invoke(context);
            }
        }
    }


}