using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Import;
using System.Data;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using Base.Service.Log;

namespace CorpProp.Services.Subject
{
    public interface IAppraiserService : ITypeObjectService<Appraiser>, IWFObjectService, IExcelImportEntity
    {

    }

    public class AppraiserService : TypeObjectService<Appraiser>, IAppraiserService
    {

        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;

        public AppraiserService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, ILogService logger
            ) : base(facade, logger)
        {
            _logger = logger;
            _securityUserService = securityUserService;
            _workflowService = workflowService;

        }
        public override Appraiser Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }
        public override IQueryable<Appraiser> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }
        public override Appraiser Update(IUnitOfWork unitOfWork, Appraiser obj)
        {
            return base.Update(unitOfWork, obj);
        }

        public override Appraiser Create(IUnitOfWork unitOfWork, Appraiser obj)
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



        protected override IObjectSaver<Appraiser> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Appraiser> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Currency)                  
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


        public List<Appraiser> FindObjects(IUnitOfWork uofw, string ksk)
        {
            List<Appraiser> list = new List<Appraiser>();
            list = uofw.GetRepository<Appraiser>().Filter(x =>
            !x.IsHistory &&
            x.SDP != null && x.SDP == ksk && !x.Hidden).ToList<Appraiser>();
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
                Appraiser obj = null;
                var sdp = ImportHelper.GetValueByName(uofw, typeof(string), row, "SDP", colsNameMapping);

                if (sdp != null
                    && !String.IsNullOrEmpty(sdp.ToString()))
                {
                    //TODO: почистить
                    List<Appraiser> list = FindObjects(uofw, sdp.ToString());
                    if (list == null || list.Count == 0)
                        obj = new Appraiser();
                    else if (list.Count > 1)
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе найдено более одной записи.", error, ErrorType.System);


                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(Appraiser),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            Appraiser newObj = null;

                            if (isNew)
                                newObj = this.Create(uofw, obj);
                            else
                                newObj = this.Update(uofw, obj);

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
