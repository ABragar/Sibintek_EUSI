using System.Collections.Generic;
using System.Linq;
using Base.DAL;

namespace Base.Service.Crud.Internal
{
    internal class BaseObjectCrudServiceImplementation<T> : IBaseObjectCrudServiceImplementation
        where T : BaseObject
    {
        public IQueryable<BaseObject> GetAll(IBaseObjectCrudService service, IUnitOfWork unitOfWork, bool? hidden)
        {
            return ((IBaseObjectService<T>)service).GetAll(unitOfWork, hidden);
        }

        public BaseObject Get(IBaseObjectCrudService service, IUnitOfWork unitOfWork, int id)
        {
            return ((IBaseObjectService<T>)service).Get(unitOfWork, id);
        }

        public BaseObject Create(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj)
        {
            return ((IBaseObjectService<T>)service).Create(unitOfWork, (T)obj);
        }

        public IReadOnlyCollection<BaseObject> CreateCollection(IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> obj)
        {
            return ((IBaseObjectService<T>)service).CreateCollection(unitOfWork, (IReadOnlyCollection<T>)obj);
        }

        public BaseObject Update(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj)
        {
            return ((IBaseObjectService<T>)service).Update(unitOfWork, (T)obj);
        }

        public IReadOnlyCollection<BaseObject> UpdateCollection(IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> obj)
        {
            return ((IBaseObjectService<T>)service).UpdateCollection(unitOfWork, (IReadOnlyCollection<T>)obj);
        }

        public void Delete(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj)
        {
            ((IBaseObjectService<T>)service).Delete(unitOfWork, (T)obj);
        }

        public void DeleteCollection(IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> obj)
        {
            ((IBaseObjectService<T>)service).DeleteCollection(unitOfWork, (IReadOnlyCollection<T>)obj);
        }

        public void ChangeSortOrder(IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj, int posId)
        {
            ((IBaseObjectService<T>)service).ChangeSortOrder(unitOfWork, (T)obj, posId);
        }

        public BaseObject CreateDefault(IBaseObjectCrudService service, IUnitOfWork unitOfWork)
        {
            return ((IBaseObjectService<T>)service).CreateDefault(unitOfWork);
        }

        public IQueryable<BaseObject> GetById(IBaseObjectCrudService service, IUnitOfWork unitOfWork, bool? hidden, int id)
        {
            return ((IBaseObjectService<T>)service).GetById(unitOfWork, hidden, id);
        }
    }
}