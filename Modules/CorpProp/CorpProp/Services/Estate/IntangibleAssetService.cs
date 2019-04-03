using Base;
using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Asset;
using CorpProp.Services.Asset;
using CorpProp.Services.Base;
using CorpProp.Services.Accounting;
using CorpProp.Entities.Accounting;
using Base.Service.Log;

namespace CorpProp.Services.Estate
{
    /// <summary>
    /// Предоставляет данные и методы сервиса для работы с НМА.
    /// </summary>
    public interface IIntangibleAssetService : ITypeObjectService<CorpProp.Entities.Estate.IntangibleAsset>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с НМА.
    /// </summary>
    public class IntangibleAssetService : BaseEstateService<CorpProp.Entities.Estate.IntangibleAsset>, IIntangibleAssetService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса IntangibleAssetService.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="nonCoreAssetService"></param>
        /// <param name="nonCoreAssetAndList"></param>
        /// <param name="securityUserService"></param>
        /// <param name="pathHelper"></param>
        /// <param name="workflowService"></param>
        public IntangibleAssetService(IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) :
            base(facade, nonCoreAssetService, osService, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания ОИ.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>НМА.</returns>
        public override Entities.Estate.IntangibleAsset Create(IUnitOfWork unitOfWork, Entities.Estate.IntangibleAsset obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод сохранения.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="objectSaver"></param>
        /// <returns></returns>
        protected override IObjectSaver<CorpProp.Entities.Estate.IntangibleAsset> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<CorpProp.Entities.Estate.IntangibleAsset> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                                      
                   
                    .SaveOneObject(x => x.IntangibleAssetType)
                    .SaveOneObject(x => x.IntangibleAssetStatus)
                    .SaveOneObject(x => x.Image)
                    .SaveOneObject(x => x.SignType)
                    //---save Estate-------------------------                  
                    //.SaveOneObject(x => x.EstateType)
                    //.SaveOneObject(x => x.ClassFixedAsset)
                    //.SaveOneObject(x => x.Owner)
                    //.SaveOneObject(x => x.BusinessArea)
                    //.SaveOneObject(x => x.WhoUse)
                    //.SaveOneObject(x => x.ReceiptReason)
                    //.SaveOneObject(x => x.LeavingReason)
                    //.SaveOneObject(x => x.OKOF94)
                    //.SaveOneObject(x => x.OKOF2014)
                    //.SaveOneObject(x => x.OKTMO)
                    //.SaveOneObject(x => x.OKTMORegion)
                    //.SaveOneObject(x => x.OKATO)
                    //.SaveOneObject(x => x.OKATORegion)
                    //.SaveOneObject(x => x.Status)


                    // .SaveOneToMany(x => x.Images, x => x.SaveOneObject(z => z.Object))


                    //-----------------------------------
                    ;

        }
    }
}
