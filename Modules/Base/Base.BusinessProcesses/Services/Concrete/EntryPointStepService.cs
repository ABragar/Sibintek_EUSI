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
    public class EntryPointStepService : BaseObjectService<EntryPointStep>, IEntryPointStepService
    {
        public EntryPointStepService(IBaseObjectServiceFacade facade)
            :base(facade)
        {
            
        }

        public override EntryPointStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new EntryPointStep
            {
                Title = "Точка входа",
                Outputs = new List<StageAction>()
            };
        }
    }
}
