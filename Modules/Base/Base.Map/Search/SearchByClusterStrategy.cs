using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Search
{
    internal class SearchByClusterStrategy : SearchByBoundsStrategy
    {
        public SearchByClusterStrategy(ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider) :
            base(criteriaBuilder, clusterProvider)
        {
        }

        protected override IEnumerable<GeoObjectBase> DoSearch(IQueryable<IGeoObject> query, Type entityType, SearchParameters searchParameters)
        {
            if (!searchParameters.Zoom.HasValue)
            {
                return EmptyResult;
            }

            var page = searchParameters.Page ?? 0;
            var pageSize = searchParameters.PageSize ?? 0;

            var clusterGrid = ClusterProvider.CreateGrid(searchParameters.Zoom.Value);
            return clusterGrid.GetObjects(query, searchParameters.ClusterId, page, pageSize, entityType);
        }
    }
}