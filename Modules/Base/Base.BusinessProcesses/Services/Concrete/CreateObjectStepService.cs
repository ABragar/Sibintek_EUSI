using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class CreateObjectStepService :BaseObjectService<CreateObjectStep>,  ICreateObjectStepService
    {
        public CreateObjectStepService(IBaseObjectServiceFacade facade) : base(facade) { }

        public override CreateObjectStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new CreateObjectStep
            {
                Title = "Создание объекта", 
                Description = "В этом шаге создается объект",
                Outputs = new List<Output>() { new Output() },
                InitItems = new List<CreateObjectStepMemberInitItem>(),
            };
        }
    }
}