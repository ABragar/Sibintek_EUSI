using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Base.Map.Filters
{
    internal static class FilterHelper
    {
        public static int? ConvertStringToInteger(string value)
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

        public static TEnum? ConvertStringToEnum<TEnum>(string value) where TEnum : struct
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum.");
            }

            TEnum parsedValue;

            if (Enum.TryParse(value, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }

        public static object ConvertStringToEnum(Type enumType, string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            if (enumType == null || !enumType.IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum.");
            }

            try
            {
                var typeConverter = TypeDescriptor.GetConverter(enumType);
                return typeConverter.ConvertFromInvariantString(value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static readonly MethodInfo _castMethod = typeof(FilterHelper).GetMethod(nameof(GenericCastToType),
            BindingFlags.NonPublic | BindingFlags.Static);

        public static IEnumerable CastToType(IEnumerable source, Type type)
        {
            var method = _castMethod.MakeGenericMethod(type);
            return (IEnumerable)method.Invoke(null, new object[] { source });
        }

        private static List<T> GenericCastToType<T>(IEnumerable source)
        {
            return source.Cast<T>().ToList();
        }
    }
}