using System;
using Base.BusinessProcesses.Entities;
using Base.Security;
using System.Linq;

namespace Base.BusinessProcesses.Strategies
{
    public interface IWorkflowListStrategy
    {
        IQueryable<Workflow> GetWorkflows(ISecurityUser user, BaseObject obj, IQueryable<Workflow> workflows);
    }
}