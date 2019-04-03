using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities.Steps;
using Base.Service;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class ConditionalStepService : BaseObjectService<ConditionalStep> , IConditionalStepService
    {
        public ConditionalStepService(IBaseObjectServiceFacade facade)
            :base(facade)
        {
                
        }

        public override ConditionalStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new ConditionalStep
            {
                Outputs = new List<ConditionalBranch>(),
                Title = "Условный переход"
            };
        }
    }
}
