using Base.DAL;
using Base.Service;
using Base.Service.Log;
using Base.UI.Service;
using CorpProp.Common;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using CorpProp.Services.Base;
using EUSI.Entities.Accounting;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace EUSI.Services.Accounting
{
    /// <summary>
    /// Предоставляет методы сервиса инициации миграции ОС/НМА в ЕУСИ.
    /// </summary>
    public interface IMigrateOSService : ITypeObjectService<MigrateOS>, IExcelImportEntity, IExcelImportChecker
    {

    }

    /// <summary>
    /// Представляет сервис для инициации миграции ОС/НМА в ЕУСИ.
    /// </summary>
    public class MigrateOSService : TypeObjectService<MigrateOS>, IMigrateOSService
    {
        /// <summary>
        /// Сервис миграции ОС/НМА в ЕУСИ.
        /// </summary>
        protected IAccountingObjectMigrate _migrate;
        private readonly ILogService _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса MigrateOSService.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="migrate"></param>
        public MigrateOSService(IBaseObjectServiceFacade facade, IAccountingObjectMigrate migrate, ILogService logger) : base(facade, logger)
        {
            _migrate = migrate;
            _logger = logger;
        }

        /// <summary>
        /// Импорт.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="colsNameMapping"></param>
        /// <param name="count"></param>
        /// <param name="history"></param>
        public void Import(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {
            SecurityService.ThrowIfAccessDenied(uofw, typeof(MigrateOS), Base.Enums.TypePermission.Write | Base.Enums.TypePermission.Read | Base.Enums.TypePermission.Create);
            _migrate.Import(uofw,histUofw,table,colsNameMapping,ref count, ref history);
        }

        /// <summary>
        /// Отмена импорта.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="history"></param>
        public void CancelImport(
            IUnitOfWork uow
           , ref ImportHistory history
           )
        {
            SecurityService.ThrowIfAccessDenied(uow, typeof(MigrateOS), Base.Enums.TypePermission.Write | Base.Enums.TypePermission.Read | Base.Enums.TypePermission.Delete);
            _migrate.CancelImport(uow, ref history);
        }

        public void StartDataCheck(IUnitOfWork uofw, IUnitOfWork histUow, DataTable table, Type type, ref ImportHistory history, bool dictCode = false)
        {
            //проверки вызываются из сервиса ОС
            return;
        }
    
        public CheckImportResult CheckVersionImport(IUiFasade uiFacade, IExcelDataReader reader, ITransactionUnitOfWork uofw,
           StreamReader stream, string fileName)
        {            
            return null;
        }

        public string FormatConfirmImportMessage(List<string> fileDescriptions)
        {
            var singleName = "БЕ";
            var pluralName = "БЕ";
            return EUSI.Common.ConfirmImportMessageFormatter.FormatConfirmImportMessage(singleName, pluralName, fileDescriptions);
        }
    }


    

}
