using Base.Map.Spatial;
using System.Linq;

namespace Base.Map.Clustering
{
    public class QuadClusterProvider : IClusterProvider
    {
        public IClusterGrid CreateGrid(int zoom, Bounds? constraintBounds = null, CacheSettings cacheSettings = null)
        {
            return cacheSettings != null ?
                CreateCachedGrid(zoom, constraintBounds, cacheSettings) :
                new QuadClusterGrid(zoom, null, constraintBounds);
        }

        public IClusterGridGroup CreateGridGroup(Bounds? constraintBounds = null, CacheSettings cacheSettings = null)
        {
            return new QuadClusterGridGroup(this, constraintBounds, cacheSettings);
        }

        private IClusterGrid CreateCachedGrid(int zoom, Bounds? constraintBounds, CacheSettings cacheSettings)
        {
            var grid = new QuadClusterGrid(zoom, null, constraintBounds);
            var cache = new QuardGridCache(cacheSettings);
            return new CachedQuadClusterGrid(grid, cache);
        }

        public Bounds? GetObjectBounds(IQueryable<IGeoObject> query, CacheSettings cacheSettings = null)
        {
            var cache = new QuardGridCache(cacheSettings);
            var provider = new QuadBoundsProvider(cache);
            return provider.FindBounds(query);
        }
    }
}