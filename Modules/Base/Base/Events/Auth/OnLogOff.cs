using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Auth
{
    public class OnLogOff<T> : AuthEvent<T>, IOnLogOff<T>
        where T : IAuthResult
    {
        public OnLogOff(IUnitOfWork uow, T authResult) 
            : base(uow, authResult)
        {

        }
    }
}
