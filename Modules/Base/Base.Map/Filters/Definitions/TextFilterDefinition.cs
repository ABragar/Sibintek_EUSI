using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Base.Map.Filters.Definitions
{
    internal class TextFilterDefinition : FilterDefinition
    {
        private string _value;

        public TextFilterDefinition()
        {
            Type = FilterType.Text;
        }

        public override FilterDefinition SetValue(FilterValue value)
        {
            if (value == null || value.Count == 0)
            {
                return this;
            }

            _value = value.First();
            return this;
        }

        public override IQueryable BuildWhereClause(IQueryable query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (!string.IsNullOrWhiteSpace(_value))
            {
                query = query.Where($"{Field}.Contains(@0)", $"{_value}");
            }

            return query;
        }
    }
}