using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public interface IAutoMapperCloner
    {
        IBaseObject Copy(IBaseObject source);
        IBaseObject Copy(IBaseObject source, IBaseObject destination);
        TDestination Copy<TSource, TDestination>(TSource source);
    }
}
