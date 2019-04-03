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
    public interface INonCoreAssetSaleAcceptService : ITypeObjectService<NonCoreAssetSaleAccept>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - объект ННА.
    /// </summary>
    public class NonCoreAssetSaleAcceptService : TypeObjectService<NonCoreAssetSaleAccept>, INonCoreAssetSaleAcceptService
    {
        private readonly ILogService _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetSaleAcceptService.
        /// </summary>
        /// <param name="facade"></param>
        public NonCoreAssetSaleAcceptService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект ННА.</returns>
        public override NonCoreAssetSaleAccept Create(IUnitOfWork unitOfWork, NonCoreAssetSaleAccept obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<NonCoreAssetSaleAccept> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<NonCoreAssetSaleAccept> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.AcceptType)
                    .SaveOneObject(x => x.FileCard)
                    //.SaveManyToMany(x => x.NonCoreAssetListItems)
                    ;
        }
    }
}
