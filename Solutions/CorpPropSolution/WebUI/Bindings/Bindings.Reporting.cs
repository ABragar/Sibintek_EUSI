using SimpleInjector;

namespace WebUI.Bindings
{
    public static class ReportingBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Reporting.Initializer>();
        }
    }
}