using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Map.MapObjects;

namespace Base.Map.Services
{
    public interface IMapObjectCrudService
    {

        IReadOnlyCollection<GeoObject> UpdateGeoObjects(string layerId, IReadOnlyCollection<GeoObject> geoObjects);

        IReadOnlyCollection<GeoObject> DeleteGeoObjects(string layerId, IReadOnlyCollection<GeoObject> geoObjects);

    }
}