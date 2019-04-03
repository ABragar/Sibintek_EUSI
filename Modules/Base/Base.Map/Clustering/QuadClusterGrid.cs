using Base.Map.Helpers;
using Base.Map.MapObjects;
using Base.Map.Spatial;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace Base.Map.Clustering
{
    internal class QuadClusterGrid : IClusterGrid
    {
        private static readonly Bounds _defaultBounds = CRS.Current.Projection.GetBounds();

        private const int _defaultPage = 1;
        private const int _defaultPageSize = 50;

        private const int _minZoom = 0;
        private const int _maxZoom = 18;

        private readonly long _gridSize;
        private readonly double _cellWidth;
        private readonly double _cellHeight;
        private readonly Bounds _gridBounds;
        private readonly Bounds? _gridConstraints;

        internal int Zoom { get; }

        public QuadClusterGrid(int zoom, Bounds? gridBounds = null, Bounds? gridConstraints = null)
        {
            if (zoom < _minZoom || zoom > _maxZoom)
            {
                throw new ArgumentOutOfRangeException(nameof(zoom), $"Zoom must be in the range from {_minZoom} to {_maxZoom}.");
            }

            Zoom = zoom;
            _gridBounds = gridBounds ?? _defaultBounds;
            _gridConstraints = gridConstraints;

            _gridSize = 1L << Zoom + 1;
            _cellWidth = (_gridBounds.Max.X - _gridBounds.Min.X) / _gridSize;
            _cellHeight = (_gridBounds.Max.Y - _gridBounds.Min.Y) / _gridSize;
        }

        public IEnumerable<GeoObjectBase> GetClusters(IQueryable<IGeoObject> query, Bounds bounds, Type entityType)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var gridBounds = GetGridBounds(bounds);
            var clusters = FindClusters(query, gridBounds);
            return ProcessSingleClusters(query, clusters, entityType);
        }

        internal IEnumerable<GeoObjectBase> GetClusters(IQueryable<IGeoObject> query, QuadGridBounds gridBounds, Type entityType)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (gridBounds == null)
            {
                throw new ArgumentNullException(nameof(gridBounds));
            }

            var clusters = FindClusters(query, gridBounds);
            return ProcessSingleClusters(query, clusters, entityType);
        }

        public IEnumerable<GeoObjectBase> GetNonClusteredObjects(IQueryable<IGeoObject> query, Bounds bounds, Type entityType)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            var gridBounds = GetGridBounds(bounds);
            return FindNonClusteredObjects(query, gridBounds, entityType);
        }

        public PagingResult<GeoObjectBase> GetObjects(IQueryable<IGeoObject> query, long clusterId, int page, int pageSize, Type entityType)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            var cell = QuadGridCell.FromId(clusterId);

            if (cell.X < 0 || cell.Y < 0 || cell.X >= _gridSize || cell.Y >= _gridSize)
            {
                throw new ArgumentException("Invalid cluster id.", nameof(clusterId));
            }

            if (page < 1)
            {
                page = _defaultPage;
            }

            if (pageSize < 1)
            {
                pageSize = _defaultPageSize;
            }

            var gridBounds = GetGridBounds(new[] { cell });
            return FindObjects(query, gridBounds, page, pageSize, entityType);
        }

        public void AddObject(IGeoObject obj)
        {
            throw new InvalidOperationException("This class does not support this operation.");
        }

        public void RemoveObject(IGeoObject obj)
        {
            throw new InvalidOperationException("This class does not support this operation.");
        }

        private Point GetCellPosition(QuadGridCell cell)
        {
            return new Point(_gridBounds.Min.X + (cell.X * _cellWidth), _gridBounds.Min.Y + (cell.Y * _cellHeight));
        }

        internal QuadGridCell GetCell(Point point)
        {
            var x = (int)Math.Floor((point.X - _gridBounds.Min.X) / _cellWidth);
            var y = (int)Math.Floor((point.Y - _gridBounds.Min.Y) / _cellHeight);

            if (x >= _gridSize) x = x - 1;
            if (y >= _gridSize) y = y - 1;

            if (x < 0) x = 0;
            if (y < 0) y = 0;

            return new QuadGridCell(x, y);
        }

        internal QuadGridBounds GetGridBounds(Bounds bounds)
        {
            var constraintBounds = _gridConstraints ?? _gridBounds;

            var boundMin = constraintBounds.Contains(bounds.Min) ? bounds.Min : constraintBounds.Min;
            var boundMax = constraintBounds.Contains(bounds.Max) ? bounds.Max : constraintBounds.Max;

            var minCell = GetCell(boundMin);
            var maxCell = GetCell(boundMax);

            return GetGridBounds(new[] { minCell, maxCell });
        }

        internal QuadGridBounds GetGridBounds(IReadOnlyCollection<QuadGridCell> gridCells)
        {
            if (gridCells == null || !gridCells.Any())
            {
                throw new ArgumentException("Grid cells is empty.", nameof(gridCells));
            }

            var minMaxCell = gridCells.GroupBy(x => 1).Select(x => new
            {
                MinCell = new QuadGridCell(x.Min(c => c.X), x.Min(c => c.Y)),
                MaxCell = new QuadGridCell(x.Max(c => c.X), x.Max(c => c.Y))
            }).First();

            var min = GetCellPosition(minMaxCell.MinCell);

            var maxCellPos = GetCellPosition(minMaxCell.MaxCell);
            var max = new Point(maxCellPos.X + _cellWidth, maxCellPos.Y + _cellHeight);

            return new QuadGridBounds(minMaxCell.MinCell, minMaxCell.MaxCell, min, max);
        }

        private IEnumerable<GeoObjectBase> ProcessSingleClusters(IQueryable<IGeoObject> query, IEnumerable<GeoCluster> clusters, Type entityType)
        {
            if (!clusters.Any())
            {
                return Enumerable.Empty<GeoObjectBase>();
            }

            var resultObjects = new List<GeoObjectBase>();
            var notClusters = new Dictionary<QuadGridCell, GeoCluster>(new QuadGridCellComparer());

            foreach (var cluster in clusters)
            {
                if (cluster.Count == 1)
                {
                    notClusters[new QuadGridCell(cluster.X, cluster.Y)] = cluster;
                }
                else
                {
                    resultObjects.Add(cluster);
                }
            }

            if (notClusters.Count == 0)
            {
                return resultObjects;
            }

            var gridBounds = GetGridBounds(notClusters.Keys);
            var geoObjects = FindSingleObjects(query, gridBounds, entityType);

            foreach (var notCluster in notClusters)
            {
                GeoObjectBase obj;
                if (geoObjects.TryGetValue(notCluster.Key, out obj))
                {
                    resultObjects.Add(obj);
                }
            }

            return resultObjects;
        }

        private IEnumerable<GeoCluster> FindClusters(IQueryable<IGeoObject> query, QuadGridBounds gridBounds)
        {
            if (!gridBounds.IsValid)
            {
                return Enumerable.Empty<GeoCluster>();
            }

            var cellWidth = gridBounds.CellWidth;
            var cellHeight = gridBounds.CellHeight;

            query = BuildClusterWhereClause(query, gridBounds.Bounds, cellWidth, cellHeight);

            var clusters = query.GroupBy(x => new
            {
                xCell = Math.Floor((x.Location.CenterPointX - gridBounds.Bounds.Min.X) / cellWidth),
                yCell = Math.Floor((x.Location.CenterPointY - gridBounds.Bounds.Min.Y) / cellHeight)
            }).Select(x => new
            {
                XCell = x.Key.xCell,
                YCell = x.Key.yCell,
                Count = x.Count(),
                Geometry = SqlSpatialFunctions.PointGeography(
                    // Y to Latitude: (2 * Math.Atan(Math.Exp(y / EarthRadius)) - PI2) * Rad2Deg
                    (2 * SqlFunctions.Atan(SqlFunctions.Exp(x.Average(k => k.Location.CenterPointY) / SphericalMercator.EarthRadius)) - MathHelper.PI2) * MathHelper.Rad2Deg,

                    // X to Longitude: (x * Rad2Deg) / EarthRadius
                    (x.Average(k => k.Location.CenterPointX) * MathHelper.Rad2Deg) / SphericalMercator.EarthRadius,

                    // EPSG: 4326
                    DbGeography.DefaultCoordinateSystemId)
            }).AsEnumerable().Select(x => new GeoCluster
            {
                ID = QuadGridCell.GetId(gridBounds.MinCell.X + (int)x.XCell, gridBounds.MinCell.Y + (int)x.YCell),
                X = gridBounds.MinCell.X + (int)x.XCell,
                Y = gridBounds.MinCell.Y + (int)x.YCell,
                Count = x.Count,
                Geometry = x.Geometry,
                Type = GeoObjectType.Cluster
            }).ToList();

            return clusters;
        }

        private IEnumerable<GeoObjectBase> FindNonClusteredObjects(IQueryable<IGeoObject> query, QuadGridBounds gridBounds, Type entityType)
        {
            if (!gridBounds.IsValid)
            {
                return Enumerable.Empty<GeoObjectBase>();
            }

            query = BuildNonClusteredWhereClause(query, gridBounds.Bounds, gridBounds.CellWidth, gridBounds.CellHeight);

            var select = CheckType.IsIconGeoObject(entityType) ?
                SelectIconGeoObjects((IQueryable<IIconGeoObject>)query) :
                SelectGeoObjects(query);

            return select.ToList();
        }

        private PagingResult<GeoObjectBase> FindObjects(IQueryable<IGeoObject> query, QuadGridBounds gridBounds, int page, int pageSize, Type entityType)
        {
            if (!gridBounds.IsValid)
            {
                return PagingResult<GeoObjectBase>.Empty;
            }

            query = BuildClusterWhereClause(query, gridBounds.Bounds, gridBounds.CellWidth, gridBounds.CellHeight);

            var totalCount = query.Count();

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var select = CheckType.IsIconGeoObject(entityType) ?
                SelectIconGeoObjects((IQueryable<IIconGeoObject>)query) :
                SelectGeoObjects(query);

            var pagingResult = new PagingResult<GeoObjectBase>
            {
                PageSize = pageSize,
                Page = page,
                TotalCount = totalCount,
                Items = select.ToList()
            };

            return pagingResult;
        }

        private IDictionary<QuadGridCell, GeoObjectBase> FindSingleObjects(IQueryable<IGeoObject> query, QuadGridBounds gridBounds, Type entityType)
        {
            if (!gridBounds.IsValid)
            {
                return new Dictionary<QuadGridCell, GeoObjectBase>();
            }

            query = BuildClusterWhereClause(query, gridBounds.Bounds, gridBounds.CellWidth, gridBounds.CellHeight);

            var select = CheckType.IsIconGeoObject(entityType) ?
                SelectIconGeoObjects((IQueryable<IIconGeoObject>)query) :
                SelectGeoObjects(query);

            var result = new Dictionary<QuadGridCell, GeoObjectBase>(new QuadGridCellComparer());

            foreach (var obj in select)
            {
                var cell = GetCell(new Point(obj.PX, obj.PY));
                obj.X = cell.X;
                obj.Y = cell.Y;
                result[cell] = obj;
            }

            return result;
        }

        public IQueryable<IGeoObject> BuildClusterWhereClause(IQueryable<IGeoObject> query, Bounds bbox, double width, double height)
        {
            var predicate = PredicateBuilder.False<IGeoObject>();

            predicate = predicate.Or(x => x.Location.HasBounds &&
                x.Location.HasCenterPoint &&
                x.Location.CenterPointY >= bbox.Min.Y &&
                x.Location.CenterPointY <= bbox.Max.Y &&
                x.Location.CenterPointX >= bbox.Min.X &&
                x.Location.CenterPointX <= bbox.Max.X &&
                (x.Location.BoundMaxX - x.Location.BoundMinX) <= width &&
                (x.Location.BoundMaxY - x.Location.BoundMinY) <= height);

            predicate = predicate.Or(x => !x.Location.HasBounds &&
                x.Location.HasCenterPoint &&
                x.Location.CenterPointY >= bbox.Min.Y &&
                x.Location.CenterPointY <= bbox.Max.Y &&
                x.Location.CenterPointX >= bbox.Min.X &&
                x.Location.CenterPointX <= bbox.Max.X);

            return query.AsExpandable().Where(predicate);
        }

        public IQueryable<IGeoObject> BuildNonClusteredWhereClause(IQueryable<IGeoObject> query, Bounds bbox, double width, double height)
        {
            query = query.Where(x => x.Location.HasBounds &&
                (x.Location.BoundMinY - bbox.Max.Y <= 0) &&
                (x.Location.BoundMinX - bbox.Max.X <= 0) &&
                (bbox.Min.Y - x.Location.BoundMaxY <= 0) &&
                (bbox.Min.X - x.Location.BoundMaxX <= 0) &&
                (x.Location.BoundMaxX - x.Location.BoundMinX) > width &&
                (x.Location.BoundMaxY - x.Location.BoundMinY) > height);

            return query;
        }

        protected IEnumerable<GeoObject> SelectGeoObjects(IQueryable<IGeoObject> query)
        {
            return query.Select(model => new GeoObject
            {
                ID = model.ID,
                Title = model.Title,
                Description = model.Description,
                PX = model.Location.CenterPointX,
                PY = model.Location.CenterPointY,
                Geometry = model.Location.Disposition,
                Type = GeoObjectType.Object
            });
        }

        protected IEnumerable<GeoObject> SelectIconGeoObjects(IQueryable<IIconGeoObject> query)
        {
            return query.Select(model => new IconGeoObject
            {
                ID = model.ID,
                Title = model.Title,
                Description = model.Description,
                PX = model.Location.CenterPointX,
                PY = model.Location.CenterPointY,
                Geometry = model.Location.Disposition,
                Icon = model.Icon.Value,
                Color = model.Icon.Color,
                Type = GeoObjectType.IconObject
            });
        }
    }
}