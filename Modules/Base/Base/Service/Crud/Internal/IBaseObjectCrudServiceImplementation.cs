using System.Collections.Generic;
using System.Linq;
using Base.DAL;

namespace Base.Service.Crud.Internal
{
    internal interface IBaseObjectCrudServiceImplementation
    {
        IQueryable<BaseObject> GetAll(IBaseObjectCrudService service, IUnitOfWork unitOfWork, bool? hidden);

        BaseObject Get(IBaseObjectCrudService service, IUnitOfWork unitOfWork, int id);

        BaseObject Create(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj);
        IReadOnlyCollection<BaseObject> CreateCollection(IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> obj);

        BaseObject Update(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj);
        IReadOnlyCollection<BaseObject> UpdateCollection(IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> obj);

        void Delete(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj);
        void DeleteCollection(IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> obj);

        void ChangeSortOrder(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj, int posId);

        BaseObject CreateDefault(IBaseObjectCrudService service, IUnitOfWork unitOfWork);

        IQueryable<BaseObject> GetById(IBaseObjectCrudService service, IUnitOfWork unitOfWork, bool? hidden, int id);
    }
}