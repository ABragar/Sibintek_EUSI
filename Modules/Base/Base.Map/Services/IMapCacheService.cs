using Base.Events;
using Base.Map.Clustering;
using System.Collections.Generic;

namespace Base.Map.Services
{
    public interface IMapCacheService
    {
        CacheInfo GetCacheInfo(string groupKey);

        IEnumerable<CacheInfo> GetAllCacheInfo();

        IEnumerable<CacheGroup> GetCacheGroups();

        void EnableCache(string groupKey);

        void DisableCache(string groupKey);

        int UpdateCache(string groupKey, int? level = null);

        void ClearCache(string groupKey);

        void ClearAllCache();

        CacheStats GetCacheStats();

        void ResetCacheStats();
    }
}