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
    /// Предоставляет данные и методы сервиса объекта - Кадастровый номер.
    /// </summary>
    public interface IOldNumberService : IBaseObjectService<OldNumber>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - Кадастровый номер.
    /// </summary>
    public class OldNumberService : BaseObjectService<OldNumber>, IOldNumberService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса OldNumberService.
        /// </summary>
        /// <param name="facade"></param>
        public OldNumberService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Кадастровый номер.</returns>
        public override OldNumber Create(IUnitOfWork unitOfWork, OldNumber obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<OldNumber> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<OldNumber> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Extract)
                    .SaveOneObject(x => x.ObjectRecord)
                   

                    ;
        }
    }
}
