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
    /// Представляет справочник "Налоговая ставка ТС".
    /// </summary>
    [EnableFullTextSearch]
    public class TaxRateTS : DictObject
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
        /// Получает или задает ID Реквизиты решения органов МО ТС.
        /// </summary>
        public int? DecisionsDetailsTSID { get; set; }

        /// <summary>
        /// Получает или задает Реквизиты решения органов МО ТС.
        /// </summary>
        [DetailView(Name = "Реквизиты решения органов МО ТС", ReadOnly = true)]
        [ListView]
        public DecisionsDetailsTS DecisionsDetailsTS { get; set; }

        /// <summary>
        /// Получает или задает Наименование объекта налогообложения.
        /// </summary>
        [DetailView(Name = "Наименование объекта налогообложения", ReadOnly = true)]
        [ListView]
        public string TaxObjectName { get; set; }

        /// <summary>
        /// Значение Минимальное значение мощности.
        /// </summary>
        [ListView(Name = "Минимальное значение мощности")]
        [DetailView(Name = "Минимальное значение мощности")]
        public decimal? MinPower { get; set; }

        /// <summary>
        /// Максимальное значение мощности
        /// </summary>
        [ListView(Name = "Максимальное значение мощности")]
        [DetailView(Name = "Максимальное значение мощности")]
        public decimal? MaxPower { get; set; }

        /// <summary>
        /// Размер, руб.
        /// </summary>
        [ListView(Name = "Размер, руб.")]
        [DetailView(Name = "Размер, руб.")]
        public decimal? Value { get; set; }
    }
}
