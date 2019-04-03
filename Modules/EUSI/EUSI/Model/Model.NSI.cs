using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Services.Base;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using EUSI.Services.Estate;
using System.Linq;
using Base.Attributes;
using EUSI.Entities.Models;

namespace EUSI.Model
{
    public static class NSIModel
    {

        public static void Init(IInitializerContext context)
        {

            context.ModifyVmConfig<DictObject>()
                .DetailView(builder => builder
                .Editors(factory => factory.Add(o => o.PublishCode, editorBuilder => editorBuilder.Visible(false))));

            context.ModifyVmConfig<Consolidation>()
                   .DetailView(dv => dv.Editors(eds => eds
                   .Add(ed => ed.TypeAccounting, ac => ac.Visible(true))
                   .Add(ed => ed.INN, ac => ac.Visible(true))

                   ))
                   .ListView(lv => lv.Columns(col => col
                   .Add(c => c.Code, ac => ac.Visible(true))
                   .Add(c => c.TypeAccounting, ac => ac.Visible(true))
                   .Add(ed => ed.INN, ac => ac.Visible(true))
                   ))
                   .LookupProperty(x => x.Text(t => t.Code));

            context.ModifyVmConfig<Consolidation>("ConsolidationMenu")
                  .DetailView(dv => dv.Editors(eds => eds
                    .Add(ed => ed.TypeAccounting, ac => ac.Visible(true))
                    .Add(ed => ed.ConnectToEUSI, ac => ac.Visible(true))
                    .AddManyToManyLeftAssociation<ConsolidationAndReportMonitoringEventType>("Consolidation_ReportMonitoringEventType", y => y.TabName("Типы контролей").Mnemonic("ReportMonitoringEventTypeForConsolidation"))

                  ))
                  .ListView(lv => lv.Columns(col => col
                    .Add(c => c.TypeAccounting, ac => ac.Visible(true))
                   .Add(c => c.ConnectToEUSI, ac => ac.Visible(true))
                  ))
                  .LookupProperty(x => x.Text(t => t.Code));

            context.ModifyVmConfig<PositionConsolidation>()
                  .ListView(lv => lv.Columns(col => col.Add(c => c.Code, ac => ac.Visible(true))))
                  .LookupProperty(x => x.Text(t => t.Code));

            context.ModifyVmConfig<EstateType>()
                  .ListView(lv => lv.Columns(col => col.Add(c => c.Code, ac => ac.Visible(true))))
                  .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<EstateType>(nameof(EstateType), "EstateTypeFiltered")
                .Service<IEstateTypeCustomService>();

            context.ModifyVmConfig<OKTMO>()
                    .ListView(lv => lv.Columns(col => col.Add(c => c.Code, ac => ac.Visible(true))))
                   .LookupProperty(x => x.Text(t => t.Code));
            context.ModifyVmConfig<OKOF2014>()
                    .Title("ОКОФ")
                    .ListView(lv => lv.Title("ОКОФ").Columns(col => col.Add(c => c.Code, ac => ac.Visible(true))))
                    .DetailView(dv => dv.Title("ОКОФ").Editors(eds => eds
                    .AddOneToManyAssociation<AddonOKOF2014>("AddonOKOF2014_OKOF2014", edt => edt
                            .Title("Доп. коды. ОКОФ2")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .TabName("Доп. коды. ОКОФ2")
                            .Create((uofw, entity, id) =>
                            {
                                entity.OKOF2014ID = id;
                                entity.OKOF2014 = uofw.GetRepository<OKOF2014>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.OKOF2014 = null;
                                entity.OKOF2014ID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.OKOF2014ID == id && !w.Hidden))
                        )
                    ))
                   .LookupProperty(x => x.Text(t => t.Code));


            context.ModifyVmConfig<LandType>()
                  .Title("Тп ЗУ")
                  .ListView(lv => lv.Title("Тип ЗУ"))
                  .DetailView(dv => dv.Title("Тип ЗУ"));

            context.CreateVmConfigOnBase<DictObject, AddonOKOF2014>()
                  .Service<IDictHistoryService<AddonOKOF2014>>()
                  .Title("Доп. код. ОКОФ")
                  .ListView(builder => builder.Title("Новый доп. коды ОКОФ (ОКОФ 2)"))
                  .DetailView(builder => builder.Title("Новые доп. код ОКОФ (ОКОФ 2)"));

            context.CreateVmConfigOnBase<DictObject, Angle>()
                 .Service<IDictHistoryService<Angle>>()
                 .Title("Ракурс")
                 .ListView(builder => builder.Title("Ракурсы"))
                 .DetailView(builder => builder.Title("Ракурс"));

            context.CreateVmConfigOnBase<Angle>("DictMenu", "AngleMenu")
                .Service<IDictHistoryService<Angle>>()
                .Title("Ракурс")
                .ListView(builder => builder.Title("Ракурс"))
                .DetailView(builder => builder.Title("Ракурс"))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ERReceiptReason>()
                 .Service<IDictHistoryService<ERReceiptReason>>()
                 .Title("Способ поступления")
                 .ListView(builder => builder.Title("Способ поступления"))
                 .DetailView(builder => builder.Title("Способ поступления"));

            context.CreateVmConfigOnBase<ERReceiptReason>("DictMenu", "ERReceiptReasonMenu")
                .Service<IDictHistoryService<ERReceiptReason>>()
                .Title("Способ поступления(Заявка на регистрацию)")
                .ListView(builder => builder.Title("Способ поступления(Заявка на регистрацию)"))
                .DetailView(builder => builder.Title("Способ поступления(Заявка на регистрацию)"))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, EstateRegistrationTypeNSI>()
                   .Service<IDictHistoryService<EstateRegistrationTypeNSI>>()
                   .ListView(builder => builder.Title("Вид объекта заявки"))
                   .DetailView(builder => builder.Title("Вид объекта заявки"));

            context.CreateVmConfigOnBase<EstateRegistrationTypeNSI>("DictMenu", "EstateRegistrationTypeNSIMenu")
                .Service<IDictHistoryService<EstateRegistrationTypeNSI>>()
                .Title("Вид объекта заявки")
                .ListView(builder => builder.Title("Вид объекта заявки"))
                .DetailView(builder => builder.Title("Вид объекта заявки"))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, EstateRegistrationStateNSI>()
                   .Service<IDictHistoryService<EstateRegistrationStateNSI>>()
                   .ListView(builder => builder.Title("Статусы заявки на регистрацию ОИ"))
                   .DetailView(builder => builder.Title("Статус заявки на регистрацию ОИ"));

            context.CreateVmConfigOnBase<EstateRegistrationStateNSI>("DictMenu", "EstateRegistrationStateNSIMenu")
                .Service<IDictHistoryService<EstateRegistrationStateNSI>>()
                .Title("Статусы заявки на регистрацию ОИ")
                .ListView(builder => builder.Title("Статусы заявки на регистрацию ОИ"))
                .DetailView(builder => builder.Title("Статусы заявки на регистрацию ОИ"))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, LoadType>()
                 .Service<IDictHistoryService<LoadType>>()
                 .ListView(builder => builder.Title("Вид загрузки"))
                 .DetailView(builder => builder.Title("Вид загрузки"));

            context.CreateVmConfigOnBase<LoadType>("DictMenu", "LoadTypeMenu")
                .Service<IDictHistoryService<LoadType>>()
                .Title("Вид загрузки")
                .ListView(builder => builder.Title("Вид загрузки"))
                .DetailView(builder => builder.Title("Вид загрузки"))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, MovingType>()
                 .Service<IDictHistoryService<MovingType>>()
                 .ListView(builder => builder.Title("Вид движения"))
                 .DetailView(builder => builder.Title("Вид движения"));

            context.CreateVmConfigOnBase<MovingType>("DictMenu", "MovingTypeMenu")
                .Service<IDictHistoryService<MovingType>>()
                .Title("Вид движения")
                .ListView(builder => builder.Title("Вид движения"))
                .DetailView(builder => builder.Title("Вид движения"))
                .LookupProperty(lp => lp.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, OKOFClassNSI>()
                   .Service<IDictHistoryService<OKOFClassNSI>>()
                   .ListView(builder => builder.Title("Класс ОКОФ"))
                   .DetailView(builder => builder.Title("Классы ОКОФ"));

            context.CreateVmConfigOnBase<OKOFClassNSI>("DictMenu", "OKOFClassNSIMenu")
                .Service<IDictHistoryService<OKOFClassNSI>>()
                .Title("Класс ОКОФ")
                .ListView(builder => builder.Title("Класс ОКОФ"))
                .DetailView(builder => builder.Title("Класс ОКОФ"))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<AddonOKOF2014>("DictMenu", "AddonOKOF2014Menu")
                .Service<IDictHistoryService<AddonOKOF2014>>()
                .Title("Новый доп. коды ОКОФ (ОКОФ 2)")
                .ListView(builder => builder.Title("Новый доп. коды ОКОФ (ОКОФ 2)")
                            .Columns(cols => cols.Add(col => col.OKOF2014, ac => ac.Visible(true).Order(4)))
                 )
                .DetailView(builder => builder.Title("Новый доп. коды ОКОФ (ОКОФ 2)")
                            .Editors(eds => eds.Add(ed => ed.OKOF2014, ac => ac.Visible(true))
                ))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, EstateRegistrationOriginator>()
                .Service<IDictObjectService<EstateRegistrationOriginator>>()
                .Title("Инициатор заявки")
                .ListView(builder => builder.Title("Инициаторы заявок")
                .Columns(cols => cols.Clear()
                    .Add(c => c.Name, ac => ac.Visible(true))
                    .Add(c => c.PublishCode, ac => ac.Visible(true))
                    .Add(c => c.LastName, ac => ac.Visible(true))
                    .Add(c => c.FirstName, ac => ac.Visible(true))
                    .Add(c => c.Patronymic, ac => ac.Visible(true))
                    .Add(c => c.ContactEmail, ac => ac.Visible(true))
                    .Add(c => c.Consolidation, ac => ac.Visible(true))
                    .Add(c => c.Code, ac => ac.Visible(false))
                 ))
                .DetailView(builder => builder.Title("Инициатор заявки")
                .Editors(eds => eds.Clear()
                    .Add(c => c.Name, ac => ac.Visible(true))
                    .Add(c => c.PublishCode, ac => ac.Visible(true))
                    .Add(c => c.LastName, ac => ac.Visible(true))
                    .Add(c => c.FirstName, ac => ac.Visible(true))
                    .Add(c => c.Patronymic, ac => ac.Visible(true))
                    .Add(c => c.ContactEmail, ac => ac.Visible(true))
                    .Add(c => c.Consolidation, ac => ac.Visible(true)))
                )
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<EstateRegistrationOriginator>("DictMenu", "EstateRegistartionOriginatorMenu")
                .Service<IDictHistoryService<EstateRegistrationOriginator>>()
                .Title("Инициатор заявки")
                .ListView(builder => builder.Title("Инициаторы заявок")
                .Columns(cols => cols.Clear()
                    .Add(c => c.Name, ac => ac.Visible(true))
                    .Add(c => c.PublishCode, ac => ac.Visible(true))
                    .Add(c => c.LastName, ac => ac.Visible(true))
                    .Add(c => c.FirstName, ac => ac.Visible(true))
                    .Add(c => c.Patronymic, ac => ac.Visible(true))
                    .Add(c => c.ContactEmail, ac => ac.Visible(true))
                    .Add(c => c.Consolidation, ac => ac.Visible(true))
                    .Add(c => c.Code, ac => ac.Visible(false))
                 ))
                .DetailView(builder => builder.Title("Инициатор заявки")
                .Editors(eds => eds.Clear()
                    .Add(c => c.Name, ac => ac.Visible(true))
                    .Add(c => c.PublishCode, ac => ac.Visible(true))
                    .Add(c => c.LastName, ac => ac.Visible(true))
                    .Add(c => c.FirstName, ac => ac.Visible(true))
                    .Add(c => c.Patronymic, ac => ac.Visible(true))
                    .Add(c => c.ContactEmail, ac => ac.Visible(true))
                    .Add(c => c.Consolidation, ac => ac.Visible(true)))
                )
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ReportMonitoringEventType>()
               .Service<IDictObjectService<ReportMonitoringEventType>>()
               .ListView(builder => builder.Title("Типы событий журнала выполнения контролей")
                  .Columns(cols => cols.Clear()
                        .Add(col => col.Code, ac => ac.Visible(true).Order(1))
                        .Add(col => col.Name, ac => ac.Visible(true).Order(2))
                        .Add(col => col.ReportMonitoringEventKind, ac => ac.Visible(true).Order(3))
                        .Add(col => col.UseAdjournmentMonitor, ac => ac.Visible(true).Order(4))
                        .Add(col => col.EventPeriodicity, ac => ac.Visible(true).Order(5))
                        .Add(col => col.PlanDayOfMonth, ac => ac.Visible(true).Order(6))
                        .Add(col => col.Note, ac => ac.Order(7))
                        .Add(col => col.SortIndex, ac => ac.Visible(false).Order(8))
               ))
               .DetailView(builder => builder.Title("Тип события журнала выполнения контролей")
                  .Editors(eds => eds.Clear()
                        .Add(ed => ed.Code, ac => ac.Visible(true).Order(1))
                        .Add(ed => ed.Name, ac => ac.Visible(true).Order(2))
                        .Add(ed => ed.ReportMonitoringEventKind, ac => ac.Visible(true).Order(3))
                        .Add(ed => ed.UseAdjournmentMonitor, ac => ac.IsRequired(false).Visible(true).Order(4))
                        .Add(col => col.EventPeriodicity, ac => ac.Visible(true).Order(5))
                        .Add(ed => ed.PlanDayOfMonth, ac => ac.Visible(true).Order(6))
                        .Add(ed => ed.Note, ac => ac.Visible(true).Order(7))
                        .Add(ed => ed.SortIndex, ac => ac.Visible(false).Order(8))
                        .AddManyToManyRigthAssociation<ConsolidationAndReportMonitoringEventType>("ReportMonitoringEventType_Consolidation", y => y.TabName("БЕ"))
                        .AddOneToManyAssociation<MonitorEventTypeAndResult>(
                            "MonitorEventType_Results"
                            , editor => editor
                                .TabName("Результат")
                                .IsLabelVisible(false)
                                .Mnemonic(nameof(MonitorEventTypeAndResult))
                                .Filter((uow, q, id, oid) => q.Where(w => w.ObjLeftId == id))
                                .Create((uow, entity, id) =>
                                {
                                    entity.ObjLeftId = id;
                                    entity.IsManualPick = true;
                                })
                                .Delete((uow, entity, id) =>
                                {
                                    if (entity.IsManualPick)
                                        entity.Hidden = true;
                                    else
                                        throw new System.Exception("Невозможно удалить результат, недоступный для ручного выбора.");
                                })
                            )
                            .AddManyToManyLeftAssociation<MonitorEventPreceding>("ReportMonitoringEventType_Precedings", y => y.TabName("Предшествующие контроли").IsLabelVisible(false))

                  ));

            context.CreateVmConfigOnBase<ReportMonitoringEventType>("DictMenu", "ReportMonitoringEventTypeForConsolidation")
                .Service<IDictHistoryService<ReportMonitoringEventType>>()
                .ListView(builder => builder.Title("Типы событий журнала выполнения контролей")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Edit, LvAction.Export })
                .OnClientEditRow(@"return;")
                .IsMultiSelect(true)
                    .Columns(cols => cols.Clear()
                        .Add(col => col.ReportMonitoringEventKind, ac => ac.Visible(true).Order(1))
                        .Add(col => col.Name, ac => ac.Visible(true).Order(2))
                        .Add(col => col.PlanDayOfMonth, ac => ac.Visible(true).Order(3))
                        .Add(col => col.EventPeriodicity, ac => ac.Visible(true).Order(4))

                        //видимость отключена
                        .Add(col => col.Code, ac => ac.Visible(false))
                        .Add(col => col.DateFrom, ac => ac.Visible(false))
                        .Add(col => col.DateTo, ac => ac.Visible(false))
                        .Add(col => col.DictObjectState, ac => ac.Visible(false))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                        .Add(col => col.PublishCode, ac => ac.Visible(false))
                        .Add(col => col.Note, ac => ac.Visible(false))
                        .Add(col => col.SortIndex, ac => ac.Visible(false))


                     ))
                .DetailView(builder => builder.Title("Тип события журнала выполнения контролей").HideToolbar(true).IsMaximized(false)
                    .Editors(eds => eds.Clear()
                        .Add(ed => ed.PublishCode, ac => ac.Visible(true).Order(1))
                        .Add(ed => ed.Code, ac => ac.Visible(false).Order(2))
                        .Add(ed => ed.Name, ac => ac.Visible(true).Order(3))
                        .Add(ed => ed.PlanDayOfMonth, ac => ac.Visible(true).Order(4))
                        .Add(ed => ed.DateTo, ac => ac.Visible(false).Order(5))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false).Order(7))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false).Order(8))
                        .Add(ed => ed.EventPeriodicity, ac => ac.Visible(true).Order(9))

                    )
                    )
                .LookupProperty(lp => lp.Text(t => t.Name));



            context.CreateVmConfigOnBase<DictObject, ReportMonitoringResult>()
              .Service<IDictObjectService<ReportMonitoringResult>>()
              .ListView(builder => builder.Title("Результаты выполнения контрольных процедур"))
              .DetailView(builder => builder.Title("Результат выполнения контрольных процедур"));

            context.CreateVmConfigOnBase<ReportMonitoringResult>(nameof(ReportMonitoringResult), "ReportMonitoringResult_Filtered")
             .Service<Services.Monitor.MonitorResultCustomService>()
             .ListView(builder => builder.Title("Результаты выполнения контрольных процедур"))
             .DetailView(builder => builder.Title("Результат выполнения контрольных процедур"));


            context.CreateVmConfigOnBase<DictObject, TransactionKind>()
               .Service<IDictObjectService<TransactionKind>>()
               .ListView(builder => builder.Title("Виды операций (аренда)"))
               .DetailView(builder => builder.Title("Вид операции (аренда)"));



            context.CreateVmConfigOnBase<TransactionKind>("DictMenu", "TransactionKindMenu")
                .Service<IDictHistoryService<TransactionKind>>()
                .ListView(builder => builder.Title("Виды операций (аренда)")
                    .Columns(cols => cols
                        .Add(col => col.PublishCode, ac => ac.Visible(true).Order(1))
                        .Add(col => col.Code, ac => ac.Visible(false).Order(2))
                        .Add(col => col.Name, ac => ac.Visible(true).Order(3))
                        .Add(col => col.DateFrom, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DateTo, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(7))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(8))

                     ))
                .DetailView(builder => builder.Title("Вид операции (аренда)")
                    .Editors(eds => eds

                        .Add(ed => ed.PublishCode, ac => ac.Visible(true).Order(1))
                        .Add(ed => ed.Code, ac => ac.Visible(false).Order(2))
                        .Add(ed => ed.Name, ac => ac.Visible(true).Order(3))
                        .Add(ed => ed.DateFrom, ac => ac.Visible(true).Order(4))
                        .Add(ed => ed.DateTo, ac => ac.Visible(true).Order(5))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(true).Order(7))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(true).Order(8))
                    ))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, ZoneResponsibility>()
                .Service<IDictObjectService<ZoneResponsibility>>()
                .Title("Зона ответственности БЕ")
                .ListView(x => x.Title("Зона ответственности БЕ"))
                .DetailView(x => x.Title("Зона ответственности БЕ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<ZoneResponsibility>("DictMenu", "ZoneResponsibilityMenu")
                .Service<IDictHistoryService<ZoneResponsibility>>()
                .Title("Зона ответственности БЕ")
                .ListView(x => x.Title("Зона ответственности БЕ"))
                .DetailView(x => x.Title("Зона ответственности БЕ"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, Responsible>()
                .Service<IDictObjectService<Responsible>>()
                .Title("Ответственный по БЕ")
                .ListView(x => x.Title("Ответственный по БЕ")
                .Columns(cols => cols.Clear()
                    .Add(ed => ed.ZoneResponsibility, ac => ac.Visible(true))
                    .Add(ed => ed.FIO, ac => ac.Visible(true))
                    .Add(ed => ed.Phone, ac => ac.Visible(true))
                    .Add(ed => ed.Email, ac => ac.Visible(true))
                    .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                ))
                .DetailView(x => x.Title("Ответственный по БЕ")
                .Editors(eds => eds.Clear()
                    .Add(ed => ed.ZoneResponsibility, ac => ac.Visible(true))
                    .Add(ed => ed.FIO, ac => ac.Visible(true))
                    .Add(ed => ed.Phone, ac => ac.Visible(true))
                    .Add(ed => ed.Email, ac => ac.Visible(true))
                    .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                ))
                .LookupProperty(x => x.Text(t => t.FIO));

            context.CreateVmConfigOnBase<Responsible>("DictMenu", "ResponsibleMenu")
                .Service<IDictHistoryService<Responsible>>()
                .Title("Ответственный по БЕ")
                .ListView(x => x.Title("Ответственный по БЕ")
                .Columns(cols => cols.Clear()
                    .Add(ed => ed.ZoneResponsibility, ac => ac.Visible(true))
                    .Add(ed => ed.FIO, ac => ac.Visible(true))
                    .Add(ed => ed.Phone, ac => ac.Visible(true))
                    .Add(ed => ed.Email, ac => ac.Visible(true))
                    .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                ))
                .DetailView(x => x.Title("Ответственный по БЕ")
                .Editors(eds => eds.Clear()
                    .Add(ed => ed.ZoneResponsibility, ac => ac.Visible(true))
                    .Add(ed => ed.FIO, ac => ac.Visible(true))
                    .Add(ed => ed.Phone, ac => ac.Visible(true))
                    .Add(ed => ed.Email, ac => ac.Visible(true))
                    .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                ))
                .LookupProperty(x => x.Text(t => t.FIO));

            context.CreateVmConfigOnBase<DictObject, PropertyListTaxBaseCadastral>()
               .Service<IDictObjectService<PropertyListTaxBaseCadastral>>()
               .ListView(builder => builder.Title("Перечень объектов недвижимого имущества, налоговая база в отношении которых определяется как их кадастровая стоимость"))
               .DetailView(builder => builder.Title("Объект недвижимого имущества, налоговая база в отношении которых определяется как их кадастровая стоимость"));

            context.CreateVmConfig<UpdateTaxBaseCadastralObjects>(
                    "updateTaxBaseCadastralObjects").Title("Обновление атрибутов ОИ")
                .DetailView(dv => dv.Editors(e => e.Add(ed => ed.Year, ac => ac.DataType(PropertyDataType.Year)))
                );

            context.CreateVmConfigOnBase<PropertyListTaxBaseCadastral>("DictMenu", "PropertyListTaxBaseCadastralMenu")
                .Service<IDictHistoryService<PropertyListTaxBaseCadastral>>()
                .ListView(builder => builder.Title("Перечень объектов недвижимого имущества, налоговая база в отношении которых определяется как их кадастровая стоимость")
                    .Columns(cols => cols
                        .Add(col => col.Name, ac => ac.Visible(true).Order(1))
                        .Add(col => col.CadastralNumber, ac => ac.Title("Кадастровый номер здания (строения, сооружения)").Visible(true).Order(2))
                        .Add(col => col.RoomCadastralNumber, ac => ac.Visible(true).Order(3))
                        .Add(col => col.ConditionalNumber, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DateFrom, ac => ac.Visible(true).Order(5))
                        .Add(col => col.DateTo, ac => ac.Visible(true).Order(6))
                        .Add(col => col.ApprovingDocNumber, ac => ac.Visible(true).Order(7))
                        .Add(col => col.ApprovingDocDate, ac => ac.Visible(true).Order(8))
                        .Add(col => col.SibRegion, ac => ac.Visible(true).Order(9))
                        .Add(col => col.Address, ac => ac.Visible(true).Order(10))
                        .Add(col => col.IsCadastralEstateUpdated, ac => ac.Visible(true).Order(11))
                        .Add(col => col.CadastralEstateUpdatedDate, ac => ac.Visible(true).Order(12))
                        .Add(col => col.Code, ac => ac.Visible(false))
                        .Add(col => col.PublishCode, ac => ac.Visible(false))
                        .Add(col => col.DictObjectState, ac => ac.Visible(false))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                        
                     ))
                .DetailView(builder => builder.Title("Объект недвижимого имущества, налоговая база в отношении которых определяется как их кадастровая стоимость")
                    .Editors(eds => eds
                        .Add(ed => ed.Name, ac => ac.IsRequired(false).Visible(true))
                        .Add(ed => ed.CadastralNumber, ac => ac.Title("Кадастровый номер здания (строения, сооружения)").Visible(true))
                        .Add(ed => ed.RoomCadastralNumber, ac => ac.Visible(true))
                        .Add(ed => ed.ConditionalNumber, ac => ac.Visible(true))
                        .Add(ed => ed.DateFrom, ac => ac.IsRequired(false).Visible(true).Order(4))
                        .Add(ed => ed.DateTo, ac => ac.Visible(true).Order(5))
                        .Add(ed => ed.ApprovingDocNumber, ac => ac.Visible(true))
                        .Add(ed => ed.ApprovingDocDate, ac => ac.Visible(true))
                        .Add(ed => ed.SibRegion, ac => ac.Visible(true))
                        .Add(ed => ed.Address, ac => ac.Visible(true))
                        .Add(ed => ed.IsCadastralEstateUpdated, ac => ac.Visible(true).IsReadOnly(true))
                        .Add(ed => ed.CadastralEstateUpdatedDate, ac => ac.Visible(true).IsReadOnly(true))
                        .Add(ed => ed.Code, ac => ac.Visible(false))
                        .Add(ed => ed.PublishCode, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                        
                    ))
                .LookupProperty(lp => lp.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, SibCountry>("SibCountryPropertyComplexIO")
                .Service<IDictObjectService<SibCountry>>()
                .ListView(lv => lv.Columns(cols => cols.Clear()
                    .Add(col => col.Name, ac => ac.Visible(true))));

            context.CreateVmConfigOnBase<DictObject, SibFederalDistrict>("SibFederalDistrictPropertyComplexIO")
                .Service<IDictObjectService<SibFederalDistrict>>()
                .ListView(lv => lv.Columns(cols => cols.Clear()
                    .Add(col => col.Name, ac => ac.Visible(true))));

            context.CreateVmConfigOnBase<DictObject, SibRegion>("SibRegionPropertyComplexIO")
                .Service<IDictObjectService<SibRegion>>()
                .ListView(lv => lv.Columns(cols => cols.Clear()
                    .Add(col => col.Name, ac => ac.Visible(true).Order(1))
                    .Add(col => col.PublishCode, ac => ac.Visible(true).Order(2))));

            context.CreateVmConfigOnBase<DictObject, HolidayWorkDay>()
                .Service<IDictObjectService<HolidayWorkDay>>()
                .Title("Выходные и рабочие дни")
                .ListView(x => x.Title("Выходные и рабочие дни"))
                .DetailView(x => x.Title("Выходные и рабочие дни"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<HolidayWorkDay>("DictMenu", "HolidayWorkDayMenu")
                 .Service<IDictHistoryService<HolidayWorkDay>>()
                 .Title("Выходные и рабочие дни")
                 .ListView(x => x.Title("Выходные и рабочие дни"))
                 .DetailView(x => x.Title("Выходные и рабочие дни"))
                 .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, MITDictionary>()
                  .Service<IDictObjectService<MITDictionary>>()
                  .Title("Справочник Минпромторга (Кп ТС)")
                  .ListView(builder => builder.Title("Справочник Минпромторга (Кп ТС)")
                    .Columns(cols => cols.Clear()
                        .Add(col => col.Brand, ac => ac.Visible(true))
                        .Add(col => col.Name, ac => ac.Title("Модель").Visible(true))
                        .Add(col => col.EngineType, ac => ac.Visible(true))
                        .Add(col => col.EngineCapacity, ac => ac.Visible(true))
                        .Add(col => col.MaxAge, ac => ac.Visible(true))
                        .Add(col => col.LowBoundRange, ac => ac.Visible(true))
                        .Add(col => col.UpBoundRange, ac => ac.Visible(true))

                        .Add(col => col.Code, ac => ac.Visible(false))
                        .Add(col => col.PublishCode, ac => ac.Visible(false))
                        .Add(col => col.DictObjectState, ac => ac.Visible(false))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                  ))
                  .DetailView(builder => builder.Title("Справочник Минпромторга (Кп ТС)")
                    .Editors(eds => eds
                        .Add(ed => ed.Brand, ac => ac.Visible(true))
                        .Add(ed => ed.Name, ac => ac.Title("Модель").Visible(true))
                        .Add(ed => ed.EngineType, ac => ac.Visible(true))
                        .Add(ed => ed.EngineCapacity, ac => ac.Visible(true))
                        .Add(ed => ed.MaxAge, ac => ac.Visible(true))
                        .Add(ed => ed.LowBoundRange, ac => ac.Visible(true))
                        .Add(ed => ed.UpBoundRange, ac => ac.Visible(true))

                        .Add(ed => ed.Code, ac => ac.Visible(false))
                        .Add(ed => ed.PublishCode, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                  ));

            context.CreateVmConfigOnBase<MITDictionary>("DictMenu", "MITDictionaryMenu")
                 .Service<IDictHistoryService<MITDictionary>>()
                 .Title("Справочник Минпромторга (Кп ТС)")
                 .ListView(builder => builder.Title("Справочник Минпромторга (Кп ТС)")
                    .Columns(cols => cols.Clear()
                        .Add(col => col.Brand, ac => ac.Visible(true))
                        .Add(col => col.Name, ac => ac.Title("Модель").Visible(true))
                        .Add(col => col.EngineType, ac => ac.Visible(true))
                        .Add(col => col.EngineCapacity, ac => ac.Visible(true))
                        .Add(col => col.MaxAge, ac => ac.Visible(true))
                        .Add(col => col.LowBoundRange, ac => ac.Visible(true))
                        .Add(col => col.UpBoundRange, ac => ac.Visible(true))

                        .Add(col => col.Code, ac => ac.Visible(false))
                        .Add(col => col.PublishCode, ac => ac.Visible(false))
                        .Add(col => col.DictObjectState, ac => ac.Visible(false))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                  ))
                 .DetailView(builder => builder.Title("Справочник Минпромторга (Кп ТС)")
                    .Editors(eds => eds
                        .Add(ed => ed.Brand, ac => ac.Visible(true))
                        .Add(ed => ed.Name, ac => ac.Title("Модель").Visible(true))
                        .Add(ed => ed.EngineType, ac => ac.Visible(true))
                        .Add(ed => ed.EngineCapacity, ac => ac.Visible(true))
                        .Add(ed => ed.MaxAge, ac => ac.Visible(true))
                        .Add(ed => ed.LowBoundRange, ac => ac.Visible(true))
                        .Add(ed => ed.UpBoundRange, ac => ac.Visible(true))

                        .Add(ed => ed.Code, ac => ac.Visible(false))
                        .Add(ed => ed.PublishCode, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                  ));

            context.CreateVmConfigOnBase<DictObject, EngineType>()
                  .Service<IDictObjectService<EngineType>>()
                  .Title("Тип Двигателя")
                  .ListView(builder => builder.Title("Тип Двигателя"))
                  .DetailView(builder => builder.Title("Тип Двигателя"))
                  .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<EngineType>("DictMenu", "EngineTypeMenu")
                 .Service<IDictHistoryService<EngineType>>()
                 .Title("Тип Двигателя")
                 .ListView(builder => builder.Title("Тип Двигателя"))
                 .DetailView(builder => builder.Title("Тип Двигателя"))
                 .LookupProperty(x => x.Text(t => t.Name));

            context.ModifyVmConfig<BoostOrReductionFactor>("BoostOrReductionFactor")
                .ListView(x => x.Title("Повышающий/понижающий коэффициент")
                    .Columns(cols => cols.Clear()
                        .Add(col => col.Name, ac => ac.Visible(true))
                        .Add(col => col.LowBoundRange, ac => ac.Visible(true))
                        .Add(col => col.UpBoundRange, ac => ac.Visible(true))
                        .Add(col => col.MaxAge, ac => ac.Visible(true))
                        .Add(col => col.Value, ac => ac.Visible(true))

                        .Add(col => col.Code, ac => ac.Visible(false))
                        .Add(col => col.PublishCode, ac => ac.Visible(false))
                        .Add(col => col.DictObjectState, ac => ac.Visible(false))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                 ))
                .DetailView(x => x.Title("Повышающий/понижающий коэффициент")
                    .Editors(eds => eds
                        .Add(ed => ed.Name, ac => ac.Visible(true))
                        .Add(ed => ed.LowBoundRange, ac => ac.Visible(true))
                        .Add(ed => ed.UpBoundRange, ac => ac.Visible(true))
                        .Add(ed => ed.MaxAge, ac => ac.Visible(true))
                        .Add(ed => ed.Value, ac => ac.Visible(true))

                        .Add(ed => ed.Code, ac => ac.Visible(false))
                        .Add(ed => ed.PublishCode, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                 ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.ModifyVmConfig<BoostOrReductionFactor>("BoostOrReductionFactorMenu")
                .ListView(x => x.Title("Повышающий/понижающий коэффициент")
                    .Columns(cols => cols.Clear()
                        .Add(col => col.Name, ac => ac.Visible(true))
                        .Add(col => col.LowBoundRange, ac => ac.Visible(true))
                        .Add(col => col.UpBoundRange, ac => ac.Visible(true))
                        .Add(col => col.MaxAge, ac => ac.Visible(true))
                        .Add(col => col.Value, ac => ac.Visible(true))

                        .Add(col => col.Code, ac => ac.Visible(false))
                        .Add(col => col.PublishCode, ac => ac.Visible(false))
                        .Add(col => col.DictObjectState, ac => ac.Visible(false))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(false))
                 ))
                .DetailView(x => x.Title("Повышающий/понижающий коэффициент")
                    .Editors(eds => eds
                        .Add(ed => ed.Name, ac => ac.Visible(true))
                        .Add(ed => ed.LowBoundRange, ac => ac.Visible(true))
                        .Add(ed => ed.UpBoundRange, ac => ac.Visible(true))
                        .Add(ed => ed.MaxAge, ac => ac.Visible(true))
                        .Add(ed => ed.Value, ac => ac.Visible(true))

                        .Add(ed => ed.Code, ac => ac.Visible(false))
                        .Add(ed => ed.PublishCode, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectState, ac => ac.Visible(false))
                        .Add(ed => ed.DictObjectStatus, ac => ac.Visible(false))
                 ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<DictObject, Periodicity>()
               .Service<IDictObjectService<Periodicity>>()
               .ListView(builder => builder.Title("Периодичность"))
               .DetailView(dv => dv.Title("Периодичность"));
        }
    }
}