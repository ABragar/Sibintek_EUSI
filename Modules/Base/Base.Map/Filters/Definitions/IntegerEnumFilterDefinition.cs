using System;
using System.Collections;
using System.Linq;

namespace Base.Map.Filters.Definitions
{
    internal class IntegerEnumFilterDefinition : EnumFilterDefinition
    {
        protected override IList ParseValue(FilterValue value)
        {
            return (from val in value
                    where !String.IsNullOrWhiteSpace(val)
                    let number = FilterHelper.ConvertStringToInteger(val)
                    where number.HasValue
                    select number.Value).ToList();
        }
    }
}