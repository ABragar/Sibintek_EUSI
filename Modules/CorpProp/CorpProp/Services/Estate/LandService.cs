using System.Linq;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Estate;
using CorpProp.Helpers;
using CorpProp.Services.Accounting;
using CorpProp.Services.Asset;


namespace CorpProp.Services.Estate
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - земельный участок.
    /// </summary>
    public interface ILandService : Base.ITypeObjectService<Land>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - земельный участок.
    /// </summary>
    public class LandService : BaseEstateService<Land>, ILandService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса ShipService.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="nonCoreAssetService"></param>
        public LandService(IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) :
            base(facade, nonCoreAssetService, osService, logger)
        {
            _logger = logger;
         
        }
        public override IQueryable<Land> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Судно.</returns>
        public override Land Create(IUnitOfWork unitOfWork, Land obj)
        {
            //var result = EstateHelper.AddMembers<Entities.Estate.Land>(unitOfWork, base.Create(unitOfWork, obj));
            //return result;
            return base.Create(unitOfWork, obj);
        }

        public override Entities.Estate.Land Update(IUnitOfWork unitOfWork, Entities.Estate.Land obj)
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
        protected override IObjectSaver<Land> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Land> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.GroundCategory)
                    .SaveOneObject(x => x.LandType)                    

                    //---save Cadastral-----------------
                    .SaveOneObject(x => x.AreaUnit)

                    //-----------------------------------   


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
