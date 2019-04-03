using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using Base.Utils.Common;
using CorpProp.Common;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.DocumentFlow
{
    public interface ISibDealService : ITypeObjectService<SibDeal>, IExcelImportEntity
    {

    }
    public class SibDealService : TypeObjectService<SibDeal>, ISibDealService
    {
        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;

        public SibDealService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, ILogService logger
            ) : base(facade, logger)
        {
            _logger = logger;
            _securityUserService = securityUserService;
            _workflowService = workflowService;

        }

        protected override IObjectSaver<SibDeal> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SibDeal> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.DealType)
                    .SaveOneObject(x => x.DocKind)
                    .SaveOneObject(x => x.ContragentKind)
                    .SaveOneObject(x => x.SibDealStatus)
                    .SaveOneObject(x => x.InformationSource)
                    .SaveOneObject(x => x.ParentDeal)
                    .SaveOneObject(x => x.ConsolidationUnit)
                    .SaveOneObject(x => x.DocTypeOperation)
                   
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

        public List<SibDeal> FindObjects(IUnitOfWork uofw, string sysNum)
        {
            List<SibDeal> list = new List<SibDeal>();
            list = uofw.GetRepository<SibDeal>().Filter(x => 
            !x.IsHistory &&
            x.SystemNumber != null && x.SystemNumber == sysNum
            && !x.Hidden).ToList<SibDeal>();
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
                SibDeal obj = null;
                //читаем СДП              
                var sysNumber = ImportHelper.GetValueByName(uofw, typeof(string), row, "SystemNumber", colsNameMapping);

                //ищем в Системе
                if (sysNumber != null 
                    && !String.IsNullOrEmpty(sysNumber.ToString()))
                {
                    List<SibDeal> list = FindObjects(uofw, sysNumber.ToString());
                    if (list == null || list.Count == 0)
                        obj = new SibDeal();
                    else if (list.Count > 1)
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе найдено более одной записи.", error, ErrorType.System);
                        
                    
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }

                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(SibDeal),
                          row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;

                        SibDeal newObj = null;

                        if (isNew)
                            newObj = this.Create(uofw, obj);
                        else
                            newObj = this.Update(uofw, obj);
                    }
                }
                else
                {
                    error += $"Неверное значение системного номера. {System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                    return;
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
