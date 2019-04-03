using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Base.Extensions;
using CorpProp.Entities.Security;
using CorpProp.Services.Base;
using CorpProp.Services.Import.BulkMerge;
using CorpProp.Extentions;
using Base.Service.Log;

namespace CorpProp.Services.Subject
{
    public interface ISocietyService : ITypeObjectService<Society>, IExcelImportEntity
    {

    }

    public class SocietyService : TypeObjectService<Society>, ISocietyService
    {

        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;
        private readonly IBaseObjectServiceFacade _facade;
        private readonly IAccessService _accessService;

        public SocietyService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _facade = facade;
            _securityUserService = securityUserService;
            _workflowService = workflowService;
            _accessService = accessService;
        }

        public override Society Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }

        public override IQueryable<Society> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }

        public override IQueryable<Society> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            //return base.GetAllByDate(uow, date);
            var society = base.GetAllByDate(uow, date);
            var q = society.Where(f => (f.DateExclusionFromPerimeter != null && f.DateExclusionFromPerimeter > date) || (f.DateExclusionFromPerimeter == null));
            return q;
        }

        void SetResponsable(IUnitOfWork unitOfWork, Society obj)
        {
            if (obj.ResponsableForResponse != null)
            {
                var users = unitOfWork.GetRepository<SibUser>().Filter(user => user.SocietyID == obj.ID && user.ID != obj.ResponsableForResponse.ID);
                users.ForEach(user => user.ResponsibleOnRequest = false);
                obj.ResponsableForResponse.ResponsibleOnRequest = true;
                unitOfWork.SaveChanges();
            }
        }

        public override Society Update(IUnitOfWork unitOfWork, Society obj)
        {
            var result = base.Update(unitOfWork, obj);
            SetResponsable(unitOfWork, result);
            return result;
        }

        public override Society Create(IUnitOfWork unitOfWork, Society obj)
        {
            var result = base.Create(unitOfWork, obj);
            SetResponsable(unitOfWork, result);
            return result;
        }


        public void OnActionExecuting(ActionExecuteArgs args)
        {

        }



        public int GetWorkflowID(IUnitOfWork unitOfWork, BaseObject obj)
        {
            return 0;//Workflow.Default;
        }

        public void BeforeInvoke(BaseObject obj)
        {
        }

        protected override IObjectSaver<Society> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Society> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver) ;
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
                //Type type = typeof(Society);
                //ImportLoader.ImportSociety(uofw, _accessService, table, colsNameMapping, type, ref count, ref history);

                string err = "";              
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


        public List<Society> FindObjects(IUnitOfWork uofw, string idEup)
        {
            idEup = idEup.Trim();
            List<Society> list = new List<Society>();
            list = uofw.GetRepository<Society>().Filter(x =>
            !x.Hidden &&
            !x.IsHistory &&
            x.IDEUP != null && x.IDEUP == idEup).ToList<Society>();
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
                Society obj = null;
                //читаем СДП              
                var idEupObj = ImportHelper.GetValueByName(uofw, typeof(string), row, "IDEUP", colsNameMapping);

                //ищем в Системе
                if (idEupObj != null && !String.IsNullOrWhiteSpace(idEupObj.ToString().Trim()))
                {
                    string idEup = ImportHelper.GetIDEUP(idEupObj);
                    List<Society> list = FindObjects(uofw, idEup.ToString());
                    if (list == null || list.Count == 0)
                        obj = uofw.GetRepository<Society>().Create(new Society());
                    else if (list.Count > 1)
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе найдено более одной записи.", error, ErrorType.System);

                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }                    
                    //контроль версий
                    DateTime? dateFrom = null;
                    DateTime? dateTo = null;
                    var df = ImportHelper.GetValueByName(uofw, typeof(string), row, "DataDateFrom", colsNameMapping);
                    var dt = ImportHelper.GetValueByName(uofw, typeof(string), row, "DataDateTo", colsNameMapping);
                    if (df != null)
                        dateFrom = df.ToString().GetDate();
                    if (dt != null)
                        dateTo = dt.ToString().GetDate();

                    SocietyVersionControl version = new SocietyVersionControl(uofw
                        , row.Table
                        , colsNameMapping
                        , DateTime.Now
                        , ref history);
                    version.StartPeriod = dateFrom ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); 
                    version.EndPeriod = dateTo ?? version.StartPeriod.AddMonths(1).AddDays(-1); 

                    version.Execute(row, ref obj, ref history);
                    version = null;
                    uofw.SaveChanges();

                }
                else
                {
                    error += $"Неверное значение ИДЕУП. {System.Environment.NewLine}";
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
