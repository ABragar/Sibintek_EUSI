using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Base.Map.Filters.Definitions
{
    internal abstract class NumericFilterDefinition<TValue> : FilterDefinition
        where TValue : struct
    {
        private TValue? _startValue;
        private TValue? _endValue;

        protected NumericFilterDefinition()
        {
            Type = FilterType.Numeric;
        }

        public TValue MinValue { get; set; }

        public TValue MaxValue { get; set; }

        public int? Step { get; set; } = null;

        public override FilterDefinition SetValue(FilterValue value)
        {
            if (value == null || value.Count == 0)
            {
                return this;
            }

            if (value.Count == 1)
            {
                _startValue = ParseValue(value.First());
            }
            else if (value.Count == 2)
            {
                _startValue = ParseValue(value[0]);
                _endValue = ParseValue(value[1]);
            }

            return this;
        }

        protected abstract TValue? ParseValue(string value);

        public override IQueryable BuildWhereClause(IQueryable query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (_startValue.HasValue &&
                _endValue.HasValue &&
                _startValue.Value.Equals(_endValue.Value))
            {
                query = query.Where($"{Field} = @0", _startValue.Value);
            }
            else
            {
                if (_startValue.HasValue)
                {
                    query = query.Where($"{Field} >= @0", _startValue.Value);
                }

                if (_endValue.HasValue)
                {
                    query = query.Where($"{Field} <= @0", _endValue.Value);
                }
            }

            return query;
        }
    }
}