using System;
using Base.DAL;
using Base.Service;
using System.Linq;
using Base.Helpers;
using Base.Utils.Common.Caching;

namespace Base.Settings
{
    public class SettingService<T> : BaseObjectService<T>, ISettingService<T> where T : SettingItem, new()
    {
        private readonly ISimpleCacheWrapper _cache_wrapper;
        private readonly IHelperJsonConverter _json_converter;
        private static readonly CacheAccessor<T> CacheKey = new CacheAccessor<T>(TimeSpan.FromDays(1));

        public SettingService(IBaseObjectServiceFacade facade, ISimpleCacheWrapper cache_wrapper,
            IHelperJsonConverter json_converter)
            : base(facade)
        {
            _cache_wrapper = cache_wrapper;
            _json_converter = json_converter;
        }

        private string GetKey()
        {
            return nameof(T);
        }

        private void RemoveCache()
        {
            _cache_wrapper.TryRemove(CacheKey, GetKey());
        }

        public override T Create(IUnitOfWork unitOfWork, T obj)
        {
            return !Any(unitOfWork) ? base.Create(unitOfWork, obj) : Update(unitOfWork, obj);
        }

        public override T Update(IUnitOfWork unitOfWork, T obj)
        {
            obj.ID = unitOfWork.GetRepository<T>().All().Where(x => !x.Hidden).Select(x => x.ID).Single();
            var res = base.Update(unitOfWork, obj);
            RemoveCache();
            return res;
        }

        public override void Delete(IUnitOfWork unitOfWork, T obj)
        {
            base.Delete(unitOfWork, obj);
            RemoveCache();
        }

        public bool Any(IUnitOfWork unitOfWork)
        {
            return unitOfWork.GetRepository<T>().All().Any();
        }

        public T Get()
        {
            return _cache_wrapper.GetOrAdd(CacheKey, GetKey(), GetSettItem);
        }

        private T GetSettItem()
        {
            using (var uofw = UnitOfWorkFactory.Create())
            {
                var sett = uofw.GetRepository<T>().All().SingleOrDefault(m => !m.Hidden);

                if (sett != null)
                {
                    string serialized = _json_converter.SerializeObject(sett, completeGraph: true);
                    var res = _json_converter.DeserializeObject<T>(serialized);
                    return res;
                }
                else
                {
                    return new T();
                }
            }
        }
    }
}