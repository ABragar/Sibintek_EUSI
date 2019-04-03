namespace Base.Utils.Common.Caching
{
    public interface IExtendedCacheWrapper : ISimpleCacheWrapper
    {
        void AddOrUpdate<T>(CacheAccessor<T> accessor, string key, T value, params CacheDependencyKey[] dependencies);

        bool TryGet<T>(CacheAccessor<T> accessor, string key, out T value);

    }
}