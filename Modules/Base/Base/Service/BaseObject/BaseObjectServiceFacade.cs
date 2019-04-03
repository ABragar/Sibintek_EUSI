using Base.DAL;
using Base.Events;
using Base.Helpers;

namespace Base.Service
{
    public class BaseObjectServiceFacade: IBaseObjectServiceFacade
    {
        public BaseObjectServiceFacade(IUnitOfWorkFactory unitOfWorkFactory, IEventBus eventBus, IAccessService accessService)
        {
           UnitOfWorkFactory = unitOfWorkFactory;
            EventBus = eventBus;
            AccessService = accessService;
        }

        public IAccessService AccessService { get; }
        public IUnitOfWorkFactory UnitOfWorkFactory { get; }
        public IEventBus EventBus { get; }
    }
}
