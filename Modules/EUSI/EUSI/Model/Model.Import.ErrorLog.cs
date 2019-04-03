using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Import;
using CorpProp.Services.Import;
using EUSI.Entities.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Model.Import
{
    public static class ImportErrorLogModel
    {
        /// <summary>
        /// Мнемоника для журнала ошибок импорта заявок на регистрацию
        /// </summary>
        public const string ImportErrorLogEstateRegistration = nameof(ImportErrorLogEstateRegistration);

        /// <summary>
        /// Создает конфигурацию модели журнала ошибок.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            // Журнал ошибок/Отчёт истории импорта (Заявки на регистрацию ОИ)
            context.CreateVmConfig<ImportErrorLog>(ImportErrorLogEstateRegistration)
                .Service<IImportErrorLogService>()
                .Title("Журнал ошибок/Отчёт истории импорта (Заявки на регистрацию ОИ)")
                .DetailView_EstateRegistration()
                .ListView_EstateRegistration()
                .LookupProperty(x => x.Text(c => c.ID));
        }

        /// <summary>
        /// Конфигурация карточки журнала ошибок для заявок на регитрацию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportErrorLog> DetailView_EstateRegistration(this ViewModelConfigBuilder<ImportErrorLog> conf)
        {
            return conf.DetailView(x => x.Title(conf.Config.Title));
        }

        /// <summary>
        /// Конфигурация реестра журнала ошибок для заявок на регитрацию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportErrorLog> ListView_EstateRegistration(this ViewModelConfigBuilder<ImportErrorLog> conf)
        {
            return conf.ListView(x => x
                .HiddenActions(new[] { LvAction.Create, LvAction.Edit })
                .Columns(cols => cols.Clear()
                    .Add(col => col.HistoryImportDateTime, ac => ac.Visible(true))
                    .Add(col => col.ConsolidationTitle, ac => ac.Visible(true))
                    .Add(col => col.ContactName, ac => ac.Visible(true).Title("Инициатор заявки"))
                    .Add(col => col.ContactEmail, ac => ac.Visible(true).Title("Email  инициатора"))
                    .Add(col => col.ContactPhone, ac => ac.Visible(true).Title("Телефон инициатора"))
                    .Add(col => col.ErrorType, ac => ac.Visible(true))
                    .Add(col => col.Comment, ac => ac.Visible(true))
                )
                .DataSource(ds => ds
                    .Filter(irf => irf.ImportHistory.Mnemonic == nameof(EstateRegistration))
                    .Groups(grs => grs.Groupable(true).Add(gr => gr.ConsolidationTitle).Add(gr => gr.ContactName))
                ));
        }
    }
}
