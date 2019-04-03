using Base.DAL;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using EUSI.Entities.NSI;
using EUSI.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Accounting;

namespace EUSI.Services.Monitor
{
    public sealed class MonitorReportingImportService
    {
        private readonly Dictionary<string, string> _mappingMnemonicEventTypeCode = new Dictionary<string, string>
        {
            { nameof(AccountingObject), "IMP_AccState" },
            { nameof(AccountingMoving), "IMP_AccStateMov" },
            { nameof(BalanceReconciliationReport), "IMP_CoordinationBalanceAcc" },
            { nameof(RentalOS), "IMP_Rent" },
            { nameof(RentalOSMoving), "IMP_AccStateMovRent" },
            { nameof(RentalOSState), "IMP_AccStateRent" },
            { nameof(AccountingMovingMSFO), "IMP_AccStateMovSimpleTemplate" }
        };

        public const string SuccessImportCode = "loaded";

        public const string FailedImportCode = "notloaded";

        /// <summary>
        /// Фиксация в мониотре события о неудачном импорте
        /// </summary>
        /// <param name="history"></param>
        /// <param name="count">Количество обработанных объектов</param>
        /// <param name="histUofw"></param>
        /// <param name="eventCode">Если null, то событие определяется по мнемонике</param>
        public void CreateMonitorReportingForFailedImport(ImportHistory history, int count, IUnitOfWork histUofw, string eventCode = null)
        {
            CreateMonitorReportingForImport(history, false, count, histUofw, eventCode);
        }

        /// <summary>
        /// Фиксация в мониотре события об удачном импорте
        /// </summary>
        /// <param name="history"></param>
        /// <param name="count"></param>
        /// <param name="histUofw"></param>
        /// <param name="eventCode">Если null, то событие определяется по мнемонике</param>
        public void CreateMonitorReportingForSuccessImport(ImportHistory history, int count, IUnitOfWork histUofw, string eventCode = null)
        {
            CreateMonitorReportingForImport(history, true, count, histUofw, eventCode);
        }

        /// <summary>
        /// Метод, предзаполняющий модель ReportMonitoring из ImportHistory для передачи в общий сервис.
        /// </summary>
        /// <param name="history">История импорта</param>
        /// <param name="isSuccess"></param>
        /// <param name="count"></param>
        /// <param name="histUofw"></param>
        /// <param name="eventCode">Используется для фиксации дочерних событий</param>
        private void CreateMonitorReportingForImport(ImportHistory history, bool isSuccess, int count, IUnitOfWork histUofw, string eventCode)
        {
            var baseMonitorService = new MonitorReportingService();
            var reportMonitoring = new ReportMonitoring
            {
                ImportDateTime = history.ImportDateTime,
                IsValid = isSuccess,
                Mnemonic = history.Mnemonic,
                SibUser = history.SibUser,
                Consolidation = history.Consolidation
            };

            // Eсли eventCode пустой, то ищем по мнемонике
            if (string.IsNullOrEmpty(eventCode) && _mappingMnemonicEventTypeCode.ContainsKey(history.Mnemonic))
            {
                eventCode = _mappingMnemonicEventTypeCode[history.Mnemonic];
            }

            reportMonitoring.ReportMonitoringEventType = histUofw.GetRepository<ReportMonitoringEventType>().Filter(f => !f.Hidden && f.Code == eventCode).FirstOrDefault();
            reportMonitoring.ReportName = reportMonitoring.ReportMonitoringEventType?.Name;

            var dateFromHistory = history.Period;

            if (dateFromHistory.HasValue)
            {
                reportMonitoring.StartDate = new DateTime(dateFromHistory.Value.Year, dateFromHistory.Value.Month, 1);
                int lastDay = DateTime.DaysInMonth(dateFromHistory.Value.Year, dateFromHistory.Value.Month);
                reportMonitoring.EndDate = new DateTime(dateFromHistory.Value.Year, dateFromHistory.Value.Month, lastDay);
            }

            var resultCode = (isSuccess) ? SuccessImportCode : FailedImportCode;
            reportMonitoring.ReportMonitoringResultID = histUofw.GetRepository<ReportMonitoringResult>()
                .FilterAsNoTracking(f => !string.IsNullOrEmpty(f.Code) && f.Code.ToLower() == resultCode)
                .FirstOrDefault()?.ID;
            reportMonitoring.Comment = (isSuccess)? ImportHelper.GetSuccessHistoryResultText(history.Mnemonic, history.ResultText, count) 
                : ImportHelper.GetFailedHistoryResultText(history.ResultText, count);            

            histUofw.GetRepository<ReportMonitoring>().Create(reportMonitoring);
            histUofw.SaveChanges();

            baseMonitorService.CreateMonitorReporting(reportMonitoring, histUofw);
        }
    }
}