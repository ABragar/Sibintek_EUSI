using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelConfigAttribute: Attribute
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string LookupProperty { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
