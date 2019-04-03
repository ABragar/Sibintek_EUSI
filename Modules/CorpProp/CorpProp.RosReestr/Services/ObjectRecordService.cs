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
    /// Предоставляет данные и методы сервиса объекта - ОНИ.
    /// </summary>
    public interface IObjectRecordService : IBaseObjectService<ObjectRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - ОНИ.
    /// </summary>
    public class ObjectRecordService : BaseObjectService<ObjectRecord>, IObjectRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ObjectRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public ObjectRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>ОНИ.</returns>
        public override ObjectRecord Create(IUnitOfWork unitOfWork, ObjectRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ObjectRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ObjectRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Extract)
                    .SaveOneObject(x => x.Cadastral)
                    ;
        }
    }
}
