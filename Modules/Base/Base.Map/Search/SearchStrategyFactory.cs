using Base.Map.Clustering;
using Base.Map.Criteria;
using System;
using System.ComponentModel;

namespace Base.Map.Search
{
    internal class SearchStrategyFactory : ISearchStrategyFactory
    {
        private readonly ICriteriaBuilder _criteriaBuilder;
        private readonly IClusterProvider _clusterProvider;

        public SearchStrategyFactory(ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider)
        {
            if (criteriaBuilder == null)
            {
                throw new ArgumentNullException(nameof(criteriaBuilder));
            }

            if (clusterProvider == null)
            {
                throw new ArgumentNullException(nameof(clusterProvider));
            }

            _criteriaBuilder = criteriaBuilder;
            _clusterProvider = clusterProvider;
        }

        public SearchStrategyBase Create(SearchStrategyType searchStrategyType)
        {
            switch (searchStrategyType)
            {
                case SearchStrategyType.Default:
                    return new DefaultSearchStrategy(_criteriaBuilder, _clusterProvider);

                case SearchStrategyType.SearchOnClick:
                    return new SearchOnClickStrategy(_criteriaBuilder, _clusterProvider);

                case SearchStrategyType.ClusteringSearch:
                    return new ClusteringSearchStrategy(_criteriaBuilder, _clusterProvider);

                case SearchStrategyType.SearchByCluster:
                    return new SearchByClusterStrategy(_criteriaBuilder, _clusterProvider);

                case SearchStrategyType.SearchByBounds:
                    return new SearchByBoundsStrategy(_criteriaBuilder, _clusterProvider);

                default:
                    throw new InvalidEnumArgumentException(nameof(searchStrategyType),
                       (int)searchStrategyType, typeof(SearchStrategyType));
            }
        }

        public SearchStrategyBase Create(SearchParameters searchParameters)
        {
            if (searchParameters.ClusterId > 0)
            {
                return Create(SearchStrategyType.SearchByCluster);
            }

            if (searchParameters.SearchOnClickEnabled)
            {
                return Create(SearchStrategyType.SearchOnClick);
            }

            if (searchParameters.ClusteringEnabled)
            {
                return Create(SearchStrategyType.ClusteringSearch);
            }

            if (searchParameters.ViewBounds.HasValue)
            {
                return Create(SearchStrategyType.SearchByBounds);
            }

            return Create(SearchStrategyType.Default);
        }
    }
}