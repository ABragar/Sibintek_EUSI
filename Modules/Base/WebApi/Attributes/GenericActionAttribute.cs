using System;

namespace WebApi.Attributes
{
    public class GenericActionAttribute : Attribute
    {
        public string Name { get; }

        public GenericActionAttribute(string name)
        {
            Name = name;
        }
    }
}