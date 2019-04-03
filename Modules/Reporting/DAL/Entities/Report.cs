using System;

namespace DAL.Entities
{
    /// <summary>
    /// Представляет отчет.
    /// </summary>
    public class Report
    {

        /// <summary>
        /// Получает или задает идентификатор.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Получает или аздает УИД файла отчета.
        /// </summary>
        public Guid GuidId { get; set; }

        /// <summary>
        /// Получает или заадет наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает или заадет расширение файла отчета.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Получает или задает описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Получает или азадет категории пользователей, которым доступен данный отчет.
        /// </summary>
        public string UserCategories { get; set; }

        /// <summary>
        /// Получает или задает код отчета.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Получает или задает параметры отчета.
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// Получает или задает метку удаления.
        /// </summary>
        public bool? Hidden { get; set; }

        /// <summary>
        /// Получает или задает относительный путь файла отчета.
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// Получает или задает тип отчета.
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// Получает или задает функциональный модуль отчета.
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// Получает или задает номер отчета.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Получает или задает код отчета.
        /// </summary>
        public string ReportPublishCode { get; set; }

        /// <summary>
        /// Получает или задает версию отчета.
        /// </summary>
        public string ReportVersion { get; set; }
    }
}