using Base.DAL;
using Base.Events;

namespace Base.Service
{
    public interface IBaseObjectServiceFacade
    {
        IAccessService AccessService { get; }
        IUnitOfWorkFactory UnitOfWorkFactory { get; }
        IEventBus EventBus { get; }
    }
}
