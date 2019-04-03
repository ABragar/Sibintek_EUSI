using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.MapObjects;
using Base.Map.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Search
{
    internal class ClusteringSearchStrategy : SearchByBoundsStrategy
    {
        private Bounds? _constraintBounds;

        public ClusteringSearchStrategy(ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider) :
            base(criteriaBuilder, clusterProvider)
        {
        }

        public override IEnumerable<GeoObjectBase> Search(IQueryable query, Type entityType, SearchParameters searchParameters)
        {
            if (!IsClusteringDisabled(searchParameters))
            {
                IsResetOrderBy = true;
                _constraintBounds = FindConstraintBounds(query, searchParameters, entityType);
            }

            return base.Search(query, entityType, searchParameters);
        }

        protected override IEnumerable<GeoObjectBase> DoSearch(IQueryable<IGeoObject> query, Type entityType, SearchParameters searchParameters)
        {
            if (IsClusteringDisabled(searchParameters))
            {
                return base.DoSearch(query, entityType, searchParameters);
            }

            if (!searchParameters.Zoom.HasValue || (!searchParameters.ViewBounds.HasValue && !searchParameters.UseCacheBoundsAsView))
            {
                return EmptyResult;
            }

            var viewBounds = GetViewBounds(searchParameters);

            if (viewBounds.Equals(Bounds.Empty))
            {
                return EmptyResult;
            }

            var grid = CreateGrid(searchParameters, entityType, _constraintBounds);
            var clusters = grid.GetClusters(query, viewBounds, entityType);

            if (!searchParameters.FetchNonClusteredObjects)
            {
                return clusters;
            }

            var nonClusteredObjects = grid.GetNonClusteredObjects(query, viewBounds, entityType);
            return clusters.Concat(nonClusteredObjects);
        }

        private bool IsClusteringDisabled(SearchParameters searchParameters)
        {
            return searchParameters.Zoom.HasValue &&
                searchParameters.DisableClusteringAtZoom.HasValue &&
                searchParameters.Zoom.Value >= searchParameters.DisableClusteringAtZoom.Value;
        }

        private Bounds GetViewBounds(SearchParameters searchParameters)
        {
            var result = Bounds.Empty;

            if (searchParameters.UseCacheBoundsAsView && _constraintBounds.HasValue)
            {
                result = _constraintBounds.Value;
            }
            else if (searchParameters.ViewBounds.HasValue)
            {
                result = CRS.Current.Project(searchParameters.ViewBounds.Value);
            }

            return result;
        }

        private IClusterGrid CreateGrid(SearchParameters searchParameters, Type entityType, Bounds? constraintBounds)
        {
            var isCaching = searchParameters.CacheEnabled && searchParameters.CachingOptions != null;

            if (!searchParameters.FilterEnabled && isCaching)
            {
                return ClusterProvider.CreateGrid(searchParameters.Zoom ?? 0, constraintBounds,
                    new CacheSettings(searchParameters.CachingOptions.GroupKey,
                        searchParameters.CachingOptions.GroupTitle,
                        entityType));


            }

            return ClusterProvider.CreateGrid(searchParameters.Zoom ?? 0);
        }

        private Bounds? FindConstraintBounds(IQueryable query, SearchParameters searchParameters, Type entityType)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (searchParameters == null)
            {
                throw new ArgumentNullException(nameof(searchParameters));
            }

            if (!searchParameters.CacheEnabled || searchParameters.CachingOptions == null)
            {
                return null;
            }

            Bounds? result = null;

            if (searchParameters.CachingOptions.AutoCacheBounds)
            {
                var resQuery = BuildBaseCriteria((IQueryable<IGeoObject>)BuildBaseCriteria(query, searchParameters, true), searchParameters);

                result = ClusterProvider.GetObjectBounds(resQuery, new CacheSettings(
                    searchParameters.CachingOptions.GroupKey,
                    searchParameters.CachingOptions.GroupTitle,
                    entityType
                    ));
            }
            else if (searchParameters.CachingOptions.CacheBounds.HasValue &&
                !searchParameters.CachingOptions.CacheBounds.Equals(GeoBounds.Empty))
            {
                var constraintBounds = CRS.Current.Project(searchParameters.CachingOptions.CacheBounds.Value);

                if (!constraintBounds.Equals(Bounds.Empty))
                {
                    result = constraintBounds;
                }
            }

            return result;
        }
    }
}