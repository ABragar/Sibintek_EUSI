using System;
using System.Collections.Concurrent;
using System.Linq;
using Base.DAL;
using Base.Service.Crud.Internal;

namespace Base.Service.Crud
{
    public static class CategorizedItemCrudServiceExtensions
    {
        private static ConcurrentDictionary<Type, ICategorizedItemCrudServiceImplementation> _dictionary = new ConcurrentDictionary<Type, ICategorizedItemCrudServiceImplementation>();

        private static ICategorizedItemCrudServiceImplementation GetImplimentation(ICategorizedItemCrudService service)
        {

            return _dictionary.GetOrAdd(service.EntityType,
                x => (ICategorizedItemCrudServiceImplementation)Activator.CreateInstance(typeof(CategorizedItemCrudServiceImplementation<>).MakeGenericType(x)));
        }

        public static IQueryable GetCategorizedItems(this ICategorizedItemCrudService service,
            IUnitOfWork unitOfWork,
            int categoryID,
            bool? hidden = false)
        {
            return GetImplimentation(service).GetCategorizedItems(service, unitOfWork, categoryID, hidden);
        }

        public static IQueryable GetAllCategorizedItems(this ICategorizedItemCrudService service,
            IUnitOfWork unitOfWork,
            int categoryID,
            bool? hidden = false)
        {
            return GetImplimentation(service).GetAllCategorizedItems(service, unitOfWork, categoryID, hidden);
        }

        public static void ChangeCategory(this ICategorizedItemCrudService service,
            IUnitOfWork unitOfWork,
            int id,
            int newCategoryID)
        {
            GetImplimentation(service).ChangeCategory(service, unitOfWork, id, newCategoryID);
        }
    }
}