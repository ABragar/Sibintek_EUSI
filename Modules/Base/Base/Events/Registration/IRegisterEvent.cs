using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Registration
{
    public interface IRegisterEvent<T> : IEvent
        where T : IRegisterResult
    {
        IUnitOfWork UnitOfWork { get; }
        T Result { get; }
    }
}
