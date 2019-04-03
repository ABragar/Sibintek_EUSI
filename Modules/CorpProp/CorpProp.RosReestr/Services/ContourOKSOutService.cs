using Base.DAL;
using Base.Service;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Services
{

    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - Контур ОКС.
    /// </summary>
    public interface IContourOKSOutService : IBaseObjectService<ContourOKSOut>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - Контур ОКС.
    /// </summary>
    public class ContourOKSOutService : BaseObjectService<ContourOKSOut>, IContourOKSOutService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ContourOKSOutService.
        /// </summary>
        /// <param name="facade"></param>
        public ContourOKSOutService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Контур ОКС.</returns>
        public override ContourOKSOut Create(IUnitOfWork unitOfWork, ContourOKSOut obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ContourOKSOut> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ContourOKSOut> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ObjectRecord)
                    .SaveOneObject(x => x.Extract)
                    ;
        }
    }
}
