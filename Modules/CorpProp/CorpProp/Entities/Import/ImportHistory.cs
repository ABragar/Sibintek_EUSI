using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace CorpProp.Entities.Import
{
    /// <summary>
    /// История импорта.
    /// </summary>
    [EnableFullTextSearch]
    public class ImportHistory : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ImportHistory.
        /// </summary>
        public ImportHistory() : base()
        {
            ImportDate = DateTime.Now;
            ImportDateTime = DateTime.Now;
            ImportErrorLogs = new List<ImportErrorLog>() { };
            ResultText = "";
        }

        /// <summary>
        /// Получает или задает дату/время импорта.
        /// </summary>
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? ImportDateTime { get; set; }

        /// <summary>
        /// Получает или задает мнемонику импортируемого объекта.
        /// </summary>
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Получает или задает ИД профиля пользователя.
        /// </summary>
        public int? SibUserID { get; set; }

        /// <summary>
        /// Получает или задает профиль пользователя.
        /// </summary>
        [SystemProperty]
        public virtual SibUser SibUser { get; set; }

        /// <summary>
        /// Получает или задает наименование импортируемого файла.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string FileName { get; set; }

        /// <summary>
        /// Получает или задает текст ошибки.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string ResultText { get; set; }

        /// <summary>
        /// Получает или задает наименование шаблона.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string TemplateName { get; set; }

        [SystemProperty]
        public int? FileCardID { get; set; }

        [DetailView("Файл", Visible = false)]
        public virtual FileCard FileCard { get; set; }

        /// <summary>
        /// Получает или задает признак отмены импорта.
        /// </summary>
        [SystemProperty]
        [DefaultValue(false)]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Признак успешного завершения.
        /// </summary>
        [SystemProperty]
        public bool IsSuccess { get; set; }

        public int? SocietyID { get; set; }

        /// <summary>
        /// ОГ.
        /// </summary>
        public Society Society { get; set; }

        /// <summary>
        /// Признак корректировки.
        /// </summary>
        [DefaultValue(false)]
        public bool IsCorrection { get; set; }

        /// <summary>
        /// Период.
        /// </summary>
        [PropertyDataType(PropertyDataType.Month)]
        public DateTime? Period { get; set; }

        /// <summary>
        /// Версия.
        /// </summary>
        public int? Version { get; set; }

        /// <summary>
        /// Версия данных.
        /// </summary>
        [DetailView("Версия данных", Visible = false)]
        [ListView(Visible = false)]
        public int? DataVersion { get; set; }

        /// <summary>
        /// Контактный телефон ответственного за заполнение файла
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Email ответственного за заполнение файла
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// ФИО ответственного за заполнение файла
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Получает или задает признак успешной отправки уведомления о результате импорта по e-mail.
        /// </summary>
        [SystemProperty]
        [DefaultValue(false)]
        [DetailView("Отправлено по E-mail", Visible = false)]
        [ListView(Visible = false)]
        public bool IsResultSentByEmail { get; set; }

        /// <summary>
        /// Получает или задает дату/время отправки уведомления о результате импорта по e-mail.
        /// </summary>
        [SystemProperty]
        [DetailView("Дата/Время отправки уведомления", Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? SentByEmailDate { get; set; }

        /// <summary>
        /// Получает или задает кол-во обработанных объектов.
        /// </summary>
        [SystemProperty]
        public int? Count { get; set; }

        public ICollection<ImportErrorLog> ImportErrorLogs { get; set; }

        [SystemProperty]
        public int? ImportHistoryStateID { get; set; }

        [DetailView("Статус", Visible = false), ListView(Visible = false)]
        public ImportHistoryState ImportHistoryState { get; set; }

        /// <summary>
        /// Получает или задаёт БЕ(балансовую единицу).
        /// </summary>
        [SystemProperty]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задаёт БЕ(балансовую единицу).
        /// </summary>
        [DetailView("БЕ", Visible = false), ListView(Visible = false)]
        public Consolidation Consolidation { get; set; }

        /// <summary>
        /// Получает или задает дату/время актуальности импортируемых данных.
        /// </summary>
        [DetailView("Актуальность данных", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? ActualityDate { get; set; }

        /// <summary>
        /// Получает или задаёт текущего пользователя, указанного в файле импорта.
        /// </summary>
        ///<remarks>Поле странного наименования, используется в шаблоне импортра BCS.</remarks>
        [DetailView("Текущий пользователь", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CurrentFileUser { get; set; }

        /// <summary>
        /// Получает или задает номер заявки ЦДС.
        /// Используется для указания номера ЦДС в теме нотификации
        /// </summary>
        public string NumberCDS { get; set; }

        /// <summary>
        /// Устанавливает в логах номер ЕУСИ и Инвентарный номер значениями из файла Excel.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colsMapping"></param>
        public void SetInvAndEUSILogs(IExcelDataReader reader)
        {
            try
            {
                var table = reader.GetVisbleTables()[0];
                var colsMapping = Helpers.ImportHelper.ColumnsNameMapping(table);

                var logs = ImportErrorLogs.Where(log => log.RowNumber.HasValue && log.RowNumber > 0
                && (String.IsNullOrEmpty(log.EusiNumber) || String.IsNullOrEmpty(log.InventoryNumber))).DefaultIfEmpty().ToList();
                if (!logs.Any())
                    return;

                var eusi = nameof(Accounting.AccountingObject.EUSINumber);
                var inv = nameof(Accounting.AccountingObject.InventoryNumber);

                int? eusiColumn = -1;
                int? invColumn = -1;

                if (colsMapping.ContainsValue(eusi))
                    eusiColumn = Helpers.ImportHelper.GetColumnNumber(table, colsMapping.FirstOrDefault(x => x.Value == eusi).Key);

                if (colsMapping.ContainsValue(inv))
                    invColumn = Helpers.ImportHelper.GetColumnNumber(table, colsMapping.FirstOrDefault(x => x.Value == inv).Key);

                var rows = table.Rows.Cast<System.Data.DataRow>();
                foreach (var log in logs.Where(l => l.RowNumber.HasValue))
                {
                    var row = rows.ElementAtOrDefault(log.RowNumber.Value - 1);
                    if (row != null)
                    {
                        if (eusiColumn.HasValue)
                            log.EusiNumber = row.ItemArray.ElementAtOrDefault(eusiColumn.Value)?.ToString();
                        if (invColumn.HasValue)
                            log.InventoryNumber = row.ItemArray.ElementAtOrDefault(invColumn.Value)?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}