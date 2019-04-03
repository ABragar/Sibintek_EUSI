using Base.Map.Filters;
using Base.Map.MapObjects;
using System.Collections.Generic;

namespace Base.Map.Services
{
    public interface IMapLayerService
    {
        IReadOnlyCollection<MapLayer> GetLayers(params string[] layerIds);

        IReadOnlyCollection<MapLayer> GetPublicLayers();

        IMapLayerConfig GetLayerConfig(string layerId);

        IReadOnlyCollection<FilterDefinition> GetFilters(string layerId);
    }
}