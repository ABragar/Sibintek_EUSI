using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Import;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CorpProp.Services.CorporateGovernance
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - оценка.
    /// </summary>
    public interface IAppraisalService : ITypeObjectService<Appraisal>//, IExcelImportEntity
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - оценка.
    /// </summary>
    public class AppraisalService : TypeObjectService<Appraisal>, IAppraisalService
    {
        private readonly ILogService _logger;
        private readonly EstateAppraisalService _estateAppraisalService;
        /// <summary>
        /// Инициализирует новый экземпляр класса AppraisalService.
        /// </summary>
        /// <param name="facade"></param>
        public AppraisalService(IBaseObjectServiceFacade facade, EstateAppraisalService estateAppraisalService, ILogService logger) : base(facade, logger)
        {
            _estateAppraisalService = estateAppraisalService;
            _logger = logger;
        }

        /// <summary>
        /// Переопределяет метод при событии обновления объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Оценка.</returns>
        public override Appraisal Update(IUnitOfWork unitOfWork, Appraisal obj)
        {
            var newObj = WriteContact(unitOfWork, obj, true);
            return base.Update(unitOfWork, newObj);
        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Оценка.</returns>
        public override Appraisal Create(IUnitOfWork unitOfWork, Appraisal obj)
        {
            var newObj = WriteContact(unitOfWork, obj);
            return base.Create(unitOfWork, newObj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<Appraisal> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Appraisal> objectSaver)
        {
            
            var obj =
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Executor)                  
                    .SaveOneObject(x => x.SibRegion)
                    .SaveOneObject(x => x.Customer)
                    .SaveOneObject(x => x.Owner)
                    .SaveOneObject(x => x.Appraiser)
                    .SaveOneObject(x => x.AppType)
                    .SaveOneObject(x => x.AppraisalGoal)
                    .SaveOneObject(x => x.AppraisalPurpose)                   
                    .SaveOneObject(x => x.Deal)
                    .SaveOneObject(x => x.FileCard)
                    .SaveOneObject(x => x.FileAcceptInSociety)
                    .SaveOneObject(x => x.FileAcceptDept)                    
                    ;
            UpdateNCAStatus(unitOfWork, obj.Dest);
            SetTaskNumber(unitOfWork, obj.Dest);
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="isUpdate">Признак обновления объекта.</param>
        /// <returns></returns>
        private Appraisal WriteContact(IUnitOfWork unitOfWork, Appraisal obj, bool isUpdate = false)
        {
            CheckAppraisalNumber(unitOfWork, obj, isUpdate);

            if (obj.ReportDate.Date < obj.AppraisalDate.Date)
                throw new Exception("Дата отчета должна быть больше или равной дате оценки.");

            if (obj.Executor == null || obj.Executor?.ID == 0)
                return obj;

            var executor = unitOfWork.GetRepository<Entities.Security.SibUser>().Find(f => f.ID == obj.Executor.ID);

            if (executor == null)
                return obj;

            obj.ExecutorLastName = executor.LastName;
            obj.ExecutorFirstName = executor.FirstName;
            obj.ExecutorMiddleName = executor.MiddleName;
            obj.ExecutorDeptName = executor.SocietyDeptName;
            obj.ExecutorPhone = executor.Phone;
            obj.ExecutorEmail = executor.Email;
            obj.ExecutorMobile = executor.Mobile;
            obj.ExecutorPostName = obj.ExecutorPostName ?? executor.PostName;

            return obj;
        }

        /// <summary>
        /// Проверка на существование оценки с идентичным номером.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Оценка</param>
        /// <param name="isUpdate">Операция (Обновление/иное).</param>
        private void CheckAppraisalNumber(IUnitOfWork unitOfWork, Appraisal obj, bool isUpdate = false)
        {
            var appraisals = this.GetAll(unitOfWork).Cast<Appraisal>()
                .Where(x => x.ReportNumber == obj.ReportNumber).ToList<Appraisal>();

            if (appraisals.Count > 0 && (!isUpdate || isUpdate && (appraisals.FirstOrDefault().ID != obj.ID)))
                throw new Exception("Оценка с заданным номером отчета уже присутствует.");
        }

        /// <summary>
        /// Смена статуса ННА.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Оценка.</param>
        private void UpdateNCAStatus(IUnitOfWork unitOfWork, Appraisal obj)
        {
            if (!obj.AppraisalNNA)
                return;
            //Если указаны "Номер поручения об оценке" и "Дата поручения об оценке" то ННА проставляем статус "Дано поручение об оценке"
            if (!String.IsNullOrEmpty(obj.TaskNumber) && obj.TaskDate != null)
            {
                var ncaRepo = unitOfWork.GetRepository<Entities.Asset.NonCoreAsset>();
                var eaList = unitOfWork.GetRepository<EstateAppraisal>().Filter(f => f.AppraisalID == obj.ID && f.AccountingObjectID != null).ToList<EstateAppraisal>();
                var ncaStatus = unitOfWork.GetRepository<Entities.Asset.NonCoreAssetStatus>()
                    .Filter(f => !f.Hidden && f.Code == "05" && !f.IsHistory)
                    .FirstOrDefault();
                if (eaList.Count > 0)
                {
                    foreach (EstateAppraisal ea in eaList)
                    {

                        var ao = unitOfWork.GetRepository<Entities.Accounting.AccountingObject>()
                            .Find(f => f.ID == ea.AccountingObjectID && f.EstateID != null);
                        if (ao == null)
                            return;

                        var ncaList = ncaRepo.Filter(f => f.EstateObjectID == ao.EstateID).ToList<Entities.Asset.NonCoreAsset>();

                        if (ncaList.Count > 0)
                        {
                            foreach (Entities.Asset.NonCoreAsset nca in ncaList)
                            {
                                if (nca.NonCoreAssetStatusID == ncaStatus.ID)
                                    return;

                                nca.NonCoreAssetStatus = ncaStatus;
                                nca.NonCoreAssetStatusID = ncaStatus.ID;
                                ncaRepo.Update(nca);
                            }
                        }
                    }
                }
            }
            //Если указана "Дата согласования ДС ЦАУК" то ННА проставляем статус "Оценка проведена и согласована ДС"
            if (obj.AcceptCAUKDate != null)
            {
                var ncaRepo = unitOfWork.GetRepository<Entities.Asset.NonCoreAsset>();
                var eaList = unitOfWork.GetRepository<EstateAppraisal>().Filter(f => f.AppraisalID == obj.ID && f.AccountingObjectID != null).ToList<EstateAppraisal>();
                var ncaStatus = unitOfWork.GetRepository<Entities.Asset.NonCoreAssetStatus>()
                    .Filter(f => !f.Hidden && f.Code == "06" && !f.IsHistory)
                    .FirstOrDefault();
                if (eaList.Count > 0)
                {
                    foreach (EstateAppraisal ea in eaList)
                    {

                        var ao = unitOfWork.GetRepository<Entities.Accounting.AccountingObject>()
                            .Find(f => f.ID == ea.AccountingObjectID && f.EstateID != null);
                        if (ao == null)
                            return;

                        var ncaList = ncaRepo.Filter(f => f.EstateObjectID == ao.EstateID && !f.Hidden)
                            .ToList<Entities.Asset.NonCoreAsset>();

                        if (ncaList.Count > 0)
                        {
                            foreach (Entities.Asset.NonCoreAsset nca in ncaList)
                            {
                                if (nca.NonCoreAssetStatusID == ncaStatus.ID)
                                    return;

                                nca.NonCoreAssetStatus = ncaStatus;
                                nca.NonCoreAssetStatusID = ncaStatus.ID;
                                ncaRepo.Update(nca);
                            }
                        }
                    }
                }
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
            , IExcelDataReader reader
            , Type type
            , ref int count
            , ref ImportHistory history)         
        {
            try
            {
                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[0];
                ImportStarter.DeleteEmptyRows(ref entryTable);
                DataTable estateAppraisalTable = tables[1];

                string err = "";

                if (type != null && Type.Equals(type, typeof(Appraisal)))
                {
                    Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(entryTable);
                    Dictionary<string, string> colsNameMappingEstateAppraisal = ImportHelper.ColumnsNameMapping(estateAppraisalTable);
                    
                    int start = ImportHelper.GetRowStartIndex(entryTable);
                    for (int i = start; i < entryTable.Rows.Count; i++)
                    {
                        var row = entryTable.Rows[i];
                        ImportObject(uofw, row, colsNameMapping, ref err, ref count, ref history);
                        count++;
                    }

                    new ImportChecker().StartDataCheck(uofw, histUofw, estateAppraisalTable, typeof(EstateAppraisal), ref history);

                    if (history.ImportErrorLogs.Count > 0)
                    {
                        throw new ImportException("Ошибки при проверке.");
                    }
                    _estateAppraisalService.Import(uofw, histUofw, estateAppraisalTable, colsNameMappingEstateAppraisal, ref count, ref history);
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }


        public List<Appraisal> FindObjects(IUnitOfWork uofw, string reportNumber)
        {
            List<Appraisal> list = new List<Appraisal>();
            list = uofw.GetRepository<Appraisal>().Filter(x =>
            x.ReportNumber != null && x.ReportNumber == reportNumber && !x.Hidden).ToList<Appraisal>();
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
            , ref string error
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                bool isNew = true;
                Appraisal obj = null;
                var reportNumber = ImportHelper.GetValueByName(uofw, typeof(string), row, "ReportNumber", colsNameMapping);

                if (reportNumber != null
                    && !String.IsNullOrEmpty(reportNumber.ToString()))
                {
                    //TODO: почистить
                    List<Appraisal> list = FindObjects(uofw, reportNumber.ToString());
                    if (list == null || list.Count == 0)
                        obj = new Appraisal();
                    else if (list.Count > 1)
                    {
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                        return;
                    }
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(Appraisal),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;
                        if (obj.Executor == null )
                        {
                            var executor = ImportHelper.GetValueByName(uofw, typeof(string), row, "Executor", colsNameMapping);
                            var email = ImportHelper.GetValueByName(uofw, typeof(string), row, "ExecutorEmail", colsNameMapping);
                            var mobile = ImportHelper.GetValueByName(uofw, typeof(string), row, "ExecutorMobile", colsNameMapping);
                            var phone = ImportHelper.GetValueByName(uofw, typeof(string), row, "ExecutorPhone", colsNameMapping);
                            obj.Executor = ImportHelper.CreateSibUser(uofw, executor.ToString(), phone.ToString(), mobile.ToString(), email.ToString());
                        }

                        Appraisal newObj = null;

                        if (isNew)
                            newObj = this.Create(uofw, obj);
                        else
                            newObj = this.Update(uofw, obj);
                    }
                }
                else
                {
                    error = $"Не заполнена колонка <Номер отчёта>. {System.Environment.NewLine}";
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
        /// Устанавливает значение номера и даты поручения оценки ННА.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="obj">Оценка.</param>
        public void SetTaskNumber(IUnitOfWork uow, Appraisal obj)
        {
           if ( obj.ID != 0 
                && !String.IsNullOrEmpty(obj.TaskNumber)
                && obj.TaskDate != null
                )
            {
                var original = uow.GetRepository<Appraisal>().GetOriginal(obj.ID);

                if (obj.TaskDate != original.TaskDate
                    || obj.TaskNumber != original.TaskNumber)
                {
                    var nna = uow.GetRepository<EstateAppraisal>()
                    .Filter(f => !f.Hidden && !f.IsHistory
                    && f.Appraisal != null
                    && f.AppraisalID == obj.ID)
                    .Select(s => s.AccountingObject)
                    .Where(w => w.Estate != null && !w.Hidden && !w.IsHistory)
                    .Select(ss => ss.Estate)
                    .Join(
                    uow.GetRepository<NonCoreAsset>()
                    .Filter(f => !f.Hidden && !f.IsHistory)
                    , e => e.ID, o => o.EstateObjectID, (e, o) => o)
                    .ToList();
                    ;
                    foreach (var item in nna)
                    {                        
                        item.NonCoreAssetStatus = uow.GetRepository<NonCoreAssetStatus>()
                            .Filter(f => !f.Hidden && !f.IsHistory && f.Code == "05")
                            .FirstOrDefault();
                        item.TaskNumber = obj.TaskNumber;
                        item.TaskDate = obj.TaskDate;
                    }
                }
                    
            }
            
        }
    }
}
