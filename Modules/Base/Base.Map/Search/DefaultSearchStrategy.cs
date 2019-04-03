using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Search
{
    internal class DefaultSearchStrategy : SearchStrategyBase
    {
        public DefaultSearchStrategy(ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider) :
            base(criteriaBuilder, clusterProvider)
        {
        }

        protected override IEnumerable<GeoObjectBase> DoSearch(IQueryable<IGeoObject> query, Type entityType, SearchParameters searchParameters)
        {
            return SelectBaseGeoObjects(query, entityType, searchParameters.IsSingle, searchParameters.Page, searchParameters.PageSize);
        }
    }
}