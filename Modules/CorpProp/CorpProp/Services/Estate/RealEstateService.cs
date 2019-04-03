using System.Linq;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Helpers;
using CorpProp.Services.Accounting;
using CorpProp.Services.Asset;
using CorpProp.Services.Base;

namespace CorpProp.Services.Estate
{
    /// <summary>
    /// Предоставляет данные и методы сервиса записи об объекте недвижимого имущества.
    /// </summary>
    public interface IRealEstateService : ITypeObjectService<RealEstate>
    {
    }

    /// <summary>
    /// Представляет сервис записи объекта недвижимого имущества.
    /// </summary>
    public class RealEstateService : BaseEstateService<RealEstate>, IRealEstateService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса RealEstateService.
        /// </summary>
        /// <param name="facade"></param>
        public RealEstateService(IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) :
            base(facade, nonCoreAssetService, osService, logger)
        {
            _logger = logger;
          
        }
        public override IQueryable<RealEstate> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Объект недвижимого имущества.</returns>
        public override RealEstate Create(IUnitOfWork unitOfWork, RealEstate obj)
        {
            var result = base.Create(unitOfWork, obj);
            return result;
        }

        public override Entities.Estate.RealEstate Update(IUnitOfWork unitOfWork, Entities.Estate.RealEstate obj)
        {
            var result = base.Update(unitOfWork, obj);
            return result;
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RealEstate> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RealEstate> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    


                    //---save RealEstate-----------------
                    .SaveOneObject(x => x.RealEstateKind)                   
                    .SaveOneObject(x => x.FeatureTypes)
                    .SaveOneObject(x => x.FeatureUnits)
                    .SaveOneObject(x => x.RealEstatePurpose)
                    .SaveOneObject(x => x.FeatureTypes)
                    //-----------------------------------


                    //---save InventoryObject-----------------
                    .SaveOneObject(x => x.StageOfCompletion)
                    .SaveOneObject(x => x.StatusConstruction)
                    .SaveOneObject(x => x.LayingType)
                    .SaveOneObject(x => x.PropertyComplex)
                  
                    .SaveOneObject(x => x.Parent)
                    .SaveOneObject(x => x.SibRegion)
                    //.SaveOneToMany(x => x.Parents)
                    //.SaveOneToMany(x => x.Childs)                   
                    //-----------------------------------

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


                   //-----------------------------------
                   ;
        }
    }
}
