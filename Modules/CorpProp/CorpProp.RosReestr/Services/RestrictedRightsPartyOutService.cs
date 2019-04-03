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
    /// Предоставляет данные и методы сервиса объекта - .
    /// </summary>
    public interface IRestrictedRightsPartyOutService : IBaseObjectService<RestrictedRightsPartyOut>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - .
    /// </summary>
    public class RestrictedRightsPartyOutService : BaseObjectService<RestrictedRightsPartyOut>, IRestrictedRightsPartyOutService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RestrictedRightsPartyOutService.
        /// </summary>
        /// <param name="facade"></param>
        public RestrictedRightsPartyOutService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override RestrictedRightsPartyOut Create(IUnitOfWork unitOfWork, RestrictedRightsPartyOut obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RestrictedRightsPartyOut> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RestrictedRightsPartyOut> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Subject)

                    ;
        }
    }
}
