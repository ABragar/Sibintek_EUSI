using Base.DAL;
using Base.UI;
using Base.UI.Service;
using Base.Utils.Common.Caching;

namespace Base.Map.Services
{
    public class MapServiceFacade : IMapServiceFacade
    {
        public MapServiceFacade(IViewModelConfigService viewModelConfigService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IUiEnumService uiEnumService,
            ISimpleCacheWrapper cacheWrapper)
        {
            ViewModelConfigService = viewModelConfigService;
            UnitOfWorkFactory = unitOfWorkFactory;
            UiEnumService = uiEnumService;
            CacheWrapper = cacheWrapper;
        }

        public IViewModelConfigService ViewModelConfigService { get; }

        public IUnitOfWorkFactory UnitOfWorkFactory { get; }

        public IUiEnumService UiEnumService { get; }

        public ISimpleCacheWrapper CacheWrapper { get; }
    }
}