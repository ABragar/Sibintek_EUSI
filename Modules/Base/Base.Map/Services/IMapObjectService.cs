using Base.Map.Filters;
using Base.Map.MapObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.Map.Services
{
    public interface IMapObjectService
    {
        IEnumerable<GeoObjectBase> GetGeoObjects(string layerId, double[] bbox = null, int? zoom = null, FilterValues filters = null, bool? single = null, string searchString = null);

        PagingResult<GeoObjectBase> GetGeoObjectsByCluster(string layerId, long clusterId, int zoom, int page, int pageSize, string searchString = null);

        PagingResult<GeoObjectBase> GetPagingGeoObjects(string layerId, int page, int pageSize, FilterValues filters = null, string searchString = null);

        Task<int> GetGeoObjectCountAsync(string layerId, FilterValues filters = null);

    }
}