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
    /// Предоставляет данные и методы сервиса объекта - ППР.
    /// </summary>
    public interface IPublicSubjectService : IBaseObjectService<PublicSubject>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - ППР.
    /// </summary>
    public class PublicSubjectService : BaseObjectService<PublicSubject>, IPublicSubjectService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса PublicSubjectService.
        /// </summary>
        /// <param name="facade"></param>
        public PublicSubjectService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>ППР.</returns>
        public override PublicSubject Create(IUnitOfWork unitOfWork, PublicSubject obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<PublicSubject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<PublicSubject> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Partner)
                    
                    ;
        }
    }
}
