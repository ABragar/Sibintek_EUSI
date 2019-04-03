using Base.DAL;
using Base.Service;
using CorpProp.Entities.Import;
using CorpProp.Services.Base;
using System.Linq;
using CorpProp.Entities.Subject;
using Base.Service.Log;
using CorpProp.Helpers.Import.Extentions;

namespace CorpProp.Services.Import
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - история импорта.
    /// </summary>
    public interface IImportHistoryService : ITypeObjectService<ImportHistory>
    {
        /// <summary>
        /// Инициализация ОГ в истории импорта
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="code"></param>
        /// <param name="sysObjName"></param>
        /// <param name="history"></param>
        void InitSociety(IUnitOfWork uofw, string code, bool isRequired, ref ImportHistory history);

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - история импорта.
    /// </summary>
    public class ImportHistoryService : TypeObjectService<ImportHistory>, IImportHistoryService
    {
        private readonly ILogService _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса ImportHistoryService.
        /// </summary>
        /// <param name="facade"></param>
        public ImportHistoryService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        public virtual void InitConsolidation(IUnitOfWork uofw, ref ImportHistory history, string consolidationCode)
        {
            return;
        }

        /// <summary>
        /// Метод для инициализации ОГ в истории импорта, поиск по коду ЕУП
        /// </summary>
        /// <param name="uofw">сессия</param>
        /// <param name="code">код ЕУП</param>
        /// <param name="sysObjName">название импортируемого типа</param>
        /// <param name="history">история импорта</param>
        public virtual void InitSociety(IUnitOfWork uofw, string code, bool isRequired, ref ImportHistory history)
        {
            history.Society = uofw.GetRepository<Society>()
                    .Filter(f => !f.Hidden && !f.IsHistory && f.IDEUP == code)
                    .FirstOrDefault();

            if (isRequired)
                history.ImportErrorLogs.AddError(Helpers.ErrorTypeName.InvalidFileNameFormat                 
                + $"ОГ с кодом ЕУП <{code}> не найдено в Системе.");
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ImportHistory> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ImportHistory> objectSaver)
        {

            return base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.SibUser)
                    .SaveOneObject(x => x.Society)
                    .SaveOneObject(x => x.ImportHistoryState)
                    .SaveOneObject(x => x.Consolidation)
                    ;
        }
    }


}
