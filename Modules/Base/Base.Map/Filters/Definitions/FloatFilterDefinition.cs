using System;
using System.Globalization;
using System.Threading;

namespace Base.Map.Filters.Definitions
{
    internal class FloatFilterDefinition : NumericFilterDefinition<float>
    {
        protected override float? ParseValue(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            float parsedValue;

            if (float.TryParse(value, NumberStyles.Number, Thread.CurrentThread.CurrentCulture, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }
    }
}