using Base.Service;
using CorpProp.Entities.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using CorpProp.Services.Base;
using Base.Service.Log;

namespace CorpProp.Services.Import
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - история импорта.
    /// </summary>
    public interface IImportErrorLogService : ITypeObjectService<ImportErrorLog>
    {

    }
    public class ImportErrorLogService : TypeObjectService<ImportErrorLog>, IImportErrorLogService
    {

        private readonly ILogService _logger;

        public ImportErrorLogService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        public override ImportErrorLog Create(IUnitOfWork unitOfWork, ImportErrorLog obj)
        {
            return base.Create(unitOfWork, obj);
        }

        protected override IObjectSaver<ImportErrorLog> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ImportErrorLog> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                       .SaveOneObject(x => x.ImportHistory);
        }
    }
}
