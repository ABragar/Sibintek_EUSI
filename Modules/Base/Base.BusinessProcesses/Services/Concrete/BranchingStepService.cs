using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class BranchingStepService : BaseObjectService<BranchingStep>, IBranchingStepService
    {
        public BranchingStepService(IBaseObjectServiceFacade facade) : base(facade) { }

        public override BranchingStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new BranchingStep { Title = "Без названия", Outputs = new List<Branch>() };
        }
    }
}
