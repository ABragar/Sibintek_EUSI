using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Auth
{
    public interface IOnLogOff<T> : IAuthEvent<T>
        where T : IAuthResult
    {
    }
}
