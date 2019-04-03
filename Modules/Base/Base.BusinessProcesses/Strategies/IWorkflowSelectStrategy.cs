using System;
using Base.BusinessProcesses.Entities;
using Base.DAL;

namespace Base.BusinessProcesses.Strategies
{
    public interface IWorkflowSelectStrategy
    {
        Workflow GetWorkflow(IUnitOfWork uow, Type objType);
    }
}