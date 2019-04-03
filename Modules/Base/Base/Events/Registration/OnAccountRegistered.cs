using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Registration
{
    public class OnAccountRegistered<T> : RegisterEvent<T>, IOnAccountRegistered<T>
        where T : IRegisterResult
    {
        public OnAccountRegistered(IUnitOfWork uow, T result) : base(uow, result)
        {

        }
    }
}
