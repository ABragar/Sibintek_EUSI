using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Filters
{
    public interface IFilterManager
    {
        IReadOnlyCollection<FilterDefinition> GetFilterDefinitions(string mnemonic);

        IQueryable BuildFilterWhereClause(IQueryable query, string mnemonic, FilterValues filters);
    }
}