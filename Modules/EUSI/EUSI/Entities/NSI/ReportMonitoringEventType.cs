using Base.Attributes;
using Base.UI.ViewModal;
using CorpProp.Entities.Base;
using CorpProp.Helpers;

namespace EUSI.Entities.NSI
{
    /// <summary>
    /// Представляет типы событий журнала контроля.
    /// </summary>
    public class ReportMonitoringEventType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ReportMonitoringEventType.
        /// </summary>
        public ReportMonitoringEventType() : base()
        {
        }

        /// <summary>
        /// Получает или задает Учет в мониторе закрытия.
        /// </summary>
        [DetailView(Name = "Учет в мониторе закрытия", Visible = true)]
        [ListView(Name = "Учет в мониторе закрытия", Visible = true)]
        public bool UseAdjournmentMonitor { get; set; }

        /// <summary>
        /// Получает или задает Вид события журнала выполнения контролей.
        /// </summary>
        [DetailView(Name = "Вид события журнала выполнения контролей", Visible = true)]
        [ListView(Name = "Вид события журнала выполнения контролей", Visible = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReportMonitoringEventKind { get; set; }

        /// <summary>
        /// Получает или задает Число месяца (план выполнения события данного типа).
        /// </summary>
        [DetailView(Name = "Число месяца (план выполнения события данного типа)", Visible = true)]
        [ListView(Name = "Число число месяца (план выполнения события данного типа)", Visible = false)]
        public int? PlanDayOfMonth { get; set; }

        /// <summary>
        /// Получает или задает Примечание.
        /// </summary>
        [DetailView(Name = "Примечание", Visible = true)]
        [ListView(Name = "Примечание", Visible = true)]
        public string Note { get; set; }

        /// <summary>
        /// Получает или задает Индекс сортировки.
        /// </summary>
        [DetailView(Name = "Индекс сортировки", Visible = false)]
        [ListView(Name = "Индекс сортировки", Visible = false)]
        public int? SortIndex { get; set; }

        /// <summary>
        /// Получает или задает Периодичность выполнения события.
        /// </summary>
        [DetailView(Name = "Периодичность выполнения события", Visible = false)]
        [ListView(Name = "Периодичность выполнения события", Visible = false)]
        public Periodicity EventPeriodicity { get; set; }

        /// <summary>
        /// Получает или задает ИД периодичности выполнения события.
        /// </summary>
        [DetailView(Visible = false)]
        public int? EventPeriodicityID { get; set; }
    }
}