using System;
using System.Collections.Generic;

namespace Base.UI.QueryFilter
{
    public class QueryTreeFilterModel
    {
        public string Title { get; set; }
        public IDictionary<string, QueryTreeFilterItemModel> Items { get; set; }
        public string Mnemonic { get; set; }
        public Type Type { get; set; }
    }
}