using Base.DAL;
using Base.UI;
using Base.UI.Service;
using Base.Utils.Common.Caching;

namespace Base.Map.Services
{
    public interface IMapServiceFacade
    {        
        IUnitOfWorkFactory UnitOfWorkFactory { get; }
        IViewModelConfigService ViewModelConfigService { get; }
        IUiEnumService UiEnumService { get; }
        ISimpleCacheWrapper CacheWrapper { get; }
    }
}