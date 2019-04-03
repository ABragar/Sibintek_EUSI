using Newtonsoft.Json;
using System.Data.Entity.Spatial;

namespace Base.Map.MapObjects
{
    public class GeoObjectBase
    {
        public long ID { get; set; }

        public DbGeography Geometry { get; set; }

        public GeoObjectType Type { get; set; }

        #region Internal Properties

        [JsonIgnore]
        internal int X { get; set; }

        [JsonIgnore]
        internal int Y { get; set; }

        #endregion Internal Properties
    }
}