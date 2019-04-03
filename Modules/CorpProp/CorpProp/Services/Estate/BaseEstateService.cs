using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Base;
using Base.Service;
using Base.DAL;
using Base.Extensions;
using CorpProp.Entities.Asset;
using CorpProp.Services.Asset;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Extentions;
using CorpProp.Entities.Accounting;
using CorpProp.Services.Accounting;
using CorpProp.Entities.Estate;
using Base.Service.Log;

namespace CorpProp.Services.Estate
{
    public class BaseEstateService<T>: Base.TypeObjectService<T>
        where T : Entities.Estate.Estate
    {
        private readonly ILogService _logger;
        private readonly INonCoreAssetService _nonCoreAssetService;
        private readonly IAccountingObjectService _osService;

        public BaseEstateService(IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) : base(facade, logger)
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
        public override T Create(IUnitOfWork unitOfWork, T obj)
        {
            var result = base.Create(unitOfWork, obj);
            return result;
        }

        public override T Update(IUnitOfWork unitOfWork, T obj)
        {
            RemoveNna(estateNew: obj);
            if ((obj.EstateStatus == Entities.NSI.EstateStatus.Create || obj.EstateStatus == Entities.NSI.EstateStatus.Undefined)
                && IsChange(unitOfWork, obj))
            {
                obj.EstateStatus = Entities.NSI.EstateStatus.Update;
                if (obj.EnrichmentDate == null)
                    obj.EnrichmentDate = DateTime.Now;
            }
                
            var result = base.Update(unitOfWork, obj);
            return result;
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden).Where(est => !est.IsArchived.HasValue || !est.IsArchived.Value);
        }

        private void RemoveNna(T estateNew)
        {
            bool newIsNonCoreAsset = estateNew.IsNonCoreAsset;
            var estateId = estateNew.ID;

            using (var uow = UnitOfWorkFactory.Create())
            {
                var prevEstate = Get(uow, estateId);
                if (prevEstate == null)
                    return;
                if (!newIsNonCoreAsset && prevEstate.IsNonCoreAsset)
                {
                    var nonCoreAssets = _nonCoreAssetService.GetAll(uow)
                                                            .Where(asset => asset.EstateObjectID == estateId);
                    var nonCoreAssetAndListsRepo = uow.GetRepository<NonCoreAssetAndList>();
                    var nonCoreAssetAndLists = nonCoreAssetAndListsRepo.All();

                    nonCoreAssetAndLists.Where(list => list.ObjLeft != null && list.ObjLeft.EstateObjectID == estateId)
                                        .ForEach(list => nonCoreAssetAndListsRepo.Delete(list));
                    nonCoreAssets.ForEach(asset => asset.Hidden = true);
                    uow.SaveChanges();
                }
            }
        }

        private bool IsChange(IUnitOfWork unitOfWork, T obj)
        {
            var original = unitOfWork.GetRepository<T>().GetOriginal(obj.ID);
            foreach (var prop in obj.GetType().GetProperties())
            {
                var org = original.GetType().GetProperty(prop.Name).GetValue(original);
                if (!Object.Equals(org, prop.GetValue(obj)))
                    return true;
            }
            return false;
        }
        public override IQueryable<T> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {

            return base.GetAllByDate(uow, date).Where(est => !est.IsArchived.HasValue || !est.IsArchived.Value);
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
