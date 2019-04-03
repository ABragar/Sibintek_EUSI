using Base.DAL;
using System.Collections.Generic;
using Base.Service.Crud;
using System.Linq;
namespace Base.Service
{


    public interface IBaseObjectService<T> : IBaseObjectCrudService, IQueryService<T>
        where T : IBaseObject
    {
        T Get(IUnitOfWork unitOfWork, int id);
        T Create(IUnitOfWork unitOfWork, T obj);
        IReadOnlyCollection<T> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection);
        T Update(IUnitOfWork unitOfWork, T obj);
        IReadOnlyCollection<T> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection);
        void Delete(IUnitOfWork unitOfWork, T obj);
        void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection);
        void ChangeSortOrder(IUnitOfWork unitOfWork, T obj, int posId);
        T CreateDefault(IUnitOfWork unitOfWork);
        IQueryable<T> GetById(IUnitOfWork unitOfWork, bool? hidden, int id);

    }
}
