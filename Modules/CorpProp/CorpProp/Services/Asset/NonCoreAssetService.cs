using Base.DAL;
using Ambient =Base.Ambient;
using Base.Service;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Extentions;
using CorpProp.Services.Base;
using Base.Service.Log;

namespace CorpProp.Services.Asset
{
    /// <summary>
    ///     Предоставляет данные и методы сервиса объекта - объект ННА.
    /// </summary>
    public interface INonCoreAssetService : ITypeObjectService<NonCoreAsset>
    {
    }

    /// <summary>
    ///     Представляет сервис для работы с объектом - объект ННА.
    /// </summary>
    public class NonCoreAssetService : TypeObjectService<NonCoreAsset>, INonCoreAssetService
    {

        private readonly ILogService _logger;
        /// <summary>
        ///     Инициализирует новый экземпляр класса NonCoreAssetService.
        /// </summary>
        /// <param name="facade"></param>
        public NonCoreAssetService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        public override NonCoreAsset Create(IUnitOfWork unitOfWork, NonCoreAsset obj)
        {
            var result = base.Create(unitOfWork, obj);
            if (result.EstateObject != null)
                result.EstateObject.IsNonCoreAsset = true;
            unitOfWork.SaveChanges();
            return result;
        }

        /// <summary>
        ///     Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<NonCoreAsset> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<NonCoreAsset> objectSaver)
        {
            var obj=
                base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.EstateObject)
                .SaveOneObject(x => x.NonCoreAssetType)
                .SaveOneObject(x => x.NonCoreAssetStatus)
                .SaveOneObject(x => x.AssetOwner)
                .SaveOneObject(x => x.AssetMainOwner)
                .SaveOneObject(x => x.ApprovedListDocument)
                .SaveOneObject(x => x.FormDocument)
                .SaveOneObject(x => x.JustificationOfAuthorityDocument)
                .SaveOneObject(x => x.WorkingGroupConclusionDocument)
                .SaveOneObject(x => x.NonCoreAssetOwnerCategory)
                .SaveOneObject(x => x.AppraisalAssignment)
                ;
           
            //TODO: а как же ролевой доступ? :'(
            if (obj.Dest.NonCoreAssetStatus != null)
            {
                if (obj.Dest.NonCoreAssetStatus.Code == "05"
                    || obj.Dest.NonCoreAssetStatus.Code == "06"
                    && !Ambient.AppContext.SecurityUser.IsFromCauk(unitOfWork))
                    throw new Exception(
                        $"Изменять статус ННА на <{obj.Dest.NonCoreAssetStatus.Name}> могут только сотрудники ЦАУК!");

                if (obj.Dest.NonCoreAssetStatus.Code == "08" && obj.Dest.EstateObjectID != null)
                    SetAppraisalDescription(unitOfWork, (int)obj.Dest.EstateObjectID);
            }

            return obj;
        }

        /// <summary>
        /// Импорт ГГР из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="reader"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        public void Import(IUnitOfWork uofw, IExcelDataReader reader, NonCoreAssetList ncaList, ref int count, ref ImportHistory history)
        {
            try
            {

                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[1];
                ImportStarter.DeleteEmptyRows(ref entryTable);
                string typeName = ImportHelper.FindSystemName(entryTable);
                Type type = TypesHelper.GetTypeByName(typeName);

                if (type != null && Type.Equals(type, typeof(NonCoreAsset)))
                {
                    Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(entryTable);

                    //пропускаем первые 9 строк файла не считая строки названия колонок.
                    int start = ImportHelper.GetRowStartIndex(entryTable);
                    for (int i = start; i < entryTable.Rows.Count; i++)
                    {
                        var rowError = "";
                        var row = entryTable.Rows[i];

                        NonCoreAsset item = ImportObject(uofw, row, colsNameMapping, ref rowError, ref count, ref history);

                        if (item != null)
                        {
                            uofw.GetRepository<NonCoreAssetAndList>().Create(new NonCoreAssetAndList()
                            {
                                ID = 0,
                                ObjRigth = ncaList,
                                ObjLeft = item
                            });
                            uofw.SaveChanges();
                        }
                        else
                        {
                            history.ImportErrorLogs.AddError(i, null, null, rowError, ErrorType.System);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="numb"></param>
        /// <param name="idEup"></param>
        /// <returns></returns>
        public List<NonCoreAsset> FindObjects(IUnitOfWork uofw, string type, string idEup)
        {
            
            List<NonCoreAsset> list = new List<NonCoreAsset>();
            return list;
            list = uofw.GetRepository<NonCoreAsset>().Filter(x =>
            (x.AssetOwner != null && x.AssetOwner.IDEUP == idEup) 
            && (x.NonCoreAssetType != null && x.NonCoreAssetType.Name == type) 
            && !x.Hidden).ToList<NonCoreAsset>();
            return list;
        }

        /// <summary>
        /// Имопртирует из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public NonCoreAsset ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping, ref string error, ref int count, ref ImportHistory history)
        {
            try
            {
                bool isNew = true;
                NonCoreAsset obj = null;

                //читаем инв номер и ИД ЕУП               
                var type = ImportHelper.GetValueByName(uofw, typeof(string), row, "NonCoreAssetType", colsNameMapping);
                var IDEUP = ImportHelper.GetValueByName(uofw, typeof(string), row, "AssetOwner", colsNameMapping);

                //ищем в Системе ОБУ
                if (IDEUP != null && type != null
                    && !String.IsNullOrEmpty(type.ToString())
                    && !String.IsNullOrEmpty(IDEUP.ToString()))
                {
                    string ideup = ImportHelper.GetIDEUP(IDEUP);
                    //TODO: почистить
                    List<NonCoreAsset> list = FindObjects(uofw, type.ToString(), ideup);
                    if (list == null || list.Count == 0)
                        obj = new NonCoreAsset();
                    else if (list.Count > 1)
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }

                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(NonCoreAsset),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            NonCoreAssetStatus status = uofw.GetRepository<NonCoreAssetStatus>()
                                .Filter(f => !f.Hidden && f.Code == "03")
                                .FirstOrDefault();

                            obj.NonCoreAssetStatus = status;
                            obj.NonCoreAssetStatusID = status.ID;

                            count++;
                            if (isNew)
                                return this.Create(uofw, obj);
                            else
                                return this.Update(uofw, obj);
                        }
                    }
                }
                else
                {
                    error += $"Неверное значение года, ИД ЕУП или типа. {System.Environment.NewLine}";
                    obj = null;
                }
                return null;
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
                return null;
            }
        }

        /// <summary>
        /// Отметка в описании оценке об исключении ОЦ из ННА.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="estateId">ИД ОИ.</param>
        private void SetAppraisalDescription(IUnitOfWork unitOfWork, int estateId)
        {
            List<Appraisal> appraisals = unitOfWork.GetRepository<AccountingObject>().Filter(f => !f.Hidden && f.EstateID == estateId)
                .Join(unitOfWork.GetRepository<EstateAppraisal>().Filter(f => !f.Hidden), e => e.ID,
                    o => o.AccountingObjectID, (e, o) => o)
                .Join(unitOfWork.GetRepository<Appraisal>().Filter(f => !f.Hidden), e => e.AppraisalID,
                    o => o.ID, (e, o) => o).ToList();

            foreach (Appraisal appraisal in appraisals)
            {
                appraisal.Description += string.IsNullOrEmpty(appraisal.Description) ? "Объект исключен из ННА." : $"{Environment.NewLine}Объект исключен из ННА.";
                unitOfWork.GetRepository<Appraisal>().Update(appraisal);
            }

            unitOfWork.SaveChanges();
        }

        public override NonCoreAsset CreateDefault(IUnitOfWork unitOfWork)
        {

            var obj = base.CreateDefault(unitOfWork);
            if (obj.NonCoreAssetStatus == null && obj.ID == 0)
            {
                NonCoreAssetStatus defaultStatus = unitOfWork.GetRepository<NonCoreAssetStatus>()
                    .Filter(f => !f.Hidden && f.Code == "01")
                    .FirstOrDefault();

                if (defaultStatus != null)
                {
                    obj.NonCoreAssetStatus = defaultStatus;
                    obj.NonCoreAssetStatusID = defaultStatus.ID;
                }
            }
            if (obj.SpecialNonCoreAssetCriteria == "" || obj.SpecialNonCoreAssetCriteria == null)
                obj.SpecialNonCoreAssetCriteria = "Отсутствуют";
            return obj;
        }

       
    }
}
