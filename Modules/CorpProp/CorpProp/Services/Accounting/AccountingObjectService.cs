using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Extensions;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CorpProp.Services.Accounting
{
    /// <summary>
    /// Предоставляет данные и методы сервиса записи об объекте БУ.
    /// </summary>
    public interface IAccountingObjectService : Base.ITypeObjectService<AccountingObject>, IExcelImportEntity
    {
        
    }

    /// <summary>
    /// Представляет сервис записи об объекте БУ.
    /// </summary>
    public class AccountingObjectService : TypeObjectService<AccountingObject>, IAccountingObjectService
    {

        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService; 
        protected readonly List<IAccountingObjectRecordValidator> _validators = new List<IAccountingObjectRecordValidator>();
        protected readonly IAccessService _accessService;
        private readonly ILogService _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingObjectService.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="securityUserService"></param>
        /// <param name="pathHelper"></param>
        /// <param name="workflowService"></param>
        public AccountingObjectService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
            IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService,  ILogService logger
            ) : base(facade, logger)
        {
            _securityUserService = securityUserService;
            _workflowService = workflowService;
            _accessService = accessService;
            _logger = logger;
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
                if (!DataСhecks(uofw, histUofw, table, colsNameMapping, ref history))
                    return;

                IAccountingObjectRecordValidator relationValidator = new RelationValidator(uofw);
                _validators.Add(relationValidator);
                                             
                int start = ImportHelper.GetRowStartIndex(table);

                DateTime tPeriod = DateTime.MinValue;
                if (history.Period != null)
                    tPeriod = history.Period.Value;

                OBUVersionControl version = new OBUVersionControl(uofw, table, colsNameMapping, tPeriod, ref history);
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
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);
            }
        }
                
        /// <summary>
        /// Идентификация ОБУ при импорте.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="row"></param>
        /// <param name="colsNameMapping"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public virtual List<AccountingObject> FindObjects(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , ref ImportHistory history            
            )
        {
            List<AccountingObject> list = new List<AccountingObject>();
            //читаем инв номер и ИД ЕУП               
            var InvNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryNumber", colsNameMapping);
            var IDEUP = ImportHelper.GetValueByName(uofw, typeof(string), row, "Owner", colsNameMapping);

            //ищем в Системе ОБУ
            if (InvNumb != null && IDEUP != null
                && !String.IsNullOrEmpty(InvNumb.ToString())
                && !String.IsNullOrEmpty(IDEUP.ToString()))
            {
                string str = ImportHelper.GetIDEUP(IDEUP);
                var inv = InvNumb.ToString();
                list = uofw.GetRepository<AccountingObject>().Filter(x =>
                !x.Hidden &&
                !x.IsHistory &&
                x.Owner != null && x.Owner.IDEUP == str
                && x.InventoryNumber == inv)
                .Include(inc => inc.Estate)
                .ToList<AccountingObject>();

                if (list.Count == 0)
                    list.Add(uofw.GetRepository<AccountingObject>().Create(Activator.CreateInstance<AccountingObject>()));
                else if (list.Count > 1)
                {                    
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, ""
                        , $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}", ErrorType.System);
                }
            }
            else
            {                
                history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", $"Неверное значение Инвентарного номера и ИД ЕУП. {System.Environment.NewLine}", ErrorType.System);
            }

            return list;
        }

       

        /// <summary>
        /// Имопртирует ОБУ из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public virtual void ImportObject(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping           
            , ref int count
            , ref ImportHistory history
            , OBUVersionControl version)
        {
            try
            {                           
                bool isNew = true;
                AccountingObject obj = null;

                List<AccountingObject> list = FindObjects(uofw, row, colsNameMapping, ref history);                
                obj = (list.Count == 1) ? list[0] : null;
                if (obj == null) return;

                //выполняем необходимые действия с версионностью истории
                version.Execute(row, ref obj, ref history);
                isNew = (obj.ID == 0);                
                FindOrCreateEstate(uofw, obj, row, colsNameMapping, ref history);  
                AddonLogic(uofw, obj, row, colsNameMapping, ref history);
                UpdateEstateData(uofw, obj, ref history);
                
                if (isNew)
                    this.CreateFromImport(uofw, obj, history);
                else
                    this.UpdateFromImport(uofw, obj, history);

                foreach (var recordValidator in _validators)
                {
                    recordValidator.Validate(obj);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Обновляет данные ОИ по данным ОБУ.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected virtual void UpdateEstateData(IUnitOfWork uow, AccountingObject obj, ref ImportHistory history)
        {            
            ImportAccountingObject.UpdateEstateData(uow, obj);
        }

        /// <summary>
        /// Ищет и устанавливает связь с ОИ или создает новый ОИ.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected virtual void FindOrCreateEstate(IUnitOfWork uow
            , AccountingObject obj
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , ref ImportHistory history)
        {            
            Entities.Estate.Estate est = obj.Estate;
            if (est == null && obj.EstateID == null)
            {
                est = ImportAccountingObject.FindOrCreateEstate(uow, obj, ref history) as Entities.Estate.Estate;
                if (est != null)                
                    obj.Estate = est;  
            }   
        }

        /// <summary>
        /// Дополнительная логика обработки импортируемого объекта.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected virtual void AddonLogic(IUnitOfWork uow, AccountingObject obj, DataRow row, Dictionary<string, string> colsNameMapping, ref ImportHistory history)
        {
            //установление признака - спорный объект
            if (obj.Estate != null && obj.Estate is MovableEstate && !String.IsNullOrEmpty(obj.RegNumber))
                obj.IsDispute = true;
            
            //создание ИК
            CreateIK(uow, obj);
        }

        /// <summary>
        /// Создание имущественного комплекса.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="obj">Текущий экземлпяр ОБУ.</param>
        protected internal void CreateIK(IUnitOfWork uow, AccountingObject obj)
        {
            if (obj.Estate is InventoryObject && !String.IsNullOrEmpty(obj.PropertyComplexName)
                       && ((InventoryObject)obj.Estate).ParentID == null)
            {
                var ik = uow.GetRepository<PropertyComplexIO>()
                    .Filter(f => !f.Hidden && !f.IsHistory && f.Name == obj.PropertyComplexName)
                    .FirstOrDefault();
                if (ik != null)
                    ((InventoryObject)obj.Estate).Parent = ik;
                else
                    ((InventoryObject)obj.Estate).Parent =
                    uow.GetRepository<PropertyComplexIO>()
                    .Create(new PropertyComplexIO()
                    {
                        Name = obj.PropertyComplexName,
                        NameEUSI = obj.PropertyComplexName,
                        NameTIS = obj.PropertyComplexName,
                        IsPropertyComplex = true,

                    });
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
            var hisID = history.Oid;
            //TODO: придется материализовать
            var imported = uow.GetRepository<ImportObject>()
                .Filter(f => !f.Hidden
                && f.ImportHistoryOid == hisID
                && f.Type == TypeImportObject.CreateObject)
                .ToList<ImportObject>();

            string err = "";
            int count = 0;
            foreach (var item in imported)
            {
                var obj = ImportHelper.GetBaseObject(uow, item.Entity.GetTypeBo(), item.Entity.ID, ref err);
                if (obj != null)
                {
                    ImportHelper.UpdateRepositoryObject(uow, item.Entity.GetTypeBo(), obj);
                    var pr = obj.GetType().GetProperty("Hidden");
                    if (pr != null)
                        pr.SetValue(obj, true);
                    item.Hidden = true;
                }
                count++;                    
            }
            history.ResultText = $"Импорт отменен. Обработано {count} объектов.";
            history.IsCanceled = true;
            uow.SaveChanges();
        }


        /// <summary>
        /// Установление связи созданного объекта с историей импорта.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        public void CreateFromImport(
            IUnitOfWork uow
            , AccountingObject obj
            , ImportHistory history) 
        {
            var newEstate = (obj.Estate != null && obj.Estate.ID == 0);

            uow.SaveChanges();
            uow.GetRepository<ImportObject>()
                .Create(new ImportObject(obj, history.Oid, TypeImportObject.CreateObject));

            if (newEstate)
                uow.GetRepository<ImportObject>()
               .Create(new ImportObject(obj.Estate, history.Oid, TypeImportObject.CreateObject));
            uow.SaveChanges();
            return;
        }

        /// <summary>
        /// Установление связи обновленного объекта с историей импорта.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        public void UpdateFromImport(
           IUnitOfWork uow
           , AccountingObject obj
           , ImportHistory history)
        {
            var newEstate = (obj.Estate != null && obj.Estate.ID == 0);

            uow.GetRepository<ImportObject>()
                .Create(new ImportObject(obj, history.Oid, TypeImportObject.UpdateObject));

            if (newEstate)
            {
                uow.SaveChanges();
                uow.GetRepository<ImportObject>()
                .Create(new ImportObject(obj.Estate, history.Oid, TypeImportObject.CreateObject));
            }               

            if (uow is ITransactionUnitOfWork)
                uow.SaveChanges();
            return ;
        }

        /// <summary>        
        /// Дополнительная, кастомизированная проверка импортируемых данных.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="histUofw">Сессия истории импорта.</param>
        /// <param name="table">Таблица</param>
        /// <param name="colsNameMapping">Мэппинг имен колонок.</param>
        /// <param name="history">Экземпляр истории импорта.</param>       
        /// <returns>True, если проверка данных успешно пройдена, иначе False.</returns>
        public virtual bool DataСhecks(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref ImportHistory history)
        {
            RequiredChecks(uofw, histUofw, table, colsNameMapping, ref history);

            //проверка на дубли инвентарников
            DataColumn indexInv = table.Columns[colsNameMapping.FirstOrDefault(f => f.Value == "InventoryNumber").Key]; //индекс колонки инвентарного номера
            DataColumn indexBe = table.Columns[colsNameMapping.FirstOrDefault(f => f.Value == "Owner").Key]; // индекс балансодержатель
            var duplicates = table.Rows.Cast<DataRow>()
                .Where(r => r[indexInv] != null && r[indexBe] != null
                && !String.IsNullOrEmpty(r[indexInv].ToString())
                && !String.IsNullOrEmpty(r[indexBe].ToString())
                )
                .DefaultIfEmpty()
                .GroupBy(gr => new { Owner = gr[indexBe], Inventory = gr[indexInv] })
                .Select(s => new { Key = s.Key, Count = s.ToList().Count })
                .DefaultIfEmpty()
                .Any(f => f.Count > 1);
            if (duplicates)
            {
                history.ImportErrorLogs.AddError(ErrorTypeName.DuplicateRowErr);

            }

            return !history.ImportErrorLogs.Any();
        }

        /// <summary>
        /// Выполнение обязательных проверок импортируемых данных.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="colsNameMapping"></param>
        /// <param name="history"></param>
        /// <remarks>
        /// Выполняемые здесь проверки должны быть обязательными для АИС КС и ЕУСИ.
        /// </remarks>
        protected internal void RequiredChecks(
             IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref ImportHistory history)
        {
            int columnsUserName = ImportHelper.GetRowUserNameRow(table);
            //проверка периода загрузки данных
            if (history.Period == null)
                history.ImportErrorLogs.AddError(ErrorTypeName.InvalidFileNameFormat);


            //проверка даты ввода в эксплуатацию
            if (colsNameMapping.ContainsValue("InServiceDate"))
            {
                DataColumn inServiceDateColumn = table.Columns[colsNameMapping.FirstOrDefault(f => f.Value == "InServiceDate").Key];
                var rows = table.Rows.Cast<DataRow>()
                    .Where(row => !row[inServiceDateColumn].Equals(System.DBNull.Value)
                    && row[inServiceDateColumn].ToString().GetDate() > DateTime.Now);
                var colName = table.Rows[columnsUserName][inServiceDateColumn].ToString();
                foreach (var row in rows)
                {
                    history.ImportErrorLogs
                        .AddError(
                         table.Rows.IndexOf(row) + 1
                        , colsNameMapping.Values.ToList().IndexOf("InServiceDate") + 1
                        , $"{colName} не должна быть позднее текущей даты."
                        , ErrorType.System
                        );
                }
            }
        }

    }
    public interface IAccountingObjectRecordValidator
    {
        void Validate(AccountingObject newObj);
        void ProcessValidationResult(ref ImportHistory history);
    }

    /// <summary>
    /// 5571  проверка по составу связей ОБУ и ОП
    /// </summary>
    public class RelationValidator : IAccountingObjectRecordValidator
    {
        private int _relationCheckOk = 0;
        private int _processedRecords = 0;

        private readonly IUnitOfWork _uofw;
        public RelationValidator(IUnitOfWork uofw)
        {
            this._uofw = uofw;
        }
        public void Validate(AccountingObject newObj)
        {
            if (newObj != null && !string.IsNullOrEmpty(newObj.RegNumber))
            {
                _processedRecords++;
                if (!newObj.EstateID.HasValue)
                {
                    return;
                }
                var estateId = newObj.EstateID.Value;

                var validateResult = _uofw.GetRepository<Right>().All().Any(x => x.EstateID == estateId && x.RegNumber == newObj.RegNumber);
                if (validateResult)
                {
                    _relationCheckOk++;
                }
            }
        }

        public void ProcessValidationResult(ref ImportHistory history)
        {
            var relationCheckFail = _processedRecords - _relationCheckOk;
            var relationCheckText = $"Проверка по составу связей ОБУ и ОП (по полю \"Номер записи гос регистрации\"): для {_relationCheckOk} ОБУ связь с объектом права найдена, для {relationCheckFail} ОБУ связь с ОП не найдена. ";
            history.ResultText += relationCheckText;
        }
    }

}
