using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник "Федеральные льготы ЗУ".
    /// </summary>
    [EnableFullTextSearch]
    public class TaxFederalExemptionLand : DictObject
    {
        public int? TaxNameID { get; set; }

        [DetailView(Name = "Имя налога", ReadOnly = true)]
        [ListView]
        public TaxName TaxName { get; set; }

        public int? TaxPeriodID { get; set; }

        [DetailView(Name = "Налоговый период", ReadOnly = true)]
        [ListView]
        public TaxPeriod TaxPeriod { get; set; }

        public int? SibRegionID { get; set; }

        [DetailView(Name = "Регион", ReadOnly = true)]
        [ListView]
        public SibRegion SibRegion { get; set; }

        public int? TaxExemptionLandID { get; set; }

        [DetailView(Name = "Код налоговой льготы ЗУ", ReadOnly = true)]
        [ListView]
        public TaxExemptionLand TaxExemptionLand { get; set; }

        [DetailView(Name = "Категория налогоплательщиков, для которых установлена льгота")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxpayersCategory { get; set; }

        [DetailView(Name = "№ Глава закона РФ")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumChapterLawRF { get; set; }

        [DetailView(Name = "№ Статьи закона РФ")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumArticleLawRF { get; set; }

        [DetailView(Name = "№ Пункта закона РФ")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumParagraphLawRF { get; set; }

        [DetailView(Name = "Основания предоставления льготы")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string BasisForGranting { get; set; }

        [DetailView(Name = "Размер")]
        [ListView]
        public decimal? Value { get; set; }

        [DetailView(Name = "Ед. изм.", ReadOnly = true)]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Unit { get; set; }


        [DetailView(Name = "Условия предоставления льготы", ReadOnly = true)]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string ConditionForGranting { get; set; }
    }
}
