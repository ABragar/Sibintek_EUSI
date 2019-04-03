using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Registration
{
    public interface IOnAccountRegistered<T> : IRegisterEvent<T>
        where T : IRegisterResult
    {

    }
}
