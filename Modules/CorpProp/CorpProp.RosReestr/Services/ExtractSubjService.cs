using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.RosReestr.Entities;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Services
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - выписка ЮЛ.
    /// </summary>
    public interface IExtractSubjService : ITypeObjectService<ExtractSubj>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - выписка ЮЛ.
    /// </summary>
    public class ExtractSubjService : TypeObjectService<ExtractSubj>, IExtractSubjService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractSubjService.
        /// </summary>
        /// <param name="facade"></param>
        public ExtractSubjService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>выписка ЮЛ.</returns>
        public override ExtractSubj Create(IUnitOfWork unitOfWork, ExtractSubj obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ExtractSubj> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ExtractSubj> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ExtractType)
                    .SaveOneObject(x => x.ExtractFormat)
                    .SaveOneObject(x => x.Society)
                    .SaveOneObject(x => x.FileCard)
                    ;
        }
    }
}
