using Newtonsoft.Json.Linq;

namespace WebApi.Models.Crud
{
    public class PatchModel<T>
    {
        public JObject model { get; set; }
    }
}