using System.Collections.Generic;

namespace Base.UI.QueryFilter
{
    public interface IQueryTreeService
    {
        QueryTreeFilterModel GetFilters(string mnemonic);

        IEnumerable<QueryTreeItemViewModel> GetAggregatableProperties(string mnemonic);
    }
}