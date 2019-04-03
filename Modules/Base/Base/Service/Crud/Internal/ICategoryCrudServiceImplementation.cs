using System.Linq;
using Base.DAL;

namespace Base.Service.Crud.Internal
{
    public interface ICategoryCrudServiceImplementation
    {
        IQueryable<HCategory> GetRoots(ICategoryCrudService service, IUnitOfWork unitOfWork, bool? hidden);   
        IQueryable<HCategory> GetAllChildren(ICategoryCrudService service, IUnitOfWork unitOfWork, int parentID, bool? hidden);
        IQueryable<HCategory> GetChildren(ICategoryCrudService service, IUnitOfWork unitOfWork, int parentID, bool? hidden);
        void ChangePosition(ICategoryCrudService service, IUnitOfWork unitOfWork, HCategory obj, int? posChangeID, string typePosChange);
    }
}