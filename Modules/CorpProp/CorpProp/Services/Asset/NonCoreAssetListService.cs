using System;
using System.Linq;
using Base.DAL;
using Base.Security;
using Base.Service;
using CorpProp.Entities.Asset;
using ExcelDataReader;
using System.Data;
using System.Collections.Generic;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Common;
using Base.Entities.Complex;
using CorpProp.Entities.Settings;
using System.Reflection;
using Base.Extensions;
using CorpProp.Services.Base;
using CorpProp.Extentions;
using Base.Service.Log;

namespace CorpProp.Services.Asset
{
    /// <summary>
    ///     Предоставляет данные и методы сервиса объекта - объект ННА.
    /// </summary>
    public interface INonCoreAssetListService : ITypeObjectService<NonCoreAssetList>, ISibNotification
    {
    }

    /// <summary>
    ///     Представляет сервис для работы с объектом - объект ННА.
    /// </summary>
    public class NonCoreAssetListService : TypeObjectService<NonCoreAssetList>, INonCoreAssetListService
    {

        private readonly NonCoreAssetService _ncaService;
        private readonly ILogService _logger;

        /// <summary>
        ///     Инициализирует новый экземпляр класса NonCoreAssetListService.
        /// </summary>
        /// <param name="facade"></param>
        public NonCoreAssetListService(IBaseObjectServiceFacade facade, NonCoreAssetService ncaService, ILogService logger) : base(facade, logger)
        {
            _ncaService = ncaService;
            _logger = logger;
        }

        /// <summary>
        ///     Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект ННА.</returns>
        public override NonCoreAssetList Create(IUnitOfWork unitOfWork, NonCoreAssetList obj)
        {
            return base.Create(unitOfWork, obj);
        }

        public override void Delete(IUnitOfWork unitOfWork, NonCoreAssetList obj)
        {

            base.Delete(unitOfWork, obj);
        }

        public override NonCoreAssetList Update(IUnitOfWork unitOfWork, NonCoreAssetList obj)
        {
            //var existingEntity = Get(unitOfWork, obj.ID);

            // Медленно
            //if (obj.ApprovalDeadline.Date < DateTime.Now.Date &&
            //    !existingEntity.NonCoreAssetListItems.OrderBy(t => t.ID).SequenceEqual(
            //        obj.NonCoreAssetListItems.OrderBy(t => t.ID), new NonCoreAssetListItemComparer()))
            //    throw new AccessDeniedException("Невозможно изменить ННА в перечне после срока утверждения.");

            //if (obj.AvailabilityDeadline.Date < DateTime.Now.Date &&
            //    !existingEntity.NonCoreAssetListItems.OrderBy(t => t.ID).Select(p => p.ID).SequenceEqual(
            //        obj.NonCoreAssetListItems.OrderBy(t => t.ID).Select(p => p.ID)))
            //    throw new AccessDeniedException("Невозможно изменить состав перечня ННА после срока предоставления.");

            return base.Update(unitOfWork, obj);
        }

        /// <summary>
        ///     Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<NonCoreAssetList> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<NonCoreAssetList> objectSaver)
        {
            var element = objectSaver.Src;

            ChangeNCAAndListResidualCost(element);

            using (IUnitOfWork uofw = UnitOfWorkFactory.Create())
            {
                var itemRepo = uofw.GetRepository<NonCoreAssetAndList>();
                var inventory = element.NonCoreAssetInventory == null ? null : 
                    uofw.GetRepository<NonCoreAssetInventory>().Find(f => f.ID == element.NonCoreAssetInventory.ID);
                var items = itemRepo.Filter(f => f.ObjRigthId == element.ID).ToList<NonCoreAssetAndList>();

                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        item.NonCoreAssetInventory = inventory;
                        item.NonCoreAssetInventoryID = inventory?.ID;
                        itemRepo.Update(item);
                        uofw.SaveChanges();
                    }
                }
            }


            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.NonCoreAssetInventory)
                    .SaveOneObject(x => x.Society)
                    .SaveOneObject(x => x.NonCoreAssetListType)
                    .SaveOneObject(x => x.NonCoreAssetListKind)
                    .SaveOneObject(x => x.FileCard)
                    .SaveOneObject(x => x.NonCoreAssetListState)
                                    ;
        }

        /// <summary>
        /// Импорт ГГР из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="reader"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        public void Import(IUnitOfWork uofw, IUnitOfWork histUnitOfWork, IExcelDataReader reader, Type type, ref int count, ref ImportHistory history)
        {
            try
            {

                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[0];
                ImportStarter.DeleteEmptyRows(ref entryTable);

                if (type != null && Type.Equals(type, typeof(NonCoreAssetList)))
                {
                    Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(entryTable);

                    //пропускаем первые 9 строк файла не считая строки названия колонок.
                    int start = ImportHelper.GetRowStartIndex(entryTable);
                    for (int i = start; i < entryTable.Rows.Count; i++)
                    {
                        var rowError = "";
                        var row = entryTable.Rows[i];

                        NonCoreAssetList item = ImportObject(uofw, row, colsNameMapping, ref rowError, ref count, ref history);

                        if (item != null)
                        {
                            new ImportChecker().StartDataCheck(uofw, histUnitOfWork, tables[1], typeof(NonCoreAsset), ref history);

                            if (history.ImportErrorLogs.Count > 0)
                            {
                                throw new ImportException("Ошибки при проверке.");
                            }
                            _ncaService.Import(uofw, reader, item, ref count, ref history);
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
        public List<NonCoreAssetList> FindObjects(IUnitOfWork uofw, string type, string idEup, int year)
        {
            //return null;
            List<NonCoreAssetList> list = new List<NonCoreAssetList>();
            list = uofw.GetRepository<NonCoreAssetList>().Filter(x =>
            (x.Society != null && x.Society.IDEUP == idEup)
            && (x.NonCoreAssetListType != null && x.NonCoreAssetListType.Name == type)
            && (x.Year != null && x.Year == year)
            && !x.Hidden).ToList<NonCoreAssetList>();
            return list;
        }

        /// <summary>
        /// Импортирует из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public NonCoreAssetList ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping, ref string error, ref int count, ref ImportHistory history)
        {
            try
            {
                bool isNew = true;
                int yearInt = 0;
                NonCoreAssetList obj = null;

                //читаем инв номер и ИД ЕУП               
                var type = ImportHelper.GetValueByName(uofw, typeof(string), row, "NonCoreAssetListKind", colsNameMapping);
                var IDEUP = ImportHelper.GetValueByName(uofw, typeof(string), row, "Society", colsNameMapping);
                var year = ImportHelper.GetValueByName(uofw, typeof(int), row, "Year", colsNameMapping);
                //ищем в Системе ОБУ
                if (IDEUP != null && type != null && year != null
                    && !String.IsNullOrEmpty(type.ToString())
                    && !String.IsNullOrEmpty(IDEUP.ToString())
                    && int.TryParse(year.ToString(), out yearInt)
                    && yearInt != 0)
                {
                    string ideup = ImportHelper.GetIDEUP(IDEUP);
                    //TODO: почистить
                    List<NonCoreAssetList> list = FindObjects(uofw, type.ToString(), ideup, yearInt);
                    if (list == null || list.Count == 0)
                        obj = new NonCoreAssetList();
                    else if (list.Count > 1)
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }

                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(NonCoreAssetList),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping) ;
                        obj.ImportDate = DateTime.Now;
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            //При импорте, проставляем статус = Проверка ДС
                            NonCoreAssetListState status = uofw.GetRepository<NonCoreAssetListState>()
                                .Filter(f => !f.Hidden && f.Code == "104")
                                .FirstOrDefault();

                            obj.NonCoreAssetListState = status;
                            obj.NonCoreAssetListStateID = status.ID;

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

        public override NonCoreAssetList CreateDefault(IUnitOfWork unitOfWork)
        {

            var obj = base.CreateDefault(unitOfWork);
            if (obj.NonCoreAssetListState == null && obj.ID == 0)
            {
                NonCoreAssetListState defaultStatus = unitOfWork.GetRepository<NonCoreAssetListState>()
                    .Filter(f => !f.Hidden && f.Code == "101").FirstOrDefault();
                if (defaultStatus != null)
                {
                    obj.NonCoreAssetListState = defaultStatus;
                    obj.NonCoreAssetListStateID = defaultStatus.ID;
                }
            }
            if (obj.Society == null)
            {
                                
                var uid = AppContext.SecurityUser.ID;
                if (uid != 0)
                {
                    var currentSibUser = unitOfWork.GetRepository<SibUser>().Find(sibUser => sibUser.UserID == uid);
                    if (currentSibUser != null)
                    {
                        var societyID = currentSibUser.SocietyID;
                        var society = unitOfWork.GetRepository<Society>().Find(soc => soc.ID == societyID);

                        obj.Society = society;
                        obj.SocietyID = societyID;
                    }
                }
                
            }
            return obj;
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
            PropertyInfo propertyInfo = typeof(NonCoreAssetList).GetProperty(notification.PropertyName);

            var ncaLists = this.GetAll(unitOfWork)
                .Where(NotificationHelper.PropertyEquals<NonCoreAssetList, DateTime>(propertyInfo, dtNow.Date))
                .Where(w => !w.Hidden && w.SocietyID != null).ToList();

            List<SibNotificationObject> notificationObjects = new List<SibNotificationObject>();

            try
            {
                if (ncaLists.Count > 0)
                {
                    foreach (NonCoreAssetList ncaList in ncaLists)
                    {
                        List<int> responsiblesIds = new List<int>();
                        DateTime dtEnd = new DateTime(((DateTime)propertyInfo.GetValue(ncaList, null)).Year
                            , ((DateTime)propertyInfo.GetValue(ncaList, null)).Month
                            , ((DateTime)propertyInfo.GetValue(ncaList, null)).Day
                            , ((DateTime)propertyInfo.GetValue(ncaList, null)).Hour
                            , 0
                            , 0);

                        DateTime? remindDate = NotificationHelper.CalculateRemindDateTime(notification.RemindPeriod, ((DateTime)propertyInfo.GetValue(ncaList, null)));

                        if (remindDate == null || remindDate.Value.Date != dtNow.Date)
                            continue;

                        if (notification.SendToAllSocieties)
                            responsiblesIds = unitOfWork.GetRepository<SibUser>().All()
                                .Where(w => w.User != null && w.Society != null)
                                .Where(w => w.SocietyID == ncaList.SocietyID)
                                .Select(s => s.User.ID).ToList<int>();

                        if (notification.Reciever != null && notification.Reciever.UserID != null)
                            responsiblesIds.Add((int)notification.Reciever.UserID);


                        SibNotificationObject nObject = new SibNotificationObject()
                        {
                            Subject = notification.Subject,
                            Message = notification.Message,
                            LinkBaseObject = NotificationHelper.GetLinkedObj(ncaList),
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

        /// <summary>
        /// Заполнение остаточной стоимости при смене статуса.
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeNCAAndListResidualCost(NonCoreAssetList obj)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                var ncaAndListRepo = unitOfWork.GetRepository<NonCoreAssetAndList>();
                List<NonCoreAssetAndList> ncaAndLists = ncaAndListRepo.Filter(f => f.ObjRigthId == obj.ID && !f.Hidden).ToList();

                if (obj.NonCoreAssetListState == null)
                    return;

                string statusCode = unitOfWork.GetRepository<NonCoreAssetListState>().Find(f => f.ID == obj.NonCoreAssetListState.ID).Code;

                if (statusCode != "103")
                    return;
                
                foreach (var item in ncaAndLists)
                {
                    int? estateId = unitOfWork.GetRepository<NonCoreAsset>().Find(f => f.ID == item.ObjLeftId).EstateObjectID;

                    if (estateId == null)
                        continue;

                    var ao = unitOfWork.GetRepository<Entities.Accounting.AccountingObject>()
                        .Filter(f => f.EstateID == estateId && f.OwnerID == item.ObjLeft.AssetOwnerID && !f.Hidden)
                        .FirstOrDefault();
                    if (ao != null)
                    {
                        item.ResidualCostAgreement = ao.ResidualCost;
                        item.ResidualCostDateAgreement = ao.UpdateDate;

                        ncaAndListRepo.Update(item);
                    }
                    
                }

                unitOfWork.SaveChanges();
            }
        }


        /// <summary>
        /// Создание дубля перечня ННА, его строк и объектов ННА для заданного нового ОГ.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="currentID">Текущий ИД Перечня.</param>
        /// <param name="ogID">ИД ОГ.</param>
        public void ChangeOG(IUnitOfWork uow, int currentID, int? ogID)
        {
            if (ogID == null) return;            
            CloneAndChangeOG(uow, currentID, ogID);            
            uow.SaveChanges();
        }


        private void CloneAndChangeOG(IUnitOfWork uow, int currentID, int? ogID)
        {           
            var old = uow.GetRepository<NonCoreAssetList>().Find(currentID);
            if (old == null) return;
                       

            NonCoreAssetList nnaList = uow.GetRepository<NonCoreAssetList>().Create(new NonCoreAssetList());
            nnaList.CopyFrom(old);          
            nnaList.SocietyID = ogID;

            old.NonCoreAssetListState = uow.GetRepository<NonCoreAssetListState>()
               .Filter(f => !f.Hidden && f.Code == "Arhive")
               .FirstOrDefault();

            var nnaListItems = uow.GetRepository<NonCoreAssetAndList>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.ObjRigthId == currentID)
                .Include(inc => inc.ObjLeft);

            foreach (var item in nnaListItems)
            {
                var row = uow.GetRepository<NonCoreAssetAndList>().Create(new NonCoreAssetAndList());
                row.InitialCost = item.InitialCost;
                row.IsNCAPreviousPeriod = item.IsNCAPreviousPeriod;
                row.NonCoreAssetInventoryID = item.NonCoreAssetInventoryID;
                row.NonCoreAssetListItemStateID = item.NonCoreAssetListItemStateID;
                row.ResidualCost = item.ResidualCost;
                row.ResidualCostAgreement = item.ResidualCostAgreement;
                row.ResidualCostDate = item.ResidualCostDate;
                row.ResidualCostDateAgreement = item.ResidualCostDateAgreement;
                row.ResidualCostDateMatching = item.ResidualCostDateMatching;
                row.ResidualCostDateStatement = item.ResidualCostDateStatement;
                row.ResidualCostMatching = item.ResidualCostMatching;
                row.ResidualCostStatement = item.ResidualCostStatement;

                var nna = uow.GetRepository<NonCoreAsset>().Find(item.ObjLeftId);
                var newNNA = uow.GetRepository<NonCoreAsset>().Create(new NonCoreAsset());
                newNNA.CopyFrom(nna);
                newNNA.AssetOwnerID = ogID;
                newNNA.AssetMainOwnerID = (nna.AssetMainOwnerID == nna.AssetOwnerID) ? ogID : nna.AssetMainOwnerID;
                row.ObjLeft = newNNA;
                row.ObjRigth = nnaList;

                nna.NonCoreAssetStatus = uow.GetRepository<NonCoreAssetStatus>()
                    .Filter(f => !f.Hidden && f.Code == "Arhive").FirstOrDefault();
            }

        }

    }
}
