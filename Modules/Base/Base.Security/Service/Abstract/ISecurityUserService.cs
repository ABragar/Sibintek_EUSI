using Base.Events;
using Base.Security.Entities.Concrete;
using Base.Service;

namespace Base.Security.Service
{
    public interface ISecurityUserService : IService,
        IEventBusHandler<IChangeObjectEvent<SimpleProfile>>,
        IEventBusHandler<IChangeObjectEvent<User>>,
        IEventBusHandler<IChangeObjectEvent<UserCategory>>,
        IEventBusHandler<IChangeObjectEvent<Role>>

    {
        ISecurityUser FindSecurityUser(int id,string login);
        void ClearAll();
        void Clear(int id);
        void Clear(ISecurityUser securityUser);
    }
}
