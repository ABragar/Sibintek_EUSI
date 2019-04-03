using Base.MailAdmin.Services;
using Base.MailAdmin.Entities;
using Base.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class MailAdminBindings
    {

        public static void Bind(Container container)
        {
            container.Register<Base.MailAdmin.Initializer>();
            container.Register<IMailAccountService, MailAccountService>(Lifestyle.Singleton);
            container.Register<IMailAdminClient, MailAdminClient>(Lifestyle.Singleton);
        }

    }
}