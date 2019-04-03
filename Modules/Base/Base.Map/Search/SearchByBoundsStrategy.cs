using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Search
{
    internal class SearchByBoundsStrategy : SearchStrategyBase
    {
        public SearchByBoundsStrategy(ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider) :
            base(criteriaBuilder, clusterProvider)
        {
        }

        protected override IEnumerable<GeoObjectBase> DoSearch(IQueryable<IGeoObject> query, Type entityType, SearchParameters searchParameters)
        {
            if (!searchParameters.ViewBounds.HasValue)
            {
                return EmptyResult;
            }

            query = CriteriaBuilder.BuildBoundsWhereClause(query, searchParameters.ViewBounds.Value,
                searchParameters.ViewBoundsMode);

            return SelectBaseGeoObjects(query, entityType);
        }
    }
}