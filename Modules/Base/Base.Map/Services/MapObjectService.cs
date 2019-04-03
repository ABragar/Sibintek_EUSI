using Base.Extensions;
using Base.Map.Criteria;
using Base.Map.Filters;
using Base.Map.Helpers;
using Base.Map.MapObjects;
using Base.Map.Search;
using Base.Map.Spatial;
using Base.UI.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Service;
using Base.UI;
using Base.UI.Extensions;

namespace Base.Map.Services
{
    public class MapObjectService : IMapObjectService
    {
        private readonly IMapSearchService _searchService;
        private readonly IMapLayerService _layerService;
        private readonly IMapServiceFacade _serviceFacade;
        private readonly ICriteriaBuilder _criteriaBuilder;
        private readonly IViewModelConfigService _configService;

        public MapObjectService(IMapSearchService searchService, IMapLayerService layerService,
            IMapServiceFacade serviceFacade, ICriteriaBuilder criteriaBuilder, IViewModelConfigService configService)
        {

            _searchService = searchService;
            _layerService = layerService;
            _serviceFacade = serviceFacade;
            _criteriaBuilder = criteriaBuilder;
            _configService = configService;
        }

        public IEnumerable<GeoObjectBase> GetGeoObjects(string layerId, double[] bbox = null, int? zoom = null, FilterValues filters = null,
            bool? single = null, string searchString = null)
        {
            var layerConfig = GetLayerConfig(layerId);

            var searchParameters = new SearchParameters(layerConfig.Mnemonic)
            {
                Zoom = zoom,
                Filters = filters,
                IsSingle = single ?? false,
                LocationNotNull = true,
                LayerID = layerId,
                //  BaseObjectTypeId = layerConfig.BoTypeId,
                SearchString = searchString,
            };

            if (layerConfig.Mode == MapLayerMode.Server)
            {
                searchParameters.LocationNotNull = false;
                searchParameters.MinSearchZoom = layerConfig.MinSearchZoom;
                searchParameters.MaxSearchZoom = layerConfig.MaxSearchZoom;
                searchParameters.DisableClusteringAtZoom = layerConfig.DisableClusteringAtZoom;
                searchParameters.FetchNonClusteredObjects = layerConfig.DisplayNonClusteredObjects;
                searchParameters.SearchOnClickEnabled = layerConfig.SearchOnClick;
                searchParameters.ClusteringEnabled = layerConfig.ServerClustering;
                searchParameters.CacheEnabled = layerConfig.CacheEnabled;
                searchParameters.CachingOptions = new SearchParameters.CacheOptions(
                    layerId,
                    layerConfig.Name,
                    layerConfig.CacheBounds,
                    layerConfig.AutoCacheBounds);

                searchParameters.SetViewBounds(bbox, BoundsMode.Intersects);
            }

            return _searchService.Search(searchParameters);
        }

        public PagingResult<GeoObjectBase> GetGeoObjectsByCluster(string layerId, long clusterId, int zoom, int page, int pageSize,
            string searchString = null)
        {
            var layerConfig = _layerService.GetLayerConfig(layerId);

            var searchParameters = new SearchParameters(layerConfig.Mnemonic)
            {
                ClusterId = clusterId,
                Zoom = zoom,
                Page = page,
                PageSize = pageSize,
                LayerID = layerId,
                //   BaseObjectTypeId = layerConfig.BoTypeId,
                SearchString = searchString
            };

            return _searchService.Search(searchParameters) as PagingResult<GeoObjectBase>;
        }

        public PagingResult<GeoObjectBase> GetPagingGeoObjects(string layerId, int page, int pageSize, FilterValues filters = null,
            string searchString = null)
        {
            var layerConfig = _layerService.GetLayerConfig(layerId);

            var searchParameters = new SearchParameters(layerConfig.Mnemonic)
            {
                Filters = filters,
                Page = page,
                PageSize = pageSize,
                LocationNotNull = true,
                LayerID = layerId,
                //  BaseObjectTypeId = layerConfig.BoTypeId,
                SearchString = searchString
            };

            return _searchService.Search(searchParameters) as PagingResult<GeoObjectBase>;
        }

        public async Task<int> GetGeoObjectCountAsync(string layerId, FilterValues filters = null)
        {
            var layerConfig = _layerService.GetLayerConfig(layerId);

            var service = GetQueryService(layerConfig.Mnemonic);
            var config = _configService.Get(layerConfig.Mnemonic);

            using (var uofw = _serviceFacade.UnitOfWorkFactory.Create())
            {
                IQueryable query = service.GetAll(uofw).ListViewFilter(config.ListView);

                if (filters != null && filters.Any())
                {
                    query = _criteriaBuilder.BuildFilterWhereClause(query, layerConfig.Mnemonic, filters);
                }

                var resQuery = _criteriaBuilder.BuildLocationNotNullWhereClause((IQueryable<IGeoObject>)query);
                return await resQuery.CountAsync();
            }
        }


        #region Helper Methods


        private IQueryService<object> GetQueryService(string mnemonic)
        {
            return GetViewModelConfig(mnemonic).GetService<IQueryService<object>>();
        }

        private ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return ViewModelConfigHelper.GetViewModelConfig(_serviceFacade.ViewModelConfigService, mnemonic);
        }


        private IMapLayerConfig GetLayerConfig(string layerId)
        {
            var layerConfig = _layerService.GetLayerConfig(layerId);

            if (layerConfig == null)
            {
                throw new ArgumentException($"Could not found map layer with id: ${layerId}");
            }

            return layerConfig;
        }
        #endregion Helper Methods
    }
}