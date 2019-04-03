using System.Linq;
using Base.DAL;

namespace Base.Service.Crud.Internal
{
    internal interface ICategorizedItemCrudServiceImplementation
    {
        IQueryable<BaseObject> GetCategorizedItems(ICategorizedItemCrudService service,
            IUnitOfWork unitOfWork,
            int categoryID,
            bool? hidden = false);


        IQueryable<BaseObject> GetAllCategorizedItems(ICategorizedItemCrudService service,
            IUnitOfWork unitOfWork,
            int categoryID,
            bool? hidden = false);

        void ChangeCategory(ICategorizedItemCrudService service, IUnitOfWork unitOfWork, int id, int newCategoryID);
    }
}