using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using EUSI.Entities.Accounting;
using EUSI.Services.Monitor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace EUSI.Services.Accounting
{
    public interface IBalanceReconciliationReportService : IBaseObjectService<BalanceReconciliationReport>, IExcelImportEntity
    {

    }

    public class BalanceReconciliationReportService : BaseObjectService<BalanceReconciliationReport>,
        IBalanceReconciliationReportService
    {
        private readonly MonitorReportingImportService monitor = new MonitorReportingImportService();

        public BalanceReconciliationReportService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        protected override IObjectSaver<BalanceReconciliationReport> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<BalanceReconciliationReport> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.PositionConsolidation).SaveOneObject(x => x.Consolidation);
        }

        public void Import(IUnitOfWork uofw, IUnitOfWork histUofw, DataTable table,
            Dictionary<string, string> colsNameMapping, ref int count, ref ImportHistory history)
        {
            try
            {
                string err = "";
                //пропускаем первые 9 строк файла не считая строки названия колонок.
                int start = ImportHelper.GetRowStartIndex(table);

                if (!history.Period.HasValue || history.Consolidation == null)
                {
                    string formatName = FindFormatName(table);
                    ParseFormatData(histUofw, formatName, ref history);
                }

                int? consolidationId = history.Consolidation?.ID;

                var consolidation = uofw.GetRepository<Consolidation>()
                    .Filter(x => x.ID == consolidationId).FirstOrDefault();

                for (int i = start; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    ImportObject(uofw, row, colsNameMapping, ref err, ref count, ref history, consolidation);
                    count++;
                }

                if (!history.ImportErrorLogs.Any())
                {
                    monitor.CreateMonitorReportingForSuccessImport(history, count, histUofw);
                }
                else
                {
                    monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
                monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }
        }

        /// <summary>
        /// Имопртирует из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>
        /// <param name="colsNameMapping">Мэппинг колонок.</param>
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        /// <param name="history">Лог импорта.</param>
        public void ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping,
            ref string error, ref int count, ref ImportHistory history, Consolidation consolidation)
        {
            try
            {
                BalanceReconciliationReport balanceReconciliationReport = new BalanceReconciliationReport();

                balanceReconciliationReport.FillObject(uofw, typeof(BalanceReconciliationReport),
                    row, row.Table.Columns, ref error, ref history, colsNameMapping, true);

                BalanceReconciliationReport newBalanceReconciliationReport = Create(uofw, balanceReconciliationReport);

                if (consolidation != null)
                {
                    newBalanceReconciliationReport.Consolidation = consolidation;
                }

                newBalanceReconciliationReport.Period = history.Period;
                uofw.SaveChanges();
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public void CancelImport(IUnitOfWork uofw, ref ImportHistory history)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Поиск в таблице ячйки со значением наименования формата
        /// </summary>
        /// <param name="table">Таблица с данными из Excel</param>
        /// <returns></returns>
        private string FindFormatName(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];

                if (row.ItemArray[0].ToString().ToLower() == "наименование формата")
                {
                    return row.ItemArray[1].ToString();
                }
            }

            return "";
        }

        /// <summary>
        /// Получение ЕК и периода из наименования формата и установка ЕК в историю импорта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="formatName">Строка формата.</param>
        private void ParseFormatData(IUnitOfWork unitOfWork, string formatName, ref ImportHistory history)
        {
            if (string.IsNullOrEmpty(formatName))
            {
                throw new Exception("Строка <Наименование формата> пуста.");
            }

            string[] formatData = formatName.Split('_');

            if (formatData.Length < 3)
            {
                throw new Exception("Неверный формат в строке <Наименование формата>.");
            }

            string consolidationCode = formatData[0];
            var consolidation = string.IsNullOrEmpty(consolidationCode) ? null : unitOfWork.GetRepository<Consolidation>().Filter(f => f.Code == consolidationCode).FirstOrDefault();
            DateTime? parsedDate = DateTime.ParseExact(formatData[2], "ddMMyyyy", CultureInfo.InvariantCulture);

            if (consolidation == null || parsedDate == null)
            {
                throw new Exception("Не удалось определить еденицу консолидации или период. Проверьте строку <Наименование формата>.");
            }
            
            if (!history.Period.HasValue)
            {
                history.Period = new DateTime(parsedDate.Value.Year, parsedDate.Value.Month, 1);
            }

            if (history.Consolidation == null)
            {
                history.Consolidation = consolidation;
            }
        }
    }
}