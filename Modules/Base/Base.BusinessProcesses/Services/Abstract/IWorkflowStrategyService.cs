using Base.BusinessProcesses.Strategies;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface IWorkflowStrategyService : IService
    {
        IWorkflowSelectStrategy GetWorkflowSelectStrategy();
        IWorkflowListStrategy GetWorkflowListStrategy();
        IEnumerable<IStakeholdersSelectionStrategy> GetStakeholdersSelectionStrategies(Type type);
        IStakeholdersSelectionStrategy GetStakeholdersSelectionStrategy(string type);
    }
}