using System.Linq;
using Base.DAL;

namespace Base.Service.Crud.Internal
{
    internal class CategorizedItemCrudServiceImplementation<T> : ICategorizedItemCrudServiceImplementation
        where T : BaseObject, ICategorizedItem
    {
        public IQueryable<BaseObject> GetCategorizedItems(ICategorizedItemCrudService service, IUnitOfWork unitOfWork, int categoryID, bool? hidden)
        {
            return ((IBaseCategorizedItemService<T>)service).GetCategorizedItems(unitOfWork, categoryID, hidden);
        }

        public IQueryable<BaseObject> GetAllCategorizedItems(ICategorizedItemCrudService service,
            IUnitOfWork unitOfWork,
            int categoryID,
            bool? hidden)
        {
            return ((IBaseCategorizedItemService<T>)service).GetAllCategorizedItems(unitOfWork, categoryID, hidden);
        }

        public void ChangeCategory(ICategorizedItemCrudService service, IUnitOfWork unitOfWork, int id, int newCategoryID)
        {
            ((IBaseCategorizedItemService<T>)service).ChangeCategory(unitOfWork, id, newCategoryID);
        }
    }
}