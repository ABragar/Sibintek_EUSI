using System;
using Newtonsoft.Json;

namespace WebUI.Helpers
{
    public class IntConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int res = 0;
            int.TryParse(reader.Value.ToString(), out res);
            return res;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }
    }
}