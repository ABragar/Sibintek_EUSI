using System.ComponentModel;
using Base.Audit.Entities;
using Base.DAL;
using Base.Settings;
using System.Linq;
using Base.Audit.Services;
using Base.UI;

namespace Base.Audit
{

    public class Initializer : IModuleInitializer
    {
        private readonly ISettingService<AuditSetting> _settingService;
        
        public Initializer(ISettingService<AuditSetting> settingService)
        {
            _settingService = settingService;
        }

        public void Init(IInitializerContext context)
        {

            context.CreateVmConfig<AuditItem>()
                .Service<IAuditItemService>()
                .Title("Аудит")
                .DetailView(x => x.Title("Аудит - Запись"))
                .ListView(x => x
                    .Title("Аудит")
                    .DataSource(ds => ds
                        .Sort(sort => sort.Add(s => s.ID, ListSortDirection.Descending))));

            context.CreateVmConfig<AuditSetting>()
               .Service<ISettingService<AuditSetting>>()
               .Title("Настройки аудита");

            context.CreateVmConfig<AuditAuthResult>()
                .DetailView(x => x.Title("Cобытие авторизации").HideToolbar(true))
                .LookupProperty(x => x.Text(t => t.Login));

            context.CreateVmConfig<AuditRegisterResult>()
                .DetailView(x => x.Title("Cобытие регистрации").HideToolbar(true))
                .LookupProperty(x => x.Text(t => t.Login));

            context.DataInitializer("Audit", "0.1", () =>
            {
                _settingService.Create(context.UnitOfWork, new AuditSetting()
                {
                    Title = "Аудит",
                    RegisterLogIn = true,
                    MaxRecordCount = -1
                });
            });
        }
    }
}
