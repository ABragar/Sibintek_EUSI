using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Law;
using CorpProp.Services.Base;
using System;
using System.IO;
using System.Linq;

namespace CorpProp.Services.Law
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - выписка.
    /// </summary>
    public interface IExtractService : ITypeObjectService<Extract>, IXmlDataImport
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - выписка.
    /// </summary>
    public class ExtractService : TypeObjectService<Extract>, IExtractService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractService.
        /// </summary>
        /// <param name="facade"></param>
        public ExtractService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>выписка.</returns>
        public override Extract Create(IUnitOfWork unitOfWork, Extract obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<Extract> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Extract> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ExtractType)
                    .SaveOneObject(x => x.ExtractFormat)
                    .SaveOneObject(x => x.Society)
                    .SaveOneObject(x => x.FileCard)
                    ;
        }


        public void ImportXML(IUnitOfWork unitOfWork, Stream stream, ref string error)
        {
            return;
        }
    }
}
