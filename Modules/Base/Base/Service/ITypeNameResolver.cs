using System;

namespace Base.Service
{
    public interface ITypeNameResolver
    {
        string GetName(Type type);
    }

}