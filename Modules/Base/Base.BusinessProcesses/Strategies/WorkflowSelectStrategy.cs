using System;
using System.Linq;
using Base.BusinessProcesses.Entities;
using Base.DAL;

namespace Base.BusinessProcesses.Strategies
{
    public class WorkflowSelectStrategy : IWorkflowSelectStrategy
    {
        public Workflow GetWorkflow(IUnitOfWork uow, Type objType)
        {
            string typeName = objType.GetTypeName();
            string fullName = objType.FullName;
            var rep = uow.GetRepository<Workflow>();
            var wfs = rep.All()
                .Where(x => (x.ObjectType == typeName || x.ObjectType == fullName) && x.IsTemplate && x.IsDefault && !x.Hidden);

            if (wfs.Any())
            {
                if (wfs.Count() == 1)
                {
                    return wfs.First();
                }
                else
                {
                    throw new Exception($"Есть несколько бизнес процессов для {objType.Name}");
                }
            }
            if (objType.BaseType != null)
            {
                return GetWorkflow(uow, objType.BaseType);
            }

            return null;
        }
    }
}