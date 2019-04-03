using Base.Map.Filters.Definitions;
using Base.Map.Helpers;
using System;

namespace Base.Map.Filters
{
    internal static class FilterDefinitionFactory
    {
        public static FilterDefinition createNumeric(Type numericType, object minValue, object maxValue)
        {
            if (CheckType.IsValueType<int>(numericType))
            {
                return new IntegerFilterDefinition
                {
                    MaxValue = maxValue != null ? Convert.ToInt32(maxValue) : 0,
                    MinValue = minValue != null ? Convert.ToInt32(minValue) : 0
                };
            }
            else if (CheckType.IsValueType<float>(numericType))
            {
                return new FloatFilterDefinition
                {
                    MaxValue = maxValue != null ? Convert.ToSingle(maxValue) : 0,
                    MinValue = minValue != null ? Convert.ToSingle(minValue) : 0
                };
            }
            else if (CheckType.IsValueType<double>(numericType))
            {
                return new DoubleFilterDefinition
                {
                    MaxValue = maxValue != null ? Convert.ToDouble(maxValue) : 0,
                    MinValue = minValue != null ? Convert.ToDouble(minValue) : 0
                };
            }
            else if (CheckType.IsValueType<decimal>(numericType))
            {
                return new DecimalFilterDefinition
                {
                    MaxValue = maxValue != null ? Convert.ToDecimal(maxValue) : 0,
                    MinValue = minValue != null ? Convert.ToDecimal(minValue) : 0
                };
            }
            else
            {
                throw new FilterDefinitionNotFoundException(
                    $"Couldn't found filter definition for type: {numericType.ToString()}");
            }
        }
    }
}