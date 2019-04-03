using Newtonsoft.Json;

namespace Base.WebApi.Services
{
    public interface IJsonSettingsProvider
    {

        void ApplySettings(JsonSerializerSettings settings);
    }
}