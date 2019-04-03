using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.Mapping;
using CorpProp.Entities.NSI;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Subject;
using CorpProp.Services.Import;
using System.Collections.Generic;
using System.Linq;

namespace CorpProp.Model.Import
{
    public static class ImportHistoryModel
    {
        static void CreateVmConfigBasedOnFileCard(IInitializerContext context, string[] mnemonics)
        {
            foreach (var mnemonic in mnemonics)
            {
                context.CreateVmConfigOnBase<FileCard>("FileCardTree", "FileCardTree" + mnemonic);
            }
        }
        /// <summary>
        /// Создает конфигурацию модели истории импорта по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            //по умолчанию
            context.CreateVmConfig<ImportHistory>()
                   .Service<IImportHistoryService>()
                   .Title("История импорта")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.ID))
                   ;

            //импорт данных БУ
            context.CreateVmConfig<ImportHistory>("ImportAccountingObject")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных БУ")
                  .DetailView_Default()
                  .ListView_ImportAccountingObject()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;

            //импорт данных ОГ
            context.CreateVmConfig<ImportHistory>("ImportSociety")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных ОГ")
                  .DetailView_Default()
                  .ListView_ImportSociety()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных ОГ
            context.CreateVmConfig<ImportHistory>("ImportShareholder")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных акционеров")
                  .DetailView_Default()
                  .ListView_ImportShareholder()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных ДП
            context.CreateVmConfig<ImportHistory>("ImportSubject")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных ДП")
                  .DetailView_Default()
                  .ListView_ImportSubject()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт банковских реквизитов
            context.CreateVmConfig<ImportHistory>("ImportBankingDetail")
                  .Service<IImportHistoryService>()
                  .Title("Импорт банковских реквизитов")
                  .DetailView_Default()
                  .ListView_ImportBankingDetail()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных оценщика за финансовый год
            context.CreateVmConfig<ImportHistory>("ImportAppraiserDataFinYear")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных оценщика за финансовый год")
                  .DetailView_Default()
                  .ListView_ImportAppraiserDataFinYear()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных оценочных организаций
            context.CreateVmConfig<ImportHistory>("ImportAppraisalOrgData")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных оценочных организаций")
                  .DetailView_Default()
                  .ListView_ImportAppraisalOrgData()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных оценок
            context.CreateVmConfig<ImportHistory>("ImportAppraisal")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных оценок")
                  .DetailView_Default()
                  .ListView_ImportAppraisal()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных объектов оценки
            context.CreateVmConfig<ImportHistory>("ImportEstateAppraisal")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных объектов оценки")
                  .DetailView_Default()
                  .ListView_ImportEstateAppraisal()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных ггр
            context.CreateVmConfig<ImportHistory>("ImportSSR")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных ГГР")
                  .DetailView_Default()
                  .ListView_ImportSSR()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных нна
            context.CreateVmConfig<ImportHistory>("ImportNCA")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных ННА")
                  .DetailView_Default()
                  .ListView_ImportNCA()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт данных сделок
            context.CreateVmConfig<ImportHistory>("ImportDeal")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных сделок")
                  .DetailView_Default()
                  .ListView_ImportDeal()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт прочих данных КИС
            context.CreateVmConfig<ImportHistory>("ImportKIS")
                  .Service<IImportHistoryService>()
                  .Title("Импорт прочих данных КИС")
                  .DetailView_Default()
                  .ListView_ImportKIS()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //импорт НСИ
            context.CreateVmConfig<ImportHistory>("ImportNSI")
                  .Service<IImportHistoryService>()
                  .Title("Импорт НСИ")
                  .DetailView_Default()
                  .ListView_ImportNSI()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();


            //импорт пользовательских файлов
            context.CreateVmConfig<ImportHistory>("ImportOtherFiles")
                  .Service<IImportHistoryService>()
                  .Title("Импорт пользовательских файлов")
                  .DetailView_Default()
                  .ListView_ImportOtherFiles()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();


            //импорт данных Росреестра
            context.CreateVmConfig<ImportHistory>("ImportRosReestr")
                  .Service<IImportHistoryService>()
                  .Title("Импорт данных Росреестра")
                  .DetailView_ImportRosReestr()
                  .ListView_ImportRosReestr()
                  .LookupProperty(x => x.Text(c => c.ID))
                  ;//.IsReadOnly();

            //журнал передачи данных
            context.CreateVmConfig<ImportHistory>("ImportDataTransfer")
                .Service<IImportHistoryService>()
                .Title("Журнал передачи данных")
                .DetailView_ImportDataTransfer()
                .ListView_ImportDataTransfer()
                .LookupProperty(x => x.Text(c => c.ID))
                ;//.IsReadOnly();


            //архив истории импорта
            context.CreateVmConfig<ImportHistory>("ImportHistoryArhive")
                .Service<IImportHistoryService>()
                .Title("Архив истории импорта")
                .DetailView_Default()
                .ListView_ImportArhive()
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
                    .Add(ed => ed.DataVersion, ac => ac.Title("Версия данных").Visible(true).IsReadOnly(true).Order(5))
                    .Add(ed => ed.ResultText, ac => ac.Title("Результат").IsReadOnly(true).Order(6))
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
                .DataSource(ds => ds.Filter(f => f.ImportHistoryState == null || (f.ImportHistoryState != null && f.ImportHistoryState.Code != "Arhive")))
                 .Columns(col => col
                    .Add(c => c.ImportDateTime, ac => ac.Title("Дата/Время").Visible(true).Order(1))
                    .Add(c => c.Mnemonic, ac => ac.Title("Тип объекта").Visible(true).Order(2).DataType(Base.Attributes.PropertyDataType.ExtraId))
                    .Add(c => c.SibUser, ac => ac.Title("Пользователь").Visible(true).Order(3))
                    .Add(c => c.FileCard, ac => ac.Title("Файл").Visible(true).Order(4))
                    .Add(ed => ed.DataVersion, ac => ac.Title("Версия данных").Visible(true).Order(5))
                    .Add(c => c.ResultText, ac => ac.Title("Результат").Visible(true).Order(6))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false).Order(7))
                    .Add(c => c.IsResultSentByEmail, ac => ac.Visible(false).Order(600))
                    .Add(c => c.SentByEmailDate, ac => ac.Visible(false).Order(700))
                 )
               );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных БУ.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportAccountingObject(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(AccountingObject).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l                           
                             .Title(conf.Config.Title)
                             .DataSource(ds=>ds.Filter(f=>f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных ОГ.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportSociety(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(Society).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных акционеров.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportShareholder(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(Shareholder).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных ДП.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportSubject(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(Subject).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта банковских реквизитов.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportBankingDetail(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(BankingDetail).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных оценщика за финансовый год.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportAppraiserDataFinYear(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(AppraiserDataFinYear).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных оценочных организаций.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportAppraisalOrgData(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(AppraisalOrgData).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }
        

        /// <summary>
        /// Конфигурация реестра истории импорта данных оценок.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportAppraisal(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(Appraisal).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных оценок.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportEstateAppraisal(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(EstateAppraisal).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных ггр.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportSSR(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(ScheduleStateRegistration).Name;
            string str1 = typeof(ScheduleStateTerminate).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str || f.Mnemonic == str1))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных нна.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportNCA(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(NonCoreAsset).Name;
            string str1 = typeof(NonCoreAssetList).Name;
            string str2 = typeof(NonCoreAssetAndList).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str || f.Mnemonic == str1 || f.Mnemonic == str2))
                             );

        }


        /// <summary>
        /// Конфигурация реестра истории импорта данных оценок.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportDeal(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(SibDeal).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.Mnemonic == str))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта прочих данных КИС.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportKIS(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(Society).Name;
            str += ";" + typeof(SibDeal).Name;
            str += ";" + typeof(Shareholder).Name;
          
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => str.Contains(f.Mnemonic)))
                             );

        }




        /// <summary>
        /// Конфигурация реестра истории импорта НСИ.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportNSI(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            #region Мнемоники справочников
            //Локальные
            string str = typeof(EstateType).Name;
            str += ";" + typeof(BusinessArea).Name;
            str += ";" + typeof(SubjectKind).Name;
            str += ";" + typeof(RightKind).Name;
            str += ";" + typeof(EncumbranceType).Name;
            str += ";" + typeof(ReceiptReason).Name;
            str += ";" + typeof(LeavingReason).Name;
            str += ";" + typeof(AccountingStatus).Name;
            str += ";" + typeof(DocKind).Name;
            str += ";" + typeof(ShipType).Name;
            str += ";" + typeof(ShipAssignment).Name;
            str += ";" + typeof(ShipClass).Name;
            str += ";" + typeof(AircraftKind).Name;
            str += ";" + typeof(AircraftType).Name;
            str += ";" + typeof(StageOfCompletion).Name;
            str += ";" + typeof(BaseInclusionInPerimeter).Name;
            str += ";" + typeof(BaseExclusionFromPerimeter).Name;
            str += ";" + typeof(IntangibleAssetType).Name;
            str += ";" + typeof(PropertyComplexKind).Name;
            str += ";" + typeof(RealEstateKind).Name;
            str += ";" + typeof(NonCoreAssetType).Name;
            str += ";" + typeof(NonCoreAssetListType).Name;
            str += ";" + typeof(NonCoreAssetListKind).Name;
            str += ";" + typeof(NonCoreAssetAppraisalType).Name;
            str += ";" + typeof(ImplementationWay).Name;
            str += ";" + typeof(NonCoreAssetSaleAcceptType).Name;
            str += ";" + typeof(OwnershipType).Name;
            str += ";" + typeof(FeatureType).Name;
            str += ";" + typeof(SibProjectType).Name;
            str += ";" + typeof(EstateAppraisalType).Name;
            str += ";" + typeof(AppraisalType).Name;
            str += ";" + typeof(LandType).Name;
            str += ";" + typeof(RightHolderKind).Name;
            str += ";" + typeof(IntangibleAssetStatus).Name;
            str += ";" + typeof(ScheduleStateRegistrationStatus).Name;
            str += ";" + typeof(RegistrationBasis).Name;
            str += ";" + typeof(BIK).Name;
            str += ";" + typeof(InvestmentType).Name;
            str += ";" + typeof(SuccessionType).Name;
            str += ";" + typeof(DocStatus).Name;
            str += ";" + typeof(DocTypeOperation).Name;
            str += ";" + typeof(ShipKind).Name;
            str += ";" + typeof(VehicleCategory).Name;
            str += ";" + typeof(ExtractFormat).Name;
            str += ";" + typeof(ExtractType).Name;
            str += ";" + typeof(RightType).Name;
            str += ";" + typeof(ExternalMappingSystem).Name;
            str += ";" + typeof(AccountingMovingType).Name;
            str += ";" + typeof(ActualKindActivity).Name;
            str += ";" + typeof(AddonAttributeGroundCategory).Name;
            str += ";" + typeof(AppraisalGoal).Name;
            str += ";" + typeof(AppraisalPurpose).Name;
            str += ";" + typeof(BasisForInvestments).Name;
            str += ";" + typeof(BusinessDirection).Name;
            str += ";" + typeof(BusinessSegment).Name;
            str += ";" + typeof(ContourType).Name;
            str += ";" + typeof(ContragentKind).Name;
            str += ";" + typeof(DealType).Name;
            str += ";" + typeof(DepreciationGroup).Name;
            str += ";" + typeof(InformationSource).Name;
            str += ";" + typeof(IntangibleAssetRightType).Name;
            str += ";" + typeof(LayingType).Name;
            str += ";" + typeof(SibLocation).Name;
            str += ";" + typeof(NonCoreAssetListItemState).Name;
            str += ";" + typeof(OKOFS).Name;
            str += ";" + typeof(OKOGU).Name;
            str += ";" + typeof(OKOPF).Name;
            str += ";" + typeof(ProductionBlock).Name;
            str += ";" + typeof(RealEstatePurpose).Name;
            str += ";" + typeof(RequestStatus).Name;
            str += ";" + typeof(ResponseStatus).Name;
            str += ";" + typeof(SibBank).Name;
            str += ";" + typeof(SibKBK).Name;
            str += ";" + typeof(SignType).Name;
            str += ";" + typeof(SocietyCategory1).Name;
            str += ";" + typeof(SocietyCategory2).Name;
            str += ";" + typeof(SourceInformationType).Name;
            str += ";" + typeof(StatusConstruction).Name;
            str += ";" + typeof(TaxExemption).Name;
            str += ";" + typeof(TaxNumberInSheet).Name;
            str += ";" + typeof(TypeData).Name;
            str += ";" + typeof(UnitOfCompany).Name;
            str += ";" + typeof(VehicleKindCode).Name;
            str += ";" + typeof(WellCategory).Name;
            str += ";" + typeof(SibProjectStatus).Name;
            str += ";" + typeof(SibTaskReportStatus).Name;
            str += ";" + typeof(SibTaskStatus).Name;
            str += ";" + typeof(SubjectActivityKind).Name;
            str += ";" + typeof(SubjectType).Name;
            str += ";" + typeof(HolidaysCalendar).Name;
            //Централизованные
            str += ";" + typeof(OPF).Name;
            str += ";" + typeof(SibCountry).Name;
            str += ";" + typeof(SibFederalDistrict).Name;
            str += ";" + typeof(SibRegion).Name;
            str += ";" + typeof(SubjectType).Name;
            str += ";" + typeof(ClassFixedAsset).Name;
            str += ";" + typeof(VehicleType).Name;
            str += ";" + typeof(OKATO).Name;
            str += ";" + typeof(OKTMO).Name;
            str += ";" + typeof(OKOF94).Name;
            str += ";" + typeof(AddonOKOF).Name;
            str += ";" + typeof(OKOF2014).Name;
            str += ";" + typeof(RSBU).Name;
            str += ";" + typeof(SibMeasure).Name;
            str += ";" + typeof(GroundCategory).Name;
            str += ";" + typeof(SibDealStatus).Name;
            str += ";" + typeof(DocKind).Name;
            str += ";" + typeof(DocTypeOperation).Name;
            str += ";" + typeof(Currency).Name;
            str += ";" + typeof(Consolidation).Name;
            str += ";" + typeof(TenureType).Name;
            str += ";" + typeof(PropertyComplexIOType).Name;
            #endregion

            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => str.Contains(f.Mnemonic)))
                             );

        }

        /// <summary>
        /// Конфигурация реестра истории импорта пользовательских файлов.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportOtherFiles(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            #region Мнемоники Не пользовательских файлов
            //Локальные
            string str = typeof(EstateType).Name;
            str += ";" + typeof(BusinessArea).Name;
            str += ";" + typeof(BusinessBlock).Name;
            str += ";" + typeof(SubjectKind).Name;
            str += ";" + typeof(RightKind).Name;
            str += ";" + typeof(EncumbranceType).Name;
            str += ";" + typeof(ReceiptReason).Name;
            str += ";" + typeof(LeavingReason).Name;
            str += ";" + typeof(AccountingStatus).Name;
            str += ";" + typeof(DocKind).Name;
            str += ";" + typeof(ShipType).Name;
            str += ";" + typeof(ShipAssignment).Name;
            str += ";" + typeof(ShipClass).Name;
            str += ";" + typeof(AircraftKind).Name;
            str += ";" + typeof(AircraftType).Name;
            str += ";" + typeof(StageOfCompletion).Name;
            str += ";" + typeof(BaseInclusionInPerimeter).Name;
            str += ";" + typeof(BaseExclusionFromPerimeter).Name;
            str += ";" + typeof(IntangibleAssetType).Name;
            str += ";" + typeof(PropertyComplexKind).Name;
            str += ";" + typeof(RealEstateKind).Name;
            str += ";" + typeof(NonCoreAssetType).Name;
            str += ";" + typeof(NonCoreAssetListType).Name;
            str += ";" + typeof(NonCoreAssetListKind).Name;
            str += ";" + typeof(NonCoreAssetAppraisalType).Name;
            str += ";" + typeof(ImplementationWay).Name;
            str += ";" + typeof(NonCoreAssetSaleAcceptType).Name;
            str += ";" + typeof(OwnershipType).Name;
            str += ";" + typeof(FeatureType).Name;
            str += ";" + typeof(SibProjectType).Name;
            str += ";" + typeof(EstateAppraisalType).Name;
            str += ";" + typeof(AppraisalType).Name;
            str += ";" + typeof(LandType).Name;
            str += ";" + typeof(RightHolderKind).Name;
            str += ";" + typeof(IntangibleAssetStatus).Name;
            str += ";" + typeof(ScheduleStateRegistrationStatus).Name;
            str += ";" + typeof(RegistrationBasis).Name;
            str += ";" + typeof(BIK).Name;
            str += ";" + typeof(InvestmentType).Name;
            str += ";" + typeof(SuccessionType).Name;
            str += ";" + typeof(DocStatus).Name;
            str += ";" + typeof(DocTypeOperation).Name;
            str += ";" + typeof(ShipKind).Name;
            str += ";" + typeof(VehicleCategory).Name;
            str += ";" + typeof(ExtractFormat).Name;
            str += ";" + typeof(ExtractType).Name;
            str += ";" + typeof(RightType).Name;
            str += ";" + typeof(ExternalMappingSystem).Name;
            str += ";" + typeof(AccountingMovingType).Name;
            str += ";" + typeof(ActualKindActivity).Name;
            str += ";" + typeof(AddonAttributeGroundCategory).Name;
            str += ";" + typeof(AppraisalGoal).Name;
            str += ";" + typeof(AppraisalPurpose).Name;
            str += ";" + typeof(BasisForInvestments).Name;
            str += ";" + typeof(BusinessDirection).Name;
            str += ";" + typeof(BusinessSegment).Name;
            str += ";" + typeof(ContourType).Name;
            str += ";" + typeof(ContragentKind).Name;
            str += ";" + typeof(DealType).Name;
            str += ";" + typeof(DepreciationGroup).Name;
            str += ";" + typeof(InformationSource).Name;
            str += ";" + typeof(IntangibleAssetRightType).Name;
            str += ";" + typeof(LayingType).Name;
            str += ";" + typeof(SibLocation).Name;
            str += ";" + typeof(NonCoreAssetListItemState).Name;
            str += ";" + typeof(AddonOKOF).Name;
            str += ";" + typeof(OKOFS).Name;
            str += ";" + typeof(OKOGU).Name;
            str += ";" + typeof(OKOPF).Name;
            str += ";" + typeof(ProductionBlock).Name;
            str += ";" + typeof(RealEstatePurpose).Name;
            str += ";" + typeof(RequestStatus).Name;
            str += ";" + typeof(ResponseStatus).Name;
            str += ";" + typeof(SibBank).Name;
            str += ";" + typeof(SibKBK).Name;
            str += ";" + typeof(SignType).Name;
            str += ";" + typeof(SocietyCategory1).Name;
            str += ";" + typeof(SocietyCategory2).Name;
            str += ";" + typeof(SourceInformationType).Name;
            str += ";" + typeof(StatusConstruction).Name;
            str += ";" + typeof(TaxExemption).Name;
            str += ";" + typeof(TaxNumberInSheet).Name;
            str += ";" + typeof(TypeData).Name;
            str += ";" + typeof(UnitOfCompany).Name;
            str += ";" + typeof(VehicleKindCode).Name;
            str += ";" + typeof(WellCategory).Name;
            str += ";" + typeof(SibProjectStatus).Name;
            str += ";" + typeof(SibTaskReportStatus).Name;
            str += ";" + typeof(SibTaskStatus).Name;
            str += ";" + typeof(SubjectActivityKind).Name;
            str += ";" + typeof(SubjectType).Name;
            str += ";" + typeof(HolidaysCalendar).Name;
            
            //Централизованные
            str += ";" + typeof(OPF).Name;
            str += ";" + typeof(SibCountry).Name;
            str += ";" + typeof(SibFederalDistrict).Name;
            str += ";" + typeof(SibRegion).Name;
            str += ";" + typeof(SubjectType).Name;
            str += ";" + typeof(ClassFixedAsset).Name;
            str += ";" + typeof(VehicleType).Name;
            str += ";" + typeof(OKATO).Name;
            str += ";" + typeof(OKTMO).Name;
            str += ";" + typeof(OKOF94).Name;
            str += ";" + typeof(OKOF2014).Name;
            str += ";" + typeof(RSBU).Name;
            str += ";" + typeof(SibMeasure).Name;
            str += ";" + typeof(GroundCategory).Name;
            str += ";" + typeof(SibDealStatus).Name;
            str += ";" + typeof(DocKind).Name;
            str += ";" + typeof(DocTypeOperation).Name;
            str += ";" + typeof(Currency).Name;
            str += ";" + typeof(Consolidation).Name;

            str += ";" + typeof(Society).Name;
            str += ";" + typeof(SibDeal).Name;
            str += ";" + typeof(Shareholder).Name;
            str += ";" + typeof(AccountingObject).Name;
            str += ";" + typeof(TenureType).Name;
            #endregion

            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => !str.Contains(f.Mnemonic)))
                             );

        }


        /// <summary>
        /// Конфигурация карточки истории импорта данных Росреестра.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> DetailView_ImportRosReestr(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                    .Add(ed => ed.ImportDateTime, ac => ac.Title("Дата/Время").IsReadOnly(true).Order(1))
                    .Add(ed => ed.Mnemonic, ac => ac.Title("Тип объекта").IsReadOnly(true).Order(2))
                    .Add(ed => ed.SibUser, ac => ac.Title("Пользователь").Order(3).IsReadOnly(true).Visible(true))
                    .Add(ed => ed.FileCard, ac => ac.Title("Файл").IsReadOnly(true).Order(4))
                    .Add(ed => ed.DataVersion, ac => ac.Title("Версия данных").IsReadOnly(true).Visible(true).Order(5))
                    .Add(ed => ed.ResultText, ac => ac.Title("Результат").IsReadOnly(true).Order(6))

                    .AddOneToManyAssociation<ImportErrorLog>("ImportHelper_RosReestrLogs",
                        y => y.TabName("Журнал ошибок")
                            .IsReadOnly(true)
                            .TabName("Журнал ошибок")
                            .Mnemonic("RosReestrLogs")
                            .Create((uofw, entity, id) =>
                            {
                                entity.ImportHistory = uofw.GetRepository<ImportHistory>().Find(id);
                            })
                            .Create((uofw, entity, id) =>
                            {
                                entity.ImportHistory = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ImportHistoryID == id))
                        )
              ));
        }

        /// <summary>
        /// Конфигурация реестра истории импорта данных Росреестра.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportRosReestr(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = "Extract;ExtractSubj;ExtractBuild;ExtractBuild;ExtractLand";
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => str.Contains(f.Mnemonic)))
                             );
        }


        public static ViewModelConfigBuilder<ImportHistory> DetailView_ImportDataTransfer(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            return
                conf.DetailView_Default()
                    .Title("Журнал передачи данных")
                    .DetailView(dv => dv
                        .Editors(e => e
                            .Add(a => a.IsCorrection, h => h.Title("Корректировка"))
                            .Add(a => a.Period, h => h.Title("Период"))
                            .Add(a => a.Version, h => h.Title("Версия"))
                            .Add(a => a.DataVersion, h => h.Title("Версия данных"))
                        )
                    );
        }


        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportDataTransfer(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            return
                conf.ListView_Default()
                    .ListView(l => l
                        .Title("Журнал передачи данных")
                        .Columns(c => c
                            .Clear()
                            .Add(a => a.ImportDateTime, h => h.Title("Дата/Время"))
                            .Add(a => a.Period, h => h.Title("Период"))
                            .Add(a => a.SibUser, h => h.Title("Пользователь"))
                            .Add(a => a.FileName, h => h.Title("Имя файла"))
                            .Add(a => a.DataVersion, h => h.Title("Версия данных"))
                            .Add(a => a.IsCorrection, h => h.Title("Корректировка"))
                            .Add(a => a.Version, h => h.Title("Версия"))
                            )
                        .DataSource(ds => ds.Filter(f =>
                            f.IsSuccess
                            && f.Period != null
                            && !f.Hidden
                            )));
        }


        public static ViewModelConfigBuilder<ImportHistory> ListView_ImportArhive(this ViewModelConfigBuilder<ImportHistory> conf)
        {
            string str = typeof(AccountingObject).Name;
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Title(conf.Config.Title)
                             .DataSource(ds => ds.Filter(f => f.ImportHistoryState != null && f.ImportHistoryState.Code == "Arhive"))
                             );

        }


    }
}
