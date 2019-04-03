using System;
using System.Collections.Generic;

namespace Base.UI.ViewModal
{
    [Serializable]
    public class CompositeFilter
    {
        public FilterObjectType FilterType { get; set; }

        public List<CompositeFilter> Filters { get; set; }
        public string Logic { get; set; }

        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }

    public enum FilterObjectType
    {
        SingleObject = 0,
        CompositeObject = 1
    }
}