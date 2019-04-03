using System;

namespace Base.Utils.Common.Caching
{
    public class CacheOptions
    {
        public static TimeSpan DefaultDuration = TimeSpan.FromHours(1);
    }

    public class CacheAccessor<T>
    {
        public CacheAccessor(TimeSpan? duration = null, bool renew = true)
        {
            CacheKey = Guid.NewGuid().ToString("N");

            Duration = duration ?? CacheOptions.DefaultDuration;
            Renew = renew;
        }

        public string CacheKey { get; }

        public TimeSpan Duration { get; }

        public bool Renew { get; }

        public CacheDependencyKey GetDependencyKey(string key)
        {
            return new CacheDependencyKey(CacheKey + key);
        }
    }
}