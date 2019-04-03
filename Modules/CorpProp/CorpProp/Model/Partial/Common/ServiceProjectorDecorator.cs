using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service;
using CorpProp.Common;

namespace CorpProp.Model
{
    /// <summary>
    /// Декоратор сервисов для проекции методов сервиса <typeparamref name="TBaseService"/> обобщенных от типа <typeparamref name="TBaseServiceObject"/> в методы сервиса обобщенного типа <typeparamref name="TProjectionServiceObject"/>
    /// </summary>
    /// <typeparam name="TBaseService"></typeparam>
    /// <typeparam name="TBaseServiceObject"></typeparam>
    /// <typeparam name="TProjectionServiceObject"></typeparam>
    public class ServiceProjectorDecorator<TBaseService, TBaseServiceObject, TProjectionServiceObject> : IBaseObjectService<TProjectionServiceObject>
        where TBaseService : IBaseObjectService<TBaseServiceObject>
        where TBaseServiceObject : IBaseObject, new()
        where TProjectionServiceObject : IBaseObject, new()
    {
        private readonly TBaseService _service;
        private readonly Func<TBaseServiceObject, TProjectionServiceObject> _selectorDelegate;

        public ServiceProjectorDecorator(TBaseService service)
        {
            _service = service;
            _selectorDelegate = Projector.Selector<TBaseServiceObject, TProjectionServiceObject>().Compile();
        }

        public IQueryable<TProjectionServiceObject> GetAll(IUnitOfWork unit_of_work, bool? hidden)
        {
            return _service.GetAll(unit_of_work, hidden).Select(Projector.Selector<TBaseServiceObject, TProjectionServiceObject>());
        }

        public TProjectionServiceObject Get(IUnitOfWork unitOfWork, int id)
        {
            return _selectorDelegate(_service.Get(unitOfWork, id));
        }

        public TProjectionServiceObject Create(IUnitOfWork unitOfWork, TProjectionServiceObject obj)
        {
            return Projector.Map<TProjectionServiceObject, TBaseServiceObject>(unitOfWork, obj, _service.Create);
        }

        public IReadOnlyCollection<TProjectionServiceObject> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<TProjectionServiceObject> collection)
        {
            throw new NotImplementedException();
        }

        public TProjectionServiceObject Update(IUnitOfWork unitOfWork, TProjectionServiceObject obj)
        {
            return Projector.Map<TProjectionServiceObject, TBaseServiceObject>(unitOfWork, obj, _service.Update);
        }

        public IReadOnlyCollection<TProjectionServiceObject> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<TProjectionServiceObject> collection)
        {
            throw new NotImplementedException();
        }

        public void Delete(IUnitOfWork unitOfWork, TProjectionServiceObject obj)
        {
            throw new NotImplementedException();
        }

        public void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<TProjectionServiceObject> collection)
        {
            throw new NotImplementedException();
        }

        public void ChangeSortOrder(IUnitOfWork unitOfWork, TProjectionServiceObject obj, int posId)
        {
            throw new NotImplementedException();
        }

        public TProjectionServiceObject CreateDefault(IUnitOfWork unitOfWork)
        {
            var createdDefaultBase = _service.CreateDefault(unitOfWork);
            var createdDefault = _selectorDelegate(createdDefaultBase);
            return createdDefault;
        }

        public IQueryable<TProjectionServiceObject> GetById(IUnitOfWork unitOfWork, bool? hidden, int id)
        {
            return GetAll(unitOfWork, hidden).Where(estate => estate.ID == id);
        }

        public Type EntityType { get; }
    }

}
