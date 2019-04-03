using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;
using Base.Task.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class StageService : BaseObjectService<Stage>, IStageService
    {
        public StageService(IBaseObjectServiceFacade facade) : base(facade) { }

        public override Stage CreateDefault(IUnitOfWork unitOfWork)
        {
            var stage = new Stage
            {
                AssignedToCategory = new List<StageUserCategory>(),
                AssignedToUsers = new List<StageUser>(),
                Outputs = new List<StageAction>()
            };
            
            return stage;
        }
    }
}
