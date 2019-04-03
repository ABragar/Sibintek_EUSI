using System.Globalization;
using System.Threading;

namespace Base.Map.Filters.Definitions
{
    internal class DoubleFilterDefinition : NumericFilterDefinition<double>
    {
        protected override double? ParseValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            double parsedValue;

            if (double.TryParse(value, NumberStyles.Number, Thread.CurrentThread.CurrentCulture, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }
    }
}