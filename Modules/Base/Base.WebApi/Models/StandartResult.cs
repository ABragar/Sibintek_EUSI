using Newtonsoft.Json;

namespace Base.WebApi.Models
{
    public class StandartResult
    {
        [JsonProperty("details")]
        public string Details { get; set; }


        [JsonProperty("extended", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Extended { get; set; }
    }

}