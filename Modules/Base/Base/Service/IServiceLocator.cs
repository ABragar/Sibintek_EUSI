using System;
using System.Collections.Generic;

namespace Base.Service
{

    public interface IServiceLocator
    {
        T GetService<T>() where T: class;

        object GetService(Type type);

        [Obsolete]
        IEnumerable<object> GetServices(Type type);
    }
}