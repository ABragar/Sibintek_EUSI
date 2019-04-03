using Base;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Estate;
using CorpProp.Helpers;
using CorpProp.Services.Accounting;
using CorpProp.Services.Asset;
using System;

namespace CorpProp.Services.Estate
{
    /// <summary>
    /// Предоставляет данные и методы сервиса записи об инвентарном объекте.
    /// </summary>
    public interface IInventoryObjectService : Base.ITypeObjectService<InventoryObject>
    {
        void CheckEstatesInIK(IUnitOfWork uofw, BaseObject obj);
    }

    /// <summary>
    /// Представляет сервис записи инвентарного объекта.
    /// </summary>
    public class InventoryObjectService : BaseEstateService<InventoryObject>, IInventoryObjectService
    {
        private readonly ILogService _logger;
        //private readonly IAccountingObjectService<AccountingObject> _osService;

        /// <summary>
        /// Инициализирует новый экземпляр класса InventoryObjectService.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="nonCoreAssetService"></param>
        /// <param name="nonCoreAssetAndList"></param>
        public InventoryObjectService(IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) :base(facade, nonCoreAssetService, osService, logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Инвентарный объект.</returns>
        public override InventoryObject Create(IUnitOfWork unitOfWork, InventoryObject obj)
        {
           var result = base.Create(unitOfWork, obj);
           return result;
        }

        public override Entities.Estate.InventoryObject Update(IUnitOfWork unitOfWork, Entities.Estate.InventoryObject obj)
        {
            var result = base.Update(unitOfWork, obj);
            return result;
        }        

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<InventoryObject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<InventoryObject> objectSaver)
        {

            var est =
                base.GetForSave(unitOfWork, objectSaver);
            return est;
        }

        public void CheckEstatesInIK(IUnitOfWork uofw, BaseObject obj)
        {
            if (obj is InventoryObject)
            {
                EstateHelper.CheckRightsEstate(uofw, obj);
            }

        }
    }
}
