using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security;
using Base.Service;
using Base.BusinessProcesses.Exceptions;
using Base.Enums;
using Base.Service.Crud;
using Base.Task.Entities;
using AppContext = Base.Ambient.AppContext;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class WorkflowContextService : BaseObjectService<WorkflowContext>, IWorkflowContextService
    {
        private readonly IStageUserService _stageUserService;
        private readonly IProductionCalendarService _calendarService;
        private readonly ITaskServiceFacade _taskService;


        public WorkflowContextService(IBaseObjectServiceFacade facade, IStageUserService userService, IProductionCalendarService calendarService, ITaskServiceFacade taskService)
            : base(facade)
        {
            _stageUserService = userService;
            _calendarService = calendarService;
            _taskService = taskService;
        }

        public List<StagePerform> GetCurrentStages(IUnitOfWork uinitOfWork, IBPObject obj)
        {
            var context = obj.WorkflowContext ?? uinitOfWork.GetRepository<WorkflowContext>()
                .All()
                .FirstOrDefault(x => x.ID == obj.WorkflowContextID && !x.Hidden);

            if (context == null)
                throw new Exception("Не удалось найти контекст исполнения");

            var currentStages = context.CurrentStages.ToList();

            if (!AppContext.SecurityUser.IsAdmin)
            {
                var outputs = currentStages.SelectMany(x => x.Stage.Outputs);
                foreach (var stageAction in outputs)
                {
                    if (!CurrentUserInActionRoles(stageAction))
                        stageAction.Hidden = true;
                }
            }
            return currentStages;
        }

        private bool CurrentUserInActionRoles(StageAction action)
        {
            var rolesID = action.Roles.Select(x => x.ObjectID).ToList();
            if (rolesID.Any())
            {
                return rolesID.Any(roleid => AppContext.SecurityUser.IsRole(roleid.GetValueOrDefault()));
            }
            return true;
        }

        public PerformerType GetPerformerType(IUnitOfWork unitOfWork, StagePerform perform, InvokeStageContext stageContext)
        {
            var performer = PerformerType.Denied;
            var stage = perform.Stage;

            if (unitOfWork is ISystemUnitOfWork || AppContext.SecurityUser.IsAdmin || AppContext.SecurityUser.IsSysRole(SystemRole.AdminWF))
            {
                performer = PerformerType.Admin;
            }
            else if (stage.WorkflowImplementation.Workflow.CuratorID == AppContext.SecurityUser.ID)
            {
                performer = PerformerType.Curator;
            }
            else if (perform.PerformUserID == AppContext.SecurityUser.ID)
            {
                performer = PerformerType.Performer;
            }
            else if (_stageUserService.GetStakeholders(unitOfWork, perform.Stage, stageContext.BPObject).Any(x => x.ID == AppContext.SecurityUser.ID))
            {
                performer = PerformerType.Regular;
            }

            return performer;
        }

        public void AutoTakeForPerform(IUnitOfWork unitOfWork, StagePerform perform, InvokeStageContext stageContext)
        {
            if (stageContext.PerformUserID != null)
            {
                TakeForPerformImpl(unitOfWork, stageContext.BPObject, stageContext.PerformUserID.Value, perform, true);
            }
            else
            {
                if (perform.Tasks.Count() == 1)
                {
                    var task = perform.Tasks.First();
                    TakeForPerformImpl(unitOfWork, stageContext.BPObject, task.AssignedTo.ID, perform, true);
                }
                else
                {
                    var single = perform.Tasks.SingleOrDefault(x => x.AssignedTo.ID == AppContext.SecurityUser.ID);
                    if (single != null)
                        TakeForPerformImpl(unitOfWork, stageContext.BPObject, single.AssignedTo.ID, perform, false);
                }
            }

         //   _taskService.ProcessTasks(unitOfWork, perform.Tasks);
        }

        public void TakeForPerform(IUnitOfWork unitOfWork, IBaseObjectCrudService objectService, int? userID, int? performID, int objectID)
        {
            bool isForcePerform = userID.HasValue;

            if (userID == null)
                userID = AppContext.SecurityUser.ID;

            var obj = (IBPObject)objectService.Get(unitOfWork, objectID);

            var context = obj.WorkflowContext ?? unitOfWork.GetRepository<WorkflowContext>().All().FirstOrDefault(x => x.ID == obj.WorkflowContextID);

            if (context == null)
                throw new Exception("Не удалось найти контекст исполнения");

            var stageContext = new InvokeStageContext
            {
                BPObject = obj
            };

            var perform = context.CurrentStages.FirstOrDefault(x => x.ID == performID);

            if (perform == null)
                throw ExceptionHelper.ActionInvokeException("Не удалось найти этап");

            if (GetPerformerType(unitOfWork, perform, stageContext) == PerformerType.Denied)
                throw ExceptionHelper.ActionInvokeException("Доступ запрещен");

            if (perform?.PerformUser != null)
                throw ExceptionHelper.ActionInvokeException("Объект уже на исполнеии у другого пользователя");

            TakeForPerformImpl(unitOfWork, obj, userID.Value, perform, isForcePerform);

            _taskService.ProcessTasks(unitOfWork, perform.Tasks);

            objectService.Update(unitOfWork, (BaseObject)obj);
        }

        private void TakeForPerformImpl(IUnitOfWork uow, IBPObject obj, int userID, StagePerform perform, bool isForcePerform)
        {
            perform.PerformUserID = userID;
            perform.FromUserID = AppContext.SecurityUser.ID;

            foreach (var task in perform.Tasks.Where(x => x.AssignedTo.ID != userID))
            {
                task.Status = TaskStatus.NotRelevant;
            }

            var performerTask = perform.Tasks.FirstOrDefault(x => x.AssignedTo.ID == userID);

            if (performerTask == null)
            {
                performerTask = _taskService.CreateBPTask(uow, AppContext.SecurityUser.ID, userID, perform.Stage, obj, AppContext.DateTime.Now);
                perform.Tasks.Add(performerTask);
            }

            performerTask.Status = TaskStatus.InProcess;
            if (isForcePerform)
            {
                performerTask.ForcedTask = true;
                performerTask.Status = TaskStatus.New;
            }
        }

        public IEnumerable<BPTask> GetTasksForAllStageUsers(IUnitOfWork unitOfWork, Stage stage, IBPObject baseObject, DateTime dt)
        {
            return _stageUserService.GetStakeholders(unitOfWork, stage, baseObject)
                .Take(100)
                .ToList()
                .Select(user => _taskService.CreateBPTask(unitOfWork, AppContext.SecurityUser.ID, user.ID, stage, baseObject, dt));
        }

        public void ReleasePerform(IUnitOfWork unitOfWork, IBaseObjectCrudService objectService, int performID, int objectID)
        {
            var obj = (IBPObject)objectService.Get(unitOfWork, objectID);

            var stageContext = new InvokeStageContext
            {
                BPObject = obj
            };

            var context = obj.WorkflowContext ?? unitOfWork.GetRepository<WorkflowContext>().All().FirstOrDefault(x => x.ID == obj.WorkflowContextID);

            if (context == null)
                throw new Exception("Не удалост найти контекст исполнения");

            var perform = context.CurrentStages.Single(x => x.ID == performID);

            var performerType = GetPerformerType(unitOfWork, perform, stageContext);
            if (performerType == PerformerType.Admin || performerType == PerformerType.Performer || performerType == PerformerType.Curator)
            {
                if (perform != null)
                {
                    if (perform.Tasks != null)
                    {
                        foreach (var bpTask in perform.Tasks)
                        {
                            bpTask.Status = bpTask.AssignedTo == perform.PerformUser ? TaskStatus.Abolished : TaskStatus.New;
                        }
                    }

                    perform.PerformUser = null;
                    perform.PerformUserID = null;


                    _taskService.ProcessTasks(unitOfWork, perform.Tasks);
                }

                objectService.Update(unitOfWork, (BaseObject)obj);
            }
            else
            {
                throw ExceptionHelper.ActionInvokeException("Объект уже на исполнении у другого пользователя");
            }
        }

        public void UpdateContext(IUnitOfWork uow, WorkflowContext context, StagePerform oldPerform, ICollection<StagePerform> newPerforms)
        {
            if (context.CurrentStages == null)
                context.CurrentStages = new List<StagePerform>();

            if (oldPerform != null)
            {
                oldPerform.Hidden = true;
                context.CurrentStages.Remove(oldPerform);
            }

            foreach (var newPerform in newPerforms)
            {
                SetEndDate(newPerform);
                context.CurrentStages.Add(newPerform);
            }

            uow.GetRepository<WorkflowContext>().Update(context);
            
            uow.SaveChanges();


        }

        public bool CanSelectPreformer(IUnitOfWork uow, int stageID)
        {
            var stage = uow.GetRepository<Stage>().Find(stageID);
            if (stage == null)
                throw new Exception("Этап не найден");

            return stage.IsCustomPerformer;
        }

        private void SetEndDate(StagePerform perform)
        {
            if (perform.Stage.AutoProcess && perform.Stage.Outputs.Any(x => x.IsDefaultAction))
            {
                var ts = TimeSpan.FromMinutes(perform.Stage.PerformancePeriod);

                perform.EndDate = _calendarService.GetEndDate(perform.BeginDate, ts, perform.Stage.PerfomancePeriodType);
            }
        }

        protected override IObjectSaver<WorkflowContext> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<WorkflowContext> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.CurrentStages, x => x
                    .SaveOneObject(o => o.FromUser)
                    .SaveOneObject(o => o.PerformUser)
                    .SaveOneObject(o => o.Stage)
                    .SaveOneObject(o => o.Position));
        }

    }
}
