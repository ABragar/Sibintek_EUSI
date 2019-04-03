using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Helpers;
using CorpProp.Services.ProjectActivity;
using System.Linq;

namespace CorpProp.Model.ProjectActivity
{
    public static class TaskModel
    {
        public static void CreateModelConfig(this IInitializerContext context)
        {
            #region conf
            context.CreateVmConfig<SibTask>("SibTaskMenuList")
                .Title("Задача")
                .Service<ISibTaskService>()
                .IsNotify()
                .ListView_Default()
                .DetailView_Default()
                .LookupProperty(lp => lp.Text(t => t.Title))
                ;

            context.CreateVmConfig<SibTask>("SibTaskGantt")
                .Title("Задача")
                .Service<ISibTaskService>()
                .ListView_DefaultGantt()
                .DetailView_Default()
                .LookupProperty(lp => lp.Text(t => t.Title))
                ;

            context.CreateVmConfig<SibTask>("SibTaskScheduler")
                .Title("Задача")
                .Service<ISibTaskService>()
                .ListView_DefaultScheduler()
                .DetailView_Default()
                .LookupProperty(lp => lp.Text(t => t.Title))
                ;

            context.CreateVmConfig<SibTask>("SibTaskTreeList")
                .Title("Задача")
                .Service<ISibTaskService>()
                .ListView_DefaultTreeList()
                .DetailView_Default()
                .LookupProperty(lp => lp.Text(t => t.Title))
                ;

            context.CreateVmConfig<SibTask>("SibTaskTemplate")
                .Title("Шаблон задачи")
                .Service<ISibTaskTemplateService>()
                .ListView_TemplateDefault()
                .DetailView_TemplateDefault()
                .LookupProperty(lp => lp.Text(t => t.Title))
                ;

            context.CreateVmConfig<SibTaskGanttDependency>()
                .Service<ISibTaskGanttDependencyService>()
                .Title("Связь")
                .ListView(x => x.Title("Связи"))
                .DetailView(x => x.Title("Связь"))
                .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<SibTaskReport>()
                .Service<ISibTaskReportService>()
                .Title("Отчет по задаче")
                .ListView(x => x.Title("Отчеты по задачам"))
                .DetailView(x => x.Title("Отчет по задаче")
                    .Editors(e => e
                        .Add(a => a.Task, edt => edt.Mnemonic("SibTaskMenuList"))
                        .AddManyToManyLeftAssociation<SibTaskReportAndFileCard>("SibTaskReportAndFileCard", edt => edt.TabName("[2]Документы"))
                        .AddManyToManyLeftAssociation<SibTaskReportAndRight>("SibTaskReportAndRight", edt => edt.TabName("[5]Права"))
                        .AddManyToManyLeftAssociation<SibTaskReportAndAppraisal>("SibTaskReportAndAppraisal", edt => edt.TabName("[6]Оценки"))
                        .AddManyToManyLeftAssociation<SibTaskReportAndDeal>("SibTaskReportAndDeal", edt => edt.TabName("[4]Сделки"))
                        .AddManyToManyLeftAssociation<SibTaskReportAndEstate>("SibTaskReportAndEstate", edt => edt.TabName("[3]Объекты имущества"))
                    )
                )
                .LookupProperty(x => x.Text(t => t.TextReport));

            context.CreateVmConfig<SibTask>("SibTaskTemplateTree")
                .Title("Задача")
                .Service<ISibTaskService>()
                .ListView_TemplateTreeList()
                .DetailView_TemplateDefault()
                .LookupProperty(lp => lp.Text(t => t.Title))
                ;
            #endregion
        }

        #region ListView
        public static ViewModelConfigBuilder<SibTask> ListView_Default(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.ListView(lv => lv
                    .Title("Задачи")
                    .Columns(c => c
                        .Clear()
                        .Add(a => a.ProjectName, h => h.Visible(true))
                        .Add(a => a.ProjectDateFrom, h => h.Visible(true))
                        .Add(a => a.ProjectDateTo, h => h.Visible(true))
                        .Add(a => a.ProjectInitiator, h => h.Visible(true))
                        .Add(a => a.Number, h => h.Title("Номер задачи").Visible(true))
                        .Add(a => a.Title, h => h.Title("Наименование задачи").Visible(true))
                        .Add(a => a.Start, h => h.Title("Дата начала задачи").Visible(true))
                        .Add(a => a.End, h => h.Title("Дата окончания задачи").Visible(true))
                        .Add(a => a.SibStatus, h => h.Visible(true))
                        .Add(a => a.Initiator, h => h.Title("Инициатор задачи").Visible(true))
                        .Add(a => a.Responsible, h => h.Title("Ответственный исполнитель задачи").Visible(true))
                        .Add(a => a.PercentComplete, h => h.Visible(false))
                    )
                    .DataSource(ds => ds
                        .Filter(f =>
                            !f.IsTemplate
                            && !f.Hidden
                        )
                    )
                    .Type(ListViewType.Grid)
                )
                ;
        }

        public static ViewModelConfigBuilder<SibTask> ListView_DefaultTreeList(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.ListView(lv => lv
                    .Title("Задачи")
                    .Columns(c => c
                        .Clear()
                        .Add(a => a.ParentID, h => h.Visible(false))
                        .Add(a => a.ProjectID, h => h.Visible(false))
                        .Add(a => a.Name)
                        .Add(a => a.SibStatus)
                    )
                    .DataSource(ds => ds
                        .Filter(f =>
                            !f.Hidden
                            && !f.IsTemplate
                        )
                    )
                    .Type(ListViewType.TreeListView)
                )
                ;
        }

        public static ViewModelConfigBuilder<SibTask> ListView_DefaultGantt(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.ListView(lv => lv
                    .Title("Диаграмма Ганта")
                    .Columns(c => c
                        .Clear()
                        .Add(a => a.Start)
                        .Add(a => a.End)
                        .Add(a => a.PercentComplete)
                        .Add(a => a.Number)
                    )
                    .DataSource(ds => ds
                        .Filter(f => 
                            !f.Hidden
                            && !f.IsTemplate
                        )
                    )
                    .Type(ListViewType.Gantt)
                )
                ;
        }

        public static ViewModelConfigBuilder<SibTask> ListView_DefaultScheduler(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.ListView(lv => lv
                    .Title("Задачи (Календарь)")
                    .Columns(c => c
                        .Clear()
                        .Add(a => a.Title)
                        .Add(a => a.SibStatus)
                        .Add(a => a.Start)
                        .Add(a => a.End)
                        .Add(a => a.Initiator)
                        .Add(a => a.Responsible)
                    )
                    .DataSource(ds => ds
                        .Filter(f => 
                            !f.Hidden
                            && !f.IsTemplate
                            && f.SibStatus != null
                            && f.SibStatus.Code == "Appoint"
                        )
                    )
                    .Type(ListViewType.TreeListView)
                )
                ;
        }

        public static ViewModelConfigBuilder<SibTask> ListView_TemplateDefault(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.ListView_Default()
                    .ListView(lv => lv
                        .Title("Шаблоны задач")
                        .Columns(c => c
                            .Clear()
                            .Add(a => a.ProjectName, h => h.Visible(true))
                            .Add(a => a.ProjectDateFrom, h => h.Visible(true))
                            .Add(a => a.ProjectDateTo, h => h.Visible(true))
                            .Add(a => a.ProjectInitiator, h => h.Visible(true))
                            .Add(a => a.Title, h => h.Title("Наименование задачи").Visible(true))
                            .Add(a => a.Start, h => h.Title("Дата начала задачи").Visible(true))
                            .Add(a => a.End, h => h.Title("Дата окончания задачи").Visible(true))
                            .Add(a => a.SibStatus, h => h.Visible(true))
                            .Add(a => a.Initiator, h => h.Title("Инициатор задачи").Visible(true))
                            .Add(a => a.Responsible, h => h.Title("Ответственный исполнитель задачи").Visible(true))
                            .Add(a => a.PercentComplete, h => h.Visible(false))
                        )
                        .DataSource(ds => ds
                            .Filter(f =>
                                f.IsTemplate
                                && !f.Hidden
                            )
                        )
                        .Type(ListViewType.Grid)
                    )
                ;
        }

        public static ViewModelConfigBuilder<SibTask> ListView_TemplateTreeList(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.ListView_DefaultTreeList()
                    .ListView(lv => lv
                        .Title("Шаблоны задач")
                        .DataSource(ds => ds
                            .Filter(f =>
                                !f.Hidden
                                && f.IsTemplate
                            )
                        )
                        .Type(ListViewType.TreeListView)
                    )
                ;
        }
        #endregion

        #region DetailView
        public static ViewModelConfigBuilder<SibTask> DetailView_Default(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.DetailView(dv => dv
                    .Editors(e => e
                        .Clear()
                        .Add(a => a.Number, a => a.Order(1))
                        .Add(a => a.Name, a => a.Order(2).TabName(CaptionHelper.DefaultTabName))
                        .Add(a => a.SibStatus, a => a.Order(3))
                        .Add(a => a.Initiator, a => a.Order(4))
                        .Add(a => a.Responsible, a => a.Order(5))
                        .Add(a => a.DateEnd, a => a.Order(6))
                        .Add(a => a.Period, a => a.Order(7).TabName(CaptionHelper.DefaultTabName).Title("Срок выполнения задачи"))
                        .Add(a => a.Description, a => a.Order(8))
                        .Add(a => a.PercentComplete, a => a.Order(9).IsRequired(false).TabName(CaptionHelper.DefaultTabName))
                        .Add(a => a.IsRequiredLinkReportFile, a => a.Order(11))
                        .Add(a => a.TaskParent, a => a.Order(17).TabName(CaptionHelper.DefaultTabName).Mnemonic("SibTaskMenuList").CascadeFrom<SibProject>(c => c.Project))
                        .Add(a => a.Project, a => a.Order(18).Mnemonic("SibProjectMenuList"))
                        .Add(a => a.NotificationEnabled, a => a.Order(19))
                        .Add(a => a.RemindPeriod, a => a.Order(20))
                        .Add(a => a.PropertyName, a => a.Order(21))
                        .Add(a => a.NotificationSubject, a => a.Order(22))
                        .Add(a => a.NotificationMessage, a => a.Order(23))
                        .AddOneToManyAssociation<SibTaskReport>("SibTask_SibTaskReport", edt => edt
                            .TabName("Связанные данные")
                            .Title("Отчеты")
                            .IsReadOnly(false)
                            .IsLabelVisible(true)
                            .Create((uofw, entity, id) =>
                            {
                                entity.Task = uofw.GetRepository<SibTask>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Task = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.TaskID == id && !w.Hidden))
                            .Type(EditorAssociationType.InLine)
                        )
                        .AddManyToManyLeftAssociation<SibTaskAndFileCard>("SibTaskAndFileCard", edt => edt.TabName("Связанные данные").Title("Документы").IsLabelVisible(true))
                        .AddManyToManyLeftAssociation<SibTaskAndRight>("SibTaskAndRight", edt => edt.TabName("Права"))
                        .AddManyToManyLeftAssociation<SibTaskAndAppraisal>("SibTaskAndAppraisal", edt => edt.TabName("Оценки"))
                        .AddManyToManyLeftAssociation<SibTaskAndDeal>("SibTaskAndDeal", edt => edt.TabName("Связанные данные").Title("Сделки").IsLabelVisible(true))
                        .AddManyToManyLeftAssociation<SibTaskAndEstate>("SibTaskAndEstate", edt => edt.TabName("Объекты имущества"))
                        .AddManyToManyLeftAssociation<SibTaskAndSibUser>("SibTaskAndSibUser", edt => edt.TabName("[2]Соисполнители"))
                    )
                )
                ;
        }

        public static ViewModelConfigBuilder<SibTask> DetailView_TemplateDefault(this ViewModelConfigBuilder<SibTask> conf)
        {
            return
                conf.DetailView_Default()
                    .DetailView(dv => dv
                        .Editors(e => e
                            .Clear()
                            //.Add(a => a.Number, a => a.Order(1).Visible(false))
                            .Add(a => a.Name, a => a.Order(2).TabName(CaptionHelper.DefaultTabName))
                            .Add(a => a.SibStatus, a => a.Order(3))
                            .Add(a => a.Initiator, a => a.Order(4))
                            .Add(a => a.Responsible, a => a.Order(5))
                            .Add(a => a.DateEnd, a => a.Order(6))
                            .Add(a => a.Period, a => a.Order(7).TabName(CaptionHelper.DefaultTabName).Title("Срок выполнения задачи").IsRequired(false))
                            .Add(a => a.Description, a => a.Order(8))
                            .Add(a => a.PercentComplete, a => a.Order(9).IsRequired(false).TabName(CaptionHelper.DefaultTabName))
                            .Add(a => a.IsRequiredLinkReportFile, a => a.Order(11))
                            .Add(a => a.TaskParent, a => a.Order(17).TabName(CaptionHelper.DefaultTabName))
                            .Add(a => a.Project, a => a.Order(18))
                        )
                        .DefaultSettings((uow, r, commonEditorViewModel) =>
                        {
                            r.IsTemplate = true;
                        })
                    )
                    ;
        }

        #endregion

    }
}
