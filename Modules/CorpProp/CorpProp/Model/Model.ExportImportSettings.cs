using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.UI;
using CorpProp.Entities.Settings;
using CorpProp.Services.Settings;

namespace CorpProp.Model.Settings
{
    public static class ExportImportSettingsModel
    {
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<ExportImportSettings>()
                .Service<IExportImportSettingsService>()
                .Title("Настройки импорта/экспорта")
                .ListView_Default()
                .DetailView_Default()
                .LookupProperty(lp => lp.Text(t => t.Title));
                ;
        }

        public static ViewModelConfigBuilder<ExportImportSettings> ListView_Default(
            this ViewModelConfigBuilder<ExportImportSettings> conf)
        {
            return
                conf.ListView(lv => lv
                    .Title("Настройки импорта/экспорта")
                    .Columns(c => c
                        .Add(a => a.OperationType)
                        .Add(a => a.Society)
                        .Add(a => a.FileCard)
                        .Add(a => a.AccountingSystem)
                    )
                )
                ;
        }

        public static ViewModelConfigBuilder<ExportImportSettings> DetailView_Default(
            this ViewModelConfigBuilder<ExportImportSettings> conf)
        {
            return
                conf.DetailView(dv => dv
                    .Editors(edt => edt
                        .Add(a => a.OperationType)
                        .Add(a => a.Society)
                        .Add(a => a.FileCard)
                        .Add(a => a.AccountingSystem)
                    )
                )
                ;
        }
    }
}
