using Base.Map.Filters;
using Base.Map.Spatial;
using LinqKit;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic;

namespace Base.Map.Criteria
{
    public class CriteriaBuilder : ICriteriaBuilder
    {
        private readonly IFilterManager _filterManager;

        public CriteriaBuilder(IFilterManager filterManager)
        {
            if (filterManager == null)
            {
                throw new ArgumentNullException(nameof(filterManager));
            }

            _filterManager = filterManager;
        }

        public IQueryable BuildFilterWhereClause(IQueryable query, string mnemonic, FilterValues filters)
        {
            return _filterManager.BuildFilterWhereClause(query, mnemonic, filters);
        }

        public IQueryable<IGeoObject> BuildBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox, BoundsMode mode)
        {
            return BuildBoundsWhereClause(query, CRS.Current.Project(bbox), mode);
        }

        public IQueryable<IGeoObject> BuildBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox, BoundsMode mode)
        {
            switch (mode)
            {
                case BoundsMode.Intersects:
                    query = BuildIntersectsBoundsWhereClause(query, bbox);
                    break;

                case BoundsMode.Contains:
                    query = BuildContainsBoundsWhereClause(query, bbox);
                    break;

                case BoundsMode.Overlaps:
                    query = BuildOverlapBoundsWhereClause(query, bbox);
                    break;

                default:
                    throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(BoundsMode));
            }

            return query;
        }

        public IQueryable<IGeoObject> BuildAroundOnDistancePointWhereClause(IQueryable<IGeoObject> query, GeoPoint point, int? zoom,
            double? distanceRatioInPixels = null, double? minDistance = null, double? maxDistance = null)
        {
            var aroundOnDistancePoint = SpatialHelper.ConvertLatLngToDbGeography(point.Latitude, point.Longitude);

            if (zoom.HasValue)
            {
                const double defaultDistanceRatioInPixels = 20;
                const double defaultMinDistance = 10;
                const double defaultMaxDistance = 2000;

                var distanceInMeters = SpatialHelper.CalculateDistanceFromZoom(point.Latitude, zoom.Value,
                    distanceRatioInPixels ?? defaultDistanceRatioInPixels,
                    minDistance ?? defaultMinDistance,
                    maxDistance ?? defaultMaxDistance);

                query = query.Where(x => x.Location.Disposition.Distance(aroundOnDistancePoint) < distanceInMeters);
            }
            else
            {
                query = query.OrderBy(x => x.Location.Disposition.Distance(aroundOnDistancePoint)).Take(1);
            }

            return query;
        }

        public IQueryable<IGeoObject> BuildIntersectsBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox)
        {
            return BuildIntersectsBoundsWhereClause(query, CRS.Current.Project(bbox));
        }

        public IQueryable<IGeoObject> BuildIntersectsBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox)
        {
            var predicate = PredicateBuilder.False<IGeoObject>();

            // Test intersects a bounds
            predicate = predicate.Or(x => x.Location.HasBounds &&
                (x.Location.BoundMinY - bbox.Max.Y <= 0) &&
                (x.Location.BoundMinX - bbox.Max.X <= 0) &&
                (bbox.Min.Y - x.Location.BoundMaxY <= 0) &&
                (bbox.Min.X - x.Location.BoundMaxX <= 0));

            // Test include a point
            predicate = predicate.Or(x => !x.Location.HasBounds &&
                x.Location.HasCenterPoint &&
                x.Location.CenterPointY >= bbox.Min.Y &&
                x.Location.CenterPointY <= bbox.Max.Y &&
                x.Location.CenterPointX >= bbox.Min.X &&
                x.Location.CenterPointX <= bbox.Max.X);

            return query.AsExpandable().Where(predicate);
        }

        public IQueryable<IGeoObject> BuildContainsBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox)
        {
            return BuildContainsBoundsWhereClause(query, CRS.Current.Project(bbox));
        }

        public IQueryable<IGeoObject> BuildContainsBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox)
        {
            var predicate = PredicateBuilder.False<IGeoObject>();

            // Test contains a bounds
            predicate = predicate.Or(x => x.Location.HasBounds &&
                x.Location.BoundMinY >= bbox.Min.Y &&
                x.Location.BoundMinX >= bbox.Min.X &&
                x.Location.BoundMaxY <= bbox.Max.Y &&
                x.Location.BoundMaxX <= bbox.Max.X);

            // Test include a point
            predicate = predicate.Or(x => !x.Location.HasBounds &&
                x.Location.HasCenterPoint &&
                x.Location.CenterPointY >= bbox.Min.Y &&
                x.Location.CenterPointY <= bbox.Max.Y &&
                x.Location.CenterPointX >= bbox.Min.X &&
                x.Location.CenterPointX <= bbox.Max.X);

            return query.AsExpandable().Where(predicate);
        }

        public IQueryable<IGeoObject> BuildOverlapBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox)
        {
            return BuildOverlapBoundsWhereClause(query, CRS.Current.Project(bbox));
        }

        public IQueryable<IGeoObject> BuildOverlapBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox)
        {
            var predicate = PredicateBuilder.False<IGeoObject>();
            predicate = predicate.Or(x => x.Location.HasBounds);

            // Test overlaps a bounds
            predicate = predicate.And(x => !(
                x.Location.BoundMinY >= bbox.Min.Y &&
                x.Location.BoundMinX >= bbox.Min.X &&
                x.Location.BoundMaxY <= bbox.Max.Y &&
                x.Location.BoundMaxX <= bbox.Max.X));

            // Test overlap a bounds
            predicate = predicate.And(x =>
                (x.Location.BoundMinY - bbox.Max.Y <= 0) &&
                (x.Location.BoundMinX - bbox.Max.X <= 0) &&
                (bbox.Min.Y - x.Location.BoundMaxY <= 0) &&
                (bbox.Min.X - x.Location.BoundMaxX <= 0));

            return query.AsExpandable().Where(predicate);
        }

        public IQueryable<IGeoObject> BuildLocationNotNullWhereClause(IQueryable<IGeoObject> query)
        {
            return query.Where(x => x.Location.HasCenterPoint);
        }

        public IQueryable BuildBaseObjectTypeMnemonicWhereClause(IQueryable query, string mnemonic)
        {
            return query.Where("BoType.Mnemonic = @0", $"{mnemonic}");
        }

        public IQueryable BuildBaseObjectTypeIdWhereClause(IQueryable query, int id)
        {
            return query.Where("BoTypeID = @0", id);
        }

        public IQueryable<IGeoObject> BuildTitleMatchWhereClause(IQueryable<IGeoObject> query, string title)
        {
            return query.Where(x => x.Title.Contains(title));
        }

        public IQueryable<IGeoObject> BuildDescriptionMatchWhereClause(IQueryable<IGeoObject> query, string description)
        {
            return query.Where(x => x.Title.Contains(description));
        }

        public IQueryable<IGeoObject> BuildSearchStringWhereClause(IQueryable<IGeoObject> query, string searchString)
        {
            return query.Where(x => x.Title.Contains(searchString) || x.Description.Contains(searchString));
        }
    }
}