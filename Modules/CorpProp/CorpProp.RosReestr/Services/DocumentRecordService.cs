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
    /// Предоставляет данные и методы сервиса объекта - документ основания.
    /// </summary>
    public interface IDocumentRecordService : IBaseObjectService<DocumentRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - документ основания.
    /// </summary>
    public class DocumentRecordService : BaseObjectService<DocumentRecord>, IDocumentRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса DocumentRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public DocumentRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>документ основания.</returns>
        public override DocumentRecord Create(IUnitOfWork unitOfWork, DocumentRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<DocumentRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<DocumentRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.RestrictRecord)
                    .SaveOneObject(x => x.RightRecord)
                   
                    ;
        }
    }
}
