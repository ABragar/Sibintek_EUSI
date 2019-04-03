using Base;
using Base.UI;
using Base.UI.Presets;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Mapping;
using CorpProp.Entities.NSI;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Subject;
using CorpProp.Model.NSI;
using CorpProp.Services.Base;
using CorpProp.Services.FIAS;
using CorpProp.Services.Law;
using CorpProp.Services.NSI;
using System.Linq;
using SubjectObject = CorpProp.Entities.Subject;

namespace CorpProp.Model
{
    public static class NSIModel
    {
        /// <summary>
        /// Инициализация моделей справочников.
        /// </summary>
        /// <param name="context"></param>
		public static void Init(IInitializerContext context)
        {

            //конфиг для отображения элементов меню НСИ в отдельном ListView
            context.CreateVmConfig<MenuElement>("NSIMenu")
                .Title("НСИ")
                .ListView(x => x.Title("НСИ")
                .Type(Base.UI.ViewModal.ListViewType.Custom))
                .DetailView(x => x.Title("НСИ"));

            context.CreateVmConfig<CorpProp.Entities.NSI.NSI>()
               .Title("НСИ")
               .ListView(x => x
               .Title("НСИ")
               .DataSource(ds => ds.PageSize(200).Sort(sr => sr.Add(s => s.Name)).Groups(gr => gr.Groupable(true).Add(g => g.NSIType)))
               .OnClientEditRow(
                   @"var dataItem = grid.dataItem(grid.select());
                     if (dataItem) {
                        if (dataItem.Mnemonic) {
                            location.href = '/Entities/' + dataItem.Mnemonic;
                        }
                        if (dataItem.URL) {
                            location.href = dataItem.URL;
                        }
                        return;
                    }"
               ))
               .DetailView(x => x.Title("НСИ"))
               .IsReadOnly(true);

            context.CreateVmConfig<CorpProp.Entities.NSI.NSI>("NSIAdmin")
              .Title("НСИ")
              .ListView(x => x.Title("НСИ"))
              .DetailView(x => x.Title("НСИ"))
             ;

            DicObjectModel.CreateModelConfig(context);

            InitHistory(context);

            #region NSI 

            context.CreateVmConfigOnBase<DictObject, NSIType>()
               .Service<IDictObjectService<NSIType>>()
               .Title("Тип справочника")
               .ListView(x => x.Title("Типы справочников"))
               .DetailView(x => x.Title("Тип справочника"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ScheduleStateYear>()
               .Service<IScheduleStateYearService>()
               .Title("ГГР на год")
               .ListView(x => x.Title("ГГР на год")
               .Columns(cols => cols
                .Add(c => c.Code, ac => ac.Visible(false))
                .Add(c => c.DateFrom, ac => ac.Visible(true))
                .Add(c => c.DateTo, ac => ac.Visible(true))
                ))
               .DetailView(x => x.Title("ГГР на год").Editors(e => e
                         .AddOneToManyAssociation<ScheduleStateRegistration>("ScheduleStateYear_ScheduleStateRegistration", edt => edt
                            .Title("Графики регистрации прав")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .TabName("Графики регистрации прав")
                            .Create((uofw, entity, id) =>
                            {
                                entity.ScheduleStateYear = uofw.GetRepository<ScheduleStateYear>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.ScheduleStateYear = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ScheduleStateYearID == id && !w.Hidden))
                        )
                        .AddOneToManyAssociation<ScheduleStateTerminate>("ScheduleStateYear_ScheduleStateTerminate", edt => edt
                            .Title("Графики прекращения прав")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .TabName("Графики прекращения прав")
                            .Create((uofw, entity, id) =>
                            {
                                entity.ScheduleStateYear = uofw.GetRepository<ScheduleStateYear>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.ScheduleStateYear = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ScheduleStateYearID == id && !w.Hidden))
                        )
                        .AddManyToManyRigthAssociation<FileCardAndScheduleStateYear>("FileCardAndScheduleStateYear", edt => edt.TabName("Документы"))
                    )
                )
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ResponseRowState>()
               .Service<IDictObjectService<ResponseRowState>>()
               .Title("Статусы строк ответа")
               .ListView(x => x.Title("Статусы строк ответа"))
               .DetailView(x => x.Title("Статус строки ответа"))
               .LookupProperty(x => x.Text(t => t.Name));

            ///
            context.CreateVmConfigOnBase<DictObject, SibDealStatus>()
               .Service<IDictObjectService<SibDealStatus>>()
               .Title("Статус договора")
               .ListView(x => x.Title("Статус договора"))
               .DetailView(x => x.Title("Статус договора"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DocKind>()
              .Service<IDictObjectService<DocKind>>()
              .Title("Вид документа")
              .ListView(x => x.Title("Вид документа"))
              .DetailView(x => x.Title("Вид документа"))
              .LookupProperty(x => x.Text(t => t.Name))
              .IsReadOnly(true);

            context.CreateVmConfigOnBase<DictObject, DocStatus>()
                .Service<IDictObjectService<DocStatus>>()
                .Title("Статус документа")
                .ListView(x => x.Title("Статус документа"))
                .DetailView(x => x.Title("Статус документа"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DocType>()
                .Service<IDictObjectService<DocType>>()
                .Title("Тип документа")
                .ListView(x => x.Title("Тип документа"))
                .DetailView(x => x.Title("Тип документа"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, FileCardType>()
                .Service<IDictObjectService<FileCardType>>()
                .Title("Тип документа (FileCard)")
                .ListView(x => x.Title("Тип документа (FileCard)"))
                .DetailView(x => x.Title("Тип документа (FileCard)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DocTypeOperation>()
                .Service<IDictObjectService<DocTypeOperation>>()
               .Title("Вид операции")
               .ListView(x => x.Title("Вид операций"))
               .DetailView(x => x.Title("Вид операции"))
               .LookupProperty(x => x.Text(t => t.Name));


            /////
            context.CreateVmConfigOnBase<DictObject, AircraftKind>()
             .Service<IDictObjectService<AircraftKind>>()
             .Title("Вид воздушного судна")
             .ListView(x => x.Title("Виды воздушных судов"))
             .DetailView(x => x.Title("Вид воздушного судна"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AircraftType>()
             .Service<IDictObjectService<AircraftType>>()
             .Title("Тип воздушного судна")
             .ListView(x => x.Title("Типы воздушных судов"))
             .DetailView(x => x.Title("Тип воздушного судна"))
             .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, ShipClass>()
             .Service<IDictObjectService<ShipClass>>()
             .Title("Класс судна")
             .ListView(x => x.Title("Классы судов"))
             .DetailView(x => x.Title("Класс судна"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ShipKind>()
             .Service<IDictObjectService<ShipKind>>()
             .Title("Вид судна")
             .ListView(x => x.Title("Виды судов"))
             .DetailView(x => x.Title("Вид судна"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ShipType>()
             .Service<IDictObjectService<ShipType>>()
             .Title("Тип судна")
             .ListView(x => x.Title("Типы судов"))
             .DetailView(x => x.Title("Тип судна"))
             .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, VehicleCategory>()
             .Service<IDictObjectService<VehicleCategory>>()
             .Title("Категория ТС")
             .ListView(x => x.Title("Категории ТС"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Категория ТС")
             .Editors(e => e.Add(p => p.VehicleLabel, o => o.Visible(true))));

            context.CreateVmConfigOnBase<DictObject, TenureType>()
             .Service<IDictObjectService<TenureType>>()
             .Title("Тип ТС")
             .ListView(x => x.Title("Типы ТС"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Тип ТС"));

            context.CreateVmConfigOnBase<DictObject, TypeAccounting>()
            .Service<IDictObjectService<TypeAccounting>>()
            .Title("Ведение БУ")
            .ListView(x => x.Title("Вдение БУ"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("Ведение БУ"));

            context.CreateVmConfigOnBase<DictObject, VehicleType>()
             .Service<IDictObjectService<VehicleType>>()
            .Title("Вид ТС")
            .ListView(x => x.Title("Вид ТС"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("Вид ТС").Editors(eds => eds.Add(ed => ed.DictObjectState, ac => ac.Visible(false)).Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))))
            .IsReadOnly(true);

            context.CreateVmConfigOnBase<DictObject, RegistrationBasis>()
               .Service<IDictObjectService<RegistrationBasis>>()
              .Title("Основание регистрации прав")
              .ListView(x => x.Title("Основание регистрации прав"))
              .DetailView(x => x.Title("Основание регистрации прав"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, ScheduleStateRegistrationStatus>()
                .Service<IDictObjectService<ScheduleStateRegistrationStatus>>()
               .Title("Статус графика гос. регистрации")
               .ListView(x => x.Title("Статусы графиков гос. регистрации"))
               .DetailView(x => x.Title("Статус графика гос. регистрации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, RightKind>()
                .Service<IDictObjectService<RightKind>>()
                .Title("Вид права")
                .ListView(x => x.Title("Виды права"))
                .DetailView(x => x.Title("Вид права"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, RightType>()
                .Service<IDictObjectService<RightType>>()
                .Title("Тип права")
                .ListView(x => x.Title("Типы прав"))
                .DetailView(x => x.Title("Тип права"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ExtractType>()
               .Service<IDictObjectService<ExtractType>>()
               .Title("Тип выписки ЕГРН")
               .ListView(x => x.Title("Тип выписки ЕГРН"))
               .DetailView(x => x.Title("Тип выписки ЕГРН"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ExtractFormat>()
              .Service<IDictObjectService<ExtractFormat>>()
              .Title("Формат выписки")
              .ListView(x => x.Title("Форматы выписки"))
              .DetailView(x => x.Title("Формат выписка"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, EncumbranceType>()
                .Service<IDictObjectService<EncumbranceType>>()
                .Title("Вид ограничения (обременения)")
                .ListView(x => x.Title("Вид ограничения (обременения)"))
                .DetailView(x => x.Title("Вид ограничения (обременения)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AppraisalType>()
              .Service<IDictObjectService<AppraisalType>>()
              .Title("Тип оценки")
              .ListView(x => x.Title("Типы оценок"))
              .DetailView(x => x.Title("Тип оценки"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, InvestmentType>()
              .Service<IDictObjectService<InvestmentType>>()
              .Title("Тип акции / доли")
              .ListView(x => x.Title("Типы акций / долей"))
              .DetailView(x => x.Title("Тип акции / доли"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SuccessionType>()
              .Service<IDictObjectService<SuccessionType>>()
              .Title("Тип правопреемства")
              .ListView(x => x.Title("Типы правопреемства"))
              .DetailView(x => x.Title("Тип правопреемства"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, AccountingMovingType>()
               .Service<IDictObjectService<AccountingMovingType>>()
               .Title("Тип изменения ОБУ")
               .ListView(x => x.Title("Тип изменения ОБУ"))
               .DetailView(x => x.Title("Тип изменения ОБУ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AccountingStatus>()
             .Service<IDictObjectService<AccountingStatus>>()
             .Title("Статус ОБУ")
             .ListView(x => x.Title("Статусы ОБУ"))
             .DetailView(x => x.Title("Статус ОБУ"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AccountingSystem>()
            .Service<IDictObjectService<AccountingSystem>>()
            .Title("Система учёта")
            .ListView(x => x.Title("Системы учёта"))
            .DetailView(x => x.Title("Система учёта"))
            .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ActualKindActivity>()
              .Service<IDictObjectService<ActualKindActivity>>()
              .Title("Фактический вид деятельности")
              .ListView(x => x.Title("Фактические виды деятельности"))
              .DetailView(x => x.Title("Фактический вид деятельности"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AddonAttributeGroundCategory>()
              .Service<IDictObjectService<AddonAttributeGroundCategory>>()
              .Title("Дополнительный признак категории земель")
              .ListView(x => x.Title("Дополнительные признаки категории земель"))
              .DetailView(x => x.Title("Дополнительный признак категории земель"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AppraisalGoal>()
                .Service<IDictObjectService<AppraisalGoal>>()
               .Title("Цель оценки")
               .ListView(x => x.Title("Цели оценок"))
               .DetailView(x => x.Title("Цель оценки"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AppraisalPurpose>()
               .Service<IDictObjectService<AppraisalPurpose>>()
               .Title("Назначение оценки")
               .ListView(x => x.Title("Назначения оценок"))
               .DetailView(x => x.Title("Назначение оценки"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AppType>()
              .Service<IDictObjectService<AppType>>()
              .Title("Тип оценки")
              .ListView(x => x.Title("Типы оценок"))
              .DetailView(x => x.Title("Тип оценки"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BaseExclusionFromPerimeter>()
              .Service<IDictObjectService<BaseExclusionFromPerimeter>>()
              .Title("Основание для исключения объекта из Периметра")
              .ListView(x => x.Title("Основание для исключения объекта из Периметра"))
              .DetailView(x => x.Title("Основание для исключения объекта из Периметра"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BaseInclusionInPerimeter>()
             .Service<IDictObjectService<BaseInclusionInPerimeter>>()
             .Title("Основание для включения объекта в Периметр")
             .ListView(x => x.Title("Основание для включения объекта в Периметр"))
             .DetailView(x => x.Title("Основание для включения объекта в Периметр"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BasisForInvestments>()
               .Service<IDictObjectService<BasisForInvestments>>()
               .Title("Основание акционирования")
               .ListView(x => x.Title("Основания акционирования"))
               .DetailView(x => x.Title("Основание акционирования"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BusinessBlock>()
               .Service<IDictObjectService<BusinessBlock>>()
               .Title("Бизнес-блок")
               .ListView(x => x.Title("Бизнес-блоки"))
               .DetailView(x => x.Title("Бизнес-блок"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BusinessArea>()
               .Service<IDictObjectService<BusinessArea>>()
               .Title("Бизнес-сфера")
               .ListView(x => x.Title("Бизнес-сферы"))
               .DetailView(x => x.Title("Бизнес-сфера"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BusinessDirection>()
               .Service<IDictObjectService<BusinessDirection>>()
               .Title("Бизнес-направление")
               .ListView(x => x.Title("Бизнес-направления"))
               .DetailView(x => x.Title("Бизнес-направление"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, BusinessSegment>()
             .Service<IDictObjectService<BusinessSegment>>()
             .Title("Бизнес-сегмент")
             .ListView(x => x.Title("Бизнес-сегменты"))
             .DetailView(x => x.Title("Бизнес-сегмент"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BusinessUnit>()
             .Service<IDictObjectService<BusinessUnit>>()
             .Title("Бизнес-единица (Аналитика 97 ИКСО)")
             .ListView(x => x.Title("Бизнес-единицы (Аналитика 97 ИКСО)"))
             .DetailView(x => x.Title("Бизнес-единица (Аналитика 97 ИКСО)"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, BIK>()
            .Title("БИК")
            .ListView(x => x.Title("БИК"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("БИК"));

            context.CreateVmConfigOnBase<DictObject, ClassFixedAsset>()
             .Service<IDictObjectService<ClassFixedAsset>>()
              .Title("Класс (БУ)")
              .ListView(x => x.Title("Класс (БУ)"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Класс (БУ)"));

            context.CreateVmConfigOnBase<DictObject, Consolidation>()
             .Service<IDictObjectService<Consolidation>>()
                .Title("Единица консолидации (БЕ)")
                .ListView(x => x.Title("ЕК(БЕ)"))
                .DetailView(x => x.Title("Единица консолидации (БЕ)")
                           .Editors(e => e
                      .Add(a => a.DictObjectState, a => a.IsReadOnly(true))
                      .Add(a => a.DictObjectStatus, a => a.IsReadOnly(false).Visible(false))
                      .Add(a => a.DateFrom, a => a.IsReadOnly(false))
                      .Add(a => a.DateTo, a => a.IsReadOnly(false))
                      .Add(a => a.Code, a => a.IsReadOnly(true))
                      .Add(a => a.Name, a => a.IsReadOnly(true))))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<DictObject, ContourType>()
                .Service<IDictObjectService<ContourType>>()
                .Title("Тип контура")
                .ListView(x => x.Title("Типы контура"))
                .DetailView(x => x.Title("Тип контура"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ContragentKind>()
                .Service<IDictObjectService<ContragentKind>>()
                .Title("Вид контрагента")
                .ListView(x => x.Title("Виды контрагентов"))
                .DetailView(x => x.Title("Вид контрагента"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, Currency>()
               .Service<IDictObjectService<Currency>>()
               .Title("Валюта")
               .ListView(x => x.Title("Валюта"))
               .DetailView(x => x.Title("Валюта"))
               .LookupProperty(x => x.Text(t => t.Code))
               .IsReadOnly(true);

            context.CreateVmConfigOnBase<DictObject, DealType>()
              .Service<IDictObjectService<DealType>>()
              .Title("Вид договора")
              .ListView(x => x.Title("Вид договора"))
              .DetailView(x => x.Title("Вид договора"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DepreciationGroup>()
              .Service<IDictObjectService<DepreciationGroup>>()
              .Title("Амортизационная группа")
              .ListView(x => x.Title("Амортизационные группы"))
              .DetailView(x => x.Title("Амортизационная группа"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, EstateAppraisalType>()
             .Service<IDictObjectService<EstateAppraisalType>>()
              .Title("Тип объекта оценки")
              .ListView(x => x.Title("Типы объектов оценок"))
              .DetailView(x => x.Title("Тип объекта оценки"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, ExternalMappingSystem>()
               .Title("Внешняя система сопоставления")
               .ListView(x => x.Title("Внешние системы сопоставления"))
               .DetailView(x => x.Title("Внешняя система сопоставления"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, FeatureType>()
                .Service<IDictObjectService<FeatureType>>()
                .Title("Тип основной характеристики объекта недвижимости")
                .ListView(x => x.Title("Тип основной характеристики объекта недвижимости"))
                .DetailView(x => x.Title("Тип основной характеристики объекта недвижимости"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, GroundCategory>()
                .Service<IDictObjectService<GroundCategory>>()
               .Title("Код категории земель")
               .ListView(x => x.Title("Код категории земель"))
               .DetailView(x => x.Title("Код категории земель"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, InformationSource>()
                .Service<IDictObjectService<InformationSource>>()
                .Title("Источник информации")
                .ListView(x => x.Title("Источники информации"))
                .DetailView(x => x.Title("Источник информации"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, IntangibleAssetRightType>()
                .Service<IDictObjectService<IntangibleAssetRightType>>()
                .Title("Тип права на НМА")
                .ListView(x => x.Title("Типы прав на НМА"))
                .DetailView(x => x.Title("Тип права НМА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, IntangibleAssetStatus>()
                .Service<IDictObjectService<IntangibleAssetStatus>>()
               .Title("Статус НМА")
               .ListView(x => x.Title("Статусы НМА"))
               .DetailView(x => x.Title("Статус НМА"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, IntangibleAssetType>()
                .Service<IDictObjectService<IntangibleAssetType>>()
                .Title("Вид НМА")
                .ListView(x => x.Title("Вид НМА"))
                .DetailView(x => x.Title("Вид НМА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ImplementationWay>()
              .Service<IDictObjectService<ImplementationWay>>()
                .Title("Способ реализации ННА")
                .ListView(x => x.Title("Способы реализации ННА"))
                .DetailView(x => x.Title("Способ реализации ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ImportHistoryState>()
              .Service<IDictObjectService<ImportHistoryState>>()
                .Title("Статус истории импорта")
                .ListView(x => x.Title("Статусы истории импорта"))
                .DetailView(x => x.Title("Статус истории импорта"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, EstateType>()
             .Service<IDictObjectService<EstateType>>()
                .Title("Класс КС")
                .ListView(x => x.Title("Классы КС"))
                .DetailView(x => x.Title("Класс КС"))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<DictObject, LandType>()
               .Service<IDictObjectService<LandType>>()
              .Title("Тип ЗУ")
              .ListView(x => x.Title("Тип ЗУ"))
              .DetailView(x => x.Title("Тип ЗУ"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, LayingType>()
               .Service<IDictObjectService<LayingType>>()
              .Title("Тип прокладки линейных сооружений")
              .ListView(x => x.Title("Типы прокладки линейных сооружений"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Тип прокладки линейных сооружений"));

            context.CreateVmConfigOnBase<DictObject, LeavingReason>()
              .Service<IDictObjectService<LeavingReason>>()
              .Title("Причины выбытия")
              .ListView(x => x.Title("Причины выбытия"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Причина выбытия"));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetAppraisalType>()
             .Service<IDictObjectService<NonCoreAssetAppraisalType>>()
                .Title("Тип оценки ННА")
                .ListView(x => x.Title("Типы оценок ННА"))
                .DetailView(x => x.Title("Тип оценки ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetListItemState>()
           .Service<IDictObjectService<NonCoreAssetListItemState>>()
         .Title("Статус строки перечня ННА")
         .ListView(x => x.Title("Статусы строки перечня ННА"))
         .DetailView(x => x.Title("Статус строки перечня ННА"))
         .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetListItemState>("NNAItemStates")
                .Service<NNAItemStatesService>()
                .Title("Статус строки перечня ННА")
                .ListView(x => x.Title("Статусы строки перечня ННА"))
                .DetailView(x => x.Title("Статус строки перечня ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetListKind>()
             .Service<IDictObjectService<NonCoreAssetListKind>>()
            .Title("Вид перечня ННА")
            .ListView(x => x.Title("Виды перечней ННА"))
            .DetailView(x => x.Title("Вид перечня ННА"))
            .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetListType>()
             .Service<IDictObjectService<NonCoreAssetListType>>()
           .Title("Тип перечня ННА")
           .ListView(x => x.Title("Типы перечня ННА"))
           .DetailView(x => x.Title("Тип перечня ННА"))
           .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetSaleAcceptType>()
           .Service<IDictObjectService<NonCoreAssetSaleAcceptType>>()
           .Title("Вид одобрения реализации ННА")
           .ListView(x => x.Title("Виды одобрений реализаций ННА"))
           .DetailView(x => x.Title("Вид одобрения реализации ННА"))
           .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetType>()
           .Service<IDictObjectService<NonCoreAssetType>>()
           .Title("Тип ННА")
           .ListView(x => x.Title("Типы ННА"))
           .DetailView(x => x.Title("Тип ННА"))
           .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetStatus>()
             .Service<IDictObjectService<NonCoreAssetStatus>>()
             .Title("Статус ННА")
             .ListView(x => x.Title("Статусы ННА"))
             .DetailView(x => x.Title("Статус ННА"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetInventoryType>()
              .Service<IDictObjectService<NonCoreAssetInventoryType>>()
              .Title("Виды реестров ННА")
              .ListView(x => x.Title("Вид реестра ННА"))
              .DetailView(x => x.Title("Вид реестра ННА"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetOwnerCategory>()
                .Service<IDictObjectService<NonCoreAssetOwnerCategory>>()
                .Title("Категории балансодержаталей")
                .ListView(x => x.Title("Категория балансодержателя"))
                .DetailView(x => x.Title("Категория балансодержателя"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetListState>()
                .Service<IDictObjectService<NonCoreAssetListState>>()
                .Title("Статусы перечня ННА")
                .ListView(x => x.Title("Статус перечня ННА"))
                .DetailView(x => x.Title("Статус перечня ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetSaleStatus>()
                .Service<IDictObjectService<NonCoreAssetSaleStatus>>()
                .Title("Статусы реализации ННА")
                .ListView(x => x.Title("Статус реализации ННА"))
                .DetailView(x => x.Title("Статус реализации ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, NonCoreAssetInventoryState>()
                .Service<IDictObjectService<NonCoreAssetInventoryState>>()
                .Title("Статусы реестра ННА")
                .ListView(x => x.Title("Статус реестра ННА"))
                .DetailView(x => x.Title("Статус реестра ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SibLocation>("SibLocation")
              .Service<IDictObjectService<SibLocation>>()
              .Title("Местоположения")
              .ListView(x => x.Title("Местоположения"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Местоположение"));

            context.CreateVmConfigOnBase<DictObject, OKATO>()
             .Service<IDictObjectService<OKATO>>()
                .Title("Код ОКАТО")
                .ListView(x => x.Title("Код ОКАТО"))
                .DetailView(x => x.Title("Код ОКАТО"))
                .LookupProperty(x => x.Text(t => t.Title));


            context.CreateVmConfigOnBase<DictObject, OKOF94>()
                .Service<IDictObjectService<OKOF94>>()
                .Title("ОКОФ (версия 1994г.)")
                .ListView(x => x.Title("ОКОФ")
                    .Columns(cols => cols
                        .Add(col => col.AdditionalCode, ac => ac.Order(2))
                        .Add(col => col.Name, ac => ac.Order(3))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DateFrom, ac => ac.Order(6))
                        .Add(col => col.DateFrom, ac => ac.Order(7))
                    ))
                .DetailView(x => x.Title("ОКОФ")
                    .Editors(eds => eds
                        .Add(ed => ed.AdditionalCode, ac => ac.Order(2))
                        .Add(ed => ed.Name, ac => ac.Order(3))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(true).Order(4))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(true).Order(5))
                        .Add(ed => ed.DateFrom, ac => ac.Order(6))
                        .Add(ed => ed.DateFrom, ac => ac.Order(7))
                        .AddOneToManyAssociation<AddonOKOF>("AddonOKOF_OKOF94", edt => edt
                            .Title("Доп. коды. ОКОФ")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .TabName("Доп. коды. ОКОФ")
                            .Create((uofw, entity, id) =>
                            {
                                entity.OKOF94ID = id;
                                entity.OKOF94 = uofw.GetRepository<OKOF94>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.OKOF94 = null;
                                entity.OKOF94ID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.OKOF94ID == id && !w.Hidden))
                        )
                    ))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<DictObject, OKOF2014>()
                .Service<IDictObjectService<OKOF2014>>()
                .Title("ОКОФ (версия 2014г.)")
                .ListView(x => x.Title("ОКОФ 2")
                    .Columns(cols => cols
                        .Add(col => col.AdditionalCode, ac => ac.Order(2))
                        .Add(col => col.Name, ac => ac.Order(3))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DateFrom, ac => ac.Order(6))
                        .Add(col => col.DateFrom, ac => ac.Order(7))
                    ))
                .DetailView(x => x.Title("ОКОФ 2")
                    .Editors(eds => eds
                        .Add(ed => ed.AdditionalCode, ac => ac.Order(2))
                        .Add(ed => ed.Name, ac => ac.Order(3))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(true).Order(4))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(true).Order(5))
                        .Add(ed => ed.DateFrom, ac => ac.Order(6))
                        .Add(ed => ed.DateFrom, ac => ac.Order(7))
                    ))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<DictObject, AddonOKOF>()
                 .Service<IDictObjectService<AddonOKOF>>()
               .Title("Доп. коды ОКОФ")
               .ListView(x => x.Title("Доп. коды ОКОФ"))
               .DetailView(x => x.Title("Доп. код ОКОФ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, OKOFS>()
             .Service<IDictObjectService<OKOFS>>()
                .Title("ОКОФС")
                .ListView(x => x.Title("ОКОФС"))
                .DetailView(x => x.Title("ОКОФС"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, OKOGU>()
             .Service<IDictObjectService<OKOGU>>()
                .Title("ОКОГУ")
                .ListView(x => x.Title("ОКОГУ"))
                .DetailView(x => x.Title("ОКОГУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, OKOPF>()
             .Service<IDictObjectService<OKOPF>>()
                .Title("ОКОПФ")
                .ListView(x => x.Title("ОКОПФ"))
                .DetailView(x => x.Title("ОКОПФ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, OKPO>()
             .Service<IDictObjectService<OKPO>>()
                .Title("ОКПО")
                .ListView(x => x.Title("ОКПО"))
                .DetailView(x => x.Title("ОКПО"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, OKTMO>()
                .Service<IDictObjectService<OKTMO>>()
               .Title("Код ОКТМО")
               .ListView(x => x.Title("Код ОКТМО"))
               .DetailView(x => x.Title("Код ОКТМО"))
               .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfig<SibOKVED>()
                .Service<ISibOKVEDHierarhyService>()
                .Title("ОКВЭД")
                .ListView(x => x.Title("ОКВЭД"))
                .DetailView(x => x.Title("ОКВЭД"))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<DictObject, OPF>()
              .Service<IDictObjectService<OPF>>()
              .Title("ОПФ")
              .ListView(x => x.Title("ОПФ"))
              .DetailView(x => x.Title("ОПФ"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, RSBU>()
              .Service<IDictObjectService<RSBU>>()
              .Title("РСБУ")
              .ListView(x => x.Title("РСБУ"))
              .DetailView(x => x.Title("РСБУ"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, OwnershipType>()
              .Service<IDictObjectService<OwnershipType>>()
             .Title("Форма собственности")
             .ListView(x => x.Title("Формы собственности"))
             .DetailView(x => x.Title("Форма собственности"))
             .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, ProductionBlock>()
              .Service<IDictObjectService<ProductionBlock>>()
                .Title("Производственный блок")
                .ListView(x => x.Title("Производственные блоки"))
                .DetailView(x => x.Title("Производственный блок"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, PropertyComplexIOType>()
                .Service<IDictObjectService<PropertyComplexIOType>>()
                .Title("Тип ИК (ОИ)")
                .ListView(x => x.Title("Тип ИК (ОИ)"))
                .DetailView(x => x.Title("Тип ИК (ОИ)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SibProjectStatus>()
               .Service<IDictObjectService<SibProjectStatus>>()
               .Title("Статус проекта")
               .ListView(x => x.Title("Статусы проекта"))
               .DetailView(x => x.Title("Статус проект"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, RealEstateKind>()
                .Service<IDictObjectService<RealEstateKind>>()
                .Title("Вид объекта недвижимости")
                .ListView(x => x.Title("Вид объекта недвижимости"))
                .DetailView(x => x.Title("Вид объекта недвижимости"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, RealEstatePurpose>()
                 .Service<IDictObjectService<RealEstatePurpose>>()
                .Title("Назначение объекта недвижимого имущества")
                .ListView(x => x.Title("Назначения объекта недвижимого имущества"))
                .DetailView(x => x.Title("Назначение объекта недвижимого имущества"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ReceiptReason>()
                 .Service<IDictObjectService<ReceiptReason>>()
               .Title("Способ поступления")
               .ListView(x => x.Title("Способ поступления"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Способ поступления"));

            context.CreateVmConfigOnBase<DictObject, RequestStatus>()
                 .Service<IDictObjectService<RequestStatus>>()
               .Title("Статус запроса")
               .ListView(x => x.Title("Статусы запросов"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Статус запроса"));

            context.CreateVmConfigOnBase<DictObject, ResponseStatus>()
                .Service<IDictObjectService<ResponseStatus>>()
               .Title("Статус ответа")
               .ListView(x => x.Title("Статусы ответов"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Статус ответа"));

            context.CreateVmConfigOnBase<DictObject, RightHolderKind>()
                .Service<IDictObjectService<RightHolderKind>>()
              .Title("Вид основания для правообладания")
              .ListView(x => x.Title("Виды оснований для правообладания"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Вид основания для правообладания"));

            context.CreateVmConfigOnBase<DictObject, ShipAssignment>()
                 .Service<IDictObjectService<ShipAssignment>>()
             .Title("Назначение судна")
             .ListView(x => x.Title("Назначения судов"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Назначение судна"));

            context.CreateVmConfigOnBase<DictObject, SibBank>()
                .Service<IDictObjectService<SibBank>>()
             .Title("Банк")
             .ListView(x => x.Title("Банки"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Банк"));

            context.CreateVmConfigOnBase<DictObject, SibFederalDistrict>()
                 .Service<ISibFederalDistrictService>()
              .Title("Федеральный округ")
              .ListView(x => x.Title("Федеральные округа"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Федеральный округ"));

            context.CreateVmConfigOnBase<DictObject, SibKBK>()
              .Service<IDictObjectService<SibKBK>>()
              .Title("КБК")
              .ListView(x => x.Title("КБК"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("КБК"));

            context.CreateVmConfigOnBase<DictObject, SibMeasure>()
             .Service<IDictObjectService<SibMeasure>>()
             .Title("Единица измерения")
             .ListView(x => x.Title("Единицы измерения"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Единица измерения"))
             .IsReadOnly(true);

            context.CreateVmConfigOnBase<DictObject, SibProjectType>()
             .Service<IDictObjectService<SibProjectType>>()
            .Title("Тип проекта")
            .ListView(x => x.Title("Типы проектов"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("Тип проекта"));

            context.CreateVmConfigOnBase<DictObject, SibTaskReportStatus>()
                .Service<IDictObjectService<SibTaskReportStatus>>()
               .Title("Статус отчета по задаче")
               .ListView(x => x.Title("Статусы отчетов по задачам"))
               .DetailView(x => x.Title("Статус отчета по задаче"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SibTaskStatus>()
                 .Service<IDictObjectService<SibTaskStatus>>()
                .Title("Статус задачи")
                .ListView(x => x.Title("Статусы задачи"))
                .DetailView(x => x.Title("Статус задачи"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SibCountry>()
               .Service<IDictObjectService<SibCountry>>()
               .Title("Страна")
               .ListView(x => x.Title("Страна"))
               .DetailView(x => x.Title("Страна"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SibRegion>()
               .Service<ISibRegionService>()
               .Title("Субъект РФ")
               .ListView(x => x.Title("Субъекты РФ"))
               .DetailView(x => x.Title("Субъект РФ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, SignType>()
             .Service<IDictObjectService<SignType>>()
            .Title("Тип товарного знака")
            .ListView(x => x.Title("Типы товарных знаков"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("Тип товарного знака"));


            context.CreateVmConfigOnBase<DictObject, SocietyCategory1>()
             .Service<IDictObjectService<SocietyCategory1>>()
               .Title("Категория ОГ 1")
               .ListView(x => x.Title("Категории ОГ 1"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Категория ОГ 1"));

            context.CreateVmConfigOnBase<DictObject, SocietyCategory2>()
             .Service<IDictObjectService<SocietyCategory2>>()
               .Title("Категория ОГ 2")
               .ListView(x => x.Title("Категории ОГ 2"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Категория ОГ 2"));



            context.CreateVmConfigOnBase<DictObject, SourceInformationType>()
                .Service<IDictObjectService<SourceInformationType>>()
                .Title("Тип источника информации")
                .ListView(x => x.Title("Типы источников информации"))
                .DetailView(x => x.Title("Тип источника информации"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, StageOfCompletion>()
                 .Service<IDictObjectService<StageOfCompletion>>()
                .Title("Стадия готовности объекта")
                .ListView(x => x.Title("Стадия готовности объекта"))
                .DetailView(x => x.Title("Стадия готовности объекта"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, StatusConstruction>()
                .Service<IDictObjectService<StatusConstruction>>()
                .Title("Статус строительства")
                .ListView(x => x.Title("Статусы строительства"))
                .DetailView(x => x.Title("Статус строительства"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxBase>()
                .Service<IDictObjectService<TaxBase>>()
                .Title("Выбор налоговой базы")
                .ListView(x => x.Title("Выбор налоговой базы"))
                .DetailView(x => x.Title("Выбор налоговой базы"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SubjectObject.SubjectActivityKind>()
               .Service<IDictObjectService<SubjectActivityKind>>()
               .Title("Вид делового партнера")
               .ListView(x => x.Title("Виды деловых партнеров"))
               .DetailView(x => x.Title("Вид делового партнера"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SubjectObject.SubjectKind>()
                .Service<IDictObjectService<SubjectKind>>()
                .Title("Вид делового партнера")
                .ListView(x => x.Title("Виды деловых партнеров"))
                .DetailView(x => x.Title("Вид делового партнера"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SubjectObject.SubjectType>()
                .Service<IDictObjectService<SubjectType>>()
                .Title("Тип делового партнера")
                .ListView(x => x.Title("Тип делового партнера"))
                .DetailView(x => x.Title("Тип делового партнера"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<SocietyDept>()
                .Service<ISocietyDeptHierarhyService>()
                .Title("Структурное подразделение ОГ")
                .ListView(x => x.Title("Структурные подразделения ОГ"))
                .DetailView(x => x.Title("Структурное подразделение ОГ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxNumberInSheet>()
                .Service<IDictObjectService<TaxNumberInSheet>>()
                .Title("Налоговый номер в соотв. налоговой ведомости")
                .ListView(x => x.Title("Налоговые номера в соотв. налоговых ведомостях"))
                .DetailView(x => x.Title("Налоговый номер в соотв. налоговой ведомости"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TypeData>()
                .Service<IDictObjectService<TypeData>>()
                .Title("Тип данных")
                .ListView(x => x.Title("Типы данных"))
                .DetailView(x => x.Title("Тип данных"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, UnitOfCompany>()
                .Service<IDictObjectService<UnitOfCompany>>()
                .Title("Структурное подразделение")
                .ListView(x => x.Title("Структурные подразделения"))
                .DetailView(x => x.Title("Структурное подразделение"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DictObjectStatus>()
             .Service<IDictObjectService<DictObjectStatus>>()
             .Title("Статус элемента справочника")
             .ListView(x => x.Title("Статусы элементов справочника"))
             .DetailView(x => x.Title("Статус элемента справочника"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DictObjectState>()
             .Service<IDictObjectService<DictObjectState>>()
             .Title("Состояние элемента справочника")
             .ListView(x => x.Title("Состояния элементов справочника"))
             .DetailView(x => x.Title("Состояние элемента справочника"))
             .LookupProperty(x => x.Text(t => t.Name));

            //context.CreateVmConfigOnBase<DictObject, HolidaysCalendar>()
            // .Service<IDictObjectService<HolidaysCalendar>>()
            // .Title("Праздничный или выходной день")
            // .ListView(x => x.Title("Праздничные и выходные дни")
            //  .Columns(c => c
            //        .Clear()
            //        .Add(a => a.Name)
            //        .Add(a => a.DateFrom)
            //        .Add(a => a.DateTo)
            //    ))
            // .DetailView(x => x.Title("Праздничный или выходной день"))
            // .LookupProperty(x => x.Text(t => t.Name));
            context.CreateVmConfigOnBase<DictObject, TaxName>()
                .Service<IDictObjectService<TaxName>>()
                .Title("Наименование налога")
                .ListView(x => x.Title("Наименование налога"))
                .DetailView(x => x.Title("Наименование налога"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxReportPeriod>()
                .Service<IDictObjectService<TaxReportPeriod>>()
                .Title("Отчетный период по налогу")
                .ListView(x => x.Title("Отчетный период по налогу"))
                .DetailView(x => x.Title("Отчетный период по налогу"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxPeriod>()
                .Service<IDictObjectService<TaxPeriod>>()
                .Title("Налоговый период")
                .ListView(x => x.Title("Налоговый период"))
                .DetailView(x => x.Title("Налоговый период"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxRate>()
                .Service<IDictObjectService<TaxRate>>()
                .Title("Налоговая ставка Имущество")
                .ListView(x => x.Title("Налоговая ставка Имущество"))
                .DetailView(x => x.Title("Налоговая ставка Имущество"))
                .LookupProperty(x => x.Text(t => t.Value));

            context.CreateVmConfigOnBase<DictObject, TaxRateLand>()
                .Service<IDictObjectService<TaxRateLand>>()
                .Title("Налоговая ставка ЗУ")
                .ListView(x => x.Title("Налоговая ставка ЗУ"))
                .DetailView(x => x.Title("Налоговая ставка ЗУ"))
                .LookupProperty(x => x.Text(t => t.Value));

            context.CreateVmConfigOnBase<DictObject, TaxRateTS>()
                .Service<IDictObjectService<TaxRateTS>>()
                .Title("Налоговая ставка ТС")
                .ListView(x => x.Title("Налоговая ставка ТС"))
                .DetailView(x => x.Title("Налоговая ставка ТС"))
                .LookupProperty(x => x.Text(t => t.Value));

            context.CreateVmConfigOnBase<DictObject, TaxDeductionTS>()
                .Service<IDictObjectService<TaxDeductionTS>>()
                .Title("Налоговые вычеты (ТС)")
                .ListView(x => x.Title("Налоговые вычеты (ТС)"))
                .DetailView(x => x.Title("Налоговые вычеты (ТС)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DecisionsDetails>()
                .Service<IDictObjectService<DecisionsDetails>>()
                .Title("Реквизиты решений органов МО Имущество")
                .ListView(x => x.Title("Реквизиты решений органов МО Имущество"))
                .DetailView(x => x.Title("Реквизиты решений органов МО Имущество"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DecisionsDetailsLand>()
                .Service<IDictObjectService<DecisionsDetailsLand>>()
                .Title("Реквизиты решений органов МО ЗУ")
                .ListView(x => x.Title("Реквизиты решений органов МО ЗУ"))
                .DetailView(x => x.Title("Реквизиты решений органов МО ЗУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DecisionsDetailsTS>()
                .Service<IDictObjectService<DecisionsDetailsTS>>()
                .Title("Реквизиты решений органов МО ТС")
                .ListView(x => x.Title("Реквизиты решений органов МО ТС"))
                .DetailView(x => x.Title("Реквизиты решений органов МО ТС"))
                .LookupProperty(x => x.Text(t => t.Name));



            #endregion

            #region EUSI
            context.CreateVmConfigOnBase<DictObject, Deposit>()
                .Service<IDictObjectService<Deposit>>()
                .ListView(builder => builder.Title("Месторождения"))
                .DetailView(builder => builder.Title("Месторождение"));

            context.CreateVmConfigOnBase<DictObject, WellCategory>()
                .Service<IDictObjectService<WellCategory>>()
                .ListView(builder => builder.Title("Категории скважины"))
                .DetailView(builder => builder.Title("Категории скважины"));



            context.CreateVmConfigOnBase<DictObject, DepreciationMethodRSBU>()
               .Service<IDictObjectService<DepreciationMethodRSBU>>()
               .Title("Метод амортизации РСБУ")
               .ListView(x => x.Title("Методы амортизации РСБУ"))
               .DetailView(x => x.Title("Метод амортизации РСБУ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DepreciationMethodMSFO>()
             .Service<IDictObjectService<DepreciationMethodMSFO>>()
             .Title("Метод амортизации МСФО")
             .ListView(x => x.Title("Методы амортизации МСФО"))
             .DetailView(x => x.Title("Метод амортизации МСФО"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, DepreciationMethodNU>()
             .Service<IDictObjectService<DepreciationMethodNU>>()
             .Title("Метод амортизации НУ")
             .ListView(x => x.Title("Методы амортизации НУ"))
             .DetailView(x => x.Title("Метод амортизации НУ"))
             .LookupProperty(x => x.Text(t => t.Name));



            context.CreateVmConfigOnBase<DictObject, DivisibleType>()
               .Service<IDictObjectService<DivisibleType>>()
               .Title("Отделимое/неотделимое имущество")
               .ListView(x => x.Title("Отделимое/неотделимое имущество"))
               .DetailView(x => x.Title("Отделимое/неотделимое имущество"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, EcoKlass>()
                .Service<IDictObjectService<EcoKlass>>()
                .Title("Экологический класс")
                .ListView(x => x.Title("Экологический класс"))
                .DetailView(x => x.Title("Экологический класс"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, ENAOF>()
               .Service<IDictObjectService<ENAOF>>()
               .Title("ЕНАОФ")
               .ListView(x => x.Title("ЕНАОФ"))
               .DetailView(x => x.Title("ЕНАОФ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, EnergyLabel>()
               .Service<IDictObjectService<EnergyLabel>>()
               .Title("Класс энергетической эффективности")
               .ListView(x => x.Title("Класс энергетической эффективности"))
               .DetailView(x => x.Title("Класс энергетической эффективности"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<HighEnergyEfficientFacility>("DictMenu", nameof(HighEnergyEfficientFacility))
                .Service<IDictHistoryService<HighEnergyEfficientFacility>>()
                .Title("Объекты с признаком высокой энергетической эффективности")
                .ListView(x => x.Title("Объекты с признаком высокой энергетической эффективности"))
                .DetailView(x => x.Title("Объект с признаком высокой энергетической эффективности"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<HighEnergyEfficientFacilityKP>("DictMenu", nameof(HighEnergyEfficientFacilityKP))
                .Service<IDictHistoryService<HighEnergyEfficientFacilityKP>>()
                .Title("Объекты с признаком высокой энергетической эффективности (количественный показатель)")
                .ListView(x => x.Title("Объекты с признаком высокой энергетической эффективности (количественный показатель)"))
                .DetailView(x => x.Title("Объект с признаком высокой энергетической эффективности (количественный показатель)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, EstateDefinitionType>()
               .Service<IDictObjectService<EstateDefinitionType>>()
               .ListView(builder => builder.Title("Типы ОИ"))
               .DetailView(builder => builder.Title("Тип ОИ"));

            context.CreateVmConfigOnBase<DictObject, EstateMovableNSI>()
               .Service<IDictObjectService<EstateMovableNSI>>()
               .Title("Признак движимое/недвижимое имущество")
               .ListView(x => x.Title("Признак движимое/недвижимое имущество"))
               .DetailView(x => x.Title("Признак движимое/недвижимое имущество"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, GroupConsolidationRSBU>()
               .Service<IDictObjectService<GroupConsolidationRSBU>>()
               .Title("Группа консолидации")
               .ListView(x => x.Title("Группа консолидации"))
               .DetailView(x => x.Title("Группа консолидации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, LandPurpose>()
               .Service<IDictObjectService<LandPurpose>>()
               .Title("Назначение ЗУ")
                .ListView(x => x.Title("Назначение ЗУ"))
               .DetailView(x => x.Title("Назначение ЗУ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, LicenseArea>()
                .Service<IDictObjectService<LicenseArea>>()
                .Title("Лицензионный участок")
                .ListView(x => x.Title("Лицензионный участок"))
                .DetailView(x => x.Title("Лицензионный участок"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, PeriodNU>()
               .Service<IDictObjectService<PeriodNU>>()
               .Title("Период НУ")
               .ListView(x => x.Title("Период НУ"))
               .DetailView(x => x.Title("Период НУ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, PermittedUseKind>()
               .Service<IDictObjectService<PermittedUseKind>>()
               .Title("Вид разрешенного использования")
               .ListView(x => x.Title("Вид разрешенного использования"))
               .DetailView(x => x.Title("Вид разрешенного использования"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, PositionConsolidation>()
               .Service<IDictObjectService<PositionConsolidation>>()
               .Title("Позиция консолидации")
               .ListView(x => x.Title("Позиция консолидации"))
               .DetailView(x => x.Title("Позиция консолидации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SubPositionConsolidation>()
               .Service<IDictObjectService<SubPositionConsolidation>>()
               .Title("Подпозиция консолидации")
               .ListView(x => x.Title("Подпозиция консолидации"))
               .DetailView(x => x.Title("Подпозиция консолидации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, RentTypeMSFO>()
               .Service<IDictObjectService<RentTypeMSFO>>()
               .Title("Тип аренды МСФО")
               .ListView(x => x.Title("Тип аренды МСФО"))
               .DetailView(x => x.Title("Тип аренды МСФО"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, RentTypeRSBU>()
               .Service<IDictObjectService<RentTypeRSBU>>()
               .Title("Тип аренды РСБУ")
               .ListView(x => x.Title("Тип аренды РСБУ"))
               .DetailView(x => x.Title("Тип аренды РСБУ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SibCityNSI>()
               .Service<ISibCityNSIService>()
               .Title("Город")
               .ListView(x => x.Title("Город"))
               .DetailView(x => x.Title("Город"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, StateObjectMSFO>()
               .Service<IDictObjectService<StateObjectMSFO>>()
               .Title("Состояние объекта МСФО")
               .ListView(x => x.Title("Состояние объекта МСФО"))
               .DetailView(x => x.Title("Состояние объекта МСФО"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, StateObjectRSBU>()
               .Service<IDictObjectService<StateObjectRSBU>>()
               .Title("Состояние объекта РСБУ")
               .ListView(x => x.Title("Состояние объекта РСБУ"))
               .DetailView(x => x.Title("Состояние объекта РСБУ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, StructurePlan>()
               .Service<IDictObjectService<StructurePlan>>()
               .Title("СПП")
               .ListView(x => x.Title("СПП"))
               .DetailView(x => x.Title("СПП"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxFreeLand>()
              .Service<IDictObjectService<TaxFreeLand>>()
              .Title("Код налоговой льготы в виде освобождения от налогообложения. Земельный налог")
              .ListView(x => x.Title("Код налоговой льготы в виде освобождения от налогообложения. Земельный налог"))
              .DetailView(x => x.Title("Код налоговой льготы в виде освобождения от налогообложения. Земельный налог"))
              .LookupProperty(x => x.Text(t => t.Code));

            context.CreateVmConfigOnBase<DictObject, TaxFreeTS>()
              .Service<IDictObjectService<TaxFreeTS>>()
              .Title("Код налоговой льготы в виде освобождения от налогообложения. Транспортный налог")
              .ListView(x => x.Title("Код налоговой льготы в виде освобождения от налогообложения. Транспортный налог"))
              .DetailView(x => x.Title("Код налоговой льготы в виде освобождения от налогообложения. Транспортный налог"))
              .LookupProperty(x => x.Text(t => t.Code));


            context.CreateVmConfigOnBase<DictObject, TaxLower>()
              .Service<IDictObjectService<TaxLower>>()
              .Title("Код налоговой льготы в виде уменьшения суммы налога. Налог на имущество")
              .ListView(x => x.Title("Код налоговой льготы в виде уменьшения суммы налога. Налог на имущество"))
              .DetailView(x => x.Title("Код налоговой льготы в виде уменьшения суммы налога. Налог на имущество"))
              .LookupProperty(x => x.Text(t => t.Code));


            context.CreateVmConfigOnBase<DictObject, TaxLowerLand>()
              .Service<IDictObjectService<TaxLowerLand>>()
              .Title("Код налоговой льготы в виде уменьшения суммы налога. Земельный налог")
              .ListView(x => x.Title("Код налоговой льготы в виде уменьшения суммы налога. Земельный налог"))
              .DetailView(x => x.Title("Код налоговой льготы в виде уменьшения суммы налога. Земельный налог"))
              .LookupProperty(x => x.Text(t => t.Code));


            context.CreateVmConfigOnBase<DictObject, TaxLowerTS>()
              .Service<IDictObjectService<TaxLowerTS>>()
              .Title("Код налоговой льготы в виде уменьшения суммы налога. Транспортный налог")
              .ListView(x => x.Title("Код налоговой льготы в виде уменьшения суммы налога. Транспортный налог"))
              .DetailView(x => x.Title("Код налоговой льготы в виде уменьшения суммы налога. Транспортный налог"))
              .LookupProperty(x => x.Text(t => t.Code));


            context.CreateVmConfigOnBase<DictObject, TaxRateLower>()
              .Service<IDictObjectService<TaxRateLower>>()
              .Title("Код налоговой льготы в виде понижения налоговой ставки. Налог на имущество")
              .ListView(x => x.Title("Код налоговой льготы в виде понижения налоговой ставки. Налог на имущество"))
              .DetailView(x => x.Title("Код налоговой льготы в виде понижения налоговой ставки. Налог на имущество"))
              .LookupProperty(x => x.Text(t => t.Code));


            context.CreateVmConfigOnBase<DictObject, TaxRateLowerLand>()
              .Service<IDictObjectService<TaxRateLowerLand>>()
              .Title("Код налоговой льготы в виде снижения налоговой ставки. Земельный налог")
              .ListView(x => x.Title("Код налоговой льготы в виде снижения налоговой ставки. Земельный налог"))
              .DetailView(x => x.Title("Код налоговой льготы в виде снижения налоговой ставки. Земельный налог"))
              .LookupProperty(x => x.Text(t => t.Code));


            context.CreateVmConfigOnBase<DictObject, TaxRateLowerTS>()
              .Service<IDictObjectService<TaxRateLowerTS>>()
              .Title("Код налоговой льготы в виде снижения налоговой ставки. Транспортный налог")
              .ListView(x => x.Title("Код налоговой льготы в виде снижения налоговой ставки. Транспортный налог"))
              .DetailView(x => x.Title("Код налоговой льготы в виде снижения налоговой ставки. Транспортный налог"))
              .LookupProperty(x => x.Text(t => t.Code));


            context.CreateVmConfigOnBase<DictObject, TaxRateType>()
               .Service<IDictObjectService<TaxRateType>>()
               .Title("Вид налога")
               .ListView(x => x.Title("Вид налога"))
               .DetailView(x => x.Title("Вид налога"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TermOfPymentTaxRate>()
               .Service<IDictObjectService<TermOfPymentTaxRate>>()
               .Title("Срок уплаты авансовых платежей и налога (Имущество)")
               .ListView(x => x.Title("Срок уплаты авансовых платежей и налога (Имущество)"))
               .DetailView(x => x.Title("Срок уплаты авансовых платежей и налога (Имущество)"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TermOfPymentTaxRateLand>()
               .Service<IDictObjectService<TermOfPymentTaxRateLand>>()
               .Title("Срок уплаты авансовых платежей и налога (Земля)")
               .ListView(x => x.Title("Срок уплаты авансовых платежей и налога (Земля)"))
               .DetailView(x => x.Title("Срок уплаты авансовых платежей и налога (Земля)"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TermOfPymentTaxRateTS>()
               .Service<IDictObjectService<TermOfPymentTaxRateTS>>()
               .Title("Срок уплаты авансовых платежей и налога (Транспорт)")
               .ListView(x => x.Title("Срок уплаты авансовых платежей и налога (Транспорт)"))
               .DetailView(x => x.Title("Срок уплаты авансовых платежей и налога (Транспорт)"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, VehicleClass>()
               .Service<IDictObjectService<VehicleClass>>()
               .Title("Единый классификатор транспортных средств")
               .ListView(x => x.Title("Единый классификатор транспортных средств"))
               .DetailView(x => x.Title("Единый классификатор транспортных средств"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, VehicleLabel>()
               .Service<IDictObjectService<VehicleLabel>>()
               .Title("Класс ТС")
               .ListView(x => x.Title("Класс ТС"))
               .DetailView(x => x.Title("Класс ТС"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, CorpProp.Entities.NSI.VehicleModel>()
               .Service<IDictObjectService<CorpProp.Entities.NSI.VehicleModel>>()
               .Title("Марка ТС")
               .ListView(x => x.Title("Марка ТС"))
               .DetailView(x => x.Title("Марка ТС"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, BoostOrReductionFactor>()
                .Service<IDictObjectService<BoostOrReductionFactor>>()
                .Title("Повышающий/понижающий коэффициент")
                .ListView(x => x.Title("Повышающий/понижающий коэффициент"))
                .DetailView(x => x.Title("Повышающий/понижающий коэффициент"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxVehicleKindCode>()
                .Service<IDictObjectService<TaxVehicleKindCode>>()
                .Title("Код вида ТС")
                .ListView(x => x.Title("Код вида ТС"))
                .DetailView(x => x.Title("Код вида ТС"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxExemption>()
                .Service<IDictObjectService<TaxExemption>>()
                .Title("Код налоговой льготы")
                .ListView(x => x.Title("Код налоговой льготы"))
                .DetailView(x => x.Title("Код налоговой льготы"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, TaxExemptionLand>()
                .Service<IDictObjectService<TaxExemptionLand>>()
                .Title("Код налоговой льготы ЗУ")
                .ListView(x => x.Title("Код налоговой льготы ЗУ"))
                .DetailView(x => x.Title("Код налоговой льготы ЗУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxExemptionTS>()
                .Service<IDictObjectService<TaxExemptionTS>>()
                .Title("Код налоговой льготы ТС")
                .ListView(x => x.Title("Код налоговой льготы ТС"))
                .DetailView(x => x.Title("Код налоговой льготы ТС"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, TaxFederalExemption>()
                .Service<IDictObjectService<TaxFederalExemption>>()
                .Title("Федеральные льготы Имущество")
                .ListView(x => x.Title("Федеральные льготы Имущество"))
                .DetailView(x => x.Title("Федеральные льготы Имущество"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxFederalExemptionLand>()
                .Service<IDictObjectService<TaxFederalExemptionLand>>()
                .Title("Федеральные льготы ЗУ")
                .ListView(x => x.Title("Федеральные льготы ЗУ"))
                .DetailView(x => x.Title("Федеральные льготы ЗУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxFederalExemptionTS>()
                .Service<IDictObjectService<TaxFederalExemptionTS>>()
                .Title("Федеральные льготы ТС")
                .ListView(x => x.Title("Федеральные льготы ТС"))
                .DetailView(x => x.Title("Федеральные льготы ТС"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxRegionExemption>()
                .Service<IDictObjectService<TaxRegionExemption>>()
                .Title("Региональные льготы Имущество")
                .ListView(x => x.Title("Региональные льготы Имущество"))
                .DetailView(x => x.Title("Региональные льготы Имущество"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxRegionExemptionLand>()
                .Service<IDictObjectService<TaxRegionExemptionLand>>()
                .Title("Региональные льготы ЗУ")
                .ListView(x => x.Title("Региональные льготы ЗУ"))
                .DetailView(x => x.Title("Региональные льготы ЗУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, TaxRegionExemptionTS>()
                .Service<IDictObjectService<TaxRegionExemptionTS>>()
                .Title("Региональные льготы ТС")
                .ListView(x => x.Title("Региональные льготы ТС"))
                .DetailView(x => x.Title("Региональные льготы ТС"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SubsoilUser>()
                .Service<IDictObjectService<SubsoilUser>>()
                .Title("Недропользователь")
                .ListView(x => x.Title("Недропользователь"))
                .DetailView(x => x.Title("Недропользователь"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, CostKindRentalPayments>()
                .Service<IDictObjectService<CostKindRentalPayments>>()
                .Title("Вид затрат в части арендных платежей")
                .ListView(x => x.Title("Вид затрат в части арендных платежей"))
                .DetailView(x => x.Title("Вид затрат в части арендных платежей"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ObjectLocationRent>()
                .Service<IDictObjectService<ObjectLocationRent>>()
                .Title("Местоположения объекта (аренда)")
                .ListView(x => x.Title("Местоположения объекта (аренда)"))
                .DetailView(x => x.Title("Местоположения объекта (аренда)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, AssetHolderRSBU>()
                .Service<IDictObjectService<AssetHolderRSBU>>()
                .Title("На чьем балансе учитывается ОС в РСБУ")
                .ListView(x => x.Title("На чьем балансе учитывается ОС в РСБУ"))
                .DetailView(x => x.Title("На чьем балансе учитывается ОС в РСБУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, StateObjectRent>()
                .Service<IDictObjectService<StateObjectRent>>()
                .Title("Состояние ОС/НМА (аренда)")
                .ListView(x => x.Title("Состояние ОС/НМА (аренда)"))
                .DetailView(x => x.Title("Состояние ОС/НМА (аренда)"))
                .LookupProperty(x => x.Text(t => t.Name));
            #endregion //END EUSI REGION

        }

        /// <summary>
        /// Инициализация моделей справочников.
        /// </summary>
        /// <param name="context"></param>
        private static void InitHistory(IInitializerContext context)
        {
            #region NSI 
            context.CreateVmConfigOnBase<Deposit>("DictMenu", "DepositMenu")
                .Service<IDictHistoryService<Deposit>>()
                .ListView(builder => builder.Title("Месторождения"))
                .DetailView(builder => builder.Title("Месторождение"));

            context.CreateVmConfigOnBase<WellCategory>("DictMenu", "WellCategoryMenu")
                .Service<IDictHistoryService<WellCategory>>()
                .ListView(builder => builder.Title("Категории скважины"))
                .DetailView(builder => builder.Title("Категория скважины"));

            context.CreateVmConfigOnBase<ResponseRowState>("DictMenu", "ResponseRowStateMenu")
              .Service<IDictHistoryService<ResponseRowState>>()
              .Title("Статусы строк ответа")
              .ListView(x => x.Title("Статусы строк ответа"))
              .DetailView(x => x.Title("Статус строки ответа"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<SibDealStatus>("DictMenu", "SibDealStatusMenu")
              .Service<IDictHistoryService<SibDealStatus>>()
              .Title("Статус договора")
              .ListView(x => x.Title("Статус договора"))
              .DetailView(x => x.Title("Статус договора"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DocKind>("DictMenu", "DocKindMenu")
              .Service<IDictHistoryService<DocKind>>()
              .Title("Вид документа")
              .ListView(x => x.Title("Вид документа"))
              .DetailView(x => x.Title("Вид документа"))
              .LookupProperty(x => x.Text(t => t.Name))
              .IsReadOnly(true);

            context.CreateVmConfigOnBase<DocStatus>("DictMenu", "DocStatusMenu")
                .Service<IDictHistoryService<DocStatus>>()
                .Title("Статус документа")
                .ListView(x => x.Title("Статус документа")
                .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
                .DetailView(x => x.Title("Статус документа")
                 .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DocType>("DictMenu", "DocTypeMenu")
                .Service<IDictHistoryService<DocType>>()
                .Title("Тип документа")
                .ListView(x => x.Title("Тип документа"))
                .DetailView(x => x.Title("Тип документа"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<FileCardType>("DictMenu", "FileCardTypeMenu")
                .Service<IDictHistoryService<FileCardType>>()
                .Title("Тип документа (FileCard)")
                .ListView(x => x.Title("Тип документа (FileCard)"))
                .DetailView(x => x.Title("Тип документа (FileCard)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DocTypeOperation>("DictMenu", "DocTypeOperationMenu")
               .Service<IDictHistoryService<DocTypeOperation>>()
               .Title("Вид операции")
               .ListView(x => x.Title("Вид операций")
               .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
               .DetailView(x => x.Title("Вид операции")
               .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
               .LookupProperty(x => x.Text(t => t.Name));

            /////
            context.CreateVmConfigOnBase<AircraftKind>("DictMenu", "AircraftKindMenu")
             .Service<IDictHistoryService<AircraftKind>>()
             .Title("Вид воздушного судна")
             .ListView(x => x.Title("Виды воздушных судов"))
             .DetailView(x => x.Title("Вид воздушного судна"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AircraftType>("DictMenu", "AircraftTypeMenu")
             .Service<IDictHistoryService<AircraftType>>()
             .Title("Тип воздушного судна")
             .ListView(x => x.Title("Типы воздушных судов"))
             .DetailView(x => x.Title("Тип воздушного судна"))
             .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<ShipClass>("DictMenu", "ShipClassMenu")
             .Service<IDictHistoryService<ShipClass>>()
             .Title("Класс судна")
             .ListView(x => x.Title("Классы судов"))
             .DetailView(x => x.Title("Класс судна"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ShipKind>("DictMenu", "ShipKindMenu")
             .Service<IDictHistoryService<ShipKind>>()
             .Title("Вид судна")
             .ListView(x => x.Title("Виды судов"))
             .DetailView(x => x.Title("Вид судна"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ShipType>("DictMenu", "ShipTypeMenu")
             .Service<IDictHistoryService<ShipType>>()
             .Title("Тип судна")
             .ListView(x => x.Title("Типы судов"))
             .DetailView(x => x.Title("Тип судна"))
             .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<VehicleCategory>("DictMenu", "VehicleCategoryMenu")
             .Service<IDictHistoryService<VehicleCategory>>()
             .Title("Категория ТС")
             .ListView(x => x.Title("Категории ТС"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Категория ТС")
                .Editors(e => e.Add(p => p.VehicleLabel, o => o.Visible(true))));

            context.CreateVmConfigOnBase<TenureType>("DictMenu", "TenureTypeMenu")
             .Service<IDictHistoryService<TenureType>>()
             .Title("Тип ТС")
             .ListView(x => x.Title("Типы ТС"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Тип ТС"));


            context.CreateVmConfigOnBase<TypeAccounting>("DictMenu", "TypeAccountingMenu")
             .Service<IDictHistoryService<TypeAccounting>>()
             .Title("Ведение БУ")
             .ListView(x => x.Title("Ведение БУ"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Ведение БУ"));


            context.CreateVmConfigOnBase<VehicleType>("DictMenu", "VehicleTypeMenu")
             .Service<IDictHistoryService<VehicleType>>()
            .Title("Вид ТС")
            .ListView(x => x.Title("Вид ТС")
             .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("Вид ТС")
            .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
            .IsReadOnly(true);

            context.CreateVmConfigOnBase<RegistrationBasis>("DictMenu", "RegistrationBasisMenu")
               .Service<IDictHistoryService<RegistrationBasis>>()
              .Title("Основание регистрации прав")
              .ListView(x => x.Title("Основание регистрации прав"))
              .DetailView(x => x.Title("Основание регистрации прав"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<ScheduleStateRegistrationStatus>("DictMenu", "ScheduleStateRegistrationStatusMenu")
                .Service<IDictHistoryService<ScheduleStateRegistrationStatus>>()
               .Title("Статус графика гос. регистрации")
               .ListView(x => x.Title("Статусы графиков гос. регистрации"))
               .DetailView(x => x.Title("Статус графика гос. регистрации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<RightKind>("DictMenu", "RightKindMenu")
                .Service<IDictHistoryService<RightKind>>()
                .Title("Вид права")
                .ListView(x => x.Title("Виды права"))
                .DetailView(x => x.Title("Вид права"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<RightType>("DictMenu", "RightTypeMenu")
                .Service<IDictHistoryService<RightType>>()
                .Title("Тип права")
                .ListView(x => x.Title("Типы прав"))
                .DetailView(x => x.Title("Тип права"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ExtractType>("DictMenu", "ExtractTypeMenu")
               .Service<IDictHistoryService<ExtractType>>()
               .Title("Тип выписки ЕГРН")
               .ListView(x => x.Title("Тип выписки ЕГРН"))
               .DetailView(x => x.Title("Тип выписки ЕГРН"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ExtractFormat>("DictMenu", "ExtractFormatMenu")
              .Service<IDictHistoryService<ExtractFormat>>()
              .Title("Формат выписки")
              .ListView(x => x.Title("Форматы выписки"))
              .DetailView(x => x.Title("Формат выписка"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<EncumbranceType>("DictMenu", "EncumbranceTypeMenu")
                .Service<IDictHistoryService<EncumbranceType>>()
                .Title("Вид ограничения (обременения)")
                .ListView(x => x.Title("Вид ограничения (обременения)"))
                .DetailView(x => x.Title("Вид ограничения (обременения)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AppraisalType>("DictMenu", "AppraisalTypeMenu")
              .Service<IDictHistoryService<AppraisalType>>()
              .Title("Тип оценки объектов")
              .ListView(x => x.Title("Типы оценок"))
              .DetailView(x => x.Title("Тип оценки"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<InvestmentType>("DictMenu", "InvestmentTypeMenu")
              .Service<IDictHistoryService<InvestmentType>>()
              .Title("Тип акции / доли")
              .ListView(x => x.Title("Типы акций / долей"))
              .DetailView(x => x.Title("Тип акции / доли"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SuccessionType>("DictMenu", "SuccessionTypeMenu")
              .Service<IDictHistoryService<SuccessionType>>()
              .Title("Тип правопреемства")
              .ListView(x => x.Title("Типы правопреемства"))
              .DetailView(x => x.Title("Тип правопреемства"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<AccountingMovingType>("DictMenu", "AccountingMovingTypeMenu")
               .Service<IDictHistoryService<AccountingMovingType>>()
               .Title("Тип изменения ОБУ")
               .ListView(x => x.Title("Тип изменения ОБУ"))
               .DetailView(x => x.Title("Тип изменения ОБУ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<AccountingStatus>("DictMenu", "AccountingStatusMenu")
             .Service<IDictHistoryService<AccountingStatus>>()
             .Title("Статус ОБУ")
             .ListView(x => x.Title("Статусы ОБУ"))
             .DetailView(x => x.Title("Статус ОБУ"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AccountingSystem>("DictMenu", "AccountingSystemMenu")
            .Service<IDictHistoryService<AccountingSystem>>()
            .Title("Система учёта")
            .ListView(x => x.Title("Системы учёта"))
            .DetailView(x => x.Title("Система учёта"))
            .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ActualKindActivity>("DictMenu", "ActualKindActivityMenu")
              .Service<IDictHistoryService<ActualKindActivity>>()
              .Title("Фактический вид деятельности")
              .ListView(x => x.Title("Фактические виды деятельности"))
              .DetailView(x => x.Title("Фактический вид деятельности"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AddonAttributeGroundCategory>("DictMenu", "AddonAttributeGroundCategoryMenu")
              .Service<IDictHistoryService<AddonAttributeGroundCategory>>()
              .Title("Дополнительный признак категории земель")
              .ListView(x => x.Title("Дополнительные признаки категории земель"))
              .DetailView(x => x.Title("Дополнительный признак категории земель"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AppraisalGoal>("DictMenu", "AppraisalGoalMenu")
                .Service<IDictHistoryService<AppraisalGoal>>()
               .Title("Цель оценки")
               .ListView(x => x.Title("Цели оценок"))
               .DetailView(x => x.Title("Цель оценки"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AppraisalPurpose>("DictMenu", "AppraisalPurposeMenu")
               .Service<IDictHistoryService<AppraisalPurpose>>()
               .Title("Назначение оценки")
               .ListView(x => x.Title("Назначения оценок"))
               .DetailView(x => x.Title("Назначение оценки"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AppType>("DictMenu", "AppTypeMenu")
              .Service<IDictHistoryService<AppType>>()
              .Title("Тип оценки")
              .ListView(x => x.Title("Типы оценок"))
              .DetailView(x => x.Title("Тип оценки"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BaseExclusionFromPerimeter>("DictMenu", "BaseExclusionFromPerimeterMenu")
              .Service<IDictHistoryService<BaseExclusionFromPerimeter>>()
              .Title("Основание для исключения объекта из Периметра")
              .ListView(x => x.Title("Основание для исключения объекта из Периметра"))
              .DetailView(x => x.Title("Основание для исключения объекта из Периметра"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BaseInclusionInPerimeter>("DictMenu", "BaseInclusionInPerimeterMenu")
             .Service<IDictHistoryService<BaseInclusionInPerimeter>>()
             .Title("Основание для включения объекта в Периметр")
             .ListView(x => x.Title("Основание для включения объекта в Периметр"))
             .DetailView(x => x.Title("Основание для включения объекта в Периметр"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BasisForInvestments>("DictMenu", "BasisForInvestmentsMenu")
               .Service<IDictHistoryService<BasisForInvestments>>()
               .Title("Основание акционирования")
               .ListView(x => x.Title("Основания акционирования"))
               .DetailView(x => x.Title("Основание акционирования"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BusinessArea>("DictMenu", "BusinessAreaMenu")
               .Service<IDictHistoryService<BusinessArea>>()
               .Title("Бизнес-сфера")
               .ListView(x => x.Title("Бизнес-сферы")
                    .Columns(cf => cf
                       .Add(col => col.DictObjectState, ac => ac.Visible(false))
                       .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                    ))
               .DetailView(x => x.Title("Бизнес-сфера")
                    .Editors(ef => ef
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                    ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BusinessDirection>("DictMenu", "BusinessDirectionMenu")
               .Service<IDictHistoryService<BusinessDirection>>()
               .Title("Бизнес-направление")
               .ListView(x => x.Title("Бизнес-направления"))
               .DetailView(x => x.Title("Бизнес-направление"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BusinessSegment>("DictMenu", "BusinessSegmentMenu")
             .Service<IDictHistoryService<BusinessSegment>>()
             .Title("Бизнес-сегмент")
             .ListView(x => x.Title("Бизнес-сегменты"))
             .DetailView(x => x.Title("Бизнес-сегмент"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BusinessBlock>("DictMenu", "BusinessBlockMenu")
             .Service<IDictHistoryService<BusinessBlock>>()
             .Title("Бизнес-блок")
             .ListView(x => x.Title("Бизнес-блоки"))
             .DetailView(x => x.Title("Бизнес-блок"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BusinessUnit>("DictMenu", "BusinessUnitMenu")
                .Service<IDictHistoryService<BusinessUnit>>()
                .Title("Бизнес-единица (Аналитика 97 ИКСО)")
                .ListView(x => x.Title("Бизнес-единицы (Аналитика 97 ИКСО)")
                    .Columns(cf => cf
                       .Add(col => col.DictObjectState, ac => ac.Visible(false))
                       .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                       .Add(col => col.DateFrom, ac => ac.Visible(false))
                       .Add(col => col.DateTo, ac => ac.Visible(false))
                    ))
                .DetailView(x => x.Title("Бизнес-единица (Аналитика 97 ИКСО)")
                    .Editors(ef => ef
                        .Add(ed => ed.DateFrom, ac => ac.Visible(false).IsRequired(false))
                        .Add(ed => ed.DateTo, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                    ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BIK>("DictMenu", "BIKMenu")
            .Title("БИК")
            .ListView(x => x.Title("БИК"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("БИК"));

            context.CreateVmConfigOnBase<ClassFixedAsset>("DictMenu", "ClassFixedAssetMenu")
             .Service<IDictHistoryService<ClassFixedAsset>>()
              .Title("Класс (БУ)")
              .ListView(x => x.Title("Класс (БУ)")
              .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Класс (БУ)")
              .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
              ;

            context.CreateVmConfigOnBase<Consolidation>("DictMenu", "ConsolidationMenu")
             .Service<IDictHistoryService<Consolidation>>()
                .Title("Единица консолидации (БЕ)")
                .ListView(x => x.Title("ЕК(БЕ)")
                .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                )
                )
                .DetailView(x => x.Title("Единица консолидации (БЕ)")
                           .Editors(e => e
                      .Add(a => a.DictObjectState, a => a.Visible(true))
                      .Add(a => a.DictObjectStatus, a => a.Visible(true))
                      .Add(a => a.DateFrom, a => a.IsReadOnly(false))
                      .Add(a => a.DateTo, a => a.IsReadOnly(false))
                      .Add(a => a.Code, a => a.IsReadOnly(true))
                      .Add(a => a.Name, a => a.IsReadOnly(true))))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<ContourType>("DictMenu", "ContourTypeMenu")
                .Service<IDictHistoryService<ContourType>>()
                .Title("Тип контура")
                .ListView(x => x.Title("Типы контура"))
                .DetailView(x => x.Title("Тип контура"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ContragentKind>("DictMenu", "ContragentKindMenu")
                .Service<IDictHistoryService<ContragentKind>>()
                .Title("Вид контрагента")
                .ListView(x => x.Title("Виды контрагентов"))
                .DetailView(x => x.Title("Вид контрагента"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<Currency>("DictMenu", "CurrencyMenu")
               .Service<IDictHistoryService<Currency>>()
               .Title("Валюта")
               .ListView(x => x.Title("Валюта")
               .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
               .DetailView(x => x.Title("Валюта")
               .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
               .LookupProperty(x => x.Text(t => t.Code))
               .IsReadOnly(true);

            context.CreateVmConfigOnBase<DealType>("DictMenu", "DealTypeMenu")
              .Service<IDictHistoryService<DealType>>()
              .Title("Вид договора")
              .ListView(x => x.Title("Вид договора")
              .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
              .DetailView(x => x.Title("Вид договора")
              .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DepreciationGroup>("DictMenu", "DepreciationGroupMenu")
              .Service<IDictHistoryService<DepreciationGroup>>()
              .Title("Амортизационная группа")
              .ListView(x => x.Title("Амортизационные группы"))
              .DetailView(x => x.Title("Амортизационная группа"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<EstateAppraisalType>("DictMenu", "EstateAppraisalTypeMenu")
             .Service<IDictHistoryService<EstateAppraisalType>>()
              .Title("Тип объекта оценки")
              .ListView(x => x.Title("Типы объектов оценок"))
              .DetailView(x => x.Title("Тип объекта оценки"))
              .LookupProperty(x => x.Text(t => t.Name));



            context.CreateVmConfigOnBase<ExternalMappingSystem>("DictMenu", "ExternalMappingSystemMenu")
                .Service<IDictHistoryService<ExternalMappingSystem>>()
                .Title("Внешняя система сопоставления")
               .ListView(x => x.Title("Внешние системы сопоставления"))
               .DetailView(x => x.Title("Внешняя система сопоставления"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<FeatureType>("DictMenu", "FeatureTypeMenu")
                .Service<IDictHistoryService<FeatureType>>()
                .Title("Тип основной характеристики объекта недвижимости")
                .ListView(x => x.Title("Тип основной характеристики объекта недвижимости"))
                .DetailView(x => x.Title("Тип основной характеристики объекта недвижимости"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<GroundCategory>("DictMenu", "GroundCategoryMenu")
                .Service<IDictHistoryService<GroundCategory>>()
               .Title("Код категории земель")
               .ListView(x => x.Title("Код категории земель")
                .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
               .DetailView(x => x.Title("Код категории земель")
               .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<InformationSource>("DictMenu", "InformationSourceMenu")
                .Service<IDictHistoryService<InformationSource>>()
                .Title("Источник информации")
                .ListView(x => x.Title("Источники информации"))
                .DetailView(x => x.Title("Источник информации"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<IntangibleAssetRightType>("DictMenu", "IntangibleAssetRightTypeMenu")
                .Service<IDictHistoryService<IntangibleAssetRightType>>()
                .Title("Тип права на НМА")
                .ListView(x => x.Title("Типы прав на НМА"))
                .DetailView(x => x.Title("Тип права НМА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<IntangibleAssetStatus>("DictMenu", "IntangibleAssetStatusMenu")
                .Service<IDictHistoryService<IntangibleAssetStatus>>()
               .Title("Статус НМА")
               .ListView(x => x.Title("Статусы НМА"))
               .DetailView(x => x.Title("Статус НМА"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<IntangibleAssetType>("DictMenu", "IntangibleAssetTypeMenu")
                .Service<IDictHistoryService<IntangibleAssetType>>()
                .Title("Вид НМА")
                .ListView(x => x.Title("Вид НМА"))
                .DetailView(x => x.Title("Вид НМА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ImplementationWay>("DictMenu", "ImplementationWayMenu")
              .Service<IDictHistoryService<ImplementationWay>>()
                .Title("Способ реализации ННА")
                .ListView(x => x.Title("Способы реализации ННА"))
                .DetailView(x => x.Title("Способ реализации ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ImportHistoryState>("DictMenu", "ImportHistoryStateMenu")
             .Service<IDictHistoryService<ImportHistoryState>>()
               .Title("Статус истории импорта")
               .ListView(x => x.Title("Статус истории импорта"))
               .DetailView(x => x.Title("Статус истории импорта"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<EstateType>("DictMenu", "EstateTypeMenu")
             .Service<IDictHistoryService<EstateType>>()
                .Title("Класс КС")
                .ListView(x => x.Title("Классы КС"))
                .DetailView(x => x.Title("Класс КС"))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<LandType>("DictMenu", "LandTypeMenu")
               .Service<IDictHistoryService<LandType>>()
              .Title("Тип ЗУ")
              .ListView(x => x.Title("Тип ЗУ"))
              .DetailView(x => x.Title("Тип ЗУ"))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<LayingType>("DictMenu", "LayingTypeMenu")
               .Service<IDictHistoryService<LayingType>>()
              .Title("Тип прокладки линейных сооружений")
              .ListView(x => x.Title("Типы прокладки линейных сооружений"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Тип прокладки линейных сооружений"));

            context.CreateVmConfigOnBase<LeavingReason>("DictMenu", "LeavingReasonMenu")
              .Service<IDictHistoryService<LeavingReason>>()
              .Title("Причины выбытия")
              .ListView(x => x.Title("Причины выбытия"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Причина выбытия"));

            context.CreateVmConfigOnBase<NonCoreAssetAppraisalType>("DictMenu", "NonCoreAssetAppraisalTypeMenu")
             .Service<IDictHistoryService<NonCoreAssetAppraisalType>>()
                .Title("Тип оценки ННА")
                .ListView(x => x.Title("Типы оценок ННА"))
                .DetailView(x => x.Title("Тип оценки ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetListItemState>("DictMenu", "NonCoreAssetListItemStateMenu")
           .Service<IDictHistoryService<NonCoreAssetListItemState>>()
         .Title("Статус строки перечня ННА")
         .ListView(x => x.Title("Статусы строки перечня ННА"))
         .DetailView(x => x.Title("Статус строки перечня ННА"))
         .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetListKind>("DictMenu", "NonCoreAssetListKindMenu")
             .Service<IDictHistoryService<NonCoreAssetListKind>>()
            .Title("Вид перечня ННА")
            .ListView(x => x.Title("Виды перечней ННА"))
            .DetailView(x => x.Title("Вид перечня ННА"))
            .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetListType>("DictMenu", "NonCoreAssetListTypeMenu")
             .Service<IDictHistoryService<NonCoreAssetListType>>()
           .Title("Тип перечня ННА")
           .ListView(x => x.Title("Типы перечня ННА"))
           .DetailView(x => x.Title("Тип перечня ННА"))
           .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetSaleAcceptType>("DictMenu", "NonCoreAssetSaleAcceptTypeMenu")
           .Service<IDictHistoryService<NonCoreAssetSaleAcceptType>>()
           .Title("Вид одобрения реализации ННА")
           .ListView(x => x.Title("Виды одобрений реализаций ННА"))
           .DetailView(x => x.Title("Вид одобрения реализации ННА"))
           .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetType>("DictMenu", "NonCoreAssetTypeMenu")
           .Service<IDictHistoryService<NonCoreAssetType>>()
           .Title("Тип ННА")
           .ListView(x => x.Title("Типы ННА"))
           .DetailView(x => x.Title("Тип ННА"))
           .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetStatus>("DictMenu", "NonCoreAssetStatusMenu")
             .Service<IDictHistoryService<NonCoreAssetStatus>>()
             .Title("Статус ННА")
             .ListView(x => x.Title("Статусы ННА"))
             .DetailView(x => x.Title("Статус ННА"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetInventoryType>("DictMenu", "NonCoreAssetInventoryTypeMenu")
              .Service<IDictHistoryService<NonCoreAssetInventoryType>>()
              .Title("Виды реестров ННА")
              .ListView(x => x.Title("Вид реестра ННА"))
              .DetailView(x => x.Title("Вид реестра ННА"))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetOwnerCategory>("DictMenu", "NonCoreAssetOwnerCategoryMenu")
                .Service<IDictHistoryService<NonCoreAssetOwnerCategory>>()
                .Title("Категории балансодержаталей")
                .ListView(x => x.Title("Категория балансодержателя"))
                .DetailView(x => x.Title("Категория балансодержателя"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetListState>("DictMenu", "NonCoreAssetListStateMenu")
                .Service<IDictHistoryService<NonCoreAssetListState>>()
                .Title("Статусы перечня ННА")
                .ListView(x => x.Title("Статус перечня ННА"))
                .DetailView(x => x.Title("Статус перечня ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetSaleStatus>("DictMenu", "NonCoreAssetSaleStatusMemu")
                .Service<IDictHistoryService<NonCoreAssetSaleStatus>>()
                .Title("Статусы реализации ННА")
                .ListView(x => x.Title("Статус реализации ННА"))
                .DetailView(x => x.Title("Статус реализации ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetInventoryState>("DictMenu", "NonCoreAssetInventoryStateMenu")
                .Service<IDictHistoryService<NonCoreAssetInventoryState>>()
                .Title("Статусы реестра ННА")
                .ListView(x => x.Title("Статус реестра ННА"))
                .DetailView(x => x.Title("Статус реестра ННА"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SibLocation>("DictMenu", "SibLocationMenu")
              .Service<IDictHistoryService<SibLocation>>()
              .Title("Местоположения")
              .ListView(x => x.Title("Местоположения"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Местоположение"));

            context.CreateVmConfigOnBase<OKATO>("DictMenu", "OKATOMenu")
             .Service<IDictHistoryService<OKATO>>()
                .Title("Код ОКАТО")
                .ListView(x => x.Title("Код ОКАТО")
                .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
                .DetailView(x => x.Title("Код ОКАТО")
                .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
                .LookupProperty(x => x.Text(t => t.Title));


            context.CreateVmConfigOnBase<OKOF94>("DictMenu", "OKOF94Menu")
                .Service<IDictHistoryService<OKOF94>>()
                .Title("ОКОФ (версия 1994г.)")
                .ListView(x => x.Title("ОКОФ")
                    .Columns(cols => cols
                        .Add(col => col.AdditionalCode, ac => ac.Order(2))
                        .Add(col => col.Name, ac => ac.Order(3))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DateFrom, ac => ac.Order(6))
                        .Add(col => col.DateFrom, ac => ac.Order(7))
                    ))
                .DetailView(x => x.Title("ОКОФ")
                    .Editors(ed => ed
                        .Add(e => e.AdditionalCode, ac => ac.Order(2))
                        .Add(e => e.Name, ac => ac.Order(3))
                        .Add(e => e.DictObjectStatus, ac => ac.Visible(true).Order(4))
                        .Add(e => e.DictObjectState, ac => ac.Visible(true).Order(5))
                        .Add(e => e.DateFrom, ac => ac.Order(6))
                        .Add(e => e.DateFrom, ac => ac.Order(7))
                    ))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<OKOF2014>("DictMenu", "OKOF2014Menu")
                .Service<IDictHistoryService<OKOF2014>>()
                .Title("ОКОФ (версия 2014г.)")
                .ListView(x => x.Title("ОКОФ 2")
                    .Columns(cols => cols
                        .Add(col => col.AdditionalCode, ac => ac.Order(2))
                        .Add(col => col.Name, ac => ac.Order(3))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DateFrom, ac => ac.Order(6))
                        .Add(col => col.DateFrom, ac => ac.Order(7))
                    ))
                .DetailView(x => x.Title("ОКОФ 2")
                    .Editors(eds => eds
                        .Add(ed => ed.AdditionalCode, ac => ac.Order(2))
                        .Add(ed => ed.Name, ac => ac.Order(3))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(true).Order(4))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(true).Order(5))
                        .Add(ed => ed.DateFrom, ac => ac.Order(6))
                        .Add(ed => ed.DateFrom, ac => ac.Order(7))
                    ))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfigOnBase<AddonOKOF>("DictMenu", "AddonOKOFMenu")
                 .Service<IDictHistoryService<AddonOKOF>>()
               .Title("Доп. коды ОКОФ")
               .ListView(x => x.Title("Доп. коды ОКОФ")
               .Columns(cols => cols.Clear()
                    .Add(col => col.Name, ac => ac.Visible(true).Order(1))
                    .Add(col => col.PublishCode, ac => ac.Visible(true).Order(2))
                    .Add(col => col.Code, ac => ac.Visible(true).Order(3))
                    .Add(col => col.OKOF94, ac => ac.Visible(false).Order(4))
                    .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(5))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(6))
                    .Add(col => col.DateFrom, ac => ac.Visible(true).Order(7))
                    .Add(col => col.DateFrom, ac => ac.Visible(true).Order(8))
                 ))
               .DetailView(x => x.Title("Доп. код ОКОФ")
               .Editors(eds => eds.Clear()
                    .Add(ed => ed.Name, ac => ac.Visible(true).Order(1))
                    .Add(ed => ed.PublishCode, ac => ac.Visible(true).Order(2))
                    .Add(c => c.OKOF94, ac => ac.Visible(true).Order(3))
                    .Add(ed => ed.DictObjectStatus, ac => ac.Visible(true).Order(4))
                    .Add(ed => ed.DictObjectState, ac => ac.Visible(true).Order(5))
                    .Add(ed => ed.DateFrom, ac => ac.Visible(true).Order(6))
                    .Add(ed => ed.DateFrom, ac => ac.Visible(true).Order(7))

               ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<OKOFS>("DictMenu", "OKOFSMenu")
             .Service<IDictHistoryService<OKOFS>>()
                .Title("ОКОФС")
                .ListView(x => x.Title("ОКОФС"))
                .DetailView(x => x.Title("ОКОФС"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<OKOGU>("DictMenu", "OKOGUMenu")
             .Service<IDictHistoryService<OKOGU>>()
                .Title("ОКОГУ")
                .ListView(x => x.Title("ОКОГУ"))
                .DetailView(x => x.Title("ОКОГУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<OKOPF>("DictMenu", "OKOPFMenu")
             .Service<IDictHistoryService<OKOPF>>()
                .Title("ОКОПФ")
                .ListView(x => x.Title("ОКОПФ"))
                .DetailView(x => x.Title("ОКОПФ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<OKPO>("DictMenu", "OKPOMenu")
             .Service<IDictHistoryService<OKPO>>()
                .Title("ОКПО")
                .ListView(x => x.Title("ОКПО"))
                .DetailView(x => x.Title("ОКПО"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<OKTMO>("DictMenu", "OKTMOMenu")
                .Service<IDictHistoryService<OKTMO>>()
               .Title("Код ОКТМО")
               .ListView(x => x.Title("Код ОКТМО")
               .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
               .DetailView(x => x.Title("Код ОКТМО")
               .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
               .LookupProperty(x => x.Text(t => t.Title));



            context.CreateVmConfigOnBase<OPF>("DictMenu", "OPFMenu")
              .Service<IDictHistoryService<OPF>>()
              .Title("ОПФ")
              .ListView(x => x.Title("ОПФ")
              .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
              .DetailView(x => x.Title("ОПФ")
              .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
              .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<RSBU>("DictMenu", "RSBUMenu")
             .Service<IDictHistoryService<RSBU>>()
               .Title("РСБУ")
               .ListView(x => x.Title("РСБУ")
               .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
              .DetailView(x => x.Title("РСБУ")
              .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
              .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<OwnershipType>("DictMenu", "OwnershipTypeMenu")
              .Service<IDictHistoryService<OwnershipType>>()
             .Title("Форма собственности")
             .ListView(x => x.Title("Формы собственности"))
             .DetailView(x => x.Title("Форма собственности"))
             .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<ProductionBlock>("DictMenu", "ProductionBlockMenu")
              .Service<IDictHistoryService<ProductionBlock>>()
                .Title("Производственный блок")
                .ListView(x => x.Title("Производственные блоки"))
                .DetailView(x => x.Title("Производственный блок"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<PropertyComplexKind>("DictMenu", "PropertyComplexKindMenu")
                .Service<IDictHistoryService<PropertyComplexKind>>()
                .Title("Класс ИК")
                .ListView(x => x.Title("Класс ИК"))
                .DetailView(x => x.Title("Класс ИК"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<PropertyComplexIOType>("DictMenu", "PropertyComplexIOTypeMenu")
                .Service<IDictHistoryService<PropertyComplexIOType>>()
                .Title("Тип ИК (ОИ)")
                .ListView(x => x.Title("Тип ИК (ОИ)"))
                .DetailView(x => x.Title("Тип ИК (ОИ)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SibProjectStatus>("DictMenu", "SibProjectStatusMenu")
               .Service<IDictHistoryService<SibProjectStatus>>()
               .Title("Статус проекта")
               .ListView(x => x.Title("Статусы проекта"))
               .DetailView(x => x.Title("Статус проект"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<RealEstateKind>("DictMenu", "RealEstateKindMenu")
                .Service<IDictHistoryService<RealEstateKind>>()
                .Title("Вид объекта недвижимости")
                .ListView(x => x.Title("Вид объекта недвижимости"))
                .DetailView(x => x.Title("Вид объекта недвижимости"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<RealEstatePurpose>("DictMenu", "RealEstatePurposeMenu")
                 .Service<IDictHistoryService<RealEstatePurpose>>()
                .Title("Назначение объекта недвижимого имущества")
                .ListView(x => x.Title("Назначения объекта недвижимого имущества"))
                .DetailView(x => x.Title("Назначение объекта недвижимого имущества"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ReceiptReason>("DictMenu", "ReceiptReasonMenu")
                 .Service<IDictHistoryService<ReceiptReason>>()
               .Title("Способ поступления")
               .ListView(x => x.Title("Способ поступления"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Способ поступления"));

            context.CreateVmConfigOnBase<RequestStatus>("DictMenu", "RequestStatusMenu")
                 .Service<IDictHistoryService<RequestStatus>>()
               .Title("Статус запроса")
               .ListView(x => x.Title("Статусы запросов"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Статус запроса"));

            context.CreateVmConfigOnBase<ResponseStatus>("DictMenu", "ResponseStatusMenu")
                .Service<IDictHistoryService<ResponseStatus>>()
               .Title("Статус ответа")
               .ListView(x => x.Title("Статусы ответов"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Статус ответа"));

            context.CreateVmConfigOnBase<RightHolderKind>("DictMenu", "RightHolderKindMenu")
                .Service<IDictHistoryService<RightHolderKind>>()
              .Title("Вид основания для правообладания")
              .ListView(x => x.Title("Виды оснований для правообладания"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Вид основания для правообладания"));

            context.CreateVmConfigOnBase<ShipAssignment>("DictMenu", "ShipAssignmentMenu")
                 .Service<IDictHistoryService<ShipAssignment>>()
             .Title("Назначение судна")
             .ListView(x => x.Title("Назначения судов"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Назначение судна"));

            context.CreateVmConfigOnBase<SibBank>("DictMenu", "SibBankMenu")
                .Service<IDictHistoryService<SibBank>>()
             .Title("Банк")
             .ListView(x => x.Title("Банки"))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Банк"));

            context.CreateVmConfigOnBase<SibFederalDistrict>("DictMenu", "SibFederalDistrictMenu")
                 .Service<ISibFederalDistrictHistoryService>()
              .Title("Федеральный округ")
              .ListView(x => x.Title("Федеральные округа")
              .Columns(cols => cols
                    .Add(col => col.PublishCode, ac => ac.Visible(true).Order(1))
                        .Add(col => col.Name, ac => ac.Visible(true).Order(2))
                        .Add(col => col.Country, ac => ac.Visible(true).Order(3))
                        .Add(col => col.DateFrom, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DateTo, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(6))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(7))
                ))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("Федеральный округ")
               .Editors(ed => ed
                .Add(e => e.Country, ac => ac.Visible(true))
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
              ;

            context.CreateVmConfigOnBase<SibKBK>("DictMenu", "SibKBKMenu")
              .Service<IDictHistoryService<SibKBK>>()
              .Title("КБК")
              .ListView(x => x.Title("КБК"))
              .LookupProperty(x => x.Text(t => t.Name))
              .DetailView(x => x.Title("КБК"));

            context.CreateVmConfigOnBase<SibMeasure>("DictMenu", "SibMeasureMenu")
             .Service<IDictHistoryService<SibMeasure>>()
             .Title("Единица измерения")
             .ListView(x => x.Title("Единицы измерения")
              .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
             .LookupProperty(x => x.Text(t => t.Name))
             .DetailView(x => x.Title("Единица измерения")
             .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
             .IsReadOnly(true);

            context.CreateVmConfigOnBase<SibProjectType>("DictMenu", "SibProjectTypeMenu")
             .Service<IDictHistoryService<SibProjectType>>()
            .Title("Тип проекта")
            .ListView(x => x.Title("Типы проектов"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("Тип проекта"));

            context.CreateVmConfigOnBase<SibTaskReportStatus>("DictMenu", "SibTaskReportStatusMenu")
                .Service<IDictHistoryService<SibTaskReportStatus>>()
               .Title("Статус отчета по задаче")
               .ListView(x => x.Title("Статусы отчетов по задачам"))
               .DetailView(x => x.Title("Статус отчета по задаче"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SibTaskStatus>("DictMenu", "SibTaskStatusMenu")
                 .Service<IDictHistoryService<SibTaskStatus>>()
                .Title("Статус задачи")
                .ListView(x => x.Title("Статусы задачи"))
                .DetailView(x => x.Title("Статус задачи"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SibCountry>("DictMenu", "SibCountryMenu")
               .Service<IDictHistoryService<SibCountry>>()
               .Title("Страна")
               .ListView(x => x.Title("Страна")
                .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
               .DetailView(x => x.Title("Страна")
               .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SibRegion>("DictMenu", "SibRegionMenu")
               .Service<ISibRegionHistoryService>()
               .Title("Субъект РФ")
               .ListView(x => x.Title("Субъекты РФ")
               .Columns(cols => cols
                    .Add(col => col.PublishCode, ac => ac.Visible(true).Order(1))
                        .Add(col => col.Name, ac => ac.Visible(true).Order(2))
                        .Add(col => col.Country, ac => ac.Visible(true).Order(3))
                        .Add(col => col.FederalDistrict, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DateFrom, ac => ac.Order(5))
                        .Add(col => col.DateTo, ac => ac.Order(6))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(7))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(8))
                ))
               .DetailView(x => x.Title("Субъект РФ")
               .Editors(ed => ed
                .Add(e => e.Country, ac => ac.Visible(true))
                .Add(e => e.FederalDistrict, ac => ac.Visible(true))
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
               ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SibCityNSI>("DictMenu", "SibCityNSIMenu")
                .Service<ISibCityNSIHistoryService>()
                .Title("Город")
                .ListView(x => x.Title("Города")
                    .Columns(cf => cf
                        .Add(col => col.PublishCode, ac => ac.Visible(true).Order(1))
                        .Add(col => col.Name, ac => ac.Visible(true).Order(2))
                        .Add(col => col.Country, ac => ac.Visible(true).Order(3))
                        .Add(col => col.FederalDistrict, ac => ac.Visible(true).Order(4))
                        .Add(col => col.SibRegion, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DateFrom, ac => ac.Order(6))
                        .Add(col => col.DateTo, ac => ac.Order(7))
                        .Add(col => col.DictObjectState, ac => ac.Visible(false))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                    ))
                .DetailView(x => x.Title("Город")
                    .Editors(ef => ef
                        .Add(ed => ed.Country)
                        .Add(ed => ed.FederalDistrict)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                    ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SignType>("DictMenu", "SignTypeMenu")
             .Service<IDictHistoryService<SignType>>()
            .Title("Тип товарного знака")
            .ListView(x => x.Title("Типы товарных знаков"))
            .LookupProperty(x => x.Text(t => t.Name))
            .DetailView(x => x.Title("Тип товарного знака"));


            context.CreateVmConfigOnBase<SocietyCategory1>("DictMenu", "SocietyCategory1Menu")
             .Service<IDictHistoryService<SocietyCategory1>>()
               .Title("Категория ОГ 1")
               .ListView(x => x.Title("Категории ОГ 1"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Категория ОГ 1"));

            context.CreateVmConfigOnBase<SocietyCategory2>("DictMenu", "SocietyCategory2Menu")
             .Service<IDictHistoryService<SocietyCategory2>>()
               .Title("Категория ОГ 2")
               .ListView(x => x.Title("Категории ОГ 2"))
               .LookupProperty(x => x.Text(t => t.Name))
               .DetailView(x => x.Title("Категория ОГ 2"));

            context.CreateVmConfigOnBase<SourceInformationType>("DictMenu", "SourceInformationTypeMenu")
                .Service<IDictHistoryService<SourceInformationType>>()
                .Title("Тип источника информации")
                .ListView(x => x.Title("Типы источников информации"))
                .DetailView(x => x.Title("Тип источника информации"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<StageOfCompletion>("DictMenu", "StageOfCompletionMenu")
                 .Service<IDictHistoryService<StageOfCompletion>>()
                .Title("Стадия готовности объекта")
                .ListView(x => x.Title("Стадия готовности объекта"))
                .DetailView(x => x.Title("Стадия готовности объекта"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<StatusConstruction>("DictMenu", "StatusConstructionMenu")
                .Service<IDictHistoryService<StatusConstruction>>()
                .Title("Статус строительства")
                .ListView(x => x.Title("Статусы строительства"))
                .DetailView(x => x.Title("Статус строительства"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SubjectActivityKind>("DictMenu", "SubjectObject.SubjectActivityKindMenu")
               .Service<IDictHistoryService<SubjectActivityKind>>()
               .Title("Вид делового партнера")
               .ListView(x => x.Title("Виды деловых партнеров"))
               .DetailView(x => x.Title("Вид делового партнера"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SubjectKind>("DictMenu", "SubjectObject.SubjectKindMenu")
                .Service<IDictHistoryService<SubjectKind>>()
                .Title("Вид делового партнера")
                .ListView(x => x.Title("Виды деловых партнеров"))
                .DetailView(x => x.Title("Вид делового партнера"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SubjectType>("DictMenu", "SubjectObject.SubjectTypeMenu")
                .Service<IDictHistoryService<SubjectType>>()
                .Title("Тип делового партнера")
                .ListView(x => x.Title("Тип делового партнера")
                .Columns(cols => cols
                    .Add(col => col.DictObjectState, ac => ac.Visible(true))
                    .Add(col => col.DictObjectStatus, ac => ac.Visible(true))
                ))
                .DetailView(x => x.Title("Тип делового партнера")
                .Editors(ed => ed
                .Add(e => e.DictObjectStatus, ac => ac.Visible(true))
                .Add(e => e.DictObjectState, ac => ac.Visible(true))
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            //context.CreateVmConfigOnBase<SocietyDept>("DictMenu", "SocietyDeptMenu")
            //    .Service<IDictHistoryService<SocietyDept>>()
            //    .Title("Структурное подразделение ОГ")
            //    .ListView(x => x.Title("Структурные подразделения ОГ"))
            //    .DetailView(x => x.Title("Структурное подразделение ОГ"))
            //    .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxBase>("DictMenu", "TaxBaseMenu")
                 .Service<IDictHistoryService<TaxBase>>()
                .Title("Выбор налоговой базы")
                .ListView(x => x.Title("Выбор налоговой базы"))
                .DetailView(x => x.Title("Выбор налоговой базы"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<TaxNumberInSheet>("DictMenu", "TaxNumberInSheetMenu")
                .Service<IDictHistoryService<TaxNumberInSheet>>()
                .Title("Налоговый номер в соотв. налоговой ведомости")
                .ListView(x => x.Title("Налоговые номера в соотв. налоговых ведомостях"))
                .DetailView(x => x.Title("Налоговый номер в соотв. налоговой ведомости"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TypeData>("DictMenu", "TypeDataMenu")
                .Service<IDictHistoryService<TypeData>>()
                .Title("Тип данных")
                .ListView(x => x.Title("Типы данных"))
                .DetailView(x => x.Title("Тип данных"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<UnitOfCompany>("DictMenu", "UnitOfCompanyMenu")
                .Service<IDictHistoryService<UnitOfCompany>>()
                .Title("Структурное подразделение")
                .ListView(x => x.Title("Структурные подразделения"))
                .DetailView(x => x.Title("Структурное подразделение"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObjectStatus>("DictMenu", "DictObjectStatusMenu")
             .Service<IDictHistoryService<DictObjectStatus>>()
             .Title("Статус элемента справочника")
             .ListView(x => x.Title("Статусы элементов справочника"))
             .DetailView(x => x.Title("Статус элемента справочника"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObjectState>("DictMenu", "DictObjectStateMenu")
             .Service<IDictHistoryService<DictObjectState>>()
             .Title("Состояние элемента справочника")
             .ListView(x => x.Title("Состояния элементов справочника"))
             .DetailView(x => x.Title("Состояние элемента справочника"))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<HolidaysCalendar>("HolidaysCalendarMenu")
           .Service<IDictHistoryService<HolidaysCalendar>>()
           .Title("Праздничный или выходной день")
           .ListView(x => x.Title("Праздничные и выходные дни")
           .Title("Праздничные и выходные дни")
                .Columns(c => c

                    .Add(a => a.Name, a => a.Visible(true).Title("Наименование"))
                    .Add(a => a.DateFrom, a => a.Visible(true).Title("Дата с").Format("DD-MM-YYY"))
                    .Add(a => a.DateTo, a => a.Visible(true).Title("Дата по"))
                    .Add(a => a.Code, a => a.Visible(false))
                    .Add(a => a.PublishCode, a => a.Visible(false))
                    .Add(a => a.DictObjectStateID, a => a.Visible(false))
                    .Add(a => a.DictObjectStatusID, a => a.Visible(false))
                    .Add(a => a.ID, a => a.Visible(false))
                    .Add(a => a.Hidden, a => a.Visible(false))
                    .Add(a => a.SortOrder, a => a.Visible(false))
                )
           )
           .DetailView(x => x.Title("Праздничный или выходной день"))
           .LookupProperty(x => x.Text(t => t.Name))

             .DetailView(x => x
                .Title("Праздничный или выходной день")
                .Editors(e => e
                    .Add(a => a.Name, a => a.Visible(true).Title("Наименование"))
                    .Add(a => a.DateFrom, a => a.Visible(true).Title("Дата с"))
                    .Add(a => a.DateTo, a => a.Visible(true).Title("Дата по"))
                    .Add(a => a.Code, a => a.Visible(false))
                    .Add(a => a.PublishCode, a => a.Visible(false))
                )
                .DefaultSettings((uow, r, commonEditorViewModel) =>
                {
                    commonEditorViewModel.Required(re => re.DateFrom);
                    commonEditorViewModel.Required(re => re.DateTo);
                }))
             .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<HolidaysCalendar>("DictMenu", "HolidaysCalendar")
             .Service<IDictObjectService<HolidaysCalendar>>()
             .Title("Праздничный или выходной день")
             .ListView(x => x
                .Title("Праздничные и выходные дни")
                .Columns(c => c

                    .Add(a => a.Name)
                    .Add(a => a.DateFrom)
                    .Add(a => a.DateTo)
                )

             )
             .DetailView(x => x
                .Title("Праздничный или выходной день")
                .Editors(e => e
                    .Add(a => a.Code, a => a.Visible(false))
                    .Add(a => a.PublishCode, a => a.Visible(false))
                )
                .DefaultSettings((uow, r, commonEditorViewModel) =>
                {
                    commonEditorViewModel.Required(re => re.DateFrom);
                    commonEditorViewModel.Required(re => re.DateTo);
                }))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxFreeLand>("DictMenu", "TaxFreeLandMenu")
                .Service<IDictHistoryService<TaxFreeLand>>()
                .Title("Код налоговой льготы в виде освобождения от налогообложения.Земельный налог")
                .ListView(builder => builder.Title("Код налоговой льготы в виде освобождения от налогообложения.Земельный налог"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде освобождения от налогообложения.Земельный налог"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<TaxFreeTS>("DictMenu", "TaxFreeTSMenu")
                .Service<IDictHistoryService<TaxFreeTS>>()
                .Title("Код налоговой льготы в виде освобождения от налогообложения.Транспортный налог")
                .ListView(builder => builder.Title("Код налоговой льготы в виде освобождения от налогообложения.Транспортный налог"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде освобождения от налогообложения.Транспортный налог"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<TaxRateLower>("DictMenu", "TaxRateLowerMenu")
                .Service<IDictHistoryService<TaxRateLower>>()
                .Title("Код налоговой льготы в виде понижения налоговой ставки.Налог на имущество")
                .ListView(builder => builder.Title("Код налоговой льготы в виде понижения налоговой ставки.Налог на имущество"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде понижения налоговой ставки.Налог на имущество"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<TaxRateLowerLand>("DictMenu", "TaxRateLowerLandMenu")
                .Service<IDictHistoryService<TaxRateLowerLand>>()
                .Title("Код налоговой льготы в виде снижения налоговой ставки.Земельный налог")
                .ListView(builder => builder.Title("Код налоговой льготы в виде снижения налоговой ставки.Земельный налог"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде снижения налоговой ставки.Земельный налог"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<TaxRateLowerTS>("DictMenu", "TaxRateLowerTSMenu")
                .Service<IDictHistoryService<TaxRateLowerTS>>()
                .Title("Код налоговой льготы в виде снижения налоговой ставки.Транспортный налог")
                .ListView(builder => builder.Title("Код налоговой льготы в виде снижения налоговой ставки.Транспортный налог"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде снижения налоговой ставки.Транспортный налог"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<TaxLowerLand>("DictMenu", "TaxLowerLandMenu")
                .Service<IDictHistoryService<TaxLowerLand>>()
                .Title("Код налоговой льготы в виде уменьшения суммы налога.Земельный налог")
                .ListView(builder => builder.Title("Код налоговой льготы в виде уменьшения суммы налога.Земельный налог"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде уменьшения суммы налога.Земельный налог"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<TaxLower>("DictMenu", "TaxLowerMenu")
                .Service<IDictHistoryService<TaxLower>>()
                .Title("Код налоговой льготы в виде уменьшения суммы налога.Налог на имущество")
                .ListView(builder => builder.Title("Код налоговой льготы в виде уменьшения суммы налога.Налог на имущество"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде уменьшения суммы налога.Налог на имущество"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<TaxLowerTS>("DictMenu", "TaxLowerTSMenu")
                .Service<IDictHistoryService<TaxLowerTS>>()
                .Title("Код налоговой льготы в виде уменьшения суммы налога.Транспортный налог")
                .ListView(builder => builder.Title("Код налоговой льготы в виде уменьшения суммы налога.Транспортный налог"))
                .DetailView(builder => builder.Title("Код налоговой льготы в виде уменьшения суммы налога.Транспортный налог"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<GroupConsolidationMSFO>("DictMenu", "GroupConsolidationMSFOMenu")
                .Service<IDictHistoryService<GroupConsolidationMSFO>>()
                .Title("Группа учета(вид) ОС / группа консолидации по МСФО")
                .ListView(builder => builder.Title("Группа учета(вид) ОС / группа консолидации по МСФО"))
                .DetailView(builder => builder.Title("Группа учета(вид) ОС / группа консолидации по МСФО"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<VehicleKindCode>("DictMenu", "VehicleKindCodeMenu")
                .Service<IDictHistoryService<VehicleKindCode>>()
                .Title("Коды вида транспортного средства")
                .ListView(builder => builder.Title("Коды вида транспортного средства"))
                .DetailView(builder => builder.Title("Коды вида транспортного средства"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<SubjectActivityKind>("DictMenu", "SubjectActivityKindMenu")
                .Service<IDictHistoryService<SubjectActivityKind>>()
                .Title("Вид деятельности делового партнера")
                .ListView(builder => builder.Title("Вид деятельности делового партнера"))
                .DetailView(builder => builder.Title("Вид деятельности делового партнера"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<SubjectKind>("DictMenu", "SubjectKindMenu")
                .Service<IDictHistoryService<SubjectKind>>()
                .Title("Вид делового партнера")
                .ListView(builder => builder.Title("Вид делового партнера"))
                .DetailView(builder => builder.Title("Вид делового партнера"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<ScheduleStateYear>("DictMenu", "ScheduleStateYearMenu")
                .Service<IDictHistoryService<ScheduleStateYear>>()
                .Title("ГГР на год")
                .ListView(builder => builder.Title("ГГР на год"))
                .DetailView(builder => builder.Title("ГГР на год"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<GroupConsolidationRSBU>("DictMenu", "GroupConsolidationRSBUMenu")
                .Service<IDictHistoryService<GroupConsolidationRSBU>>()
                .Title("Группа консолидации")
                .ListView(builder => builder.Title("Группа консолидации"))
                .DetailView(builder => builder.Title("Группа консолидации"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<PeriodNU>("DictMenu", "PeriodNUMenu")
                .Service<IDictHistoryService<PeriodNU>>()
                .Title("Период НУ")
                .ListView(builder => builder.Title("Период НУ"))
                .DetailView(builder => builder.Title("Период НУ"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<NonCoreAssetInventory>("DictMenu", "NonCoreAssetInventoryMenu")
                .Service<IDictHistoryService<NonCoreAssetInventory>>()
                .Title("Реестр ННА")
                .ListView(builder => builder.Title("Реестр ННА"))
                .DetailView(builder => builder.Title("Реестр ННА"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<NonCoreAssetSaleStatus>("DictMenu", "NonCoreAssetSaleStatusMenu")
                .Service<IDictHistoryService<NonCoreAssetSaleStatus>>()
                .Title("Статусы реализации ННА")
                .ListView(builder => builder.Title("Статусы реализации ННА"))
                .DetailView(builder => builder.Title("Статусы реализации ННА"))
                .LookupProperty(lp => lp.Text(t => t.Name));
            context.CreateVmConfigOnBase<NSIType>("DictMenu", "NSITypeMenu")
                .Service<IDictHistoryService<NSIType>>()
                .Title("Тип справочника")
                .ListView(builder => builder.Title("Тип справочника"))
                .DetailView(builder => builder.Title("Тип справочника"))
                .LookupProperty(lp => lp.Text(t => t.Name));


            #endregion

            #region EUSI

            context.CreateVmConfigOnBase<DepreciationMethodRSBU>("DictMenu", "DepreciationMethodRSBUMenu")
               .Service<IDictHistoryService<DepreciationMethodRSBU>>()
               .Title("Метод амортизации РСБУ")
               .ListView(x => x.Title("Методы амортизации РСБУ"))
               .DetailView(x => x.Title("Метод амортизации РСБУ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DepreciationMethodMSFO>("DictMenu", "DepreciationMethodMSFOMenu")
               .Service<IDictHistoryService<DepreciationMethodMSFO>>()
               .Title("Метод амортизации МСФО")
               .ListView(x => x.Title("Методы амортизации МСФО"))
               .DetailView(x => x.Title("Метод амортизации МСФО"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DepreciationMethodNU>("DictMenu", "DepreciationMethodNUMenu")
               .Service<IDictHistoryService<DepreciationMethodNU>>()
               .Title("Метод амортизации НУ")
               .ListView(x => x.Title("Методы амортизации НУ"))
               .DetailView(x => x.Title("Метод амортизации НУ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<DivisibleType>("DictMenu", "DivisibleTypeMenu")
               .Service<IDictHistoryService<DivisibleType>>()
               .Title("Отделимое/неотделимое имущество")
               .ListView(x => x.Title("Отделимое/неотделимое имущество"))
               .DetailView(x => x.Title("Отделимое/неотделимое имущество"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<EcoKlass>("DictMenu", "EcoKlassMenu")
                .Service<IDictHistoryService<EcoKlass>>()
                .Title("Экологический класс")
                .ListView(x => x.Title("Экологический класс"))
                .DetailView(x => x.Title("Экологический класс"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ENAOF>("DictMenu", "ENAOFMenu")
               .Service<IDictHistoryService<ENAOF>>()
               .Title("ЕНАОФ")
               .ListView(x => x.Title("ЕНАОФ"))
               .DetailView(x => x.Title("ЕНАОФ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<EnergyLabel>("DictMenu", "EnergyLabelMenu")
               .Service<IDictHistoryService<EnergyLabel>>()
               .Title("Класс энергетической эффективности")
               .ListView(x => x.Title("Класс энергетической эффективности"))
               .DetailView(x => x.Title("Класс энергетической эффективности"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<HighEnergyEfficientFacility>("DictMenu", $"{nameof(HighEnergyEfficientFacility)}Menu")
                .Service<IDictHistoryService<HighEnergyEfficientFacility>>()
                .Title("Объекты с признаком высокой энергетической эффективности")
                .ListView(x => x.Title("Объекты с признаком высокой энергетической эффективности")
                    .Columns(cols => cols
                        .Add(col => col.Name, a => a.Title("Наименование объекта").Visible(true).Order(1))
                        .Add(col => col.CodeOKOF2, a => a.Visible(true).Order(2))
                        .Add(col => col.ClassNameOKOF2, a => a.Visible(true).Order(3))
                        .Add(col => col.QualityCharacteristic, a => a.Visible(true).Order(4))
                        .Add(col => col.DateFrom, a => a.Visible(true).Order(5))
                        .Add(col => col.DateTo, a => a.Visible(true).Order(6))

                        .Add(col => col.Code, a => a.Visible(false).Order(7))
                        .Add(col => col.PublishCode, a => a.Visible(false).Order(10))
                        .Add(col => col.DictObjectState, a => a.Visible(false).Order(8))
                        .Add(col => col.DictObjectStatus, a => a.Visible(false).Order(9))
                ))
                .DetailView(x => x.Title("Объект с признаком высокой энергетической эффективности")
                    .Editors(edf => edf
                        .Add(ed => ed.Name, a => a.Title("Наименование объекта"))
                        .Add(ed => ed.CodeOKOF2)
                        .Add(ed => ed.ClassNameOKOF2)
                        .Add(ed => ed.QualityCharacteristic)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<HighEnergyEfficientFacilityKP>("DictMenu", $"{nameof(HighEnergyEfficientFacilityKP)}Menu")
                .Service<IDictHistoryService<HighEnergyEfficientFacilityKP>>()
                .Title("Объекты с признаком высокой энергетической эффективности (количественный показатель)")
                .ListView(x => x.Title("Объекты с признаком высокой энергетической эффективности (количественный показатель)")
                    .Columns(cols => cols
                        .Add(col => col.Name, a => a.Title("Наименование объекта").Visible(true).Order(1))
                        .Add(col => col.CodeOKOF2, a => a.Visible(true).Order(2))
                        .Add(col => col.ClassNameOKOF2, a => a.Visible(true).Order(3))
                        .Add(col => col.EssentialCharacteristics, a => a.Visible(true).Order(4))
                        .Add(col => col.KPName, a => a.Visible(true).Order(5))
                        .Add(col => col.KPUnit, a => a.Visible(true).Order(6))
                        .Add(col => col.KPValue, a => a.Visible(true).Order(7))
                        .Add(col => col.DateFrom, a => a.Visible(true).Order(8))
                        .Add(col => col.DateTo, a => a.Visible(true).Order(9))

                        .Add(col => col.Code, a => a.Visible(false).Order(10))
                        .Add(col => col.PublishCode, a => a.Visible(false).Order(10))
                        .Add(col => col.DictObjectState, a => a.Visible(false).Order(11))
                        .Add(col => col.DictObjectStatus, a => a.Visible(false).Order(12))
                ))
                .DetailView(x => x.Title("Объект с признаком высокой энергетической эффективности (количественный показатель)")
                    .Editors(edf => edf
                        .Add(ed => ed.Name, a => a.Title("Наименование объекта"))
                        .Add(ed => ed.CodeOKOF2)
                        .Add(ed => ed.ClassNameOKOF2)
                        .Add(ed => ed.EssentialCharacteristics)
                        .Add(ed => ed.KPName)
                        .Add(ed => ed.KPUnit)
                        .Add(ed => ed.KPValue)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<EstateDefinitionType>("DictMenu", "EstateDefinitionTypeMenu")
               .Service<IDictHistoryService<EstateDefinitionType>>()
               .Title("Тип ОИ")
               .ListView(x => x.Title("Тип ОИ"))
               .DetailView(x => x.Title("Тип ОИ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<EstateMovableNSI>("DictMenu", "EstateMovableNSIMenu")
               .Service<IDictHistoryService<EstateMovableNSI>>()
               .Title("Признак движимое/недвижимое имущество")
               .ListView(x => x.Title("Признак движимое/недвижимое имущество"))
               .DetailView(x => x.Title("Признак движимое/недвижимое имущество"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<GroupConsolidationRSBU>("DictMenu", "GroupConsolidationMenu")
               .Service<IDictHistoryService<GroupConsolidationRSBU>>()
               .Title("Группа консолидации")
               .ListView(x => x.Title("Группа консолидации"))
               .DetailView(x => x.Title("Группа консолидации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<LandPurpose>("DictMenu", "LandPurposeMenu")
               .Service<IDictHistoryService<LandPurpose>>()
               .Title("Назначение ЗУ")
                .ListView(x => x.Title("Назначение ЗУ"))
               .DetailView(x => x.Title("Назначение ЗУ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<LicenseArea>("DictMenu", "LicenseAreaMenu")
                .Service<IDictHistoryService<LicenseArea>>()
                .Title("Лицензионный участок")
                .ListView(x => x.Title("Лицензионный участок"))
                .DetailView(x => x.Title("Лицензионный участок"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<PermittedUseKind>("DictMenu", "PermittedUseKindMenu")
               .Service<IDictHistoryService<PermittedUseKind>>()
               .Title("Вид разрешенного использования")
               .ListView(x => x.Title("Вид разрешенного использования"))
               .DetailView(x => x.Title("Вид разрешенного использования"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<PositionConsolidation>("DictMenu", "PositionConsolidationMenu")
               .Service<IDictHistoryService<PositionConsolidation>>()
               .Title("Позиция консолидации")
               .ListView(x => x.Title("Позиция консолидации"))
               .DetailView(x => x.Title("Позиция консолидации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SubPositionConsolidation>("DictMenu", "SubPositionConsolidationMenu")
               .Service<IDictHistoryService<SubPositionConsolidation>>()
               .Title("Подпозиция консолидации")
               .ListView(x => x.Title("Подпозиция консолидации"))
               .DetailView(x => x.Title("Подпозиция консолидации"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<RentTypeMSFO>("DictMenu", "RentTypeMSFOMenu")
               .Service<IDictHistoryService<RentTypeMSFO>>()
               .Title("Тип аренды МСФО")
               .ListView(x => x.Title("Тип аренды МСФО"))
               .DetailView(x => x.Title("Тип аренды МСФО"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<RentTypeRSBU>("DictMenu", "RentTypeRSBUMenu")
               .Service<IDictHistoryService<RentTypeRSBU>>()
               .Title("Тип аренды РСБУ")
               .ListView(x => x.Title("Тип аренды РСБУ"))
               .DetailView(x => x.Title("Тип аренды РСБУ"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<StateObjectMSFO>("DictMenu", "StateObjectMSFOMenu")
               .Service<IDictHistoryService<StateObjectMSFO>>()
               .Title("Состояние объекта МСФО")
               .ListView(x => x.Title("Состояние объекта МСФО"))
               .DetailView(x => x.Title("Состояние объекта МСФО"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<StateObjectRSBU>("DictMenu", "StateObjectRSBUMenu")
               .Service<IDictHistoryService<StateObjectRSBU>>()
               .Title("Состояние объекта РСБУ")
               .ListView(x => x.Title("Состояние объекта РСБУ"))
               .DetailView(x => x.Title("Состояние объекта РСБУ"))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<StructurePlan>("DictMenu", "StructurePlanMenu")
               .Service<IDictHistoryService<StructurePlan>>()
               .Title("СПП")
               .ListView(x => x.Title("СПП"))
               .DetailView(x => x.Title("СПП"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxRateType>("DictMenu", "TaxRateTypeMenu")
               .Service<IDictHistoryService<TaxRateType>>()
               .Title("Вид налога")
               .ListView(x => x.Title("Вид налога"))
               .DetailView(x => x.Title("Вид налога"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DecisionsDetails>("DictMenu", "DecisionsDetailsMenu")
                .Service<IDictHistoryService<DecisionsDetails>>()
                .Title("Реквизиты решений органов МО Имущество")
                .ListView(x => x.Title("Реквизиты решений органов МО Имущество")
                    .Columns(cf => cf
                        .Add(col => col.Number)
                        .Add(col => col.Date)
                        .Add(col => col.Link)
                    ))
                .DetailView(x => x.Title("Реквизиты решений органов МО Имущество")
                    .Editors(ef => ef
                        .Add(ed => ed.Number)
                        .Add(ed => ed.Date)
                        .Add(ed => ed.Link)
                    ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DecisionsDetailsLand>("DictMenu", "DecisionsDetailsLandMenu")
                .Service<IDictHistoryService<DecisionsDetailsLand>>()
                .Title("Реквизиты решений органов МО ЗУ")
                .ListView(x => x.Title("Реквизиты решений органов МО ЗУ")
                    .Columns(cf => cf
                        .Add(col => col.Number)
                        .Add(col => col.Date)
                        .Add(col => col.Link)
                    ))
                .DetailView(x => x.Title("Реквизиты решений органов МО ЗУ")
                    .Editors(ef => ef
                        .Add(ed => ed.Number)
                        .Add(ed => ed.Date)
                        .Add(ed => ed.Link)
                    ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DecisionsDetailsTS>("DictMenu", "DecisionsDetailsTSMenu")
                .Service<IDictHistoryService<DecisionsDetailsTS>>()
                .Title("Реквизиты решений органов МО ТС")
                .ListView(x => x.Title("Реквизиты решений органов МО ТС")
                    .Columns(cf => cf
                        .Add(col => col.Number)
                        .Add(col => col.Date)
                        .Add(col => col.Link)
                    ))
                .DetailView(x => x.Title("Реквизиты решений органов МО ТС")
                    .Editors(ef => ef
                        .Add(ed => ed.Number)
                        .Add(ed => ed.Date)
                        .Add(ed => ed.Link)
                    ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<SubsoilUser>("DictMenu", "SubsoilUserMenu")
                .Service<IDictObjectService<SubsoilUser>>()
                .Title("Недропользователь")
                .ListView(x => x.Title("Недропользователь"))
                .DetailView(x => x.Title("Недропользователь"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<CostKindRentalPayments>("DictMenu", "CostKindRentalPaymentsMenu")
                .Service<IDictObjectService<CostKindRentalPayments>>()
                .Title("Вид затрат в части арендных платежей")
                .ListView(x => x.Title("Вид затрат в части арендных платежей"))
                .DetailView(x => x.Title("Вид затрат в части арендных платежей"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ObjectLocationRent>("DictMenu", "ObjectLocationRentMenu")
                .Service<IDictObjectService<ObjectLocationRent>>()
                .Title("Местоположения объекта (аренда)")
                .ListView(x => x.Title("Местоположения объекта (аренда)"))
                .DetailView(x => x.Title("Местоположения объекта (аренда)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<AssetHolderRSBU>("DictMenu", "AssetHolderRSBUMenu")
                .Service<IDictObjectService<AssetHolderRSBU>>()
                .Title("На чьем балансе учитывается ОС в РСБУ")
                .ListView(x => x.Title("На чьем балансе учитывается ОС в РСБУ"))
                .DetailView(x => x.Title("На чьем балансе учитывается ОС в РСБУ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<StateObjectRent>("DictMenu", "StateObjectRentMenu")
                .Service<IDictObjectService<StateObjectRent>>()
                .Title("Состояние ОС/НМА (аренда)")
                .ListView(x => x.Title("Состояние ОС/НМА (аренда)"))
                .DetailView(x => x.Title("Состояние ОС/НМА (аренда)"))
                .LookupProperty(x => x.Text(t => t.Name));

            #region Группа справочников Имущество
            context.CreateVmConfigOnBase<TaxRate>("DictMenu", $"{nameof(TaxRate)}Menu")
                .Service<IDictHistoryService<TaxRate>>()
                .Title("Налоговая ставка Имущество")
                .ListView(x => x.Title("Налоговая ставка Имущество")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.Value)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetails)
                        .Add(col => col.CategoryName)
                    ))
                .DetailView(x => x.Title("Налоговая ставка Имущество")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.Name)
                        .Add(ed => ed.Value)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetails)
                        .Add(ed => ed.CategoryName)
                    ))
                .LookupProperty(x => x.Text(t => t.Value));



            context.CreateVmConfigOnBase<TaxFederalExemption>("DictMenu", $"{nameof(TaxFederalExemption)}Menu")
                .Service<IDictObjectService<TaxFederalExemption>>()
                .Title("Федеральные льготы Имущество")
                .ListView(x => x.Title("Федеральные льготы Имущество")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.TaxExemption)
                        .Add(col => col.TaxpayersCategory)
                        .Add(col => col.NumChapterLawRF)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.BasisForGranting)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                ))
                .DetailView(x => x.Title("Федеральные льготы Имущество")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.TaxExemption)
                        .Add(ed => ed.NumChapterLawRF)
                        .Add(ed => ed.NumArticleLawRF)
                        .Add(ed => ed.NumParagraphLawRF)
                        .Add(ed => ed.BasisForGranting)
                        .Add(ed => ed.Value)
                        .Add(ed => ed.Unit)
                        .Add(ed => ed.ConditionForGranting)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxRegionExemption>("DictMenu", $"{nameof(TaxRegionExemption)}Menu")
                .Service<IDictObjectService<TaxRegionExemption>>()
                .Title("Региональные льготы (Имущество)")
                .ListView(x => x.Title("Региональные льготы (Имущество)")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetails)
                        .Add(col => col.TaxExemption)
                        .Add(col => col.TaxpayersCategory)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.NumSubParagraphLawRF)
                        .Add(col => col.BasisForGranting)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                ))
                .DetailView(x => x.Title("Региональные льготы (Имущество)")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetails)
                        .Add(ed => ed.TaxExemption)
                        .Add(ed => ed.NumArticleLawRF)
                        .Add(ed => ed.NumParagraphLawRF)
                        .Add(ed => ed.NumSubParagraphLawRF)
                        .Add(ed => ed.BasisForGranting)
                        .Add(ed => ed.Value)
                        .Add(ed => ed.Unit)
                        .Add(ed => ed.ConditionForGranting)
                ))
                .LookupProperty(x => x.Text(t => t.Name));
            #endregion

            #region Группа справочников ЗУ
            context.CreateVmConfigOnBase<TaxRateLand>("DictMenu", $"{nameof(TaxRateLand)}Menu")
                .Service<IDictHistoryService<TaxRateLand>>()
                .Title("Налоговая ставка ЗУ")
                .ListView(x => x.Title("Налоговая ставка ЗУ")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.OKTMO)
                        .Add(col => col.DecisionsDetailsLand)
                        .Add(col => col.CategoryName)
                        .Add(col => col.Value)
                    ))
                .DetailView(x => x.Title("Налоговая ставка ЗУ")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.OKTMO)
                        .Add(ed => ed.DecisionsDetailsLand)
                        .Add(ed => ed.CategoryName)
                        .Add(ed => ed.Value)
                    ))
                .LookupProperty(x => x.Text(t => t.Value));



            context.CreateVmConfigOnBase<TaxFederalExemptionLand>("DictMenu", $"{nameof(TaxFederalExemptionLand)}Menu")
                .Service<IDictObjectService<TaxFederalExemptionLand>>()
                .Title("Федеральные льготы ЗУ")
                .ListView(x => x.Title("Федеральные льготы ЗУ")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.TaxExemptionLand)
                        .Add(col => col.TaxpayersCategory)
                        .Add(col => col.NumChapterLawRF)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.BasisForGranting)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                ))
                .DetailView(x => x.Title("Федеральные льготы ЗУ")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.TaxExemptionLand)
                        .Add(ed => ed.NumChapterLawRF)
                        .Add(ed => ed.NumArticleLawRF)
                        .Add(ed => ed.NumParagraphLawRF)
                        .Add(ed => ed.BasisForGranting)
                        .Add(ed => ed.Value)
                        .Add(ed => ed.Unit)
                        .Add(ed => ed.ConditionForGranting)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxRegionExemptionLand>("DictMenu", $"{nameof(TaxRegionExemptionLand)}Menu")
                .Service<IDictObjectService<TaxRegionExemptionLand>>()
                .Title("Региональные льготы ЗУ")
                .ListView(x => x.Title("Региональные льготы ЗУ")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetailsLand)
                        .Add(col => col.TaxExemptionLand)
                        .Add(col => col.OKTMO)
                        .Add(col => col.TaxpayersCategory)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.NumSubParagraphLawRF)
                        .Add(col => col.BasisForGranting)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                ))
                .DetailView(x => x.Title("Региональные льготы ЗУ")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetailsLand)
                        .Add(ed => ed.TaxExemptionLand)
                        .Add(ed => ed.OKTMO)
                        .Add(ed => ed.NumArticleLawRF)
                        .Add(ed => ed.NumParagraphLawRF)
                        .Add(ed => ed.NumSubParagraphLawRF)
                        .Add(ed => ed.BasisForGranting)
                        .Add(ed => ed.Value)
                        .Add(ed => ed.Unit)
                        .Add(ed => ed.ConditionForGranting)
                ))
                .LookupProperty(x => x.Text(t => t.Name));
            #endregion

            #region Группа справочников ТС
            context.CreateVmConfigOnBase<TaxRateTS>("DictMenu", $"{nameof(TaxRateTS)}Menu")
                .Service<IDictHistoryService<TaxRateTS>>()
                .Title("Налоговая ставка ТС")
                .ListView(x => x.Title("Налоговая ставка ТС")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetailsTS)
                        .Add(col => col.TaxObjectName)
                        .Add(col => col.MinPower)
                        .Add(col => col.MaxPower)
                        .Add(col => col.Value)
                    ))
                .DetailView(x => x.Title("Налоговая ставка ТС")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetailsTS)
                        .Add(ed => ed.TaxObjectName)
                        .Add(ed => ed.MinPower)
                        .Add(ed => ed.MaxPower)
                        .Add(ed => ed.Value)
                    ))
                .LookupProperty(x => x.Text(t => t.Value));

            context.CreateVmConfigOnBase<TaxDeductionTS>("DictMenu", $"{nameof(TaxDeductionTS)}Menu")
                .Service<IDictHistoryService<TaxDeductionTS>>()
                .Title("Налоговые вычеты (ТС)")
                .ListView(x => x.Title("Налоговые вычеты (ТС)")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.SibRegion)
                        .Add(col => col.TaxExemptionTS)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.NumSubParagraphLawRF)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                    )
                    )
                .DetailView(x => x.Title("Налоговые вычеты (ТС)")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.TaxExemptionTS)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.NumSubParagraphLawRF)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                    ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxExemption>("DictMenu", $"{nameof(TaxExemption)}Menu")
                .Service<IDictObjectService<TaxExemption>>()
                .Title("Код налоговой льготы Имущество")
                .ListView(x => x.Title("Код налоговой льготы Имущество")
                    .Columns(cf => cf
                        .Add(col => col.Basis)
                ))
                .DetailView(x => x.Title("Код налоговой льготы Имущество")
                    .Editors(ef => ef
                        .Add(ed => ed.Basis)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxExemptionLand>("DictMenu", $"{nameof(TaxExemptionLand)}Menu")
                .Service<IDictObjectService<TaxExemptionLand>>()
                .Title("Код налоговой льготы ЗУ")
                .ListView(x => x.Title("Код налоговой льготы ЗУ")
                    .Columns(cf => cf
                        .Add(col => col.Basis)
                ))
                .DetailView(x => x.Title("Код налоговой льготы ЗУ")
                    .Editors(ef => ef
                        .Add(ed => ed.Basis)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxExemptionTS>("DictMenu", $"{nameof(TaxExemptionTS)}Menu")
                .Service<IDictObjectService<TaxExemptionTS>>()
                .Title("Код налоговой льготы ТС")
                .ListView(x => x.Title("Код налоговой льготы ТС")
                    .Columns(cf => cf
                        .Add(col => col.Basis)
                ))
                .DetailView(x => x.Title("Код налоговой льготы ТС")
                    .Editors(ef => ef
                        .Add(ed => ed.Basis)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxFederalExemptionTS>("DictMenu", $"{nameof(TaxFederalExemptionTS)}Menu")
                .Service<IDictObjectService<TaxFederalExemptionTS>>()
                .Title("Федеральные льготы ТС")
                .ListView(x => x.Title("Федеральные льготы ТС")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.TaxpayersCategory)
                        .Add(col => col.NumChapterLawRF)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.BasisForGranting)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                ))
                .DetailView(x => x.Title("Федеральные льготы ТС")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.NumChapterLawRF)
                        .Add(ed => ed.NumArticleLawRF)
                        .Add(ed => ed.NumParagraphLawRF)
                        .Add(ed => ed.BasisForGranting)
                        .Add(ed => ed.Value)
                        .Add(ed => ed.Unit)
                        .Add(ed => ed.ConditionForGranting)
                ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxRegionExemptionTS>("DictMenu", $"{nameof(TaxRegionExemptionTS)}Menu")
                .Service<IDictObjectService<TaxRegionExemptionTS>>()
                .Title("Региональные льготы ТС")
                .ListView(x => x.Title("Региональные льготы ТС")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetailsTS)
                        .Add(col => col.TaxExemptionTS)
                        .Add(col => col.TaxpayersCategory)
                        .Add(col => col.NumArticleLawRF)
                        .Add(col => col.NumParagraphLawRF)
                        .Add(col => col.NumSubParagraphLawRF)
                        .Add(col => col.ExemptionCodeSubjectRF)
                        .Add(col => col.BasisForGranting)
                        .Add(col => col.Value)
                        .Add(col => col.Unit)
                        .Add(col => col.ConditionForGranting)
                ))
                .DetailView(x => x.Title("Региональные льготы ТС")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetailsTS)
                        .Add(ed => ed.TaxExemptionTS)
                        .Add(ed => ed.NumArticleLawRF)
                        .Add(ed => ed.NumParagraphLawRF)
                        .Add(ed => ed.NumSubParagraphLawRF)
                        .Add(ed => ed.ExemptionCodeSubjectRF)
                        .Add(ed => ed.BasisForGranting)
                        .Add(ed => ed.Value)
                        .Add(ed => ed.Unit)
                        .Add(ed => ed.ConditionForGranting)
                ))
                .LookupProperty(x => x.Text(t => t.Name));
            #endregion

            context.CreateVmConfigOnBase<TermOfPymentTaxRate>("DictMenu", "TermOfPymentTaxRateMenu")
               .Service<IDictHistoryService<TermOfPymentTaxRate>>()
               .Title("Срок уплаты авансовых платежей и налога (Имущество)")
               .ListView(x => x.Title("Срок уплаты авансовых платежей и налога (Имущество)")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetails)
                    ))
               .DetailView(x => x.Title("Срок уплаты авансовых платежей и налога (Имущество)")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetails)
                    ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TermOfPymentTaxRateLand>("DictMenu", "TermOfPymentTaxRateLandMenu")
               .Service<IDictHistoryService<TermOfPymentTaxRateLand>>()
               .Title("Срок уплаты авансовых платежей и налога (Земля)")
               .ListView(x => x.Title("Срок уплаты авансовых платежей и налога (Земля)")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetailsLand)
                        .Add(col => col.OKTMO)
                    ))
               .DetailView(x => x.Title("Срок уплаты авансовых платежей и налога (Земля)")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetailsLand)
                        .Add(ed => ed.OKTMO)
                    ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TermOfPymentTaxRateTS>("DictMenu", "TermOfPymentTaxRateTSMenu")
               .Service<IDictHistoryService<TermOfPymentTaxRateTS>>()
               .Title("Срок уплаты авансовых платежей и налога (Транспорт)")
               .ListView(x => x.Title("Срок уплаты авансовых платежей и налога (Транспорт)")
                    .Columns(cf => cf
                        .Add(col => col.TaxName)
                        .Add(col => col.TaxPeriod)
                        .Add(col => col.SibRegion)
                        .Add(col => col.DecisionsDetailsTS)
                    ))
               .DetailView(x => x.Title("Срок уплаты авансовых платежей и налога (Транспорт)")
                    .Editors(ef => ef
                        .Add(ed => ed.TaxName)
                        .Add(ed => ed.TaxPeriod)
                        .Add(ed => ed.SibRegion)
                        .Add(ed => ed.DecisionsDetailsTS)
                    ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<VehicleClass>("DictMenu", "VehicleClassMenu")
               .Service<IDictHistoryService<VehicleClass>>()
               .Title("Единый классификатор транспортных средств")
               .ListView(x => x.Title("Единый классификатор транспортных средств"))
               .DetailView(x => x.Title("Единый классификатор транспортных средств"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<VehicleLabel>("DictMenu", "VehicleLabelMenu")
               .Service<IDictHistoryService<VehicleLabel>>()
               .Title("Класс ТС")
               .ListView(x => x.Title("Класс ТС"))
               .DetailView(x => x.Title("Класс ТС"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<CorpProp.Entities.NSI.VehicleModel>("DictMenu", "VehicleModelMenu")
               .Service<IDictHistoryService<CorpProp.Entities.NSI.VehicleModel>>()
               .Title("Класс ТС")
               .ListView(x => x.Title("Класс ТС"))
               .DetailView(x => x.Title("Класс ТС"))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxVehicleKindCode>("DictMenu", "TaxVehicleKindCodeMenu")
                .Service<IDictHistoryService<TaxVehicleKindCode>>()
                .Title("Код вида ТС")
                .ListView(x => x.Title("Код вида ТС"))
                .DetailView(x => x.Title("Код вида ТС)"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxName>("DictMenu", "TaxNameMenu")
                .Service<IDictHistoryService<TaxName>>()
                .Title("Наименование налога")
                .ListView(x => x.Title("Наименование налога"))
                .DetailView(x => x.Title("Наименование налога"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxReportPeriod>("DictMenu", "TaxReportPeriodMenu")
                .Service<IDictHistoryService<TaxReportPeriod>>()
                .Title("Отчетный период по налогу")
                .ListView(x => x.Title("Отчетный период по налогу"))
                .DetailView(x => x.Title("Отчетный период по налогу"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<TaxPeriod>("DictMenu", "TaxPeriodMenu")
                .Service<IDictHistoryService<TaxPeriod>>()
                .Title("Налоговый период")
                .ListView(x => x.Title("Налоговый период"))
                .DetailView(x => x.Title("Налоговый период"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BoostOrReductionFactor>("DictMenu", "BoostOrReductionFactorMenu")
                .Service<IDictHistoryService<BoostOrReductionFactor>>()
                .Title("Повышающий/понижающий коэффициент")
                .ListView(x => x.Title("Повышающий/понижающий коэффициент"))
                .DetailView(x => x.Title("Повышающий/понижающий коэффициент"))
                .LookupProperty(x => x.Text(t => t.Name));

            #endregion//END EUSI REGION
        }
    }
}
