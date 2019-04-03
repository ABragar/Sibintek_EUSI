using Base.DAL;
using Base.Service;
using CorpProp.Entities.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Extentions;
using CorpProp.Entities.Security;
using CorpProp.Entities.FIAS;

namespace CorpProp.Services.Estate
{
    public interface IPropertyComplexService : IBaseCategoryService<PropertyComplex>
    {

    }

    public class PropertyComplexService : BaseCategoryService<PropertyComplex>, IPropertyComplexService
    {
        public PropertyComplexService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }


        public override PropertyComplex Create(IUnitOfWork unitOfWork, PropertyComplex obj)
        {
            return base.Create(unitOfWork, obj);
        }



        protected override IObjectSaver<PropertyComplex> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<PropertyComplex> objectSaver)
        {
            var obj =
                base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.PropertyComplexKind)
                .SaveOneObject(x => x.Parent_)
                ;

            ////TODO: добавить нормальную проверку на циклические зависимости
            //if (objectSaver.Src != null && objectSaver.Dest != null
            //    && objectSaver.Src.Parent != null
            //    && objectSaver.Dest.ID == objectSaver.Src.Parent.ID )
            //{
            //    throw new Exception("Невозможно сохранить запись! Циклическая зависимость, укажите другой вышестоящий ИК.");                

            //}
            //objectSaver.Dest.SetParent(objectSaver.Src.Parent);
            //objectSaver.Dest.FullName = Helpers.EstateHelper.GetAllParentNames(objectSaver.Src, "");

            return obj;

        }


        public override PropertyComplex CreateDefault(IUnitOfWork unitOfWork)
        {

            var obj = base.CreateDefault(unitOfWork);
            obj.SibUser = AppContext.SecurityUser.GetSibUser(unitOfWork);
            obj.SibUserID = obj.SibUser?.ID;

            var country = unitOfWork.GetRepository<SibCountry>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.Code == "RU")
                .FirstOrDefault();

            obj.SibCountry = country;

            return obj;
        }
    }


    //IReportPCNonCoreAssetService
    public interface IReportPCNonCoreAssetService : IBaseCategoryService<PropertyComplex>
    {

    }
    public class ReportPCNonCoreAssetService : PropertyComplexService, IReportPCNonCoreAssetService
    {
        public ReportPCNonCoreAssetService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<PropertyComplex> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<PropertyComplex> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver);
        }

        public override IQueryable<PropertyComplex> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return unitOfWork.GetRepository<InventoryObject>()
                .Filter(f => !f.Hidden && f.IsNonCoreAsset && f.PropertyComplex != null && !f.PropertyComplex.Hidden)
                .Select(s => s.PropertyComplex).Distinct();

        }
    }
}
