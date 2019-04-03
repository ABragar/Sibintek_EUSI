using System;
using System.Reflection;
using Base.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebUI.Converters
{
    public class MemberTypeConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var attribute = objectType.GetCustomAttribute<MemberTypeAttribute>();

            if (attribute == null)
                throw new InvalidOperationException();


            var obj = JObject.ReadFrom(reader);

            var type = obj[attribute.TypeProperty]?.Value<string>();

            var property = obj[attribute.ObjectProperty]?.Value<JToken>();

            obj[attribute.ObjectProperty] = null;

            var result = existingValue ?? Activator.CreateInstance(objectType);

            using (var r = obj.CreateReader())
                serializer.Populate(r, result);

            if (type != null)
            {
                var property_type = Type.GetType(type);

                using (var r = property.CreateReader())
                {
                    var property_value = serializer.Deserialize(r, property_type);
                    var prop_info = objectType.GetProperty(attribute.ObjectProperty);
                    prop_info.SetValue(result, property_value);
                }
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            var attribute = objectType.GetCustomAttribute<MemberTypeAttribute>();
            return attribute != null;
        }
    }
}