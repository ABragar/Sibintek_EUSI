using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Abstract;
using Base.BusinessProcesses.Wizard;
using Base.Security;
using Base.Task.Entities;
using Base.Task.Services.Abstract;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.ViewModal;
using Base.Utils.Common;
using System;
using System.Collections.Generic;

namespace Base.BusinessProcesses
{

    public class Initializer : IModuleInitializer
    {
        public Initializer()
        {

        }

        public void Init(IInitializerContext context)
        {

            context.ProcessConfigs(x =>
            {
                foreach (var config in x.GetAllVmConfigs())
                {
                    config.AddBusinessProcessesToolbarIfNeeded();
                }
            });

            context.CreateVmConfig<Workflow>("BPWorkflow")
                .Service<IWorkflowService>()
                .Title("Бизнес процессы")
                .ListView(x => x.Title("Бизнес процессы")
                    .Columns(cols => cols
                        .Add(c => c.ObjectType, c => c.DataType(PropertyDataType.ObjectType).FilterMulti(true)))
                    .ConditionAppearence(a => a
                        .Add("IsDefault == true", "\\#5cb85c")
                        .Add("IsDefault == false", "\\#9999ad")))
                .DetailView(x => x.Title("Бизнес процесс")
                    .Select((uofw, c) =>
                    {
                        var obj = ObjectHelper.CreateAndCopyObject<Workflow>(c, new[] { typeof(IBaseObject) });

                        obj.Creator = c.Creator?.Copy(uofw);
                        obj.Curator = c.Curator?.Copy(uofw);

                        if (c.CuratorsCategory != null)
                        {
                            obj.CuratorsCategory = new UserCategory()
                            {
                                ID = c.CuratorsCategory.ID,
                                Name = c.CuratorsCategory.Name
                            };
                        }

                        if (c.BaseTaskCategory != null)
                        {
                            obj.BaseTaskCategory = new BaseTaskCategory()
                            {
                                ID = c.BaseTaskCategory.ID,
                                Title = c.BaseTaskCategory.Title
                            };
                        }

                        obj.WorkflowImplementations = new List<WorkflowImplementation>();

                        if (c.WorkflowImplementations != null)
                            foreach (var workflowImplementation in c.WorkflowImplementations)
                            {
                                obj.WorkflowImplementations.Add(new WorkflowImplementation()
                                {
                                    ID = workflowImplementation.ID,
                                    Hidden = workflowImplementation.Hidden,
                                    SortOrder = workflowImplementation.SortOrder,
                                    Version = workflowImplementation.Version,
                                    CreateDate = workflowImplementation.CreateDate,
                                    LastChangeDate = workflowImplementation.LastChangeDate,
                                    IsDraft = workflowImplementation.IsDraft
                                });
                            }

                        return obj;
                    })
                    .Wizard("WorkflowWizard")
                );

            context.CreateVmConfig<Workflow>("BPTemplate")
             .Service<IWorkflowService>()
             .Title("Бизнес процессы - шаблон")
             .DetailView(x => x
                .Title("Бизнес процесс")
                 .Select((uofw, c) =>
                 {
                     var obj = ObjectHelper.CreateAndCopyObject<Workflow>(c, new[] { typeof(IBaseObject) });

                     obj.Creator = c.Creator?.Copy(uofw);
                     obj.Curator = c.Curator?.Copy(uofw);

                     if (c.CuratorsCategory != null)
                     {
                         obj.CuratorsCategory = new UserCategory()
                         {
                             ID = c.CuratorsCategory.ID,
                             Name = c.CuratorsCategory.Name
                         };
                     }

                     if (c.BaseTaskCategory != null)
                     {
                         obj.BaseTaskCategory = new BaseTaskCategory()
                         {
                             ID = c.BaseTaskCategory.ID,
                             Title = c.BaseTaskCategory.Title
                         };
                     }

                     obj.WorkflowImplementations = new List<WorkflowImplementation>();

                     if (c.WorkflowImplementations != null)
                         foreach (var workflowImplementation in c.WorkflowImplementations)
                         {
                             obj.WorkflowImplementations.Add(new WorkflowImplementation()
                             {
                                 ID = workflowImplementation.ID,
                                 Hidden = workflowImplementation.Hidden,
                                 SortOrder = workflowImplementation.SortOrder,
                                 Version = workflowImplementation.Version,
                                 CreateDate = workflowImplementation.CreateDate,
                                 LastChangeDate = workflowImplementation.LastChangeDate,
                                 IsDraft = workflowImplementation.IsDraft
                             });
                         }

                     return obj;
                 }))
             .ListView(x => x.DataSource(s => s.Filter(f => f.IsTemplate)));

            context.CreateVmConfig<BPTask>()
                .Service<IBaseTaskService<BPTask>>()
                .IsReadOnly()
                .Title("Бизнес процессы - задачи")
                .DetailView(x => x.Title("БП задача").HideToolbar(true).Height(500).Width(600).IsMaximized(false))
                .ListView(x => x.Title("БП задачи").HiddenActions(new[] { LvAction.Create }));

            context.CreateVmConfig<WorkflowCategory>()
                .Title("Бизнес процессы - Категории")
                .ListView(x => x.Title("Категории бизнес процессов"))
                .DetailView(x => x.Title("Категория бизнес процесса"));

            context.CreateVmConfig<WorkflowImplementation>("WorkflowImplementation")
                .Service<IWorkflowImplementationService>()
                .LookupProperty(x => x.Text(e => e.Title))
                .Title("Бизнес процес - Реализация")
                .ListView(x => x.Title("Бизнес процес - Реализации").HiddenActions(new[] { LvAction.Create, LvAction.Delete }))
                .DetailView(x => x
                    .Title("Версия")
                    .Select((uofw, c) =>
                    {
                        var obj = ObjectHelper.CreateAndCopyObject<WorkflowImplementation>(c, new[] { typeof(IBaseObject) });

                        obj.Creator = c.Creator?.Copy(uofw);
                        obj.EditorUser = c.EditorUser?.Copy(uofw);
                        // obj.Steps = c.Steps;
                        obj.Steps = new List<Step>();

                        foreach (var step in c.Steps)
                        {
                            //Старт
                            switch (step.StepType)
                            {
                                case FlowStepType.EntryPointStep:

                                    var entryPointStepSrc = step as EntryPointStep;

                                    var entryPointStepDest = ObjectHelper.CreateAndCopyObject<EntryPointStep>(entryPointStepSrc, new[] { typeof(IBaseObject) });

                                    entryPointStepDest.BaseOutputs = new List<Output>();

                                    foreach (var srcoutput in entryPointStepSrc.Outputs)
                                    {
                                        var destoutput = ObjectHelper.CreateAndCopyObject<StageAction>(srcoutput, new[] { typeof(IBaseObject) });
                                        entryPointStepDest.BaseOutputs.Add(destoutput);
                                    }

                                    obj.Steps.Add(entryPointStepDest);

                                    break;
                                case FlowStepType.EndStep:
                                    var endStepSrc = step as EndStep;

                                    var endStepDest = ObjectHelper.CreateAndCopyObject<EndStep>(endStepSrc, new[] { typeof(IBaseObject) });

                                    obj.Steps.Add(endStepDest);

                                    break;
                                case FlowStepType.Stage:
                                    {
                                        var stageSrc = step as Stage;

                                        var stageDest =
                                        ObjectHelper.CreateAndCopyObject<Stage>(stageSrc, new[] { typeof(IBaseObject) });

                                        stageDest.BaseOutputs = new List<Output>();

                                        foreach (var outputSrc in stageSrc.Outputs)
                                        {
                                            var outputDest =
                                                ObjectHelper.CreateAndCopyObject<StageAction>(outputSrc, new[] { typeof(IBaseObject) });

                                            outputDest.Roles = new List<ActionRole>();

                                            foreach (var outputDestRole in outputSrc.Roles)
                                            {
                                                var role = ObjectHelper.CreateAndCopyObject<ActionRole>(outputDestRole,
                                                    new[] { typeof(IBaseObject) });

                                                role.Object = new Role() { ID = outputDestRole.Object.ID, Name = outputDestRole.Object.Name };

                                                outputDest.Roles.Add(role);
                                            }

                                            stageDest.BaseOutputs.Add(outputDest);
                                        }

                                        stageDest.AssignedToCategory = new List<StageUserCategory>();

                                        foreach (var stageUserCategorySrc in stageSrc.AssignedToCategory)
                                        {
                                            var stageUserCategoryDest = new StageUserCategory()
                                            {
                                                ID = stageUserCategorySrc.ID,
                                                RowVersion = stageUserCategorySrc.RowVersion,
                                                Hidden = stageUserCategorySrc.Hidden,
                                                ObjectID = stageUserCategorySrc.ObjectID,
                                                Object = new UserCategory()
                                                {
                                                    ID = stageUserCategorySrc.Object.ID,
                                                    Name = stageUserCategorySrc.Object.Name,
                                                }
                                            };

                                            stageDest.AssignedToCategory.Add(stageUserCategoryDest);
                                        };

                                        stageDest.AssignedToUsers = new List<StageUser>();

                                        foreach (var stageUserSrc in stageSrc.AssignedToUsers)
                                        {
                                            var stageUserDest = new StageUser()
                                            {
                                                ID = stageUserSrc.ID,
                                                RowVersion = stageUserSrc.RowVersion,
                                                Hidden = stageUserSrc.Hidden,
                                                ObjectID = stageUserSrc.ObjectID,
                                                Object = stageUserSrc.Object.Copy(uofw)
                                            };

                                            stageDest.AssignedToUsers.Add(stageUserDest);
                                        };

                                        if (stageSrc.DvSetting != null)
                                        {
                                            stageDest.DvSetting = new DvSettingForType()
                                            {
                                                ID = stageSrc.DvSetting.ID,
                                                ObjectType = stageSrc.DvSetting.ObjectType,
                                                Name = stageSrc.DvSetting.Name
                                            };
                                        }

                                        obj.Steps.Add(stageDest);
                                    }
                                    break;
                                case FlowStepType.CreateObjectTask:
                                    {
                                        var createObjectStepSrc = step as CreateObjectStep;

                                        var createObjectStepDest = ObjectHelper.CreateAndCopyObject<CreateObjectStep>(createObjectStepSrc, new[] { typeof(IBaseObject) });

                                        createObjectStepDest.BaseOutputs = new List<Output>();

                                        foreach (var outputSrc in createObjectStepSrc.Outputs)
                                        {
                                            var outputDest =
                                                ObjectHelper.CreateAndCopyObject<Output>(outputSrc, new[] { typeof(IBaseObject) });

                                            createObjectStepDest.BaseOutputs.Add(outputDest);
                                        }


                                        createObjectStepDest.InitItems = new List<CreateObjectStepMemberInitItem>();

                                        foreach (var initItemSrc in createObjectStepSrc.InitItems)
                                        {
                                            var initItemDest =
                                                ObjectHelper.CreateAndCopyObject<CreateObjectStepMemberInitItem>(initItemSrc, new[] { typeof(IBaseObject) });

                                            createObjectStepDest.InitItems.Add(initItemDest);
                                        }


                                        obj.Steps.Add(createObjectStepDest);
                                    }
                                    break;
                                case FlowStepType.ChangeObjectStep:
                                    {
                                        var changeObjectStepSrc = step as ChangeObjectStep;

                                        var changeObjectStepDest = ObjectHelper.CreateAndCopyObject<ChangeObjectStep>(changeObjectStepSrc, new[] { typeof(IBaseObject) });

                                        changeObjectStepDest.BaseOutputs = new List<Output>();

                                        foreach (var outputSrc in changeObjectStepSrc.Outputs)
                                        {
                                            var outputDest =
                                                ObjectHelper.CreateAndCopyObject<Output>(outputSrc, new[] { typeof(IBaseObject) });

                                            changeObjectStepDest.BaseOutputs.Add(outputDest);
                                        }

                                        changeObjectStepDest.InitItems = new List<ChangeObjectStepInitItems>();

                                        foreach (var initItemSrc in changeObjectStepSrc.InitItems)
                                        {
                                            var initItemDest =
                                                ObjectHelper.CreateAndCopyObject<ChangeObjectStepInitItems>(initItemSrc, new[] { typeof(IBaseObject) });

                                            changeObjectStepDest.InitItems.Add(initItemDest);
                                        }

                                        obj.Steps.Add(changeObjectStepDest);
                                    }
                                    break;
                                case FlowStepType.ConditionalStep:

                                    var conditionalStepSrc = step as ConditionalStep;

                                    var conditionalStepDest = ObjectHelper.CreateAndCopyObject<ConditionalStep>(conditionalStepSrc, new[] { typeof(IBaseObject) });

                                    conditionalStepDest.BaseOutputs = new List<Output>();

                                    foreach (var outputSrc in conditionalStepSrc.Outputs)
                                    {
                                        var outputDest =
                                            ObjectHelper.CreateAndCopyObject<ConditionalBranch>(outputSrc, new[] { typeof(IBaseObject) });

                                        conditionalStepDest.BaseOutputs.Add(outputDest);

                                        foreach (var conditionalMacroItem in outputSrc.InitItems)
                                        {
                                            outputDest.InitItems.Add(ObjectHelper.CreateAndCopyObject<ConditionalMacroItem>(conditionalMacroItem, new[] { typeof(IBaseObject) }));
                                        }
                                    }

                                    obj.Steps.Add(conditionalStepDest);
                                    break;
                                case FlowStepType.Step:
                                    break;
                                case FlowStepType.Template:
                                    break;
                                case FlowStepType.BranchingStep:
                                    break;
                                case FlowStepType.ExtendedStage:
                                    break;
                                case FlowStepType.WorkflowOwnerStep:
                                    var workflowOwnerStepSrc = step as WorkflowOwnerStep;

                                    var workflowOwnerStepDest = ObjectHelper.CreateAndCopyObject<WorkflowOwnerStep>(workflowOwnerStepSrc, new[] { typeof(IBaseObject) });

                                    workflowOwnerStepDest.BaseOutputs = new List<Output>();

                                    foreach (var outputSrc in workflowOwnerStepSrc.Outputs)
                                    {
                                        var outputDest =
                                            ObjectHelper.CreateAndCopyObject<Output>(outputSrc, new[] { typeof(IBaseObject) });

                                        workflowOwnerStepDest.BaseOutputs.Add(outputDest);
                                    }

                                    if (workflowOwnerStepSrc.DvSetting != null)
                                    {
                                        workflowOwnerStepDest.DvSetting = new DvSettingForType()
                                        {
                                            ID = workflowOwnerStepSrc.DvSetting.ID,
                                            ObjectType = workflowOwnerStepSrc.DvSetting.ObjectType,
                                            Name = workflowOwnerStepSrc.DvSetting.Name
                                        };
                                    }

                                    if (workflowOwnerStepSrc.ChildWorkflowImplementation != null)
                                    {
                                        workflowOwnerStepDest.ChildWorkflowImplementation = new WorkflowImplementation()
                                        {
                                            ID = workflowOwnerStepSrc.ChildWorkflowImplementation.ID,
                                            Version = workflowOwnerStepSrc.ChildWorkflowImplementation.Version,
                                            CreateDate = workflowOwnerStepSrc.ChildWorkflowImplementation.CreateDate,
                                            Workflow = new Workflow()
                                            {
                                                ID = workflowOwnerStepSrc.ChildWorkflowImplementation.ID,
                                                Title = workflowOwnerStepSrc.ChildWorkflowImplementation.Title
                                            }
                                        };
                                    }

                                    obj.Steps.Add(workflowOwnerStepDest);

                                    break;
                                case FlowStepType.GotoStep:
                                    break;
                                case FlowStepType.ParalleizationStep:
                                    break;
                                case FlowStepType.ParallelEndStep:
                                    break;
                                case FlowStepType.ValidationStep:
                                    break;
                                case FlowStepType.MarkerStep:
                                    break;
                                default:
                                    throw new Exception($"this step ({step.StepType}) is not supported");
                            }


                        }

                        return obj;
                    }));

            context.CreateVmConfig<ConditionalBranch>()
                .Title("Условный переход - Ветка");


            context.CreateVmConfig<ChangeObjectStep>()
                .Title("Бизнес процессы - Изменение объекта")
                .Service<IChangeObjectStepService>();

            context.CreateVmConfig<ValidationStep>()
                .Title("Бизнес процессы - Валидация")
                .Service<IValidationStepService>();

            context.CreateVmConfig<ConditionalStep>()
                .Title("Условный переход")
                .Service<IConditionalStepService>();

            context.CreateVmConfig<StepValidationItem>("StepValidationItem")
                .Title("Бизнес процессы - Правила валидации на этапе");


            context.CreateVmConfig<Stage>()
                .Service<IStageService>()
                .Title("Бизнес процессы - Этапы")
                .DetailView(x => x.Title("Этап"))
                .ListView(x => x.Title("Этапы"));

            context.CreateVmConfig<BranchingStep>()
                .Service<IBranchingStepService>()
                .Title("Бизнес процессы - Разветвление")
                .ListView(x => x.Title("Шаблоны"))
                .DetailView(x => x.Title("Шаблон"));

            context.CreateVmConfig<EndStep>()
                .Service<IEndStepService>()
                .Title("Бизнес процессы - точки выхода")
                .DetailView(x => x.Title("Точка выхода"))
                .ListView(x => x.Title("Точки выхода"));

            context.CreateVmConfig<EntryPointStep>()
                .Title("Бизнес процессы - точка входа");

            context.CreateVmConfig<ParallelizationStep>()
                .Service<IParallelizationStepService>()
                .Title("Бизнес процессы - параллельное выполнение Распараллеливание")
                .DetailView(x => x.Title("Бизнес процессы - параллельное выполнение"))
                .ListView(x => x.Title("Бизнес процессы - параллельные выполнения"));

            context.CreateVmConfig<ParallelEndStep>()
                .Service<IParallelEndStepService>()
                .Title("Бизнес процессы - параллельное выполнение Параллельный конечный шаг")
                .DetailView(x => x.Title("Бизнес процессы - параллельное выполнение"))
                .ListView(x => x.Title("Бизнес процессы - параллельные выполнения"));

            context.CreateVmConfig<StageUser>()
                .Service<IStageUserService>()
                .Title("Бизнес процессы - Исполнители")
                .ListView(x => x.Title("Шаблоны"))
                .DetailView(x => x.Title("Шаблон"));

            context.CreateVmConfig<CreateObjectStep>()
                .Service<ICreateObjectStepService>()
                .Title("Бизнес процессы - Создание объекта")
                .DetailView(x => x.Title("Следующий БП"))
                .ListView(x => x.Title("Следующие БП"));


            context.CreateVmConfig<WorkflowOwnerStep>()
                .Service<IWorkflowOwnerStepService>()
                .Title("Бизнес процессы - Контейнер бизнес-процесса")
                .DetailView(x => x.Title("Контейнер бизнес-процесса"))
                .ListView(x => x.Title("Контейнеры бизнес-процесса"));

            context.CreateVmConfig<Weekend>()
                .Service<IWeekendService>()
                .Title("Производственный календарь")
                .DetailView(x => x.Title("Производственный календарь"))
                .ListView(x => x.Title("Календарь"));

            context.CreateVmConfig<StageAction>()
                .Title("Действия")
                .DetailView(x => x.Title("Действия - этап"))
                .ListView(x => x.Title("Действия"));

            context.CreateVmConfig<Branch>()
                .Title("Бизнес процессы - Ветви")
                .DetailView(x => x.Title("Ветвь"));

            context.CreateVmConfig<Output>()
                .Title("Бизнес процессы - Выход")
                .DetailView(x => x.Title("Выход"));


            context.CreateVmConfig<ActionComment>("ActionComment")
                .Title("Бизнес процессы - Комментарий")
                .DetailView(x => x.Title("Комментарий")
                    .Width(600)
                    .Height(200));

            context.CreateVmConfig<ActionComment>("ActionCommentRequired")
                .Title("Бизнес процессы - Комментарий")
                .DetailView(x => x.Title("Комментарий")
                    .Width(600)
                    .Height(240)
                    .Editors(e => e.Add(a => a.Message, v => v.IsRequired(true))));

            context.CreateVmConfig<ActionComment>("ActionCommentWithFile")
                .Title("Бизнес процессы - Комментарий")
                .DetailView(x => x.Title("Комментарий")
                    .Width(600)
                    .Height(400)
                    .Editors(e => e.Add(a => a.File, v => v.Visible(true))));

            context.CreateVmConfig<ActionComment>("ActionCommentRequiredWithFile")
                .Title("Бизнес процессы - Комментарий")
                .DetailView(x => x.Title("Комментарий")
                    .Width(600)
                    .Height(400)
                    .Editors(e =>
                    {
                        e.Add(a => a.Message, v => v.IsRequired(true));
                        e.Add(a => a.File, v => v.Visible(true));
                    }));


            //workflowwizard
            context.CreateVmConfig<WorkflowWizard>()
                .Service<IWorkflowWizardService>()
                .Title("Мастер - создание бизнес-процесса")
                .WizzardDetailView(w => w.Steps(steps =>
                {
                    steps.Add("first", s => s.StepProperties(prs =>
                    {
                        prs.Add(p => p.Title);
                        prs.Add(p => p.ObjectType);
                        prs.Add(p => p.BaseTaskCategory);
                        prs.Add(p => p.Curator);
                    }));
                    steps.Add("second", s => s.StepProperties(prs =>
                    {
                        prs.Add(p => p.Description);
                        prs.Add(p => p.CuratorsCategory);
                        prs.Add(p => p.PerformancePeriod);
                        prs.Add(p => p.SystemName);
                        prs.Add(p => p.IsDefault);
                    }));
                }));

        }
    }
}
