using Base.Censorship.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class CensorshipBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Censorship.Initializer>();
            container.Register<ICensorshipService, CensorshipService>(Lifestyle.Singleton);
        }
    }
}