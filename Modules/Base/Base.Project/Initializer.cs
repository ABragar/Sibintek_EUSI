using System.Linq;
using Base.Project.Service.Abstract;
using Base.Task.Entities;
using Base.UI;

namespace Base.Project
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Entities.Project>()
                .Service<IProjectService>()
                .Title("Проект Базовый")
                .DetailView(d => d
                    .Title("Проект")
                    .Editors(e => e
                        .AddOneToManyAssociation<BaseTask>("Tasks", b => b
                            .TabName("[1]Задачи(таблица)")
                            .Create((work, task, id) =>
                            {
                                task.BaseTaskCategory = work.GetRepository<Entities.Project>()
                                    .All()
                                    .Single(x => x.ID == id);
                            })
                            .Filter((uofw, query, id, oid) => query.Where(x => x.CategoryID == id))
                            .Mnemonic("TaskForProject")
                        )
                        .AddOneToManyAssociation<BaseTask>("TaskGantt", b => b
                            .TabName("[2]Задачи(Гантт)")
                            .Create((work, task, id) =>
                            {
                                task.BaseTaskCategory = work.GetRepository<Entities.Project>()
                                    .All()
                                    .Single(x => x.ID == id);
                            })
                            .Filter((uofw, query, id, oid) => query.Where(x => x.CategoryID == id))
                            .Mnemonic("TaskGantt")
                        )
                        .AddOneToManyAssociation<BaseTask>("TaskScheduller", b => b
                            .TabName("[3]Задачи(Календарь)")
                            .Create((work, task, id) =>
                            {
                                task.BaseTaskCategory = work.GetRepository<Entities.Project>()
                                    .All()
                                    .Single(x => x.ID == id);
                            })
                            .Filter((uofw, query, id, oid) => query.Where(x => x.CategoryID == id))
                            .Mnemonic("TaskScheduller")
                        )
                    )
                )
                .ListView(l => l
                    .Title("Проекты")
                    .Columns(c => c.Add(w => w.ExtraID, w => w.Visible(false))))
                .Preview(p => p
                    .Select((uofw, project) =>
                    {
                        return new
                        {
                            Title = project.Title,
                            User = project.User != null ? project.User.FullName : "",
                            Status = project.Status
                        };
                    })
                    .Fields(w => w
                        .Add(a => a.Title, builder => builder.Title("Наименование"))
                        .Add(a => a.User, builder => builder.Title("Руководитель"))
                        .Add(a => a.Status, builder => builder.Title("Статус"))
                        ));
        }
    }
}