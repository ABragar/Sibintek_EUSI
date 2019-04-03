using Base;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Export;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using CorpProp.Services.Import;
using CorpProp.Services.Import.BulkMerge;
using EUSI.Entities.Accounting;
using EUSI.Entities.Estate;
using EUSI.Entities.NSI;
using EUSI.Services.Monitor;
using ExcelDataReader;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace EUSI.Services.Accounting
{
    internal delegate bool RentalOSDiscrepancyCondition(AccountingObject os, RentalOS rent);

    public interface IRentalOSService : ITypeObjectService<RentalOS>, IExcelImportEntity, IExportToZip, IAdditionalExcelImportChecker
    {
    }

    /// <summary>
    /// Представляет сервис по работе с ОС/НМА в ИР Аренда.
    /// </summary>
    public class RentalOSService : TypeObjectService<RentalOS>, IRentalOSService
    {
        private readonly ILogService _logger;
        private readonly IFileSystemService _fileSystemService;
        private readonly MonitorReportingImportService monitor = new MonitorReportingImportService();
        private IImportHistoryService _importHistoryService;

        /// <summary>
        /// 14364
        /// Содержит перечень условий для поска расхождения
        /// Системное имя аттрибута
        /// Наименование для формирования комментария
        /// Условие расхождения
        /// Значение ФСД
        /// Значение БУС
        /// </summary>
        private readonly List<RentalOSDiscrepancyCheckItem> _rentalOsDiscrepancyCheckList = new List<RentalOSDiscrepancyCheckItem>()
            {
                new RentalOSDiscrepancyCheckItem(
                    nameof(RentalOS.OKOF2014),
                    "ОКОФ",
                    (o, r) => { return r.OKOF2014?.Code != o.OKOF2014?.Code || r.OKOF2014?.Name != o.OKOF2014?.Name; },
                    r => r.OKOF2014?.Title,
                    o => o.OKOF2014?.Title
                ),
                new RentalOSDiscrepancyCheckItem(
                    nameof(RentalOS.DepreciationGroup),
                    "Амортизационная группа",
                    (o, r) => { return r.DepreciationGroup?.Name != o.DepreciationGroup?.Name; },
                    r => r.DepreciationGroup?.Name,
                    o => o.DepreciationGroup?.Name
                ),
                new RentalOSDiscrepancyCheckItem(
                    nameof(RentalOS.ProprietorSubject),
                    "Контрагент (арендодатель)",
                    (o, r) => { return r.ProprietorSubject?.SDP != o.ProprietorSubject?.SDP; },
                    r => r.ProprietorSubject?.SDP,
                    o => o.ProprietorSubject?.SDP
                ),
                new RentalOSDiscrepancyCheckItem(
                    nameof(RentalOS.RentContractNumber),
                    "Уникальный номер договора Компании",
                    (o, r) => { return r.RentContractNumber != o.RentContractNumber; },
                    r => r.RentContractNumber,
                    o => o.RentContractNumber
                ),
                new RentalOSDiscrepancyCheckItem(
                    nameof(RentalOS.LandPurpose),
                    "Назначение ЗУ",
                    (o, r) => { return r.LandPurpose?.Name != o.LandPurpose?.Name; },
                    r => r.LandPurpose?.Name,
                    o => o.LandPurpose?.Name
                ),
                new RentalOSDiscrepancyCheckItem(
                    nameof(RentalOS.Deposit),
                    "Назначение ЗУ",
                    (o, r) => { return r.Deposit?.Name != o.Deposit?.Name; },
                    r => r.Deposit?.Name,
                    o => o.Deposit?.Name
                )
            };


        /// <summary>
        /// Инициализирует новый экземпляр класса RentalOSService.
        /// </summary>
        /// <param name="facade"></param>
        public RentalOSService(
            IBaseObjectServiceFacade facade
            , IFileSystemService fileSystemService
            , ILogService logger
           , IImportHistoryService importHistoryService) : base(facade, logger)
        {
            _logger = logger;
            _fileSystemService = fileSystemService;
            _importHistoryService = importHistoryService;
        }

        /// <summary>
        /// Импорт из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        /// <param name="history"></param>
        public virtual void Import(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {
            try
            {               
                var checker = new ImportChecker();
                if (!checker.ParseFileNameDefult(_importHistoryService, histUofw, ref history, true))
                {
                    monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
                    return;
                }
                var type = typeof(RentalOS);
                var queryBuilder = new RentalOSImportQueryBuilder(colsNameMapping, type, histUofw, new DateTime(history.Period.Value.Year, history.Period.Value.Month, 1));
                var bulkMergeManager = new BulkMergeManager(colsNameMapping, type, ref history, queryBuilder);
                bulkMergeManager.BulkInsertAll(table, type, colsNameMapping, ref count);

                if (!history.ImportErrorLogs.Any())
                    monitor.CreateMonitorReportingForSuccessImport(history, count, histUofw);
                else
                    monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);
                monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }
        }

        /// <summary>
        /// Отмена импорта. Актуально только для объектов, созданных при импорте.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="history"></param>
        public void CancelImport(
            IUnitOfWork uow
            , ref ImportHistory history
        )
        {
            throw new NotImplementedException();
        }
        

        public string ExportToZip(IUnitOfWork uow, int[] ids)
        {
            return "";
        }

        /// <summary>
        /// Экспорт в Zip.
        /// </summary>
        /// <param name="uow"></param>
        /// <returns></returns>
        public string CustomExportToZip(IUnitOfWork uow, int[] ids)
        {
            List<string> filesList = new List<string>();

            var historys = uow.GetRepository<ImportHistory>()
                        .FilterAsNoTracking(f => ids.Contains(f.ID) && f.IsSuccess && f.FileCardID != null)
                        .DefaultIfEmpty()
                        .Select(s => new
                        {
                            FileCardID = (s.FileCardID == null) ? 0 : s.FileCardID.Value
                            ,
                            s.Oid
                        })
                        .ToList();

            foreach (var hist in historys)
            {
                var file = uow.GetRepository<FileCardOne>().Filter(f => f.ID == hist.FileCardID).FirstOrDefault();

                FileData fileData = file.FileData;
                bool isExcel = (fileData.Extension == "XLS" || fileData.Extension == "XLSX");

                string filePath = _fileSystemService.GetFilePath(fileData.FileID);
                using (StreamReader stream = new StreamReader(filePath))
                {
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream.BaseStream))
                    {
                        var table = reader.GetVisbleTables()[0];
                        ImportStarter.DeleteEmptyRows(ref table);

                        var colsMap = ImportHelper.ColumnsNameMapping(table);
                        var startRowIndex = ImportHelper.GetRowStartIndex(table) + 1;
                        var startColumnIndex = table.Columns.IndexOf((table.Columns.Cast<DataColumn>()
                                .FirstOrDefault(f => f.ColumnName == colsMap.First(w => !String.IsNullOrEmpty(w.Value)).Key))) + 1;

                        var date = DateTime.Now;
                        //формат наименования файла
                        if (Regex.IsMatch(fileData.FileName, ImportHelper._FILE_NAME_TEMPLATE_CODE_YYYY_MM_dd))
                        {
                            string[] arFileName = fileData.FileName.Split('_');
                            string be = ImportHelper.GetIDEUP(arFileName[0]);
                            DateTime period = new DateTime(int.Parse(arFileName[1]), int.Parse(arFileName[2]), int.Parse(arFileName[3]));
                            date = period;
                        }

                        var qb = new RentalOSExportQueryBuilder(colsMap, typeof(RentalOS), date, hist.Oid);

                        var newFilePath = GenerateNewFilePath("", filePath, fileData);
                        using (ExcelPackage pack = new ExcelPackage(new FileInfo(newFilePath)))
                        {
                            //TODO: оптимизировать экспорт большого объёма данных
                            var dataTable = qb.SelectResult(table);

                            ExcelWorksheet worksheet = pack.Workbook.Worksheets[1];
                            worksheet.Cells[startRowIndex, startColumnIndex]
                                .LoadFromDataTable(dataTable, false);

                            MarkDiscrepancyCells(worksheet, dataTable, startRowIndex, startColumnIndex);
                            pack.Save();
                            filesList.Add(newFilePath);
                        }
                    }
                }
            }
            return ExcelExportHelper.PrepareArchive(filesList);
        }

        /// <summary>
        /// Окрашивает ячейки листа файла Excel, имеющие различия в данных ФСД и БУС, найденные при импорте и записанные в колонке комментриев.
        /// </summary>
        /// <param name="worksheet">Лист файла Excel.</param>
        /// <param name="dataTable">Таблица данных.</param>
        /// <param name="startRowIndex">Индекс начала строки записи данных.</param>
        /// <param name="startColumnIndex">Индекс начала колонки записи данных.</param>
        private void MarkDiscrepancyCells(ExcelWorksheet worksheet, DataTable dataTable, int startRowIndex, int startColumnIndex)
        {
            if (!dataTable.Columns.Contains(nameof(RentalOS.Comments)))
                return;

            dataTable.Rows.Cast<DataRow>()
                               .Where(r => !String.IsNullOrWhiteSpace(r[nameof(RentalOS.Comments)]?.ToString()))
                               .ToList().ForEach(r =>
                               {
                                   var comment = r[nameof(RentalOS.Comments)].ToString();
                                   foreach (var checkItem in _rentalOsDiscrepancyCheckList.Where(rr => comment.Contains(rr.Name)))
                                   {
                                       var columnIdx = dataTable.Columns[checkItem.SystemName].Ordinal;

                                       var backGroundColor = Color.FromArgb(255, 124, 128);
                                       var cell = worksheet.Cells[startRowIndex + dataTable.Rows.IndexOf(r), startColumnIndex + columnIdx];
                                       cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                       cell.Style.Fill.BackgroundColor.SetColor(backGroundColor);
                                   }
                               });
        }

        private string GenerateNewFilePath(string prefix, string oldPath, FileData fd)
        {
            var root = _fileSystemService.FilesDirectory;
            var oid = Guid.NewGuid();
            var newPath = root + "\\Temp\\" + oid;

            if (!System.IO.Directory.Exists(newPath))
                System.IO.Directory.CreateDirectory(newPath);

            var newFileName = $"{newPath}\\{((!String.IsNullOrEmpty(prefix)) ? prefix + "_" : "")}{fd.FileName}";

            if (System.IO.File.Exists(newFileName))
                System.IO.File.Delete(newFileName);
            File.Copy(oldPath, newFileName);
            return newFileName;
        }

        public void AdditionalChecks(IUnitOfWork uofw, IUnitOfWork histUnitOfWork, DataTable table, Type type, ref ImportHistory history,
            bool dictCode = false)
        {
            #region Проверка на дубликаты

            var shortDataTable = ImportHelper.GetNamedDataOnlyTable(table);

            var duplicates = shortDataTable.AsEnumerable()
                .GroupBy(r => new
                {
                    Consolidation = r[nameof(RentalOS.Consolidation)],
                    EUSINumber = r[nameof(RentalOS.EUSINumber)],
                    InventoryNumber = r[nameof(RentalOS.InventoryNumber)]
                })
                .Where(gr => gr.Count() > 1);

            foreach (var duplicate in duplicates)
            {
                history.ImportErrorLogs.Add(new ImportErrorLog
                {
                    MessageDate = DateTime.Now,
                    ErrorText = $"Запись по ключу БЕ:{duplicate.Key.Consolidation}, Номер ЕУСИ:{duplicate.Key.EUSINumber}, Инвентарный номер: {duplicate.Key.InventoryNumber} имеет {duplicate.Count()} " +
                                $"дубликатов в файле импорта",
                    ErrorType = ErrorTypeName.System,
                    Sheet = table.TableName
                });
            }

            #endregion Проверка на дубликаты
        }
    }

    public class RentalOSImportQueryBuilder : QueryBuilder
    {
        string errTable = $"@errorTable";
        string colOg2 = "Og2";
        string colOg2IsConnected = "Og2IsConnected";
        string varOg2BECode = "@Og2BECode";
        string colSubj = "SubjID";
        string colOg1 = "Og1";
        string colOg1IsConnected = "Og1IsConnected";
        string colOg1BECode = "Og1BECode";
        string colTransactionKindCode = "TransactionKindCode";
        string colEusiExist = "EusiExist";
        string colRule = "colRule";
        string colFindERVGP = "FindERVGP";
        string colOS1Oid = "OS1Oid";
        string colEstateID = "colEstateID";
        string colStateRentCode = "StateRentCode";
        string colNewStateRentCode = "NewStateRentCode";
        string varStartPeriod = "@StartPeriod";
        string varEndPeriod = "@EndPeriod";
        string varHistoryID = "@HistoryID";
        string varHistoryOid = "@HistoryOid";
        string varOSUpd = "@OSToUpd";
        string varRentUpd = "@RentToUpd";

        string startPeriod = "";
        string endPeriod = "";
        Dictionary<string, string> osRequiredColumns = null;
        List<SqlColumnDefinition> osColumnDefinition = null;
        Dictionary<string, string> importObjectColumns = null;

        IUnitOfWork _histUofw;

        public RentalOSImportQueryBuilder(
            Dictionary<string, string> colsNameMapping
            , Type type
            , IUnitOfWork histUofw
            , DateTime start
            ) : base(colsNameMapping, type)
        {

            startPeriod = $"'{start.ToString("yyyy-MM-dd")}'";
            endPeriod = $"'{start.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}'";
            _histUofw = histUofw;
        }

        /// <summary>
        /// Выполнение команды импорта данных, обработка результатов.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public override int Merge(SqlCommand command, ref ImportHistory history)
        {
            _histUofw.GetRepository<ImportHistory>()
                .Create(history);
            _histUofw.SaveChanges();

            command.CommandText = BuildMergeQuery(history);
            var reader = command.ExecuteReader();
            int ErrorsCount = 0;
            int ImportRowsCount = 0;

            do
            {
                reader.Read();
                var resultName = reader.GetName(0);
                switch (resultName)
                {
                    case "ErrorsCount":
                        ErrorsCount = reader.HasRows ? reader.GetInt32(0) : 0;
                        break;
                    case "ImportRowsCount":
                        ImportRowsCount = reader.HasRows ? reader.GetInt32(0) : 0;
                        break;
                }
            } while (reader.NextResult());
            reader.Close();

            //Reload History!
            _histUofw.ReloadEntity<ImportHistory>(history, nameof(ImportHistory.ImportErrorLogs));

            return ImportRowsCount;
        }

        /// <summary>
        /// Переопределяет текст скрипта, выполняемого после вставки импортируемых данных во временную тыблицу.
        /// Описание логики импорта данных в Систему.
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public override string BuildMergeQuery(ImportHistory history)
        {
            importObjectColumns = GetImportObjectDict();
            osColumnDefinition = GetSqlColumnDefinition(typeof(AccountingObjectTbl));
            osRequiredColumns = GetRequiredColumnsAndValues(typeof(AccountingObjectTbl));
            osRequiredColumns.Remove("[Oid]");

            var script = new StringBuilder();
            script.AppendLine($"");

            //Переменные           
            script.AppendLine($"DECLARE {varHistoryID} INT = {history.ID}");
            script.AppendLine($"DECLARE {varHistoryOid} UNIQUEIDENTIFIER = '{history.Oid}'");
            script.AppendLine($"DECLARE {varOg2BECode} NVARCHAR(MAX) = N'{history.Consolidation?.Code}'");
            script.AppendLine($"DECLARE {varStartPeriod} DATETIME = {startPeriod}");
            script.AppendLine($"DECLARE {varEndPeriod} DATETIME = {endPeriod}");
            //таблица ошибок
            script.AppendLine($"DECLARE {errTable} TABLE(" +
                $"rowNumb INT, " +
                $"text NVARCHAR(max), " +
                $"errResultCode NVARCHAR(max), " +
                $"errCode NVARCHAR(max)," +
                $"invNumber NVARCHAR(max)," +
                $"eusiNumber NVARCHAR(max)" +
                $")");

            //таблица ИД-ов ОС2
            script.AppendLine($"DECLARE {varOSUpd} TABLE(RowID int, OSID int, Oid uniqueidentifier NULL, Action INT)");
            //таблица ИД-ов ИР-Аренды
            script.AppendLine($"DECLARE {varRentUpd} TABLE(RowID int, ID int, Oid uniqueidentifier NULL, Action INT)");

            //Подготовка, обновление кастомных колонок 
            script.AppendLine(PrepareScript());

            //Проверки, валидаторы
            script.AppendLine(DataCheckScript());

            //Идентификация ОС
            script.AppendLine(IdentityOSScript());                       

            //Идентификация аренды, проверка статуса
            script.AppendLine(IdentityRentalScript());

            //удаление ошибочных строк из источника
            script.AppendLine(DeleteErrRowsScript());

            script.AppendLine($"IF (");
            script.AppendLine($"(SELECT COUNT(*)");
            script.AppendLine($"FROM {errTable}");
            script.AppendLine($"WHERE errCode IS NOT NULL)");
            script.AppendLine($" = 0)");
            script.AppendLine($"BEGIN");
            //Импорт только если нет ошибок
            script.AppendLine(ImportDataScript());
            script.AppendLine($"END");

            //TODO: фиксируем в ImportObject

            //Запись ошибок в БД
            script.AppendLine(InsertErrorScript());

            //всего ошибок:
            script.AppendLine($"SELECT COUNT(*) AS [ErrorsCount]");
            script.AppendLine($"FROM {errTable}");
            script.AppendLine($"WHERE errCode IS NOT NULL");

            //всего импортировано:
            script.AppendLine($"SELECT COUNT(*) AS [ImportRowsCount]");
            script.AppendLine($"FROM {GetTempTableName()}");

            return script.ToString();
        }


        /// <summary>
        /// Переопределяет скрипт создания временной таблицы импорта.
        /// </summary>
        /// <returns></returns>
        protected override string GetCreateTableScript()
        {
            var tableName = GetTempTableName();
            var properties = MainType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray();
            var createScript = new StringBuilder();
            createScript.AppendLine($"IF OBJECT_ID(N'tempdb..{tableName}') IS NOT NULL DROP TABLE {tableName} \r\n " +
                $"CREATE TABLE {tableName} ({GetTempTableColumnsSpecification(properties)}");
            createScript.AppendLine($", {colOg2} INT ");
            createScript.AppendLine($", {colOg2IsConnected} BIT DEFAULT(0)");
            createScript.AppendLine($", {colSubj} INT ");
            createScript.AppendLine($", {colOg1} INT ");
            createScript.AppendLine($", {colOg1IsConnected} BIT DEFAULT(0)");
            createScript.AppendLine($", {colOg1BECode} NVARCHAR(MAX) ");
            createScript.AppendLine($", {colTransactionKindCode} NVARCHAR(MAX) ");
            createScript.AppendLine($", {colEusiExist} BIT DEFAULT(0)");
            createScript.AppendLine($", {colRule} INT ");
            createScript.AppendLine($", {colFindERVGP} NVARCHAR(MAX) ");
            createScript.AppendLine($", {colOS1Oid} UNIQUEIDENTIFIER ");
            createScript.AppendLine($", {colStateRentCode} NVARCHAR(MAX) ");
            createScript.AppendLine($", {colNewStateRentCode} NVARCHAR(MAX) ");
            createScript.AppendLine($", {colEstateID} INT ");
            createScript.AppendLine($")");
            return createScript.ToString();
        }

        /// <summary>
        /// Формирует скрипт подготовки данных к имопрту.
        /// </summary>
        /// <returns></returns>
        string PrepareScript()
        {
            //Обновление кастомных колонок

            var script = new StringBuilder();
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"  src.{nameof(RentalOS.Consolidation)} = {varOg2BECode}");
            script.AppendLine($", src.{colOg2} = be2Dict.ID");
            script.AppendLine($", src.{colOg2IsConnected} = ISNULL(be2Dict.ConnectToEUSI,0)");
            script.AppendLine($", src.{colEusiExist} = CASE WHEN ISNULL(LTRIM(RTRIM(src.{nameof(RentalOS.EUSINumber)})),N'') = N'' THEN 0 ELSE 1 END");
            script.AppendLine($", src.{colTransactionKindCode} = trKind.Code");           
            script.AppendLine($"");
            script.AppendLine($"");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"LEFT JOIN ");
            script.AppendLine($"(");
            script.AppendLine($"SELECT TOP 1 dd.ID,dd.Code,cons.{nameof(Consolidation.ConnectToEUSI)}");
            script.AppendLine($"FROM {GetTableName(typeof(Consolidation))} cons");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} dd ON cons.ID = dd.ID");
            script.AppendLine($"WHERE dd.Hidden = 0 AND dd.IsHistory = 0 AND dd.Code = {varOg2BECode} ");
            script.AppendLine($") be2Dict ON be2Dict.Code = {varOg2BECode} ");
            script.AppendLine($"LEFT JOIN ");
            script.AppendLine($"(");
            script.AppendLine($"SELECT tr.ID, trdd.Code, trdd.Name");
            script.AppendLine($"FROM {GetTableName(typeof(TransactionKind))} tr");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} trdd ON tr.ID = trdd.ID");
            script.AppendLine($"WHERE trdd.Hidden = 0 AND trdd.IsHistory = 0");
            script.AppendLine($") trKind ON ISNULL(src.{nameof(RentalOS.TransactionKind)},N'') <> N'' AND (");
            script.AppendLine($"(trKind.Code = src.{nameof(RentalOS.TransactionKind)} ) OR");
            script.AppendLine($"(trKind.Name = src.{nameof(RentalOS.TransactionKind)} ) ");
            script.AppendLine($")");
            script.AppendLine();

            //поиск ДП 1
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET ");
            script.AppendLine($"[{colSubj}] = (");
            script.AppendLine($"SELECT TOP 1 t.ID ");
            script.AppendLine($"FROM {GetTableName(typeof(Subject))} AS t");
            script.AppendLine($"LEFT OUTER JOIN (SELECT top 1 split.a.value('.', 'VARCHAR(100)') AS Val FROM (SELECT Cast ('<M>' + REPLACE(REPLACE(ISNULL([{nameof(RentalOS.ProprietorSubject)}],''),' ',''), '/', '</M><M>')+ '</M>' AS XML) AS Data) AS A CROSS apply data.nodes ('/M') AS Split(a)) as tValue on t.sdp=tValue.Val or t.inn=tValue.Val or t.KSK=tValue.Val ");
            script.AppendLine($"LEFT OUTER JOIN (select id as 'subjID', INN,KPP from [CorpProp.Subject].Subject) as tValue2 on ((ISNULL(REPLACE(tValue2.inn,' ',''), '') + '/' + ISNULL(REPLACE(tValue2.kpp,' ',''),''))=REPLACE(ISNULL([{nameof(RentalOS.ProprietorSubject)}],''),' ','')) and tValue2.subjID=t.ID ");
            script.AppendLine($"WHERE (tValue2.subjID IS NOT NULL OR tValue.Val IS NOT NULL) AND t.Hidden = 0 AND t.IsHistory = 0");
            script.AppendLine($") ");
            script.AppendLine();

            //поиск ОГ1
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"  src.{colOg1} = soc.ID");
            script.AppendLine($", src.{colOg1IsConnected} = ISNULL(cons.ConnectToEUSI, 0)");
            script.AppendLine($", src.{colOg1BECode} = dd.Code");
            script.AppendLine($"FROM {GetTempTableName()} src ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Subject))} subj ON src.{colSubj} = subj.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Society))} soc ON subj.{nameof(Subject.SocietyID)} = soc.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(Consolidation))} cons ON soc.{nameof(Society.ConsolidationUnitID)} = cons.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} dd ON cons.ID = dd.ID");
            script.AppendLine();


            //номера бизнес-правил            
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET ");
            script.AppendLine($"{colRule} = CASE ");
            //1. ОГ-2 подключен к ЕУСИ, ОГ-1 в ЕУСИ, вид операции один из (100,300), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 1 AND {colTransactionKindCode} IN (N'100',N'300') AND {colEusiExist} = 1 THEN 1");
            //2. ОГ-2 подключен к ЕУСИ, ОГ-1 в ЕУСИ, вид операции один из (200,500), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 1 AND {colTransactionKindCode} IN (N'200',N'500') AND {colEusiExist} = 1 THEN 2");
            //3. ОГ-2 подключен к ЕУСИ, ОГ-1 в ЕУСИ, вид операции = 400, номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 1 AND {colTransactionKindCode} = N'400' AND {colEusiExist} = 1 THEN 3");
            //4. ОГ-2 подключен к ЕУСИ, ОГ-1 не в ЕУСИ, вид операции один из (100,300), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 0 AND {colTransactionKindCode} IN (N'100',N'300') AND {colEusiExist} = 1  THEN 4");
            //5. ОГ-2 в ЕУСИ, ОГ-1 не в ЕУСИ, вид операции один из (200,500), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 0 AND {colTransactionKindCode} IN (N'200',N'500') AND {colEusiExist} = 1 THEN 5");
            //6. ОГ-2 в ЕУСИ, ОГ-1 не в ЕУСИ, вид операции = 400, номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 0 AND {colTransactionKindCode} = N'400' AND {colEusiExist} = 1 THEN 6");
            //7. ОГ-2 в ЕУСИ, контрагент не ог, вид операции один из (100,300), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NULL AND {colTransactionKindCode} IN (N'100',N'300') AND {colEusiExist} = 1 THEN 7");
            //8. ОГ-2 в ЕУСИ, контрагент не ог, вид операции один из (200,500), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NULL AND {colTransactionKindCode} IN (N'200',N'500') AND {colEusiExist} = 1 THEN 8");
            //9. ОГ-2 в ЕУСИ, контрагент не ог, вид операции = 400, номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 1 AND {colOg1} IS NULL AND {colTransactionKindCode} = N'400' AND {colEusiExist} = 1 THEN 9");
            //10. ОГ-2 не в ЕУСИ, ОГ-1 в ЕУСИ, вид операции один из (100,300), номер ЕУСИ не заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 1 AND {colTransactionKindCode} IN (N'100',N'300') AND {colEusiExist} = 0 THEN 10");
            //11. ОГ-2 не в ЕУСИ, ОГ-1 в ЕУСИ, вид операции один из (200,500), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 1 AND {colTransactionKindCode} IN (N'200',N'500') AND {colEusiExist} = 1 THEN 11");
            //12. ОГ-2 не в ЕУСИ, ОГ-1 в ЕУСИ, вид операции = 400, номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 1 AND {colTransactionKindCode} = N'400' AND {colEusiExist} = 1 THEN 12");
            //13. ОГ-2 не в ЕУСИ, ОГ-1 не в ЕУСИ, вид операции один из (100,300), номер ЕУСИ не заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 0 AND {colTransactionKindCode} IN (N'100',N'300') AND {colEusiExist} = 0 THEN 13");
            //14. ОГ-2 не в ЕУСИ, ОГ-1 не в ЕУСИ, вид операции один из (200,500), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 0 AND {colTransactionKindCode} IN (N'200',N'500') AND {colEusiExist} = 1 THEN 14");
            //15. ОГ-2 не в ЕУСИ, ОГ - 1 не в ЕУСИ, вид операции = 400, номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NOT NULL AND {colOg1IsConnected} = 0 AND {colTransactionKindCode} = N'400' AND {colEusiExist} = 1 THEN 15");
            //16. ОГ-2 не в ЕУСИ, контрагент-сторонняя организация, вид операции один из (100,300), номер ЕУСИ не заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NULL AND {colTransactionKindCode} IN (N'100',N'300') AND {colEusiExist} = 0 THEN 16");
            //17. ОГ-2 не в ЕУСИ, контрагент-сторонняя организация, вид операции один из (200,500), номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NULL AND {colTransactionKindCode} IN (N'200',N'500') AND {colEusiExist} = 1 THEN 17");
            //18. ОГ-2 не в ЕУСИ, контрагент-сторонняя организация, вид операции = 400, номер ЕУСИ заполнен
            script.AppendLine($"WHEN {colOg2IsConnected} = 0 AND {colOg1} IS NULL AND {colTransactionKindCode} = N'400' AND {colEusiExist} = 1 THEN 18");
            script.AppendLine($"ELSE NULL");
            script.AppendLine($"END ");
            script.AppendLine();

            //целевой статус в соответствии с правилом
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.{colNewStateRentCode} = CASE ");
            script.AppendLine($"WHEN ISNULL({colRule},0) IN (1,2,4,5,7,8,10,11,13,14,16,17) THEN N'InRent'");
            script.AppendLine($"WHEN ISNULL({colRule},0) IN (3,6,9,12,15,18) THEN N'Leaving'");
            script.AppendLine($"ELSE NULL");
            script.AppendLine($"END");
            script.AppendLine($"FROM {GetTempTableName()} src ");


            //поиск заявок ВГП
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.{colFindERVGP} = CASE ");
            script.AppendLine($"WHEN ers.ID IS NOT NULL THEN");
            script.AppendLine($"N'Результат поиска заявки ВГП: В Системе найдена соответствующая заявка вида \"ВГП\" Номер <' + CAST(ers.Number AS NVARCHAR(MAX)) + N'>, статус <' + ers.[State] + N'>'");
            script.AppendLine($"ELSE");
            script.AppendLine($"N'Результат поиска заявки ВГП: В Системе не найдена соответствующая заявка вида \"ВГП\"'");
            script.AppendLine($"END");
            script.AppendLine($"FROM {GetTempTableName()} src ");
            script.AppendLine($"LEFT JOIN ");
            script.AppendLine($"(");
            script.AppendLine($"SELECT er.ID, er.Number, er.ERContractNumber, cons.Code AS [BeCode], st.Name AS [State]");
            script.AppendLine($"FROM {GetTableName(typeof(EstateRegistration))} er");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} cons ON er.{nameof(EstateRegistration.ConsolidationID)} = cons.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} st ON er.{nameof(EstateRegistration.StateID)} = st.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} tt ON er.{nameof(EstateRegistration.ERTypeID)} = tt.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} rr ON er.{nameof(EstateRegistration.ERReceiptReasonID)} = rr.ID");
            script.AppendLine($"WHERE er.Hidden = 0 AND er.IsHistory = 0");
            script.AppendLine($"AND UPPER(ISNULL(tt.Code,N'')) = N'OSVGP'");
            script.AppendLine($"AND UPPER(ISNULL(rr.Code,N'')) = N'RENTOUT'");
            script.AppendLine($") ers ON");
            script.AppendLine($"src.{colOg1BECode} IS NOT NULL AND src.{colOg1BECode} = ers.BeCode ");
            script.AppendLine($"AND ISNULL(src.{nameof(RentalOS.RentContractNumber)},N'') <> N'' AND src.{nameof(RentalOS.RentContractNumber)} = ers.{nameof(EstateRegistration.ERContractNumber)} ");
            script.AppendLine();

            //сопоставление колонок

            return script.ToString();
        }


        /// <summary>
        /// Формирует скрипт удаления строк из источника данных, содержащих ошибки.
        /// </summary>
        /// <returns>Далее, на импорт пойдут только строки без ошибок в данных.</returns>
        private string DeleteErrRowsScript()
        {
            var script = new StringBuilder();
            script.AppendLine($"DELETE");
            script.AppendLine($"FROM {GetTempTableName()}");
            script.AppendLine($"WHERE RowNumb IN (");
            script.AppendLine($"SELECT rowNumb");
            script.AppendLine($"FROM {errTable}");
            script.AppendLine($"WHERE errCode IS NOT NULL");
            script.AppendLine($")");

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт записи ошибок импорта в БД.
        /// </summary>
        /// <returns></returns>
        private string InsertErrorScript()
        {
            var script = new StringBuilder();
            script.AppendLine();

            script.AppendLine($"INSERT INTO {GetTableName(typeof(ImportErrorLog))}");
            script.AppendLine($"( [{nameof(ImportErrorLog.RowNumber)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.ImportHistoryID)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.ErrorText)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.ErrorCode)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.ErrorType)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.MessageDate)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.EusiNumber)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.InventoryNumber)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.Oid)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.IsHistory)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.CreateDate)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.ActualDate)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.SortOrder)}]");
            script.AppendLine($", [{nameof(ImportErrorLog.Hidden)}] )");
            script.AppendLine($"SELECT");
            script.AppendLine($"  [rowNumb]");
            script.AppendLine($", @HistoryID");
            script.AppendLine($", [text]");
            script.AppendLine($", [errCode]");
            script.AppendLine($", N'{ImportExtention.GetErrorTypeName(ErrorType.System)}'");
            script.AppendLine($", GETDATE()");
            script.AppendLine($", [eusiNumber]");
            script.AppendLine($", [invNumber]");
            script.AppendLine($", NEWID()");
            script.AppendLine($", 0");
            script.AppendLine($", GETDATE()");
            script.AppendLine($", GETDATE()");
            script.AppendLine($", 0");
            script.AppendLine($", 0 ");
            script.AppendLine($"FROM {errTable}");
            script.AppendLine($"WHERE errCode IS NOT NULL");

            script.AppendLine();

            return script.ToString();
        }

        /// <summary>
        /// Формирует текст скриптов проверок для импортируемых данных.
        /// </summary>
        /// <returns></returns>
        private string DataCheckScript()
        {
            var script = new StringBuilder();
            //script.AppendLine(CheckEusiNumberAndOriginalOSCard());
            script.AppendLine();

            //несоответствие бизнес-правилам
            script.AppendLine();
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'{ErrorTypeName.BreaksBusinessRules}'");
            script.AppendLine($", N'ERR_BP_Rent'");
            script.AppendLine($", N'ERR_BP_Rent'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE src.{colRule} IS NULL");
            script.AppendLine($"AND src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL)");
            script.AppendLine();


            return script.ToString();
        }

        /// <summary>
        /// Формирует текст скрипта комплексной проверки наличия первоначальной карточки ОС.
        /// </summary>       
        /// <returns></returns>
        private string CheckEusiNumberAndOriginalOSCard()
        {
            var script = new StringBuilder();

            //проверка номера ЕУСИ
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'№ЕУСИ не существует'");
            script.AppendLine($", N'ERR_ROW_Rent'");
            script.AppendLine($", N'IMRent01'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE ");
            script.AppendLine($"NOT (src.{colOg2} IS NOT NULL AND src.{colOg2IsConnected} = 0 AND src.{colOg1} IS NOT NULL AND src.{colOg1IsConnected} = 0 AND src.{colTransactionKindCode} IN (N'100', N'300'))");
            script.AppendLine($"AND NOT");
            script.AppendLine($"(src.{colOg2IsConnected} = 0 AND src.{colOg1} IS NULL AND src.{colTransactionKindCode} IN (N'100', N'300'))");
            script.AppendLine($"AND ISNULL(src.{nameof(RentalOS.EUSINumber)},0) <> 0");
            script.AppendLine($"AND NOT EXISTS (");
            script.AppendLine($"SELECT est.ID");
            script.AppendLine($"FROM {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} est");
            script.AppendLine($"WHERE est.Hidden = 0 AND est.IsHistory = 0");
            script.AppendLine($"AND src.{nameof(RentalOS.EUSINumber)} = est.{nameof(CorpProp.Entities.Estate.Estate.Number)}");
            script.AppendLine($")");


            //проверка карточки ОС
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'Первоначальная карточка ОС не определена'");
            script.AppendLine($", N'ERR_ROW_Rent'");
            script.AppendLine($", N'IMRent02'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE ");
            script.AppendLine($"NOT (src.{colOg2} IS NOT NULL AND src.{colOg2IsConnected} = 0 AND src.{colOg1} IS NOT NULL AND src.{colOg1IsConnected} = 0 AND src.{colTransactionKindCode} IN (N'100', N'300'))");
            script.AppendLine($"AND NOT");
            script.AppendLine($"(src.{colOg2IsConnected} = 0 AND src.{colOg1} IS NULL AND src.{colTransactionKindCode} IN (N'100', N'300'))");
            script.AppendLine($"AND src.{colOg1} IS NOT NULL AND src.{colOg1IsConnected} = 1");
            script.AppendLine($"AND src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL )");
            script.AppendLine($"AND NOT EXISTS (");
            script.AppendLine($"SELECT os.ID");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} os");
            script.AppendLine($"WHERE os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($"AND src.{nameof(RentalOS.EUSINumber)} = os.{nameof(AccountingObjectExtView.Number)}");
            script.AppendLine($"AND os.{nameof(AccountingObjectExtView.ConsolidationCode)} = src.{colOg1BECode}");
            script.AppendLine($")");

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт идентификации ОС.
        /// </summary>
        /// <returns></returns>
        private string IdentityOSScript()
        {
            var script = new StringBuilder();

            //для всех правил кроме 10,13 и 16 ОС идентифицируется по номеру ЕУСИ+БЕ
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.{nameof(RentalOS.AccountingObjectOid)} = os.Oid");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} os ON ");
            script.AppendLine($"os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($"AND src.{nameof(RentalOS.EUSINumber)} IS NOT NULL AND src.{nameof(RentalOS.Consolidation)} IS NOT NULL");
            script.AppendLine($"AND src.{nameof(RentalOS.EUSINumber)} = os.{nameof(AccountingObjectExtView.Number)}");
            script.AppendLine($"AND src.{nameof(RentalOS.Consolidation)} = os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($"WHERE src.{colRule} IS NOT NULL AND src.{colRule} NOT IN (10,13,16)");

            //ошибки идентификации ОС для правил кроме 10,13 и 16
            script.AppendLine();
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'В Системе не найден ОС'");
            script.AppendLine($", N'ERR_OS_Rent'");
            script.AppendLine($", N'ERR_OS_Rent'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE src.{colRule} IS NOT NULL AND src.{colRule} NOT IN (10,13,16)");
            script.AppendLine($"AND src.{nameof(RentalOS.AccountingObjectOid)} IS NULL");
            script.AppendLine($"AND src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL)");

            //идентификация для правила 10
            //поиск ОИ для правила 10
            script.AppendLine();
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'В Системе не найден ОИ с номером ЕУСИ <' + CAST(src.{nameof(RentalOS.EUSINumber)} AS NVARCHAR(MAX)) + N'>' + src.{colFindERVGP}");
            script.AppendLine($", N'ERR_OI_Rent'");
            script.AppendLine($", N'ERR_OI_Rent'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE src.{colRule} = 10");
            script.AppendLine($"AND src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL)");
            script.AppendLine($"AND ISNULL(src.{nameof(RentalOS.EUSINumber)},0) <> 0");
            script.AppendLine($"AND NOT EXISTS (");
            script.AppendLine($"SELECT est.ID");
            script.AppendLine($"FROM {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} est");
            script.AppendLine($"WHERE est.Hidden = 0 AND est.IsHistory = 0");
            script.AppendLine($"AND src.{nameof(RentalOS.EUSINumber)} = est.{nameof(CorpProp.Entities.Estate.Estate.Number)}");
            script.AppendLine($")");


            //поиск ОС для ОГ-1 для правила 10
            script.AppendLine();
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'В Системе не найден ОС для ОГ 1 с номером ЕУСИ <' + CAST(src.{nameof(RentalOS.EUSINumber)} AS NVARCHAR(MAX)) + N'> и кодом БЕ <' + src.{colOg1BECode} + N'>' + src.{colFindERVGP}");
            script.AppendLine($", N'ERR_OS1_Rent'");
            script.AppendLine($", N'ERR_OS1_Rent'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE src.{colRule} = 10");
            script.AppendLine($"AND src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL)");
            script.AppendLine($"AND ISNULL(src.{colOg1BECode},N'') <> N''");
            script.AppendLine($"AND NOT EXISTS (");
            script.AppendLine($"SELECT os.ID");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} os");
            script.AppendLine($"WHERE os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($"AND ISNULL(src.{nameof(RentalOS.EUSINumber)},0) <> 0 AND src.{nameof(RentalOS.EUSINumber)} = os.{nameof(AccountingObjectExtView.Number)}");
            script.AppendLine($"AND src.{colOg1BECode} = os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($")");


            //поиск ОС для ОГ-2 для правила 10
            script.AppendLine();
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'В Системе не найден ОС для ОГ 2 с номером ЕУСИ <' + CAST(src.{nameof(RentalOS.EUSINumber)} AS NVARCHAR(MAX)) + N'> и кодом БЕ <' + src.{nameof(RentalOS.Consolidation)} + N'>' + src.{colFindERVGP}");
            script.AppendLine($", N'ERR_OS1_Rent'");
            script.AppendLine($", N'ERR_OS1_Rent'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE src.{colRule} = 10");
            script.AppendLine($"AND src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL)");
            script.AppendLine($"AND ISNULL(src.{colOg1BECode},N'') <> N''");
            script.AppendLine($"AND NOT EXISTS (");
            script.AppendLine($"SELECT os.ID");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} os");
            script.AppendLine($"WHERE os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($"AND ISNULL(src.{nameof(RentalOS.EUSINumber)},0) <> 0 AND src.{nameof(RentalOS.EUSINumber)} = os.{nameof(AccountingObjectExtView.Number)}");
            script.AppendLine($"AND src.{nameof(RentalOS.Consolidation)} = os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($")");

            //идентификация ОС для правила 10
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.{nameof(RentalOS.AccountingObjectOid)} = os.Oid");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} os ON ");
            script.AppendLine($"os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($"AND src.{nameof(RentalOS.EUSINumber)} IS NOT NULL AND src.{nameof(RentalOS.Consolidation)} IS NOT NULL");
            script.AppendLine($"AND src.{nameof(RentalOS.EUSINumber)} = os.{nameof(AccountingObjectExtView.Number)}");
            script.AppendLine($"AND src.{nameof(RentalOS.Consolidation)} = os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($"WHERE src.{colRule} = 10");

            //идентификация по правилу 13 для ОС ОГ-1           
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.{colOS1Oid} = os.Oid");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} os ON ");
            script.AppendLine($"os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($"AND ISNULL(src.{nameof(RentalOS.InventoryNumber)},N'') <> N''");
            script.AppendLine($"AND src.{nameof(RentalOS.InventoryNumber)} = os.{nameof(AccountingObjectExtView.InventoryNumber)}");
            script.AppendLine($"AND ISNULL(src.{colOg1BECode},N'') <> N'' AND src.{colOg1BECode} = os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($"WHERE ISNULL(src.{colRule},0) = 13 AND src.{colOS1Oid} IS NULL");

            //идентификация по правилам 13 и 16 для ОС ОГ-2    
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.{nameof(RentalOS.AccountingObjectOid)} = os.Oid");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} os ON ");
            script.AppendLine($"os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($"AND ISNULL(src.{nameof(RentalOS.InventoryNumber)},N'') <> N''");
            script.AppendLine($"AND src.{nameof(RentalOS.InventoryNumber)} = os.{nameof(AccountingObjectExtView.InventoryNumber)}");
            script.AppendLine($"AND src.{nameof(RentalOS.Consolidation)} = os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($"WHERE ISNULL(src.{colRule},0) IN (13,16) AND src.{nameof(RentalOS.AccountingObjectOid)} IS NULL");


            //непонятные ошибки 
            script.AppendLine();
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'В Системе не идентифицирован ОС с номером ЕУСИ <' + CAST(src.{nameof(RentalOS.EUSINumber)} AS NVARCHAR(MAX)) + N'> и кодом БЕ <' + src.{nameof(RentalOS.Consolidation)} + N'>'");
            script.AppendLine($", N'ERR_OSID_Rent'");
            script.AppendLine($", N'ERR_OSID_Rent'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE ISNULL(src.{colRule},0) NOT IN (13,16)");
            script.AppendLine($"AND src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL)");
            script.AppendLine($"AND src.{nameof(RentalOS.AccountingObjectOid)} IS NULL");
            script.AppendLine();

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт идентификации данных аренды.
        /// </summary>
        /// <returns></returns>
        private string IdentityRentalScript()
        {
            var script = new StringBuilder();

            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($"src.Oid = trg.Oid");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(RentalOS))} trg ");
            script.AppendLine($"ON trg.Hidden = 0 AND trg.IsHistory = 0");
            script.AppendLine($"AND src.{nameof(RentalOS.AccountingObjectOid)} = trg.{nameof(RentalOS.AccountingObjectOid)}");
            script.AppendLine();

            //после идентификации нужно получить код статуса аренды для дальнейших проверок
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($"src.{colStateRentCode} = st.[Code]");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"LEFT JOIN ");            
            script.AppendLine($"{GetTableName(typeof(RentalOS))} rent WITH (NOLOCK)");
            script.AppendLine($"ON rent.Hidden = 0 AND rent.{nameof(RentalOS.AccountingObjectOid)} = src.{nameof(RentalOS.AccountingObjectOid)}");           
            script.AppendLine($"INNER JOIN ");
            script.AppendLine($"(");
            script.AppendLine($"SELECT rr.Oid, MAX(rr.[ActualDate]) AS [MaxActualDate]");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} rr WITH (NOLOCK)");
            script.AppendLine($"WHERE rr.Hidden = 0 AND rr.[ActualDate] <= {varStartPeriod}");
            script.AppendLine($"GROUP BY rr.Oid");
            script.AppendLine($") gr");
            script.AppendLine($"ON rent.Hidden = 0 AND gr.Oid = rent.Oid AND gr.[MaxActualDate] = rent.[ActualDate]");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} st");
            script.AppendLine($"ON st.ID = rent.{nameof(RentalOS.StateObjectRentID)}");
            script.AppendLine($"");
            script.AppendLine();

            //Проверка статуса на соответствие значению "В аренде"
            //для правил 2,3,5,6,8,9,11,12,14,15,17,18
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", N'Состояние ОС/НМА(аренда) не соответствует значению <В аренде>. Значение вида операции <' + src.{colTransactionKindCode} + N'> недопустимо'");
            script.AppendLine($", N'ERR_State_Rent'");
            script.AppendLine($", N'ERR_State_Rent'");
            script.AppendLine($", src.{nameof(RentalOS.InventoryNumber)}");
            script.AppendLine($", src.{nameof(RentalOS.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE ");
            script.AppendLine($"src.RowNumb NOT IN (SELECT rowNumb FROM {errTable} WHERE errCode IS NOT NULL)");
            script.AppendLine($"AND ISNULL(src.{colRule},0) IN (2,3,5,6,8,9,11,12,14,15,17,18)");
            script.AppendLine($"AND UPPER(ISNULL(src.{colStateRentCode},N'')) <> N'INRENT'");
            script.AppendLine();

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт импорта данных ИР-Аренда.
        /// </summary>
        /// <returns></returns>
        private string ImportDataScript()
        {
            var script = new StringBuilder();                        

            //обработка версионности ОС
            script.AppendLine(CreateVersionsOS());

            //создание ОС по правилам 13 и 16
            script.AppendLine(CreateOSByRules_13_16());

            //обновляем сссылки на ИД ОС в источнике
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET src.{nameof(RentalOS.AccountingObjectID)} = map.OSID");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {varOSUpd} map ON src.RowNumb = map.RowID ");
            script.AppendLine($"AND src.{nameof(RentalOS.AccountingObjectOid)} = map.Oid");

            //обработка версионности Аренды
            script.AppendLine(CreateVersionsRent());

            
            //обновление ОС по правилам кроме 13,16
            script.AppendLine(UpdateOS_By_ManyRules());                      

            //обновление ОС по правилам 13 и 16
            script.AppendLine(UpdateOS_By_13_16());

            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипты обработки версий ОС.
        /// </summary>
        /// <returns></returns>
        private string CreateVersionsOS()
        {
            var script = new StringBuilder();

            script.AppendLine($"----VERSIONS-OS-----------------------------------------");

            //ActualDate = StartPeriod            
            script.AppendLine($"INSERT INTO {varOSUpd} (RowID, OSID, Oid, Action)");
            script.AppendLine($"SELECT source.RowNumb, target.ID, target.Oid, 0");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON (source.{nameof(RentalOS.AccountingObjectOid)} = target.{nameof(AccountingObjectExtView.Oid)} OR source.{colOS1Oid} = target.{nameof(AccountingObjectExtView.Oid)})");
            script.AppendLine($"AND target.Hidden = 0");           
            script.AppendLine($"AND target.ActualDate = {varStartPeriod}");
           
            // ActualDate < StartPeriod 
            script.AppendLine(CreatePutStoryOS());

            // ActualDate > StartPeriod 
            script.AppendLine(CreateOldVersionOS());

            script.AppendLine($"----END-VERSIONS-OS-------------------------------------");
            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт откладывания текущей версии ОС в историю.
        /// </summary>
        /// <returns></returns>
        private string CreatePutStoryOS()
        {
            var script = new StringBuilder();

            script.AppendLine($"INSERT INTO {varOSUpd} (RowID, OSID, Oid, Action)");
            script.AppendLine($"SELECT source.RowNumb, target.ID, target.Oid, 1");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON (source.{nameof(RentalOS.AccountingObjectOid)} = target.{nameof(AccountingObjectExtView.Oid)} OR source.{colOS1Oid} = target.{nameof(AccountingObjectExtView.Oid)})");           
            script.AppendLine($"AND target.IsHistory = 0 AND target.Hidden = 0");
            script.AppendLine($"AND target.ActualDate < {varStartPeriod}");
            script.AppendLine($"WHERE target.Oid NOT IN (");
            script.AppendLine($"SELECT Oid");
            script.AppendLine($"FROM {varOSUpd}");
            script.AppendLine($")");

            script.AppendLine($"INSERT INTO {GetTableName(typeof(AccountingObjectTbl))} ({GetInsertColumnSpecificationOS()})");
            script.AppendLine($"SELECT {GetPutStoryOSSelect("target")}");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {varOSUpd} osids ON target.ID = osids.OSID AND osids.Action = 1");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON (source.{nameof(RentalOS.AccountingObjectOid)} = target.{nameof(AccountingObjectExtView.Oid)} OR source.{colOS1Oid} = target.{nameof(AccountingObjectExtView.Oid)})");

            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует текст скрипта создания истории ОС.
        /// </summary>
        /// <returns></returns>
        private string CreateOldVersionOS()
        {
            var script = new StringBuilder();

            //ActualDate > StartPeriod

            script.AppendLine($"INSERT INTO {varOSUpd} (RowID, OSID, Oid, Action)");
            script.AppendLine($"SELECT source.RowNumb, target.ID, target.Oid, 2");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON (source.{nameof(RentalOS.AccountingObjectOid)} = target.{nameof(AccountingObjectExtView.Oid)} OR source.{colOS1Oid} = target.{nameof(AccountingObjectExtView.Oid)})");
            script.AppendLine($"AND target.IsHistory = 0 AND target.Hidden = 0");
            script.AppendLine($"AND UPPER(ISNULL(target.{nameof(AccountingObjectExtView.StateObjectRSBUCode)},N'')) IN (N'DRAFT', N'OUTBUS')");
            script.AppendLine($"AND target.ActualDate > {varStartPeriod}");
            script.AppendLine($"WHERE target.Oid NOT IN (");
            script.AppendLine($"SELECT Oid");
            script.AppendLine($"FROM {varOSUpd}");
            script.AppendLine($")");
            script.AppendLine();

            script.AppendLine($"INSERT INTO {GetTableName(typeof(AccountingObjectTbl))} (");
            script.AppendLine($"{String.Join(",", osRequiredColumns.Select(s => s.Key))}");
            script.AppendLine($",[{nameof(AccountingObjectTbl.ConsolidationID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.Oid)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.EstateID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.ActualDate)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.NonActualDate)}]");
            script.AppendLine($")");
            script.AppendLine($"OUTPUT NULL, inserted.ID, inserted.Oid ,3 INTO {varOSUpd}");
            script.AppendLine($"SELECT");
            script.AppendLine($"{String.Join(",", osRequiredColumns.Select(s => (s.Key == $"[{nameof(TypeObject.IsHistory)}]")? "1" : s.Value))} ");
            script.AppendLine($", {GetDictValueSelect(typeof(Consolidation), $"{varOg2BECode}")}");
            script.AppendLine($", target.[Oid]");
            script.AppendLine($", target.[EstateID]");
            script.AppendLine($", {varStartPeriod}");
            script.AppendLine($", {varEndPeriod}");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON (source.{nameof(RentalOS.AccountingObjectOid)} = target.{nameof(AccountingObjectExtView.Oid)} OR source.{colOS1Oid} = target.{nameof(AccountingObjectExtView.Oid)})");
            script.AppendLine($"AND target.IsHistory = 0 AND target.Hidden = 0");
            script.AppendLine($"AND UPPER(ISNULL(target.{nameof(AccountingObjectExtView.StateObjectRSBUCode)},N'')) NOT IN (N'DRAFT', N'OUTBUS')");
            script.AppendLine($"AND target.ActualDate > {varStartPeriod}");
            script.AppendLine($"WHERE target.Oid NOT IN (");
            script.AppendLine($"SELECT Oid");
            script.AppendLine($"FROM {varOSUpd}");
            script.AppendLine($")");
            script.AppendLine();

            script.AppendLine($"UPDATE map");
            script.AppendLine($"SET map.RowID = src.RowNumb");
            script.AppendLine($"FROM {varOSUpd} map");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectTbl))} os");
            script.AppendLine($"ON map.Oid = os.Oid AND os.Hidden = 0 AND os.ActualDate = {varStartPeriod}");
            script.AppendLine($"AND map.RowID IS NULL AND map.[Action] = 3");
            script.AppendLine($"INNER JOIN {GetTempTableName()} src ON (os.Oid = src.{nameof(RentalOS.AccountingObjectOid)} OR (src.{colOS1Oid} IS NOT NULL AND os.Oid = src.{colOS1Oid}))");


            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт обработки версионности Аренды.
        /// </summary>
        /// <returns></returns>
        private string CreateVersionsRent()
        {
            var script = new StringBuilder();

            script.AppendLine($"----VERSIONS-RENT-----------------------------------------");

            //ActualDate = StartPeriod            
            script.AppendLine($"INSERT INTO {varRentUpd} (RowID, ID, Oid, Action)");
            script.AppendLine($"SELECT source.RowNumb, target.ID, target.Oid, 0");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON source.Oid = target.Oid ");
            script.AppendLine($"AND target.Hidden = 0");
            script.AppendLine($"AND target.ActualDate = {varStartPeriod}");

            // ActualDate < StartPeriod 
            script.AppendLine($"INSERT INTO {varRentUpd} (RowID, ID, Oid, Action)");
            script.AppendLine($"SELECT source.RowNumb, target.ID, target.Oid, 1");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON target.Oid = source.Oid");
            script.AppendLine($"AND target.IsHistory = 0 AND target.Hidden = 0");
            script.AppendLine($"AND target.ActualDate < {varStartPeriod}");
            script.AppendLine($"WHERE target.Oid NOT IN (");
            script.AppendLine($"SELECT Oid");
            script.AppendLine($"FROM {varRentUpd}");
            script.AppendLine($")");

            script.AppendLine($"INSERT INTO {GetTableName(typeof(RentalOS))} ({GetInsertColumnSpecificationRent()})");
            script.AppendLine($"SELECT {GetPutStoryRentSelect("target")}");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {varRentUpd} rr ON target.ID = rr.ID AND rr.Action = 1");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON source.Oid = target.Oid ");
            script.AppendLine();

            script.AppendLine($"UPDATE target");
            script.AppendLine($"SET ");
            script.AppendLine($"  target.[{nameof(RentalOS.ActualDate)}] = {varStartPeriod}");
            script.AppendLine($", target.[{nameof(RentalOS.NonActualDate)}] = {varEndPeriod}");
            script.AppendLine($", target.[{nameof(RentalOS.ImportDate)}] = GETDATE()");
            script.AppendLine($", target.[{nameof(RentalOS.ImportUpdateDate)}] = GETDATE()");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} target ");
            script.AppendLine($"INNER JOIN {varRentUpd} rr ON target.ID = rr.ID AND rr.Action = 1");
            script.AppendLine();

            //END ActualDate < StartPeriod 

            // ActualDate > StartPeriod 
            script.AppendLine($"INSERT INTO {GetTableName(typeof(RentalOS))} ({GetInsertColumnSpecificationRent()})");           
            script.AppendLine($"OUTPUT NULL, inserted.ID, inserted.Oid, 3 INTO {varRentUpd}");
            script.AppendLine($"SELECT {GetOldVersionRentSelect()}");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} target WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source");
            script.AppendLine($"ON source.Oid = target.Oid");
            script.AppendLine($"AND target.IsHistory = 0 AND target.Hidden = 0");
            script.AppendLine($"AND target.ActualDate > {varStartPeriod}");
            script.AppendLine($"WHERE target.Oid NOT IN (");
            script.AppendLine($"SELECT Oid");
            script.AppendLine($"FROM {varRentUpd}");
            script.AppendLine($")");
            script.AppendLine();

            script.AppendLine($"UPDATE map");
            script.AppendLine($"SET ");
            script.AppendLine($"map.RowID = src.RowNumb");
            script.AppendLine($"FROM {varRentUpd} map");
            script.AppendLine($"INNER JOIN {GetTempTableName()} src");
            script.AppendLine($"ON src.Oid = map.Oid");
            script.AppendLine($"AND map.RowID IS NULL");
            script.AppendLine($"AND map.[Action] = 3");
            script.AppendLine();

            //END ActualDate > StartPeriod 


            //Обновление данных ИР-Аренда, установка целевого статуса
            script.AppendLine($"UPDATE target");
            script.AppendLine($"SET {GetUpdateRentSelect()}");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} target");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source WITH (NOLOCK)");
            script.AppendLine($"ON target.Oid = source.Oid AND target.[ActualDate] = {varStartPeriod}");
            script.AppendLine($"WHERE source.Oid IS NOT NULL");
            script.AppendLine();


            //Insert new rows
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET [Oid] = NEWID()");
            script.AppendLine($"WHERE [Oid] IS NULL");
            script.AppendLine();
            script.AppendLine($"INSERT INTO {GetTableName(typeof(RentalOS))} ({GetInsertColumnSpecificationRent()})");
            script.AppendLine($"OUTPUT NULL, inserted.ID, inserted.Oid, 4 INTO {varRentUpd}");
            script.AppendLine($"SELECT {GetNewRentSelect()}");
            script.AppendLine($"FROM {GetTempTableName()} source WITH (NOLOCK)");
            script.AppendLine($"WHERE source.Oid NOT IN (");
            script.AppendLine($"SELECT Oid");
            script.AppendLine($"FROM {varRentUpd}");
            script.AppendLine($")");
            script.AppendLine();
            script.AppendLine($"UPDATE map");
            script.AppendLine($"SET ");
            script.AppendLine($"map.RowID = src.RowNumb");
            script.AppendLine($"FROM {varRentUpd} map");
            script.AppendLine($"INNER JOIN {GetTempTableName()} src");
            script.AppendLine($"ON src.Oid = map.Oid");
            script.AppendLine($"AND map.RowID IS NULL");
            script.AppendLine($"AND map.[Action] = 4");
            script.AppendLine();


            //фиксируем что создали, что обновили в ImportObject
            string selectIO = String.Join(",", importObjectColumns.Select(s => 
            {
                if (s.Key == $"[{nameof(ImportObject.ImportHistoryOid)}]")
                    return $"{varHistoryOid}";
                else if (s.Key == $"[Entity_ID]")
                    return $"ID";
                else if (s.Key == $"[Entity_TypeName]")
                    return $"N'{typeof(RentalOS).GetTypeName()}'";
                else if (s.Key == $"[{nameof(ImportObject.Type)}]")
                    return $"CASE WHEN MAX(ISNULL(Action,0)) IN (3,4) THEN 1 ELSE 2 END ";
                if (s.Key == $"[{nameof(ImportObject.ActualDate)}]" || s.Key == $"[{nameof(ImportObject.CreateDate)}]")
                    return $"GETDATE()";
                return s.Value;
            }));            
            script.AppendLine($"INSERT INTO {GetTableName(typeof(ImportObject))}");
            script.AppendLine($"( {String.Join(",", importObjectColumns.Select(s => s.Key))} )");
            script.AppendLine($"SELECT ");
            script.AppendLine($"{selectIO} ");
            script.AppendLine($"FROM {varRentUpd}");
            script.AppendLine($"GROUP BY ID");
            script.AppendLine();

            script.AppendLine($"----END-VERSIONS-RENT-------------------------------------");
            

            //сопоставление атрибутов ИР-аренда и ОС
            //для случаев, когда уже существует ОС
            script.AppendLine(CompareScript());


            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт обработки импорта по бизнес-правилам кроме 13,16.
        /// </summary>
        /// <returns></returns>
        private string UpdateOS_By_ManyRules()
        {
            var script = new StringBuilder();

            //Обновление ОС-2
            script.AppendLine($"UPDATE os");
            script.AppendLine($"SET");
            script.AppendLine($"  os.[{nameof(AccountingObjectTbl.ActRentDate)}] = rent.[{nameof(RentalOS.ActRentDate)}]");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.NameByDoc)}] = CASE WHEN ISNULL(os.[{nameof(AccountingObjectTbl.NameByDoc)}],N'') = N'' THEN rent.[{nameof(RentalOS.NameByDoc)}] ELSE os.[{nameof(AccountingObjectTbl.NameByDoc)}] END");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.CadastralNumber)}] = CASE WHEN ISNULL(os.[{nameof(AccountingObjectTbl.CadastralNumber)}],N'') = N'' THEN rent.[{nameof(RentalOS.CadastralNumber)}] ELSE os.[{nameof(AccountingObjectTbl.CadastralNumber)}] END");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.CadastralValue)}] = CASE WHEN ISNULL(os.[{nameof(AccountingObjectTbl.CadastralValue)}],0) = 0 THEN ISNULL(rent.[{nameof(RentalOS.CadastralValue)}],0) ELSE os.[{nameof(AccountingObjectTbl.CadastralValue)}] END");           
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectTbl))} os");
            script.AppendLine($"INNER JOIN {GetTempTableName()} src ON src.{nameof(RentalOS.AccountingObjectID)} = os.ID AND src.{colRule} NOT IN (13,16)");
            script.AppendLine($"LEFT JOIN {varRentUpd} rr ON src.RowNumb = rr.RowID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(RentalOS))} rent ON rr.ID = rent.ID");
            script.AppendLine();

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт сопоставления атрибутов ИР-Аренда с ОС и заоплнения колонки Comments.
        /// </summary>
        /// <returns></returns>
        private string CompareScript()
        {
            var script = new StringBuilder();
            var comm = $"Найдены расхождения по следующим полям: {System.Environment.NewLine}";
            var format = @"{0} (значение ФСД: {1}, значение БУС: {2}), " + System.Environment.NewLine;

            script.AppendLine($"DECLARE @Compares TABLE(ID INT");
            script.AppendLine($", OKOF2014 NVARCHAR(MAX)");
            script.AppendLine($", DepreciationGroup NVARCHAR(MAX)");
            script.AppendLine($", ProprietorSubject NVARCHAR(MAX)");
            script.AppendLine($", RentContractNumber NVARCHAR(MAX)");
            script.AppendLine($", LandPurpose NVARCHAR(MAX)");
            script.AppendLine($", Deposit NVARCHAR(MAX)");
            script.AppendLine($");");
            script.AppendLine();

            script.AppendLine($"INSERT @Compares");
            script.AppendLine($"SELECT ");
            script.AppendLine($"  rent.ID ");
            script.AppendLine($", (CASE WHEN (ISNULL(rentOKOF.Code,N'')+N' '+ISNULL(rentOKOF.Name,N'')) <> (ISNULL(osOKOF.Code,N'')+N' '+ISNULL(osOKOF.Name,N'')) THEN N'{String.Format(format, "ОКОФ", "'+ (ISNULL(rentOKOF.Code,N'')+N' '+ISNULL(rentOKOF.Name,N'')) + N'", "'+ (ISNULL(osOKOF.Code,N'')+N' '+ISNULL(osOKOF.Name,N'')) + N'")}' ELSE NULL END)");
            script.AppendLine($", (CASE WHEN ISNULL(rentDEPR.Name,N'') <> ISNULL(osDEPR.Name,N'') THEN N'{String.Format(format, "Амортизационная группа", "'+ ISNULL(rentDEPR.Name,N'') + N'", "'+ ISNULL(osDEPR.Name,N'') + N'")}' ELSE NULL END)");
            script.AppendLine($", (CASE WHEN ISNULL(rentSUBJ.SDP,N'') <> ISNULL(osSUBJ.SDP,N'') THEN N'{String.Format(format, "Контрагент (арендодатель)", "'+ ISNULL(rentSUBJ.SDP,N'') + N'", "'+ ISNULL(osSUBJ.SDP,N'') + N'")}' ELSE NULL END)");
            script.AppendLine($", (CASE WHEN ISNULL(rent.RentContractNumber,N'') <> ISNULL(os.RentContractNumber,N'') THEN N'{String.Format(format, "Уникальный номер договора Компании", "'+ ISNULL(rent.RentContractNumber,N'') + N'", "'+ ISNULL(os.RentContractNumber,N'') + N'")}' ELSE NULL END)");
            script.AppendLine($", (CASE WHEN ISNULL(rentPURP.Name,N'') <> ISNULL(osPURP.Name,N'') THEN N'{String.Format(format, "Назначение ЗУ", "'+ ISNULL(rentPURP.Name,N'') + N'", "'+ ISNULL(osPURP.Name,N'') + N'")}' ELSE NULL END)");
            script.AppendLine($", (CASE WHEN ISNULL(rentDEP.Name,N'') <> ISNULL(osDEP.Name,N'') THEN N'{String.Format(format, "Месторождение", "'+ ISNULL(rentDEP.Name,N'') + N'", "'+ ISNULL(osDEP.Name,N'') + N'")}' ELSE NULL END)");           
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {varRentUpd} rr ON src.RowNumb = rr.RowID AND src.{nameof(RentalOS.AccountingObjectOid)} IS NOT NULL");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(RentalOS))} rent WITH (NOLOCK) ON rr.ID = rent.ID");            
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} rentOKOF ON rentOKOF.ID = rent.{nameof(RentalOS.OKOF2014ID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} rentDEPR ON rentDEPR.ID = rent.{nameof(RentalOS.DepreciationGroupID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(Subject))} rentSUBJ ON rentSUBJ.ID = rent.{nameof(RentalOS.ProprietorSubjectID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} rentPURP ON rentPURP.ID = rent.{nameof(RentalOS.LandPurposeID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} rentDEP ON rentDEP.ID = rent.{nameof(RentalOS.DepositID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(AccountingObject))} os WITH (NOLOCK) ON rent.{nameof(RentalOS.AccountingObjectID)} = os.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} osOKOF ON osOKOF.ID = os.{nameof(AccountingObject.OKOF2014ID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} osDEPR ON osDEPR.ID = os.{nameof(AccountingObject.DepreciationGroupID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(Subject))} osSUBJ ON osSUBJ.ID = os.{nameof(AccountingObject.ProprietorSubjectID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} osPURP ON osPURP.ID = os.{nameof(AccountingObject.LandPurposeID)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} osDEP ON osDEP.ID = os.{nameof(AccountingObject.DepositID)}");


            script.AppendLine($"SELECT * FROM @Compares");

            script.AppendLine();
            script.AppendLine($"UPDATE rent");
            script.AppendLine($"SET");
            script.AppendLine($"rent.[{nameof(RentalOS.Comments)}] = ");
            script.AppendLine($"N'{comm}'");
            script.AppendLine($"+ ISNULL(com.OKOF2014, N'')");
            script.AppendLine($"+ ISNULL(com.DepreciationGroup, N'')");
            script.AppendLine($"+ ISNULL(com.ProprietorSubject, N'')");
            script.AppendLine($"+ ISNULL(com.RentContractNumber, N'')");
            script.AppendLine($"+ ISNULL(com.LandPurpose, N'')");
            script.AppendLine($"+ ISNULL(com.Deposit, N'')");
            script.AppendLine($"FROM {GetTableName(typeof(RentalOS))} rent");
            script.AppendLine($"INNER JOIN @Compares com ON rent.ID = com.ID");            
            script.AppendLine($"WHERE com.OKOF2014 IS NOT NULL ");
            script.AppendLine($"OR com.DepreciationGroup IS NOT NULL");
            script.AppendLine($"OR com.ProprietorSubject IS NOT NULL");
            script.AppendLine($"OR com.RentContractNumber IS NOT NULL");
            script.AppendLine($"OR com.LandPurpose IS NOT NULL");
            script.AppendLine($"OR com.Deposit IS NOT NULL");

            script.AppendLine();

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт обновления ОС по бизнес-правилам 13 и 16.
        /// </summary>
        /// <returns></returns>
        private string UpdateOS_By_13_16()
        {
            var script = new StringBuilder();            

            //Обновление существующего ОС-2 или ОС-1
            script.AppendLine($"UPDATE os");
            script.AppendLine($"SET");
            script.AppendLine($"  os.[{nameof(AccountingObjectTbl.ActRentDate)}] = rent.[{nameof(RentalOS.ActRentDate)}]");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.NameByDoc)}] = CASE WHEN ISNULL(os.[{nameof(AccountingObjectTbl.NameByDoc)}],N'') = N'' THEN rent.[{nameof(RentalOS.NameByDoc)}] ELSE os.[{nameof(AccountingObjectTbl.NameByDoc)}] END");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.CadastralNumber)}] = CASE WHEN ISNULL(os.[{nameof(AccountingObjectTbl.CadastralNumber)}],N'') = N'' THEN rent.[{nameof(RentalOS.CadastralNumber)}] ELSE os.[{nameof(AccountingObjectTbl.CadastralNumber)}] END");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.CadastralValue)}] = CASE WHEN ISNULL(os.[{nameof(AccountingObjectTbl.CadastralValue)}],0) = 0 THEN ISNULL(rent.[{nameof(RentalOS.CadastralValue)}],0) ELSE os.[{nameof(AccountingObjectTbl.CadastralValue)}] END");            
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.ProprietorSubjectID)}] = CASE WHEN (ISNULL(src.{colRule},0) = 13 AND os.[Oid] = src.{colOS1Oid}) THEN rent.[{nameof(RentalOS.ProprietorSubjectID)}] ELSE os.[{nameof(AccountingObjectTbl.ProprietorSubjectID)}] END");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.InventoryNumber)}] = rent.[{nameof(RentalOS.InventoryNumber)}]");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.OKOF2014ID)}] = rent.[{nameof(RentalOS.OKOF2014ID)}]");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.DepreciationGroupID)}] = rent.[{nameof(RentalOS.DepreciationGroupID)}]");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.Useful)}] = TRY_PARSE(rent.[{nameof(RentalOS.Useful)}] AS INT)");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.RentContractNumber)}] = rent.[{nameof(RentalOS.RentContractNumber)}]");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.LandPurposeID)}] = rent.[{nameof(RentalOS.LandPurposeID)}]");
            script.AppendLine($", os.[{nameof(AccountingObjectTbl.DepositID)}] = rent.[{nameof(RentalOS.DepositID)}]");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectTbl))} os");
            script.AppendLine($"INNER JOIN {GetTempTableName()} src ON (src.{nameof(RentalOS.AccountingObjectID)} = os.ID AND ISNULL(src.{colRule},0) IN (13,16)) ");
            script.AppendLine($"OR (os.[Hidden] = 0 AND src.{colOS1Oid} = os.Oid AND os.[ActualDate] = {varStartPeriod} AND ISNULL(src.{colRule},0) = 13)");
            script.AppendLine($"LEFT JOIN {varRentUpd} rr ON src.RowNumb = rr.RowID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(RentalOS))} rent ON rr.ID = rent.ID");
                        
            
            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт создания ОС1 и ОС2 и ОИ для правил 13 и 16.
        /// </summary>
        /// <returns></returns>
        private string CreateOSByRules_13_16()
        {
            var script = new StringBuilder();
            script.AppendLine($"----CREATE-OS-BY-RULES-13-16----------------------------");

            //создание ОИ
            script.AppendLine(CreateNewEstate());
            script.AppendLine();

            //создание ОС для ОГ-1
            script.AppendLine(CreateNewOS1());
            script.AppendLine();

            //создание ОС для ОГ-2
            script.AppendLine(CreateNewOS2());
            script.AppendLine();
            
            script.AppendLine($"----END-CREATE-OS-BY-RULES-13-16-------------------------");
            script.AppendLine();
            
            return script.ToString();
        }
        /// <summary>
        /// Формирует скрипт создания новых ОИ для бизнес-правил 13 и 16.
        /// </summary>
        /// <returns></returns>
        private string CreateNewEstate()
        {
            var script = new StringBuilder();
            script.AppendLine($"----CREATE-ESTATE-BY-RULES-13-16----------------------------");
           
            //обновление ИД ОИ во временной таблице
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($"src.[{colEstateID}] = os.[EstateID]");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectTbl))} os WITH (NOLOCK)");
            script.AppendLine($"ON (os.[Oid] = src.[{nameof(RentalOS.AccountingObjectOid)}] AND os.Hidden = 0) ");
            script.AppendLine($"OR (src.[{nameof(RentalOS.AccountingObjectID)}] IS NULL AND os.[Hidden] = 0 AND os.[Oid] = src.[{colOS1Oid}])");
            script.AppendLine();

            //таблица мэпинга с ОИ
            script.AppendLine($"CREATE TABLE #tblTypeEstateMap (RowNumb INT, oidEstate UNIQUEIDENTIFIER, typeEstate NVARCHAR(255))");
            script.AppendLine();

            //если заполнены поля ЗУ, то это ЗУ, в остальном смотрим по мэппингу ОКОФ
            script.AppendLine($"INSERT #tblTypeEstateMap");
            script.AppendLine($"SELECT");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", NEWID()");            
            script.AppendLine($", CASE");
            script.AppendLine($"WHEN (LTRIM(RTRIM(ISNULL(src.{nameof(RentalOS.LandPurpose)},N''))) <> N'' OR src.{nameof(RentalOS.UsefulEndLand)} IS NOT NULL OR LTRIM(RTRIM(ISNULL(src.{nameof(RentalOS.InventoryArendaLand)},N''))) <> N'') THEN N'Land'");
            script.AppendLine($"WHEN LTRIM(RTRIM(ISNULL(src.{nameof(RentalOS.OKOF2014)},N''))) <> N'' THEN ");            
            script.AppendLine($"(");
            script.AppendLine($"SELECT TOP 1 okofMap.[EstateType]");
            script.AppendLine($"FROM {GetTableName(typeof(CorpProp.Entities.Mapping.OKOFEstates))} okofMap");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} okof ON okofMap.Hidden = 0 AND okofMap.[OKOF2014_ID] =  okof.ID");
            script.AppendLine($"WHERE okof.Code = LTRIM(RTRIM(ISNULL(src.{nameof(RentalOS.OKOF2014)},N'')))");
            script.AppendLine($")");
            script.AppendLine($"ELSE N'{nameof(InventoryObject)}'");
            script.AppendLine($"");
            script.AppendLine($"END");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE src.{colEstateID} IS NULL AND src.{colRule} IN (13,16)");
            script.AppendLine();

            //создание ОИ
            script.AppendLine(new BulkHelper().CreateEstateScript("#tblTypeEstateMap", varStartPeriod));
            script.AppendLine();

            //обновление созданных ОИ
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.Estate.NameByDoc)} = src.[{nameof(RentalOS.NameByDoc)}]");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.NameEUSI)} = src.[{nameof(RentalOS.NameByDoc)}]");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.InventoryNumber)} = src.[{nameof(RentalOS.InventoryNumber)}]");
            script.AppendLine($"FROM {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} est");
            script.AppendLine($"INNER JOIN #tblTypeEstateMap map ON est.hidden = 0 AND est.IsHistory = 0 AND est.Oid = map.oidEstate");
            script.AppendLine($"INNER JOIN {GetTempTableName()} src ON map.RowNumb = src.RowNumb");

            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($"src.{colEstateID} = est.ID");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN #tblTypeEstateMap map ON src.RowNumb = map.RowNumb");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} est ON est.Hidden = 0 AND est.IsHistory = 0 And est.Oid = map.oidEstate");
            script.AppendLine();

           

            script.AppendLine($"DROP TABLE #tblTypeEstateMap");
            script.AppendLine();

            script.AppendLine($"----END-CREATE-ESTATE-BY-RULES-13-16-------------------------");
            script.AppendLine();
            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт создания новых ОС ОГ-1 для бизнес-правила 13.
        /// </summary>
        /// <returns></returns>
        private string CreateNewOS1()
        {
            var script = new StringBuilder(); 
            //таблица мэпинга с ОС1
            script.AppendLine($"CREATE TABLE #tblOS1Map (RowNumb INT, Oid UNIQUEIDENTIFIER, EstateID INT)");
            script.AppendLine();

            script.AppendLine($"INSERT INTO #tblOS1Map");
            script.AppendLine($"SELECT ");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", NEWID()");
            script.AppendLine($", src.[{colEstateID}]");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE ISNULL(src.[{colRule}],0) = 13 AND src.{colOS1Oid} IS NULL");
            script.AppendLine();

            script.AppendLine($"INSERT INTO {GetTableName(typeof(AccountingObjectTbl))} (");
            script.AppendLine($"{String.Join(",", osRequiredColumns.Select(s => s.Key))}");
            script.AppendLine($",[{nameof(AccountingObjectTbl.StateObjectRSBUID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.ConsolidationID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.Oid)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.EstateID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.ActualDate)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.NonActualDate)}]");
            script.AppendLine($")");
            script.AppendLine($"SELECT");
            script.AppendLine($"{String.Join(",", osRequiredColumns.Select(s => s.Value))} ");
            script.AppendLine($", {GetDictValueSelect(typeof(StateObjectRSBU), "N'Draft'")}");
            script.AppendLine($", {GetDictValueSelect(typeof(Consolidation), $"source.[{colOg1BECode}]")}");
            script.AppendLine($", src.[Oid]");
            script.AppendLine($", src.[EstateID]");
            script.AppendLine($", {varStartPeriod}");
            script.AppendLine($", {varEndPeriod}");
            script.AppendLine($"FROM #tblOS1Map src");
            script.AppendLine($"INNER JOIN {GetTempTableName()} source ON src.[RowNumb] = source.[RowNumb]");
            script.AppendLine();


            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($" src.[{colOS1Oid}] = os.Oid");
            script.AppendLine($", src.[{nameof(RentalOS.AccountingObjectID)}] = os.ID");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN #tblOS1Map map ON src.RowNumb = map.RowNumb");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectTbl))} os ON map.Oid = os.Oid");
            script.AppendLine();

            script.AppendLine($"DROP TABLE #tblOS1Map");
            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт создания новых ОС ОГ-2 для бизнес-правил 13 и 16.
        /// </summary>
        /// <returns></returns>
        private string CreateNewOS2()
        {
            var script = new StringBuilder();
            //таблица мэпинга с ОС2
            script.AppendLine($"CREATE TABLE #tblOS2Map (RowNumb INT, Oid UNIQUEIDENTIFIER, EstateID INT)");
            script.AppendLine();

            script.AppendLine($"INSERT INTO #tblOS2Map");
            script.AppendLine($"SELECT ");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", NEWID()");
            script.AppendLine($", src.[{colEstateID}]");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"WHERE ISNULL(src.[{colRule}],0) IN (13,16) AND src.{nameof(RentalOS.AccountingObjectOid)} IS NULL");
            script.AppendLine();

            script.AppendLine($"INSERT INTO {GetTableName(typeof(AccountingObjectTbl))} (");
            script.AppendLine($"{String.Join(",", osRequiredColumns.Select(s => s.Key))}");
            script.AppendLine($",[{nameof(AccountingObjectTbl.StateObjectRSBUID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.ConsolidationID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.Oid)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.EstateID)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.ActualDate)}]");
            script.AppendLine($",[{nameof(AccountingObjectTbl.NonActualDate)}]");
            script.AppendLine($")");
            script.AppendLine($"SELECT");
            script.AppendLine($"{String.Join(",", osRequiredColumns.Select(s => s.Value))} ");
            script.AppendLine($", {GetDictValueSelect(typeof(StateObjectRSBU), "N'Draft'")}");
            script.AppendLine($", {GetDictValueSelect(typeof(Consolidation), $"{varOg2BECode}")}");
            script.AppendLine($", src.[Oid]");
            script.AppendLine($", src.[EstateID]");
            script.AppendLine($", {varStartPeriod}");
            script.AppendLine($", {varEndPeriod}");
            script.AppendLine($"FROM #tblOS2Map src");
            script.AppendLine();


            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($"  src.[{nameof(RentalOS.AccountingObjectOid)}] = os.Oid");
            script.AppendLine($", src.[{nameof(RentalOS.AccountingObjectID)}] = os.ID");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN #tblOS2Map map ON src.RowNumb = map.RowNumb");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectTbl))} os ON map.Oid = os.Oid");
            script.AppendLine();

            script.AppendLine($"DROP TABLE #tblOS2Map");
            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Возвращает строку с разделителем столбцов для вставки, включая обязательные колонки для таблицы ОС.
        /// </summary>
        /// <returns></returns>
        string GetInsertColumnSpecificationOS()
        {
            var insertColumns = osColumnDefinition.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);
            return String.Join(",\r\n", insertColumns.Select(x => $"[{x}]").ToArray());
        }
        /// <summary>
        /// Возвращает строку с разделителем столбцов для вставки, включая обязательные колонки для таблицы ИР-Аренды
        /// </summary>
        /// <returns></returns>
        string GetInsertColumnSpecificationRent()
        {
            var insertColumns = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);
            return String.Join(",\r\n", insertColumns.Select(x => $"[{x}]").ToArray());
        }
       
        /// <summary>
        /// Формирует текст выборки для отложения текущей версии ОС в историю.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string GetPutStoryOSSelect(string prefix)
        {
            var insertColumns = osColumnDefinition.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);
            var p = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}.";

            return String.Join(",\r\n", insertColumns.Select(x =>
            {
                if (x == "IsHistory")
                {
                    return "1";
                }
                if (x == "NonActualDate")
                {
                    return $"ISNULL({p}{x}, DATEADD(day , -1 , {startPeriod}))";
                }
                return string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]";
            }).ToArray());
        }

        /// <summary>
        /// Формирует текст выборки для отложения текущей версии аренды в историю.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string GetPutStoryRentSelect(string prefix)
        {
            var insertColumns = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);
            var p = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}.";

            return String.Join(",\r\n", insertColumns.Select(x =>
            {
                if (x == "IsHistory")
                {
                    return "1";
                }
                if (x == "NonActualDate")
                {
                    return $"ISNULL({p}{x}, DATEADD(day , -1 , {startPeriod}))";
                }
                return string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]";
            }).ToArray());
        }


        /// <summary>
        /// Формирует текст выборки для вставки новой историчной записи аренды.
        /// </summary>
        /// <returns></returns>
        private string GetOldVersionRentSelect()
        {
            var val = new StringBuilder();
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "actualdate")
                {
                    return $"{startPeriod}";
                }
                if (x.ColumnName.ToLower() == "nonactualdate")
                {
                    return $"{endPeriod}";
                }
                if (x.ColumnName.ToLower() == "ishistory")
                {
                    return $"1";
                }
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate")
                {
                    return $"GetDate()";
                }
                if (x.ColumnName.ToLower() == (nameof(RentalOS.StateObjectRentID).ToLower()))
                {
                    return $"{GetDictValueSelect(typeof(StateObjectRent), $"source.[{colNewStateRentCode}]")}";
                }
                return x.IsNullable ? $"source.[{x.ColumnName}]" : $"ISNULL(source.[{x.ColumnName}],{GetSqlDefaultValue(x)})";
            });

            var setSpecification = new StringBuilder();

            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }
                var prop = MainType.GetProperty(colName);

                if (IsPrimitiveType(prop.PropertyType))
                {
                    if (values.ContainsKey(colName))
                    {
                        values[colName] = $"source.[{colName}]";
                    }
                }
                else
                {
                    var idColName = $"{colName}ID";                   
                    if (values.ContainsKey(idColName))
                    {
                        values[idColName] = $"{GetSelectSubQuery(prop, colName)}";
                    }
                }
            }
            foreach (var sqlExpression in values)
            {
                if (string.IsNullOrEmpty(sqlExpression.Value))
                {
                    continue;
                }
                if (setSpecification.Length > 0)
                {
                    setSpecification.AppendLine(",");
                }
                setSpecification.Append(sqlExpression.Value);
            }

            return setSpecification.ToString();
        }

        /// <summary>
        /// Формирует текст инструкции обновления записи аренды с установкой целевого статуса.
        /// </summary>
        /// <returns></returns>
        private string GetUpdateRentSelect()
        {        
            var setSpecification = new StringBuilder();
            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }

                var prop = MainType.GetProperty(colName);

                if (setSpecification.Length > 0)
                {
                    setSpecification.Append(",");
                }
                var idColName = "";               
                if (IsPrimitiveType(prop.PropertyType))
                {
                    setSpecification.AppendLine($" target.[{colName}] = source.[{colName}]");
                }
                else
                {
                    
                    idColName = $"{colName}ID";
                    if (colName == nameof(RentalOS.StateObjectRent))
                        setSpecification.AppendLine($" target.[{idColName}] = {GetSelectSubQuery(prop, colNewStateRentCode)}");
                    else
                        setSpecification.AppendLine($" target.[{idColName}] = {GetSelectSubQuery(prop, colName)}");
                }
            }
            setSpecification.AppendLine($",target.ImportUpdateDate = GETDATE()");
            setSpecification.AppendLine($",target.ActualDate = {varStartPeriod}");
            setSpecification.AppendLine($",target.NonActualDate = {varEndPeriod}");
            setSpecification.AppendLine($",target.[{nameof(RentalOS.AccountingObjectID)}] = source.[{nameof(RentalOS.AccountingObjectID)}]");
            if (!ColsNameMapping.ContainsValue(nameof(RentalOS.Comments)))
                setSpecification.AppendLine($",target.[{nameof(RentalOS.Comments)}] = source.[{nameof(RentalOS.Comments)}]");

            if (!ColsNameMapping.ContainsValue(nameof(RentalOS.StateObjectRent)) && !ColsNameMapping.ContainsValue(nameof(RentalOS.StateObjectRentID)))
                setSpecification.AppendLine($",target.[{nameof(RentalOS.StateObjectRentID)}] = {GetDictValueSelect(typeof(StateObjectRent), $"source.[{colNewStateRentCode}]")}");
            
            return setSpecification.ToString();
        }

        /// <summary>
        /// Формирует текст инструкции выборки для вставки новых данных об аренде.
        /// </summary>
        /// <returns></returns>
        private string GetNewRentSelect(string prefix = null)
        {
            var val = new StringBuilder();
            var p = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}.";
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "actualdate")
                {
                    return $"{startPeriod}";
                }
                if (x.ColumnName.ToLower() == "nonactualdate")
                {
                    return $"{endPeriod}";
                }
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate")
                {
                    return $"GetDate()";
                }
                if (x.ColumnName.ToLower() == (nameof(RentalOS.StateObjectRentID).ToLower()))
                {
                    return $"{GetDictValueSelect(typeof(StateObjectRent), $"source.[{colNewStateRentCode}]")}";
                }
                return x.IsNullable ? $"{p}[{x.ColumnName}]" : $"ISNULL({p}[{x.ColumnName}],{GetSqlDefaultValue(x)})";
            });

            var setSpecification = new StringBuilder();

            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }
                var prop = MainType.GetProperty(colName);

                if (IsPrimitiveType(prop.PropertyType))
                {
                    if (values.ContainsKey(colName))
                    {
                        values[colName] = $"{p}[{colName}]";
                    }
                }
                else
                {
                    var idColName = $"{colName}ID";
                    if (values.ContainsKey(idColName))
                    {
                        values[idColName] = $"{GetSelectSubQuery(prop, colName)}";
                    }
                }
            }
            foreach (var sqlExpression in values)
            {
                if (string.IsNullOrEmpty(sqlExpression.Value))
                {
                    continue;
                }
                if (setSpecification.Length > 0)
                {
                    setSpecification.AppendLine(",");
                }
                setSpecification.Append(sqlExpression.Value);
            }

            return setSpecification.ToString();
        }

        /// <summary>
        /// Возвращает словарь колонок и значений для вставки в ImportObject.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetImportObjectDict()
        {
            return GetSqlColumnDefinition(typeof(ImportObject))
                .Where(f => f.ColumnName != "ID")
                .OrderBy(sort => sort.ColumnName)
                .ToDictionary(x => $"[{x.ColumnName}]", x =>
                {
                    if (!x.IsNullable)
                        return GetSqlDefaultValue(x);
                    else
                        return "NULL";
                });
        }
    }

}