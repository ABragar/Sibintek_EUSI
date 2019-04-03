using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class WorkflowOwnerStepService : BaseObjectService<WorkflowOwnerStep>, IWorkflowOwnerStepService
    {
        public WorkflowOwnerStepService(IBaseObjectServiceFacade facade) : base(facade) { }

        public override WorkflowOwnerStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new WorkflowOwnerStep
            {
                Title = "Контейнер бизнес процесса",
                Description = "Данный этап является контейнером бизнес процесса",
                Outputs = new List<Output>() { new Output()},
            };
        }
    }
}
