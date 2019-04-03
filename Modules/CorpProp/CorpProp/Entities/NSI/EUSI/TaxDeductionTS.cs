using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using Base.Attributes;
using CorpProp.Helpers;
using CorpProp.Entities.FIAS;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник "Налоговые вычеты (ТС)"
    /// </summary>
    [EnableFullTextSearch]
    public class TaxDeductionTS : DictObject
    {
        public TaxDeductionTS() : base()
        {

        }

        /// <summary>
        /// Получает или задает ИД Наименование налога.
        /// </summary>
        public int? TaxNameID { get; set; }

        /// <summary>
        /// Получает или задает Наименование налога.
        /// </summary>
        [ListView("Наименование налога")]
        [DetailView("Наименование налога")]
        public TaxName TaxName { get; set; }


        /// <summary>
        /// Получает или задает ИД Регион.
        /// </summary>
        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает Регион.
        /// </summary>
        [ListView("Регион")]
        [DetailView("Регион")]
        public SibRegion SibRegion { get; set; }


        /// <summary>
        /// Получает или задает ИД Код налоговой льготы ТС.
        /// </summary>
        public int? TaxExemptionTSID { get; set; }

        /// <summary>
        /// Получает или задает Код налоговой льготы ТС.
        /// </summary>
        [ListView("Код налоговой льготы ТС")]
        [DetailView("Код налоговой льготы ТС")]
        public TaxExemptionTS TaxExemptionTS { get; set; }




        [DetailView(Name = "№ Статьи закона РФ")]
        [ListView]
        public string NumArticleLawRF { get; set; }

        [DetailView(Name = "№ Пункта закона РФ")]
        [ListView]
        public string NumParagraphLawRF { get; set; }

        [DetailView(Name = "№ Подпункта статьи закона РФ")]
        [ListView]
        public string NumSubParagraphLawRF { get; set; }

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
