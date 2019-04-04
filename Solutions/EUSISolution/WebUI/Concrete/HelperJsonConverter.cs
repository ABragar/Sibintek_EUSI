using Base.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Base;
using Base.UI;
using Newtonsoft.Json.Serialization;
using WebUI.Helpers;

namespace WebUI.Concrete
{
    public class HelperJsonConverter : IHelperJsonConverter
    {
        public JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                Converters = new List<JsonConverter> { new DbGeographyGeoJsonConverter() { } },
                //Binder = new DynamicProxySerializationBinder(),
                SerializationBinder = new DynamicProxySerializationBinder(),
            };
        }

        public string SerializeObject(BaseObject obj, bool completeGraph = false)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, GetSettings());
        }

        public BaseObject DeserializeObject(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, GetSettings()) as BaseObject;
        }
        public T DeserializeObject<T>(string value) where T : BaseObject
        {
            return JsonConvert.DeserializeObject<T>(value, GetSettings());
        }
    }

    class DynamicProxySerializationBinder : DefaultSerializationBinder
    {
        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            var type = serializedType.Namespace == "System.Data.Entity.DynamicProxies"
                ? serializedType.BaseType
                : serializedType;

            base.BindToName(type, out assemblyName, out typeName);
        }
    }
}