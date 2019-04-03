using Base.Map.Spatial;
using System.Linq;

namespace Base.Map.Clustering
{
    public interface IClusterProvider : IClusterGridFactory
    {
        IClusterGridGroup CreateGridGroup(Bounds? constraintBounds = null, CacheSettings cacheSettings = null);

        Bounds? GetObjectBounds(IQueryable<IGeoObject> query, CacheSettings cacheSettings = null);
    }
}