using System.Collections.Generic;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class ValidationStepService : BaseObjectService<ValidationStep> , IValidationStepService
    {
        public ValidationStepService(IBaseObjectServiceFacade facade)
            :base(facade)
        {
            
        }

        public override ValidationStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new ValidationStep
            {
                Title = "Валидация объекта",
                Outputs = new List<Output>() { new Output()},
                Description = "Проверка объекта на валидность",
                ValidatonRules = new List<StepValidationItem>()
            };
        }
    }
}
