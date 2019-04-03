using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Entities.Estate;
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
    /// Предоставляет данные и методы сервиса для помещение.
    /// </summary>
    public interface IRoomService : ITypeObjectService<Room>
    {
    }

    /// <summary>
    /// Представляет сервис для работы с кадастровым объектом.
    /// </summary>
    public class RoomService : BaseEstateService<Room>, IRoomService
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса RoomService.
        /// </summary>
        /// <param name="facade"></param>
        public RoomService(IBaseObjectServiceFacade facade, INonCoreAssetService nonCoreAssetService, IAccountingObjectService osService, ILogService logger) :
            base(facade, nonCoreAssetService, osService, logger)
        {
            _logger = logger;
        }

        public override IQueryable<Room> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }
        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Помещение.</returns>
        public override Room Create(IUnitOfWork unitOfWork, Room obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<Room> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Room> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
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
