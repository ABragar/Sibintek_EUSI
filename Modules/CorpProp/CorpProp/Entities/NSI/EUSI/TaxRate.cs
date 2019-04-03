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
    /// Представляет справочник "Налоговая ставка Имущество".
    /// </summary>
    [EnableFullTextSearch]
    public class TaxRate : DictObject
    {
        /// <summary>
        /// Получает или задает ИД TaxName.
        /// </summary>
        public int? TaxNameID { get; set; }

        /// <summary>
        /// Получает или задает Имя налога.
        /// </summary>
        [DetailView(Name = "Имя налога", ReadOnly = true)]
        [ListView(Hidden = true)]
        public TaxName TaxName { get; set; }

        /// <summary>
        /// Получает или задает ID Налоговый период .
        /// </summary>
        public int? TaxPeriodID { get; set; }

        /// <summary>
        /// Получает или задает Налоговый период.
        /// </summary>
        [DetailView(Name = "Налоговый период", ReadOnly = true)]
        [ListView(Hidden = true)]
        public TaxPeriod TaxPeriod { get; set; }

        /// <summary>
        /// Получает или задает ID Регион.
        /// </summary>
        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает Регион.
        /// </summary>
        [DetailView(Name = "Регион", ReadOnly = true)]
        [ListView(Hidden = true)]
        public SibRegion SibRegion { get; set; }

        /// <summary>
        /// Получает или задает ID Реквизиты решения органов МО Имущество.
        /// </summary>
        public int? DecisionsDetailsID { get; set; }

        /// <summary>
        /// Получает или задает Реквизиты решения органов МО Имущество.
        /// </summary>
        [DetailView(Name = "Реквизиты решения органов МО Имущество", ReadOnly = true)]
        [ListView(Hidden = true)]
        public DecisionsDetails DecisionsDetails { get; set; }

        /// <summary>
        /// Получает или задает Категория налогоплательщика и (или) имущества.
        /// </summary>
        [DetailView(Name = "Категория налогоплательщика и (или) имущества")]
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
