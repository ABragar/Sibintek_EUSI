using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.BusinessProcesses.Strategies;
using Base.DAL;
using Base.Security;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class StageUserService : BaseObjectService<StageUser>, IStageUserService
    {
        private readonly IWorkflowStrategyService _strategyService;

        public StageUserService(IWorkflowStrategyService strategyService, IBaseObjectServiceFacade facade) : base(facade)
        {
            _strategyService = strategyService;
        }

        public IQueryable<User> GetStakeholders(IUnitOfWork unitOfWork, Stage stage, IBPObject obj)
        {
            var users = GetPermittedUsers(unitOfWork, stage, x => x.AssignedToCategory, obj);
            return users;
        }

        private IQueryable<User> GetPermittedUsers<TStep>(IUnitOfWork unitOfWork, TStep step, Func<TStep, ICollection<StageUserCategory>> categorySelector, IBPObject obj) where TStep : Stage
        {
                        
            var users = unitOfWork.GetRepository<Stage>().All()
                .Where(x => x.ID == step.ID)
                .SelectMany(x => x.AssignedToUsers)
                .Select(x => x.Object);

            var assignedToCategories = categorySelector(step);

            if (assignedToCategories != null && assignedToCategories.Any())
            {
                foreach (var assignedToCategory in assignedToCategories)
                {
                    int catId = assignedToCategory.ObjectID.GetValueOrDefault();
                    string strId = HCategory.IdToString(catId);

                    var queryable = unitOfWork.GetRepository<User>().All()
                        .Where(a => a.IsActive && !a.Hidden)
                        .Where(x => (x.Category_.sys_all_parents != null && x.Category_.sys_all_parents.Contains(strId)) || x.Category_.ID == catId);

                    users = users.Union(queryable);

                }

                //users = (from assignedToCategory in assignedToCategories
                //                let strId = HCategory.IdToString(assignedToCategory.ObjectID.GetValueOrDefault())
                //                let catId = assignedToCategory.ObjectID.GetValueOrDefault()
                //                select unitOfWork.GetRepository<User>().All()
                //                .Where(a => a.IsActive && !a.Hidden)
                //                .Where(x => (x.Category_.sys_all_parents != null && x.Category_.sys_all_parents.Contains(strId)) || x.Category_.ID == catId))
                //                .Aggregate(users, (current, range) => current.Union(FilterByStrategy(unitOfWork, range, step, obj)));
                
            }

            var res = FilterByStrategy(unitOfWork, users, step, obj);

            return res;
        }

        private IQueryable<User> FilterByStrategy(IUnitOfWork unitOfWork, IQueryable<User> users, Step step, IBPObject obj)
        {
            if (users != null)
            {
                var stage = step as Stage;
                if (stage != null && !string.IsNullOrEmpty(stage.StakeholdersSelectionStrategy))
                {
                    var strategy =
                        _strategyService.GetStakeholdersSelectionStrategy(stage.StakeholdersSelectionStrategy);

                    if (strategy != null)
                        return strategy.FilterStackholders(unitOfWork, users, obj);
                }
            }
            return users;
        }
    }
}
