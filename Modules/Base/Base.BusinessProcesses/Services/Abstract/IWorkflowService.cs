using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.Events;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Macros.Entities;
using Base.Service.Crud;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface IWorkflowService : IBaseCategorizedItemService<Workflow>,
        IEventBusHandler<IOnCreate<IBPObject>>,
        IEventBusHandler<IOnDelete<IBPObject>>,
        IEventBusHandler<IOnUpdate<IBPObject>>,
        IExportImportObject
    {
        void InvokeStage(IUnitOfWork unitOfWork, IBaseObjectCrudService baseObjectService, IBPObject obj, StageAction action, ActionComment comment, int? userID = null);
        void AutoInvokeStage();
        ICollection<StagePerform> GetNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, int actionID);
        bool TestMacros(IUnitOfWork unitOfWork, IEnumerable<InitItem> items, Type type, Type parentType, out Exception exception);
        bool TestBranch(IUnitOfWork unitOfWork, IEnumerable<ConditionItem> items, Type type, Type parentType, out Exception exception);
        IQueryable<Workflow> GetWorkflowList(IUnitOfWork unitOfWork, Type type);
        IQueryable<Workflow> GetWorkflowList(IUnitOfWork unitOfWork, Type type, BaseObject model);
        void ExecuteNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, StageAction action, int? assignToUserID, ref double counter);
        void ReStartWorkflow(IUnitOfWork uow, IBPObject obj, IBaseObjectCrudService objectCrudService);
    }
}
