using Base.DAL;
using System.Linq;
using Base.Service.Crud;

namespace Base.Service
{
    public interface IBaseCategoryService<T> : IBaseObjectService<T>, ICategoryCrudService
        where T : HCategory
    {
        IQueryable<T> GetRoots(IUnitOfWork unitOfWork, bool? hidden = false);
   
        IQueryable<T> GetAllChildren(IUnitOfWork unitOfWork, int parentID, bool? hidden = false);
   
        IQueryable<T> GetChildren(IUnitOfWork unitOfWork, int parentID, bool? hidden = false);
   
        void ChangePosition(IUnitOfWork unitOfWork, T obj, int? posChangeID, string typePosChange);
    }
}
