using System;
using System.Reflection;
using Base.Attributes;

namespace Base.Extensions
{
    public static class EnumExtensions
    {
        public static string GetTitle(this Enum value)
        {
            var attr = GetUiEnumValue(value);

            return attr?.Title ?? value.ToString();
        }

        public static string GetColor(this Enum value)
        {
            var attr = GetUiEnumValue(value);

            return attr?.Color ?? value.ToString();
        }

        public static string GetIcon(this Enum value)
        {
            var attr = GetUiEnumValue(value);

            return attr?.Icon ?? value.ToString();
        }

        private static UiEnumValueAttribute GetUiEnumValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            return fieldInfo.GetCustomAttribute<UiEnumValueAttribute>();
        }

        public static int GetValue(this Enum value)
        {
            return (int)((object)value);
        }
    }
}
