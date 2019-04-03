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
    /// Предоставляет данные и методы сервиса объекта - Местоположение машино-мест в ОНИ.
    /// </summary>
    public interface ICarParkingSpaceLocationInBuildPlansService : IBaseObjectService<CarParkingSpaceLocationInBuildPlans>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - Местоположение машино-мест в ОНИ.
    /// </summary>
    public class CarParkingSpaceLocationInBuildPlansService : BaseObjectService<CarParkingSpaceLocationInBuildPlans>, ICarParkingSpaceLocationInBuildPlansService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса CarParkingSpaceLocationInBuildPlansService.
        /// </summary>
        /// <param name="facade"></param>
        public CarParkingSpaceLocationInBuildPlansService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Местоположение машино-мест в ОНИ.</returns>
        public override CarParkingSpaceLocationInBuildPlans Create(IUnitOfWork unitOfWork, CarParkingSpaceLocationInBuildPlans obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<CarParkingSpaceLocationInBuildPlans> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<CarParkingSpaceLocationInBuildPlans> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ObjectRecord)
                    .SaveOneObject(x => x.Extract)
                    ;
        }
    }
}
