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
    /// Представляет справочник "Региональные льготы ТС".
    /// </summary>
    [EnableFullTextSearch]
    public class TaxRegionExemptionTS : DictObject
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

        public int? DecisionsDetailsTSID { get; set; }

        [DetailView(Name = "Реквизиты решения органов МО ТС", ReadOnly = true)]
        [ListView]
        public DecisionsDetailsTS DecisionsDetailsTS { get; set; }

        public int? TaxExemptionTSID { get; set; }

        [DetailView(Name = "Код налоговой льготы ТС", ReadOnly = true)]
        [ListView]
        public TaxExemptionTS TaxExemptionTS { get; set; }

        [DetailView(Name = "Категория налогоплательщиков, для которых установлена льгота")]
        [ListView]
        public string TaxpayersCategory { get; set; }

        [DetailView(Name = "№ Статьи закона РФ")]
        [ListView]
        public string NumArticleLawRF { get; set; }

        [DetailView(Name = "№ Пункта закона РФ")]
        [ListView]
        public string NumParagraphLawRF { get; set; }

        [DetailView(Name = "№ Подпункта статьи закона РФ")]
        [ListView]
        public string NumSubParagraphLawRF { get; set; }

        [DetailView(Name = "Код льготы субъекта РФ")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExemptionCodeSubjectRF { get; set; }

        [DetailView(Name = "Основания предоставления льготы")]
        [ListView]
        public string BasisForGranting { get; set; }

        [DetailView(Name = "Размер")]
        [ListView]
        public decimal Value { get; set; }

        [DetailView(Name = "Ед. изм.", ReadOnly = true)]
        [ListView]
        public string Unit { get; set; }


        [DetailView(Name = "Условия предоставления льготы", ReadOnly = true)]
        [ListView]
        public string ConditionForGranting { get; set; }
    }
}
