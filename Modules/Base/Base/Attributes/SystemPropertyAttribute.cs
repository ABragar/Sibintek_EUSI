using System;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class SystemPropertyAttribute : Attribute
    {
    }
}
