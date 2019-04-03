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
    /// Представляет справочник "Срок уплаты авансовых платежей и налога (Имущество)"
    /// </summary>
    [EnableFullTextSearch]
    public class TermOfPymentTaxRate : DictObject
    {
        /// <summary>
        /// Получает или задает ИД TaxName.
        /// </summary>
        public int? TaxNameID { get; set; }

        /// <summary>
        /// Получает или задает Имя налога.
        /// </summary>
        [DetailView(Name = "Имя налога", ReadOnly = true)]
        [ListView]
        public TaxName TaxName { get; set; }

        /// <summary>
        /// Получает или задает ID Налоговый период .
        /// </summary>
        public int? TaxPeriodID { get; set; }

        /// <summary>
        /// Получает или задает Налоговый период.
        /// </summary>
        [DetailView(Name = "Налоговый период", ReadOnly = true)]
        [ListView]
        public TaxPeriod TaxPeriod { get; set; }

        /// <summary>
        /// Получает или задает ID Регион.
        /// </summary>
        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает Регион.
        /// </summary>
        [DetailView(Name = "Регион", ReadOnly = true)]
        [ListView]
        public SibRegion SibRegion { get; set; }

        /// <summary>
        /// Получает или задает ID Реквизиты решения органов МО Имущество.
        /// </summary>
        public int? DecisionsDetailsID { get; set; }

        /// <summary>
        /// Получает или задает Реквизиты решения органов МО Имущество.
        /// </summary>
        [DetailView(Name = "Реквизиты решения органов МО Имущество", ReadOnly = true)]
        [ListView]
        public DecisionsDetails DecisionsDetails { get; set; }


        public TermOfPymentTaxRate() : base()
        {

        }

    }
}
