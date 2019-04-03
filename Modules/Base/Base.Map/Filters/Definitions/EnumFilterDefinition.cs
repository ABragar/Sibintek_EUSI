using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Base.Map.Filters.Definitions
{
    internal abstract class EnumFilterDefinition : FilterDefinition
    {
        private const int _maxValueCounts = 1000;
        private IList _values = null;

        protected EnumFilterDefinition()
        {
            Type = FilterType.Enum;
        }

        public IEnumerable<OptionDefinition> Options { get; set; } = Enumerable.Empty<OptionDefinition>();

        public override FilterDefinition SetValue(FilterValue value)
        {
            if (value == null || value.Count == 0 || value.Count > _maxValueCounts)
            {
                return this;
            }

            _values = ParseValue(value);
            return this;
        }

        protected abstract IList ParseValue(FilterValue value);

        public override IQueryable BuildWhereClause(IQueryable query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (_values != null && _values.Count > 0 && _values.Count <= _maxValueCounts)
            {
                if (_values.Count == 1)
                {
                    query = query.Where($"{Field} == @0", _values[0]);
                }
                else
                {
                    query = query.Where($"@0.Contains({Field})", _values);
                }
            }

            return query;
        }
    }
}