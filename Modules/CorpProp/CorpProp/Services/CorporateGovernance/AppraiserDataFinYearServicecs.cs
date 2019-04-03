using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Crud;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.Subject;
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
    /// Предоставляет данные и методы сервиса объекта - Данные оценщика за финансовый год.
    /// </summary>
    public interface IAppraiserDataFinYearService : ITypeObjectService<AppraiserDataFinYear>, IExcelImportEntity
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - Данные оценщика за финансовый год.
    /// </summary>
    public class AppraiserDataFinYearService: TypeObjectService<AppraiserDataFinYear>, IAppraiserDataFinYearService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса AppraiserDataFinYearService.
        /// </summary>
        /// <param name="facade"></param>
        public AppraiserDataFinYearService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии обновления объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns> Данные оценщика за финансовый год.</returns>
        public override AppraiserDataFinYear Update(IUnitOfWork unitOfWork, AppraiserDataFinYear obj)
        {
            return base.Update(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns> Данные оценщика за финансовый год.</returns>
        public override AppraiserDataFinYear Create(IUnitOfWork unitOfWork, AppraiserDataFinYear obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<AppraiserDataFinYear> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<AppraiserDataFinYear> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Appraiser)
                    .SaveOneObject(x => x.Currency)
                    //.SaveOneToMany(x => x.EstateAppraisals, x=> x.SaveOneObject(s=>s.Appraisal))
                    //.SaveOneToMany(x => x.NonCoreAssetAppraisals, x => x.SaveOneObject(s => s.Appraisal))
                    ;
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

        public List<AppraiserDataFinYear> FindObjects(IUnitOfWork uofw, DateTime dateFrom, DateTime dateTo, Appraiser appraiser)
        {
            List<AppraiserDataFinYear> list = new List<AppraiserDataFinYear>();
            list = uofw.GetRepository<AppraiserDataFinYear>().Filter(x =>
            !x.IsHistory &&
            x.DateFrom != null && x.DateFrom == dateFrom
            && x.DateTo != null && x.DateTo == dateTo
            && x.Appraiser != null && x.Appraiser.ID == appraiser.ID
            && !x.Hidden).ToList<AppraiserDataFinYear>();
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
                AppraiserDataFinYear obj = null;
                var dateFrom = ImportHelper.GetValueByName(uofw, typeof(DateTime), row, "DateFrom", colsNameMapping);
                var dateTo = ImportHelper.GetValueByName(uofw, typeof(DateTime), row, "DateTo", colsNameMapping);
                var appraiser = ImportHelper.GetValueByName(uofw, typeof(Appraiser), row, "Appraiser", colsNameMapping);

                if (dateFrom != null && dateTo != null && appraiser != null)
                {

                    //TODO: почистить
                    List<AppraiserDataFinYear> list = FindObjects(uofw, (DateTime)dateFrom, (DateTime)dateTo, (Appraiser)appraiser);
                    if (list == null || list.Count == 0)
                        obj = new AppraiserDataFinYear();
                    else if (list.Count > 1)
                        history.ImportErrorLogs.AddError($"Невозможно обновить объект. В Системе найдено более одной записи");
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(AppraiserDataFinYear),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping) ;
                        obj.ImportDate = DateTime.Now;
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            AppraiserDataFinYear newObj = null;

                            if (isNew)
                                newObj = this.Create(uofw, obj);
                            else
                                newObj = this.Update(uofw, obj);

                        }
                    }
                }
                else
                {
                    error += $"Неверное значение дат или оценщика. {System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public void CancelImport(
             IUnitOfWork uofw
            , ref ImportHistory history
            )
        {
            throw new NotImplementedException();
        }
    }
}
