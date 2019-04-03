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
    /// Предоставляет данные и методы сервиса объекта - земля.
    /// </summary>
    public interface ILandRecordService : IBaseObjectService<LandRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - земля.
    /// </summary>
    public class LandRecordService : BaseObjectService<LandRecord>, ILandRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса LandRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public LandRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>земля.</returns>
        public override LandRecord Create(IUnitOfWork unitOfWork, LandRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<LandRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<LandRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Extract)
                    .SaveOneObject(x => x.Cadastral)
                    ;
        }
    }
}
