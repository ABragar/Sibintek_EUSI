using System;
using System.ComponentModel.DataAnnotations;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true)]
    public class UniqueIndexAttribute : IndexAttribute
    {
        public UniqueIndexAttribute(string name,int order = 0) : base(name,order)
        {

        }
    }
}