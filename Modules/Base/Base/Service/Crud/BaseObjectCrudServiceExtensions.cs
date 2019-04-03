using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service.Crud.Internal;

namespace Base.Service.Crud
{
    public static class BaseObjectCrudServiceExtensions
    {
        private static ConcurrentDictionary<Type, IBaseObjectCrudServiceImplementation> _dictionary = new ConcurrentDictionary<Type, IBaseObjectCrudServiceImplementation>();

        private static IBaseObjectCrudServiceImplementation GetImplimentation(IBaseObjectCrudService service)
        {

            return _dictionary.GetOrAdd(service.EntityType,
                x => (IBaseObjectCrudServiceImplementation)Activator.CreateInstance(typeof (BaseObjectCrudServiceImplementation<>).MakeGenericType(x)));
        }

        public static IQueryable GetAll(this IBaseObjectCrudService service,
            IUnitOfWork unitOfWork,
            bool? hidden = false)
        {
            return GetImplimentation(service).GetAll(service, unitOfWork, hidden);
        }

        public static IQueryable GetById(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, int id, bool? hidden = false)
        {
            return GetImplimentation(service).GetById(service, unitOfWork, hidden, id);
        }

        public static BaseObject Get(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, int id)
        {
            return GetImplimentation(service).Get(service, unitOfWork, id);
        }

        public static BaseObject Create(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj)
        {
            return GetImplimentation(service).Create(service, unitOfWork, obj);
        }

        public static IReadOnlyCollection<BaseObject> CreateCollection(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> objs)
        {
            return GetImplimentation(service).CreateCollection(service, unitOfWork, objs);
        }

        public static BaseObject Update(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj)
        {
            return GetImplimentation(service).Update(service, unitOfWork, obj);
        }

        public static IReadOnlyCollection<BaseObject> UpdateCollection(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> objs)
        {
            return GetImplimentation(service).UpdateCollection(service, unitOfWork, objs);
        }

        public static void Delete(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, BaseObject obj)
        {
            GetImplimentation(service).Delete(service, unitOfWork, obj);
        }

        public static void DeleteCollection(this IBaseObjectCrudService service, IUnitOfWork unitOfWork, IReadOnlyCollection<BaseObject> objs)
        {
            GetImplimentation(service).DeleteCollection(service, unitOfWork, objs);
        }

        public static void ChangeSortOrder(this IBaseObjectCrudService service,
            IUnitOfWork unitOfWork,
            BaseObject obj,
            int posId)
        {
            GetImplimentation(service).ChangeSortOrder(service, unitOfWork, obj, posId);
        }

        public static BaseObject CreateDefault(this IBaseObjectCrudService service,
            IUnitOfWork unitOfWork)
        {
            return GetImplimentation(service).CreateDefault(service, unitOfWork);
        }

    }
}