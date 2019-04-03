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
    /// Предоставляет данные и методы сервиса объекта - вид разрешенного использования.
    /// </summary>
    public interface IPermittedUseService : IBaseObjectService<PermittedUse>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - вид разрешенного использования.
    /// </summary>
    public class PermittedUseService : BaseObjectService<PermittedUse>, IPermittedUseService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса PermittedUseService.
        /// </summary>
        /// <param name="facade"></param>
        public PermittedUseService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>вид разрешенного использования.</returns>
        public override PermittedUse Create(IUnitOfWork unitOfWork, PermittedUse obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<PermittedUse> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<PermittedUse> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Extract)
                    .SaveOneObject(x => x.ObjectRecord)


                    ;
        }
    }
}
