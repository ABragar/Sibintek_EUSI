using System;
using Base.Entities;
using Base.Utils.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WebUI.Converters
{
    public class RuntimeTypeConverter : JsonConverter
    {


        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (!typeof(IRuntimeBindingType).IsAssignableFrom(objectType))
                throw new InvalidOperationException();

            var obj = serializer.Deserialize<JObject>(reader);

            var type_name = obj[nameof(IRuntimeBindingType.RuntimeType)]?.Value<string>();

            if (string.IsNullOrEmpty(type_name))
                throw new InvalidOperationException();

            var type = Type.GetType(type_name);

            if (type == null || !objectType.IsAssignableFrom(type))
                throw new InvalidOperationException();


            var new_obj = Activator.CreateInstance(type);

            using (var r = obj.CreateReader())
            {
                serializer.Populate(r, new_obj);
            }

            return new_obj;

        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IRuntimeBindingType).IsAssignableFrom(objectType);
        }
    }
}