using Base;
using Base.UI;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Helpers;
using CorpProp.Services.ProjectActivity;
using System.Linq;

namespace CorpProp.Model.ProjectActivity
{
    public static class ProjectModel
    {
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<SibProject>("SibProjectMenuList")
                .Service<ISibProjectService>()
                .Title("Проект")
                .ListView_Default()
                .DetailView_Default()
                .LookupProperty(lp => lp.Text(t => t.Title)
            )
            ;

            context.CreateVmConfig<SibProject>("SibProjectTemplate")
                .Service<ISibProjectTemplateService>()
                .Title("Шаблон проекта")
                .ListView_TemplateDefault()
                .DetailView_TemplateDefault()
                .LookupProperty(lp => lp.Text(t => t.Title)
            )
            ;
                
        }

        public static ViewModelConfigBuilder<SibProject> ListView_Default(this ViewModelConfigBuilder<SibProject> conf)
        {
            return
                conf.ListView(lv => lv
                    .Title("Проекты")
                    .Columns(c => c
                        .Clear()
                        .Add(a => a.ProjectNumber, h => h.Visible(true).Order(1))
                        .Add(a => a.Title, h => h.Visible(true).Order(2))
                        .Add(a => a.DateFrom, h => h.Visible(true).Order(3))
                        .Add(a => a.DateTo, h => h.Visible(true).Order(4))
                        .Add(a => a.Status, h => h.Visible(true).Order(5))
                        .Add(a => a.Initiator, h => h.Visible(true).Order(6))
                    )
                    //.Toolbar(factory => factory.Add("GetSibPermissions", "SibPermission"))
                    .DataSource(ds => ds
                        .Filter(f =>
                            !f.Hidden
                            && !f.IsTemplate
                        )
                    )
                );
        }

        public static ViewModelConfigBuilder<SibProject> ListView_TemplateDefault(this ViewModelConfigBuilder<SibProject> conf)
        {
            return
                conf.ListView_Default()
                    .ListView(lv => lv
                        .Title("Шаблоны проектов")
                        .Columns(c => c
                            .Add(a => a.ProjectNumber, h => h.Visible(false))
                        )
                        .DataSource(ds => ds
                            .Filter(f =>
                                f.IsTemplate
                                && !f.Hidden
                            )
                        )
                    )
                    ;
        }

        public static ViewModelConfigBuilder<SibProject> DetailView_Default(this ViewModelConfigBuilder<SibProject> conf)
        {
            return
                conf.DetailView(dv => dv
                    .Editors(e => e
                    .Add(a => a.Title, a => a.Order(1).TabName(CaptionHelper.DefaultTabName).IsRequired(true))
                    .Add(a => a.ProjectNumber, a => a.Order(2))
                    .Add(a => a.DateFrom, a => a.Order(3))
                    .Add(a => a.DateTo, a => a.Order(4))
                    .Add(a => a.SibProjectType, a => a.Order(5))
                    .Add(a => a.Status, a => a.Order(6))
                    .Add(a => a.Description, a => a.Order(7))
                    .Add(a => a.Initiator, a => a.Order(8))
                    .AddOneToManyAssociation<SibTask>("SibProject_SibTask", edt => edt
                        .TabName("Задачи")
                        .Create((uofw, entity, id) =>
                        {
                            entity.Project = uofw.GetRepository<SibProject>().Find(id);
                        })
                        .Delete((uofw, entity, id) =>
                        {
                            entity.Project = null;
                        })
                        .Filter((uofw, q, id, oid) => q.Where(w => w.ProjectID == id && !w.IsTemplate && !w.Hidden))
                        .Mnemonic("SibTaskTreeList")
                        //.Type(EditorAssociationType.InLine)
                    )
                    .AddOneToManyAssociation<SibTask>("SibProject_SibTaskGantt", b => b
                        .TabName("[2]Диаграмма Ганта")
                        .Create((work, task, id) =>
                        {
                            task.Project = work.GetRepository<SibProject>()
                            .All()
                            .Single(s => s.ID == id);
                        })
                        .Delete((uofw, entity, id) =>
                        {
                            entity.Project = null;
                        })
                        .Filter((uofw, query, id, oid) => query.Where(w => w.ProjectID == id && !w.IsTemplate && !w.Hidden))
                        .Mnemonic("SibTaskGantt")
                        .Controller("listview")
                    )
                    .AddManyToManyRigthAssociation<FileCardAndSibProject>("SibProject_FileCards", edt => edt.TabName("[3]Документы")))
                );
        }

        public static ViewModelConfigBuilder<SibProject> DetailView_TemplateDefault(this ViewModelConfigBuilder<SibProject> conf)
        {
            return
                conf.DetailView_Default()
                    .DetailView(dv => dv
                        .Editors(e => e
                            .Clear()
                            .Add(a => a.Title, a => a.Order(1).TabName(CaptionHelper.DefaultTabName))
                            .Add(a => a.ProjectNumber, a => a.Order(2).Visible(false))
                            .Add(a => a.DateFrom, a => a.Order(3).IsRequired(false))
                            .Add(a => a.DateTo, a => a.Order(4))
                            .Add(a => a.SibProjectType, a => a.Order(5))
                            .Add(a => a.Status, a => a.Order(6))
                            .Add(a => a.Description, a => a.Order(7))
                            .Add(a => a.Initiator, a => a.Order(8))
                            .AddOneToManyAssociation<SibTask>("SibProjectTemplate_SibTaskTemplate", edt => edt
                                .TabName("Задачи")
                                .Mnemonic("SibTaskTemplateTree")
                                .Create((uofw, entity, id) =>
                                {
                                    entity.Project = uofw.GetRepository<SibProject>().Find(id);
                                })
                                .Delete((uofw, entity, id) =>
                                {
                                    entity.Project = null;
                                })
                                .Filter((uofw, q, id, oid) => q.Where(w => w.ProjectID == id))
                                .Type(EditorAssociationType.InLine)
                            )
                        )
                    )
                ;
        }
    }
}
