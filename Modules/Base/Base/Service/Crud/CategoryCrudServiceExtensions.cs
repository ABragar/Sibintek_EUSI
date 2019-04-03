using System;
using System.Collections.Concurrent;
using System.Linq;
using Base.DAL;
using Base.Service.Crud.Internal;

namespace Base.Service.Crud
{
    public static class CategoryCrudServiceExtensions
    {
        private static ConcurrentDictionary<Type, ICategoryCrudServiceImplementation> _dictionary = new ConcurrentDictionary<Type, ICategoryCrudServiceImplementation>();

        private static ICategoryCrudServiceImplementation GetImplimentation(ICategoryCrudService service)
        {
            return _dictionary.GetOrAdd(service.EntityType,
                x => (ICategoryCrudServiceImplementation)Activator.CreateInstance(typeof(CategoryCrudServiceImplementation<>).MakeGenericType(x)));
        }
        public static IQueryable GetRoots(this ICategoryCrudService service,
            IUnitOfWork unitOfWork,
            bool? hidden = false)
        {
            return GetImplimentation(service).GetRoots(service, unitOfWork, hidden);
        }

        //public static Task<IEnumerable<HCategory>> GetRootsAsync(this ICategoryCrudService service,
        //    IUnitOfWork unitOfWork,
        //    bool? hidden = false)
        //{
        //    return GetImplimentation(service).GetRootsAsync(service, unitOfWork, hidden);
        //}

        public static IQueryable GetAllChildren(this ICategoryCrudService service,
            IUnitOfWork unitOfWork,
            int parentID,
            bool? hidden = false)
        {
            return GetImplimentation(service).GetAllChildren(service, unitOfWork, parentID, hidden);
        }

        //public static Task<IEnumerable<HCategory>> GetAllChildrenAsync(this ICategoryCrudService service,
        //    IUnitOfWork unitOfWork,
        //    int parentID,
        //    bool? hidden = false)
        //{
        //    return GetImplimentation(service).GetAllChildrenAsync(service, unitOfWork, parentID, hidden);
        //}

        public static IQueryable GetChildren(this ICategoryCrudService service,
            IUnitOfWork unitOfWork,
            int parentID,
            bool? hidden = false)
        {
            return GetImplimentation(service).GetChildren(service, unitOfWork, parentID, hidden);
        }

        //public static Task<IEnumerable<HCategory>> GetChildrenAsync(this ICategoryCrudService service,
        //    IUnitOfWork unitOfWork,
        //    int parentID,
        //    bool? hidden = false)
        //{
        //    return GetImplimentation(service).GetChildrenAsync(service, unitOfWork, parentID, hidden);
        //}

        public static void ChangePosition(this ICategoryCrudService service,
            IUnitOfWork unitOfWork,
            HCategory obj,
            int? posChangeID,
            string typePosChange)
        {
            GetImplimentation(service).ChangePosition(service, unitOfWork, obj, posChangeID, typePosChange);
        }
    }
}