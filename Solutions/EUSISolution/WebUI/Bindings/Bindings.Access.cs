using Base.Access.Service;
using Base.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class AccessBindings
    {
        public static void Bind(Container container)
        {
            container.Register<IAccessService, AccessService>(Lifestyle.Singleton);
            container.Register<IAccessErrorDescriber, AccessErrorDescriber>();
        }
    }
}