using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace WebUI.Helpers
{
    public class JsonNetResult : ActionResult
    {
        public const string DATE_TIME_FORMATE = "dd.MM.yyyy HH:mm:ss";
        public const string DATE_FORMATE = "dd.MM.yyyy";
        public const string MONTH_FORMATE = "MMMM yyyy";
        public const string YEAR_FORMATE = "yyyy";
        public const string TIME_FORMATE = "HH:mm";
        //public const int DepthLimit = 4;
        private readonly object _jObject;
        private readonly JsonConverter[] _converters;

        public JsonNetResult(object jObject)
        {
            _jObject = jObject;
        }

        public JsonNetResult(object jObject, params JsonConverter[] converters)
        {
            _jObject = jObject;

            _converters = converters;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";


            var serializer = GlobalConfiguration.Configuration.Formatters.JsonFormatter.CreateJsonSerializer();

            if (_converters != null)
                serializer.Converters.AddRange(_converters);

            //response.Write(SerializeObject(_jObject, DepthLimit));

            try
            {
                serializer.Serialize(response.Output, _jObject);
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public override string ToString()
        {

            var serializer = GlobalConfiguration.Configuration.Formatters.JsonFormatter.CreateJsonSerializer();

            serializer.Converters.AddRange(_converters);

            // return SerializeObject(_jObject, DepthLimit);

            using (var w = new StringWriter())
            {
                serializer.Serialize(w, _jObject);
                return w.ToString();
            }

        }

        //private static string SerializeObject(object obj, int maxDepth)
        //{
        //    using (var strWriter = new StringWriter())
        //    {
        //        using (var jsonWriter = new CustomJsonTextWriter(strWriter))
        //        {
        //            Func<bool> include = () => jsonWriter.CurrentDepth <= maxDepth;
        //            var resolver = new CustomContractResolver(include);
        //            var serializer = new JsonSerializer
        //            {
        //                ContractResolver = resolver,
        //                DateFormatString = DATE_TIME_FORMATE,
        //                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        //                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
        //            };
        //            serializer.Serialize(jsonWriter, obj);
        //        }
        //        return strWriter.ToString();
        //    }
        //}
    }


    public class CustomJsonTextWriter : JsonTextWriter
    {
        public CustomJsonTextWriter(TextWriter textWriter) : base(textWriter) { }
        public int CurrentDepth { get; private set; }
        public override void WriteStartObject()
        {
            CurrentDepth++;
            base.WriteStartObject();
        }
        public override void WriteEndObject()
        {
            CurrentDepth--;
            base.WriteEndObject();
        }
    }
    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly Func<bool> _includeProperty;
        public CustomContractResolver(Func<bool> includeProperty)
        {
            _includeProperty = includeProperty;
        }
        protected override JsonProperty CreateProperty(
            MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var shouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize = obj => _includeProperty() &&
                                              (shouldSerialize == null ||
                                               shouldSerialize(obj));
            return property;
        }
    }
}