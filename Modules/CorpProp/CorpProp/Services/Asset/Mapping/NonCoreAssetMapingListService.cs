using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Asset.NonEntity;
using CorpProp.Entities.Settings;
using System.Linq.Dynamic;
using Base.Utils.Common;
using Kendo.Mvc.Extensions;

namespace CorpProp.Services.Asset.Mapping
{
    //

    ///// <summary>
    /////     Предоставляет данные и методы сервиса проекции объекта ННА
    ///// </summary>
    //public interface INonCoreAssetAndListMappingService: IBaseObjectService<NonCoreAssetAndListMapping>, ISibNotification
    //{
    //}

    ///// <summary>
    /////     Представляет сервис для работы с проекцией объекта ННА.
    ///// </summary>
    //public class NonCoreAssetAndListMappingService: NonCoreAssetListItemService, INonCoreAssetAndListMappingService
    //{
    //    public NonCoreAssetAndListMappingService(IBaseObjectServiceFacade facade) : base(facade)
    //    {
    //    }

    //    public List<SibNotificationObject> PrepareLinkedObject(IUnitOfWork unitOfWork, SibNotification notification)
    //    {
    //        throw new NotImplementedException();
    //    }
        
    //    public NonCoreAssetAndListMapping Get(IUnitOfWork unitOfWork, int id)
    //    {
    //        return GetAll(unitOfWork).First(mapping => mapping.ID == id);
    //    }

    //    public IReadOnlyCollection<NonCoreAssetAndListMapping> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<NonCoreAssetAndListMapping> collection)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public NonCoreAssetAndListMapping Update(IUnitOfWork unitOfWork, NonCoreAssetAndListMapping obj)
    //    {
    //        var noncoreassetandlist = new NonCoreAssetAndList();
    //        MappingHelper.MapCopy(obj, noncoreassetandlist);
    //        var created = base.Update(unitOfWork, noncoreassetandlist);
    //        MappingHelper.MapCopy(created, obj);
    //        return obj;
    //    }

    //    public IReadOnlyCollection<NonCoreAssetAndListMapping> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<NonCoreAssetAndListMapping> collection)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Delete(IUnitOfWork unitOfWork, NonCoreAssetAndListMapping obj)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<NonCoreAssetAndListMapping> collection)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void ChangeSortOrder(IUnitOfWork unitOfWork, NonCoreAssetAndListMapping obj, int posId)
    //    {
    //        throw new NotImplementedException();
    //    }

        //  public IQueryable<NonCoreAssetAndListMapping> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        //  {
        //      IQueryable<NonCoreAsset> NonCoreAssets;
        //      IQueryable<NonCoreAssetAndList> NonCoreAssetAndLists;
        //      IQueryable<Entities.Estate.Estate> Estates;
        //      IQueryable<AccountingObject> AccountingObjects;

        //      //ОИ
        //      Estates = unitOfWork.GetRepository<Entities.Estate.Estate>().All();
        //          //ОБУ
        //      AccountingObjects = unitOfWork.GetRepository<AccountingObject>().All();
        //          //ННА
        //      NonCoreAssets = unitOfWork.GetRepository<NonCoreAsset>().All();
        //          //Строки ННА
        //      NonCoreAssetAndLists = unitOfWork.GetRepository<NonCoreAssetAndList>().All();

        //      var accountQuery = AccountingObjects
        //          .Join(
        //              Estates,
        //              a => a.EstateID,
        //              e => (Int32?)(e.ID),
        //              (a, e) =>
        //                  new
        //                  {
        //                      EstateID = e.ID,
        //                      OwnerID = a.OwnerID,
        //                      InitialCost = a.InitialCost,
        //                      ResidualCost = a.ResidualCost,
        //                      UpdateDate = a.UpdateDate
        //                  }
        //          ).GroupBy(x => new { x.EstateID, x.OwnerID }, (key, group) => group.FirstOrDefault());

        //      var nonCoreAssetAndListAndAccountingObjects =
        //      NonCoreAssetAndLists
        //          .Join(
        //              NonCoreAssets,
        //              nonCoreAssetAndList => nonCoreAssetAndList.ObjLeftId,
        //              noncoreasset => noncoreasset.ID,
        //              (nonCoreAssetAndList, noncoreasset) =>
        //                  new
        //                  {
        //                      nonCoreAssetAndList = nonCoreAssetAndList,
        //                      noncoreasset = noncoreasset
        //                  }
        //          )
        //          .GroupJoin(accountQuery, n => new { OwnerID = n.noncoreasset.AssetOwnerID, EstateID = n.noncoreasset.EstateObjectID.Value }, a => new { a.OwnerID, a.EstateID }, (n, a) => new { n.nonCoreAssetAndList, Accounts = a.FirstOrDefault() })
        //          .Select(n => new NonCoreAssetAndListMapping
        //          {
        //              NonCoreAssetListItemStateID = n.nonCoreAssetAndList.NonCoreAssetListItemStateID,
        //              NonCoreAssetListItemState = n.nonCoreAssetAndList.NonCoreAssetListItemState,
        //              NonCoreAssetSaleOfferID = n.nonCoreAssetAndList.NonCoreAssetSaleOfferID,
        //              Offer = n.nonCoreAssetAndList.Offer,
        //              ObjLeftId = n.nonCoreAssetAndList.ObjLeftId,
        //              ObjLeft = n.nonCoreAssetAndList.ObjLeft,
        //              ObjRigthId = n.nonCoreAssetAndList.ObjRigthId,
        //              ObjRigth = n.nonCoreAssetAndList.ObjRigth,
        //              NonCoreAssetInventoryID = n.nonCoreAssetAndList.NonCoreAssetInventoryID,
        //              NonCoreAssetInventory = n.nonCoreAssetAndList.NonCoreAssetInventory,
        //              ID = n.nonCoreAssetAndList.ID,
        //              Hidden = n.nonCoreAssetAndList.Hidden,
        //              CreateDate = n.nonCoreAssetAndList.CreateDate,
        //              ImportDate = n.nonCoreAssetAndList.ImportDate,
        //              ImportUpdateDate = n.nonCoreAssetAndList.ImportUpdateDate,
        //              RowVersion = n.nonCoreAssetAndList.RowVersion,
        //              SortOrder = n.nonCoreAssetAndList.SortOrder,
        //              InitialCost = n.Accounts.InitialCost ?? n.nonCoreAssetAndList.InitialCost,
        //              Oid = n.nonCoreAssetAndList.Oid,
        //              IsHistory = n.nonCoreAssetAndList.IsHistory,
        //              ActualDate = n.nonCoreAssetAndList.ActualDate,
        //              NonActualDate = n.nonCoreAssetAndList.NonActualDate,
        //              ResidualCost = n.nonCoreAssetAndList.ResidualCost ?? n.Accounts.ResidualCost,
        //              ResidualCostDate = n.nonCoreAssetAndList.ResidualCostDate ?? n.Accounts.UpdateDate,
        //              ResidualCostAgreement = n.nonCoreAssetAndList.ResidualCostAgreement,
        //              ResidualCostDateAgreement = n.nonCoreAssetAndList.ResidualCostDateAgreement,
        //              ResidualCostStatement = n.nonCoreAssetAndList.ResidualCostStatement,
        //              ResidualCostDateStatement = n.nonCoreAssetAndList.ResidualCostDateStatement
        //          });

        //      nonCoreAssetAndListAndAccountingObjects = nonCoreAssetAndListAndAccountingObjects.Where(mapping => hidden == null || mapping.Hidden == hidden);

        //      return nonCoreAssetAndListAndAccountingObjects;
        //  }



    //    public NonCoreAssetAndListMapping Create(IUnitOfWork unitOfWork, NonCoreAssetAndListMapping obj)
    //    {
    //        var noncoreassetandlist = new NonCoreAssetAndList();
    //        MappingHelper.MapCopy(obj, noncoreassetandlist);
    //        var created = base.Create(unitOfWork, noncoreassetandlist);
    //        MappingHelper.MapCopy(created, obj);
    //        return obj;
    //    }

    //    public NonCoreAssetAndListMapping CreateDefault(IUnitOfWork unitOfWork)
    //    {
    //        var nonCoreAssetAndList = base.CreateDefault(unitOfWork);
    //        var createdDefault = new NonCoreAssetAndListMapping();
    //        MappingHelper.MapCopy(nonCoreAssetAndList, createdDefault);
    //        return createdDefault;
    //    }

    //    public Type EntityType => typeof(NonCoreAssetAndListMapping);

    //    #region TODO ?

    //    ///// <summary>
    //    /////     Переопределяет метод при событии создания объекта.
    //    ///// </summary>
    //    ///// <param name="unitOfWork">Сессия.</param>
    //    ///// <param name="obj">Создаваемый объект.</param>
    //    ///// <returns>Объект ННА.</returns>
    //    //public override NonCoreAssetListMapping Create(IUnitOfWork unitOfWork, NonCoreAssetListMapping obj)
    //    //{
    //    //    return base.Create(unitOfWork, obj);
    //    //}

    //    //public override void Delete(IUnitOfWork unitOfWork, NonCoreAssetListMapping obj)
    //    //{

    //    //    base.Delete(unitOfWork, obj);
    //    //}

    //    //public override NonCoreAssetListMapping Update(IUnitOfWork unitOfWork, NonCoreAssetListMapping obj)
    //    //{
    //    //    //var existingEntity = Get(unitOfWork, obj.ID);

    //    //    // Медленно
    //    //    //if (obj.ApprovalDeadline.Date < DateTime.Now.Date &&
    //    //    //    !existingEntity.NonCoreAssetListMappingItems.OrderBy(t => t.ID).SequenceEqual(
    //    //    //        obj.NonCoreAssetListMappingItems.OrderBy(t => t.ID), new NonCoreAssetListMappingItemComparer()))
    //    //    //    throw new AccessDeniedException("Невозможно изменить ННА в перечне после срока утверждения.");

    //    //    //if (obj.AvailabilityDeadline.Date < DateTime.Now.Date &&
    //    //    //    !existingEntity.NonCoreAssetListMappingItems.OrderBy(t => t.ID).Select(p => p.ID).SequenceEqual(
    //    //    //        obj.NonCoreAssetListMappingItems.OrderBy(t => t.ID).Select(p => p.ID)))
    //    //    //    throw new AccessDeniedException("Невозможно изменить состав перечня ННА после срока предоставления.");

    //    //    return base.Update(unitOfWork, obj);
    //    //}

    //    ///// <summary>
    //    /////     Переопределяет метод при событии сохранения объекта.
    //    ///// </summary>
    //    ///// <param name="unitOfWork">Сессия.</param>
    //    ///// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
    //    ///// <returns></returns>
    //    //protected override IObjectSaver<NonCoreAssetListMapping> GetForSave(IUnitOfWork unitOfWork,
    //    //    IObjectSaver<NonCoreAssetListMapping> objectSaver)
    //    //{
    //    //    var element = objectSaver.Src;
    //    //    using (IUnitOfWork uofw = UnitOfWorkFactory.Create())
    //    //    {
    //    //        var itemRepo = uofw.GetRepository<NonCoreAssetAndList>();
    //    //        var inventory = element.NonCoreAssetInventory == null ? null : uofw.GetRepository<NonCoreAssetInventory>().Find(f => f.ID == element.NonCoreAssetInventory.ID);
    //    //        var items = itemRepo.Filter(f => f.ObjRigthId == element.ID).ToList<NonCoreAssetAndList>();

    //    //        if (items.Count > 0)
    //    //        {
    //    //            foreach (var item in items)
    //    //            {
    //    //                item.NonCoreAssetInventory = inventory;
    //    //                item.NonCoreAssetInventoryID = inventory?.ID;
    //    //                itemRepo.Update(item);
    //    //                uofw.SaveChanges();
    //    //            }
    //    //        }
    //    //    }


    //    //    return
    //    //        base.GetForSave(unitOfWork, objectSaver)
    //    //            .SaveOneObject(x => x.NonCoreAssetInventory)
    //    //            .SaveOneObject(x => x.Society)
    //    //            .SaveOneObject(x => x.NonCoreAssetListMappingType)
    //    //            .SaveOneObject(x => x.NonCoreAssetListMappingKind)
    //    //            .SaveOneObject(x => x.FileCard)
    //    //            .SaveOneObject(x => x.NonCoreAssetListMappingState)
    //    //                            ;
    //    //}

    //    ///// <summary>
    //    ///// Импорт ГГР из файла Excel.
    //    ///// </summary>
    //    ///// <param name="uofw"></param>
    //    ///// <param name="reader"></param>
    //    ///// <param name="error"></param>
    //    ///// <param name="count"></param>
    //    //public void Import(IUnitOfWork uofw, IExcelDataReader reader, Type type, ref int count, ref ImportHistory history)
    //    //{
    //    //    try
    //    //    {

    //    //        DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = x => new ExcelDataTableConfiguration() { UseHeaderRow = false } });
    //    //        var tables = result.Tables;
    //    //        DataTable entryTable = tables[0];


    //    //        if (type != null && Type.Equals(type, typeof(NonCoreAssetListMapping)))
    //    //        {
    //    //            Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(entryTable);

    //    //            //пропускаем первые 9 строк файла не считая строки названия колонок.

    //    //            for (int i = 9; i < entryTable.Rows.Count; i++)
    //    //            {
    //    //                var rowError = "";
    //    //                var row = entryTable.Rows[i];

    //    //                NonCoreAssetListMapping item = ImportObject(uofw, row, colsNameMapping, ref rowError, ref count, ref history);

    //    //                if (item != null)
    //    //                {
    //    //                    _ncaService.Import(uofw, reader, item, ref count, ref history);
    //    //                }
    //    //                else
    //    //                {
    //    //                    history.ImportErrorLogs.AddError(i, null, null, rowError, ErrorType.System);
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        history.ImportErrorLogs.AddError(ex);
    //    //    }
    //    //}

    //    ///// <summary>
    //    ///// 
    //    ///// </summary>
    //    ///// <param name="uofw"></param>
    //    ///// <param name="numb"></param>
    //    ///// <param name="idEup"></param>
    //    ///// <returns></returns>
    //    //public List<NonCoreAssetListMapping> FindObjects(IUnitOfWork uofw, string type, string idEup, int year)
    //    //{
    //    //    //return null;
    //    //    List<NonCoreAssetListMapping> list = new List<NonCoreAssetListMapping>();
    //    //    list = uofw.GetRepository<NonCoreAssetListMapping>().Filter(x =>
    //    //    (x.Society != null && x.Society.IDEUP == idEup)
    //    //    && (x.NonCoreAssetListMappingType != null && x.NonCoreAssetListMappingType.Name == type)
    //    //    && (x.Year != null && x.Year == year)
    //    //    && !x.Hidden).ToList<NonCoreAssetListMapping>();
    //    //    return list;
    //    //}

    //    ///// <summary>
    //    ///// Импортирует из строки файла.
    //    ///// </summary>
    //    ///// <param name="uofw">Сессия.</param>
    //    ///// <param name="row">Строка файла.</param>      
    //    ///// <param name="error">Текст ошибки.</param>
    //    ///// <param name="count">Количество импортированных объектов.</param>
    //    //public NonCoreAssetListMapping ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping, ref string error, ref int count, ref ImportHistory history)
    //    //{
    //    //    try
    //    //    {
    //    //        bool isNew = true;
    //    //        int yearInt = 0;
    //    //        NonCoreAssetListMapping obj = null;

    //    //        //читаем инв номер и ИД ЕУП               
    //    //        var type = ImportHelper.GetValueByName(uofw, typeof(string), row, "NonCoreAssetListMappingKind", colsNameMapping);
    //    //        var IDEUP = ImportHelper.GetValueByName(uofw, typeof(string), row, "Society", colsNameMapping);
    //    //        var year = ImportHelper.GetValueByName(uofw, typeof(int), row, "Year", colsNameMapping);
    //    //        //ищем в Системе ОБУ
    //    //        if (IDEUP != null && type != null && year != null
    //    //            && !String.IsNullOrEmpty(type.ToString())
    //    //            && !String.IsNullOrEmpty(IDEUP.ToString())
    //    //            && int.TryParse(year.ToString(), out yearInt)
    //    //            && yearInt != 0)
    //    //        {
    //    //            string ideup = ImportHelper.GetIDEUP(IDEUP);
    //    //            //TODO: почистить
    //    //            List<NonCoreAssetListMapping> list = FindObjects(uofw, type.ToString(), ideup, yearInt);
    //    //            if (list == null || list.Count == 0)
    //    //                obj = new NonCoreAssetListMapping();
    //    //            else if (list.Count > 1)
    //    //                error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
    //    //            else if (list.Count == 1)
    //    //            {
    //    //                isNew = false;
    //    //                obj = list[0];
    //    //            }

    //    //            if (obj != null)
    //    //            {
    //    //                obj = ImportHelper.FillObject(uofw, typeof(NonCoreAssetListMapping),
    //    //                   obj, row, row.Table.Columns, ref error, ref history, colsNameMapping) as NonCoreAssetListMapping;
    //    //                obj.ImportDate = DateTime.Now;
    //    //            }

    //    //            if (!String.IsNullOrEmpty(error))
    //    //                obj = null;
    //    //            else
    //    //            {
    //    //                if (obj != null)
    //    //                {
    //    //                    //При импорте, проставляем статус = Проверка ДС
    //    //                    NonCoreAssetListMappingState status = uofw.GetRepository<NonCoreAssetListMappingState>().Find(f => f.Code == "104");

    //    //                    obj.NonCoreAssetListMappingState = status;
    //    //                    obj.NonCoreAssetListMappingStateID = status.ID;

    //    //                    count++;
    //    //                    if (isNew)
    //    //                        return this.Create(uofw, obj);
    //    //                    else
    //    //                        return this.Update(uofw, obj);
    //    //                }
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            error += $"Неверное значение года, ИД ЕУП или типа. {System.Environment.NewLine}";
    //    //            obj = null;
    //    //        }
    //    //        return null;
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        history.ImportErrorLogs.AddError(ex);
    //    //        return null;
    //    //    }
    //    //}

    //    //public override NonCoreAssetListMapping CreateDefault(IUnitOfWork unitOfWork)
    //    //{

    //    //    var obj = base.CreateDefault(unitOfWork);
    //    //    if (obj.NonCoreAssetListMappingState == null && obj.ID == 0)
    //    //    {
    //    //        NonCoreAssetListMappingState defaultStatus = unitOfWork.GetRepository<NonCoreAssetListMappingState>().Find(f => f.Code == "101");
    //    //        if (defaultStatus != null)
    //    //        {
    //    //            obj.NonCoreAssetListMappingState = defaultStatus;
    //    //            obj.NonCoreAssetListMappingStateID = defaultStatus.ID;
    //    //        }
    //    //    }
    //    //    if (obj.Society == null)
    //    //    {

    //    //        var uid = AppContext.SecurityUser.ID;
    //    //        if (uid != 0)
    //    //        {
    //    //            var currentSibUser = unitOfWork.GetRepository<SibUser>().Find(sibUser => sibUser.UserID == uid);
    //    //            if (currentSibUser != null)
    //    //            {
    //    //                var societyID = currentSibUser.SocietyID;
    //    //                var society = unitOfWork.GetRepository<Society>().Find(soc => soc.ID == societyID);

    //    //                obj.Society = society;
    //    //                obj.SocietyID = societyID;
    //    //            }
    //    //        }

    //    //    }
    //    //    return obj;
    //    //}

    //    //public List<SibNotificationObject> PrepareLinkedObject(IUnitOfWork unitOfWork, SibNotification notification)
    //    //{
    //    //    DateTime now = DateTime.Now;
    //    //    DateTime dtNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
    //    //    PropertyInfo propertyInfo = typeof(NonCoreAssetListMapping).GetProperty(notification.PropertyName);

    //    //    var ncaLists = this.GetAll(unitOfWork)
    //    //        .Where(NotificationHelper.PropertyEquals<NonCoreAssetListMapping, DateTime>(propertyInfo, dtNow.Date))
    //    //        .Where(w => !w.Hidden && w.SocietyID != null).ToList();

    //    //    List<SibNotificationObject> notificationObjects = new List<SibNotificationObject>();

    //    //    try
    //    //    {
    //    //        if (ncaLists.Count > 0)
    //    //        {
    //    //            foreach (NonCoreAssetListMapping ncaList in ncaLists)
    //    //            {
    //    //                List<int> responsiblesIds = new List<int>();
    //    //                DateTime dtEnd = new DateTime(((DateTime)propertyInfo.GetValue(ncaList, null)).Year
    //    //                    , ((DateTime)propertyInfo.GetValue(ncaList, null)).Month
    //    //                    , ((DateTime)propertyInfo.GetValue(ncaList, null)).Day
    //    //                    , ((DateTime)propertyInfo.GetValue(ncaList, null)).Hour
    //    //                    , 0
    //    //                    , 0);

    //    //                DateTime? remindDate = NotificationHelper.CalculateRemindDateTime(notification.RemindPeriod, ((DateTime)propertyInfo.GetValue(ncaList, null)));

    //    //                if (remindDate != dtNow)
    //    //                    continue;

    //    //                if (notification.SendToAllSocieties)
    //    //                    responsiblesIds = unitOfWork.GetRepository<SibUser>().All()
    //    //                        .Where(w => w.User != null && w.Society != null)
    //    //                        .Where(w => w.SocietyID == ncaList.SocietyID)
    //    //                        .Select(s => s.User.ID).ToList<int>();

    //    //                responsiblesIds.Add((int)notification.Reciever.UserID);

    //    //                SibNotificationObject nObject = new SibNotificationObject()
    //    //                {
    //    //                    Subject = notification.Subject,
    //    //                    Message = notification.Message,
    //    //                    LinkBaseObject = NotificationHelper.GetLinkedObj(ncaList),
    //    //                    Recipients = responsiblesIds
    //    //                };

    //    //                notificationObjects.Add(nObject);
    //    //            }
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        //return ex.ToStringWithInner();
    //    //    }

    //    //    return notificationObjects;
    //    //}

    //    #endregion

    //}
}
