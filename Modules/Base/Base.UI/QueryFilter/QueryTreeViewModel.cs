using System.Collections.Generic;

namespace Base.UI.QueryFilter
{
    public class QueryTreeViewModel
    {
        public string Title { get; set; }

        public IEnumerable<QueryTreeItemViewModel> Items { get; set; }
    }
}