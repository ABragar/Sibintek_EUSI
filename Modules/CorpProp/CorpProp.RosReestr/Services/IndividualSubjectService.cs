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
    /// Предоставляет данные и методы сервиса объекта - физ.лицо.
    /// </summary>
    public interface IIndividualSubjectService : IBaseObjectService<IndividualSubject>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - физ.лицо.
    /// </summary>
    public class IndividualSubjectService : BaseObjectService<IndividualSubject>, IIndividualSubjectService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса IndividualSubjectService.
        /// </summary>
        /// <param name="facade"></param>
        public IndividualSubjectService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override IndividualSubject Create(IUnitOfWork unitOfWork, IndividualSubject obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<IndividualSubject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<IndividualSubject> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Partner)
                   
                    ;
        }
    }
}
