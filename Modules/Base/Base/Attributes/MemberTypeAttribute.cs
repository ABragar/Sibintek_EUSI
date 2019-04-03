using System;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class MemberTypeAttribute : Attribute
    {
        public MemberTypeAttribute(string type_property, string object_property)
        {
            TypeProperty = type_property;
            ObjectProperty = object_property;
        }

        public string TypeProperty { get; }

        public string ObjectProperty { get; }
    }
}