using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
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
    public interface INonCoreAssetSaleOfferService : IBaseObjectService<NonCoreAssetSaleOffer>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - объект ННА.
    /// </summary>
    public class NonCoreAssetSaleOfferService : TypeObjectService<NonCoreAssetSaleOffer>, INonCoreAssetSaleOfferService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetSaleOfferService.
        /// </summary>
        /// <param name="facade"></param>
        public NonCoreAssetSaleOfferService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект ННА.</returns>
        public override NonCoreAssetSaleOffer Create(IUnitOfWork unitOfWork, NonCoreAssetSaleOffer obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<NonCoreAssetSaleOffer> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<NonCoreAssetSaleOffer> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ImplementationWay)
                    //.SaveOneToMany(x => x.NonCoreAssetListItems, x => x.SaveOneObject(ss => ss.NonCoreAsset)
                    //.SaveOneObject(ss => ss.NonCoreAssetList)
                    //.SaveOneObject(ss => ss.Offer)
                    //.SaveOneObject(ss => ss.NonCoreAssetListItemState)
                    //.SaveManyToMany(ss => ss.Accepts)
                    //.SaveManyToMany(ss => ss.NonCoreAssetAppraisals)
                    //)
                    ;
        }
    }
}
