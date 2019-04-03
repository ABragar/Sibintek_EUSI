using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using EUSI.Entities.NSI;
using System;
using System.ComponentModel;

namespace EUSI.Entities.Report
{
    /// <summary>
    /// Журнал контроля выполнения отчетов.
    /// </summary>
    public class ReportMonitoring : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ReportMonitoring.
        /// </summary>
        public ReportMonitoring() : base()
        {

        }

        /// <summary>
        /// Получает или задает дату/время выполнения.
        /// </summary>             
        [ListView]
        [DetailView("Дата/Время")]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? ImportDateTime { get; set; }
               
        
        [ListView]
        [DetailView("Вид контрольной процедуры")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReportName { get; set; }

        [ListView(Visible = false)]
        [DetailView("Код отчета")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReportCode { get; set; }

        /// <summary>
        /// Получает или задает мнемонику контролируемой сущности.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView("Мнемоника")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Получает или задает ИД профиля пользователя.
        /// </summary>
        [SystemProperty]
        public int? SibUserID { get; set; }

        /// <summary>
        /// Получает или задает профиль пользователя.
        /// </summary>
        [ListView]
        [DetailView("Пользователь")]
        public SibUser SibUser { get; set; }

        [SystemProperty]
        public int? ConsolidationID { get; set; }

        [ListView]
        [DetailView("БЕ")]
        public Consolidation Consolidation { get; set; }
        
        [ListView]
        [DetailView("Начало периода")]
        public DateTime? StartDate { get; set; }

        [ListView]
        [DetailView("Окончание периода")]
        public DateTime? EndDate { get; set; }

        [ListView]
        [DetailView("Контроль пройден")]
        [SystemProperty]
        [DefaultValue(false)]
        public bool IsValid { get; set; }

        /// <summary>
        /// Получает или задает ИД результата выполнения контрольных процедур
        /// </summary>
        [SystemProperty]
        public int? ReportMonitoringResultID { get; set; }

        /// <summary>
        /// Получает или задает результата выполнения контрольных процедур
        /// </summary>
        [ListView]
        [DetailView("Результат")]
        public virtual ReportMonitoringResult ReportMonitoringResult { get; set; }

        /// <summary>
        /// Получает или задает атрибут "Комментарий".
        /// </summary>
        [DetailView(Name = "Комментарий")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comment { get; set; }

        /// <summary>
        /// Получает или задает индекс итерации события
        /// </summary>
        [DetailView("Индекс итерации события", Visible = false)]
        [ListView("Индекс итерации события", Visible = true)]
        public int? IterationIndex { get; set; }

        /// <summary>
        /// Получает или задает ИД типа события журнала контроля
        /// </summary>
        [DetailView(Visible = false)]
        public int? ReportMonitoringEventTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип события журнала контроля
        /// </summary>
        [ListView]
        [DetailView("Тип события журнала контроля")]
        public virtual ReportMonitoringEventType ReportMonitoringEventType { get; set; }


    }
}
