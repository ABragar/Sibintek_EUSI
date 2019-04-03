using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using EUSI.Entities.NSI;

namespace EUSI.Entities.ManyToMany
{
    /// <summary>
    /// Представляет настройки событий-предшественников для каждого отдельного типа события монитора контролей.
    /// </summary>
    /// <remarks>
    /// Слева - текущая контрольная процедура, Справа - её предшественники: 
    /// те КП,которые долдны быть выполнены перед выполнением текущей.
    /// </remarks>
    public class MonitorEventPreceding : ManyToManyAssociation<ReportMonitoringEventType, ReportMonitoringEventType>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса MonitorEventPreceding.
        /// </summary>
        public MonitorEventPreceding() : base()
        {

        }

    }
}
