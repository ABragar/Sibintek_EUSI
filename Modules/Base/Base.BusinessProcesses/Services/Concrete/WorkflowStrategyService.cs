using Base.BusinessProcesses.Services.Abstract;
using Base.BusinessProcesses.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class WorkflowStrategyService : IWorkflowStrategyService
    {
        private readonly IWorkflowServiceResolver _resolver;



        public WorkflowStrategyService(IWorkflowServiceResolver resolver)
        {
            _resolver = resolver;
        }

        public IWorkflowSelectStrategy GetWorkflowSelectStrategy()
        {
            return new WorkflowSelectStrategy();
        }

        public IWorkflowListStrategy GetWorkflowListStrategy()
        {
            return new WorkflowListStrategy();
        }

        public IEnumerable<IStakeholdersSelectionStrategy> GetStakeholdersSelectionStrategies(Type type)
        {
            var strategies = _resolver.GetStakeholdersSelectionStrategies();
            return strategies.Where(x => x.GetType().GetInterfaces().First(z => z.IsGenericType).GetGenericArguments().First().IsAssignableFrom(type));
        }

        public IStakeholdersSelectionStrategy GetStakeholdersSelectionStrategy(string type)
        {
            var strategies = _resolver.GetStakeholdersSelectionStrategies();
            var strategy = strategies.First(x => x.GetType().FullName == type);

            return strategy;
        }
    }

}