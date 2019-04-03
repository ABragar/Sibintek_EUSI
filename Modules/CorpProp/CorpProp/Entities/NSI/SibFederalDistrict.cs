using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

using Base.Attributes;
using Base.EntityFrameworkTypes.Complex;
using CorpProp.Entities.FIAS;
using CorpProp.Helpers;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник федеральных округов.
    /// </summary>
    [EnableFullTextSearch]
    public class SibFederalDistrict : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса FederalDistrict.
        /// </summary>
        public SibFederalDistrict()
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
    }
}
