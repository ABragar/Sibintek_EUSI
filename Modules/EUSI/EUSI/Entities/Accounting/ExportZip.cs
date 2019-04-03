using Base;
using Base.Attributes;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;

namespace EUSI.Entities.Accounting
{
    public class ExportZip : BaseObject
    {
        [ListView("Дата С")]
        [DetailView("Дата С", Required = true)]
        public DateTime? StartDate { get; set; }

        [ListView("Дата По")]
        [DetailView("Дата По", Required = true)]
        public DateTime? EndDate { get; set; }

        [ListView("Балансовые единицы (коды консолидаций)")]
        [DetailView("Балансовые единицы (коды консолидаций)", Required = true)]
        public virtual ICollection<ExportZipConsolidation> Consolidations { get; set; } = new List<ExportZipConsolidation>();

        /// <summary>
        /// Получает или задает признак включение в выгрузку ранее выгруженных ОС.
        /// </summary>
        [ListView("Включать ранее выгруженные")]
        [DetailView("Включать ранее выгруженные", Required = false)]
        public bool NotIncludeTransferBus { get; set; }
    }

    public class ExportZipConsolidation : EasyCollectionEntry<Consolidation>
    {
    }
}
