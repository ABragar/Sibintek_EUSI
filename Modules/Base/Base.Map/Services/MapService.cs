using Base.Map.Filters;
using Base.Map.MapObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Base.DAL;
using Base.Extensions;
using Base.Service.Crud;
using Base.UI.ViewModal;
using Base.Utils.Common;
using Base.Utils.Common.Caching;

namespace Base.Map.Services
{


    public class MapService : IMapService
    {

        private readonly IMapObjectService _mapObjectService;
        private readonly IMapLayerService _mapLayerService;
        private readonly IUnitOfWorkFactory _unit_of_work_factory;
        private readonly ISimpleCacheWrapper _cache;
        private readonly IMapObjectCrudService _crud_service;

        public MapService(IMapObjectService mapObjectService, 
            IMapLayerService mapLayerService,
            IUnitOfWorkFactory unit_of_work_factory,
            ISimpleCacheWrapper cache, 
            IMapObjectCrudService crud_service)
        {
            if (mapObjectService == null)
            {
                throw new ArgumentNullException(nameof(mapObjectService));
            }

            if (mapLayerService == null)
            {
                throw new ArgumentNullException(nameof(mapLayerService));
            }

            _mapObjectService = mapObjectService;
            _mapLayerService = mapLayerService;
            _unit_of_work_factory = unit_of_work_factory;
            _cache = cache;
            _crud_service = crud_service;
        }

        public IEnumerable<MapLayer> GetTreeLayers(params string[] layerIds)
        {
            return _mapLayerService.GetLayers(layerIds);
        }

        public IEnumerable<MapLayer> GetPublicTreeLayers()
        {
            return _mapLayerService.GetPublicLayers();
        }

        //TODO: Need to test
        private IReadOnlyCollection<ViewModelConfig> GetSearchConfigs(string[] layerIds)
        {
            var layers = _mapLayerService.GetLayers(layerIds);
            return layers.Where(x => x.Load).Select(x => x.ViewModelConfig).ToList();
        }

        private IEnumerable<MapLayer> WithFirstLevelChild(MapLayer layer)
        {
            yield return layer;

            foreach (var child in layer.Children.SelectMany(WithFirstLevelChild))
            {
                yield return child;
            }
        }

        private static IEnumerable<MapLayer> WithAllChild(MapLayer layer)
        {
            IEnumerable<MapLayer> layers = new MapLayer[] { };

            if (layer.Children.Any())
            {
                layer.Children.ForEach(childLayer =>
                {
                    layers = layers.Concat(WithAllChild(childLayer));
                });
            }
            else
            {
                layers = layers.Concat(new[] { layer });
            }

            return layers;
        }


        public class FullSearchGeoResult
        {
            public string LayerId { get; set; }
            public GeoObject GeoObject { get; set; }
        }


        private class TempSearchResult
        {
            public string Mnemonic { get; set; }
            public string LayerId { get; set; }
            public int ID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DbGeography Geometry { get; set; }
        }


        private IQueryable<TempSearchResult> CreateQueries(IUnitOfWork unit_of_work, string searchStr, string[] layerIds)
        {

            IQueryable<TempSearchResult> result = null;

            var layers = _mapLayerService.GetLayers(layerIds);

            foreach (var layer in layers)
            {
                var config = layer.ViewModelConfig;

                var service = config.GetService<IBaseObjectCrudService>();

                var query = service.GetAll(unit_of_work);

                var search_query = query.FullTextSearch(searchStr, _cache);

                if (search_query == query)
                    continue;

                var q = (IQueryable<IGeoObject>)search_query;

                var searchres =
                    q.Where(x => x.Location.Disposition != null).Select(
                        model =>
                            new TempSearchResult
                            {
                                Mnemonic = config.Mnemonic,
                                LayerId = layer.LayerId,
                                ID = model.ID,
                                Title = model.Title,
                                Description = model.Description,
                                Geometry = model.Location.Disposition,
                            });


                result = result?.Concat(searchres) ?? searchres;

            }

            return result ?? Enumerable.Empty<TempSearchResult>().AsQueryable();
        }

        private static CacheAccessor<object> FFTSCacheAccessor = new CacheAccessor<object>(TimeSpan.FromMinutes(10));

        public PagingResult<FullSearchGeoResult> FullTextSearchInLayers(string searchStr,
            int page,
            int page_size,
            params string[] layerIds)
        {
            //var configs = GetSearchConfigs(mnemonics);

            using (var unit_of_work = _unit_of_work_factory.Create())
            {
                var qq = CreateQueries(unit_of_work, searchStr, layerIds);

                var count = (int)_cache.GetOrAdd(FFTSCacheAccessor, $"{searchStr}:{string.Join(";", layerIds)}", () => qq.Count());

                page_size = page_size > 0 && page_size <= 50 ? page_size : 50;
                page = page > 0 ? page : 1;

                var result = qq.OrderBy(x => x.Title).Skip(page_size * (page - 1)).Take(page_size);

                var xxx = result.AsEnumerable().Select(x => new FullSearchGeoResult()
                {
                    LayerId = x.LayerId,
                    GeoObject = new GeoObject()
                    {
                        ID = x.ID,
                        Title = x.Title,
                        Description = x.Description,
                        Geometry = x.Geometry,
                        Type = GeoObjectType.Object
                    }
                }).ToList();

                return new PagingResult<FullSearchGeoResult>()
                {
                    Items = xxx,
                    TotalCount = count,
                    PageSize = page_size,
                    Page = page
                };
            }
        }


        public IEnumerable<GeoObjectBase> GetGeoObjects(string layerid, double[] point = null, double[] bbox = null, int? zoom = null,
            FilterValues filters = null, bool? single = null, string searchString = null)
        {
            return _mapObjectService.GetGeoObjects(layerid, bbox, zoom, filters, single, searchString);
        }

        public PagingResult<GeoObjectBase> GetGeoObjectsByCluster(string layerid, long clusterId, int zoom, int page, int pageSize,
            string searchString = null)
        {
            return _mapObjectService.GetGeoObjectsByCluster(layerid, clusterId, zoom, page, pageSize, searchString);
        }

        public PagingResult<GeoObjectBase> GetPagingGeoObjects(string layerid, int page, int pageSize, FilterValues filters = null,
            string searchString = null)
        {
            return _mapObjectService.GetPagingGeoObjects(layerid, page, pageSize, filters, searchString);
        }


        public IReadOnlyCollection<GeoObject> UpdateGeoObjects(string layerid, IReadOnlyCollection<GeoObject> geoObjects)
        {
            return _crud_service.UpdateGeoObjects(layerid, geoObjects);
        }

        public IReadOnlyCollection<GeoObject> DeleteGeoObjects(string layerid, IReadOnlyCollection<GeoObject> geoObjects)
        {
            return _crud_service.DeleteGeoObjects(layerid, geoObjects);
        }

        public Task<int> GetGeoObjectCountAsync(string layerid, FilterValues filters = null)
        {
            return _mapObjectService.GetGeoObjectCountAsync(layerid, filters);
        }

        public async Task<Dictionary<string, int>> GetGeoObjectsCountAsync(string[] layers)
        {

            var result = new Dictionary<string, int>();

            foreach (var layerId in layers)
            {
                if (!result.ContainsKey(layerId))
                {
                    result.Add(layerId, await GetGeoObjectCountAsync(layerId));
                }
            }

            return result;

        }

        public IEnumerable<FilterDefinition> GetFilters(string layerid)
        {
            return _mapLayerService.GetFilters(layerid);
        }
    }
}