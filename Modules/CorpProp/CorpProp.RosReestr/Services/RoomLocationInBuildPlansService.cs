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
    /// Предоставляет данные и методы сервиса объекта - местоположение помещения.
    /// </summary>
    public interface IRoomLocationInBuildPlansService : IBaseObjectService<RoomLocationInBuildPlans>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - местоположение помещения.
    /// </summary>
    public class RoomLocationInBuildPlansService : BaseObjectService<RoomLocationInBuildPlans>, IRoomLocationInBuildPlansService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RoomLocationInBuildPlansService.
        /// </summary>
        /// <param name="facade"></param>
        public RoomLocationInBuildPlansService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override RoomLocationInBuildPlans Create(IUnitOfWork unitOfWork, RoomLocationInBuildPlans obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RoomLocationInBuildPlans> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RoomLocationInBuildPlans> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Extract)
                    .SaveOneObject(x => x.ObjectRecord)
                    ;
        }
    }
}
