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
    public interface ISubjectRecordService : IBaseObjectService<SubjectRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - иной субъект.
    /// </summary>
    public class SubjectRecordService : BaseObjectService<SubjectRecord>, ISubjectRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса SubjectRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public SubjectRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>иной субъект.</returns>
        public override SubjectRecord Create(IUnitOfWork unitOfWork, SubjectRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<SubjectRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SubjectRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Partner)
                  
                    ;
        }
    }
}
