using Base.Audit.Services;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class AuditBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Audit.Initializer>();
            container.Register<IAuditItemService, AuditItemService>();
            container.Register<IAuditItemAuthService, AuditItemService>();
        }
    }
}