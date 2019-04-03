using Base.DAL;
using Base.Entities.Complex;
using Base.Service;
using Base.Service.Log;
using Base.UI.Service;
using CorpProp;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Accounting;
using CorpProp.Services.Base;
using CorpProp.Services.Import;
using CorpProp.Services.Import.BulkMerge;
using EUSI.Entities.Accounting;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using EUSI.Helpers;
using EUSI.Services.Monitor;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace EUSI.Services.Accounting
{
    public interface IAccountingMovingService : ITypeObjectService<AccountingMoving>, IExcelImportEntity, IExcelImportChecker
    {
    }

    public class AccountingMovingService : TypeObjectService<AccountingMoving>, IAccountingMovingService
    {
        private readonly ILogService _logger;
        private IAccountingObjectService _accountingObjectService;
        private IImportHistoryService _importHistoryService;

        private readonly MonitorReportingImportService monitor = new MonitorReportingImportService();
               
        public AccountingMovingService(
            IBaseObjectServiceFacade facade
            , IAccountingObjectService accountingObjectService
            , IImportHistoryService importHistoryService, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _accountingObjectService = accountingObjectService;
            _importHistoryService = importHistoryService;
        }

        public IAccountingObjectService AccountingObjectService
        {
            get
            {
                return _accountingObjectService;
            }
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
        public void Import(
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

                checker.StartDataCheck(uofw, histUofw, table, typeof(AccountingMoving), ref history);

                if (!CheckColumnExists(colsNameMapping, ref history))
                {
                    monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
                    return;
                }

                var version = new AccountingMovingVersionControl<AccountingMoving>(uofw, table, colsNameMapping, history.Period.Value, ref history);
                var type = typeof(AccountingMoving);

                var queryBuilder = new MovingImportQueryBuilder(colsNameMapping, type, version, histUofw);
                var bulkMergeManager = new BulkMergeManager(colsNameMapping, type, ref history, queryBuilder);
                bulkMergeManager.BulkInsertAll(table, type, colsNameMapping, ref count);

                if (!history.ImportErrorLogs.Any())
                    monitor.CreateMonitorReportingForSuccessImport(history, count, histUofw);
                else
                    monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
                monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }
        }

        /// <summary>
        /// Проверка наличия обязательных колонок в шаблоне.
        /// </summary>
        /// <param name="colsNameMapping"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public bool CheckColumnExists(
              Dictionary<string, string> colsNameMapping
            , ref ImportHistory history)
        {           

            if (!colsNameMapping.ContainsValue(nameof(AccountingMoving.ExternalID)))
                history.ImportErrorLogs.AddError($"Файл не соответствует шаблону: отсуствует колонка <Номер записи>");
            if (!colsNameMapping.ContainsValue(nameof(AccountingMoving.InventoryNumber)))
                history.ImportErrorLogs.AddError($"Файл не соответствует шаблону: отсуствует колонка <Инвентарный номер>");
            if (!colsNameMapping.ContainsValue(nameof(AccountingMoving.Angle)))
                history.ImportErrorLogs.AddError($"Файл не соответствует шаблону: отсуствует колонка <Ракурс>");
            if (!colsNameMapping.ContainsValue(nameof(AccountingMoving.Date)))
                history.ImportErrorLogs.AddError($"Файл не соответствует шаблону: отсуствует колонка <Дата проводки>");
            if (!colsNameMapping.ContainsValue(nameof(AccountingMoving.Amount)))
                history.ImportErrorLogs.AddError($"Файл не соответствует шаблону: отсуствует колонка <Сумма>");
            if (!colsNameMapping.ContainsValue(nameof(AccountingMoving.MovingType)))
                history.ImportErrorLogs.AddError($"Файл не соответствует шаблону: отсуствует колонка <Вид движения>");
            if (!colsNameMapping.ContainsValue(nameof(AccountingMoving.EUSINumber)))
                history.ImportErrorLogs.AddError($"Файл не соответствует шаблону: отсуствует колонка <Номер ЕУСИ>");

            return !(!colsNameMapping.ContainsValue(nameof(AccountingMoving.ExternalID)) ||
                !colsNameMapping.ContainsValue(nameof(AccountingMoving.InventoryNumber)) ||
                !colsNameMapping.ContainsValue(nameof(AccountingMoving.Angle)) ||
                !colsNameMapping.ContainsValue(nameof(AccountingMoving.Amount)) ||
                !colsNameMapping.ContainsValue(nameof(AccountingMoving.Amount)) ||
                !colsNameMapping.ContainsValue(nameof(AccountingMoving.MovingType)) ||
                !colsNameMapping.ContainsValue(nameof(AccountingMoving.EUSINumber)));           

        }


        /// <summary>
        /// Ищет или создает запись МСФО на основании записи РСБУ.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="rsbu"></param>
        private void FindOrCreateMSFO(IUnitOfWork uow, AccountingMoving rsbu, FileCardAndAccountingMoving link)
        {
            var id = rsbu.ExternalID + "_";
            var msfo = uow.GetRepository<AccountingMoving>()
                .Filter(f =>
                !f.Hidden
                && !f.IsHistory
                && f.ExternalID == id
                && f.InventoryNumber == rsbu.InventoryNumber
                && f.Consolidation != null
                && f.Consolidation.Code == rsbu.Consolidation.Code
                && f.Period.Start == rsbu.Period.Start
                && f.Period.End == rsbu.Period.End
                ).FirstOrDefault();

            if (msfo == null)
                this.CreateMSFO(uow, rsbu, link);
            else
            {
                msfo.AccountingObject = rsbu.AccountingObject;
                msfo.Date = rsbu.Date;
                msfo.InventoryNumber = rsbu.InventoryNumber;
                msfo.Consolidation = rsbu.Consolidation;
                msfo.Amount = rsbu.Amount;
                msfo.Period = rsbu.Period;
                msfo.LoadType = rsbu.LoadType;
                msfo.MovingType = rsbu.MovingType;
                msfo.EUSI = rsbu.EUSI;
                msfo.ImportUpdateDate = DateTime.Now;                
                AccountingMovingHelper.ClearFileLinks(uow, msfo);
                if (link != null)
                    uow.GetRepository<FileCardAndAccountingMoving>()
                        .Create(new FileCardAndAccountingMoving()
                        {
                            ObjLeft = link.ObjLeft,
                            ObjRigth = msfo
                        });

                this.Update(uow, msfo);
            }
        }

        /// <summary>
        /// Создает запись МСФО на основании записи РСБУ.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="rsbu"></param>
        private void CreateMSFO(IUnitOfWork uow, AccountingMoving rsbu, FileCardAndAccountingMoving link)
        {
            //Укладываем РСБУ в МСФО
            var msfo = new AccountingMoving();
            msfo.Angle = uow.GetRepository<Angle>().Filter(f => !f.Hidden && !f.IsHistory && f.Code == "MSFO").FirstOrDefault();
            msfo.AccountingObject = rsbu.AccountingObject;
            msfo.ExternalID = rsbu.ExternalID + "_";
            msfo.Date = rsbu.Date;
            msfo.InventoryNumber = rsbu.InventoryNumber;
            msfo.Consolidation = rsbu.Consolidation;
            msfo.Amount = rsbu.Amount;
            msfo.Period = rsbu.Period;
            msfo.LoadType = rsbu.LoadType;
            msfo.MovingType = rsbu.MovingType;
            msfo.EUSI = rsbu.EUSI;
            msfo.InRSBU = true;
            msfo.ImportDate = DateTime.Now;
            msfo.ActualDate = rsbu.ActualDate;
            msfo.NonActualDate = rsbu.NonActualDate;
            if (link != null)
                uow.GetRepository<FileCardAndAccountingMoving>()
                        .Create(new FileCardAndAccountingMoving()
                        {
                            ObjLeft = link.ObjLeft,
                            ObjRigth = msfo
                        });
            this.Create(uow, msfo);
        }

        public void CancelImport(
            IUnitOfWork uofw
           , ref ImportHistory history
           )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Переопределяет получение актуальных данных на заданную дату.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="date">Дата.</param>
        /// <returns></returns>
        public override IQueryable<AccountingMoving> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            if (!date.HasValue)            
                return base.GetAll(uow, false).Where(f => !f.IsHistory);            

            var q = base.GetAll(uow, false);

            var groups = q
                .Where(x => x.Period.Start <= date.Value)
                .GroupBy(gr => gr.Oid).Select(s => new { Oid = s.Key, MaxImportDate = s.Max(m => m.ImportDate) });

            var items = q.Join(groups, e => e.Oid, o => o.Oid, (e, o) => new { e, o })
                .Where(w => w.o.MaxImportDate == w.e.ImportDate)
                .Select(s => s.e);

            return items;
        }

        public void StartDataCheck(IUnitOfWork uofw, IUnitOfWork histUnitOfWork, DataTable table, Type type, ref ImportHistory history, bool dictCode = false)
        {
            return;
        }

        public string FormatConfirmImportMessage(List<string> fileDescriptions)
        {
            throw new NotImplementedException();
        }

        public CheckImportResult CheckVersionImport(IUiFasade uiFacade, IExcelDataReader reader, ITransactionUnitOfWork uofw, StreamReader stream, string fileName)
        {
            throw new NotImplementedException();
        }


    }

    public class MovingImportQueryBuilder : QueryBuilder
    {
        private const string RsbuCodeEng = "RSBU";
        private const string RsbuCodeRus = "РСБУ";
        private const string MsfoCodeEng = "MSFO";
        private const string MsfoCodeRus = "МСФО";
        private const string FullCodeUpCase = "FULL";
        private const string RsbuSimpleCodeUpCase = "RSBUSIMPLE";

        string errTable = $"@errorTable";
        string colOid2 = $"Oid2";
        string startPeriod = null;
        string endPeriod = null;
        string loadCode = null;
        Dictionary<string, string> fileColumns = null;
        IUnitOfWork _histUofw;


        AccountingMovingVersionControl<AccountingMoving> _version = null;

        public MovingImportQueryBuilder(
            Dictionary<string, string> colsNameMapping
            , Type type
            , AccountingMovingVersionControl<AccountingMoving> version
            , IUnitOfWork histUofw
            ) : base(colsNameMapping, type)
        {          

            startPeriod = $"'{version.StartPeriod.ToString("yyyy-MM-dd")}'";
            endPeriod = $"'{version.EndPeriod.ToString("yyyy-MM-dd")}'";
            _version = version;
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

            //TODO: Reload History!
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
            var script = new StringBuilder();
            script.AppendLine($"");
            fileColumns = GetSqlColumnDefinition(typeof(FileCard))
                .Where(f => f.ColumnName != "ID")
                .OrderBy(sort => sort.ColumnName)
                .ToDictionary(x => $"[{x.ColumnName}]", x =>
                {
                    if (!x.IsNullable)
                        return GetSqlDefaultValue(x);
                    else
                        return "NULL";
                });

            //Переменные
            loadCode = (history.IsCorrection) ? "Corrective" : "Primary";
            script.AppendLine($"DECLARE @HistoryID INT = '{history.ID}'");
            script.AppendLine($"DECLARE @BeCode NVARCHAR(MAX) = N'{history.Consolidation?.Code}'");
            script.AppendLine($"DECLARE @StartPeriod DATETIME = {startPeriod}");
            script.AppendLine($"DECLARE @EndPeriod DATETIME = {endPeriod}");
            script.AppendLine($"DECLARE @BeTypeAcc NVARCHAR(MAX) = (");
            script.AppendLine($"SELECT TOP 1 UPPER(ISNULL(tt.Code,N''))");
            script.AppendLine($"FROM {GetTableName(typeof(Consolidation))} be");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} dd ON be.ID = dd.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} tt ON be.TypeAccountingID = tt.ID");
            script.AppendLine($"WHERE dd.Code = @BeCode");            
            script.AppendLine($")");

            //таблица ошибок
            script.AppendLine($"DECLARE {errTable} TABLE(" +
                $"rowNumb INT, " +
                $"text NVARCHAR(max), " +
                $"errResultCode NVARCHAR(max), " +
                $"errCode NVARCHAR(max)," +
                $"invNumber NVARCHAR(max)," +
                $"eusiNumber NVARCHAR(max)" +
                $")");

            //Подготовка: обновление БЕ, периода, вида загрузки
            script.AppendLine(PrepareScript());

            //Проверки, валидаторы
            script.AppendLine(DataCheckScript());

            //Идентификация строк движений
            script.AppendLine(IdentityMovingScript());

            //Идентификация ОС
            script.AppendLine(IdentityOSScript());

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
            createScript.AppendLine($", [{colOid2}] UNIQUEIDENTIFIER");
            createScript.AppendLine($")");
            return createScript.ToString();
        }

        /// <summary>
        /// Формирует скрипт подготовки импортируемых данных: обновление БЕ, периода.
        /// </summary>
        /// <returns></returns>
        private string PrepareScript()
        {
            var script = new StringBuilder(); 
            
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET ");
            script.AppendLine($"  {nameof(AccountingMoving.Consolidation)} = @BeCode");
            script.AppendLine($", {nameof(AccountingMoving.EUSI)} = {nameof(AccountingMoving.EUSINumber)}");
            script.AppendLine($", {nameof(AccountingMoving.LoadTypeID)} = (");
            script.AppendLine($"SELECT TOP 1 lt.ID");
            script.AppendLine($"FROM {GetTableName(typeof(LoadType))} lt");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} dd ON lt.ID = dd.ID");
            script.AppendLine($"WHERE Hidden = 0 AND (");
            script.AppendLine($"Code = N'{loadCode}'");
            script.AppendLine($"OR Name= N'{loadCode}'");
            script.AppendLine($"OR ExternalID = N'{loadCode}')");
            script.AppendLine($")");
            script.AppendLine($", {nameof(AccountingMoving.Period)}_{nameof(Period.Start)} = @StartPeriod");
            script.AppendLine($", {nameof(AccountingMoving.Period)}_{nameof(Period.End)} = @EndPeriod");
            script.AppendLine();

            return script.ToString();
        }
                      

        /// <summary>
        /// Формирует скрипт идентификации движений.
        /// </summary>
        /// <returns></returns>
        private string IdentityMovingScript()
        {
           
            var script = new StringBuilder();
            script.AppendLine($"");

            //БЕ или Инв или Сис№ или НомерЕУСИ не заданы - ошибка
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT RowNumb" +
                $", N'ОС не идентифицирован, не задан БЕ и/или Инвентарный № и/или Системный № и/или Номер ЕУСИ'" +
                $", N'ERR_MOVID'" +
                $", N'IM_MOV03'" +
                $",[{nameof(AccountingMoving.InventoryNumber)}]" +
                $",[{nameof(AccountingMoving.EUSINumber)}]");
            script.AppendLine($"FROM {GetTempTableName()} WITH (NOLOCK) ");
            script.AppendLine($"WHERE {nameof(AccountingMoving.Consolidation)} IS NULL OR {nameof(AccountingMoving.InventoryNumber)} IS NULL");
            script.AppendLine($"OR {nameof(AccountingMoving.ExternalID)} IS NULL OR {nameof(AccountingMoving.EUSINumber)} IS NULL");
            script.AppendLine();

            //Ключ идентификации записи: Инв + БЕ + Сис№ + НомерЕУСИ + Период
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.{nameof(AccountingMoving.Oid)} = trg.{nameof(AccountingMoving.Oid)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN (");
            script.AppendLine($"SELECT mov.*, os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($", os.{nameof(AccountingObjectExtView.Number)} AS [EUSINumberFromOS]");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingMoving))} AS mov ");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os");
            script.AppendLine($"ON mov.{nameof(AccountingMoving.AccountingObjectID)} = os.[ID]");
            script.AppendLine($"WHERE mov.Hidden = 0 AND mov.IsHistory = 0");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)} = @StartPeriod");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.End)} = @EndPeriod");
            script.AppendLine($") AS trg ON ");
            script.AppendLine($" src.{nameof(AccountingMoving.Consolidation)} = trg.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.InventoryNumber)} = trg.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.ExternalID)} = trg.{nameof(AccountingMoving.ExternalID)}");
            script.AppendLine($"AND (src.{nameof(AccountingMoving.EUSINumber)} = trg.{nameof(AccountingMoving.EUSI)} OR  src.{nameof(AccountingMoving.EUSINumber)} = trg.[EUSINumberFromOS])");           
            script.AppendLine($"WHERE ");
            script.AppendLine($"src.{nameof(AccountingMoving.Consolidation)} IS NOT NULL");
            script.AppendLine($"AND src.{nameof(AccountingMoving.InventoryNumber)} IS NOT NULL");
            script.AppendLine($"AND src.{nameof(AccountingMoving.ExternalID)} IS NOT NULL");
            script.AppendLine($"AND src.{nameof(AccountingMoving.EUSINumber)} IS NOT NULL");
            script.AppendLine();

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт идентификации ОС/НМА по движениям.
        /// </summary>
        /// <returns></returns>
        private string IdentityOSScript()
        {
            var script = new StringBuilder();
            //устанавливаем связь с ОС
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET ");
            script.AppendLine($"src.[{nameof(AccountingMoving.AccountingObjectID)}] = os.[ID]");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK) ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} os");
            script.AppendLine($"ON os.[Hidden] = 0 AND os.[IsHistory] = 0");
            script.AppendLine($"AND @BeCode = os.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($"AND src.[{nameof(AccountingMoving.InventoryNumber)}] = os.[{nameof(AccountingObjectExtView.InventoryNumber)}]");
            script.AppendLine($"AND CAST(ISNULL(os.[{nameof(AccountingObjectExtView.Number)}],0) AS NVARCHAR(MAX)) = src.[{nameof(AccountingMoving.EUSINumber)}]");
            script.AppendLine($"WHERE ");
            script.AppendLine($"src.[{nameof(AccountingMoving.EUSINumber)}] IS NOT NULL");
            script.AppendLine($"AND src.[{nameof(AccountingMoving.InventoryNumber)}] IS NOT NULL");

            //Если ОС не найден - ошибка
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT ");
            script.AppendLine($"  RowNumb");
            script.AppendLine($", N'ОC с указанным номером ЕУСИ <' + src.{nameof(AccountingMoving.EUSINumber)} + N'> и инвентарным номером <'+ src.{nameof(AccountingMoving.InventoryNumber)}+N'>  не найден'");
            script.AppendLine($", N'ERR_OSID'");
            script.AppendLine($", N'IM_MOV02'");
            script.AppendLine($", src.[{nameof(AccountingMoving.InventoryNumber)}]");
            script.AppendLine($", src.[{nameof(AccountingMoving.EUSINumber)}]");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK) ");
            script.AppendLine($"WHERE ");
            script.AppendLine($"src.[{nameof(AccountingMoving.EUSINumber)}] IS NOT NULL");
            script.AppendLine($"AND src.[{nameof(AccountingMoving.InventoryNumber)}] IS NOT NULL");
            script.AppendLine($"AND src.[{nameof(AccountingMoving.AccountingObjectID)}] IS NULL");

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт по основной логике импорта данных о движениях. 
        /// </summary>
        /// <returns></returns>
        private string ImportDataScript()
        {
            var script = new StringBuilder();
            script.AppendLine();

            //обновление идентифицированных строк
            script.AppendLine(UpdateMoving());                                

            //создание новых движений
            script.AppendLine(CreateMoving());

            //создать запись МСФО на основании данных РСБУ
            script.AppendLine(CreateMSFOByRSBUScript());

            // перезапись ссылок на первичные док-ты
            if (this.ColsNameMapping.Where(x => x.Value.ToLower() == "filecardlink").Any())
                script.AppendLine(RewriteFileLinks());

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт обновления записи движения.
        /// </summary>
        /// <returns></returns>
        private string UpdateMoving()
        {
            var script = new StringBuilder();
            //откладываем текущую запись в историю
            script.AppendLine($"INSERT INTO {GetTableName(MainType)} ({GetInsertColumnSpecification()})");
            script.AppendLine($"SELECT ");
            script.AppendLine($"{GetCreateHistorySelector("trg")}");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingMoving))} trg");
            script.AppendLine($"ON src.Oid = trg.Oid AND trg.Hidden = 0 AND trg.[IsHistory] = 0");

            //обновляем текущую запись
            script.AppendLine($"UPDATE target");
            script.AppendLine($"SET ");
            script.AppendLine($"{GetUpdateSelector()}");
            script.AppendLine($"FROM {GetTempTableName()} source WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingMoving))} target");
            script.AppendLine($"ON source.Oid = target.Oid AND target.Hidden = 0 AND target.[IsHistory] = 0");

            script.AppendLine($"");
            return script.ToString();
        }

        /// <summary>
        /// Формирует текстовое описание выборки данных для откладывания записи движения в историю.
        /// </summary>
        /// <param name="prefix">Префикс.</param>
        /// <returns></returns>
        private string GetCreateHistorySelector(string prefix)
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
                    return $"@EndPeriod";
                }
                return string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]";
            }).ToArray());
        }

        /// <summary>
        /// Формирует текстовое описание выборки данных для обновления записи движения.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string GetUpdateSelector()
        {
            var sets = base.GetSetSpecification();
            sets += $", target.{nameof(AccountingMoving.ImportDate)} = GETDATE()";
            if (!ColsNameMapping.ContainsValue(nameof(AccountingMoving.LoadType)) 
                && !ColsNameMapping.ContainsValue(nameof(AccountingMoving.LoadTypeID)))
                sets += $", target.[{nameof(AccountingMoving.LoadTypeID)}] = source.[{nameof(AccountingMoving.LoadTypeID)}]";
            return sets;
        }

        /// <summary>
        /// Формирует скрипт создания новых записей движений.
        /// </summary>
        /// <returns></returns>
        private string CreateMoving()
        {
            var script = new StringBuilder();

            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET ");
            script.AppendLine($"Oid = NEWID()");
            script.AppendLine($", ID = -1");
            script.AppendLine($"WHERE Oid IS NULL");

            script.AppendLine($"INSERT INTO {GetTableName(MainType)} ({GetInsertColumnSpecification()})");
            script.AppendLine($"SELECT ");
            script.AppendLine($"{GetInsertNewSelector("source")}");
            script.AppendLine($"FROM {GetTempTableName()} source WITH (NOLOCK)");
            script.AppendLine($"WHERE");
            script.AppendLine($"source.ID = -1");


            script.AppendLine($"");
            return script.ToString();
        }

        /// <summary>
        /// Формирует текст выборки для создания новых записей движений.
        /// </summary>
        /// <returns></returns>
        private string GetInsertNewSelector(string prefix = null)
        {           
            var val = new StringBuilder();
            var p = string.IsNullOrWhiteSpace(prefix) ? string.Empty : $"{prefix}.";
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {               
                if (x.ColumnName.ToLower() == "actualdate")
                {
                    return $"@StartPeriod";
                }
                if (x.ColumnName.ToLower() == "nonactualdate")
                {
                    return $"@EndPeriod";
                }
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate" || x.ColumnName.ToLower() == "importupdatedate")
                {
                    return $"GetDate()";
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
        /// Формирует текст скрипта перезаписи ссылок на первичные документы.
        /// </summary>
        /// <returns></returns>
        private string RewriteFileLinks()
        {
            var script = new StringBuilder();
            script.AppendLine($"CREATE TABLE #tblFilesMapDel (MovingRowNumb INT, LinkID INT, FileID INT)");
            script.AppendLine();
            script.AppendLine($"CREATE TABLE #tblFilesMapCreate (MovingRowNumb INT, [FileName] NVARCHAR(MAX), NewFileOid UNIQUEIDENTIFIER)");
            script.AppendLine();
            script.AppendLine();
            script.AppendLine($"INSERT INTO #tblFilesMapDel");
            script.AppendLine($"SELECT ");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", files.ID");
            script.AppendLine($", files.[{nameof(FileCardAndAccountingMoving.ObjLeftId)}]");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingMoving))} mov WITH (NOLOCK)");
            script.AppendLine($"ON (src.[Oid] = mov.[Oid] OR src.[{colOid2}] = mov.[Oid]) AND mov.Hidden = 0 AND mov.IsHistory = 0 AND mov.[ActualDate] = @StartPeriod ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(FileCardAndAccountingMoving))} files ON mov.ID = files.ObjRigthId AND files.Hidden = 0 AND files.[ObjLeftId] = mov.[{nameof(AccountingMoving.FileCardLinkID)}]");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(FileCard))} fileCard ON files.[ObjLeftId] = fileCard.ID");
            script.AppendLine($"WHERE ISNULL(src.{nameof(AccountingMoving.FileCardLink)},N'') <> N'' AND src.{nameof(AccountingMoving.FileCardLink)} <> ISNULL(fileCard.[{nameof(FileCard.Name)}],N'')");
            script.AppendLine();
            script.AppendLine();
            script.AppendLine($"INSERT INTO #tblFilesMapCreate");
            script.AppendLine($"SELECT ");
            script.AppendLine($"  src.RowNumb");
            script.AppendLine($", src.{nameof(AccountingMoving.FileCardLink)}");            
            script.AppendLine($", NEWID()");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingMoving))} mov WITH (NOLOCK)");
            script.AppendLine($"ON src.[Oid] = mov.[Oid] AND mov.Hidden = 0 AND mov.IsHistory = 0 AND mov.[ActualDate] = @StartPeriod ");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(FileCard))} fileCard ON fileCard.ID = mov.[{nameof(AccountingMoving.FileCardLinkID)}]");
            script.AppendLine($"WHERE ISNULL(src.{nameof(AccountingMoving.FileCardLink)},N'') <> N'' AND src.{nameof(AccountingMoving.FileCardLink)} <> ISNULL(fileCard.[{nameof(FileCard.Name)}],N'')");
            script.AppendLine();
            script.AppendLine();


            //удаление старых док-тов и связей
            script.AppendLine($"UPDATE files");
            script.AppendLine($"SET ");
            script.AppendLine($"files.Hidden = 1");
            script.AppendLine($"FROM {GetTableName(typeof(FileCardAndAccountingMoving))} files ");
            script.AppendLine($"INNER JOIN  #tblFilesMapDel map ON map.[LinkID] = files.[ObjLeftId] ");
            script.AppendLine();
            script.AppendLine($"UPDATE fileCard");
            script.AppendLine($"SET ");
            script.AppendLine($"fileCard.Hidden = 1");
            script.AppendLine($"FROM {GetTableName(typeof(FileCard))} fileCard ");
            script.AppendLine($"INNER JOIN #tblFilesMapDel map ON map.[FileID] = fileCard.ID");
            script.AppendLine();


            var fileSelect = String.Join(",", fileColumns.Select(s => {
                if (s.Key == $"[{nameof(TypeObject.Oid)}]")
                    return "src.[NewFileOid]";
                else if (s.Key == $"[{nameof(FileCard.CategoryID)}]")
                    return $"(SELECT MIN(ID) FROM {GetTableName(typeof(CardFolder))} WHERE Hidden = 0)";
                else if (s.Key == $"[{nameof(FileCard.Name)}]")
                    return $"src.[FileName]";
                else return s.Value;
            }));

            //создание новой ссылки
            script.AppendLine($"DECLARE @FileLinks TABLE(ID INT)");
            script.AppendLine($"INSERT {GetTableName(typeof(FileCard))} ");
            script.AppendLine($"({String.Join(",", fileColumns.Select(s => s.Key))})");
            script.AppendLine($"OUTPUT inserted.ID INTO @FileLinks");
            script.AppendLine($"SELECT ");
            script.AppendLine($"{fileSelect}"); 
            script.AppendLine($"FROM #tblFilesMapCreate src");
            script.AppendLine();
            script.AppendLine($"INSERT INTO {GetTableName(typeof(FileCardOne))} ([ID])");
            script.AppendLine($"SELECT ID FROM @FileLinks");
            script.AppendLine();
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($"src.[{nameof(AccountingMoving.FileCardLinkID)}] = fileCard.[ID]");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN #tblFilesMapCreate map ON src.[RowNumb] = map.[MovingRowNumb]");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(FileCard))} fileCard ON map.[NewFileOid] = fileCard.[Oid] AND fileCard.[Hidden] = 0");
            script.AppendLine();
            script.AppendLine();
            script.AppendLine($"INSERT INTO {GetTableName(typeof(FileCardAndAccountingMoving))} ([ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])");
            script.AppendLine($"SELECT ");
            script.AppendLine($"  src.[{nameof(AccountingMoving.FileCardLinkID)}]");
            script.AppendLine($", mov.[ID]");
            script.AppendLine($", 0");
            script.AppendLine($", 0");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN #tblFilesMapCreate map ON src.[RowNumb] = map.[MovingRowNumb]");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingMoving))} mov WITH (NOLOCK)");
            script.AppendLine($"ON mov.Hidden = 0 AND mov.IsHistory = 0 AND (src.Oid = mov.Oid OR src.{colOid2} = mov.Oid) AND mov.[ActualDate] = @StartPeriod");
            script.AppendLine($"");
            script.AppendLine();
            //обновление движений
            script.AppendLine($"UPDATE mov");
            script.AppendLine($"SET");
            script.AppendLine($"mov.[{nameof(AccountingMoving.FileCardLinkID)}] = src.[{nameof(AccountingMoving.FileCardLinkID)}]");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingMoving))} mov ");
            script.AppendLine($"ON (mov.Oid = src.[Oid] OR mov.[Oid] = src.[{colOid2}]) AND mov.Hidden = 0 AND mov.IsHistory = 0 AND mov.[ActualDate] = @StartPeriod");
            script.AppendLine();
            script.AppendLine($"DROP TABLE #tblFilesMapCreate");
            script.AppendLine();
            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует текст скрипта создания движений МСФО по данным РСБУ.
        /// </summary>
        /// <returns></returns>
        private string CreateMSFOByRSBUScript()
        {
            var script = new StringBuilder();
            //поиск МСФО, откладывание в историю, создание новых записей

            script.AppendLine($"IF (@BeTypeAcc = N'RSBU') ");
            script.AppendLine($"BEGIN ");
            //откладываем текущие записи МСФО в историю
            script.AppendLine($"INSERT INTO {GetTableName(typeof(AccountingMoving))} ({GetInsertColumnSpecification()})");
            script.AppendLine($"SELECT ");
            script.AppendLine($"{GetCreateHistorySelector("trg")}");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN (");
            script.AppendLine($"SELECT mov.*, be.Code AS [ConsolidationCode], angle.Code AS [AngleCode]");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingMoving))} mov");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} be ON mov.{nameof(AccountingMoving.ConsolidationID)} = be.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} angle ON mov.{nameof(AccountingMoving.AngleID)} = angle.ID");
            script.AppendLine($"WHERE mov.Hidden = 0 AND mov.IsHistory = 0");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)} = @StartPeriod");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.End)} = @EndPeriod");
            script.AppendLine($"AND UPPER(ISNULL(angle.Code,N'')) = N'MSFO'");
            script.AppendLine($"AND be.Code = @BeCode");
            script.AppendLine($") trg");
            script.AppendLine($"ON");
            script.AppendLine($"trg.{nameof(AccountingMoving.ExternalID)} = (ISNULL(src.{nameof(AccountingMoving.ExternalID)},N'')+N'_')");
            script.AppendLine($"AND trg.{nameof(AccountingMoving.InventoryNumber)} = src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($"AND (SELECT TOP 1 t2.Code FROM {GetTableName(typeof(Angle))} AS t1 INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID WHERE Hidden = 0 AND (Code = CAST(src.[Angle] as NVARCHAR(MAX)) or Name=CAST(src.[Angle] as NVARCHAR(MAX)) or ExternalID=CAST(src.[Angle] as NVARCHAR(MAX))))  = N'RSBU'");


            //обновляем актуальные записи МСФО по данным РСБУ
            script.AppendLine($"UPDATE target");
            script.AppendLine($"SET ");
            script.AppendLine($"{GetUpdateMSFOByRSBUSelector()}");
            script.AppendLine($"FROM {GetTempTableName()} source WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN (");
            script.AppendLine($"SELECT mov.*, be.Code AS [ConsolidationCode], angle.Code AS [AngleCode]");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingMoving))} mov");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} be ON mov.{nameof(AccountingMoving.ConsolidationID)} = be.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} angle ON mov.{nameof(AccountingMoving.AngleID)} = angle.ID");
            script.AppendLine($"WHERE mov.Hidden = 0 AND mov.IsHistory = 0");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)} = @StartPeriod");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.End)} = @EndPeriod");
            script.AppendLine($"AND UPPER(ISNULL(angle.Code,N'')) = N'MSFO'");
            script.AppendLine($"AND be.Code = @BeCode");
            script.AppendLine($") target");
            script.AppendLine($"ON");
            script.AppendLine($"target.{nameof(AccountingMoving.ExternalID)} = (ISNULL(source.{nameof(AccountingMoving.ExternalID)},N'')+N'_')");
            script.AppendLine($"AND target.{nameof(AccountingMoving.InventoryNumber)} = source.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($"AND (SELECT TOP 1 t2.Code FROM {GetTableName(typeof(Angle))} AS t1 INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID WHERE Hidden = 0 AND (Code = CAST(source.[Angle] as NVARCHAR(MAX)) or Name=CAST(source.[Angle] as NVARCHAR(MAX)) or ExternalID=CAST(source.[Angle] as NVARCHAR(MAX)))) = N'RSBU'");

            //создаём новые записи МСФО
            script.AppendLine($"INSERT INTO {GetTableName(typeof(AccountingMoving))} ({GetInsertColumnSpecification()})");
            script.AppendLine($"SELECT ");
            script.AppendLine($"{InsertMSFOScript("source")}");
            script.AppendLine($"FROM {GetTempTableName()} source WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN (");
            script.AppendLine($"SELECT mov.*, be.Code AS [ConsolidationCode], angle.Code AS [AngleCode]");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingMoving))} mov WITH (NOLOCK)");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} be ON mov.{nameof(AccountingMoving.ConsolidationID)} = be.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} angle ON mov.{nameof(AccountingMoving.AngleID)} = angle.ID");
            script.AppendLine($"WHERE mov.Hidden = 0 AND mov.IsHistory = 0 AND UPPER(ISNULL(angle.Code,N'')) = N'RSBU') trg");
            script.AppendLine($"ON trg.Oid = source.Oid");
            script.AppendLine($"WHERE NOT EXISTS (");
            script.AppendLine($"SELECT TOP 1 msfo.ID");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingMoving))} msfo WITH (NOLOCK)");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} be ON msfo.{nameof(AccountingMoving.ConsolidationID)} = be.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} angle ON msfo.{nameof(AccountingMoving.AngleID)} = angle.ID");
            script.AppendLine($"WHERE msfo.Hidden = 0 AND msfo.IsHistory = 0 AND UPPER(ISNULL(angle.Code,N'')) = N'MSFO'");
            script.AppendLine($"AND be.Code = trg.[ConsolidationCode] ");
            script.AppendLine($"AND msfo.{nameof(AccountingMoving.ExternalID)} = (trg.{nameof(AccountingMoving.ExternalID)} + N'_')");
            script.AppendLine($"AND msfo.{nameof(AccountingMoving.InventoryNumber)} = trg.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($"AND msfo.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)} = @StartPeriod");
            script.AppendLine($"AND msfo.{nameof(AccountingMoving.Period)}_{nameof(Period.End)} = @EndPeriod");
            script.AppendLine($")");


            //обновление ссылок на Oid МСФО
            script.AppendLine($"UPDATE src");
            script.AppendLine($"SET");
            script.AppendLine($"src.[{colOid2}] = trg.[Oid]");
            script.AppendLine($"FROM {GetTempTableName()} src");
            script.AppendLine($"INNER JOIN (");
            script.AppendLine($"SELECT mov.*, be.Code AS [ConsolidationCode], angle.Code AS [AngleCode]");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingMoving))} mov");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} be ON mov.{nameof(AccountingMoving.ConsolidationID)} = be.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} angle ON mov.{nameof(AccountingMoving.AngleID)} = angle.ID");
            script.AppendLine($"WHERE mov.Hidden = 0 AND mov.IsHistory = 0");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)} = @StartPeriod");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.End)} = @EndPeriod");
            script.AppendLine($"AND UPPER(ISNULL(angle.Code,N'')) = N'MSFO'");
            script.AppendLine($"AND be.Code = @BeCode");
            script.AppendLine($") trg");
            script.AppendLine($"ON");
            script.AppendLine($"trg.{nameof(AccountingMoving.ExternalID)} = (ISNULL(src.{nameof(AccountingMoving.ExternalID)},N'')+N'_')");
            script.AppendLine($"AND trg.{nameof(AccountingMoving.InventoryNumber)} = src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($"AND (SELECT TOP 1 t2.Code FROM {GetTableName(typeof(Angle))} AS t1 INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID WHERE Hidden = 0 AND (Code = CAST(src.[Angle] as NVARCHAR(MAX)) or Name=CAST(src.[Angle] as NVARCHAR(MAX)) or ExternalID=CAST(src.[Angle] as NVARCHAR(MAX))))  = N'RSBU'");
            script.AppendLine();

            script.AppendLine($"END ");

            script.AppendLine();
            return script.ToString();
        }

        /// <summary>
        /// Формирует текст крипта выборки данных для вставки новых записей МСФО по данным РСБУ.
        /// </summary>
        /// <returns></returns>        
        private string InsertMSFOScript(string prefix = null)
        {
            var val = new StringBuilder();
            var p = string.IsNullOrWhiteSpace(prefix) ? string.Empty : $"{prefix}.";
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName == nameof(AccountingMoving.Oid))
                {
                    return $"NEWID()";
                }
                if (x.ColumnName == nameof(AccountingMoving.InRSBU))
                {
                    return $"1";
                }
                if (x.ColumnName.ToLower() == "actualdate")
                {
                    return $"@StartPeriod";
                }
                if (x.ColumnName.ToLower() == "nonactualdate")
                {
                    return $"@EndPeriod";
                }
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate" || x.ColumnName.ToLower() == "importupdatedate")
                {
                    return $"GetDate()";
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
                    if (prop.Name == nameof(AccountingMoving.ExternalID))
                    {
                        values[colName] = $"({p}[{colName}] + N'_')";
                    }                    
                    else if (values.ContainsKey(colName))
                    {
                        values[colName] = $"{p}[{colName}]";
                    }
                }
                else
                {
                    var idColName = $"{colName}ID";
                    if (values.ContainsKey(idColName))
                    {
                        //подмена ракурса
                        if (idColName == nameof(AccountingMoving.AngleID))
                            values[idColName] = $"(SELECT TOP 1 t1.ID FROM {GetTableName(typeof(Angle))} AS t1 INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID WHERE Hidden = 0 AND UPPER(Code) = N'MSFO')";
                        else
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
        /// Формирует текст выборки обновления записей МСФО по данным РСБУ.
        /// </summary>
        /// <param name="angleCode"></param>
        /// <returns></returns>
        private string GetUpdateMSFOByRSBUSelector(string angleCode = null)
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
                if (IsPrimitiveType(prop.PropertyType))
                {
                    if (colName == nameof(AccountingMoving.ExternalID))
                        setSpecification.AppendLine($" target.[{colName}] = (source.[{colName}] + N'_')");
                    else
                        setSpecification.AppendLine($" target.[{colName}] = source.[{colName}]");
                }
                else
                {
                    var idColName = $"{colName}ID";
                    //подмена ракурса
                    if (idColName == nameof(AccountingMoving.AngleID))
                        setSpecification.AppendLine($" target.[{idColName}] = (SELECT TOP 1 t1.ID FROM {GetTableName(typeof(Angle))} AS t1 INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID WHERE Hidden = 0 AND UPPER(Code) = N'MSFO')");
                    else
                        setSpecification.AppendLine($" target.[{idColName}] = {GetSelectSubQuery(prop, colName)}");
                }
            }
            setSpecification.AppendLine($",target.ImportUpdateDate = GETDATE()");
            setSpecification.AppendLine($",target.ImportDate = GETDATE()");
            return setSpecification.ToString();
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
        /// Формирует текст скрипта проверок данных импорта движений.
        /// </summary>
        /// <returns></returns>
        private string DataCheckScript()
        {
            var script = new StringBuilder();

            //TODO: 0.Файл не соответствует шаблону: проверки наличия колонок?????

            //1. проверка на дубликаты
            script.AppendLine(CheckDuplicateScript());

            //2. Проверка даты проводки
            script.AppendLine(CheckDateMovingScript());

            //3. проверка на использования формуляра для упрощенного внедрения
            script.AppendLine(CheckFormScript());

            //4. проверка соответствия типа ведения БУ
            script.AppendLine(CheckBeTypeAccScript());

            //5. проверка наличия данных за иной период
            script.AppendLine(CheckDataOtherPeriodScript());

            return script.ToString();
        }

        /// <summary>
        /// Формирует текст скрипта проверки на дубликаты строк в источнике данных.
        /// </summary>
        /// <returns></returns>
        private string CheckDuplicateScript()
        {
            var script = new StringBuilder();
            script.AppendLine();
            
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"src.RowNumb");
            script.AppendLine($",N'Найдены дубликаты строк.'");
            script.AppendLine($",N'ERR_MOV_DUPL'");
            script.AppendLine($",N'IM_MOV_DUPL'");
            script.AppendLine($",src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($",src.{nameof(AccountingMoving.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"INNER JOIN (");
            script.AppendLine($"SELECT");
            script.AppendLine($"COUNT(RowNumb) AS [Count]");
            script.AppendLine($",{nameof(AccountingMoving.ExternalID)}");
            script.AppendLine($",{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($",{nameof(AccountingMoving.Consolidation)}");
            script.AppendLine($",{nameof(AccountingMoving.Angle)}");
            script.AppendLine($",{nameof(AccountingMoving.Date)}");
            script.AppendLine($",{nameof(AccountingMoving.Amount)}");
            script.AppendLine($",{nameof(AccountingMoving.Period)}_{nameof(Period.Start)}");
            script.AppendLine($",{nameof(AccountingMoving.Period)}_{nameof(Period.End)}");
            script.AppendLine($",{nameof(AccountingMoving.MovingType)}");
            script.AppendLine($"FROM {GetTempTableName()} WITH (NOLOCK)");
            script.AppendLine($"GROUP BY ");
            script.AppendLine($"{nameof(AccountingMoving.ExternalID)}");
            script.AppendLine($",{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($",{nameof(AccountingMoving.Consolidation)}");
            script.AppendLine($",{nameof(AccountingMoving.Angle)}");
            script.AppendLine($",{nameof(AccountingMoving.Date)}");
            script.AppendLine($",{nameof(AccountingMoving.Amount)}");
            script.AppendLine($",{nameof(AccountingMoving.Period)}_{nameof(Period.Start)}");
            script.AppendLine($",{nameof(AccountingMoving.Period)}_{nameof(Period.End)}");
            script.AppendLine($",{nameof(AccountingMoving.MovingType)}");
            script.AppendLine($"HAVING Count(RowNumb) > 1");
            script.AppendLine($") gr");
            script.AppendLine($"ON ");
            script.AppendLine($" src.{nameof(AccountingMoving.ExternalID)} = gr.{nameof(AccountingMoving.ExternalID)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.InventoryNumber)} = gr.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.Consolidation)} = gr.{nameof(AccountingMoving.Consolidation)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.Angle)} = gr.{nameof(AccountingMoving.Angle)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.Date)} = gr.{nameof(AccountingMoving.Date)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.Amount)} = gr.{nameof(AccountingMoving.Amount)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)} = gr.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.Period)}_{nameof(Period.End)} = gr.{nameof(AccountingMoving.Period)}_{nameof(Period.End)}");
            script.AppendLine($"AND src.{nameof(AccountingMoving.MovingType)} = gr.{nameof(AccountingMoving.MovingType)}");

            return script.ToString();
        }

        /// <summary>
        /// Формирует текст скрипта проверки даты проводки.
        /// </summary>
        /// <returns></returns>
        private string CheckDateMovingScript()
        {
            var script = new StringBuilder();
            script.AppendLine();

            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"src.RowNumb");
            script.AppendLine($",N'Дата проводки не соответствует отчетному периоду.'");
            script.AppendLine($",N'ERR_MOV_DATE'");
            script.AppendLine($",N'IM_MOV_DATE'");
            script.AppendLine($",src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($",src.{nameof(AccountingMoving.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"WHERE src.{nameof(AccountingMoving.Date)} NOT BETWEEN @StartPeriod AND @EndPeriod");

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт проверки на использования формуляра для упрощенного внедрения.
        /// </summary>
        /// <returns></returns>
        private string CheckFormScript()
        {
            var script = new StringBuilder();
            script.AppendLine();

            // Среди всех строк есть строки c заполненным значением «РСБУ» в поле ракурс.
            // и
            // Для БЕ установлен параметр Ведения учета «РСБУ+Упрощенное внедрение»

            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"src.RowNumb");
            script.AppendLine($",N'Для импорта данных о движениях по ракурсу РСБУ для данного ОГ, необходимо использовать формуляры для упрощенного внедрения.'");
            script.AppendLine($",N'ERR_MOV_FORM'");
            script.AppendLine($",N'IM_MOV_FORM'");
            script.AppendLine($",src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($",src.{nameof(AccountingMoving.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"WHERE @BeTypeAcc = N'{RsbuSimpleCodeUpCase}' AND UPPER(ISNULL(src.{nameof(AccountingMoving.Angle)},N'')) IN (N'{RsbuCodeRus}',N'{RsbuCodeEng}')");

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт проверки соответствия типа ведения БУ.
        /// </summary>
        /// <returns></returns>
        private string CheckBeTypeAccScript()
        {
            var script = new StringBuilder();
            script.AppendLine();
            
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"src.RowNumb");
            script.AppendLine($",N'БЕ с кодом <'+ @BeCode +N'> не ведет импортиуемый тип бухгалтерского учета в ракурсе <'+ src.Angle + N'>.'");
            script.AppendLine($",N'ERR_MOV_BeTypeAcc'");
            script.AppendLine($",N'IM_MOV_BeTypeAcc'");
            script.AppendLine($",src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($",src.{nameof(AccountingMoving.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"WHERE");
            script.AppendLine($"   (@BeTypeAcc = N'{RsbuCodeEng}' AND NOT (UPPER(ISNULL(src.{nameof(AccountingMoving.Angle)},N'')) IN (N'{RsbuCodeEng}', N'{RsbuCodeRus}')))");
            script.AppendLine($"OR (@BeTypeAcc = N'{FullCodeUpCase}' AND NOT (UPPER(ISNULL(src.{nameof(AccountingMoving.Angle)},N'')) IN (N'{RsbuCodeEng}',N'{MsfoCodeEng}',N'{RsbuCodeRus}',N'{MsfoCodeRus}')))");
            script.AppendLine($"OR (@BeTypeAcc = N'{RsbuSimpleCodeUpCase}' AND NOT (UPPER(ISNULL(src.{nameof(AccountingMoving.Angle)},N'')) IN (N'{MsfoCodeEng}',N'{MsfoCodeRus}')))");
            script.AppendLine($"OR @BeTypeAcc IS NULL");

            return script.ToString();

            /* Со стороны кода:
            //системный код ракурса
            angle = angle == RsbuCodeRus ? RsbuCodeEng : (angle == MsfoCodeRus ? MsfoCodeEng : angle);
            var typeAcc = uow.GetRepository<Consolidation>()
                .FilterAsNoTracking(f => !f.Hidden && f.Code == beCode)
                .FirstOrDefault()?.TypeAccounting?.Code?.ToUpper();
            var check = false;
            switch (typeAcc)
            {
                case "RSBU":
                    //можно только РСБУ
                    check = (angle == typeAcc);
                    break;
                case "FULL":
                    //можно РСБУ и МСФО
                    check = (angle == RsbuCodeEng || angle == MsfoCodeEng);
                    break;
                case "RSBUSIMPLE":
                    //можно МСФО и шаблоны упрощенного внедрения 
                    check = (angle == MsfoCodeEng);
                    break;
                default:
                    check = false;
                    break;
            }
            return check;
             */
        }


        /// <summary>
        /// Формирует скрипт проверки наличия данных за иной период.
        /// </summary>
        /// <returns></returns>
        private string CheckDataOtherPeriodScript()
        {
            var script = new StringBuilder();
            script.AppendLine();

            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"src.RowNumb");
            script.AppendLine($",N'Импортируемые данные содержат информацию о движениях, импортированных в иной период.'");
            script.AppendLine($",N'ERR_MOV_OtherData'");
            script.AppendLine($",N'IM_MOV_OtherData'");
            script.AppendLine($",src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($",src.{nameof(AccountingMoving.EUSINumber)}");
            script.AppendLine($"FROM {GetTempTableName()} src WITH (NOLOCK)");
            script.AppendLine($"WHERE EXISTS (");
            script.AppendLine($"SELECT mov.ID");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingMoving))} mov WITH (NOLOCK)");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} cons ON mov.{nameof(AccountingMoving.ConsolidationID)} = cons.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(AccountingObjectExtView))} os ON mov.{nameof(AccountingMoving.AccountingObjectID)} = os.ID");
           
            script.AppendLine($"WHERE mov.Hidden = 0 AND mov.IsHistory = 0");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.ExternalID)} = src.{nameof(AccountingMoving.ExternalID)}");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.InventoryNumber)} = src.{nameof(AccountingMoving.InventoryNumber)}");
            script.AppendLine($"AND cons.Code = @BeCode");
            script.AppendLine($"AND CAST(ISNULL(os.[{nameof(AccountingObjectExtView.Number)}],0) AS NVARCHAR(MAX)) = src.[{nameof(AccountingMoving.EUSINumber)}]");
            script.AppendLine($" AND (mov.{nameof(AccountingMoving.Period)}_{nameof(Period.Start)} <> @StartPeriod");
            script.AppendLine($"AND mov.{nameof(AccountingMoving.Period)}_{nameof(Period.End)} <> @EndPeriod)");
            script.AppendLine($")");

            script.AppendLine();
            return script.ToString();

           
        }
    }


}
