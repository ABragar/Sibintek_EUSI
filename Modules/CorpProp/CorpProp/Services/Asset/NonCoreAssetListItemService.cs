using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Asset
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - объект ННА.
    /// </summary>
    public interface INonCoreAssetListItemService: IBaseObjectService<NonCoreAssetAndList>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - объект ННА.
    /// </summary>
    public class NonCoreAssetListItemService: TypeObjectService<NonCoreAssetAndList>, INonCoreAssetListItemService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetListItemService.
        /// </summary>
        /// <param name="facade"></param>
        public NonCoreAssetListItemService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект ННА.</returns>
        //public override NonCoreAssetAndList Create(IUnitOfWork unitOfWork, NonCoreAssetAndList obj)
        //{
        //    return base.Create(unitOfWork, obj);
        //}

        public override IReadOnlyCollection<NonCoreAssetAndList> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<NonCoreAssetAndList> collection)
        {
            CheckItemsEdit(unitOfWork, collection);
            return base.UpdateCollection(unitOfWork, collection);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<NonCoreAssetAndList> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<NonCoreAssetAndList> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ObjLeft)
                    .SaveOneObject(x => x.ObjRigth)
                    .SaveOneObject(x => x.NonCoreAssetListItemState)
                    .SaveOneObject(x => x.Offer)
                    .SaveOneObject(x => x.NonCoreAssetInventory)

                    //.SaveOneToMany(x => x.Accepts, x => x.SaveOneObject(ss => ss.AcceptType)
                    //    .SaveOneObject(ss => ss.FileCard)
                    //    .SaveManyToMany(ss => ss.NonCoreAssetListItems)                        
                    //)
                    //.SaveManyToMany(x => x.NonCoreAssetAppraisals)
                    ;
        }

        private void CheckItemsEdit(IUnitOfWork uofw, IReadOnlyCollection<NonCoreAssetAndList> collection)
        {
            foreach (var item in collection)
            {
                NonCoreAssetList list = uofw.GetRepository<NonCoreAssetList>().Find(item.ObjRigthId);
                if (list == null)
                    continue;

                NonCoreAssetAndList existingItem = this.Get(uofw, item.ID);
                IQueryable<NonCoreAssetAndList> existingItemsCollection = this.GetAll(uofw).Where(w => w.ObjRigthId == list.ID).OrderBy(o => o.ID);

                if (list.AvailabilityDeadline.Date < DateTime.Now.Date && !existingItemsCollection.Select(s => s.ID).Contains(item.ID))
                    throw new AccessDeniedException("Невозможно изменить состав перечня ННА после срока предоставления.");
            }
        }
    }
}
