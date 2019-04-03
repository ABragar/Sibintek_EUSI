using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using Base.Translations;

namespace CorpProp.Entities.Import
{
    /// <summary>
    /// Представляет журнал ошибок импорта.
    /// </summary>
    [EnableFullTextSearch]
    public class ImportErrorLog : TypeObject
    {
        private static readonly CompiledExpression<ImportErrorLog, string> _ContactName =
          DefaultTranslationOf<ImportErrorLog>.Property(x => x.ContactName).Is(x => (x.ImportHistory == null)? "": x.ImportHistory.ContactName );

        private static readonly CompiledExpression<ImportErrorLog, string> _ContactEmail =
          DefaultTranslationOf<ImportErrorLog>.Property(x => x.ContactEmail).Is(x => (x.ImportHistory == null) ? "" : x.ImportHistory.ContactEmail);

        private static readonly CompiledExpression<ImportErrorLog, string> _ContactPhone =
           DefaultTranslationOf<ImportErrorLog>.Property(x => x.ContactPhone).Is(x => (x.ImportHistory == null) ? "" : x.ImportHistory.ContactPhone);

        private static readonly CompiledExpression<ImportErrorLog, DateTime?> _HistoryImportDateTime =
          DefaultTranslationOf<ImportErrorLog>.Property(x => x.HistoryImportDateTime).Is(x => (x.ImportHistory == null) ? null : x.ImportHistory.ImportDateTime);

        private static readonly CompiledExpression<ImportErrorLog, string> _ConsolidationTitle =
            DefaultTranslationOf<ImportErrorLog>.Property(x => x.ConsolidationTitle).Is(x => x.ImportHistory != null ?
                (x.ImportHistory.Society != null ? (x.ImportHistory.Society.ConsolidationUnit != null ? (x.ImportHistory.Society.ConsolidationUnit.Title)
                : "") : "") : "");

        /// <summary>
        /// Инициализирует новый экземпляр класса ImportErrorLog.
        /// </summary>
        public ImportErrorLog() : base() { }

        /// <summary>
        /// Получает или задает номер колонки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Номер колонки", ReadOnly = true)]
        public int? ColumnNumber { get; set; }

        /// <summary>
        /// Получает или задает номер строки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Номер строки", ReadOnly = true)]
        public int? RowNumber { get; set; }


        /// <summary>
        /// Получает или задает имя свойства.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PropetyName { get; set; }

        /// <summary>
        /// Получает или задает имя листа файла excel.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Лист", ReadOnly = true, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Sheet { get; set; }

        /// <summary>
        /// Получает или задает текст ошибки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Текст ошибки", ReadOnly = true)]      
        public string ErrorText { get; set; }

        /// <summary>
        /// Получает или задает тип ошибки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Тип ошибки", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ErrorType { get; set; }

        /// <summary>
        /// Получает или задает дату/вермя ошибки.
        /// </summary>
        [DetailView(Name = "Время сообщения", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? MessageDate { get; set; }

        /// <summary>
        /// Получает или задает атрибут "Комментарий".
        /// </summary>
        [DetailView(Name = "Комментарий")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comment { get; set; }

        /// <summary>
        /// Получает или задает ИД истории импорта.
        /// </summary>
        public int? ImportHistoryID { get; set; }

        /// <summary>
        /// ПОлучает или задает историю импорта.
        /// </summary>
        [DetailView(Name = "История импорта", ReadOnly = true)]
        public ImportHistory ImportHistory { get; set; }

        /// <summary>
        /// Получает ФИО ответственного за формирование файла импорта.
        /// </summary>
        [DetailView("Ответственный", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContactName => _ContactName.Evaluate(this);

        /// <summary>
        /// Получает email ответственного за формирование файла импорта.
        /// </summary>
        [DetailView("Email", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContactEmail => _ContactEmail.Evaluate(this);

        /// <summary>
        /// Получает телефон ответственного за формирование файла импорта.
        /// </summary>
        [DetailView("Телефон", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContactPhone => _ContactPhone.Evaluate(this);

        /// <summary>
        /// Получает дату/время связанной истории импорта.
        /// </summary>
        [DetailView("Дата/время импорта", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? HistoryImportDateTime => _HistoryImportDateTime.Evaluate(this);

        /// <summary>
        /// Получает БЕ.
        /// </summary>
        [DetailView("БЕ", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ConsolidationTitle => _ConsolidationTitle.Evaluate(this);        

        /// <summary>
        /// Получает или задает номер ЕУСИ.
        /// </summary>
        [DetailView("Номер ЕУСИ", Visible = false, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string EusiNumber { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>
        [DetailView("Инвентарный номер", Visible = false, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает код ошибки.
        /// </summary>
        [DetailView("Код ошибки", Visible = false, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ErrorCode { get; set; }

    }
}
