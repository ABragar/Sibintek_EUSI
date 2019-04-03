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
    /// Предоставляет данные и методы сервиса объекта - правообладатель.
    /// </summary>
    public interface IRightHolderService : IBaseObjectService<RightHolder>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - правообладатель.
    /// </summary>
    public class RightHolderService : BaseObjectService<RightHolder>, IRightHolderService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RightHolderService.
        /// </summary>
        /// <param name="facade"></param>
        public RightHolderService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override RightHolder Create(IUnitOfWork unitOfWork, RightHolder obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<RightHolder> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<RightHolder> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)

                    .SaveOneObject(x => x.RightRecord)
                    .SaveOneObject(x => x.Extract)
                  
                    ;
        }
    }
}
