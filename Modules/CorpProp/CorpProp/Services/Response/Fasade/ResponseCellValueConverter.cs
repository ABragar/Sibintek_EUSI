using System;
using System.ComponentModel;

namespace CorpProp.Services.Response.Fasade
{
    public static class ResponseCellValueConverter
    {
        public static TReturn Converter<TReturn, TReturnBase>(string s)
        {
            if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
            {
                TypeConverter conv = TypeDescriptor.GetConverter(typeof(TReturnBase));
                var converted = conv.ConvertFrom(s);
                if (converted == null)
                    throw new NotSupportedException($"Нет механизма преобразование строки к типу ${typeof(TReturn).Name}");
                var result = (TReturn)converted;
                return result;
            }
            return default(TReturn);
        }

        public static T NonNullableConverter<T>(string s)
        {
            return Converter<T,T>(s);
        }

        public static T? NulableConverter<T>(string s)
            where T : struct
        {
            return Converter<T?, T>(s);
        }
    }
}
