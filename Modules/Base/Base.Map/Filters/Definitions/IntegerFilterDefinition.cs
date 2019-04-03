using System;
using System.Globalization;
using System.Threading;

namespace Base.Map.Filters.Definitions
{
    internal class IntegerFilterDefinition : NumericFilterDefinition<int>
    {
        protected override int? ParseValue(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            int parsedValue;

            if (int.TryParse(value, NumberStyles.Number, Thread.CurrentThread.CurrentCulture, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }
    }
}