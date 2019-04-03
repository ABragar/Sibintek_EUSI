using Base.Ambient;
using Base.DAL;
using Base.Reporting.Entities;
using Base.Settings;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Reporting
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<ReportingSetting>()
                .Service<ISettingService<ReportingSetting>>()
                .Title("Настройки отчетов")
                .DetailView(x => x.Title("Настройки отчетов"))
                .ListView(x => x.Title("Настройки отчетов").HiddenActions(new[] { LvAction.Create, LvAction.Delete }));

            context.CreateVmConfig<PrintedFormList>()
                .Title("Реестр печатных форм")
                .ListView(l => l
                    .Title("Реестр печатных форм"));

            context.CreateVmConfig<PrintedFormRegistry>()
                .Title("Администрирование печатных форм")
                .ListView(l => l.Title("Администрирование печатных форм"))
                .DetailView(d => d.Title("Печатная форма"));
        }
    }
}