using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Mail.Entities;
using Base.Mail.Service;
using Base.Security;
using Base.Settings;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Mail
{

    public class Initializer : IModuleInitializer
    {
        private readonly ISettingService<MailSetting> _settingService;

        public Initializer(ISettingService<MailSetting> settingService)
        {
            _settingService = settingService;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<MailQueueItem>();

            context.CreateVmConfig<MailFolder>()
                .Title("Почта - Папки")
                .IsReadOnly(true)
                .ListView(x => x.Title("Папка"))
                .DetailView(x => x.Title("Папка"));

            context.CreateVmConfig<MailMessageViewModel>("Mail")
                .Title("Почта")
                .ListView(x => x
                    .Title("Почта")
                    .Type(ListViewType.Custom)                   
                    .Sortable(false))
                .DetailView(x => x.Title("Почтовое сообщение"))
                .Preset<ProfileMailSettings>();

            context.CreateVmConfig<ProfileMailSettings>()
                .Title("Почта - Настройки пользователя")
                .Service<IProfileMailSettingsService>()
                .DetailView(
                    x =>
                        x.Title("Настройки")
                            .IsMaximized(true)
                            .Toolbar(
                                t =>
                                    t.Add("GetToolbarPreset", "View",
                                        d => d.Title("Действия").ListParams(p => p.Add("mnemonic", "ProfileMailSettings")))))
                .ListView(x => x.Title("Настройки почты"));


            context.DataInitializer("Mail", "0.1", () =>
            {
                _settingService.Create(context.UnitOfWork, new MailSetting()
                {
                    Title = "Корпоративная почта"
                });
            });

            context.CreateVmConfig<MailSetting>()
                .Service<ISettingService<MailSetting>>();
        }
    }
}
