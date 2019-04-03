using Base.DAL;
using Base.Service;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Security;
using CorpProp.Exceptions;
using CorpProp.Services.Accounting;
using CorpProp.Services.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.FIAS;
using AppContext = Base.Ambient.AppContext;
using Base.Service.Log;

namespace CorpProp.Services.Estate
{
    public interface IPropertyComplexIOService : Base.ITypeObjectService<PropertyComplexIO>
    {

    }
    public class PropertyComplexIOService : BaseEstateService<PropertyComplexIO>, IPropertyComplexIOService
    {
        private readonly ILogService _logger;
        public PropertyComplexIOService(IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) :
            base(facade, nonCoreAssetService, osService, logger)
        {
            _logger = logger;

        }

        protected override IObjectSaver<PropertyComplexIO> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<PropertyComplexIO> objectSaver)
        {
            objectSaver.Dest.IsPropertyComplex = true;
            objectSaver.Dest.NameEUSI = objectSaver.Src.Name;

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(s => s.SibUser)
                .SaveOneObject(s => s.Country)
                .SaveOneObject(s => s.Parent)
                .SaveOneObject(s => s.PropertyComplexIOType)
                .SaveOneObject(s => s.Land)
                ;
        }

        public override PropertyComplexIO CreateDefault(IUnitOfWork unitOfWork)
        {
            var obj = base.CreateDefault(unitOfWork);

            obj.IsPropertyComplex = true;
            var country = unitOfWork.GetRepository<SibCountry>()
                                    .FilterAsNoTracking(c => !c.Hidden && !c.IsHistory && c.Code == "RU").FirstOrDefault();
            if (country != null)
            {
                obj.Country = country;
            }

            if (obj.SibUser == null)
            {
                var uid = AppContext.SecurityUser.ID;
                if (uid != 0)
                {
                    var currentSibUser = unitOfWork.GetRepository<SibUser>().Find(f => f.UserID == uid);
                    if (currentSibUser != null)
                    {
                        obj.SibUser = currentSibUser;
                        obj.SibUserID = currentSibUser.ID;
                    }
                }
            }

            return obj;
        }

        private void CheckLandCollection(PropertyComplexIO obj)
        {
            //Изменение требования, если ИК наполнен ОИ, то кол-ия ЗУ обязательна к заполнению
            using (var uofw = UnitOfWorkFactory.Create())
            {
                if (uofw.GetRepository<InventoryObject>().FilterAsNoTracking(f => !f.Hidden 
                && !f.IsHistory 
                && f.ParentID == obj.ID 
                && !f.IsPropertyComplex).Any())
                {
                    if (!uofw.GetRepository<IKAndLand>()
                        .FilterAsNoTracking(f => !f.Hidden && f.ObjLeftId == obj.ID).Any())
                    {
                        throw new EmptyLandCollectionException(
                            "Коллекция «Земельные участки» обязательна для заполнения. Необходимо добавить запись", obj.ID);
                    }
                }
                
            }
        }

        public override PropertyComplexIO Create(IUnitOfWork unitOfWork, PropertyComplexIO obj)
        {
            var newObj = base.Create(unitOfWork, obj);            
            return newObj;
        }

        protected override void OnSave(PropertyComplexIO obj)
        {
            base.OnSave(obj);
            if (obj.ID != 0)
            {
                CheckLandCollection(obj);
            }
        }

        /*
        public override IQueryable<PropertyComplexIO> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return GetAllByPermissions(unitOfWork, TypePermission.Read);

        }

        public override IQueryable<PropertyComplexIO> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {

            if (date == null)
                return this.GetAll(uow, false).Where(f => !f.IsHistory);
            else
                date = date.Value.Date;

            var q = this.GetAll(uow, false);
            var groups = q
                .Where(x => (x.ActualDate == null || (x.ActualDate != null && x.ActualDate.Value.Date <= date)))
                .GroupBy(gr => gr.Oid).Select(s => new { Oid = s.Key, MaxActualDate = s.Max(m => m.ActualDate) });

            var items = q.Join(groups, e => e.Oid, o => o.Oid, (e, o) => new { e, o })
                .Where(w => w.o.MaxActualDate == w.e.ActualDate)
                .Select(s => s.e);


            return items;
        }
        */
    }
}
