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
    /// Предоставляет данные и методы сервиса объекта - иной субъект.
    /// </summary>
    public interface IAnotherSubjectService : IBaseObjectService<AnotherSubject>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - иной субъект.
    /// </summary>
    public class AnotherSubjectService : BaseObjectService<AnotherSubject>, IAnotherSubjectService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса AnotherSubjectService.
        /// </summary>
        /// <param name="facade"></param>
        public AnotherSubjectService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>иной субъект.</returns>
        public override AnotherSubject Create(IUnitOfWork unitOfWork, AnotherSubject obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<AnotherSubject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<AnotherSubject> objectSaver)
        {
           
            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Partner)
                   
                    ;
        }
    }
}
