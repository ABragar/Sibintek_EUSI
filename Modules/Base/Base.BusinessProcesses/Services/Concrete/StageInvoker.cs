using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Exceptions;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Macros.Entities;
using Base.Service.Crud;
using Base.Utils.Common.Maybe;
using AppContext = Base.Ambient.AppContext;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class StageInvoker : IStageInvoker
    {
        private readonly IChangeHistoryService _changeHistoryService;
        private readonly IWorkflowContextService _workflowContextService;
        private readonly IWorkflowServiceFacade _workflowServiceFacade;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ITaskServiceFacade _taskServiceFacade;

        public StageInvoker(IChangeHistoryService changeHistoryService, IWorkflowContextService workflowContextService, IUnitOfWorkFactory unitOfWorkFactory, IWorkflowServiceFacade workflowServiceFacade, ITaskServiceFacade taskServiceFacade)
        {
            _changeHistoryService = changeHistoryService;
            _workflowContextService = workflowContextService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _workflowServiceFacade = workflowServiceFacade;
            _taskServiceFacade = taskServiceFacade;
        }

        private ICollection<StagePerform> MoveToNextStage(IUnitOfWork unitOfWork, InvokeStageContext stageContext, ref double sortOrder)
        {
            var stageJumper = FindNextStage(unitOfWork, stageContext.Action, stageContext.BPObject);

            _changeHistoryService.WriteStepsBetweenStages(unitOfWork, stageJumper.Steps.ToList(), ref sortOrder, stageContext);

            var nextStages = stageJumper.StagePerforms;

            foreach (var nextStage in nextStages)
            {
                if (nextStage.Stage.CreateTask)
                {
                    nextStage.Tasks = _workflowContextService.GetTasksForAllStageUsers(unitOfWork, nextStage.Stage, stageContext.BPObject, AppContext.DateTime.Now).ToList();
                }
                else if (stageContext.PerformUserID.HasValue)
                {
                    nextStage.Tasks.Add(_taskServiceFacade.CreateBPTask(unitOfWork, AppContext.SecurityUser.ID,
                        stageContext.PerformUserID.Value, nextStage.Stage, stageContext.BPObject,
                        AppContext.DateTime.Now));
                }
                _workflowContextService.AutoTakeForPerform(unitOfWork, nextStage, stageContext);
            }
            return stageJumper.StagePerforms;
        }

        public StageJumper FindNextStage(IUnitOfWork unitOfWork, Output output, IBPObject obj)
        {
            var workflowContext = obj.WorkflowContext ??
                                  unitOfWork.GetRepository<WorkflowContext>()
                                      .All()
                                      .FirstOrDefault(x => x.ID == obj.WorkflowContextID);

            if (workflowContext == null)
                throw new Exception("Не удалось найти контекст исполнения");

            var stagePerform = workflowContext.CurrentStages.FirstOrDefault(x => x.StageID == output.Step.ID);
            if (stagePerform == null)
                throw new Exception("Не удалось найти этап в контексте");

            WorkflowHierarchyPosition position = stagePerform.Position;

            StageJumper jumper = new StageJumper() { WorkflowPosition = position };
            return FindNextStageImpl(unitOfWork, output, obj, jumper);
        }

        private StageJumper FindNextStageImpl(IUnitOfWork unitOfWork, Output output, IBPObject obj, StageJumper stageJumper)
        {
            var stepType = output.NextStep.StepType;

            switch (stepType)
            {
                case FlowStepType.Stage:
                    return ProcessStage(unitOfWork, output, stageJumper);

                case FlowStepType.EndStep:
                    return ProcessEndStep(unitOfWork, output, obj, stageJumper);

                case FlowStepType.BranchingStep:
                    return ProcessBranchingStep(unitOfWork, (BranchingStep)output.NextStep, obj, stageJumper);

                case FlowStepType.CreateObjectTask:
                    return ProcessCreateObjectStep(unitOfWork, output, obj, stageJumper);

                case FlowStepType.WorkflowOwnerStep:
                    return ProcessWorkflowOwnerStep(unitOfWork, output, obj, stageJumper);

                case FlowStepType.ParalleizationStep:
                    return ProcessParallelStep(unitOfWork, output, obj, stageJumper);

                case FlowStepType.ParallelEndStep:
                    return ProcessParallelEndStep(unitOfWork, output, obj, stageJumper);

                case FlowStepType.ChangeObjectStep:
                    return ProcessChangeObjectStep(unitOfWork, output, obj, stageJumper);

                case FlowStepType.ValidationStep:
                    return ProcessValidationStep(unitOfWork, output, obj, stageJumper);

                case FlowStepType.ConditionalStep:
                    return ProcessConditionalStep(unitOfWork, output, obj, stageJumper);
            }

            throw new Exception("Не удалось найти след шаг");
        }

        private StageJumper ProcessValidationStep(IUnitOfWork unitOfWork, Output ouput, IBPObject obj, StageJumper stageJumper)
        {
            var step = ouput.NextStep as ValidationStep;
            return FindNextStageImpl(unitOfWork, step.Outputs.FirstOrDefault(), obj, stageJumper);
        }

        private StageJumper ProcessChangeObjectStep(IUnitOfWork unitOfWork, Output ouput, IBPObject obj, StageJumper stageJumper)
        {
            var step = (ChangeObjectStep)ouput.NextStep;
            _workflowServiceFacade.ModifyObject(AppContext.SecurityUser, (BaseObject)obj, step.InitItems);
            return FindNextStageImpl(unitOfWork, step.Outputs.FirstOrDefault(), obj, stageJumper);
        }

        private StageJumper ProcessParallelEndStep(IUnitOfWork unitOfWork, Output output, IBPObject obj, StageJumper stageJumper)
        {
            var endParallelStep = (ParallelEndStep)output.NextStep;
            if (endParallelStep.WaitAllThreads)
            {
                if (obj.WorkflowContext.CurrentStages.Count == 1)// Тип если в текщих этапах тока один то отдаем след                    
                {
                    stageJumper.Steps.Add(endParallelStep);
                    return FindNextStageImpl(unitOfWork, endParallelStep.Outputs.FirstOrDefault(), (IBPObject)obj, stageJumper);
                }
                var stages = stageJumper.Steps.Where(x => x.StepType == FlowStepType.Stage);
                foreach (var step in stages)
                {
                    stageJumper.Steps.Remove(step);
                }
                return stageJumper;
            }

            stageJumper.Steps.Add(endParallelStep);
            return FindNextStageImpl(unitOfWork, endParallelStep.Outputs.FirstOrDefault(), (IBPObject)obj, stageJumper);

        }

        private StageJumper ProcessParallelStep(IUnitOfWork unitOfWork, Output output, IBPObject obj, StageJumper stageJumper)
        {
            ParallelizationStep step = output.NextStep as ParallelizationStep;
            if (step == null)
                throw new NullReferenceException("Paralell not paralell");
            stageJumper.Steps.Add(step);
            foreach (var so in step.Outputs)
            {
                FindNextStageImpl(unitOfWork, so, obj, stageJumper);
            }
            return stageJumper;
        }

        private StageJumper ProcessWorkflowOwnerStep(IUnitOfWork unitOfWork, Output output, IBPObject obj, StageJumper stageJumper)
        {
            var owner = (WorkflowOwnerStep)output.NextStep;
            stageJumper.Steps.Add(owner);

            var entryStep = (EntryPointStep)owner.ChildWorkflowImplementation.Steps.FirstOrDefault(x => x.StepType == FlowStepType.EntryPointStep);
            if (entryStep != null)
            {
                var newPosition = new WorkflowHierarchyPosition { CurrentWorkflowContainer = owner, Parent = stageJumper.WorkflowPosition };
                stageJumper.WorkflowPosition = newPosition;
                stageJumper.Steps.Add(entryStep);
                return FindNextStageImpl(unitOfWork, entryStep.BaseOutputs.FirstOrDefault(), obj, stageJumper);
            }
            throw new Exception("В контейнере бизнес процесса не указан дочерний бизнес процесс");
        }

        private StageJumper ProcessCreateObjectStep(IUnitOfWork unitOfWork, Output output, IBPObject obj, StageJumper stageJumper)
        {
            var createObjectStep = output.NextStep as CreateObjectStep;
            stageJumper.Steps.Add(createObjectStep);
            return FindNextStageImpl(unitOfWork, createObjectStep.Outputs.FirstOrDefault(), obj, stageJumper);
        }

        private StageJumper ProcessBranchingStep(IUnitOfWork unitOfWork, BranchingStep step, IBPObject obj, StageJumper stageJumper)
        {
            var branchingStep = step;
            stageJumper.Steps.Add(branchingStep);
            // ReSharper disable once PossibleNullReferenceException
            var branch = branchingStep.Outputs.FirstOrDefault(x => ExecutePredicate(x, (BaseObject)obj));

            if (branch == null)
            {
                branch = branchingStep.Outputs.FirstOrDefault(x => x.IsDefaultBranch && !x.Hidden);
                if (branch == null)
                {
                    throw ExceptionHelper.ActionInvokeException(
                        "Can't resolve path, branch's predicate returns \"false\" and default branch not founded");
                }
            }

            return FindNextStageImpl(unitOfWork, branch, obj, stageJumper);


            // throw ExceptionHelper.ActionInvokeException("Can't resolve path, infinite loop");
        }

        private StageJumper ProcessStage(IUnitOfWork unitOfWork, Output output, StageJumper stageJumper)
        {
            var stage = output.NextStep as Stage;
            if (stage == null)
                throw new Exception("Stage is not stage");

            StagePerform perform = new StagePerform()
            {
                Stage = stage,
                Position = stageJumper.WorkflowPosition,
                BeginDate = AppContext.DateTime.Now,
            };
            stageJumper.StagePerforms.Add(perform);

            return stageJumper;
        }

        private StageJumper ProcessEndStep(IUnitOfWork unitOfWork, Output output, IBPObject obj, StageJumper stageJumper)
        {
            var endStep = output.NextStep as EndStep;
            if (endStep == null)
                throw new NullReferenceException("Конец не конец");

            var owner = stageJumper.WorkflowPosition.CurrentWorkflowContainer;
            stageJumper.Steps.Add(endStep);

            if (owner != null)
            {
                var coutput = owner.Outputs.FirstOrDefault();
                if (coutput == null)
                    throw new Exception("У контейнера нет выхода");

                stageJumper.WorkflowPosition = stageJumper.WorkflowPosition.Parent;
                return FindNextStageImpl(unitOfWork, coutput, obj, stageJumper);
            }
            else
            {
                var perform = new StagePerform()
                {
                    Stage = endStep,
                    Position = stageJumper.WorkflowPosition,
                    BeginDate = AppContext.DateTime.Now,
                };

                stageJumper.StagePerforms.Add(perform);  // Это конец
                return stageJumper;
            }

        }

        private StageJumper ProcessConditionalStep(IUnitOfWork unitOfWork, Output output, IBPObject obj, StageJumper stageJumper)
        {
            var conditionalStep = output.NextStep as ConditionalStep;
            if (conditionalStep == null)
                throw new Exception();

            foreach (var item in conditionalStep.Outputs.Where(x => x.Hidden == false))
            {
                if (_workflowServiceFacade.CheckBranch(unitOfWork, obj as BaseObject, item.InitItems))
                {
                    return FindNextStageImpl(unitOfWork, item, obj, stageJumper);
                }
            }

            var def = conditionalStep.Outputs.FirstOrDefault(x => x.IsDefaultBranch);
            if (def == null)
            {
                throw new Exception("Ни одна ведвь правльная, и нет действия по умолчанию");
            }
            return FindNextStageImpl(unitOfWork, def, obj, stageJumper);
        }

        private bool ExecutePredicate(Branch branch, BaseObject obj)
        {
            return GetPropertyEnumPairs(branch.BranchConditions, obj).Any(pair => pair.EqualsFor(obj));
        }

        private IEnumerable<PropertyEnumPair> GetPropertyEnumPairs(IEnumerable<InitItem> macoItems, BaseObject obj)
        {
            return macoItems.Join(obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance),
                a => a.Member, p => p.Name, (a, p) => new { Property = p, Action = a })
                .Select(x =>
                {
                    var propType = x.Property.PropertyType;

                    if (propType.IsGenericType &&
                        propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propType = propType.GetGenericArguments()[0];
                    }

                    return new
                    {
                        x.Property,
                        PropertyType = propType,
                        x.Action
                    };
                })
                .Where(x => Enum.IsDefined(x.PropertyType, x.Action.Value))
                .Select(x => new PropertyEnumPair(x.Property, Enum.Parse(x.PropertyType, x.Action.Value)));
        }

        public void InvokeStage(IUnitOfWork uofw, InvokeStageContext stageContext)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateSystem(uofw))
            {
                #region req

                var workflowContext = stageContext.BPObject.WorkflowContext ?? unitOfWork.GetRepository<WorkflowContext>().All().FirstOrDefault(x => x.ID == stageContext.BPObject.WorkflowContextID);
                if (workflowContext == null)
                    throw new Exception("Не удалось найти контекст исполнения");

                StagePerform currentPerformer = workflowContext.CurrentStages.FirstOrDefault(x => x.StageID == stageContext.Action.Step.ID);
                if (currentPerformer == null)
                    throw ExceptionHelper.ActionInvokeException("Current performer is null");

                var performerType = _workflowContextService.GetPerformerType(unitOfWork, currentPerformer, stageContext);
                if (performerType == PerformerType.Denied)
                    throw ExceptionHelper.ActionInvokeException("Доступ запрещен");

                #endregion

                double sortOrder = GetLastSortOrder(unitOfWork, stageContext.BPObject);

                _changeHistoryService.WriteStageToHistory(unitOfWork, currentPerformer, stageContext, ref sortOrder);

                if (currentPerformer.Tasks != null)
                    _taskServiceFacade.UpdateTasks(uofw, currentPerformer);

                var nextStagePerforms = MoveToNextStage(unitOfWork, stageContext, ref sortOrder);

                var wfcontext = stageContext.BPObject.With(x => x.WorkflowContext);
                _workflowContextService.UpdateContext(unitOfWork, wfcontext, currentPerformer, nextStagePerforms);
                _taskServiceFacade.CreateNotifications(uofw, nextStagePerforms.SelectMany(x=>x.Tasks).ToList(), BaseEntityState.Added);                
            }
        }

        public ICollection<StagePerform> GetNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, int actionID)
        {
            var action = unitOfWork.GetRepository<StageAction>().All().FirstOrDefault(x => x.ID == actionID);
            if (action == null) throw ExceptionHelper.ActionNotFoundException("Действие не найдено");

            return FindNextStage(unitOfWork, action, baseObject).StagePerforms;
        }

        public void ExecuteNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, StageAction action, int? assignToUserID, ref double counter)
        {
            if (!baseObject.WorkflowContextID.HasValue)
                throw new Exception("Объект без контекста БП");

            var context = _workflowContextService.Get(unitOfWork, baseObject.WorkflowContextID.Value);
            if (context == null)
                throw new Exception("Не удалось найти контекст выполенения");

            StagePerform currentPerformer = context.CurrentStages.FirstOrDefault(x => action != null && x.StageID == action.Step.ID);

            var stageContext = new InvokeStageContext()
            {
                Action = action,
                BPObject = baseObject
            };

            _changeHistoryService.WriteStageToHistory(unitOfWork, currentPerformer, stageContext, ref counter);

            var nextPerformers = MoveToNextStage(unitOfWork, stageContext, ref counter);

            if (context.CurrentStages.Count == 1 && nextPerformers.Count == 1)
            {
                StagePerform stagePerform = nextPerformers.FirstOrDefault();
                var endStep = stagePerform?.Stage;
                if (endStep?.StepType == FlowStepType.EndStep)
                {
                    stageContext = new InvokeStageContext()
                    {
                        ActionComment = new ActionComment() { Message = "Завершение бизнес процесса" }

                    };
                    var endPerform = stagePerform;
                    _changeHistoryService.WriteStageToHistory(unitOfWork, endPerform, stageContext, ref counter);
                    nextPerformers.Clear();
                }
            }

            _workflowContextService.UpdateContext(unitOfWork, context, currentPerformer, nextPerformers);

            var objService = _workflowServiceFacade.GetService(baseObject.GetType().GetBaseObjectType().FullName);
            objService.Update(unitOfWork, (BaseObject)baseObject);
        }

        private double GetLastSortOrder(IUnitOfWork unitOfWork, IBPObject obj)
        {
            try
            {
                string objectType = obj.GetType().GetBaseObjectType().GetTypeName();
                return unitOfWork.GetRepository<ChangeHistory>().All()
                    .Where(x => x.ObjectID == obj.ID && x.ObjectType == objectType)
                    .Max(x => x.SortOrder);
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        private class PropertyEnumPair
        {
            private readonly PropertyInfo _property;
            private readonly object _value;

            public PropertyEnumPair(PropertyInfo property, object value)
            {
                _property = property;
                _value = value;
            }

            public bool EqualsFor(BaseObject obj)
            {
                var value = _property.GetValue(obj);
                return value != null && value.ToString() == _value.ToString();
            }
        }
    }
}
