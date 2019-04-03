using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Base;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Services.Base;
using CorpProp.Services.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Security;
using CorpProp.Extentions;

namespace CorpProp.Model
{
    public static class LawModel
    {
        /// <summary>
        /// Инициализация моделей прав.
        /// </summary>
        /// <param name="context"></param>
		public static void Init(IInitializerContext context)
        {

            CorpProp.Model.Law.RightModel.CreateModelConfig(context);
            CorpProp.Model.Law.ScheduleStateRegistrationRecordModel.CreateModelConfig(context);

            #region Law 
            context.CreateVmConfig<DuplicateRightView>()
             .Service<IDuplicateRightViewService>()
             .Title("Непрекращенные права")
             .ListView(x => x.Title("Контроль задвоения прав")
               .HiddenActions(new[] { LvAction.Create, LvAction.Edit, LvAction.Delete, LvAction.Link })
               .DataSource(ds => ds.Groups(gr => gr.Groupable(true).ShowFooter(true).Add(p => p.CadastralNumber))
               .Aggregate(ag => ag.Add(p => p.CadastralNumber, AggregateType.Count).Add(p => p.ID, AggregateType.Count))
               )

              )
             .DetailView(x => x.Title("Право АИС КС"))
             .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfig<RightCostView>()
             .Service<IRightCostViewService>()
             .Title("Стоимость права по данным БУ")
             .ListView(x => x.Title("Стоимость права по данным БУ")
               .HiddenActions(new[] { LvAction.Create, LvAction.Edit, LvAction.Delete, LvAction.Link })
               )
             .DetailView(x => x.Title("Право АИС КС"))
             .LookupProperty(x => x.Text(t => t.Title))
             .IsReadOnly(true);

            context.CreateVmConfig<Encumbrance>()
                .Service<IEncumbranceService>()
                .Title("Обременение/Ограничение")
                .ListView(x => x.Title("Обременения/Ограничения"))
                .DetailView(x => x.Title("Обременение/Ограничение"))
                .LookupProperty(x => x.Text(t => t.Title));

            context.CreateVmConfig<Encumbrance>("Cadastral_Encumbrances")
                .Service<IEncumbranceService>()
                .Title("Обременение/Ограничение")
                .ListView(x => x.Title("Обременения/Ограничения"))
                .DetailView(x => x.Title("Обременение/Ограничение"))
                .LookupProperty(x => x.Text(t => t.Title));

            

            context.CreateVmConfig<Extract>()
             .Service<IExtractService>()
             .Title("Выписка")
             .DetailView(x => x.Title("Выписка"))
             .ListView(x => x.Title("Выписки"))
             .LookupProperty(x => x.Text(t => t.Name))
             ;

          

            context.CreateVmConfig<ExtractItem>()
                .Title("Строка выписки")
                .ListView(x => x.Title("Строки выписки"))
                .DetailView(x => x.Title("Строка выписки"))
                .LookupProperty(x => x.Text(t => t.Extract.Name));
            
            context.CreateVmConfig<IntangibleAssetRight>()
               .Service<IIntangibleAssetRightService>()
               .Title("Право на НМА")
               .ListView(x => x.Title("Права на НМА")
                .Columns(c =>
                            c.Add(t => t.ID, h => h.Visible(false))
                             .Add(t => t.IntangibleAssetRightType, h => h.Visible(true))
                             .Add(t => t.RegNumber, h => h.Visible(true))
                             .Add(t => t.RightHolder, h => h.Visible(true))
                             .Add(t => t.PrioritySign, h => h.Visible(true))
                             .Add(t => t.DateFrom, h => h.Visible(true))
                             .Add(t => t.DateTo, h => h.Visible(true))

                             .Add(t => t.Number, h => h.Visible(false))
                             .Add(t => t.Image, h => h.Visible(false))
                          )
               )
               .DetailView(x => x.Title("Право на НМА"))
               .LookupProperty(x => x.Text(t => t.Title));


           

            context.CreateVmConfig<ScheduleStateRegistration>()
                .Service<IScheduleStateRegistrationService>()
                .Title("График государственной регистрации права")
                .ListView(x => x.Title("Графики государственной регистрации прав")
                    .DataSource(t => t.Groups(g => g.Add(d => d.Year).Add(d => d.Society)).Sort(s => s.Add<int?>(a => a.Year, System.ComponentModel.ListSortDirection.Descending)))
                    .Columns(c =>
                            c.Add(t => t.ID, h => h.Visible(false))
                             .Add(t => t.Year, h => h.Visible(true))
                             .Add(t => t.Society, h => h.Visible(true))
                             .Add(t => t.Executor, h => h.Visible(true))
                             .Add(t => t.ExecutorPhone, h => h.Visible(true))
                             .Add(t => t.ExecutorEmail, h => h.Visible(true))
                             .Add(t => t.SocietyEmail, h => h.Visible(true))
                             .Add(t => t.DateSchedule, h => h.Visible(true))
                             .Add(t => t.ScheduleStateRegistrationStatus, h => h.Visible(true))
                             .Add(t => t.Description, h => h.Visible(true))
                          )
                    )
                .DetailView(x => x.Title("График государственной регистрации права")
                    .Editors(e => e

                    //Закладка "Основные данные"
                   
                   .Add(ed => ed.ScheduleStateYear, ac => ac.Order(1).Group("Основные данные").TabName("Основные данные"))
                   .Add(ed => ed.Year, ac => ac.Order(2).Group("Основные данные").TabName("Основные данные"))
                   .Add(ed => ed.DateSchedule, ac => ac.Order(4).Group("Основные данные").TabName("Основные данные"))

                   .Add(ed => ed.Society, ac => ac.Order(5).Group("Исполнитель").TabName("Основные данные"))
                   .Add(ed => ed.ExecutorPhone, ac => ac.Order(6).Group("Исполнитель").TabName("Основные данные"))
                   .AddEmpty(ac => ac.Order(7).Group("Исполнитель").TabName("Основные данные"))
                   .Add(ed => ed.EmployeeUploadedData, ac => ac.Order(8).Group("Исполнитель").TabName("Основные данные"))
                   .Add(ed => ed.ExecutorEmail, ac => ac.Order(9).Group("Исполнитель").TabName("Основные данные"))
                   .AddEmpty(ac => ac.Order(10).Group("Исполнитель").TabName("Основные данные"))
                   .Add(ed => ed.Executor, ac => ac.Order(11).Group("Исполнитель").TabName("Основные данные"))
                   .Add(ed => ed.SocietyEmail, ac => ac.Order(12).Group("Исполнитель").TabName("Основные данные"))

                   .Add(ed => ed.ScheduleStateRegistrationStatus, ac => ac.Order(13).Group("Статус").TabName("Основные данные"))
                   .Add(ed => ed.Description, ac => ac.Order(14).Group("Статус").TabName("Основные данные"))
                   .Add(ed => ed.RewortNotes, ac => ac.Order(15).Group("Статус").TabName("Основные данные"))
                   .Add(ed => ed.FileCard, ac => ac.Order(15).Group("Статус").TabName("Основные данные"))
                   


                    .AddOneToManyAssociation<ScheduleStateRegistrationRecord>("ScheduleStateRegistration_ScheduleStateRegistrationRecord", edt => edt
                            .Title("Строка перечня")
                            .IsLabelVisible(false)
                            .Visible(true)
                            
                            .TabName("Строки графика гос. регистрации")
                            .Create((uofw, entity, id) =>
                            {
                                entity.ScheduleStateRegistration = uofw.GetRepository<ScheduleStateRegistration>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.ScheduleStateRegistration = null;
                                entity.ScheduleStateRegistrationID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ScheduleStateRegistrationID == id && !w.Hidden))
                        ))
                    .DefaultSettings((uow, r, commonEditorViewModel) =>
                    {
                        SibUser currentSibUser = null;
                        var profileUserID = Base.Ambient.AppContext.SecurityUser.ProfileInfo?.ID;

                        if (profileUserID != null)
                        {
                            currentSibUser = uow.GetRepository<SibUser>().Find(sUser => sUser.ID == profileUserID);
                        }

                        if (currentSibUser != null && !currentSibUser.IsFromCauk())
                            commonEditorViewModel.Visible(v => v.ScheduleStateYear, false);

                        if (r.ScheduleStateRegistrationStatus != null && (r.ScheduleStateRegistrationStatus.Code == "106" || r.ScheduleStateRegistrationStatus.Code == "102"))
                        {
                            commonEditorViewModel.SetReadOnlyAll();
                            commonEditorViewModel.ReadOnly(ro => ro.ScheduleStateRegistrationStatus, false);
                            commonEditorViewModel.ReadOnly(ro => ro.Description, false);
                        }
                    })
                )
                .LookupProperty(x => x.Text(t => t.Name))
                ;

            context.CreateVmConfigOnBase<ScheduleStateRegistration>(nameof(ScheduleStateRegistration), "ActualSSR")
                .ListView(lv => lv.DataSource(ds => ds.Filter(f => f.Year != null && f.Year > DateTime.Now.Year)));

            context.GetVmConfig<ScheduleStateRegistration>().DetailView.Toolbars.Add(new Toolbar()
            {
                AjaxAction =
                    new AjaxAction
                    {
                        Name = "GetExportLndToolbar",
                        Controller = "Toolbar",
                        Params =
                            new Dictionary<string, string>() { { "mnemonic", "ScheduleStateRegistration" }, { "objectID", "[ID]" } }
                    },
                Title = "Экспорт (формат ЛНД)",
                IsAjax = true,
            });



            context.CreateVmConfig<ScheduleStateTerminate>()
               .Service<IScheduleStateTerminateService>()
               .Title("График государственной регистрации прекращения права")
               .ListView(x => x.Title("Графики государственной регистрации прекращения прав")
                            .DataSource(t => t.Groups(g => g.Add(d => d.Year).Add(d => d.Society)).Sort(s => s.Add<int?>(a => a.Year, System.ComponentModel.ListSortDirection.Descending)))
                            .Columns(c =>
                                    c.Add(t => t.ID, h => h.Visible(false))
                                     .Add(t => t.Year, h => h.Visible(true))
                                     .Add(t => t.Society, h => h.Visible(true))
                                     .Add(t => t.Executor, h => h.Visible(true))
                                     .Add(t => t.ExecutorPhone, h => h.Visible(true))
                                     .Add(t => t.ExecutorEmail, h => h.Visible(true))
                                     .Add(t => t.SocietyEmail, h => h.Visible(true))
                                     .Add(t => t.DateSchedule, h => h.Visible(true))
                                     .Add(t => t.ScheduleStateRegistrationStatus, h => h.Visible(true))
                                     .Add(t => t.Description, h => h.Visible(true))
                                  )
                        )
               .DetailView(x => x.Title("График государственной регистрации прекращения права")
                   .Editors(e => e
                            .AddOneToManyAssociation<ScheduleStateTerminateRecord>("ScheduleStateTerminate_ScheduleStateTerminateRecord", edt => edt
                            .Title("Строка перечня")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .TabName("Строки графика гос. регистрации")
                            .Create((uofw, entity, id) =>
                            {
                                entity.ScheduleStateTerminate = uofw.GetRepository<ScheduleStateTerminate>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.ScheduleStateTerminate = null;
                                entity.ScheduleStateTerminateID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ScheduleStateTerminateID == id && !w.Hidden))
                        ))
                    .DefaultSettings((uow, r, commonEditorViewModel) =>
                    {
                        if (r.ScheduleStateRegistrationStatus != null && (r.ScheduleStateRegistrationStatus.Code == "106" || r.ScheduleStateRegistrationStatus.Code == "102"))
                            {
                            commonEditorViewModel.SetReadOnlyAll();
                            commonEditorViewModel.ReadOnly(ro => ro.ScheduleStateRegistrationStatus, false);
                        }
                    })
                )
               .LookupProperty(x => x.Text(t => t.Name))
               ;
            context.GetVmConfig<ScheduleStateTerminate>().DetailView.Toolbars.Add(new Toolbar()
            {
                AjaxAction =
                new AjaxAction
                {
                    Name = "GetExportLndToolbar",
                    Controller = "Toolbar",
                    Params =
                        new Dictionary<string, string>()
                        {
                                            {"mnemonic", "ScheduleStateTerminate"},
                                            {"objectID", "[ID]"}
                        }
                },
                Title = "Экспорт (формат ЛНД)",
                IsAjax = true,
            });

            context.CreateVmConfigOnBase<ScheduleStateTerminate>(nameof(ScheduleStateTerminate), "ActualSST")
                .ListView(lv => lv.DataSource(ds => ds.Filter(f => f.Year != null && f.Year > DateTime.Now.Year)));

            context.CreateVmConfig<ScheduleStateTerminateRecord>()
                .Service<IScheduleStateTerminateRecordService>()
                .Title("Строка графика государственной регистрации прекращения права")
                .ListView(x => x.Title("Строки графиков государственной регистрации прекращения прав").IsMultiSelect(true))
                .DetailView(x => x.Title("Строка графика государственной регистрации прекращения права")
                .Editors(e => e
                        .Add(edt => edt.AccountingObject, ac => ac.OnChangeClientScript(" corpProp.dv.editors.onChange.SSRTR_AccountingObject(form, isChange);"))

                        .AddManyToManyRigthAssociation<FileCardAndScheduleStateTerminateRecord>("FileCardAndScheduleStateTerminateRecord", edt => edt.TabName("Документы"))
                 )
                 .DefaultSettings((uow, r, commonEditorViewModel) =>
                 {
                     commonEditorViewModel.ReadOnly(ro => ro.ScheduleStateTerminate);                     

                     if (r.ScheduleStateTerminate != null && r.ScheduleStateTerminate.ScheduleStateRegistrationStatus != null
                     && (r.ScheduleStateTerminate.ScheduleStateRegistrationStatus.Code == "102" ||
                     r.ScheduleStateTerminate.ScheduleStateRegistrationStatus.Code == "106"))
                     {
                         commonEditorViewModel.SetReadOnlyAll();
                         commonEditorViewModel.ReadOnly(ro => r.Description, false);

                         commonEditorViewModel.ReadOnly(ro => ro.DateActualFilingDocument, false);
                         commonEditorViewModel.ReadOnly(ro => ro.DateActualRegistration, false);
                        
                     }
                 })
                )
                .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<ScheduleStateTerminateRecord>("ScheduleStateTerminateRecordPivot")
                .Service<IScheduleStateTerminateRecordService>()
                .Title("Строка графика государственной регистрации прекращения права")
                .ListView(x => x.Title("Строки графиков государственной регистрации прекращения прав")
                .Type(ListViewType.Pivot))
                .DetailView(x => x.Title("Строка графика государственной регистрации прекращения права"))
                .LookupProperty(x => x.Text(t => t.ID));

            

            #endregion

        }
    }
}
