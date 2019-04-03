using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Extentions;
using Base.Service.Log;

namespace CorpProp.Services.CorporateGovernance
{
    public interface IInvestmentService : CorpProp.Services.Base.ITypeObjectService<Investment>, IWFObjectService//, IExcelImportEntity
    {

    }

    public class InvestmentService : TypeObjectService<Investment>, IInvestmentService
    {
        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;

        public InvestmentService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, ILogService logger
            ) : base(facade, logger)
        {
            _logger = logger;
            _securityUserService = securityUserService;
            _workflowService = workflowService;

        }
        public override Investment Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }
        public override IQueryable<Investment> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }
        public override Investment Update(IUnitOfWork unitOfWork, Investment obj)
        {
            return base.Update(unitOfWork, obj);
        }

        public override Investment Create(IUnitOfWork unitOfWork, Investment obj)
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



        protected override IObjectSaver<Investment> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Investment> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.InvestmentType)
                    .SaveOneObject(x => x.SocietyIssuer)
                    ;
        }

        public void Import(IUnitOfWork uofw, IExcelDataReader reader, Dictionary<string, string> colsNameMapping, ref string error, ref int count)
        {
            try
            {

                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[0];
                ImportStarter.DeleteEmptyRows(ref entryTable);
                Type tt = ImportHelper.GetEntityType(entryTable, ref error);

                if (tt != null && Type.Equals(tt, typeof(Investment)))
                {
                    //пропускаем первые 6 строк файла не считая строки названия колонок.
                    for (int i = 6; i < entryTable.Rows.Count; i++)
                    {
                        var rowError = "";
                        var row = entryTable.Rows[i];
                        ImportObject(uofw, row, colsNameMapping, ref rowError, ref count);
                        if (!String.IsNullOrEmpty(rowError))
                            error += $"Строка {i + 2} ошибка: {rowError}.{System.Environment.NewLine}";
                    }
                }
                else
                {
                    if (tt != null)
                        error += $"Вы пытаетесь импортировать в реестр объекты иного типа.{System.Environment.NewLine}";
                }



            }
            catch (Exception ex)
            {
                error += $"Ошибка: {ex.Message}.{System.Environment.NewLine}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="numb"></param>
        /// <param name="idEup"></param>
        /// <returns></returns>
        public List<Investment> FindObjectsByNumber(IUnitOfWork uofw, string numb, string idEup, DateTime dt)
        {
            List<Investment> list = new List<Investment>();
            list = uofw.GetRepository<Investment>().Filter(x =>
            x.IDEUP ==  idEup && x.RegistrationNumber == numb 
            && x.DateRelease == dt ).ToList<Investment>();
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public void ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping, ref string error, ref int count)
        {
            try
            {
                bool isNew = true;
                Investment obj = null;

                //читаем инв номер и ИД ЕУП               
                var RegNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "RegistrationNumber", colsNameMapping);
                var IDEUP = ImportHelper.GetValueByName(uofw, typeof(string), row, "IDEUP", colsNameMapping);
                var DateRelease = ImportHelper.GetValueByName(uofw, typeof(DateTime), row, "DateRelease", colsNameMapping);

               
                if (RegNumb != null && IDEUP != null && DateRelease != null
                    && !String.IsNullOrEmpty(RegNumb.ToString())
                    && !String.IsNullOrEmpty(IDEUP.ToString()))
                {
                    DateTime df = DateTime.MinValue;
                    DateTime.TryParse(DateRelease.ToString(), out df);

                    //TODO: почистить
                    List<Investment> list = FindObjectsByNumber(uofw, RegNumb.ToString(), IDEUP.ToString(), df);
                    if (list == null || list.Count == 0)
                        obj = new Investment();
                    else if (list.Count > 1)
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        //obj = ImportHelper.FillObject(uofw, typeof(Investment),
                        //   obj, row, row.Table.Columns, ref error) as Investment;                       
                        obj.SocietyIssuer = ImportHelper.GetSocietyByIDEUP(uofw, IDEUP.ToString());
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            count++;
                            if (isNew)
                                this.Create(uofw, obj);
                            else
                                this.Update(uofw, obj);
                        }
                    }
                }
                else
                {
                    error += $"Неверное значение ИД ЕУП, Регистрационного номера или даты выпуска. {System.Environment.NewLine}";
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                error += $"Ошибка: {ex.Message}.{System.Environment.NewLine}";
            }
        }


    }
}
