using Base.Help.Services;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class HelpBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Help.Initializer>();
            container.Register<IHelpItemService, HelpItemService>();
            container.Register<IHelpItemTagService, HelpItemTagService>();
        }
    }
}