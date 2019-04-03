using Base.Map.MapObjects;
using Base.Map.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Clustering
{
    internal class CachedQuadClusterGrid : IClusterGrid
    {
        private readonly QuadClusterGrid _grid;
        private readonly QuardGridCache _cache;

        public CachedQuadClusterGrid(QuadClusterGrid grid, QuardGridCache cache)
        {
            if (grid == null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            _grid = grid;
            _cache = cache;
        }

        public IEnumerable<GeoObjectBase> GetClusters(IQueryable<IGeoObject> query, Bounds bounds, Type entityType)
        {
            var gridBounds = _grid.GetGridBounds(bounds);

            if (!gridBounds.IsValid)
            {
                return Enumerable.Empty<GeoCluster>();
            }

            var cachedObjects = new Dictionary<long, GeoObjectBase>();
            var notCachedCells = new List<QuadGridCell>();

            foreach (var gridCell in gridBounds.Cells)
            {
                GeoObjectBase item;

                if (_cache.TryGet(gridCell.X, gridCell.Y, _grid.Zoom, out item))
                {
                    if (item != null)
                    {
                        cachedObjects.Add(QuadGridCell.GetId(item.X, item.Y), item);
                    }

                    _cache.Hit();
                }
                else
                {
                    notCachedCells.Add(gridCell);
                    _cache.Miss();
                }
            }

            if (!notCachedCells.Any())
            {
                return cachedObjects.Values;
            }

            var notCachedBounds = _grid.GetGridBounds(notCachedCells);
            var notCachedObjects = _grid.GetClusters(query, notCachedBounds, entityType);

            foreach (var obj in notCachedObjects)
            {
                cachedObjects[QuadGridCell.GetId(obj.X, obj.Y)] = obj;
            }

            foreach (var gridCell in notCachedBounds.Cells)
            {
                var cellId = gridCell.Id;
                _cache.Insert(gridCell.X, gridCell.Y, _grid.Zoom, cachedObjects.ContainsKey(cellId) ?
                    cachedObjects[cellId] : null);
            }

            return cachedObjects.Values;
        }

        public IEnumerable<GeoObjectBase> GetNonClusteredObjects(IQueryable<IGeoObject> query, Bounds bounds, Type entityType)
        {
            return _grid.GetNonClusteredObjects(query, bounds, entityType);
        }

        public PagingResult<GeoObjectBase> GetObjects(IQueryable<IGeoObject> query, long clusterId, int page, int pageSize, Type entityType)
        {
            return _grid.GetObjects(query, clusterId, page, pageSize, entityType);
        }

        public void AddObject(IGeoObject obj)
        {
            RemoveObject(obj);
        }

        public void RemoveObject(IGeoObject obj)
        {
            if (obj?.Location == null)
            {
                return;
            }

            var point = new Point(obj.Location.CenterPointX, obj.Location.CenterPointY);
            var cell = _grid.GetCell(point);

            _cache.Remove(cell.X, cell.Y, _grid.Zoom);
            _cache.RemoveSettings();
        }
    }
}