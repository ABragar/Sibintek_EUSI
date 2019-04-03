using Base.BusinessProcesses.Entities;
using Base.Security;
using System.Linq;

namespace Base.BusinessProcesses.Strategies
{
    public class WorkflowListStrategy : IWorkflowListStrategy
    {
        public IQueryable<Workflow> GetWorkflows(ISecurityUser user, BaseObject obj, IQueryable<Workflow> workflows)
        {
            var wfs = workflows.Where(x => x.IsTemplate && !x.Hidden && (x.ObjectType == obj.GetType().FullName || x.ObjectType == obj.GetType().GetTypeName()));

            return wfs;
        }
    }
}