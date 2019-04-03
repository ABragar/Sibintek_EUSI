using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.Security;
using Base.Security.ObjectAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Macros;
using Base.Macros.Entities;
using Base.Service.Crud;

namespace Base.BusinessProcesses.Services.Concrete
{
    public interface  IWorkflowServiceFacade
    {
        ObjectAccessItem CreateAccessItem(IUnitOfWork uow, BaseObject obj);                
        void InitializeObject(ISecurityUser securityUser, BaseObject src, BaseObject dest, IEnumerable<InitItem> inits);
        void ModifyObject(ISecurityUser securityUser, BaseObject src, IEnumerable<InitItem> inits);
        Workflow CloneWorkflow(Workflow initWorkflow);
        void CreateChildAccessItem(IUnitOfWork unitOfWork, Workflow wf);
        Workflow GetWorkflow(IUnitOfWork uow, Type objType);
        IQueryable<Workflow> GetWorkflowList(ISecurityUser securityUser, Type type, BaseObject model, IQueryable<Workflow> all);
        IBaseObjectCrudService GetService(string objectTypeStr, IUnitOfWork unitOfWork = null);
        StageAction GetEntryPoint(WorkflowContext wf);
        string ExportWorkflow(Workflow wf, bool completegraph = false);
        Workflow ImportWorkflow(IUnitOfWork uow, string obj);
        bool CheckBranch(IUnitOfWork uow, BaseObject obj, IEnumerable<ConditionItem> inits);
    }
}