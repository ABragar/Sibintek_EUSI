using Base.BusinessProcesses.Entities;
using Base.Service;
using BaseSubject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Security.Service;
using Base.BusinessProcesses.Services.Abstract;
using Base;
using CorpProp.Common;
using ExcelDataReader;
using System.Data;
using CorpProp.Helpers;
using CorpProp.Entities.Import;
using CorpProp.Helpers.Import.Extentions;
using Base.Utils.Common;
using CorpProp.Entities.Subject;
using CorpProp.Services.Base;
using Base.Service.Log;

namespace CorpProp.Services.Subject
{
    public interface ISubjectService : ITypeObjectService<BaseSubject.Subject>, IWFObjectService, IExcelImportEntity
    {

    }

    public class SubjectService : TypeObjectService<BaseSubject.Subject>, ISubjectService
    {

        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;
        private readonly AppraiserService _appraiserService;

        public SubjectService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, AppraiserService appraiserService, ILogService logger
            ) : base(facade, logger)
        {
            _logger = logger;
            _securityUserService = securityUserService;
            _workflowService = workflowService;
            _appraiserService = appraiserService;
        }
        public override BaseSubject.Subject Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }
        public override IQueryable<BaseSubject.Subject> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }
        public override BaseSubject.Subject Update(IUnitOfWork unitOfWork, BaseSubject.Subject obj)
        {
            return base.Update(unitOfWork, obj);
        }

        public override BaseSubject.Subject Create(IUnitOfWork unitOfWork, BaseSubject.Subject obj)
        {
            return base.Create(unitOfWork, obj);
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



        protected override IObjectSaver<BaseSubject.Subject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<BaseSubject.Subject> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.SubjectType)
                    .SaveOneObject(x => x.SubjectKind)
                    .SaveOneObject(x => x.Country)
                    .SaveOneObject(x => x.FederalDistrict)
                    .SaveOneObject(x => x.Region)
                    .SaveOneObject(x => x.OPF)
                    .SaveOneObject(x => x.OKTMO)
                    .SaveOneObject(x => x.OKVED)
                    .SaveOneObject(x => x.OKATO)
                    .SaveOneObject(x => x.Society)
                    ;
        }


        /// <summary>
        /// Импорт ДП из файла Excel.
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


        public List<BaseSubject.Subject> FindObjects(IUnitOfWork uofw, string ksk)
        {
            ksk = ksk.Trim();
            List<BaseSubject.Subject> list = new List<BaseSubject.Subject>();
            list = uofw.GetRepository<BaseSubject.Subject>().Filter(x =>
            x.SDP != null && x.SDP == ksk && !x.Hidden).ToList<BaseSubject.Subject>();
            return list;
        }



        /// <summary>
        /// Имопртирует ДП из строки файла.
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
                BaseSubject.Subject obj = null;
                //читаем СДП              
                var sdp = ImportHelper.GetValueByName(uofw, typeof(string), row, "SDP", colsNameMapping);

                //ищем в Системе
                if (sdp != null
                    && !String.IsNullOrWhiteSpace(sdp.ToString().Trim()))
                {
                    List<BaseSubject.Subject> list = FindObjects(uofw, sdp.ToString());
                    if (list == null || list.Count == 0)
                        obj = new BaseSubject.Subject();
                    else if (list.Count > 1)
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе найдено более одной записи.", error, ErrorType.System);

                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(BaseSubject.Subject),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;
                        obj.Society = ImportHelper.GetSocietyByKSK(uofw, obj.KSK);
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            BaseSubject.Subject newObj = null;

                            if (obj.IsAppraiser)
                            {
                                _appraiserService.ImportObject(uofw, row, colsNameMapping, ref error, ref count, ref history);
                            }
                            else
                            {
                                if (isNew)
                                    newObj = this.Create(uofw, obj);
                                else
                                    newObj = this.Update(uofw, obj);
                            }
                        }
                    }
                }
                else
                {
                    error = $"Неверное значение идентификатора ДП. {System.Environment.NewLine}";
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
