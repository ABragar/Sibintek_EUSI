using Base.DAL;
using Base.Events;
using Base.Service;
using Base.Service.Log;
using Base.Utils.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CorpProp.Services.Law
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - ГГР.
    /// </summary>
    public interface IScheduleStateTerminateRecordService : ITypeObjectService<ScheduleStateTerminateRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - ГГР.
    /// </summary>
    public class ScheduleStateTerminateRecordService : TypeObjectService<ScheduleStateTerminateRecord>, IScheduleStateTerminateRecordService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateTerminateService.
        /// </summary>
        /// <param name="facade"></param>
        public ScheduleStateTerminateRecordService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>ГГР.</returns>
        public override ScheduleStateTerminateRecord Create(IUnitOfWork unitOfWork, ScheduleStateTerminateRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        protected override IObjectSaver<ScheduleStateTerminateRecord> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ScheduleStateTerminateRecord> objectSaver)
        {
            if (objectSaver != null && objectSaver.Src != null)
            {
                var obj = objectSaver.Src;

                if((obj.DateActualRegistration != null && obj.DateActualFilingDocument != null) && (obj.DateActualRegistration.Value.Date < obj.DateActualFilingDocument.Value.Date))
                    throw new Exception("Дата подачи документов больше даты регистрации.");

                if ((obj.DateShownDocumentRight != null && obj.DateActualFilingDocument != null) && (obj.DateShownDocumentRight.Value.Date > obj.DateActualFilingDocument.Value.Date))
                    throw new Exception("Дата предоставления документов больше даты подачи.");

                if ((obj.DateShownDocumentRight != null && obj.DateActualRegistration != null) && (obj.DateActualRegistration.Value.Date < obj.DateShownDocumentRight.Value.Date))
                    throw new Exception("Дата предоставления документов больше даты регистрации.");

                var aoID = obj.AccountingObject?.ID;
                if (aoID != null)
                {
                    var aObject = unitOfWork.GetRepository<CorpProp.Entities.Accounting.AccountingObject>().Find(x => x.ID == aoID);
                    if (aObject != null)
                    {
                        objectSaver.Dest.InServiceDate = aObject.InServiceDate;
                        var ownerID = aObject?.OwnerID;
                        if (ownerID != null)
                        {    
                            var owner = unitOfWork.GetRepository<CorpProp.Entities.Subject.Society>().Find(x => x.ID == ownerID);
                            objectSaver.Dest.Owner = owner;
                        }
                    }
                }

            }

            var saver = base.GetForSave(unitOfWork, objectSaver)
                //.SaveOneObject(x => x.RightBefore)
                .SaveOneObject(x => x.AccountingObject)
                .SaveOneObject(x => x.ScheduleStateTerminate)
                .SaveOneObject(x => x.Owner)
                .SaveOneObject(x => x.ResponsibleUnitProvidingDocuments)
                ;
            saver.Dest.SetAccountingInfo();
            return saver;
        }

        /// <summary>
        /// Импорт из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="reader"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        public void Import(IUnitOfWork uofw, IExcelDataReader reader, ref int count, ref ImportHistory history, ScheduleStateTerminate parentObj = null)
        {
            try
            {
                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[1];
                ImportStarter.DeleteEmptyRows(ref entryTable);
                string typeName = ImportHelper.FindSystemName(entryTable);                
                Type type = TypesHelper.GetTypeByName(typeName);

                if (type != null && Type.Equals(type, typeof(ScheduleStateTerminateRecord)))
                {
                    Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(entryTable);
                    int start = ImportHelper.GetRowStartIndex(entryTable);
                    for (int i = start; i < entryTable.Rows.Count; i++)
                    {
                        var rowError = "";
                        var row = entryTable.Rows[i];

                        ImportObject(uofw, row, colsNameMapping, ref rowError, ref count, ref history, parentObj);
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
        public List<ScheduleStateTerminateRecord> FindObjects(IUnitOfWork uofw, string numb, string idEup)
        {
            List<ScheduleStateTerminateRecord> list = new List<ScheduleStateTerminateRecord>();
            list = uofw.GetRepository<ScheduleStateTerminateRecord>().Filter(x =>
            x.Owner != null && x.Owner.IDEUP == idEup
            && x.InventoryNumber == numb).ToList<ScheduleStateTerminateRecord>();
            return list;
        }

        /// <summary>
        /// Имопртирует из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public void ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping, ref string error, ref int count, ref ImportHistory history, ScheduleStateTerminate parentObj = null)
        {
            try
            {
                bool isNew = true;
                ScheduleStateTerminateRecord obj = null;

                //читаем инв номер и ИД ЕУП               
                var invNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryNumber", colsNameMapping);
                var owner = ImportHelper.GetValueByName(uofw, typeof(string), row, "Owner", colsNameMapping);


                //TODO: почистить
                if (owner != null && invNumb != null
                    && !String.IsNullOrEmpty(owner.ToString())
                    && !String.IsNullOrEmpty(invNumb.ToString()))
                {
                    List<ScheduleStateTerminateRecord> list = null;
                    string ideup = ImportHelper.GetIDEUP(owner);
                    list = FindObjects(uofw, invNumb.ToString(), ideup);
                    if (list == null || list.Count == 0)
                        obj = new ScheduleStateTerminateRecord();
                    else if (list.Count > 1)
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(ScheduleStateTerminateRecord),
                          row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            obj.Owner = owner != null ? ImportHelper.GetSocietyByIDEUP(uofw, ideup) : null;
                            obj.ScheduleStateTerminate = parentObj;
                            obj.GroupName = (obj.Owner != null && obj.ScheduleStateTerminate != null && obj.ScheduleStateTerminate.Society != null) &&
                                (obj.Owner.ID == obj.ScheduleStateTerminate.Society.ID) ? "Общество" : obj.ScheduleStateTerminate.Society?.ShortName ?? "";

                            obj.Year = obj.ScheduleStateTerminate?.Year;
                            obj.DatePlannedFilingDocumentGroup = obj.DatePlannedFilingDocument != null ? (DateTime?)new DateTime(obj.DatePlannedFilingDocument.Value.Year, obj.DatePlannedFilingDocument.Value.Month, 1) : null;
                            obj.AccountingObject = uofw.GetRepository<AccountingObject>()
                                .Filter(f => !f.Hidden && !f.IsHistory &&
                                    f.Owner.ID == obj.Owner.ID && f.InventoryNumber == obj.InventoryNumber)
                                .FirstOrDefault();
                            obj.SocietyName = obj.ScheduleStateTerminate?.Society?.ShortName ?? "";

                            count++;
                            if (isNew)
                                this.Create(uofw, obj);
                            else
                                this.Update(uofw, obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error += $"Ошибка: {ex.ToStringWithInner()}.{System.Environment.NewLine}";
            }
        }


        public override void OnEvent(IOnCreate<ScheduleStateTerminateRecord> evnt)
        {
            
            evnt.Modified.SetSSRAccountingObject(evnt.UnitOfWork, evnt.Original?.AccountingObject);
        }

        public override void OnEvent(IOnUpdate<ScheduleStateTerminateRecord> evnt)
        {
            
            evnt.Modified.SetSSRAccountingObject(evnt.UnitOfWork, evnt.Original?.AccountingObject);
        }
    }
}
