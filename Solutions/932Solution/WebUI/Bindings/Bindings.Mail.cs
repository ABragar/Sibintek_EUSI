using Base.App;
using Base.Mail.DAL;
using Base.Mail.Entities;
using Base.Mail.Service;
using Base.Service;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class MailBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Mail.Initializer>();
            container.Register<IMailFolderService, MailFolderService>();
            container.Register<IMailMessageService, MailMessageService>();

            container.Register<IMailClient, MailClient>();
            container.Register<IEmailService, EmailService>();
            container.Register<IMailQueueService, MailQueueService>();
            container.Register<IProfileMailSettingsService, ProfileMailSettingsService>();
            container.Register<IPresetService<ProfileMailSettings>, PresetService<ProfileMailSettings>>(Lifestyle.Singleton);
            container.Register<IPresetFactory<ProfileMailSettings>, DefaultPresetFactory<ProfileMailSettings>>(Lifestyle.Singleton);
        }
    }
}