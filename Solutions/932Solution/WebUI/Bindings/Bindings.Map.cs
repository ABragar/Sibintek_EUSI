using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.Filters;
using Base.Map.Services;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class MapBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Map.Initializer>();
            container.Register<IMapServiceFacade, MapServiceFacade>();
            container.Register<IMapSearchService, MapSearchService>();
            container.Register<IMapCacheService, MapCacheService>();
            container.Register<IMapLayerService, MapLayerService>();

            container.Register<IMapLayerConfigService, MapLayerConfigService>();

            container.Register<IMapObjectService, MapObjectService>();
            container.Register<IMapObjectCrudService,MapObjectCrudService>();
            container.Register<IMapService, MapService>();

            container.Register<ICriteriaBuilder, CriteriaBuilder>();
            container.Register<IFilterManager, FilterManager>();
            container.Register<IClusterProvider, QuadClusterProvider>();
            container.Register<IClusterCacheManager, QuadClusterCacheManager>();


        }
    }
}