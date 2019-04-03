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
    public interface IRightRecordService : IBaseObjectService<RightRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - ограничение.
    /// </summary>
    public class RightRecordService : BaseObjectService<RightRecord>, IRightRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RightRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public RightRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override RightRecord Create(IUnitOfWork unitOfWork, RightRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RightRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RightRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    
                    .SaveOneObject(x => x.ObjectRecord)
                    .SaveOneObject(x => x.Extract)
                    ;
        }
    }
}
