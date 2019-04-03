using Base.DAL;
using System.Linq;
using Base.Service.Crud;

namespace Base.Service
{
    public interface IBaseCategorizedItemService<T> : IBaseObjectService<T> , ICategorizedItemCrudService
        where T : IBaseObject, ICategorizedItem 
    {
        IQueryable<T> GetCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = false);
        IQueryable<T> GetAllCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = false);
        void ChangeCategory(IUnitOfWork unitOfWork, int id, int newCategoryID);
    }
}
