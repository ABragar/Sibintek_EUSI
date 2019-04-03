using System;
using System.Threading.Tasks;

namespace Base.Utils.Common.Caching
{
    public interface ISimpleCacheWrapper
    {
        bool TryRemove<T>(CacheAccessor<T> accessor, string key);
        T GetOrAdd<T>(CacheAccessor<T> accessor, string key, Func<T> func, params CacheDependencyKey[] dependencies);
        Task<T> GetOrAddAsync<T>(CacheAccessor<T> accessor, string key, Func<Task<T>> func, params CacheDependencyKey[] dependencies);

    }
}