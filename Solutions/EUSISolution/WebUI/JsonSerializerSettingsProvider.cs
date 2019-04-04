using System.Net.Http.Formatting;
using Base.WebApi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Converters;
using WebUI.Helpers;

namespace WebUI
{
    public class JsonSettingsProvider : IJsonSettingsProvider
    {


        public void ApplySettings(JsonSerializerSettings settings)
        {

            settings.ContractResolver = new DefaultContractResolver()
            {
                IgnoreSerializableAttribute = true,
                IgnoreSerializableInterface = true,
            };

            settings.DateFormatString = "dd.MM.yyyy HH:mm:ss";
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

            settings.Converters.Add(new DbGeographyGeoJsonConverter());

            settings.Converters.Add(new RuntimeTypeConverter());
            settings.Converters.Add(new MemberTypeConverter());
            settings.Converters.Add(new IntConverter());
            settings.TypeNameHandling = TypeNameHandling.None;
        }
    }
}