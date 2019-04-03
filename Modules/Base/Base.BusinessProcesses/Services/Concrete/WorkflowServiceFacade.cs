using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Exceptions;
using Base.BusinessProcesses.Services.Abstract;
using Base.BusinessProcesses.Strategies;
using Base.DAL;
using Base.Helpers;
using Base.Security;
using Base.Security.ObjectAccess;
using Base.Security.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities.Steps;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Base.Macros;
using Base.Macros.Entities;
using Base.Service.Crud;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class WorkflowServiceFacade : IWorkflowServiceFacade
    {
        private readonly ISecurityService _accessItemService;
        private readonly IMacrosService _objectInitializer;
        private readonly IWorkflowServiceResolver _workflowServiceResolver;
        private readonly IWorkflowStrategyService _strategyService;
        
        public WorkflowServiceFacade(
            ISecurityService accessItemService,
            IMacrosService objectInitializer,
            IWorkflowStrategyService strategyService,
            IWorkflowServiceResolver workflowServiceResolver)
        {
            _accessItemService = accessItemService;
            _objectInitializer = objectInitializer;
            _strategyService = strategyService;
            _workflowServiceResolver = workflowServiceResolver;
        }

        public ObjectAccessItem CreateAccessItem(IUnitOfWork uow, BaseObject obj)
        {
            return _accessItemService.CreateAndSaveAccessItem(uow, obj);
        }

        public void InitializeObject(ISecurityUser securityUser, BaseObject src, BaseObject dest, IEnumerable<InitItem> inits)
        {
            _objectInitializer.InitializeObject(securityUser, src, dest, inits);
        }
        
        public Workflow CloneWorkflow(Workflow wf)
        {
            throw new NotImplementedException();
        }

        public string ExportWorkflow(Workflow wf, bool completegraph = false)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                Binder = new DynamicProxySerializationBinder()
            };

            var result = JsonConvert.SerializeObject(wf, null, settings);
            return result;
        }

        public Workflow ImportWorkflow(IUnitOfWork uow, string obj)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                Binder = new DynamicProxySerializationBinder()
            };

            var result = JsonConvert.DeserializeObject<Workflow>(obj);
            return result;
        }



        class DynamicProxySerializationBinder : DefaultSerializationBinder
        {
            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                var type = serializedType.Namespace == "System.Data.Entity.DynamicProxies"
                    ? serializedType.BaseType
                    : serializedType;

                base.BindToName(type, out assemblyName, out typeName);
            }
        }

        public void CreateChildAccessItem(IUnitOfWork unitOfWork, Workflow wf)
        {
            //var wfParentSteps = wf.Steps.OfType<WorkflowOwnerStep>();
            //foreach (var parentStep in wfParentSteps)
            //{
            //    CreateAccessItem(unitOfWork, parentStep.ChildWorkflow);
            //    CreateChildAccessItem(unitOfWork, parentStep.ChildWorkflow);
            //}
        }

        public Workflow GetWorkflow(IUnitOfWork uow, Type objType)
        {
            var str = _strategyService.GetWorkflowSelectStrategy();
            return str.GetWorkflow(uow, objType);
        }

        public IQueryable<Workflow> GetWorkflowList(ISecurityUser securityUser, Type type, BaseObject model, IQueryable<Workflow> all)
        {
            var listStrategy = _strategyService.GetWorkflowListStrategy();

            return listStrategy.GetWorkflows(securityUser, model, all).Where(x => x.ObjectType == type.GetTypeName());
        }

        public void ModifyObject(ISecurityUser securityUser, BaseObject src, IEnumerable<InitItem> inits)
        {
            _objectInitializer.InitializeObject(securityUser, src, src, inits);
        }

        public bool CheckBranch(IUnitOfWork uow, BaseObject obj, IEnumerable<ConditionItem> inits)
        {
            return _objectInitializer.CheckBranch(uow, obj, inits);
        }

        public IBaseObjectCrudService GetService(string objectTypeStr, IUnitOfWork unitOfWork = null)
        {
            return _workflowServiceResolver.GetObjectService(objectTypeStr, unitOfWork);
        }

        public StageAction GetEntryPoint(WorkflowContext ctx)
        {
            StageAction entryAction = null;

            var entryPointStage = (EntryPointStep)ctx.WorkflowImplementation.Steps.FirstOrDefault(x => x.StepType == FlowStepType.EntryPointStep);

            if (entryPointStage != null)
                entryAction = entryPointStage.Outputs.FirstOrDefault();

            if (entryAction != null)
                return entryAction;

            throw ExceptionHelper.ActionNotFoundException("Action not found");

        }


    }
}