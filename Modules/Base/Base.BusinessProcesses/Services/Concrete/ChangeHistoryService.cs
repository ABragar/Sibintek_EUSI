using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Links.Service.Abstract;
using Base.Service;
using Base.Service.Crud;
using AppContext = Base.Ambient.AppContext;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class ChangeHistoryService : BaseObjectService<ChangeHistory>, IChangeHistoryService
    {
        private readonly IWorkflowServiceFacade _workflowServiceFacade;
        private readonly ILinkItemService _linkItemService;

        public ChangeHistoryService(IBaseObjectServiceFacade facade, IWorkflowServiceFacade workflowServiceFacade, ILinkItemService linkItemService)
            : base(facade)
        {
            _workflowServiceFacade = workflowServiceFacade;
            _linkItemService = linkItemService;
        }

        public IQueryable<ChangeHistory> GetChangeHistory(IUnitOfWork unitOfWork, IBPObject obj, int? implID = null, int? count = null)
        {
            string objectType = obj.GetType().GetBaseObjectType().GetTypeName();
            int objectID = obj.ID;
            var context = obj.WorkflowContext ?? unitOfWork.GetRepository<WorkflowContext>().All().FirstOrDefault(x => x.ID == obj.WorkflowContextID);

            if (context == null)
                throw new Exception("Не удалось найти конекст исполнения");

            int impl = implID ?? context.WorkflowImplementation.ID;

            IQueryable<ChangeHistory> q = unitOfWork.GetRepository<ChangeHistory>().All()
                .Where(x => x.WorkflowContextID == context.ID && x.WorkflowVersionID == impl && x.ObjectID == objectID && x.ObjectType == objectType)
                .Where(x => x.Step is Stage || x.Step is WorkflowOwnerStep)
                .OrderByDescending(x => x.SortOrder);

            return count.HasValue ? q.Take(count.Value) : q;
        }

        public void WriteStepsBetweenStages(IUnitOfWork unitOfWork,  List<Step> steps, ref double sortOrder, InvokeStageContext stageContext)
        {
            var dt = AppContext.DateTime.Now;
            var histRep = unitOfWork.GetRepository<ChangeHistory>();
            foreach (var step in steps.Where(x => x.StepType != FlowStepType.Stage))
            {
                ChangeHistory ch = new ChangeHistory
                {
                    WorkflowVersionID = step.WorkflowImplementationID,
                    WorkflowContextID = stageContext.BPObject.WorkflowContext.ID,
                    ObjectID = stageContext.BPObject.ID,
                    ObjectType = stageContext.BPObject.GetType().GetBaseObjectType().GetTypeName(),
                    UserID = AppContext.SecurityUser.ID,
                    Date = AppContext.DateTime.Now,
                    Step = step,
                    StepID = step.ID,
                    SortOrder = ++sortOrder,
                };
                if (step is CreateObjectStep)
                {
                    CreateObjectStep cos = step as CreateObjectStep;
                    ch.CreatedObject = StartWorkflows(unitOfWork, (BaseObject)stageContext.BPObject, cos);
                }
                if (step is EntryPointStep)
                {
                    var action = (StageAction)step.BaseOutputs.FirstOrDefault();
                    ch.AgreementItem = new AgreementItem
                    {
                        UserID = AppContext.SecurityUser.ID,
                        Comment = "Запуск дочернего бизнес процесса",
                        Date = dt,
                        Action = action,
                    };
                }

                histRep.Create(ch);
                dt += new TimeSpan(0, 0, 1);
            }
        }

        private CreatedObject StartWorkflows(IUnitOfWork unitOfWork, BaseObject src, CreateObjectStep createObjectStep)
        {
            var service = _workflowServiceFacade.GetService(createObjectStep.ObjectType, unitOfWork);
            var dest = service.CreateDefault(unitOfWork);
            _workflowServiceFacade.InitializeObject(AppContext.SecurityUser, src, dest, createObjectStep.InitItems);

            _linkItemService.InitLinkObject(unitOfWork, src, dest);
            
            service.Create(unitOfWork, dest);

            _linkItemService.SaveLink(unitOfWork, dest, src);

            return new CreatedObject { ObjectID = dest.ID, Type = createObjectStep.ObjectType, ObjectStepID = createObjectStep.ID };
        }

        public void WriteStageToHistory(IUnitOfWork unitOfWork, StagePerform perform, InvokeStageContext stageContext ,ref double sortOrder)
        {
            ChangeHistory ch = new ChangeHistory
            {
                WorkflowVersionID = stageContext.Action.Step.WorkflowImplementation.ID,
                WorkflowContextID = stageContext.BPObject.WorkflowContext.ID,
                ObjectID = stageContext.BPObject.ID,
                ObjectType = stageContext.BPObject.GetType().GetBaseObjectType().GetTypeName(),
                UserID = AppContext.SecurityUser.ID,
                Step = perform.Stage,
                SortOrder = ++sortOrder,
                Date = perform.BeginDate,
                AgreementItem = new AgreementItem
                {
                    ActionID = stageContext.Action.ID,
                    Comment = stageContext.ActionComment?.Message,
                    File = stageContext.ActionComment?.File,
                    Date = AppContext.DateTime.Now,
                    UserID = AppContext.SecurityUser.ID,
                    FromUserID = AppContext.SecurityUser.ID,
                },
            };


            var rep = unitOfWork.GetRepository<ChangeHistory>();
            rep.Create(ch);
        }
    }

}
