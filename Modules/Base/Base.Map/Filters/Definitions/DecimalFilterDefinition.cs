using System.Globalization;
using System.Threading;

namespace Base.Map.Filters.Definitions
{
    internal class DecimalFilterDefinition : NumericFilterDefinition<decimal>
    {
        protected override decimal? ParseValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            decimal parsedValue;

            if (decimal.TryParse(value, NumberStyles.Number, Thread.CurrentThread.CurrentCulture, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }
    }
}