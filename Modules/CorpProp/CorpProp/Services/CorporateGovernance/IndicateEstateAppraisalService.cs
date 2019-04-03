using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.CorporateGovernance
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - индикация оценки.
    /// </summary>
    public interface IIndicateEstateAppraisalService : ITypeObjectService<IndicateEstateAppraisalView>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - право.
    /// </summary>
    public class IndicateEstateAppraisalService : TypeObjectService<IndicateEstateAppraisalView>, IIndicateEstateAppraisalService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса IndicateEstateAppraisalService.
        /// </summary>
        /// <param name="facade"></param>
        public IndicateEstateAppraisalService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Индикация оценки.</returns>
        public override IndicateEstateAppraisalView Create(IUnitOfWork unitOfWork, IndicateEstateAppraisalView obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<IndicateEstateAppraisalView> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<IndicateEstateAppraisalView> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                   
                    ;
        }
    }
}
