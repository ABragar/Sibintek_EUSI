using Base.Attributes;

namespace Base.Reporting.Entities
{
    /// <summary>
    /// Представляет печатную форму отчета.
    /// </summary>
    public sealed class PrintedFormList : BaseObject
    {
        /// <summary>
        /// Получает или задает номер отчета.
        /// </summary>
        [DetailView("Номер"), ListView]
        public string Number { get; set; }

        /// <summary>
        /// Получает или задает код отчета.
        /// </summary>
        [DetailView("Код"), ListView]
        public string ReportPublishCode { get; set; }

        /// <summary>
        /// Получает или задает версию отчета.
        /// </summary>
        [DetailView("Версия"), ListView]
        public string ReportVersion { get; set; }

        /// <summary>
        /// Получает или задает наименование отчета.
        /// </summary>
        [DetailView("Наименование", ReadOnly = true), ListView]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает описание отчета.
        /// </summary>
        [DetailView("Описание", ReadOnly = true), ListView]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает файл отчета.
        /// </summary>
        [DetailView("Файл", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.FileSelector)]
        public string GuidId { get; set; }

        /// <summary>
        /// Получает или задает тип отчета.
        /// </summary>
        [DetailView("Тип"), ListView]
        public string ReportType { get; set; }

        /// <summary>
        /// Получает или задает функциональный модуль отчета.
        /// </summary>
        [DetailView("Функциональный модуль"), ListView]
        public string Module { get; set; }
    }
}