using Base.Map.MapObjects;
using Base.Map.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Clustering
{
    public interface IClusterGrid
    {
        IEnumerable<GeoObjectBase> GetClusters(IQueryable<IGeoObject> query, Bounds bounds, Type entityType);

        IEnumerable<GeoObjectBase> GetNonClusteredObjects(IQueryable<IGeoObject> query, Bounds bounds, Type entityType);

        PagingResult<GeoObjectBase> GetObjects(IQueryable<IGeoObject> query, long clusterId, int page, int pageSize, Type entityType);

        void AddObject(IGeoObject obj);

        void RemoveObject(IGeoObject obj);
    }
}