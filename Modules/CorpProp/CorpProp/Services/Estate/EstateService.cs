using System;
using System.Linq;
using System.Linq.Dynamic;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using CorpProp.Entities.Estate;
using CorpProp.Helpers;
using CorpProp.Services.Asset;
using Kendo.Mvc.Extensions;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Extentions;
using CorpProp.Entities.Accounting;
using CorpProp.Services.Accounting;
using Base.Service.Log;

namespace CorpProp.Services.Estate
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта имущества.
    /// </summary>
    public interface IEstateService : Base.ITypeObjectService<CorpProp.Entities.Estate.Estate>
    {
        
    }

    /// <summary>
    /// Представляет сервис объекта имущества.
    /// </summary>
    public class EstateService : Base.TypeObjectService<CorpProp.Entities.Estate.Estate>, IEstateService
    {
        private readonly ILogService _logger;
        private readonly INonCoreAssetService _nonCoreAssetService;
        private readonly IAccountingObjectService _osService;

        /// <summary>
        /// Инициализирует новый экземпляр класса EstateService.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="securityUserService"></param>
        /// <param name="pathHelper"></param>
        /// <param name="workflowService"></param>
        public EstateService(
            IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _nonCoreAssetService = nonCoreAssetService;
            _osService = osService;
        }

        /// <summary>
        /// Переопределяет метод при событии создания ОИ.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект имущества.</returns>
        public override Entities.Estate.Estate Create(IUnitOfWork unitOfWork, Entities.Estate.Estate obj)
        {
            var result = base.Create(unitOfWork, obj);
            return result;
        }

        public override Entities.Estate.Estate Update(IUnitOfWork unitOfWork, Entities.Estate.Estate obj)
        {
            RemoveNna(estateNew: obj);
            var result = base.Update(unitOfWork, obj);
            return result;
        }

        private void RemoveNna(Entities.Estate.Estate estateNew)
        {
            bool newIsNonCoreAsset = estateNew.IsNonCoreAsset;
            var estateId = estateNew.ID;

            using (var uow = UnitOfWorkFactory.Create())
            {
                var prevEstate = Get(uow, estateId);
                if (prevEstate == null)
                    return;
                if (!newIsNonCoreAsset || prevEstate.IsNonCoreAsset)
                    return;
                var nonCoreAssets = _nonCoreAssetService.GetAll(uow).Where(asset => asset.EstateObjectID == estateId);
                nonCoreAssets.ForEach(asset => asset.Hidden = true);
            }
            


            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Переопределяет метод сохранения.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="objectSaver"></param>
        /// <returns></returns>
        protected override IObjectSaver<CorpProp.Entities.Estate.Estate> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<CorpProp.Entities.Estate.Estate> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)

                    ////---save Estate-------------------------                  
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


                    //.SaveOneToMany(x => x.Images, x => x.SaveOneObject(z => z.Object))
                   

                    ////-----------------------------------
                    ;
        }

        public override IQueryable<Entities.Estate.Estate> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
           

             return base.GetAllByDate(uow, date);
            /*var estates = base.GetAllByDate(uow, date);
            var os = _osService.GetAllByDate(uow, date);

            var idEUP = AppContext.SecurityUser.GetUserIDEUP(uow);
            var isFromCauk = AppContext.SecurityUser.IsFromCauk(uow);
            var isFromService = AppContext.SecurityUser.IsFromService(uow);
            var agents = AppContext.SecurityUser.GetUserAgents(uow);
            var belows = AppContext.SecurityUser.GetUserBelows(uow);

            var q = estates.Join(os, e => e.Oid, o => o.Estate.Oid, (e, o) => new
            {
                Estate = e,                
                Owner = o.Owner,
                WhoUse = o.WhoUse,
                MainOwner = o.MainOwner
            })
            .Where(f =>
                isFromCauk || isFromService
                || (f.Owner != null && f.Owner.IDEUP == idEUP)
                || (f.MainOwner != null && f.MainOwner.IDEUP == idEUP) 
                || (belows.Contains(f.Owner.IDEUP))
                || ((f.WhoUse != null && f.WhoUse.IDEUP == idEUP) && (agents.Contains(f.Owner.IDEUP)))
                )
            .Select(s => s.Estate);
            //Join(estates, jois => jois.EstateOID, est => est.Oid, (jois, est) => est);
            return q;*/
        }
    }
}
