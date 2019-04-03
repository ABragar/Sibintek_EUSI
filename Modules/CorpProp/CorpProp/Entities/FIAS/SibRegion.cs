using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.EntityFrameworkTypes.Complex;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;

namespace CorpProp.Entities.FIAS
{
    /// <summary>
    /// Представляет справочник регионов.
    /// </summary>
    [EnableFullTextSearch]
    public class SibRegion : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibRegion.
        /// </summary>
        public SibRegion()
        {

        }
        [ListView(Hidden = true)]
        [DetailView(Name = "Субъект РФ", Visible = false)]
        public string Title { get; set; }


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
    }
}
