using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class UiEnumAttribute: Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class UiEnumValueAttribute : Attribute
    {
        public UiEnumValueAttribute(){}

        public UiEnumValueAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int Value { get; set; }

    }
}
