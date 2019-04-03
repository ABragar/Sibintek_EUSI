using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Law
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - право.
    /// </summary>
    public interface IRightService : ITypeObjectService<Right>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - право.
    /// </summary>
    public class RightService : TypeObjectService<Right>, IRightService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса RightService.
        /// </summary>
        /// <param name="facade"></param>
        public RightService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Право.</returns>
        public override Right Create(IUnitOfWork unitOfWork, Right obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<Right> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Right> objectSaver)
        {          

            return
                base.GetForSave(unitOfWork, objectSaver)     
                    .SaveOneObject(x => x.RealEstateKind)
                    .SaveOneObject(x => x.AreaUnit)
                    .SaveOneObject(x => x.Region) 
                    .SaveOneObject(x => x.Estate)                    
                    .SaveOneObject(x => x.RightKind)                   
                    .SaveOneObject(x => x.RightType)
                    .SaveOneObject(x => x.Society)
                    .SaveOneObject(x => x.RightHolderKind)
                    .SaveOneObject(x => x.OwnershipType)
                    ;
        }

        protected override void OnSave(Right obj)
        {
            if (obj != null )
            {
                obj.SetShare();
                obj.SetKindAndShare();              
            }
            base.OnSave(obj);
        }
    }
}
