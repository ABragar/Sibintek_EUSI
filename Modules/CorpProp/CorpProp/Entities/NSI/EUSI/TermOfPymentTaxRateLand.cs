using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник "Срок уплаты авансовых платежей и налога (Земля)"
    /// </summary>
    [EnableFullTextSearch]
    public class TermOfPymentTaxRateLand : DictObject
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
        /// Получает или задает ID Реквизиты решения органов МО ЗУ.
        /// </summary>
        public int? DecisionsDetailsLandID { get; set; }

        /// <summary>
        /// Получает или задает Реквизиты решения органов МО ЗУ.
        /// </summary>
        [DetailView(Name = "Реквизиты решения органов МО ЗУ", ReadOnly = true)]
        [ListView]
        public DecisionsDetailsLand DecisionsDetailsLand { get; set; }

        /// <summary>
        /// Получает или задает ИД OKTMO.
        /// </summary>
        [SystemProperty]
        public int? OKTMOID { get; set; }

        /// <summary>
        /// Получает или задает OKTMO.
        /// </summary>
        [DetailView("ОКТМО", ReadOnly = true)]
        public OKTMO OKTMO { get; set; }


        public TermOfPymentTaxRateLand() : base()
        {

        }

    }
}
