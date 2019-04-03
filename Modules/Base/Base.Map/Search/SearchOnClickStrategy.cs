using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.MapObjects;
using Base.Map.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Search
{
    internal class SearchOnClickStrategy : SearchStrategyBase
    {
        public SearchOnClickStrategy(ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider) :
            base(criteriaBuilder, clusterProvider)
        {
            IsResetOrderBy = true;
        }

        protected override IEnumerable<GeoObjectBase> DoSearch(IQueryable<IGeoObject> query, Type entityType, SearchParameters searchParameters)
        {
            if (!searchParameters.ViewBounds.HasValue)
            {
                return EmptyResult;
            }

            query = CriteriaBuilder.BuildBoundsWhereClause(query, searchParameters.ViewBounds.Value,
                    searchParameters.ViewBoundsMode);

            var bounds = SpatialHelper.CreatePolygonFromGeoBounds(searchParameters.ViewBounds.Value);
            query = query.Where(x => x.Location.Disposition.Intersects(bounds));

            return SelectBaseGeoObjects(query, entityType, searchParameters.IsSingle);
        }
    }
}