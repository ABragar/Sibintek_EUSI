using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Events;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Exceptions;
using Base.Macros.Entities;
using AppContext = Base.Ambient.AppContext;
using Base.Service.Crud;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Services.Concrete
{
    public enum PerformerType { Admin = 0, Curator = 1, Performer = 2, Regular = 3, Denied = 4 }

    // In transient scope for workflow transaction !
    public class WorkflowService : BaseCategorizedItemService<Workflow>, IWorkflowService
    {
        private readonly IWorkflowServiceFacade _serviceFacade;
        private readonly IWorkflowContextService _workflowContextService;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IStageInvoker _stageInvoker;        


        public WorkflowService(
            IWorkflowServiceFacade serviceFacade,
            IBaseObjectServiceFacade facade,
            IUnitOfWorkFactory uowFactory,
            IStageInvoker stageInvoker,
            IWorkflowContextService workflowContextService)
            : base(facade)
        {
            _serviceFacade = serviceFacade;
            _uowFactory = uowFactory;
            _stageInvoker = stageInvoker;
            _workflowContextService = workflowContextService;
        }


        private void StartWorkflow(ISystemUnitOfWork unitOfWork, IBaseObjectCrudService baseObjectService, IBPObject baseObject, WorkflowContext wfctx)
        {
            var action = _serviceFacade.GetEntryPoint(wfctx);
            if (action != null)
            {
                var currentStage = action.Step as Stage;
                if (currentStage == null)
                    throw new Exception("Для действия не задан этап");
                const string comment = "Запуск бизнес процесса";

                var context = (baseObject).With(x => x.WorkflowContext);
                StagePerform perform = new StagePerform
                {
                    Stage = (Stage)action.Step,
                    BeginDate = AppContext.DateTime.Now,
                    Position = new WorkflowHierarchyPosition { CurrentWorkflowContainer = null, Parent = null }
                };

                _workflowContextService.UpdateContext(unitOfWork, context, null, new List<StagePerform>() { perform });

                InvokeStage(unitOfWork, baseObjectService, baseObject, action, new ActionComment { Message = comment });
            }
            else
            {
                throw new Exception("Не найдена точка входа");
            }
        }

        public override IQueryable<Workflow> GetAllCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = false)
        {
            string strID = HCategory.IdToString(categoryID);

            return
                GetAll(unitOfWork, hidden)
                    .Where(
                        a => a.IsTemplate &&
                            (a.Category_.sys_all_parents != null && a.Category_.sys_all_parents.Contains(strID)) ||
                            a.Category_.ID == categoryID);
        }

        public override IQueryable<Workflow> GetCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = false)
        {
            return
                GetAll(unitOfWork).Where(a => a.IsTemplate && a.CategoryID == categoryID && a.Hidden == hidden);
        }

        private Workflow LoadWorkflow(IBPObject obj, IUnitOfWork unitOfWork, Type typeStr)
        {
            Workflow autoWf;

            var initWFObject = obj as IInitBPObject;

            if (initWFObject?.InitWorkflow != null)
            {
                autoWf = unitOfWork.GetRepository<Workflow>().Find(x => x.ID == initWFObject.InitWorkflow.ID);
                if (autoWf == null)
                    throw new Exception("Для объекта необходимо указать бизнес процесс вручную");
            }
            else
            {
                autoWf = _serviceFacade.GetWorkflow(unitOfWork, typeStr);

                if (autoWf == null)
                    throw new Exception("Не найден бизнес процесс для объекта");
            }

            return autoWf;
        }

        public void InvokeStage(IUnitOfWork unitOfWork, IBaseObjectCrudService baseObjectService, IBPObject obj, StageAction action, ActionComment comment, int? userID = null)
        {
            if (obj == null)
                throw new Exception("Object is not IBPObject");

            if (action == null)
                throw new ArgumentException("Не удалось найти действие");

            var oldobj = (IBPObject)obj.ToObject(obj.GetType().GetBaseObjectType());

            var stagecontext = new InvokeStageContext
            {
                ActionComment = comment,
                Action = action,
                BPObject = obj,
                PerformUserID = userID
            };

            _stageInvoker.InvokeStage(unitOfWork, stagecontext);

            var wfObjectService = baseObjectService as IWFObjectService;

            if (wfObjectService != null)
            {
                CompleteActionExecuting(unitOfWork, wfObjectService, oldobj, stagecontext);
            }
            
            baseObjectService.Update(unitOfWork, (BaseObject)obj);
        }
        
        private void CompleteActionExecuting(IUnitOfWork unitOfWork, IWFObjectService baseObjectService, IBPObject oldObject, InvokeStageContext stageContext)
        {
            baseObjectService?.OnActionExecuting(new ActionExecuteArgs
            {
                UnitOfWork = unitOfWork,
                OldObject = (BaseObject)oldObject,
                NewObject = (BaseObject)stageContext.BPObject,
                CurrentStage = stageContext.Action.Step as Stage,
                Workflow = stageContext.BPObject.WorkflowContext.WorkflowImplementation.Workflow,
                EvaluatedAction = stageContext.Action,
                GetStageJumper = (output, obj) => _stageInvoker.FindNextStage(unitOfWork, output, (IBPObject)obj),
                Comment = stageContext.ActionComment?.Message,
            });
        }

        public void AutoInvokeStage()
        {
            using (var uow = _uowFactory.CreateSystem())
            {
                IQueryable<StagePerform> performs = uow.GetRepository<StagePerform>().All();
                performs = performs.Where(x => !x.Hidden && x.Stage.AutoProcess);
                performs = performs.Where(x => x.EndDate < AppContext.DateTime.Now);

                foreach (var stagePerform in performs)
                {
                    var service = _serviceFacade.GetService(stagePerform.WorkflowContext.ObjectType, uow);

                    var obj = (IBPObject)service.Get(uow, stagePerform.WorkflowContext.ObjectID.GetValueOrDefault(0));

                    ActionComment comment = new ActionComment() { Message = "Авто выполнение бизнес процесса" };

                    InvokeStage(uow, service, obj, stagePerform.Stage.Outputs.First(x => x.IsDefaultAction), comment);
                }
            }
        }
        
        public ICollection<StagePerform> GetNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, int actionID)
        {
            return _stageInvoker.GetNextStage(unitOfWork, baseObject, actionID);
        }

        public string Export(IUnitOfWork unitOfWork, int objectID)
        {
            var wf = unitOfWork.GetRepository<Workflow>().All().FirstOrDefault(x => x.ID == objectID && !x.Hidden);
            if (wf == null)
                throw new Exception("wf is null");
            return _serviceFacade.ExportWorkflow(wf);
        }

        public void Import(IUnitOfWork unitOfWork, string obj)
        {
            var wf = _serviceFacade.ImportWorkflow(unitOfWork, obj);
            Create(unitOfWork, wf);
            unitOfWork.SaveChanges();
        }


        public bool TestMacros(IUnitOfWork unitOfWork, IEnumerable<InitItem> items, Type type, Type parentType, out Exception exception)
        {
            exception = default(Exception);

            if (type != null && parentType != null)
            {
                var obj = Activator.CreateInstance(type) as BaseObject;
                var parentObj = Activator.CreateInstance(type) as BaseObject;

                try
                {
                    _serviceFacade.InitializeObject(AppContext.SecurityUser, parentObj, obj, items);
                    return true;
                }
                catch (Exception e)
                {
                    exception = e;

                    return false;
                }
            }

            return false;
        }

        public bool TestBranch(IUnitOfWork unitOfWork, IEnumerable<ConditionItem> items, Type type, Type parentType, out Exception exception)
        {
            exception = default(Exception);

            if (type != null && parentType != null)
            {
                var obj = Activator.CreateInstance(type) as BaseObject;

                try
                {
                    _serviceFacade.CheckBranch(unitOfWork, obj, items);
                    return true;
                }
                catch (Exception e)
                {
                    exception = e;

                    return false;
                }
            }

            return false;
        }

        public IQueryable<Workflow> GetWorkflowList(IUnitOfWork uow, Type objType)
        {
            string typeName = objType.GetTypeName();
            string fullName = objType.FullName;
            var rep = uow.GetRepository<Workflow>();

            var allTypes = rep.All().Select(x => x.ObjectType).ToList().Select(Type.GetType).Distinct().ToList();     

            allTypes = allTypes.Where(x=> x.IsAssignableFrom(objType) || objType.IsAssignableFrom(x)).ToList();

            var types = allTypes.Select(x=> x.GetTypeName()).ToList();

            var wfs =
                rep.All()
                    .Where(
                        x =>
                            !x.Hidden &&
                            (x.ObjectType == typeName || x.ObjectType == fullName || types.Contains(x.ObjectType)));

            return wfs;

        }

        public IQueryable<Workflow> GetWorkflowList(IUnitOfWork unitOfWork, Type type, BaseObject model)
        {
            return _serviceFacade.GetWorkflowList(AppContext.SecurityUser, type, model, GetAll(unitOfWork));
        }

        public void ExecuteNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, StageAction action, int? assignToUserID, ref double counter)
        {
            _stageInvoker.ExecuteNextStage(unitOfWork, baseObject, action, assignToUserID, ref counter);
        }

        public override Workflow CreateDefault(IUnitOfWork uow)
        {
            var wf = base.CreateDefault(uow);
            if (wf != null)
                wf.IsTemplate = true;

            return wf;
        }
    
        public override Workflow Create(IUnitOfWork unitOfWork, Workflow obj)
        {
            var haveDef = unitOfWork.GetRepository<Workflow>()
                .All()
                .Any(x => !x.Hidden && x.ObjectType == obj.ObjectType && x.IsDefault);
            if (!haveDef)
                obj.IsDefault = true;

            return base.Create(unitOfWork, obj);
        }

        protected override IObjectSaver<Workflow> GetForSave(IUnitOfWork uow, IObjectSaver<Workflow> objectSaver)
        {
            var temp = base.GetForSave(uow, objectSaver)
                .SaveOneObject(x => x.Curator)
                .SaveOneObject(x => x.CuratorsCategory)
                .SaveOneObject(x => x.BaseTaskCategory)
                .SaveOneObject(x => x.Creator);
            return temp;
        }

        private WorkflowImplementation GetLastVersion(Workflow wf)
        {
            var versions = wf.WorkflowImplementations.Where(x => !x.Hidden && !x.IsDraft).ToList();
            if (!versions.Any())
            {
                throw new Exception("Нет ни одной валидной версии, возможно помечены как черновик");
            }

            int lstVersion = versions.Max(x => x.Version);

            if (versions.Count(x => x.Version == lstVersion) > 1)
            {
                throw new Exception("Бизнес процессов с одной версией больше 1");
            }

            return versions.FirstOrDefault(x => x.Version == lstVersion);
        }

        public void ReStartWorkflow(IUnitOfWork uow, IBPObject obj, IBaseObjectCrudService objectCrudService)
        {
            var typeStr = obj.GetType().GetBaseObjectType();
            var systemUnitOfWork = _uowFactory.CreateSystem(uow);

            var wf = LoadWorkflow(obj, uow, typeStr);

            var lastVersion = GetLastVersion(wf);
            obj.WorkflowContext = new WorkflowContext
            {
                WorkflowImplementationID = lastVersion.ID,
                ObjectID = obj.ID,
                ObjectType = obj.GetType().GetBaseObjectType().FullName
            };

            systemUnitOfWork.GetRepository<WorkflowContext>().Create(obj.WorkflowContext);
            systemUnitOfWork.SaveChanges();

            StartWorkflow(systemUnitOfWork, objectCrudService, obj, obj.WorkflowContext);
        }

        public void OnEvent(IOnCreate<IBPObject> evnt)
        {
            var unitOfWork = evnt.UnitOfWork;
            var obj = evnt.Modified;
            var objectService = evnt.Source as IBaseObjectCrudService;

            if (obj != null && objectService != null)
            {
                var typeStr = obj.GetType().GetBaseObjectType();
                var systemUnitOfWork = _uowFactory.CreateSystem(unitOfWork);

                Workflow wf = null;

                try
                {
                    wf = LoadWorkflow(obj, unitOfWork, typeStr);
                }
                catch (Exception)
                {
                    // ignored
                }

                if(wf == null)
                    return;

                var lastVersion = GetLastVersion(wf);
                obj.WorkflowContext = new WorkflowContext
                {
                    WorkflowImplementation = lastVersion,
                    ObjectID = obj.ID,
                    ObjectType = obj.GetType().GetBaseObjectType().FullName
                };

                systemUnitOfWork.GetRepository<WorkflowContext>().Create(obj.WorkflowContext);
                systemUnitOfWork.SaveChanges();

                StartWorkflow(systemUnitOfWork, objectService, obj, obj.WorkflowContext);
            }
            else
            {
                //TODO : Сгенерировать искл или нет?
            }
        }

        public void OnEvent(IOnDelete<IBPObject> evnt)
        {
            var obj = evnt.Modified;

            if (obj.WorkflowContext != null)
            {
                obj.WorkflowContext.Hidden = true;
                foreach (var currentStage in obj.WorkflowContext.CurrentStages)
                {
                    currentStage.Hidden = true;
                }
            }
        }

        public void OnEvent(IOnUpdate<IBPObject> evnt)
        {
            //TODO : че будет при изменении вф?
            // OnBPObjectCreate(sender, baseObjectEventArgs);

        }
    }


    public class StageJumper
    {
        public StageJumper()
        {
            Steps = new List<Step>();
            StagePerforms = new List<StagePerform>();
        }

        public WorkflowHierarchyPosition WorkflowPosition { get; set; }

        public List<StagePerform> StagePerforms { get; private set; }

        public ICollection<Step> Steps { get; private set; }
    }
}