using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Base.UI;
using Base.UI.Editors;

namespace EUSI.Extentions
{
    public static class StringExtentions
    {
        public static Decimal GetValue(this string str)
        {
            try
            {
                if (!String.IsNullOrEmpty(str))
                    return Decimal.Parse(str);
            }
            catch { }
            return 0m;
        }

        public static Int32 GetIntValue(this string str)
        {
            try
            {
                if (!String.IsNullOrEmpty(str))
                    return Int32.Parse(str);
            }
            catch { }
            return 0;
        }
    }



    public static class EnumExtentions
    {
        public static string GetEnumAttrValue<TEnum>(this TEnum value) where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum) return null;

            var member = enumType.GetMember(value.ToString()).FirstOrDefault();
            if (member == null) return null;

            var attribute = member.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault();
            if (attribute == null) return null;
            return attribute.Name;
        }
    }
}
