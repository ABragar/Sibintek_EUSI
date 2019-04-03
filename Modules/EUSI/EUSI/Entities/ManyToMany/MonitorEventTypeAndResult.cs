using Base;
using Base.Attributes;
using EUSI.Entities.NSI;
using System.ComponentModel;

namespace EUSI.Entities.ManyToMany
{
    /// <summary>
    /// Представляет настроечную таблицу связей между сравочником типов событий журнала контроля и справочником "Результат выполнения контрольных процедур".
    /// </summary>
    public class MonitorEventTypeAndResult : ManyToManyAssociation<ReportMonitoringEventType, ReportMonitoringResult>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса MonitorResultAndMonitorEvent.
        /// </summary>
        public MonitorEventTypeAndResult() : base()
        {

        }        

        /// <summary>
        /// Получает или задает признак доступности ручного выбора.
        /// </summary>        
        [DetailView("Доступен для ручного выбора", Visible = false), ListView(Visible = false)]
        [DefaultValue(false)]
        public bool IsManualPick { get; set; }
    }
}
