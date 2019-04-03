using Base.Attributes;
using Base.EntityFrameworkTypes.Complex;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    [EnableFullTextSearch]
    public class SibCityNSI : DictObject
    {
        public SibCityNSI()
        {
        }

        [DetailView("Местоположение", TabName = CaptionHelper.DefaultTabName, Order = 1, Required = true, Visible = false)]
        [ListView(Hidden = true)]
        [PropertyDataType(PropertyDataType.LocationPolygon)]
        public Location Location { get; set; } = new Location();

        /// <summary>
        /// Получает или задает ИД страны.
        /// </summary>
        [SystemProperty]
        public int? CountryID { get; set; }

        /// <summary>
        /// Получает или задает Федеральный округ.
        /// </summary>
        [DetailView(Name = "Страна", ReadOnly = true)]
        [ListView(Hidden = true)]
        public SibCountry Country { get; set; }

        /// <summary>
        /// Получает или задает ИД федерального округа.
        /// </summary>    
        [SystemProperty]
        public int? FederalDistrictID { get; set; }

        /// <summary>
        /// Получает или задает Федеральный округ.
        /// </summary>
        [DetailView(Name = "Федеральный округ", ReadOnly = true)]
        [ListView(Hidden = true)]
        public SibFederalDistrict FederalDistrict { get; set; }

        /// <summary>
        /// Получает или задает ИД региона.
        /// </summary>
        [SystemProperty]
        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает Федеральный округ.
        /// </summary>
        [DetailView(Name = "Регион", ReadOnly = true)]
        [ListView(Hidden = true)]
        public SibRegion SibRegion { get; set; }
    
    }
}
