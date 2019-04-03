using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.Helpers;
using Base.Map.MapObjects;
using Base.Map.Search;
using Base.Service.Crud;
using System;
using System.Collections.Generic;
using Base.Service;
using Base.UI.Extensions;

namespace Base.Map.Services
{
    public class MapSearchService : IMapSearchService
    {
        private readonly IMapServiceFacade _serviceFacade;
        private readonly ISearchStrategyFactory _strategyFactory;

        public MapSearchService(IMapServiceFacade serviceFacade, ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider)
        {
            if (serviceFacade == null)
            {
                throw new ArgumentNullException(nameof(serviceFacade));
            }

            if (criteriaBuilder == null)
            {
                throw new ArgumentNullException(nameof(criteriaBuilder));
            }

            _serviceFacade = serviceFacade;
            _strategyFactory = new SearchStrategyFactory(criteriaBuilder, clusterProvider);
        }

        public IEnumerable<GeoObjectBase> Search(SearchParameters searchParameters)
        {
            var config = ViewModelConfigHelper.GetViewModelConfig(_serviceFacade.ViewModelConfigService, searchParameters.Mnemonic);
            ViewModelConfigHelper.CheckGeoObjectType(config);

            using (var unitOfWork = _serviceFacade.UnitOfWorkFactory.Create())
            {
                var service = config.GetService<IQueryService<object>>();
                var searchStrategy = _strategyFactory.Create(searchParameters);
                return searchStrategy.Search(service.GetAll(unitOfWork).ListViewFilter(config.ListView), config.TypeEntity, searchParameters);
            }
        }
    }
}