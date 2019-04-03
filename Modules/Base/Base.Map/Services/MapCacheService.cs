using Base.Events;
using Base.Map.Clustering;
using Base.Map.Helpers;
using Base.Map.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Services
{
    public class MapCacheService : IMapCacheService
    {
        private readonly IClusterCacheManager _cacheManager;
        private readonly IMapServiceFacade _serviceFacade;
        private readonly IMapSearchService _searchService;
        private readonly IMapLayerConfigService _config_service;
        private readonly IMapLayerService _layer_service;

        public MapCacheService(
            IClusterCacheManager cacheManager,
            IMapServiceFacade serviceFacade,
            IMapSearchService searchService,
            IMapLayerConfigService config_service,
            IMapLayerService layer_service)
        {
            _cacheManager = cacheManager;
            _serviceFacade = serviceFacade;
            _searchService = searchService;
            _config_service = config_service;
            _layer_service = layer_service;
        }

        #region Manage Cache

        public CacheInfo GetCacheInfo(string groupKey)
        {
            CheckGroupKey(groupKey);
            return _cacheManager.GetCacheInfo(groupKey);
        }

        public IEnumerable<CacheInfo> GetAllCacheInfo()
        {
            return (from cacheGroup in GetCacheGroups()
                    let info = GetCacheInfo(cacheGroup.Key)
                    where info != null
                    select info).ToList();
        }

        public IEnumerable<CacheGroup> GetCacheGroups()
        {
            return _cacheManager.GetCacheGroups();
        }

        public void EnableCache(string groupKey)
        {
            EnableCacheInternal(groupKey, true);
        }

        public void DisableCache(string groupKey)
        {
            EnableCacheInternal(groupKey, false);
        }

        private void EnableCacheInternal(string groupKey, bool flag)
        {
            if (string.IsNullOrEmpty(groupKey))
            {
                throw new ArgumentNullException(nameof(groupKey));
            }

            using (var unitOfWork = _serviceFacade.UnitOfWorkFactory.Create())
            {

                var layer = _config_service.GetByLayerId(unitOfWork, groupKey);

                
                layer.EnableCache = flag;
                layer.AutoCacheBounds = flag;
                _config_service.Update(unitOfWork, layer);
            }
        }

        public int UpdateCache(string groupKey, int? level = null)
        {
            if (string.IsNullOrEmpty(groupKey))
            {
                throw new ArgumentNullException(nameof(groupKey));
            }

            var layer = _layer_service.GetLayerConfig(groupKey);

            if (!layer.CacheEnabled)
            {
                throw new InvalidOperationException($"Cache is disabled for group key: {groupKey}");
            }

            if (level.HasValue && (level.Value < 0 || level.Value >= layer.DisableClusteringAtZoom))
            {
                throw new ArgumentOutOfRangeException(nameof(level), $"Level must be in the range from 0 to {layer.DisableClusteringAtZoom}.");
            }

            var searchParameters = new SearchParameters(layer.Mnemonic)
            {
                //BaseObjectTypeId = layer.ID,
                ClusteringEnabled = true,
                UseCacheBoundsAsView = true,
                CacheEnabled = true,
                CachingOptions = new SearchParameters.CacheOptions
                (
                    groupKey,
                    layer.Name,
                    layer.CacheBounds,
                    layer.AutoCacheBounds
                )
            };

            var updatedItems = 0;

            if (level.HasValue)
            {
                searchParameters.Zoom = level;
                var result = _searchService.Search(searchParameters);
                updatedItems += result.Count();
            }

            else
            {
                _cacheManager.ClearCache(groupKey);

                for (var i = 0; i < layer.DisableClusteringAtZoom; i++)
                {
                    searchParameters.Zoom = i;
                    var result = _searchService.Search(searchParameters);
                    updatedItems += result.Count();
                }
            }

            _cacheManager.ResetStats();
            return updatedItems;
        }

        public void ClearCache(string groupKey)
        {
            CheckGroupKey(groupKey);
            _cacheManager.ClearCache(groupKey);
        }

        public void ClearAllCache()
        {
            _cacheManager.ClearAllCache();
        }

        public CacheStats GetCacheStats()
        {
            return _cacheManager.GetCacheStats();
        }

        public void ResetCacheStats()
        {
            _cacheManager.ResetStats();
        }

        private void CheckGroupKey(string groupKey)
        {
            if (string.IsNullOrEmpty(groupKey))
            {
                throw new ArgumentNullException(nameof(groupKey));
            }

            if (!_cacheManager.HasCacheGroup(groupKey))
            {
                throw new ArgumentException($"Group key not found: {groupKey}", nameof(groupKey));
            }
        }

        #endregion Manage Cache








    }

}