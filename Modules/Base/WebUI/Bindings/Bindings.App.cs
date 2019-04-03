using Base.App;
using Base.UI.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class AppBindings
    {
        public static void Bind(Container container)
        {
			container.Register<Initializer>();
        }
    }
}