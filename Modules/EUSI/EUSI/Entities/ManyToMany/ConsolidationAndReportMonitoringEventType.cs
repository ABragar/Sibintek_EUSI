using Base;
using EUSI.Entities.NSI;
using CorpProp.Entities.NSI;
using System.ComponentModel;
using Base.Attributes;

namespace EUSI.Entities.ManyToMany
{
    public class ConsolidationAndReportMonitoringEventType: ManyToManyAssociation<Consolidation, ReportMonitoringEventType>
    {

        public ConsolidationAndReportMonitoringEventType() : base()
        {
        }

        /// <summary>
        /// Получает число планового выполнения события
        /// </summary>
        [ListView("Число планового выполнения события", Visible = true)]
        [DetailView("Число планового выполнения события", Visible = true)]
        [DefaultValue(25)]
        public int? PlanDay { get; set; }




    }
 
}
