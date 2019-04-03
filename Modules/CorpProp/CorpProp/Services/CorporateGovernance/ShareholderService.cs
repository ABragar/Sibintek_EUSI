using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.CorporateGovernance
{
    public interface IShareholderService : Base.ITypeObjectService<Shareholder>, IWFObjectService, IExcelImportEntity
    {

    }

    public class ShareholderService : Base.TypeObjectService<Shareholder>, IShareholderService
    {

        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;

        public ShareholderService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, ILogService logger
            ) : base(facade, logger)
        {
            _logger = logger;
            _securityUserService = securityUserService;
            _workflowService = workflowService;

        }
        public override Shareholder Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }
        public override IQueryable<Shareholder> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }
        public override Shareholder Update(IUnitOfWork unitOfWork, Shareholder obj)
        {
            return base.Update(unitOfWork, obj);
        }

        public override Shareholder Create(IUnitOfWork unitOfWork, Shareholder obj)
        {
            return base.Create(unitOfWork, obj);
        }


        public void OnActionExecuting(ActionExecuteArgs args)
        {

        }



        public int GetWorkflowID(IUnitOfWork unitOfWork, BaseObject obj)
        {
            return 0;// Workflow.Default;
        }

        public void BeforeInvoke(BaseObject obj)
        {
        }



        protected override IObjectSaver<Shareholder> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Shareholder> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.SocietyShareholder)
                    .SaveOneObject(x => x.SocietyRecipient)
                    ;
        }


        /// <summary>
        /// Импорт ОГ из файла Excel.
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


        public List<Shareholder> FindObjects(IUnitOfWork uofw, string ridEup, string sidEup, DateTime dateFrom)
        {
            List<Shareholder> list = new List<Shareholder>();
            list = uofw.GetRepository<Shareholder>().Filter(x => 
            !x.IsHistory &&
            x.SocietyRecipient != null && x.SocietyRecipient.IDEUP == ridEup
            && x.SocietyShareholder != null && x.SocietyShareholder.IDEUP == sidEup 
            && x.DateFrom == dateFrom 
            && !x.Hidden).ToList<Shareholder>();
            return list;
        }



        /// <summary>
        /// Имопртирует ОГ из строки файла.
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
                Shareholder obj = null;
                //читаем СДП              
                var recipientIdEup = ImportHelper.GetValueByName(uofw, typeof(string), row, "SocietyRecipient", colsNameMapping);
                var shareHolderIdEup = ImportHelper.GetValueByName(uofw, typeof(string), row, "SocietyShareholder", colsNameMapping);
                var dateFrom = ImportHelper.GetValueByName(uofw, typeof(DateTime), row, "DateFrom", colsNameMapping);

                //ищем в Системе
                if (recipientIdEup != null && shareHolderIdEup != null && dateFrom != null 
                    && !String.IsNullOrEmpty(recipientIdEup.ToString())
                    && !String.IsNullOrEmpty(recipientIdEup.ToString())
                    && !String.IsNullOrEmpty(dateFrom.ToString()))
                {
                    string ridEup = ImportHelper.GetIDEUP(recipientIdEup);
                    string sidEup = ImportHelper.GetIDEUP(shareHolderIdEup);

                    List<Shareholder> list = FindObjects(uofw, ridEup, sidEup, (DateTime)dateFrom);
                    if (list == null || list.Count == 0)
                        obj = new Shareholder();
                    else if (list.Count > 1)                                            
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", $"Невозможно обновить объект. В Системе найдено более одной записи.", ErrorType.System);
                       
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }

                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(Shareholder),
                            row, row.Table.Columns, ref error, ref history, colsNameMapping) ;
                        obj.ImportDate = DateTime.Now;

                        Shareholder newObj = null;

                        if (isNew)
                            newObj = this.Create(uofw, obj);
                        else
                            newObj = this.Update(uofw, obj);
                    }
                }
                else
                {
                    error += $"Неверное значение ИДЕУП. {System.Environment.NewLine}";
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
