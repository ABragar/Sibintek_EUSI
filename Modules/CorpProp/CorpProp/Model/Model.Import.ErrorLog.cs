using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Services.Import;
using System.Linq;

namespace CorpProp.Model.Import
{
    public static class ImportErrorLogModel
    {
        /// <summary>
        /// Создает конфигурацию модели журнала ошибок по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ImportErrorLog>()
                   .Service<IImportErrorLogService>()
                   .Title("Журнал ошибок")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.ID))
                   ;

            //конфиг для журнала ошибок импорта данных Росреестра.
            context.CreateVmConfig<ImportErrorLog>("RosReestrLogs")
                  .Service<IImportErrorLogService>()
                  .Title("Журнал ошибок")
                  .DetailView_RosReestrLogs()
                  .ListView_RosReestrLogs()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

            //конфиг для журнала ошибок импорта БУ.
            context.CreateVmConfigOnBase<ImportErrorLog>(nameof(ImportErrorLog), "OSLogs")
                  .DetailView(dv => dv
                    .Editors(eds => eds
                        .Add(ed => ed.ConsolidationTitle, ac => ac.Visible(true).IsReadOnly(true))
                        .Add(ed => ed.EusiNumber, ac => ac.Visible(true).IsReadOnly(true))
                        .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).IsReadOnly(true))
                       ))
                  .ListView(lv => lv
                    .Columns(cols => cols
                        .Add(ed => ed.ConsolidationTitle, ac => ac.Visible(true))
                        .Add(col => col.EusiNumber, ac => ac.Visible(true))
                        .Add(col => col.InventoryNumber, ac => ac.Visible(true))                       
                        ))
                        ;
        }

        /// <summary>
        /// Конфигурация карточки журнала ошибок по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportErrorLog> DetailView_Default(this ViewModelConfigBuilder<ImportErrorLog> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title));
        }

        /// <summary>
        /// Конфигурация реестра журнала ошибок по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportErrorLog> ListView_Default(this ViewModelConfigBuilder<ImportErrorLog> conf)
        {
            return
                conf.ListView(x => x
                .Title("Журнал ошибок")
                .HiddenActions(new[] { LvAction.Create, LvAction.Edit })
                .Columns(cols => cols
                .Add(col => col.ColumnNumber, ac => ac.Order(-100))
                .Add(col => col.PropetyName, ac => ac.Order(-99))
                .Add(col => col.RowNumber, ac => ac.Order(-98))
                .Add(col => col.ContactName)
                .Add(col => col.ContactEmail)
                .Add(col => col.ContactPhone)
                .Add(col => col.HistoryImportDateTime)
                )
               );

        }


        /// <summary>
        /// Конфигурация карточки журнала ошибок импора данных Росреестра.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportErrorLog> DetailView_RosReestrLogs(this ViewModelConfigBuilder<ImportErrorLog> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors=>editors
                .Add(ed => ed.RowNumber, ac => ac.Visible(true).Title("Номер строки").Order(1))
                .Add(ed=>ed.ColumnNumber, ac=>ac.Visible(true).Title("Номер позиции").Order(2))                
                .Add(ed => ed.PropetyName, ac => ac.Visible(false).Title("Элемент"))
                .Add(ed => ed.ErrorText, ac => ac.Visible(true))
                .Add(ed => ed.ErrorType, ac => ac.Visible(false))
                .Add(ed => ed.MessageDate, ac => ac.Visible(true))              
               ));
        }

        /// <summary>
        /// Конфигурация реестра журнала ошибок импора данных Росреестра.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportErrorLog> ListView_RosReestrLogs(this ViewModelConfigBuilder<ImportErrorLog> conf)
        {
            return
                conf.ListView(x => x
                .Title("Журнал ошибок")
                .Columns(columns=>columns
                    .Add(ed => ed.RowNumber, ac => ac.Visible(true).Title("Номер строки").Order(1))
                    .Add(ed => ed.ColumnNumber, ac => ac.Visible(true).Title("Номер позиции").Order(2))
                    .Add(ed => ed.PropetyName, ac => ac.Visible(false).Title("Элемент"))
                    .Add(ed => ed.ErrorText, ac => ac.Visible(true))
                    .Add(ed => ed.ErrorType, ac => ac.Visible(false))
                    .Add(ed => ed.MessageDate, ac => ac.Visible(true))                    
                ));

        }

    }
}
