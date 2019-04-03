using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Auth
{
    public class OnLogOn<T> : AuthEvent<T>, IOnLogOn<T>
        where T : IAuthResult
    {
        public OnLogOn(IUnitOfWork uow, T authResult) 
            : base(uow, authResult)
        {

        }
    }
}
