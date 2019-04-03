using System;

namespace Base.Access.Service
{
    public interface IAccessErrorDescriber
    {
        string GetAccessDenied(Type type);
    }
}
