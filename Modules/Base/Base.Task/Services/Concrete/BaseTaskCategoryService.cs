using Base.DAL;
using Base.Entities.Complex;
using Base.Service;
using Base.Task.Entities;
using System.Linq;
using Base.Settings;


namespace Base.Task.Services
{
    public class BaseTaskCategoryService : BaseObjectService<BaseTaskCategory>, IBaseTaskCategoryService
    {
        public BaseTaskCategoryService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}
