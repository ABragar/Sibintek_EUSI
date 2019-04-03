using System.Collections.Generic;
using System.Linq;
using Base.Ambient;
using Base.DAL;
using Base.Service;
using Base.Task.Entities;
using Base.Task.Services.Abstract;

namespace Base.Task.Services.Concrete
{
    public class InTaskCategoryService : BaseCategoryService<TaskCategory>, IInTaskCategoryService
    {
        private readonly ITaskCategoryService _taskCategoryService;
        
        public InTaskCategoryService(IBaseObjectServiceFacade facade, ITaskCategoryService taskCategoryService) : base(facade)
        {
            _taskCategoryService = taskCategoryService;
        }

        public override IQueryable<TaskCategory> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            var taskCategories = unitOfWork.GetRepository<BaseTask>().All()
                .Where(x => x.AssignedToID == AppContext.SecurityUser.ID)
                .Select(x => x.TaskCategory).Distinct().Select(x => new
                {
                    x.ID,
                    x.ParentID,
                    Parents = x.sys_all_parents,
                });

            var ids = new List<int>();

            foreach (var category in taskCategories)
            {
                if (category.ParentID != null)
                    ids.AddRange(category.Parents.Split(HCategory.Seperator).Select(HCategory.IdToInt));

                ids.Add(category.ID);
            }

            return _taskCategoryService.GetAll(unitOfWork, hidden).Where(x => ids.Contains(x.ID));

        }
    }
}
