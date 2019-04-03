using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;

namespace Base.Events.Auth
{
    public abstract class AuthEvent<T> : BaseEvent, IAuthEvent<T>
        where T : IAuthResult
    {
        public AuthEvent(IUnitOfWork uow, T authResult)
        {
            UnitOfWork = uow;
            AuthResult = authResult;
        }

        public IUnitOfWork UnitOfWork { get; private set; }
        public T AuthResult { get; private set; }
    }
}
