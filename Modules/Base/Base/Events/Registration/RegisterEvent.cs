using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Registration
{
    public abstract class RegisterEvent<T> : BaseEvent, IRegisterEvent<T>
        where T : IRegisterResult
    {
        public RegisterEvent(IUnitOfWork uow, T result)
        {
            UnitOfWork = uow;
            Result = result;
        }

        public IUnitOfWork UnitOfWork { get; private set; }
        public T Result { get; private set; }
    }
}
