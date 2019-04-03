using Base;
using Base.Attributes;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{
    public class ExportMoving : BaseObject
    {
        [ListView("Дата С")]
        [DetailView("Дата С", Required = true)]
        public DateTime? StartDate { get; set; }

        [ListView("Дата По")]
        [DetailView("Дата По", Required = true)]
        public DateTime? EndDate { get; set; }

        [ListView("Балансовые единицы (коды консолидаций)")]
        [DetailView("Балансовые единицы (коды консолидаций)", Required = true)]
        public virtual ICollection<ExportMovingConsolidation> Consolidations { get; set; } = new List<ExportMovingConsolidation>();


    }

    public class ExportMovingConsolidation : EasyCollectionEntry<Consolidation>
    {
    }
}
