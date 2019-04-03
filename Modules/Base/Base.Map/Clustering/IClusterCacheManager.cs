using System.Collections.Generic;
using Base.Events;

namespace Base.Map.Clustering
{
    public interface IClusterCacheManager: IEventBusHandler<IChangeObjectEvent<IGeoObject>>
    {
        CacheInfo GetCacheInfo(string groupKey);

        IEnumerable<CacheGroup> GetCacheGroups();

        bool HasCacheGroup(string groupKey);

        CacheStats GetCacheStats();

        void ResetStats();

        void ClearCache(string groupKey);

        void ClearAllCache();
    }
}