using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Base.Map.Filters.Definitions
{
    internal class BoolFilterDefinition : FilterDefinition
    {
        private bool? _value = null;

        public BoolFilterDefinition()
        {
            Type = FilterType.Bool;
        }

        public override FilterDefinition SetValue(FilterValue value)
        {
            if (value == null || value.Count == 0)
            {
                return this;
            }

            bool parsedValue;
            if (Boolean.TryParse(value.First(), out parsedValue))
            {
                _value = parsedValue;
            }

            return this;
        }

        public override IQueryable BuildWhereClause(IQueryable query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (_value != null)
            {
                query = query.Where($"{Field} == @0", _value);
            }

            return query;
        }
    }
}