using Base.Map.Filters;
using Base.Map.MapObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.Map.Services
{
    public interface IMapService
    {
        IEnumerable<MapLayer> GetTreeLayers(params string[] mnemonics);

        IEnumerable<MapLayer> GetPublicTreeLayers();

        PagingResult<MapService.FullSearchGeoResult> FullTextSearchInLayers(string searchStr,
            int paze,
            int page_size,
            params string[] mnemonics);

        IEnumerable<GeoObjectBase> GetGeoObjects(string layerId, double[] point = null, double[] bbox = null, int? zoom = null, FilterValues filters = null, bool? single = null, string searchString = null);

        PagingResult<GeoObjectBase> GetGeoObjectsByCluster(string layerId, long clusterId, int zoom, int page, int pageSize, string searchString = null);

        PagingResult<GeoObjectBase> GetPagingGeoObjects(string layerId, int page, int pageSize, FilterValues filters = null, string searchString = null);

        IReadOnlyCollection<GeoObject> UpdateGeoObjects(string layerId, IReadOnlyCollection<GeoObject> geoObjects);

        IReadOnlyCollection<GeoObject> DeleteGeoObjects(string layerId, IReadOnlyCollection<GeoObject> geoObjects);

        Task<int> GetGeoObjectCountAsync(string layerId, FilterValues filters = null);

        Task<Dictionary<string, int>> GetGeoObjectsCountAsync(string[] layerId);


        IEnumerable<FilterDefinition> GetFilters(string layerId);
    }
}