using Base.Entities;
using Base.Events;
using Base.Events.Auth;
using Base.Events.Registration;

namespace Base.Audit.Services
{
    public interface IAuditItemAuthService : IReadOnly,
        IEventBusHandler<IOnLogOn<IAuthResult>>,
        IEventBusHandler<IOnLogOff<IAuthResult>>,
        IEventBusHandler<IOnLogOnError<IAuthResult>>,
        IEventBusHandler<IOnAccountRegistered<IRegisterResult>>
    {
    }
}