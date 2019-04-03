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
    /// Предоставляет данные и методы сервиса объекта - юр. лицо.
    /// </summary>
    public interface ILegalSubjectService : IBaseObjectService<LegalSubject>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - юр. лицо.
    /// </summary>
    public class LegalSubjectService : BaseObjectService<LegalSubject>, ILegalSubjectService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса LegalSubjectService.
        /// </summary>
        /// <param name="facade"></param>
        public LegalSubjectService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>юр. лицо.</returns>
        public override LegalSubject Create(IUnitOfWork unitOfWork, LegalSubject obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<LegalSubject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<LegalSubject> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Partner)                   
                    ;
        }
    }
}
