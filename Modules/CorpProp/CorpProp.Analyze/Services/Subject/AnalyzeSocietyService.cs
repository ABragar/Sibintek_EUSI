using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Extensions;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Analyze.Entities.Subject;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.Import;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using CorpProp.Services.Subject;

namespace CorpProp.Analyze.Services.Subject
{
    public interface IAnalyzeSocietyService : ITypeObjectService<AnalyzeSociety>, IExcelImportEntity
    {

    }

    public class AnalyzeSocietyService : TypeObjectService<AnalyzeSociety>, IAnalyzeSocietyService
    {
        private readonly ILogService _logger;

        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;
        private readonly IBaseObjectServiceFacade _facade;

        public AnalyzeSocietyService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, ILogService logger
            ) : base(facade, logger)
        {
            _logger = logger;
            _facade = facade;
            _securityUserService = securityUserService;
            _workflowService = workflowService;
        }

        protected override IObjectSaver<AnalyzeSociety> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<AnalyzeSociety> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(account => account.Owner);
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
                //пропускаем первые 10 строк (историчность) файла не считая строки названия колонок.
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
            List<Society> list = new List<Society>();
            list = uofw.GetRepository<Society>().Filter(x =>
                !x.Hidden &&
                !x.IsHistory &&
                x.IDEUP != null && x.IDEUP == idEup).ToList<Society>();
            return list;
        }

        public List<AnalyzeSociety> FindObjectsAnalizeSocieties(IUnitOfWork uofw, string idEup)
        {
            List<AnalyzeSociety> list = new List<AnalyzeSociety>();
            list = uofw.GetRepository<AnalyzeSociety>().Filter(x =>
                !x.Hidden &&
                !x.IsHistory &&
                x.Owner.IDEUP != null && x.Owner.IDEUP == idEup).ToList<AnalyzeSociety>();
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
                AnalyzeSociety obj = null;       
               var idEupObj = ImportHelper.GetValueByName(uofw, typeof(string), row, "Owner", colsNameMapping);

                if (idEupObj != null && !String.IsNullOrEmpty(idEupObj.ToString()))
                {
                    string idEup = ImportHelper.GetIDEUP(idEupObj);

//                    List<Society> societyList = FindObjects(uofw, idEup.ToString());
                    List<AnalyzeSociety> list = FindObjectsAnalizeSocieties(uofw, idEup.ToString());

//                    if (societyList == null || societyList.Count == 0)
//                    {
//                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе не найдено родительского объекта.", error, ErrorType.System);
//                    }
//                    else if (societyList.Count > 1)
//                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе найдено более одной записи.", error, ErrorType.System);
//                    else if (societyList.Count == 1)
//                    {
                        if (list == null || list.Count == 0)
                            obj = new AnalyzeSociety();
                        else if (list.Count > 1)
                            history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе найдено более одной записи.", error, ErrorType.System);
                        else if (list.Count == 1)
                        {
                            isNew = false;
                            obj = list[0];
                        }
//                    }
                    
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(AnalyzeSociety),
                             row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            if (isNew)
                            {
                                this.CreateFromImport(uofw, obj, history);
                            }
                            else
                            {
                                UpdateFromImport(uofw, obj, history);
                            }
                        }
                    }
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
        public AnalyzeSociety CreateFromImport(
            IUnitOfWork uow
            , AnalyzeSociety obj
            , ImportHistory history)
        {
            var bo = this.Create(uow, obj);
            uow.GetRepository<ImportObject>()
                .Create(new ImportObject(bo, history.Oid, TypeImportObject.CreateObject));

            uow.SaveChanges();
            return bo;
        }
        public AnalyzeSociety UpdateFromImport(
            IUnitOfWork uow
            , AnalyzeSociety obj
            , ImportHistory history)
        {
            var bo = this.Update(uow, obj);
            uow.GetRepository<ImportObject>()
                .Update(new ImportObject(bo, history.Oid, TypeImportObject.CreateObject));

            uow.SaveChanges();
            return bo;
        }

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
    }
}