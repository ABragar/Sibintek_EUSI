using Base.DAL;
using Base.Notification.Entities;
using Base.Settings;
using System.Linq;
using Base.Notification.Service.Abstract;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Notification
{
    public class Initializer : IModuleInitializer
    {
        private readonly ISettingService<NotificationSetting> _settingService;

        public Initializer(ISettingService<NotificationSetting> settingService)
        {
            _settingService = settingService;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Entities.Notification>()
                .Service<INotificationService>()
                .Title("Уведомления")
                .ListView(
                    x =>
                        x.Title("Уведомления")
                            .HiddenActions(new[] {
                                LvAction.Create,
                                LvAction.ChangeCategory,
                                LvAction.AllCategorizedItems,
                                LvAction.Settings,
                                LvAction.Export,
                                LvAction.Search,
                                LvAction.Edit})
                            .DataSource(
                                d =>
                                    d.Filter(f => !f.Hidden && f.UserID == FilterParams.CurrentUserId)))
                .DetailView(x => x.Title("Уведомлениe"));

            context.CreateVmConfig<NotificationSetting>().Title("Уведомления - Настройки");

            context.DataInitializer("Notification", "0.1", () =>
            {
                _settingService.Create(context.UnitOfWork, new NotificationSetting()
                {
                    Title = "Уведомления",
                    EnableEmail = false,
                    AccountTitle = "System"
                });
            });

        }
    }
}
