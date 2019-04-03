using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using EUSI.Entities.Accounting;
using EUSI.Services.Monitor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EUSI.Services.Accounting
{
    /// <summary>
    /// Предоставляет методы сервиса инициации импорта состояний ОС/НМА ФСД аренда в ЕУСИ.
    /// </summary>
    public interface IRentalOSStateService : ITypeObjectService<RentalOSState>, IExcelImportEntity
    {

    }

    /// <summary>
    /// Представляет сервис для инициации импорта состояний ОС/НМА ФСД аренда в ЕУСИ.
    /// </summary>
    public class RentalOSStateService : TypeObjectService<RentalOSState>, IRentalOSStateService
    {

        private readonly ILogService _logger;
        protected IAccountingObjectExtService _osService;
        private readonly MonitorReportingImportService monitor = new MonitorReportingImportService();



        public RentalOSStateService(IBaseObjectServiceFacade facade, IAccountingObjectMigrate osService, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _osService = osService;
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
            try
            {
                SecurityService.ThrowIfAccessDenied(uofw, typeof(RentalOSState), Base.Enums.TypePermission.Write | Base.Enums.TypePermission.Read | Base.Enums.TypePermission.Create);
                _osService.Import(uofw, histUofw, table, colsNameMapping, ref count, ref history);
                if (!history.ImportErrorLogs.Any())
                {
                    monitor.CreateMonitorReportingForSuccessImport(history, count, histUofw);
                }
                else
                {
                    monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
                monitor.CreateMonitorReportingForFailedImport(history, count, histUofw);
            }


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
            SecurityService.ThrowIfAccessDenied(uow, typeof(RentalOSState), Base.Enums.TypePermission.Write | Base.Enums.TypePermission.Read | Base.Enums.TypePermission.Delete);
            _osService.CancelImport(uow, ref history);
        }

    }




}
