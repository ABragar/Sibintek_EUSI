using System;
using System.Dynamic;

namespace CorpProp.Analyze.DefaultData
{
    [Serializable]
    public class SibRegionPolygon
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Polygon { get; set; }
    }

    [Serializable]
    public class SibFederalDistrictPolygon
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Polygon { get; set; }
    }
}