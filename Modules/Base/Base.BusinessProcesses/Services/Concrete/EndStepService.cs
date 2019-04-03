using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class EndStepService : BaseObjectService<EndStep>, IEndStepService
    {
        public EndStepService(IBaseObjectServiceFacade facade) : base(facade) { }


        public override EndStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new EndStep
            {
                Title = "Точка выхода",
                BaseOutputs =  new List<Output>(),
            };
        }
    }
}
