using Base.DAL;
using Base.UI.Service;
using CorpProp;
using CorpProp.Common;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using EUSI.Services.Monitor;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace EUSI.Services.Import
{
    public class EUSIImportChecker : ImportChecker
    {
        private const int CountOfImportedRecords = 0;


        public override void StartDataCheck(IUnitOfWork uofw, IUnitOfWork histUow, DataTable table, Type type,
            ref ImportHistory history, bool dictCode = false)
        {
            base.StartDataCheck(uofw, histUow, table, type, ref history, dictCode);

            CreateMonitorRecord(history, histUow);
        }

        public override string FormatConfirmImportMessage(List<string> fileDescriptions)
        {
            return $"По " +
                   $"{(fileDescriptions.Count > 1 ? $"найденным объектам " : $"найденному объекту")} " +
                   $"{string.Join("и ", fileDescriptions)} " +
                   $"данные будут перезаписаны";
        }

        public static void CreateMonitorRecord(ImportHistory history, IUnitOfWork histUow)
        {
            var mnemonicsForMonitor = new List<string>
            {
                "AccountingObject",
                "AccountingMoving",
                "BalanceReconciliationReport"
                //TODO: Добавить в список сущности RentalOS после их появления
            };

            if (history.ImportErrorLogs.Any() && mnemonicsForMonitor.Contains(history.Mnemonic))
            {
                new MonitorReportingImportService()
                    .CreateMonitorReportingForFailedImport(history, CountOfImportedRecords, histUow);
            }
        }
    }
}