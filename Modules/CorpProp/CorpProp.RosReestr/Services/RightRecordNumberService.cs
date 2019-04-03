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
    /// Предоставляет данные и методы сервиса объекта - наименования.
    /// </summary>
    public interface IRightRecordNumberService : IBaseObjectService<RightRecordNumber>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - наименования.
    /// </summary>
    public class RightRecordNumberService : BaseObjectService<RightRecordNumber>, IRightRecordNumberService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RightRecordNumberService.
        /// </summary>
        /// <param name="facade"></param>
        public RightRecordNumberService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override RightRecordNumber Create(IUnitOfWork unitOfWork, RightRecordNumber obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RightRecordNumber> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RightRecordNumber> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.RestrictRecord)

                    ;
        }
    }
}
