using System;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace WebUI.SimpleInjector
{
    public static class ContainerExtensions
    {

        private static readonly object _key = new object();

        public static HttpContextScopeManager GetCurrentHttpContextScopeManager(this Container container)
        {

            var manager = (HttpContextScopeManager)container.GetItem(_key);

            if (manager == null)
            {
                lock (_key)
                {
                    manager = (HttpContextScopeManager)container.GetItem(_key);
                    if (manager == null)
                    {
                        manager = new HttpContextScopeManager();
                        container.SetItem(_key, manager);
                    }
                }
            }
            return manager;
        }

        public static Func<IDisposable> GetBeginHttpContextScopeProvider(this Container container)
        {
            var manager = container.GetCurrentHttpContextScopeManager();

            return () => manager.BeginScope();
        }

    }
}