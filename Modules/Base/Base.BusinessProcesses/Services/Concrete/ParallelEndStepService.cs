﻿using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Service;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class ParallelEndStepService : BaseObjectService<ParallelEndStep>, IParallelEndStepService
    {
        public ParallelEndStepService(IBaseObjectServiceFacade facade)
            : base(facade)
        {
            
        }

        public override ParallelEndStep CreateDefault(IUnitOfWork unitOfWork)
        {
            return new ParallelEndStep
            {
                Title = "Завершение потока",
                WaitAllThreads = true,
                Outputs = new List<Output> { new Output() },
            };
        }
    }
}