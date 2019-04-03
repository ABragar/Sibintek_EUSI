using Base.Hangfire;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class HangFireBindings
    {
        public static void Bind(Container container)
        {
            container.Register<IHangFireService, HangFireService>(Lifestyle.Singleton);
        }
    }
}