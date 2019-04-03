using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Enums;
using Base.Events;
using Base.Extensions;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using Base.UI.Service;
using Base.Utils.Common;
using CorpProp;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Common;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Accounting;
using CorpProp.Services.Import.BulkMerge;
using EUSI.Helpers;
using EUSI.Import.BulkMerge;
using EUSI.Model;
using EUSI.Services.Monitor;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EUSI.Services.Accounting
{
    public interface IAccountingObjectExtService : IAccountingObjectService, IConfirmImportChecker, IExcelImportChecker
    {
    }

    /// <summary>
    /// Представляет сервис по работе с ОС/НМА в ЕУСИ.
    /// </summary>
    public class AccountingObjectExtService : AccountingObjectService, IAccountingObjectExtService
    {
        private readonly MonitorReportingImportService monitor = new MonitorReportingImportService();
        private readonly ILogService _logger;

        /// <summary>
        /// Значение поля объекта ОС/НМА "Причина/способ поступления", при котором необходимо проводить другую проверку
        /// обязательных полей.
        /// </summary>
        private const string ReceiptReasonValue = "'Взятие в аренду/пользование', 'взятие в аренду/пользование'";

        private const string ColumnsNotFoundErrorText =
            "Для объектов ОС/НМА с причиной/способом поступления \"Взятие в аренду/пользование\" отсутствует обязательный столбец";

        public const int DifferenceBetweenStartAndRequiredRow = 3;

        public const string RequiredPositiveValue = "да";

        public const int ColumnNotFoundIndex = 0;

        /// <summary>
        /// Список полей, обязательных к заполнению, если у объекта ОС/НМА "Способ поступления" равен "Взятие в аренду/пользование"
        /// </summary>
        private readonly Dictionary<string, string> _fieldNamesForReceiptReason =
            new Dictionary<string, string>
            {
                { "propNameEUSINumber", nameof(AccountingObject.EUSINumber)},
                { "propNameConsolidation", nameof(AccountingObject.Consolidation)},
                { "propNamePositionConsolidation", nameof(AccountingObject.PositionConsolidation)},
                { "propNameRentContractNumber", nameof(AccountingObject.RentContractNumber)},
                { "propNameRentContractDate", nameof(AccountingObject.RentContractDate)},
                { "propNameName", nameof(AccountingObject.Name)},
                { "propNameInventoryNumber", nameof(AccountingObject.InventoryNumber)},
                { "propNameStateObjectRSBU", nameof(AccountingObject.StateObjectRSBU)},
                { "propNameAccountNumber", nameof(AccountingObject.AccountNumber)}
            };

        public AccountingObjectExtService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
            IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService, ILogService logger
            ) : base(facade, securityUserService, pathHelper, workflowService, accessService, logger)
        {
            _logger = logger;
        }

        protected override IObjectSaver<AccountingObject> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<AccountingObject> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(s => s.Consolidation)
                .SaveOneObject(s => s.LessorSubject)
                .SaveOneObject(s => s.StateObjectRSBU)
                .SaveOneObject(s => s.DepreciationMethodRSBU)
                .SaveOneObject(s => s.DepreciationGroup)
                .SaveOneObject(s => s.Deposit)
                .SaveOneObject(s => s.RentTypeRSBU)
                .SaveOneObject(s => s.WellCategory)
                .SaveOneObject(s => s.EstateMovableNSI)
                .SaveOneObject(s => s.EnergyLabel)
                .SaveOneObject(s => s.MainOwner)
                ;
        }

        public override IQueryable<AccountingObject> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            return base.GetAll(unitOfWork, hidden).Where(obu => !obu.IsArchived.HasValue || !obu.IsArchived.Value);
        }

        /// <summary>
        /// Переопределяет проверки при импорте данных.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="colsNameMapping"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public override bool DataСhecks(IUnitOfWork uofw, IUnitOfWork histUofw, DataTable table, Dictionary<string, string> colsNameMapping, ref ImportHistory history)
        {
            ImportChecker.CheckContacts(uofw, table, ref history);
            if (colsNameMapping.Values.Contains(nameof(AccountingObject.ReceiptReason)))
                CheckRequiredFields(table, ref history);
            else
                ImportChecker.CheckRequiredFields(table, typeof(AccountingObject), ref history);
            ImportChecker.CheckFieldData(table, typeof(AccountingObject), ref history, uofw, false);
            RequiredChecks(uofw, histUofw, table, colsNameMapping, ref history);
            new Validators.AccountingObjectDuplicatesValidator().Validate(table, ref history);
            return !history.ImportErrorLogs.Any();
        }

        /// <summary>
        /// Переопределяет обновление ОИ по данным ОС/НМА.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected override void UpdateEstateData(IUnitOfWork uow, AccountingObject obj, ref ImportHistory history)
        {
            var est = obj.Estate;
            if (est == null || obj.IsHistory)
            {
                return;
            }
            //UnfinishedConstruction
            est.Fill("StartDateUse", obj.StartDateUse);
            //Cadastral
            est.Fill("CadastralNumber", obj.CadastralNumber);
            //InventoryObject
            est.Fill("SibCountryID", (obj.SibCountry != null) ? obj.SibCountry.ID : obj.SibCountryID);
            est.Fill("SibFederalDistrictID", (obj.SibFederalDistrict != null) ? obj.SibFederalDistrict.ID : obj.SibFederalDistrictID);
            est.Fill("SibRegionID", (obj.Region != null) ? obj.Region.ID : obj.RegionID);
            est.Fill("SibCityNSIID", (obj.SibCityNSI != null) ? obj.SibCityNSI.ID : obj.SibCityNSIID);
            est.Fill("Address", obj.Address);
            //Vehicle
            est.Fill("RegDate", obj.VehicleRegDate, est.GetType().IsTS());
            est.Fill("YearOfIssue", obj.YearOfIssue);
            est.Fill("VehicleCategoryID", (obj.VehicleCategory != null) ? obj.VehicleCategory.ID : obj.VehicleCategoryID);
            est.Fill("DieselEngine", obj.DieselEngine);
            est.Fill("SibMeasureID", (obj.SibMeasure != null) ? obj.SibMeasure.ID : obj.SibMeasureID);
            est.Fill("Power", obj.Power);
            est.Fill("SerialNumber", obj.SerialNumber, est.GetType().IsTS());
            est.Fill("EngineSize", obj.EngineSize);
            est.Fill("VehicleModelID", (obj.Model != null) ? obj.Model.ID : obj.VehicleModelID);
            est.Fill("RegNumber", obj.VehicleRegNumber, est.GetType().IsTS());
            //Estate
            est.Fill("InventoryNumber", obj.InventoryNumber);
        }

        /// <summary>
        /// Переопределяет дополнительную бизнес/логику обработки записи импорта.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="row"></param>
        /// <param name="colsNameMapping"></param>
        /// <param name="history"></param>
        protected override void AddonLogic(IUnitOfWork uow, AccountingObject obj, DataRow row, Dictionary<string, string> colsNameMapping, ref ImportHistory history)
        {
            base.AddonLogic(uow, obj, row, colsNameMapping, ref history);

            // Задача 6748: Изменение логики обработки файла импорта ОСНМА (ОБУ) по полю ОКТМО (Поиск соответствующего значения в справочнике по полю код)
            // При импорте ОБУ при указании в файле импорта в поле ОКТМО кода ОКТМО должно 
            // производится автоматизированная связка ОБУ и ОКТМО. (поиск в справочнике по publishCode)
            if (obj.OKTMO == null && obj.OKTMOID == null)
            {
                var oktmoId = ImportHelper.GetValueByName(uow, typeof(string), row, "OKTMO", colsNameMapping);
                if (oktmoId != null && !String.IsNullOrEmpty(oktmoId.ToString()))
                {
                    var oktmo = EUSIImportHelper.GetDictByPublishCode(uow, typeof(OKTMO), oktmoId.ToString());
                    if (oktmo == null)
                    {
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "",
                            "Неверный код ОКТМО", ErrorType.System);
                        return;
                    }
                    else if (Type.Equals(oktmo, typeof(OKTMO)))
                    {
                        obj.OKTMO = (OKTMO)oktmo;
                        obj.OKTMOID = ((OKTMO)oktmo).ID;
                    }
                }
            }

            if (obj.OwnerID == null)
            {
                var error = "";
                var be = ImportHelper.GetValueByName(uow, typeof(string), row, "Consolidation", colsNameMapping);
                var society = ImportHelper.GetSocietyByConsolidationCode(uow, be.ToString(), ref error);
                if (society != null && Type.Equals(society, typeof(CorpProp.Entities.Subject.Society)))
                {
                    obj.Owner = society as CorpProp.Entities.Subject.Society;
                    obj.OwnerID = society?.ID;
                }
            }

            //Установка признака "Энергоэффективное оборудование"
            obj.Estate?.SetIsEnergy(uow);
        }
        public string FormatConfirmImportMessage(List<string> fileDescriptions)
        {
            var singleName = "БЕ";
            var pluralName = "БЕ";
            return EUSI.Common.ConfirmImportMessageFormatter.FormatConfirmImportMessage(singleName, pluralName, fileDescriptions);
        }

        public void StartDataCheck(IUnitOfWork uofw, IUnitOfWork histUow, DataTable table, Type type, ref ImportHistory history, bool dictCode = false)
        {
                   
        }

        protected void CheckRequiredFields(DataTable table, ref ImportHistory history)
        {
            Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(table);
            DataTable cleanDataTable = ImportHelper.GetDataTableWithoutHeader(table);
            int startDataRowIndex = ImportHelper.GetRowStartIndex(table);
            int systemNameRowIndex = ImportHelper.FindFieldSystemNameRow(table);
            int requiredValueRowIndex = startDataRowIndex - DifferenceBetweenStartAndRequiredRow;

            string errorType = ImportExtention.GetErrorTypeName(ErrorType.System);
            List<DataRow> enumerableTable = table.AsEnumerable().Skip(startDataRowIndex).ToList();
            DataTable shortDataTable = enumerableTable.CopyToDataTable();

            string propNameReceiptReason = nameof(AccountingObject.ReceiptReason);
            string columnNameReceiptReason = colsNameMapping.FirstOrDefault(x => x.Value == propNameReceiptReason).Key;
            int columnNumberReceiptReason = ImportHelper.GetColumnNumber(cleanDataTable, columnNameReceiptReason);

            PrepareReceiptReasonForValidation(shortDataTable, columnNumberReceiptReason);

            Dictionary<string, string> fieldNamesForRequiredAttr = new Dictionary<string, string>();
            Dictionary<string, string> requiredForRental = new Dictionary<string, string>();
            for (var c = 1; c < table.Columns.Count; c++)
            {
                if (table.Rows[requiredValueRowIndex][c].ToString().ToLower() == RequiredPositiveValue)
                {
                    var fieldName = table.Rows[systemNameRowIndex][c].ToString();
                    var propName = $"propName{fieldName}";

                    if (!fieldNamesForRequiredAttr.Keys.Contains(propName))
                    {
                        fieldNamesForRequiredAttr.Add(propName, fieldName);
                        
                    }
                    if (!requiredForRental.Keys.Contains(propName) && _fieldNamesForReceiptReason.ContainsKey($"propName{fieldName}"))
                    {
                        requiredForRental.Add(propName, fieldName);
                    }
                }
            }

            Dictionary<string, int> colNumsForRequiredAttr =
                ImportHelper.GetDictionaryOfColumnNumbers(fieldNamesForRequiredAttr, colsNameMapping, cleanDataTable, true);
            
            Dictionary<string, int> colNumsForReceiptReason = null;            

            if (columnNumberReceiptReason != ColumnNotFoundIndex)
            {
                colNumsForReceiptReason = ImportHelper.GetDictionaryOfColumnNumbers(requiredForRental,
                        colsNameMapping, cleanDataTable, true);
            }

            string standardCondition = $"NOT ({columnNameReceiptReason} IN ({ReceiptReasonValue}))";

            CheckValuesInRequiredCells(colNumsForRequiredAttr, fieldNamesForRequiredAttr,
                shortDataTable, startDataRowIndex, standardCondition, ref history);

            string receiptreasonCondition = $"{columnNameReceiptReason} IN ({ReceiptReasonValue})";
            CheckValuesInRequiredCells(colNumsForReceiptReason, requiredForRental,
                shortDataTable, startDataRowIndex, receiptreasonCondition, ref history);
        }

        private void CheckValuesInRequiredCells(Dictionary<string, int> colNums, Dictionary<string, string> colsNames,
            DataTable table, int startDataRow, string secondCondition, ref ImportHistory history)
        {
            if (colNums == null)
                return;
            foreach (var column in colNums)
            {
                var foundRows = table.Select($"Column{column.Value} IS NULL AND {secondCondition}").ToList();

                if (foundRows.Any())
                {
                    for (var r = 0; r < foundRows.Count; r++)
                    {
                        var errorRowNumber = table.Rows.IndexOf(foundRows[r]);
                        history.ImportErrorLogs.AddError(errorRowNumber + startDataRow + 1, column.Value, 
                            colsNames[column.Key], ErrorType.Required);
                    }
                }
            }
        }

        private void PrepareReceiptReasonForValidation(DataTable table, int receiptReasonColNumber)
        {           
            table.Rows.Cast<DataRow>()                               
                                .Where(r => !String.IsNullOrWhiteSpace(r[receiptReasonColNumber]?.ToString()))
                                .ToList().ForEach(r => {                                    
                                        r[receiptReasonColNumber] = r[receiptReasonColNumber].ToString().ToLower().Trim();                                    
                                });
        }

        /// <summary>
        /// Переопределяет идентификацию записи ОС/НМА при импорте.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="row"></param>
        /// <param name="colsNameMapping"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public override List<AccountingObject> FindObjects(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , ref ImportHistory history)
        {
            List<AccountingObject> list = new List<AccountingObject>();

            //читаем инв номер, ЕУСИ, БЕ      
            var invNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryNumber", colsNameMapping);
            var eusiNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "EUSINumber", colsNameMapping);
            var be = ImportHelper.GetValueByName(uofw, typeof(string), row, "Consolidation", colsNameMapping);

            //ищем в Системе ОС/НМА
            if (eusiNumb != null && be != null
                && !String.IsNullOrEmpty(eusiNumb.ToString())
                && !String.IsNullOrEmpty(be.ToString())
                )
            {
                var beCode = be.ToString();
                var numbEUSI = eusiNumb.ToString();
                var invNumber = (invNumb == null) ? "" : invNumb.ToString();

                list = (!String.IsNullOrEmpty(invNumber)) ?
                //если инвентарник заполнен
                uofw.GetRepository<AccountingObject>().Filter(x =>
                !x.Hidden &&
                !x.IsHistory &&
                x.Consolidation != null && x.Consolidation.Code == beCode
                && x.InventoryNumber == invNumber
                && x.Estate != null && x.Estate.Number.ToString() == numbEUSI)
                .Include(inc => inc.Estate)
                .ToList<AccountingObject>() :
                //если инвентарник не заполнен
                 uofw.GetRepository<AccountingObject>().Filter(x =>
                !x.Hidden &&
                !x.IsHistory &&
                x.Consolidation != null && x.Consolidation.Code == beCode
                && x.Estate != null && x.Estate.Number.ToString() == numbEUSI)
                .Include(inc => inc.Estate)
                .ToList<AccountingObject>();

                //Такие ОС/НМА не найдены, пытаемся еще раз найти какой-нибудь ОС у БЕ с таким номером ЕУСИ.
                if (list.Count == 0)
                {
                    list = uofw.GetRepository<AccountingObject>().Filter(x =>
                            !x.Hidden &&
                            !x.IsHistory &&
                            x.Consolidation != null && x.Consolidation.Code == beCode
                            && x.Estate != null && x.Estate.Number.ToString() == numbEUSI)
                            .Include(inc => inc.Estate)
                            .ToList<AccountingObject>();
                }

                if (list == null || list.Count == 0)
                {
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, ""
                        , $"В Системе не найден ОС/НМА для обновления. Проверьте значения в атрибутах: Номер ЕУСИ, БЕ, Инвентарный номер {System.Environment.NewLine}"
                        , ErrorType.System
                        , eusiNumb?.ToString()
                        , invNumb?.ToString()
                        , be?.ToString());
                }
                if (list.Count > 1)
                {
                    var err = $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row)
                        , 0
                        , ""
                        , err
                        , ErrorType.System
                        , eusiNumb?.ToString()
                        , invNumb?.ToString()
                        , be?.ToString());
                    list = null;
                }
            }
            else
            {
                history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, ""
                    , $"Невозможно идентифицировать объект по атрибутам: Номер ЕУСИ, БЕ, Инвентарный номер. {System.Environment.NewLine}"
                    , ErrorType.System
                    , eusiNumb?.ToString()
                    , invNumb?.ToString()
                    , be?.ToString());
            }

            return list;
        }

        /// <summary>
        /// Переопределяет поиск и создание ОИ при импорте состояния.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected override void FindOrCreateEstate(
            IUnitOfWork uow
            , AccountingObject obj
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , ref ImportHistory history)
        {
            //связка по номеру ЕУСИ
            var eusi = ImportHelper.GetValueByName(uow, typeof(int), row, "EUSINumber", colsNameMapping);
            if (eusi == null)
            {
                history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "",
                   "Неверный номер ЕУСИ", ErrorType.System);
                return;
            }
            var est = EUSIImportHelper.FindEstate(uow, (int)eusi, ref history);
            if (est == null)
            {
                history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "",
                   "Неверный номер ЕУСИ", ErrorType.System);
                return;
            }
            if (obj.Estate == null && obj.EstateID == null)
            {
                obj.Estate = est as CorpProp.Entities.Estate.Estate;
            }
        }

        public override AccountingObject Get(IUnitOfWork unitOfWork, int id)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(AccountingObject), TypePermission.Read);

            // Задача 10185:Реализовать автоматическое установление значания признака "За балансом" (реестр замечаний п.п. 39)
            // Обязательно прогружаем ReceiptReason, т.к. он необходим для вычисляемого поля OutOfBalance
            var obj = unitOfWork.GetRepository<AccountingObject>().All().Include(a => a.ReceiptReason).SingleOrDefault(x => x.ID == id);

            if (obj != null)
            {
                OnGet.Raise(() => new OnGet<AccountingObject>(obj, unitOfWork));
            }

            return obj;
        }

        /// <summary>
        /// Импорт ОБУ из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        /// <param name="history"></param>
        public override void Import(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                ImportBulk(uofw, histUofw,table,colsNameMapping,ref count, ref history);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);
                monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }
        }

        protected void ImportBulk(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {           
            try
            {                

                DataСhecks(uofw, histUofw, table, colsNameMapping, ref history);

                DateTime tPeriod = DateTime.MinValue;
                if (history.Period != null)
                    tPeriod = history.Period.Value;

                OBUVersionControl version = new OBUVersionControl(uofw, table, colsNameMapping, tPeriod, ref history);

                var type = typeof(AccountingObject);
                try
                {
                    _accessService.ThrowIfAccessDenied(uofw, type, TypePermission.Create | TypePermission.Write);

                    var queryBuilder = new OSQueryBuilder(colsNameMapping, type, version);

                    var bulkMergeManager = new BulkMergeManager(colsNameMapping, type, ref history, queryBuilder);
                    bulkMergeManager.BulkInsertAll(table, type, colsNameMapping, ref count);
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                    throw new Exception($"В процессе импорта данных были выявлены критические ошибки в файле шаблона импорта. Дальнейший импорт невозможен. {System.Environment.NewLine}Системный текст ошибки: {ex.ToStringWithInner()}");
                }

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
        /// Импорт ОБУ из файла Excel. Старая версия.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        /// <param name="history"></param>
        protected void ImportOld(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                DataСhecks(uofw, histUofw, table, colsNameMapping, ref history);

                IAccountingObjectRecordValidator relationValidator = new RelationValidator(uofw);
                _validators.Add(relationValidator);

                int start = ImportHelper.GetRowStartIndex(table);

                DateTime tPeriod = DateTime.MinValue;
                if (history.Period != null)
                    tPeriod = history.Period.Value;

                var version = new OBUVersionControl(uofw, table, colsNameMapping, tPeriod, ref history);

                for (int i = start; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    ImportObject(uofw, row, colsNameMapping, ref count, ref history, version);
                    count++;
                }

                foreach (var recordValidator in _validators)
                {
                    recordValidator.ProcessValidationResult(ref history);
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
                _logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);
                monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }
        }

        public CheckImportResult CheckConfirmResult(IUnitOfWork uow
           , string fileName
           , ExcelDataReader.IExcelDataReader reader
           , DataTable table
           )
        {
            if (Regex.IsMatch(fileName, ImportHelper._FILE_NAME_TEMPLATE_CODE_YYYY_MM_dd))
            {
                string[] arFileName = fileName.Split('_');
                string be = ImportHelper.GetIDEUP(arFileName[0]);
                DateTime period = new DateTime(int.Parse(arFileName[1]), int.Parse(arFileName[2]), int.Parse(arFileName[3]));
                int currentVersion = ImportHelper.FindDataVersionValue(reader);
                var mnemonic = ImportHelper.FindTypeName(reader); 
                var oldVersion = uow.GetRepository<ImportHistory>()
                .FilterAsNoTracking(f => !f.Hidden && f.IsSuccess && f.Mnemonic == mnemonic
                && f.Consolidation != null && f.Consolidation.Code == be
                && (f.Period != null && f.Period.Value.Year == period.Year && f.Period.Value.Month == period.Month) 
                && f.DataVersion < currentVersion)
                .FirstOrDefault();

                if (oldVersion != null)
                {
                    CheckImportResult res = new CheckImportResult();
                    res.IsConfirmationRequired = true;
                    res.ConfirmationItemDescription = $"{be} за период \"{period.ToString("MMMM yyyy")}\"";
                    return res;
                }
            }
            return null;
        }
        public CheckImportResult CheckVersionImport(IUiFasade uiFacade, IExcelDataReader reader, ITransactionUnitOfWork uofw,
           StreamReader stream, string fileName)
        {
            //реализовано в EUSIImportChecker
            return null;
        }
    }

    /// <summary>
    /// Методы расширения для объекта имущества.
    /// </summary>
    public static class EstateExtentions
    {
        /// <summary>
        /// Устанавливает передаваемое значение свойству объекта по его наименованию.
        /// </summary>
        /// <param name="est">Экземпляр объекта имущества.</param>
        /// <param name="property">Наименование свойства.</param>
        /// <param name="value">Устанавливаемое значение.</param>
        public static void Fill(
            this CorpProp.Entities.Estate.Estate est
            , string property
            , object value
            , bool condition = true)
        {
            PropertyInfo prop = est.GetType().GetProperty(property);
            if (prop != null && prop.SetMethod != null)
            {
                var oldVal = prop.GetValue(est);
                if (oldVal == null && condition)
                {
                    prop.SetValue(est, value);
                }
            }
        }

        /// <summary>
        /// Устанавливает признак "Энергоэффективное оборудование" у объекта ОИ.
        /// </summary>
        /// <param name="est">Экземпляр объекта имущества.</param>
        public static void SetIsEnergy(
           this CorpProp.Entities.Estate.Estate est
            , IUnitOfWork uow)
        {
            List<string> energyLabels = new List<string>() { "А", "А+", "А++" };
            string okof = est.OKOF2014?.Code;
            bool okofInHighEnergy = uow.GetRepository<HighEnergyEfficientFacility>()
                .FilterAsNoTracking(f => !f.Hidden && !String.IsNullOrEmpty(f.CodeOKOF2) && f.CodeOKOF2 == okof).Any();
            bool okofInHighEnergyKP = uow.GetRepository<HighEnergyEfficientFacilityKP>()
                .FilterAsNoTracking(f => !f.Hidden && !String.IsNullOrEmpty(f.CodeOKOF2) && f.CodeOKOF2 == okof).Any();

            var inv = est as InventoryObject;
            if (inv != null)
            {
                var taxes = uow.GetRepository<EstateTaxes>().FilterAsNoTracking(x => x.TaxesOfID == inv.ID)?.FirstOrDefault();
                if (taxes != null)
                {
                    bool energyDocsNotNull = (taxes.EnergyDocsExist != null);

                    if (((okofInHighEnergy || okofInHighEnergyKP) && energyDocsNotNull) ||
                        (inv != null && taxes.EnergyLabel != null && energyLabels.Contains(taxes.EnergyLabel.Code)))
                    {
                        est.Fill("IsEnergy", true);
                    }
                }
                
            }
        }
    }
}