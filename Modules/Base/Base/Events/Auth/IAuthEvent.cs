using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Auth
{
    public interface IAuthEvent<T> : IEvent
        where T : IAuthResult
    {
        IUnitOfWork UnitOfWork { get; }
        T AuthResult { get; }
    }
}
