using System;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace WebUI.SimpleInjector
{

    
    public class HttpContextHybridScopeLifestyle : ExecutionContextScopeLifestyle
    {

        protected override Func<Scope> CreateCurrentScopeProvider(Container container)
        {
            var base_provider = base.CreateCurrentScopeProvider(container);
            var manager = container.GetCurrentHttpContextScopeManager();

            return () => base_provider() ?? manager.GetCurrent();
        }

        protected override Scope GetCurrentScopeCore(Container container)
        {
            return base.GetCurrentScopeCore(container) ?? container.GetCurrentHttpContextScopeManager().GetCurrent();
        }
    }

    
}