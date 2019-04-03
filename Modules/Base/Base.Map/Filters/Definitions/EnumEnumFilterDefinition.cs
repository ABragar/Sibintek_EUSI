using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Filters.Definitions
{
    internal class EnumEnumFilterDefinition : EnumFilterDefinition
    {
        private readonly Type _enumType;

        public EnumEnumFilterDefinition(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            _enumType = enumType;
        }

        protected override IList ParseValue(FilterValue value)
        {
            var listType = typeof(List<>).MakeGenericType(_enumType);
            var values = (IList)Activator.CreateInstance(listType);

            var enumValues = value.Where(val => !String.IsNullOrWhiteSpace(val))
                    .Select(val => FilterHelper.ConvertStringToEnum(_enumType, val))
                    .Where(enumValue => enumValue != null);

            foreach (var enumValue in enumValues)
            {
                values.Add(enumValue);
            }

            return values;
        }
    }
}