using Newtonsoft.Json;

namespace Base.Map.MapObjects
{
    public class GeoObject : GeoObjectBase
    {
        public string Title { get; set; }

        public string Description { get; set; }

        #region Internal Properties

        [JsonIgnore]
        internal double PX { get; set; }

        [JsonIgnore]
        internal double PY { get; set; }

        #endregion Internal Properties
    }
}