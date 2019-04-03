using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class ChangeObjectStepService : BaseObjectService<ChangeObjectStep>, IChangeObjectStepService
    {
        public ChangeObjectStepService(IBaseObjectServiceFacade facade)
            : base(facade)
        {

        }

        public override ChangeObjectStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new ChangeObjectStep
            {
                Title = "Изменение объекта",
                Outputs = new List<Output>() { new Output() },
                Description = "Изменяет значание свойств объекта",
                InitItems = new List<ChangeObjectStepInitItems>(),
            };
        }
    }
}
