using Base.Attributes;
using Base.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Reporting.Entities
{
    /// <summary>
    /// Представляет отчетную форму для администрирования.
    /// </summary>
    public sealed class PrintedFormRegistry : BaseObject
    {
        /// <summary>
        /// Получает или задает номер отчета.
        /// </summary>
        [DetailView("Номер"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Получает или задает код отчета.
        /// </summary>
        [DetailView("Код"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReportPublishCode { get; set; }

        /// <summary>
        /// Получает или задает версию отчета.
        /// </summary>
        [DetailView("Версия"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReportVersion { get; set; }

        /// <summary>
        /// Получает или задает наименование отчета.
        /// </summary>
        [MaxLength(255)]
        [DetailView("Наименование"), ListView]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает описание отчета.
        /// </summary>
        [DetailView("Описание"), ListView]
        public string Description { get; set; }

        /// <summary>
        /// Получает категории пользователей, которым будет доступна печатная форма отчета.
        /// </summary>
        [DetailView("Категории пользователей")]
        public ICollection<PrintedFormUserCategory> UserCategories { get; set; } = new List<PrintedFormUserCategory>();

        /// <summary>
        /// Получает или задает файл отчета.
        /// </summary>
        [DetailView("Файл", Required = true)]
        [PropertyDataType(PropertyDataType.FileSelector)]
        public string GuidId { get; set; }

        /// <summary>
        /// Получает или задает системный код отчета.
        /// </summary>
        [DetailView("Код отчета", Required = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Code { get; set; }

        /// <summary>
        /// Получает или задает параметры отчета.
        /// </summary>
        [DetailView("Параметры", Description = "Формат: \"ИмяПараметра\":\"ЗначениеПараметра\".")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Params { get; set; }

        /// <summary>
        /// Получает или задает относительный путь файла отчета.
        /// </summary>
        [DetailView("Относительный путь", Description = "Используется только для ReportBook")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RelativePath { get; set; }

        /// <summary>
        /// Получает или задает тип отчета.
        /// </summary>
        [DetailView("Тип"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReportType { get; set; }

        /// <summary>
        /// Получает или задает функциональный модуль отчета.
        /// </summary>
        [DetailView("Функциональный модуль"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Module { get; set; }
    }

    /// <summary>
    /// Представляет связь категории пользователей с отчетной формой.
    /// </summary>
    public class PrintedFormUserCategory : EasyCollectionEntry<UserCategory>
    {

    }
}