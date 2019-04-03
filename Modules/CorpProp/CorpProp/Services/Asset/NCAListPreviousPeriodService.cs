using Base.DAL;
using Base.Service;
using CorpProp.Entities.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Extentions;
using CorpProp.Services.Base;
using Base.Service.Log;

namespace CorpProp.Services.Asset
{   

    public interface INCAListPreviousPeriodService : ITypeObjectService<NonCoreAssetList>
    {
    }

    public class NCAListPreviousPeriodService : TypeObjectService<NonCoreAssetList>, INCAListPreviousPeriodService
    {
        private readonly ILogService _logger;

        public NCAListPreviousPeriodService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        public override IQueryable<NonCoreAssetList> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            //TODO: Разрешения по текущему пользоватлею

            var currEUP = AppContext.SecurityUser.GetUserIDEUP(unitOfWork);
            var planDate = DateTime.ParseExact(("01.01." + DateTime.Now.Year.ToString()), "dd.MM.yyyy", null);           

            var sales = unitOfWork.GetRepository<NonCoreAssetSale>()
                       .Filter(ff => !ff.Hidden).DefaultIfEmpty();

            var q = unitOfWork.GetRepository<NonCoreAssetAndList>()
            .Filter(f => !f.Hidden && f.ObjLeft != null
            && !f.ObjLeft.Hidden
            && !f.ObjLeft.IsHistory
            && f.ObjLeft.ForecastPeriod > planDate
            && f.ObjLeft.AssetOwner != null
            && f.ObjLeft.AssetOwner.IDEUP == currEUP
            && f.ObjRigth != null
            && !f.ObjRigth.Hidden
            && !f.ObjRigth.IsHistory)
            .Where(w => !sales.Select(sale => sale.NonCoreAssetID).Contains(w.ObjLeftId)).DefaultIfEmpty()
            .Select(s => s.ObjRigth)
            .Distinct();

            return q;
        }

    }
}
