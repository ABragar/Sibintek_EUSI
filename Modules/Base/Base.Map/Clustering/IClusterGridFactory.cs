using Base.Map.Spatial;

namespace Base.Map.Clustering
{
    public interface IClusterGridFactory
    {
        IClusterGrid CreateGrid(int zoom, Bounds? constraintBounds = null, CacheSettings cacheSettings = null);
    }
}