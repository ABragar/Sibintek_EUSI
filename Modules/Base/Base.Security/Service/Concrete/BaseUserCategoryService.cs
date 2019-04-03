using System;
using System.Linq;
using Base.DAL;
using Base.Service;

namespace Base.Security.Service
{
    public class BaseUserCategoryService<T> : BaseCategoryService<T>, IBaseUserCategoryService<T> where T : UserCategory
    {
        public BaseUserCategoryService(IBaseObjectServiceFacade facade) : base(facade) { }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.Presets, x => x.SaveOneObject(o => o.Object))
                .SaveManyToMany(x => x.Roles);
        }
    }
}