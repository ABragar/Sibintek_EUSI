using System.Collections.Generic;

namespace Base.Map.MapObjects
{
    public class DetailViewTab
    {
        public string Title { get; set; }

        public Dictionary<string, DetailViewProperty> Properties { get; set; }
    }
}