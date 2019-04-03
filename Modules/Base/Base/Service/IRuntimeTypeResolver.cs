using System;

namespace WebUI.Converters
{
    public interface IRuntimeTypeResolver
    {
        Type GetType(string name);
    }
}