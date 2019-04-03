using System.Linq;
using Base.DAL;

namespace Base.Service.Crud.Internal
{
    public class CategoryCrudServiceImplementation<T> : ICategoryCrudServiceImplementation
        where T: HCategory
    {
        public IQueryable<HCategory> GetRoots(ICategoryCrudService service, IUnitOfWork unitOfWork, bool? hidden)
        {
            return ((IBaseCategoryService<T>)service).GetRoots(unitOfWork, hidden);
        }


        public IQueryable<HCategory> GetAllChildren(ICategoryCrudService service, IUnitOfWork unitOfWork, int parentID, bool? hidden)
        {
            return ((IBaseCategoryService<T>)service).GetAllChildren(unitOfWork, parentID, hidden);
        }


        public IQueryable<HCategory> GetChildren(ICategoryCrudService service, IUnitOfWork unitOfWork, int parentID, bool? hidden)
        {
            return ((IBaseCategoryService<T>)service).GetChildren(unitOfWork, parentID, hidden);

        }

        public void ChangePosition(ICategoryCrudService service,
            IUnitOfWork unitOfWork,
            HCategory obj,
            int? posChangeID,
            string typePosChange)
        {
            ((IBaseCategoryService<T>)service).ChangePosition(unitOfWork, (T)obj, posChangeID, typePosChange);
        }
    }
}