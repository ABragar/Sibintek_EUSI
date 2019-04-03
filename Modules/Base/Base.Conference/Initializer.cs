using System.Linq;
using Base.Conference.Entities;
using Base.Conference.Service;
using Base.DAL;
using Base.Settings;
using Base.UI;

namespace Base.Conference
{

    public class Initializer : IModuleInitializer
    {
        private readonly ISettingService<ConferenceSetting> _settingService;
        

        public Initializer(ISettingService<ConferenceSetting> settingService)
        {
            _settingService = settingService;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Entities.Conference>()
                .Service<IConferenceService>()
                .Title("Конференции")
                .DetailView(x => x.Title("Конференция"))
                .ListView(x => x.Title("Конференции"));

            context.CreateVmConfig<Entities.PrivateMessage>()
                .Service<IPrivateMessageService>()
                .Title("Приватные сообщения")
                .DetailView(x => x.Title("Сообщение"))
                .ListView(x => x.Title("Приватные сообщения"));

            context.CreateVmConfig<Entities.PrivateMessage>("MissedPrivateMessage")
                .Service<IPrivateMessageService>()
                .Title("Пропущенные приватные сообщения")
                .ListView(x =>
                    x.DataSource(d =>
                        d.Filter(f => f.IsNew && f.ToUserId == FilterParams.CurrentUserId)));

            context.CreateVmConfig<Entities.PublicMessage>()
                .Service<IPublicMessageService>()
                .Title("Публичные сообщения")
                .DetailView(x => x.Title("Сообщение"))
                .ListView(x => x.Title("Публичные сообщения"));

            context.CreateVmConfig<Entities.PublicMessage>("MissedPublicMessage")
                .Service<IPublicMessageService>()
                .Title("Пропущенные публичные сообщения")
                .ListView(x =>
                    x.DataSource(d =>
                        d.Filter(f => f.IsNew && f.ToConference.Members.Any(g => g.ObjectID == FilterParams.CurrentUserId) && f.FromId!= FilterParams.CurrentUserId)));

            context.CreateVmConfig<ConferenceSetting>()
                .Service<ISettingService<ConferenceSetting>>()
                .Title("Настройки ИКР")
                .ListView(l => l.Title("Настройки ИКР"))
                .DetailView(d => d.Title("Настройки ИКР"));

            context.DataInitializer("Conference", "0.1", () =>
            {
                _settingService.Create(context.UnitOfWork, new ConferenceSetting()
                {
                    Title = "ИКР",
                    StorageSize = 500,
                    StorageTime = 720
                });
            });
        }
    }
}
