using Base.DAL;
using Base.Events;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.Security;
using CorpProp.Entities.Settings;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CorpProp.Services.Law
{
    public interface IScheduleStateRegistrationRecordService : ITypeObjectService<ScheduleStateRegistrationRecord>, ISibNotification
    {

    }
    public class ScheduleStateRegistrationRecordService : TypeObjectService<ScheduleStateRegistrationRecord>, IScheduleStateRegistrationRecordService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateRegistrationService.
        /// </summary>
        /// <param name="facade"></param>
        public ScheduleStateRegistrationRecordService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>ГГР.</returns>
        public override ScheduleStateRegistrationRecord Create(IUnitOfWork unitOfWork, ScheduleStateRegistrationRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        public override IQueryable<ScheduleStateRegistrationRecord> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ScheduleStateRegistrationRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ScheduleStateRegistrationRecord> objectSaver)
        {
            var saver =
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.RightAfter)
                    .SaveOneObject(x => x.AccountingObject)
                    .SaveOneObject(x => x.ScheduleStateRegistration)
                    .SaveOneObject(x => x.RegistrationBasis)
                    .SaveOneObject(x => x.Owner)
                    .SaveOneObject(x => x.ResponsibleUnitProvidingDocuments)
                    .SaveOneObject(x => x.EmployeeUploadedData)
                    ;
            var obj = saver.Dest;

            if ((obj.DateActualRegistration != null && obj.DateActualFilingDocument != null) &&
                    (obj.DateActualRegistration.Value.Date < obj.DateActualFilingDocument.Value.Date))
                throw new Exception("Дата подачи документов больше даты регистрации.");

            if ((obj.DateShownDocumentRight != null && obj.DateActualFilingDocument != null) &&
                (obj.DateShownDocumentRight.Value.Date > obj.DateActualFilingDocument.Value.Date))
                throw new Exception("Дата предоставления документов больше даты подачи.");

            if ((obj.DateShownDocumentRight != null && obj.DateActualRegistration != null) &&
                (obj.DateActualRegistration.Value.Date < obj.DateShownDocumentRight.Value.Date))
                throw new Exception("Дата предоставления документов больше даты регистрации.");

            if ((obj.DatePlannedRegistration != null && obj.DatePlannedFilingDocument != null) &&
                (obj.DatePlannedRegistration.Value.Date < obj.DatePlannedFilingDocument.Value.Date))
                throw new Exception("Плановая дата подачи документов больше плановой даты регистрации.");

            if ((obj.DateShownDocumentRight != null && obj.DatePlannedFilingDocument != null) &&
                (obj.DateShownDocumentRight.Value.Date > obj.DatePlannedFilingDocument.Value.Date))
                throw new Exception("Дата предоставления документов больше плановой даты подачи.");

            if ((obj.DateShownDocumentRight != null && obj.DatePlannedRegistration != null) &&
                (obj.DatePlannedRegistration.Value.Date < obj.DateShownDocumentRight.Value.Date))
                throw new Exception("Дата предоставления документов больше плановой даты регистрации.");

            if ((obj.DateRegDoc != null && obj.DateShownDocumentRight != null) &&
                (obj.DateShownDocumentRight.Value.Date < obj.DateRegDoc.Value.Date))
                throw new Exception("Дата передачи документов в службу Куратора меньше Даты оформления правоустанавливающего документа.");

            var aoID = obj.AccountingObject?.ID;
            if (aoID != null)
            {
                var aObject = unitOfWork.GetRepository<CorpProp.Entities.Accounting.AccountingObject>().Find(x => x.ID == aoID);
                if (aObject != null)
                {
                    objectSaver.Dest.InventoryNumber = aObject.InventoryNumber;
                    objectSaver.Dest.InServiceDate = aObject.InServiceDate;
                    var ownerID = aObject?.OwnerID;
                    if (ownerID != null)
                    {
                        var owner = unitOfWork.GetRepository<CorpProp.Entities.Subject.Society>().Find(x => x.ID == ownerID);
                        objectSaver.Dest.Owner = owner;
                    }
                }
            }

            saver.Dest.SetAccountingInfo();
            return saver;
            
        }

        /// <summary>
        /// Импорт из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <param name="history"></param>
        /// <param name="parentObj"></param>
        public void Import(IUnitOfWork uofw, IExcelDataReader reader, ref int count, ref ImportHistory history, ScheduleStateRegistration parentObj = null)
        {
            try
            {
                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[1];
                ImportStarter.DeleteEmptyRows(ref entryTable);

                string typeName = ImportHelper.FindSystemName(entryTable);
                Type type = TypesHelper.GetTypeByName(typeName);

                if (type != null && Type.Equals(type, typeof(ScheduleStateRegistrationRecord)))
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
        public List<ScheduleStateRegistrationRecord> FindObjects(IUnitOfWork uofw, string numb, string idEup)
        {
            List<ScheduleStateRegistrationRecord> list = new List<ScheduleStateRegistrationRecord>();
            list = uofw.GetRepository<ScheduleStateRegistrationRecord>().Filter(x =>
            x.Owner != null && x.Owner.IDEUP == idEup
            && x.InventoryNumber == numb).ToList<ScheduleStateRegistrationRecord>();
            return list;
        }

        /// <summary>
        /// Имопртирует из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>
        /// <param name="colsNameMapping">Мэппинг колонок.</param>
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        /// <param name="history">Журнал истории.</param>
        /// <param name="parentObj">ГГР.</param>
        public void ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping, ref string error, ref int count, ref ImportHistory history, ScheduleStateRegistration parentObj = null)
        {
            try
            {
                bool isNew = true;
                ScheduleStateRegistrationRecord obj = null;

                //читаем инв номер и ИД ЕУП               
                var invNumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryNumber", colsNameMapping);
                var owner = ImportHelper.GetValueByName(uofw, typeof(string), row, "Owner", colsNameMapping);
                

                //TODO: почистить
                if (owner != null && invNumb != null
                    && !String.IsNullOrEmpty(owner.ToString())
                    && !String.IsNullOrEmpty(invNumb.ToString()))
                {
                    List<ScheduleStateRegistrationRecord> list = null;
                    string ideup = ImportHelper.GetIDEUP(owner);
                    list = FindObjects(uofw, invNumb.ToString(), ideup);
                    if (list == null || list.Count == 0)
                        obj = new ScheduleStateRegistrationRecord();
                    else if (list.Count > 1)
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(ScheduleStateRegistrationRecord),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;

                        if (history.SibUser != null)
                            obj.EmployeeUploadedData = GetUploader(uofw, history.SibUser.ID);
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            obj.Owner = owner != null ? ImportHelper.GetSocietyByIDEUP(uofw, ideup) : null;
                            obj.ScheduleStateRegistration = parentObj;
                            obj.GroupName = (obj.Owner != null && obj.ScheduleStateRegistration != null && obj.ScheduleStateRegistration.Society != null) &&
                                (obj.Owner.ID == obj.ScheduleStateRegistration.Society.ID) ? "Общество" : obj.ScheduleStateRegistration.Society?.ShortName ?? "";

                            obj.Year = obj.ScheduleStateRegistration?.Year;
                            obj.DatePlannedFilingDocumentGroup = obj.DatePlannedFilingDocument != null ? (DateTime?)new DateTime(obj.DatePlannedFilingDocument.Value.Year, obj.DatePlannedFilingDocument.Value.Month, 1) : null;

                            var ownerID = obj.Owner?.ID;
                            if (ownerID != null)
                                obj.AccountingObject = uofw.GetRepository<AccountingObject>()
                                .Filter(f => !f.Hidden && !f.IsHistory &&
                                f.Owner.ID == ownerID && f.InventoryNumber == obj.InventoryNumber)
                                .FirstOrDefault();
                            obj.SocietyName = obj.ScheduleStateRegistration?.Society?.ShortName ?? "";

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
                history.ImportErrorLogs.AddError(ex);
            }
        }

        private SibUser GetUploader(IUnitOfWork uofw, int userId)
        {
            return uofw.GetRepository<SibUser>().Find(f => f.ID == userId);
        }


       

        public override void OnEvent(IOnCreate<ScheduleStateRegistrationRecord> evnt)
        {            
            evnt.Modified.SetSSRAccountingObject(evnt.UnitOfWork, evnt.Original?.AccountingObject);
        }

        public override void OnEvent(IOnUpdate<ScheduleStateRegistrationRecord> evnt)
        {            
            evnt.Modified.SetSSRAccountingObject(evnt.UnitOfWork, evnt.Original?.AccountingObject);
        }

        /// <summary>
        /// Создает список объектов уведомления.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="notification">Уведомление.</param>
        /// <returns></returns>
        public List<SibNotificationObject> PrepareLinkedObject(IUnitOfWork unitOfWork, SibNotification notification)
        {
            DateTime now = DateTime.Now;
            DateTime dtNow = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            PropertyInfo propertyInfo = typeof(ScheduleStateRegistrationRecord).GetProperty(notification.PropertyName);

            var ssrRecords = this.GetAll(unitOfWork)
                .Where(NotificationHelper.PropertyEquals<ScheduleStateRegistrationRecord, DateTime>(propertyInfo, dtNow.Date))
                .Where(w => !w.Hidden && w.ScheduleStateRegistrationID != null).ToList();
            List<SibNotificationObject> notificationObjects = new List<SibNotificationObject>();

            try
            {
                if (ssrRecords.Count > 0)
                {
                    foreach (ScheduleStateRegistrationRecord record in ssrRecords)
                    {
                        List<int> responsiblesIds = new List<int>();
                        DateTime dtEnd = new DateTime(
                            ((DateTime)propertyInfo.GetValue(record, null)).Year
                            , ((DateTime)propertyInfo.GetValue(record, null)).Month
                            , ((DateTime)propertyInfo.GetValue(record, null)).Day
                            , ((DateTime)propertyInfo.GetValue(record, null)).Hour
                            , 0
                            , 0);

                        DateTime? remindDate = NotificationHelper.CalculateRemindDateTime(notification.RemindPeriod, ((DateTime)propertyInfo.GetValue(record, null)));

                        if (remindDate == null || remindDate.Value.Date != dtNow.Date)
                            continue;

                        var ssr = unitOfWork.GetRepository<ScheduleStateRegistration>().Find(f => f.ID == record.ScheduleStateRegistrationID);

                        if (notification.SendToAllSocieties && ssr.SocietyID != null)
                            responsiblesIds = unitOfWork.GetRepository<SibUser>().All()
                                .Where(w => w.User != null && w.Society != null)
                                .Where(w => w.SocietyID == ssr.SocietyID)
                                .Select(s => s.User.ID).ToList<int>();

                        if (notification.Reciever != null && notification.Reciever.UserID != null)
                            responsiblesIds.Add((int)notification.Reciever.UserID);

                        SibNotificationObject nObject = new SibNotificationObject()
                        {
                            Subject = notification.Subject,
                            Message = notification.Message,
                            LinkBaseObject = NotificationHelper.GetLinkedObj(record),
                            Recipients = responsiblesIds
                        };

                        notificationObjects.Add(nObject);
                    }
                }
            }
            catch (Exception ex)
            {
                //return ex.ToStringWithInner();
            }

            return notificationObjects;
        }
    }
}
