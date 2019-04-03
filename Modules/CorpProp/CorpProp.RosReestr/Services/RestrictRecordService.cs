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
    /// Предоставляет данные и методы сервиса объекта - ограничение.
    /// </summary>
    public interface IRestrictRecordService : IBaseObjectService<RestrictRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - ограничение.
    /// </summary>
    public class RestrictRecordService : BaseObjectService<RestrictRecord>, IRestrictRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RestrictRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public RestrictRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override RestrictRecord Create(IUnitOfWork unitOfWork, RestrictRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RestrictRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RestrictRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.RightRecord)
                    .SaveOneObject(x => x.ObjectRecord)
                    .SaveOneObject(x => x.Extract)
                    ;
        }
    }
}
