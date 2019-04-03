using Base.Audit.Entities;
using Base.DAL;
using Base.Service;
using System;
using System.Threading.Tasks;
using Base.Events;
using Base.Events.Auth;

namespace Base.Audit.Services
{
    public interface IAuditItemService : IBaseObjectService<AuditItem>, IReadOnly,
        IEventBusHandler<IOnCreate<BaseObject>>,
        IEventBusHandler<IOnDelete<BaseObject>>,
        IEventBusHandler<IOnUpdate<BaseObject>>,
        IEventBusHandler<IOnLogOn<IAuthResult>>,
        IEventBusHandler<IOnLogOff<IAuthResult>>,
        IEventBusHandler<IOnLogOnError<IAuthResult>>
    {
    }
}
