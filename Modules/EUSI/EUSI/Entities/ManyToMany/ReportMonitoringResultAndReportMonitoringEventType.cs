using Base;
using Base.Attributes;
using CorpProp.Entities.Base;
using EUSI.Entities.NSI;

namespace EUSI.Entities.ManyToMany
{
    /// <summary>
    /// Связывает сравочник типов событий журнала контроля и справочник "Результат выполнения контрольных процедур"
    /// </summary>
    public class ReportMonitoringResultAndReportMonitoringEventType : TypeObject
    //ManyToManyAssociation<ReportMonitoringResult, ReportMonitoringEventType>
    {
        public ReportMonitoringResultAndReportMonitoringEventType() : base()
        {
        }

        [SystemProperty]
        public int ObjLeftId { get; set; }

        [DetailView("ReportMonitoringResult")]
        [ListView("ReportMonitoringResult")]
        public ReportMonitoringResult ObjLeft { get; set; }

        [SystemProperty]
        public int ObjRigthId { get; set; }

        [DetailView("ReportMonitoringEventType")]
        [ListView("ReportMonitoringEventType")]
        public ReportMonitoringEventType ObjRigth { get; set; }

        /// <summary>
        /// Доступен ли для ручного выбора
        /// </summary>
        [ListView("Ручной выбор", Visible = true)]
        [DetailView("Ручной выбор", Visible = true)]
        public bool IsManualPick { get; set; }
    }
}