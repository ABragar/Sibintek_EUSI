using System.Collections.Generic;

namespace Base.UI.QueryFilter
{
    public class QueryTreeItemViewModel
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public IEnumerable<string> Plugins { get; set; }

        public string PrimitiveType { get; set; }

        public IEnumerable<string> Operators { get; set; }
        public string SystemData { get; set; }
    }
}