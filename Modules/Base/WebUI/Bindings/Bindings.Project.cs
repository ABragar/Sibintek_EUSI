using SimpleInjector;

namespace WebUI.Bindings
{
    public class ProjectBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Project.Initializer>();           
        }
    }
}