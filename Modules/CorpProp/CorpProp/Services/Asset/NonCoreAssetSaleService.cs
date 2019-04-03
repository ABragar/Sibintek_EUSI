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
    public interface INonCoreAssetSaleService : ITypeObjectService<NonCoreAssetSale>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - объект ННА.
    /// </summary>
    public class NonCoreAssetSaleService : TypeObjectService<NonCoreAssetSale>, INonCoreAssetSaleService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetSaleService.
        /// </summary>
        /// <param name="facade"></param>
        public NonCoreAssetSaleService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект ННА.</returns>
        public override NonCoreAssetSale Create(IUnitOfWork unitOfWork, NonCoreAssetSale obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<NonCoreAssetSale> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<NonCoreAssetSale> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.NonCoreAsset)
                    .SaveOneObject(x => x.SibDeal)
                    .SaveOneObject(x => x.ImplementationWay)
                    .SaveOneObject(x => x.NonCoreAssetSaleStatus)
                    .SaveOneObject(x => x.FileCard)
                    ;
        }
    }
}
