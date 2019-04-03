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
    /// Представляет справочник "Налоговая ставка ЗУ".
    /// </summary>
    [EnableFullTextSearch]
    public class TaxRateLand : DictObject
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
        /// Получает или задает ИД ОКТМО.
        /// </summary>
        public int? OKTMOID { get; set; }

        /// <summary>
        /// Получает или задает ОКТМО.
        /// </summary>
        [DetailView(Name = "ОКТМО", ReadOnly = true)]
        [ListView]
        public OKTMO OKTMO { get; set; }

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

        [DetailView(Name = "Наименование категории земель/разрешенного использования земельного участка")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Значение ставки в %.
        /// </summary>
        [ListView(Name = "Значение ставки в %")]
        [DetailView(Name = "Значение ставки в %")]
        public decimal? Value { get; set; }
    }
}
