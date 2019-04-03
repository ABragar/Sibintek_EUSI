using System;
using Newtonsoft.Json.Converters;

namespace Base.WebApi.Converters
{
    public class StringEnumConverter<T> : StringEnumConverter
        where T : struct
    {
        public override bool CanConvert(Type objectType)
        {
            return (typeof(T) == objectType || typeof(T?) == objectType) && base.CanConvert(objectType);
        }
    }
}