using System.Collections.Generic;
using System.Linq;
using Base.Events;

namespace Base.Map.Clustering
{
    public class QuadClusterCacheManager : IClusterCacheManager
    {
        private readonly IClusterProvider _cluster_provider;

        public QuadClusterCacheManager(IClusterProvider cluster_provider)
        {
            _cluster_provider = cluster_provider;
        }

        public CacheInfo GetCacheInfo(string groupKey)
        {
            return QuardGridCache.GetInfo(groupKey);
        }

        public IEnumerable<CacheGroup> GetCacheGroups()
        {
            return QuardGridCache.GetGroups();
        }

        public bool HasCacheGroup(string groupKey)
        {
            return QuardGridCache.HasGroup(groupKey);
        }

        public CacheStats GetCacheStats()
        {
            return QuardGridCache.GetStats();
        }

        public void ResetStats()
        {
            QuardGridCache.ResetStats();
        }

        public void ClearCache(string groupKey)
        {
            QuardGridCache.Clear(groupKey);
        }

        public void ClearAllCache()
        {
            QuardGridCache.ClearAll();
        }

        public void OnEvent(IChangeObjectEvent<IGeoObject> evnt)
        {

            foreach (var group in QuardGridCache.GetGroups().Where(x=>x.Type == evnt.Modified?.GetType()))
            {
                var grid = _cluster_provider.CreateGridGroup(null, new CacheSettings(group.Key, null, null));
                if (evnt.Modified != null)
                    grid.RemoveObject(evnt.Modified);

                if (evnt.Original != null)
                    grid.AddObject(evnt.Original);
            }

        }
    }
}