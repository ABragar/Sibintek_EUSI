using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Auth
{
    public class OnLogOnError<T> : AuthEvent<T>, IOnLogOnError<T>
        where T : IAuthResult
    {
        public OnLogOnError(IUnitOfWork uow, T authResult) 
            : base(uow, authResult)
        {

        }
    }
}
