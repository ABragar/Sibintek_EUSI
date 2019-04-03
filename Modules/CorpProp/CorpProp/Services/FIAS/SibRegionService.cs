using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.FIAS
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - регион.
    /// </summary>
    public interface ISibRegionService : IBaseObjectService<SibRegion>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - регион.
    /// </summary>
    public class SibRegionService : DictObjectService<SibRegion>, ISibRegionService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса SibRegionService.
        /// </summary>
        /// <param name="facade"></param>
        public SibRegionService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Регион.</returns>
        public override SibRegion Create(IUnitOfWork unitOfWork, SibRegion obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<SibRegion> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SibRegion> objectSaver)
        {
            
            return
                base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Country)
                .SaveOneObject(x => x.FederalDistrict)
                ;
        }

        protected override void OnSave(SibRegion obj)
        {
            
            base.OnSave(obj);
            obj.Title = obj.Code + " " + obj.Name;
        }
    }


    public interface ISibRegionHistoryService : IBaseObjectService<SibRegion>
    {

    }

    public class SibRegionHistoryService : DictHistoryService<SibRegion>, ISibRegionHistoryService
    {
        public SibRegionHistoryService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        protected override IObjectSaver<SibRegion> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibRegion> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Country)
                .SaveOneObject(x => x.FederalDistrict)
                ;
        }

        protected override void OnSave(SibRegion obj)
        {
            obj.Title = obj.Code + " " + obj.Name;
            base.OnSave(obj);
        }
    }
}
