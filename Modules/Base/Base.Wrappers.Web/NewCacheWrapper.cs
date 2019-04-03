using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Caching;
using Base.Utils.Common.Caching;

namespace Base.Wrappers.Web
{
    public class NewCacheWrapper : IExtendedCacheWrapper
    {
        private readonly Cache _cache;

        public NewCacheWrapper(Cache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));


            _cache = cache;
        }

        public bool TryRemove<T>(CacheAccessor<T> accessor, string key)
        {

            return _cache.Remove(accessor.CacheKey + key) != null;

        }

        public T GetOrAdd<T>(CacheAccessor<T> accessor, string key, Func<T> func, params CacheDependencyKey[] dependencies)
        {

            TaskCompletionSource<T> source = null;
            Action remove_trigger;

            do
            {
                var task = (Task<T>)_cache.Get(accessor.CacheKey + key);

                try
                {
                    if (task != null)
                        return task.Result;
                }
                catch (OperationCanceledException)
                {
                    continue;

                }

                if (source == null)
                {
                    source = new TaskCompletionSource<T>();

                }
                var new_task = source.Task;


                task = (Task<T>)_cache.Add(
                    accessor.CacheKey + key,
                    new_task,
                    CreateDependency(dependencies, true, out remove_trigger),
                    accessor.Renew ? Cache.NoAbsoluteExpiration : DateTime.Now.Add(accessor.Duration),
                    accessor.Renew ? accessor.Duration : Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);


                try
                {
                    if (task != null && task != new_task)
                    {
                        var result = task.Result;
                        source.SetResult(result);
                        return result;
                    }
                }
                catch (OperationCanceledException)
                {
                    continue;
                }

                break;



            } while (true);


            try
            {
                var result = func();

                source.SetResult(result);

                return result;
            }
            catch (Exception ex)
            {
                remove_trigger();
                source.SetCanceled();
                throw;
            }



        }

        public async Task<T> GetOrAddAsync<T>(CacheAccessor<T> accessor, string key, Func<Task<T>> func, params CacheDependencyKey[] dependencies)
        {


            TaskCompletionSource<T> source = null;
            Action remove_trigger;

            do
            {
                var task = (Task<T>)_cache.Get(accessor.CacheKey + key);

                try
                {
                    if (task != null)
                        return await task;
                }
                catch (OperationCanceledException)
                {
                    continue;

                }


                if (source == null)
                {
                    source = new TaskCompletionSource<T>();

                }

                var new_task = source.Task;

                task = (Task<T>)_cache.Add(
                    accessor.CacheKey + key,
                    new_task,
                    CreateDependency(dependencies, true, out remove_trigger),
                    accessor.Renew ? Cache.NoAbsoluteExpiration : DateTime.Now.Add(accessor.Duration),
                    accessor.Renew ? accessor.Duration : Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);



                try
                {
                    if (task != null && task != new_task)
                    {
                        var result = await task;
                        source.SetResult(result);
                        return result;
                    }
                }
                catch (OperationCanceledException)
                {
                    continue;
                }

                break;



            } while (true);


            try
            {
                var result = await func();

                source.SetResult(result);

                return result;
            }
            catch (Exception)
            {
                remove_trigger();
                source.SetCanceled();
                throw;
            }



        }


        public void AddOrUpdate<T>(CacheAccessor<T> accessor, string key, T value, params CacheDependencyKey[] dependencies)
        {
            Action trigger;

            _cache.Insert(
                accessor.CacheKey + key,
                Task.FromResult(value),
                CreateDependency(dependencies, false, out trigger),
                accessor.Renew ? Cache.NoAbsoluteExpiration : DateTime.Now.Add(accessor.Duration),
                accessor.Renew ? accessor.Duration : Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                null);
        }


        public bool TryGet<T>(CacheAccessor<T> accessor, string key, out T value)
        {

            var task = _cache.Get(accessor.CacheKey + key) as Task<T>;

            if (task != null)
            {
                value = task.Result;
                return true;
            }

            value = default(T);

            return false;
        }


        private static CacheDependency CreateDependency(CacheDependencyKey[] dependencies, bool need_trigger, out Action trigger)
        {
            if (dependencies == null && !need_trigger)
            {
                trigger = null;
                return null;
            }
            var dependency = new Dependency(dependencies?.Select(x => x.Value).ToArray());
            trigger = dependency.Remove;

            return dependency;
        }

        private class Dependency : CacheDependency
        {
            public Dependency(string[] keys) : base(null, keys)
            {

            }

            public void Remove()
            {
                NotifyDependencyChanged(this, EventArgs.Empty);
            }
        }
    }
}