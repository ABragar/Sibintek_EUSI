using Base.BusinessProcesses.Strategies;
using Base.DAL;
using System;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities;
using Base.Service.Crud;

namespace Base.BusinessProcesses.Services.Concrete
{
    public interface IWorkflowServiceResolver
    {
        IBaseObjectCrudService GetObjectService(string objectTypeStr, IUnitOfWork unitOfWork = null);

        ICollection<IStakeholdersSelectionStrategy> GetStakeholdersSelectionStrategies();
        IStakeholdersSelectionStrategy GetStakeholdersSelectionStrategy(Type type);
    }
}