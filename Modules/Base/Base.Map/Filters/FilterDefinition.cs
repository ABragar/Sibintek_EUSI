using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Base.Map.Filters
{
    public abstract class FilterDefinition
    {
        public string Title { get; set; }

        public string Field { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FilterType Type { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FilterUIType UIType { get; set; }

        public abstract FilterDefinition SetValue(FilterValue value);

        public abstract IQueryable BuildWhereClause(IQueryable query);
    }
}