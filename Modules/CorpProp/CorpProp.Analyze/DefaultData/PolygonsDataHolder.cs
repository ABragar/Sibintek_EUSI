using System.Collections.Generic;
using System.Xml.Serialization;
using CorpProp.DefaultData;
using CorpProp.Entities.ProjectActivity;

namespace CorpProp.Analyze.DefaultData
{
    [DataHolder(@"CorpProp.Analyze.DefaultData.XML.PolygonsDataHolder.xml")]
    public class PolygonsDataHolder
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса PolygonsDataHolder.
        /// </summary>
        public PolygonsDataHolder()
        {
        }

        /// <summary>
        /// Получает или задает дефолтные полигоны регионов РФ.
        /// </summary>
        [XmlArray("SibRegionPolygons")]
        [XmlArrayItem("SibRegionPolygon")]
        public List<SibRegionPolygon> SibRegionPolygons { get; set; }

        /// <summary>
        /// Получает или задает дефолтные полигоны ФО РФ.
        /// </summary>
        [XmlArray("SibFederalDistrictPolygons")]
        [XmlArrayItem("SibFederalDistrictPolygon")]
        public List<SibFederalDistrictPolygon> SibFederalDistrictPolygons { get; set; }

    }
}