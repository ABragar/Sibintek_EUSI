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
    /// Предоставляет данные и методы сервиса объекта - стоимость права.
    /// </summary>
    public interface IRightCostViewService : ITypeObjectService<RightCostView>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - стоимость права.
    /// </summary>
    public class RightCostViewService : TypeObjectService<RightCostView>, IRightCostViewService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса RightCostViewService.
        /// </summary>
        /// <param name="facade"></param>
        public RightCostViewService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>стоимость права.</returns>
        public override RightCostView Create(IUnitOfWork unitOfWork, RightCostView obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RightCostView> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RightCostView> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver);
        }
    }
}
