using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Asset;
using CorpProp.Services.Base;

namespace CorpProp.Services.Asset
{
    public interface INonCoreAssetInventoryService : ITypeObjectService<NonCoreAssetInventory>
    {
    }

    public class NonCoreAssetInventoryService : TypeObjectService<NonCoreAssetInventory>, INonCoreAssetInventoryService
    {
        private readonly ILogService _logger;
        public NonCoreAssetInventoryService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        public override NonCoreAssetInventory Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }

        protected override IObjectSaver<NonCoreAssetInventory> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<NonCoreAssetInventory> objectSaver)
        {
            ChangeNCAAndListResidualCost(objectSaver.Src);

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(p => p.Society)
                .SaveOneObject(p => p.NonCoreAssetInventoryType)
                .SaveOneObject(p => p.NonCoreAssetInventoryState)
                    //.SaveOneToMany(x => x.NonCoreAssetLists, x => x.SaveOneObject(p => p.NonCoreAssetInventory)
                    //    .SaveOneObject(p => p.Society)
                .SaveOneObject(p => p.FileCard)
                    //    .SaveOneObject(p => p.NonCoreAssetListType)
                    //    .SaveOneObject(p => p.NonCoreAssetListKind)
                    ;
        }

        public override NonCoreAssetInventory CreateDefault(IUnitOfWork unitOfWork)
        {

            var obj = base.CreateDefault(unitOfWork);
            if (obj.NonCoreAssetInventoryState == null && obj.ID == 0)
            {
                NonCoreAssetInventoryState defaultStatus = 
                    unitOfWork.GetRepository<NonCoreAssetInventoryState>()
                    .Filter(f => !f.Hidden && !f.IsHistory && f.Code == "101")
                    .FirstOrDefault();
                if (defaultStatus != null)
                {
                    obj.NonCoreAssetInventoryState = defaultStatus;
                    //obj.NonCoreAssetInventoryStateID = defaultStatus.ID;
                }
            }

            return obj;
        }

        private void ChangeNCAAndListResidualCost(NonCoreAssetInventory obj)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                var ncaAndListRepo = unitOfWork.GetRepository<NonCoreAssetAndList>();
                List<NonCoreAssetAndList> ncaAndLists = 
                    ncaAndListRepo.Filter(f => f.NonCoreAssetInventoryID == obj.ID && !f.Hidden).ToList();

                if (obj.NonCoreAssetInventoryState == null)
                    return;

                string statusCode = obj.NonCoreAssetInventoryState.Code;

                //string statusCode = unitOfWork.GetRepository<NonCoreAssetInventoryState>()
                //    .Filter(f => f.ID == obj.NonCoreAssetInventoryState.ID).FirstOrDefault()?.Code;
                //    ///.Find(f => f.ID == obj.NonCoreAssetInventoryState.ID).Code;

                if (statusCode != "102" || statusCode != "101")
                    return;

                foreach (var item in ncaAndLists)
                {
                    int? estateId = unitOfWork.GetRepository<NonCoreAsset>()
                        .Filter(f => f.ID == item.ObjLeftId).FirstOrDefault()?.EstateObjectID;
                        //.Find(f => f.ID == item.ObjLeftId).EstateObjectID;

                    if (estateId == null)
                        continue;

                    var ao = unitOfWork.GetRepository<Entities.Accounting.AccountingObject>().Filter(f => f.EstateID == estateId && f.OwnerID == item.ObjLeft.AssetOwnerID && !f.Hidden).FirstOrDefault();

                    if (ao == null)
                        continue;

                    if (statusCode == "102")
                    {
                        item.ResidualCostStatement = ao.ResidualCost;
                        item.ResidualCostDateStatement = ao.UpdateDate;
                    }
                    else
                    {
                        item.ResidualCostMatching = ao.ResidualCost;
                        item.ResidualCostDateMatching = ao.UpdateDate;
                    }

                    ncaAndListRepo.Update(item);
                }

                unitOfWork.SaveChanges();
            }
        }
    }
}
