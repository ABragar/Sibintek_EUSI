using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Services.Import;
using EUSI.Entities.Estate;
using System.Collections.Generic;
using System.Linq;

namespace EUSI.Model.Import
{
    public static class ImportHistoryModel
    {
        /// <summary>
        /// Создает конфигурацию модели истории импорта по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            
            context.CreateVmConfigOnBase<ImportErrorLog>(nameof(ImportErrorLog), "ImportErrorLog_Sheet")
                .DetailView(dv => dv.Editors(eds => eds.Add(ed => ed.Sheet, ac=> ac.Visible(true))))
                .ListView(lv => lv.Columns(cols => cols.Add(col => col.Sheet, ac => ac.Visible(true))))
                ;


            //импорт заявок
            context.CreateVmConfig<ImportHistory>("ImportEstateRegistration")
                  .Service<IImportHistoryService>()
                  .Title("Импорт заявок на регистрацию")
                  .DetailView(x => x
                   .Title("Импорт заявок на регистрацию")
                   .Editors(editors => editors
                    .Add(ed => ed.ImportDateTime, ac => ac.Title("Дата/Время").IsReadOnly(true).Order(1))
                    .Add(ed => ed.Mnemonic, ac => ac.Title("Тип объекта").IsReadOnly(true).Order(2))
                    .Add(ed => ed.SibUser, ac => ac.Title("Пользователь").Order(3).IsReadOnly(true).Visible(true))
                    .Add(ed => ed.FileCard, ac => ac.Title("Файл").Visible(true).IsReadOnly(true).Order(4))
                    .Add(ed => ed.ResultText, ac => ac.Title("Результат").IsReadOnly(true).Order(5))
                    .Add(ed => ed.IsResultSentByEmail, ac => ac.IsReadOnly(true).Order(500))
                    .Add(ed => ed.SentByEmailDate, ac => ac.IsReadOnly(true).Order(600))

                    .AddOneToManyAssociation<ImportErrorLog>("ImportHistory_ImportErrorLog",
                        y => y.TabName("Журнал ошибок")
                            .IsReadOnly(true)
                            .TabName("Журнал ошибок")
                            .Mnemonic("ImportErrorLog_Sheet")                            
                            .Filter((work, logs, id, oid) => logs.Where(w => w.ImportHistoryID == id))                        
                        )
                   ))
                  .ListView_ImportER()
                  .LookupProperty(x => x.Text(c => c.ID))
                 ;

            //импорт пользовательских файлов
            context.CreateVmConfig<ImportHistory>("ImportBCSData")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных BCS")
                  .DetailView_Default()
                  .DetailView(dv => dv.Editors(eds =>eds
                        .Add(ed => ed.ActualityDate, ac=>ac.Visible(true).IsReadOnly(true))
                        .Add(ed => ed.CurrentFileUser, ac => ac.Visible(true).IsReadOnly(true))))
                  .ListView_ImportBCSData()                  
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

            context.ModifyVmConfig<ImportHistory>("ImportAccountingObject")
                .DetailView(dv => dv.Editors(eds => eds
                .Add(ed => ed.ContactName, ac => ac.Visible(true).Title("ФИО ответственного за заполнение файла"))
                .Add(ed => ed.ContactPhone, ac => ac.Visible(true).Title("Контактный телефон ответственного за заполнение файла"))
                .Add(ed => ed.ContactEmail, ac => ac.Visible(true).Title("E-mail ответственного за заполнение файла"))
                .Add(c => c.IsResultSentByEmail, ac => ac.Visible(true))
                .Add(c => c.SentByEmailDate, ac => ac.Visible(true))
                ))
                .ListView(lv => lv.Columns(cols => cols
                .Add(c => c.ContactEmail, ac => ac.Visible(true).Title("E-mail ответственного").Order(100))
                .Add(c => c.IsResultSentByEmail, ac=>ac.Visible(true).Order(500))
                .Add(c => c.SentByEmailDate, ac => ac.Visible(true).Order(600))
                )
                .DataSource(ds => ds.Filter(f => f.Mnemonic.Contains("AccountingObject") || f.Mnemonic.Contains("AccountingMoving")))
                 );


            //импорт данных ИС НА
            context.CreateVmConfig<ImportHistory>("ImportDeclaration")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных ИС НА")
                  .DetailView_Default()
                  .ListView_ImportDeclaration()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;


            //импорт журнала загрузки из БУС
            context.CreateVmConfig<ImportHistory>("ImportExternalLog")
                  .Service<IImportHistoryService>()
                  .Title("Импорт журнала загрузки из БУС")
                  .DetailView_Default()
                  .ListView_Default()
                  .ListView(lv => lv.DataSource(ds => ds.Filter(f => f.Mnemonic == "ExternalImportLog")))
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

            //импорт протокола сверки сальдо
            context.CreateVmConfig<ImportHistory>("ImportSaldo")
                  .Service<IImportHistoryService>()
                  .Title("Импорт протокола сверки сальдо")
                  .DetailView_Default()
                  .ListView_Default()
                  .ListView(lv => lv.DataSource(ds => ds.Filter(f => f.Mnemonic == "BalanceReconciliationReport")))
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;


            //импорт миграции ОС/НМА
            context.CreateVmConfig<ImportHistory>("ImportMigrateOS")
                  .Service<IImportHistoryService>()
                  .Title("Миграция ОС/НМА")
                  .DetailView_Default()
                  .ListView_Default()
                  .ListView(lv => lv.DataSource(ds => ds.Filter(f => f.Mnemonic == nameof(Entities.Accounting.MigrateOS))))
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

            
            //импорт ИР-Аренда
            context.CreateVmConfig<ImportHistory>("ImportRentalOS")
                  .Service<IImportHistoryService>()
                  .Title("Импорт ФСД аренда")
                  .DetailView_Default()
                  .ListView_Default()
                  .ListView(lv => lv.DataSource(ds => ds
                    .Filter(f => f.Mnemonic == nameof(Entities.Accounting.RentalOS))))
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

            context.CreateVmConfig<ImportHistory>("ImportRentalOSMoving")
                  .Service<IImportHistoryService>()
                  .Title("Импорт ФСД движения (аренда)")
                  .DetailView_Default()
                  .ListView_Default()
                  .ListView(lv => lv.DataSource(ds => ds
                    .Filter(f => f.Mnemonic == nameof(Entities.Accounting.RentalOSMoving))))
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

            context.CreateVmConfig<ImportHistory>("ImportRentalOSState")
                  .Service<IImportHistoryService>()
                  .Title("Импорт ФСД состояния (аренда)")
                  .DetailView_Default()
                  .ListView_Default()
                  .ListView(lv => lv.DataSource(ds => ds
                    .Filter(f => f.Mnemonic == nameof(Entities.Accounting.RentalOSState))))
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

        }

        /// <summary>
        /// Конфигурация карточки истории импорта по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> DetailView_Default(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                    .Add(ed => ed.ImportDateTime, ac=>ac.Title("Дата/Время").IsReadOnly(true).Order(1))
                    .Add(ed => ed.Mnemonic, ac => ac.Title("Тип объекта").IsReadOnly(true).Order(2))
                    .Add(ed => ed.SibUser, ac => ac.Title("Пользователь").Order(3).IsReadOnly(true).Visible(true))
                    .Add(ed => ed.FileCard, ac => ac.Title("Файл").Visible(true).IsReadOnly(true).Order(4))
                    .Add(ed => ed.ResultText, ac => ac.Title("Результат").IsReadOnly(true).Order(5))
                    .Add(ed => ed.IsResultSentByEmail, ac => ac.IsReadOnly(true).Order(500))
                    .Add(ed => ed.SentByEmailDate, ac => ac.IsReadOnly(true).Order(600))

                     .AddOneToManyAssociation<ImportErrorLog>("ImportHelper_ImportErrorLog",
                        y => y.TabName("Журнал ошибок")
                            .IsReadOnly(true)
                            .TabName("Журнал ошибок")
                            .Mnemonic("ImportErrorLog")
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ImportHistoryID == id))
                        )

                    .AddOneToManyAssociation<ImportErrorLog>("ImportHistory_OSLogs",
                        y => y.TabName("Журнал ошибок")
                            .IsReadOnly(true)
                            .TabName("Журнал ошибок")
                            .Mnemonic("OSLogs")
                            .Filter((work, logs, id, oid) => logs.Where(w => w.ImportHistoryID == id))
                        )
              )
              .DefaultSettings((uow, obj, editor) =>
              {
                  if (obj.ID == 0)
                      return;

                  var mnems = new List<string>()
                  {
                      nameof(AccountingObject)
                      , "AccountingMoving"
                  };
                  if (mnems.Contains(obj.Mnemonic))
                  {
                      editor.Visible("ImportHelper_ImportErrorLog", false);
                      editor.Visible("ImportHistory_OSLogs", true);
                  }
                  else
                  {
                      editor.Visible("ImportHelper_ImportErrorLog", true);
                      editor.Visible("ImportHistory_OSLogs", false);
                  }
              })
              );
        }

        /// <summary>
        /// Конфигурация реестра истории импорта по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_Default(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            return
                conf.ListView(x => x
                .Title("История импорта")
                .HiddenActions(new[] { LvAction.Create, LvAction.Edit })
                 .Columns(col => col
                    .Add(c => c.ImportDateTime, ac => ac.Title("Дата/Время").Visible(true).Order(1))
                    .Add(c => c.Mnemonic, ac => ac.Title("Тип объекта").Visible(true).Order(2).DataType(Base.Attributes.PropertyDataType.ExtraId))
                    .Add(c => c.SibUser, ac => ac.Title("Пользователь").Visible(true).Order(3))
                    .Add(c => c.FileCard, ac => ac.Title("Файл").Visible(true).Order(4))
                    .Add(c => c.ResultText, ac => ac.Title("Результат").Visible(true).Order(5))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false).Order(6))
                    .Add(c => c.IsResultSentByEmail, ac => ac.Visible(false).Order(600))
                    .Add(c => c.SentByEmailDate, ac => ac.Visible(false).Order(700))
                 )
               );

        }
        
        /// <summary>
        /// Конфигурация реестра истории импорта заявок.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportER(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = nameof(EstateRegistration);

            return conf.ListView_Default()
                .ListView(l => l
                    .Title(conf.Config.Title)
                    .DataSource(ds => ds.Filter(f => str.Contains(f.Mnemonic)))
                    .Columns(cols => cols
                        .Add(c => c.IsResultSentByEmail, ac => ac.Visible(true))
                        .Add(c => c.SentByEmailDate, ac => ac.Visible(true))
                    )
                );
        }

        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportBCSData(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(Entities.BSC.BCSData).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => str.Contains(f.Mnemonic)))
                             .Columns(cols => cols
                                .Add(col => col.ActualityDate, ac => ac.Visible(true).Order(900))
                                .Add(col => col.CurrentFileUser, ac => ac.Visible(true).Order(901))
                                )
                             );

        }

        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportDeclaration(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            List<string> list = new List<string>()
            {
                 nameof(EUSI.Entities.NU.Declaration)
                ,nameof(EUSI.Entities.NU.DeclarationCalcEstate)
                ,nameof(EUSI.Entities.NU.DeclarationEstate)
                ,nameof(EUSI.Entities.NU.DeclarationLand)              
                ,nameof(EUSI.Entities.NU.DeclarationVehicle)
            };

            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => list.Contains(f.Mnemonic)))
                             );

        }


    }
}
