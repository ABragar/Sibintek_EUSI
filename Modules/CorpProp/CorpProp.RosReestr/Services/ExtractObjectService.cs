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
    /// Предоставляет данные и методы сервиса объекта - выписка на ОНИ.
    /// </summary>
    public interface IExtractObjectService : ITypeObjectService<ExtractObject>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - выписка на ОНИ.
    /// </summary>
    public class ExtractObjectService : TypeObjectService<ExtractObject>, IExtractObjectService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractObjectService.
        /// </summary>
        /// <param name="facade"></param>
        public ExtractObjectService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>выписка - здание.</returns>
        public override ExtractObject Create(IUnitOfWork unitOfWork, ExtractObject obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ExtractObject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ExtractObject> objectSaver)
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
