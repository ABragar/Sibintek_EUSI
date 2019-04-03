using System;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true)]
    public class IndexAttribute : Attribute
    {
        public int Order { get; }

        public IndexAttribute(string name,int order = 0)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            Name = name;
            Order = order;
        }
        public string Name { get; }
    }
}