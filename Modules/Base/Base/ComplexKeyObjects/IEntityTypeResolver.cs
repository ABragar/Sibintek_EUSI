using System;
using System.Collections.Generic;

namespace Base.ComplexKeyObjects
{
    public interface IEntityTypeResolver
    {
        IEnumerable<Type> GetEntityTypes();
    }
}