using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Accounting;
using EUSI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using EUSI.Import.BulkMerge;
using Base.Enums;
using CorpProp.Services.Import.BulkMerge;
using Base.Service.Log;
using Base.Utils.Common;

namespace EUSI.Services.Accounting
{
    /// <summary>
    /// Предоставляет методы сервиса миграции ОС/НМА в ЕУСИ.
    /// </summary>
    public interface IAccountingObjectMigrate : IAccountingObjectExtService
    {

    }

    /// <summary>
    /// Представляет сервис миграции ОС/НМА с гибридным функционалом импорта АИС КС и ЕУСИ.
    /// </summary>
    public class AccountingObjectMigrate : AccountingObjectExtService, IAccountingObjectMigrate
    {

        private readonly ILogService _logger;

        /// <summary>
        /// Инициализиует новый экземпляр класса AccountingObjectMigrate.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="securityUserService"></param>
        /// <param name="pathHelper"></param>
        /// <param name="workflowService"></param>
        /// <param name="accessService"></param>
        public AccountingObjectMigrate(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
            IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService, ILogService logger
            ) : base(facade, securityUserService, pathHelper, workflowService, accessService, logger)
        {
            _logger = logger;
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

            //читаем инв номер, ЕУСИ, БЕ, системный номер      
            var invNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryNumber", colsNameMapping);
            var eusiNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "EUSINumber", colsNameMapping);
            var sysNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "ExternalID", colsNameMapping);
            var be = ImportHelper.GetValueByName(uofw, typeof(string), row, "Consolidation", colsNameMapping);


            if (invNumb == null || be == null  
                || (invNumb != null && String.IsNullOrEmpty(invNumb.ToString()))
                || (be != null && String.IsNullOrEmpty(be.ToString()))
                )
            {                
                history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "",
                    $"Невозможно идентифицировать объект. Проверьте значения атрибутов: <Инвентарный номер>, <БЕ>{System.Environment.NewLine}"
                    , ErrorType.System);
                return list;
            }
            var beCode = be.ToString();
            var invNumber = invNumb.ToString();

            //Если номер ЕУСИ заполнен
            if (eusiNumb != null && !String.IsNullOrEmpty(eusiNumb.ToString()))
            {
                var numbEUSI = eusiNumb.ToString();
                //идентификация по: БЕ+Инв+НомерЕУСИ
                list = uofw.GetRepository<AccountingObject>().Filter(x =>
                !x.Hidden &&
                !x.IsHistory &&
                x.Consolidation != null && x.Consolidation.Code == beCode
                && x.InventoryNumber == invNumber
                && x.Estate != null && x.Estate.Number.ToString() == numbEUSI)
                .Include(inc => inc.Estate)
                .ToList<AccountingObject>();
               
                //не нашли, идентификация по: БЕ+НомерЕУСИ
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
            }
            //Если номер ЕУСИ НЕ заполнен
            else
            {
                //если заполнен системный номер
                if (sysNumb != null && !String.IsNullOrEmpty(sysNumb.ToString()))
                {
                    var numbSys = sysNumb.ToString();
                    //идентификация по: Инв+БЕ+СисНомер
                    list = uofw.GetRepository<AccountingObject>().Filter(x =>
                           !x.Hidden &&
                           !x.IsHistory &&
                           x.Consolidation != null && x.Consolidation.Code == beCode
                           && x.InventoryNumber == invNumber
                           && x.ExternalID == numbSys)
                       .Include(inc => inc.Estate)
                       .ToList<AccountingObject>();
                }
                //сис номер не заполнен или ничего не найдено по Инв+БЕ+СисНомер, тогда ищем по: БЕ+Инв
                if (list.Count == 0)
                    list = uofw.GetRepository<AccountingObject>().Filter(x =>
                           !x.Hidden &&
                           !x.IsHistory &&
                           x.Consolidation != null && x.Consolidation.Code == beCode
                           && x.InventoryNumber == invNumber)
                       .Include(inc => inc.Estate)
                       .ToList<AccountingObject>();

            }

            //ничего не найдено, то создаём новый ОС по логике АИС КС
            if (list.Count == 0)                            
                list.Add(uofw.GetRepository<AccountingObject>().Create(Activator.CreateInstance<AccountingObject>()));
            
            if (list.Count > 1)
            {
                var err = $"Невозможно идентифицировать объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", err, ErrorType.System);
                list = null;
            }
            
            return list;
        }

        /// <summary>
        /// Переопределяет поиск и создание ОИ при импорте.
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
            
            if (obj.Estate == null && obj.EstateID == null)
            {
                object est = null;
                var eusiCol = ImportHelper.GetValueByName(uow, typeof(string), row, "EUSINumber", colsNameMapping);
                if (eusiCol != null && !String.IsNullOrEmpty(eusiCol.ToString()))
                {
                    var eusi = ImportHelper.GetValueByName(uow, typeof(int), row, "EUSINumber", colsNameMapping);
                    //связка по номеру ЕУСИ
                    est = EUSIImportHelper.FindEstate(uow, (int)eusi, ref history);
                    if (est == null)
                    {
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "",
                           "Неверный номер ЕУСИ", ErrorType.System);
                        return;
                    }                    
                }
                else //создание ОИ по логике АИС КС                 
                    est = ImportAccountingObject.CreateEstateByRules(uow, obj, ref history) as CorpProp.Entities.Estate.Estate;

                if (est != null)
                    obj.Estate = est as CorpProp.Entities.Estate.Estate;
            }
        }

        public override void Import(IUnitOfWork uofw, IUnitOfWork histUofw, DataTable table, Dictionary<string, string> colsNameMapping, ref int count, ref ImportHistory history)
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

                    var queryBuilder = new OSMigrateQueryBuilder(colsNameMapping, type, version);

                    var bulkMergeManager = new BulkMergeManager(colsNameMapping, type, ref history, queryBuilder);
                    bulkMergeManager.BulkInsertAll(table, type, colsNameMapping, ref count);
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                    throw new Exception($"В процессе импорта данных были выявлены критические ошибки в файле шаблона импорта. Дальнейший импорт невозможен. {System.Environment.NewLine}Системный текст ошибки: {ex.ToStringWithInner()}");
                }

            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);                
            }
        }

        /// <summary>
        /// Переопределяет кастомизированные проверки при импорте.
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
            base.CheckRequiredFields(table, ref history);
            ImportChecker.CheckFieldData(table, typeof(AccountingObject), ref history, uofw, false);
            RequiredChecks(uofw, histUofw, table, colsNameMapping, ref history);            
            return !history.ImportErrorLogs.Any();
        }
    }
}
