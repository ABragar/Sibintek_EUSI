using System.Linq;
using Base.Ambient;
using Base.DAL;
using Base.Settings;
using Base.Task;
using Base.Task.Entities;
using Base.Task.Services;
using Base.Task.Services.Abstract;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Task
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<BaseTask>()
                .Service<IBaseTaskService<BaseTask>>()
                .Title("Задачи - Базовая задача")
                .ListView(x => x
                    .Title("Задачи")
                    .Type(ListViewType.Grid)
                    .DataSource(d => d
                        .Groups(g => g
                            .Groupable(true)
                            .Add(w => w.BaseTaskCategory)))
                    .AddTaskConditionAppearence()
                    .Columns(c => c.Add(t => t.AssignedFrom, h => h.Visible(true))
                        .Add(t => t.AssignedTo, h => h.Visible(true))
                        .Add(t => t.ID, p => p.Visible(false))))
                .AddTaskToolbar()
                .DetailView(x => x.Title("Задача"))
                .Preview(p => p
                    .Select((uofw, task) =>
                    {
                        return new
                        {
                            Title = task.Title,
                            Creator = task.AssignedFrom != null ? task.AssignedFrom.FullName : "",
                            AssignedTo = task.AssignedTo != null ? task.AssignedTo.FullName : ""
                        };
                    })
                    .Fields(w => w
                        .Add(a => a.Title, builder => builder.Title("Наименование"))
                        .Add(a => a.Creator, builder => builder.Title("Автор"))
                        .Add(a => a.AssignedTo, builder => builder.Title("Ответственный"))));


            context.CreateVmConfig<Entities.Task>()
                .Service<ITaskService>()
                .Title("Задачи")
                .ListView(l => l.Title("Задачи").Type(ListViewType.Grid))
                .DetailView(x => x
                    .Title("Задача")
                     .DefaultSettings((uow, o, commonEditorViewModel) =>
                     {
                         if (o.ID != 0)
                         {
                             if (o.AssignedFromID != AppContext.SecurityUser.ID &&
                                 o.AssignedToID != AppContext.SecurityUser.ID &&
                                 o.AssignedToUsers.All(w => w.ObjectID != AppContext.SecurityUser.ID))
                             {
                                 commonEditorViewModel.SetReadOnlyAll();
                             }
                             else
                             {
                                 if ((o.AssignedToID == AppContext.SecurityUser.ID || o.AssignedToUsers.Any(w => w.ObjectID == AppContext.SecurityUser.ID)) &&
                                     o.AssignedFromID != AppContext.SecurityUser.ID)
                                 {
                                     if (!o.CanChangePerformer)
                                     {
                                         commonEditorViewModel.ReadOnly(l => l.AssignedTo, true);
                                     }
                                     if (!o.CanPerformerChangeDate)
                                     {
                                         commonEditorViewModel.ReadOnly(l => l.Period, true);
                                     }
                                     commonEditorViewModel.ReadOnly(l => l.AssignedFrom, true)
                                         .ReadOnly(l => l.Description, true)
                                         .ReadOnly(l => l.Name, true)
                                         .ReadOnly(l => l.Priority, true)
                                         .Visible(l => l.CanPerformerChangeDate, false)
                                         .Visible(l => l.CanChangePerformer, false);
                                 }
                                 if (o.AssignedFromID == AppContext.SecurityUser.ID)
                                 {
                                     commonEditorViewModel
                                         .Required(l => l.AssignedTo, true)
                                         .ReadOnly(l => l.AssignedFrom, true);
                                 }
                             }

                             if (o.CanPerformerChangeDate &&
                                 (o.AssignedTo != null && o.AssignedTo.ID == Base.Ambient.AppContext.SecurityUser.ID ||
                                  o.AssignedToUsers.Any(w => w.ObjectID == AppContext.SecurityUser.ID)))
                             {
                                 commonEditorViewModel.ReadOnly(w => w.Period, false);
                             }
                             else
                             {
                                 commonEditorViewModel.ReadOnly(w => w.Period);
                             }
                         }

                         if (o.AssignedFrom == null || o.AssignedFrom.ID != Base.Ambient.AppContext.SecurityUser.ID)
                         {
                             commonEditorViewModel.ReadOnly(w => w.CanPerformerChangeDate);
                             commonEditorViewModel.ReadOnly(w => w.CanChangePerformer);
                             commonEditorViewModel.ReadOnly(w => w.Color);
                         }
                         
                         commonEditorViewModel.Visible(p => p.CompliteDate, o.Status == TaskStatus.Complete);
                     }))
                .Preview(p => p
                    .Select((uofw, project) =>
                    {
                        return new
                        {
                            Title = project.Title,
                            Creator = project.AssignedFrom != null ? project.AssignedFrom.FullName : "",
                            AssignedTo = project.AssignedTo != null ? project.AssignedTo.FullName : ""
                        };
                    })
                    .Fields(w => w
                        .Add(a => a.Title, builder => builder.Title("Наименование"))
                        .Add(a => a.Creator, builder => builder.Title("Автор"))
                        .Add(a => a.AssignedTo, builder => builder.Title("Ответственный"))));

            context.CreateVmConfigOnBase<BaseTask>("BaseTask", "TaskGantt")
                .Title("Задачи - Гантт")
                .ListView(l => l.Type(ListViewType.Gantt));

            context.CreateVmConfigOnBase<BaseTask>("BaseTask", "OutTask")
                .Title("Задачи - Исходящие задачи")
                .ListView(l => l
                    .Type(ListViewType.Grid)
                    .DataSource(d => d.Filter(f => f.AssignedFromID == Base.Ambient.AppContext.SecurityUser.ID)));

            context.CreateVmConfigOnBase<BaseTask>("BaseTask", "InTask")
                .Title("Задачи - Входящие задачи")
                .ListView(l => l
                    .Type(ListViewType.Grid)
                    .DataSource(d => d.Filter(f => f.AssignedToID == Base.Ambient.AppContext.SecurityUser.ID ||
                                                   f.AssignedToUsers.Any(
                                                       w => w.ObjectID == Base.Ambient.AppContext.SecurityUser.ID))));

            context.CreateVmConfigOnBase<BaseTask>("BaseTask", "TaskScheduller")
                .Title("Задачи - Календарь")
                .ListView(l => l.Type(ListViewType.Scheduler));

            context.CreateVmConfigOnBase<BaseTask>("BaseTask", "TaskForProject")
                .Title("Задачи - Задачи без категории (для проекта)")
                .ListView(l => l
                    .DataSource(d => d.Groups(w => w.Groupable(false)))
                    .Columns(c => c.Add(w => w.BaseTaskCategory, w => w.Visible(false))));

            context.CreateVmConfig<BaseTaskCategory>()
                .Service<IBaseTaskCategoryService>()
                .Title("Задачи - Базовые категории")
                 .ListView(x => x.Title("Категории"))
                .DetailView(x => x.Title("Категория"));

            context.CreateVmConfig<TaskCategory>()
                .Title("Задачи - Категории")
                .ListView(x => x.Title("Категории"))
                .DetailView(x => x.Title("Категория"))
                .Preview(p => p
                    .Select((uofw, category) =>
                    {
                        return new
                        {
                            Title = category.Title,
                        };
                    })
                    .Fields(w => w.Add(a => a.Title, builder => builder.Title("Наименование"))));

            context.CreateVmConfig<BaseTaskDependency>()
                .Title("Задачи - связи");
        }
    }
}
