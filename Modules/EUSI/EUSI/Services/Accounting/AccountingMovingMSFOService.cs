using Base.DAL;
using Base.Service;
using Base.Service.Log;
using Base.UI.Service;
using CorpProp;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Accounting;
using CorpProp.Services.Base;
using EUSI.Entities.Accounting;
using EUSI.Entities.NSI;
using EUSI.Helpers;
using EUSI.Validators;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EUSI.Services.Monitor;

namespace EUSI.Services.Accounting
{
    public interface IAccountingMovingMSFOService : ITypeObjectService<AccountingMovingMSFO>, IExcelImportEntity, IExcelImportChecker
    {

    }

    public class AccountingMovingMSFOService : TypeObjectService<AccountingMovingMSFO>, IAccountingMovingMSFOService
    {
        private readonly ILogService _logger;
        private IAccountingObjectService _accountingObjectService;
        private string _eventCode;
        private string _movingName;
        private const string FileNameTemplate = @"^[а-яА-Яa-zA-Z\d]*_[\d]{4}_[\d]{2}_[\d]{2}_\w*";

        private readonly MonitorReportingImportService monitor = new MonitorReportingImportService();

        private readonly Dictionary<string, string> _mappingMovingNameEventTypeCode = new Dictionary<string, string>
        {
            {nameof(TypeMovingMSFO.Debit01), "IMP_ST_Debet_01"},
            {nameof(TypeMovingMSFO.Credit01), "IMP_ST_Credit_01"},
            {nameof(TypeMovingMSFO.Depreciation01), "IMP_ST_Depreciation_01"},
            {nameof(TypeMovingMSFO.Debit07), "IMP_ST_Debet_07"},
            {nameof(TypeMovingMSFO.Credit07), "IMP_ST_Credit_07"},
            {nameof(TypeMovingMSFO.Debit08), "IMP_ST_Debet_08"},
            {nameof(TypeMovingMSFO.Credit08), "IMP_ST_Credit_08"}
        };

        public AccountingMovingMSFOService(
            IBaseObjectServiceFacade facade
            , IAccountingObjectService accountingObjectService, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _accountingObjectService = accountingObjectService;
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
                if (Regex.IsMatch(history.FileName, FileNameTemplate))
                {
                    string err = "";
                    int start = ImportHelper.GetRowStartIndex(table);
                    history.TemplateName = ImportHelper.GetTemplateName(table);

                    var arr = history.TemplateName.Split('_');
                    _movingName = arr[arr.Length - 1];
                    if (!string.IsNullOrEmpty(_movingName) && _mappingMovingNameEventTypeCode.ContainsKey(_movingName))
                    {
                        _eventCode = _mappingMovingNameEventTypeCode[_movingName];
                    }

                    int rs = ImportHelper.FindFieldSystemNameRow(table);
                    int colIndex = GetColumnIndex(table, rs);

                    new ImportChecker().StartDataCheck(uofw, histUofw, table, typeof(AccountingMovingMSFO), ref history);
                    new AccountingMovingMSFODataValidator().Validate(table, ref history);
                    CheckTypeAccounting(uofw, history.Consolidation?.Code, ref history);

                    var arFileName = history.FileName.Split('_');
                    string beCode = ImportHelper.GetConsolidationIDFromFileName(arFileName);
                    var consolidation = uofw.GetRepository<Consolidation>()
                        .Filter(f => !f.Hidden && f.Code == beCode)
                        .FirstOrDefault();
                    var periodStart = ImportHelper.GetPeriodStartEnd(arFileName, "Start");
                    var periodEnd = ImportHelper.GetPeriodStartEnd(arFileName, "End");
                    var version = new AccountingMovingVersionControl<AccountingMovingMSFO>(uofw, table, colsNameMapping, periodStart, ref history);

                    for (int i = start; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        ImportObject(uofw, row, colsNameMapping, consolidation, periodStart, periodEnd, ref err, ref count, ref history, version);
                        count++;
                    }

                    if (!history.ImportErrorLogs.Any())
                    {
                        monitor.CreateMonitorReportingForSuccessImport(history, count, histUofw, _eventCode);
                    }
                    else
                    {
                        monitor.CreateMonitorReportingForFailedImport(history, count, histUofw, _eventCode);
                    }
                }
                else
                {
                    history.ImportErrorLogs.AddError(ErrorTypeName.InvalidFileNameFormat);
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
                monitor.CreateMonitorReportingForFailedImport(history, count, histUofw, _eventCode);
            }
        }


        private int GetColumnIndex(DataTable table, int rs)
        {
            for (int i = 0; i < table.Rows[rs].ItemArray.Length; i++)
            {
                if (table.Rows[rs].ItemArray[i] != null &&
                    table.Rows[rs].ItemArray[i].ToString() == "Consolidation")
                    return i;
            }
            return 0;
        }

        /// <summary>
        /// Проверка БЕ на ведение соответсвующего типа бух учета.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="table"></param>
        /// <param name="start"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        private bool CheckTypeAccounting(
            IUnitOfWork uow
            , string beCode
            , ref ImportHistory history)
        {
            var typeAcc = uow.GetRepository<Consolidation>()
                .FilterAsNoTracking(f => !f.Hidden && f.Code == beCode)
                .FirstOrDefault()?.TypeAccounting?.Code?.ToUpper();
            var check = (typeAcc == "RSBUSIMPLE");

            if (!check)
                history.ImportErrorLogs
                    .AddError($"БЕ с кодом <{beCode}> не ведет импортиуемый тип бухгалтерского учета.");

            return check;
        }

        public List<AccountingMovingMSFO> FindObjects(
            IUnitOfWork uofw
            , string inventoryNumber
            , string consolidation
            , object externalID
            , TypeMovingMSFO movingName
            , DateTime periodStart
            , DateTime periodEnd)
        {
            string val = externalID.ToString();
            var list = uofw.GetRepository<AccountingMovingMSFO>()
                .Filter(x =>
                    !x.Hidden &&
                    !x.IsHistory &&
                    x.ExternalID == val &&
                    x.Consolidation != null &&
                    x.Consolidation.Code == consolidation &&
                    x.Period.Start == periodStart &&
                    x.Period.End == periodEnd
                );

            switch (movingName)
            {
                case TypeMovingMSFO.Credit01:
                case TypeMovingMSFO.Depreciation01:
                    list = (IExtendedQueryable<AccountingMovingMSFO>)list
                       .Where(x => x.InventoryCredit == inventoryNumber);
                    break;
                case TypeMovingMSFO.Credit08:
                case TypeMovingMSFO.Debit08:
                    list = (IExtendedQueryable<AccountingMovingMSFO>)list
                        .Where(x => x.TypeMovingMSFO == movingName);
                    break;
                case TypeMovingMSFO.Debit01:
                    list = (IExtendedQueryable<AccountingMovingMSFO>)list
                        .Where(x => x.InventoryDebit == inventoryNumber);
                    break;
                default:
                    break;
            }
            return list.ToList();
        }

        public List<AccountingMovingMSFO> FindObjects(
          IUnitOfWork uofw
          , string consolidation
          , object externalID
          , DateTime periodStart
          , DateTime periodEnd)
        {
            string val = externalID.ToString();
            List<AccountingMovingMSFO> list = uofw
                .GetRepository<AccountingMovingMSFO>()
                .Filter(x =>
                    !x.Hidden &&
                    !x.IsHistory &&
                    x.ExternalID == val &&
                    x.Consolidation != null &&                   
                    x.Consolidation.Code == consolidation &&
                    x.Period.Start == periodStart &&
                    x.Period.End == periodEnd
                 )
                 .ToList();

            return list;
        }


        /// <summary>
        /// Имопртирует из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public void ImportObject(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , Consolidation consolidation
            , DateTime periodStart
            , DateTime periodEnd
            , ref string error
            , ref int count
            , ref ImportHistory history
            , AccountingMovingVersionControl<AccountingMovingMSFO> version)
        {
            try
            {              
                if (System.Enum.TryParse<TypeMovingMSFO>(_movingName, out var mt))
                {
                    switch (mt)
                    {
                        //Связываем с ОБУ только кредит01, амортизация01 и дебет01        
                        case TypeMovingMSFO.Credit01:
                        case TypeMovingMSFO.Debit01:
                        case TypeMovingMSFO.Depreciation01:
                        case TypeMovingMSFO.Credit08:
                        case TypeMovingMSFO.Debit08:
                            ImportToOS(uofw, row, colsNameMapping, mt, consolidation, periodStart, periodEnd, ref error, ref count, ref history, version);
                            break;
                        default:
                            ImportMoving(uofw, row, colsNameMapping, mt, consolidation, periodStart, periodEnd, ref error, ref count, ref history, version);
                            break;
                    }
                }
                else
                {
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, ""
                        , "Неизвестный шаблон файла."
                        , ErrorType.System);
                }

            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }


        private void ImportMoving(
           IUnitOfWork uofw
           , DataRow row
           , Dictionary<string, string> colsNameMapping
           , TypeMovingMSFO movingName
           , Consolidation consolidation
           , DateTime periodStart
           , DateTime periodEnd
           , ref string error
           , ref int count
           , ref ImportHistory history
           , AccountingMovingVersionControl<AccountingMovingMSFO> version)
        {
            try
            {                
                AccountingMovingMSFO obj = null;
                var externalID = ImportHelper.GetValueByName(uofw, typeof(string), row, "ExternalID", colsNameMapping);
                var eusiNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "EUSINumber", colsNameMapping);
                if (eusiNumb == null)
                    eusiNumb = "";

                if (consolidation != null && externalID != null)
                {
                    List<AccountingMovingMSFO> list = FindObjects(uofw, consolidation?.Code, externalID, periodStart, periodEnd);
                    if (list == null || list.Count == 0)
                    {
                        obj = uofw.GetRepository<AccountingMovingMSFO>().Create(new AccountingMovingMSFO());
                    }
                    else if (list.Count > 1)
                    {
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                        return;
                    }
                    else if (list.Count == 1)
                    {                       
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        version.Execute(row, ref obj, ref history);                       
                        obj.Consolidation = consolidation;
                        obj.ConsolidationID = consolidation?.ID;
                        obj.Period.Start = periodStart;
                        obj.Period.End = periodEnd;

                        //вид загрузки
                        obj.LoadTypeID = GetLoadTypeID(uofw, history);
                        obj.EUSI = eusiNumb.ToString();
                        //тип движения МСФО
                        obj.TypeMovingMSFO = movingName;
                        if (obj.ID != 0)
                        {
                            AccountingMovingHelper.ClearFileLinks(uofw, obj);
                        }
                        AccountingMovingHelper.AddFileLink(uofw, row, colsNameMapping, obj);                                                
                        uofw.SaveChanges();
                    }
                }
                else
                {
                    error += $"Невозможно идентифицировать запись. {System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        private void ImportToOS(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , TypeMovingMSFO movingName
            , Consolidation consolidation
            , DateTime periodStart
            , DateTime periodEnd
            , ref string error
            , ref int count
            , ref ImportHistory history
            , AccountingMovingVersionControl<AccountingMovingMSFO> version)
        {
            try
            {
                //Связываем с ОБУ только кредит01, амортизация01 и дебет01  
                AccountingMovingMSFO obj = null;

                var inventoryNumber =
                    (movingName == TypeMovingMSFO.Debit01) ?
                    ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryDebit", colsNameMapping) :
                    ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryCredit", colsNameMapping);

                var externalID = ImportHelper.GetValueByName(uofw, typeof(string), row, "ExternalID", colsNameMapping);
                var eusiNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "EUSINumber", colsNameMapping);
                if (inventoryNumber == null)
                    inventoryNumber = "";

                if (eusiNumb == null)
                    eusiNumb = "";

                if ((movingName == TypeMovingMSFO.Credit08 || movingName == TypeMovingMSFO.Debit08)
                    && String.IsNullOrEmpty(eusiNumb.ToString()))
                {
                    ImportMoving(uofw, row, colsNameMapping, movingName, consolidation, periodStart, periodEnd, ref error, ref count, ref history, version);
                    return;
                }

                if (consolidation != null && externalID != null)
                {

                    List<AccountingMovingMSFO> list = FindObjects(uofw, inventoryNumber.ToString(), consolidation?.Code, externalID, movingName, periodStart, periodEnd);
                    if (list == null || list.Count == 0)
                    {
                        AccountingObject accobj = FindAccObject(uofw, eusiNumb.ToString(), inventoryNumber.ToString(), consolidation?.Code, movingName);
                        if (accobj == null)
                        {
                            error += $"В Системе не найден связанный ОС/НМА.{System.Environment.NewLine}";
                            history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                            return;
                        }
                        obj = uofw.GetRepository<AccountingMovingMSFO>().Create(new AccountingMovingMSFO());
                        obj.AccountingObject = accobj;
                    }

                    else if (list.Count > 1)
                    {
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                        return;
                    }
                    else if (list.Count == 1)
                    {                        
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        version.Execute(row, ref obj, ref history);
                        obj.Consolidation = consolidation;
                        obj.ConsolidationID = consolidation?.ID;
                        obj.Period.Start = periodStart;
                        obj.Period.End = periodEnd;

                        //вид загрузки
                        obj.LoadTypeID = GetLoadTypeID(uofw, history);
                        //тип движения МСФО                        
                        obj.TypeMovingMSFO = movingName;
                        obj.EUSI = eusiNumb.ToString();
                        if (obj.ID != 0)
                        {
                            AccountingMovingHelper.ClearFileLinks(uofw, obj);
                        }
                        AccountingMovingHelper.AddFileLink(uofw, row, colsNameMapping, obj);                                               

                        //При импорте файлов регистра движения (Упрощенное внедрение) 
                        //«Кредитование 01», «Дебетование 01»,  «Амортизация 01» 
                        //должны формироваться строки регистра движения РСБУ.
                        FindOrUpdateRSBU(uofw, obj);
                        uofw.SaveChanges();
                    }
                }
                else
                {
                    error += $"Невозможно идентифицировать запись. {System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Ищет или обновляет запись РСБУ на основании записи упрощенного внедрения.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="hard"></param>
        private void FindOrUpdateRSBU(IUnitOfWork uow, AccountingMovingMSFO hard)
        {
            AccountingMoving rsbu = null;
            var id = hard.ExternalID + "_";
            switch (hard.TypeMovingMSFO)
            {
                case TypeMovingMSFO.Credit01:
                case TypeMovingMSFO.Depreciation01:
                    rsbu = uow.GetRepository<AccountingMoving>()
                      .Filter(f =>
                      !f.Hidden
                      && !f.IsHistory
                      && f.ExternalID == id
                      && f.InventoryNumber == hard.InventoryCredit
                      && f.Consolidation != null
                      && f.Consolidation.Code == hard.Consolidation.Code
                      && f.Period.Start == hard.Period.Start 
                      && f.Period.End == hard.Period.End
                      ).FirstOrDefault();
                    break;
                case TypeMovingMSFO.Debit01:
                    rsbu = uow.GetRepository<AccountingMoving>()
                     .Filter(f =>
                     !f.Hidden
                     && !f.IsHistory
                     && f.ExternalID == id
                     && f.InventoryNumber == hard.InventoryDebit
                     && f.Consolidation != null
                     && f.Consolidation.Code == hard.Consolidation.Code 
                     && f.Period.Start == hard.Period.Start
                     && f.Period.End == hard.Period.End
                     ).FirstOrDefault();
                    break;

                default:
                    return;
            }

            if (rsbu == null)
                rsbu = uow.GetRepository<AccountingMoving>().Create(new AccountingMoving());
            else
                uow.GetRepository<AccountingMoving>().Update(rsbu);

            rsbu.Angle = uow.GetRepository<Angle>().Filter(f => !f.Hidden && !f.IsHistory && f.Code == "RSBU").FirstOrDefault();
            rsbu.AccountingObject = hard.AccountingObject;
            rsbu.ExternalID = hard.ExternalID + "_";
            rsbu.Date = hard.Date;
            rsbu.InventoryNumber = (hard.TypeMovingMSFO == TypeMovingMSFO.Debit01) ? hard.InventoryDebit : hard.InventoryCredit;
            rsbu.Consolidation = hard.Consolidation;
            rsbu.Amount = (hard.TypeMovingMSFO == TypeMovingMSFO.Depreciation01) ? hard.CostDepreciation : hard.Cost;
            rsbu.Period.Start = new DateTime(hard.Date.Value.Year, hard.Date.Value.Month, 1);
            rsbu.Period.End = rsbu.Period.Start.AddMonths(1).AddDays(-1);
            rsbu.LoadType = hard.LoadType;
            rsbu.MovingType = hard.MovingType;
            rsbu.EUSI = hard.EUSI;
            rsbu.ImportDate = DateTime.Now;
            rsbu.ImportUpdateDate = DateTime.Now;
            if (rsbu.ID == 0)
            {
                rsbu.ActualDate = hard.ActualDate;
                rsbu.NonActualDate = hard.NonActualDate;
            }            
            uow.SaveChanges();
        }



        public void CancelImport(
            IUnitOfWork uofw
           , ref ImportHistory history
           )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Находит ОС по инвентарному номеру и коду консолидации
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="numb"></param>
        /// <param name="idEup"></param>
        /// <returns></returns>
        public AccountingObject FindAccObject(IUnitOfWork uofw, string eusiNumb, string numb, string consolidation, TypeMovingMSFO movingName)
        {

            List<AccountingObject> list = new List<AccountingObject>();

            if (movingName == TypeMovingMSFO.Debit08 || movingName == TypeMovingMSFO.Credit08)
            {
                eusiNumb = ImportHelper.GetIDEUP(eusiNumb);

                list = uofw.GetRepository<AccountingObject>().Filter(x =>
                   !x.Hidden &&
                   !x.IsHistory &&
                   x.Consolidation != null && x.Consolidation.Code == consolidation
                   && x.Estate != null
                   && x.Estate.Number != null
                   && x.Estate.Number.ToString() == eusiNumb
                   ).ToList<AccountingObject>();
            }
            else
                list = uofw.GetRepository<AccountingObject>().Filter(x =>
                !x.Hidden &&
                !x.IsHistory &&
                x.Consolidation != null && x.Consolidation.Code == consolidation
                && x.InventoryNumber == numb).ToList<AccountingObject>();

            if (list.Count == 1)
                return list.FirstOrDefault();
            return null;
        }


        public int? GetLoadTypeID(IUnitOfWork uow, ImportHistory history)
        {
            string code = "Primary";
            if (history.IsCorrection)
                code = "Corrective";

            var obj = uow.GetRepository<LoadType>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.Code == code)
                .FirstOrDefault();
            if (obj != null)
                return obj.ID;
            return null;
        }

        /// <summary>
        /// Переопределяет получение актуальных данных на заданную дату.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="date">Дата.</param>
        /// <returns></returns>
        public override IQueryable<AccountingMovingMSFO> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            if (!date.HasValue)            
                return base.GetAll(uow, false).Where(f => !f.IsHistory);            

            var q = base.GetAll(uow, false);

            var groups = q
                .Where(x => x.Period.Start <= date.Value && x.ImportDate.HasValue)
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
}
