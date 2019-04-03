using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Helpers
{
    public static class CheckType
    {
        public static bool IsGeoObject(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return typeof(IGeoObject).IsAssignableFrom(type);
        }

        public static bool IsIconGeoObject(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return typeof(IIconGeoObject).IsAssignableFrom(type);
        }

        public static bool IsValueType<T>(Type type)
            where T : struct
        {
            return typeof(T) == type || typeof(T?) == type;
        }

        public static bool IsNumeric(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return IsValueType<int>(type) ||
                IsValueType<float>(type) ||
                IsValueType<double>(type) ||
                IsValueType<decimal>(type);
        }

        public static bool IsNullableEnum(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var u = Nullable.GetUnderlyingType(type);
            return (u != null) && u.IsEnum;
        }

        public static bool IsEnum(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsEnum || IsNullableEnum(type);
        }

        public static bool IsEnumerable(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetInterfaces().Any(x =>
                (x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)) || x == typeof(IEnumerable));
        }

        public static bool IsCollection(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetInterfaces().Any(x =>
                (x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)) || x == typeof(ICollection));
        }
    }
}