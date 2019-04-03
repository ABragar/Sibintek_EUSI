using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.CorporateGovernance
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - объект оценки.
    /// </summary>
    public interface IEstateAppraisalService : ITypeObjectService<EstateAppraisal>//, IExcelImportEntity
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - объект оценки.
    /// </summary>
    public class EstateAppraisalService : TypeObjectService<EstateAppraisal>, IEstateAppraisalService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса EstateAppraisalService.
        /// </summary>
        /// <param name="facade"></param>
        public EstateAppraisalService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект оценки.</returns>
        public override EstateAppraisal Create(IUnitOfWork unitOfWork, EstateAppraisal obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<EstateAppraisal> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<EstateAppraisal> objectSaver)
        {
            int? newAppraisalId = objectSaver.Dest.Appraisal != null ? objectSaver.Dest.Appraisal.ID : objectSaver.Dest.AppraisalID;
            int? oldAppraisalId = objectSaver.Src.Appraisal != null ? objectSaver.Src.Appraisal.ID : objectSaver.Src.AppraisalID;

            if (objectSaver.Src.AccountingObject != null && objectSaver.Src.AccountingObject.ID != objectSaver.Dest.AccountingObjectID)
            {
                var ao = unitOfWork.GetRepository<Entities.Accounting.AccountingObject>().Find(f => f.ID == objectSaver.Src.AccountingObject.ID);
                var owner = ao.OwnerID != null ? unitOfWork.GetRepository<Entities.Subject.Society>().Find(f => f.ID == ao.OwnerID) : null;


                if (owner != null)
                {
                    objectSaver.Src.AOOwner = owner;
                    objectSaver.Src.AOOwnerID = owner.ID;
                }
            }

            var obj =
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.AppraisalType)
                    .SaveOneObject(x => x.EstateAppraisalType)
                    .SaveOneObject(x => x.Appraisal)
                    .SaveOneObject(x => x.AccountingObject)
                    .SaveOneObject(x => x.AOOwner)
                    .SaveOneObject(x => x.AppType)
                    ;

            obj.Dest.CalculateEstateAppraisalCost(unitOfWork, newAppraisalId, oldAppraisalId);

            return obj;
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
                string err = "";
                //пропускаем первые 9 строк файла не считая строки названия колонок.
                int start = ImportHelper.GetRowStartIndex(table);
                for (int i = start; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    ImportObject(uofw, row, colsNameMapping, ref err, ref count, ref history);
                    count++;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
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
                EstateAppraisal obj = new EstateAppraisal();

                obj.FillObject(uofw, typeof(EstateAppraisal),
                    row, row.Table.Columns, ref error, ref history, colsNameMapping);
                obj.ImportDate = DateTime.Now;
                var appraisal = ImportHelper.GetValueByName(uofw, typeof(string), row, "ReportNumber", colsNameMapping);
                if (obj.Appraisal == null)
                    obj.Appraisal = ImportHelper.GetAppraisalByDateAndNumber(uofw
                        , ImportHelper.GetValueByName(uofw, typeof(string), row, "Appraisal", colsNameMapping)
                        , ImportHelper.GetValueByName(uofw, typeof(string), row, "AppraisalDate", colsNameMapping)
                        , ref error);
                if (obj.AccountingObject == null)
                {
                    var inv = ImportHelper.GetValueByName(uofw, typeof(string), row, "AccountingObject", colsNameMapping);
                    //TODO: ОБУ идентифицируется по инвентарному+ балансодержатель!!!!!!!
                    if (inv != null)
                        obj.AccountingObject =
                        uofw.GetRepository<AccountingObject>()
                        .Filter(f => !f.Hidden && !f.IsHistory && f.InventoryNumber == inv.ToString())
                        .FirstOrDefault();
                }                   
                

                EstateAppraisal newObj = null;

                if (isNew)
                    newObj = this.Create(uofw, obj);
                else
                    newObj = this.Update(uofw, obj);
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        
    }
}
