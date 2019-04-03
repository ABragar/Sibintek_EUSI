using System.Collections.Generic;

namespace Base.Map.MapObjects
{
    public class DetailViewExtendedProperty : DetailViewProperty
    {
        public DetailViewExtendedProperty()
        {
            Type = "Extended";
        }

        public string Extended { get; set; }

        public Dictionary<string, DetailViewProperty> Properties { get; set; }
    }
}