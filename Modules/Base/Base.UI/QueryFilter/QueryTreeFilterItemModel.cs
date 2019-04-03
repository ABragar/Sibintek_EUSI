using System;
using System.Collections.Generic;

namespace Base.UI.QueryFilter
{
    public class QueryTreeFilterItemModel
    {
        public QueryTreeFilterItemModel()
        {
            Plugins = new List<string>();
        }

        public string Id { get; set; }

        public string Label { get; set; }

        public string PropertyName { get; set; }

        public ICollection<string> Plugins { get; set; }


        public Dictionary<string, QueryTreeOperator> Operators { get; set; }

        public string SystemData { get; set; }

        public string PrimitiveType { get; set; }

        public Type Type { get; set; }

        public bool IsRequired { get; set; }
    }
}