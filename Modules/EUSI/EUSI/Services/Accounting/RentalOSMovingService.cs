using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Services.Base;
using EUSI.Entities.Accounting;
using System.Collections.Generic;
using System.Data;

namespace EUSI.Services.Accounting
{
    /// <summary>
    /// Предоставляет методы сервиса инициации импорта ФСД движений ОС/НМА по аренде в ЕУСИ.
    /// </summary>
    public interface IRentalOSMovingService : ITypeObjectService<RentalOSMoving>, IExcelImportEntity
    {

    }

    /// <summary>
    /// Представляет сервис для инициации импорта ФСД движений ОС/НМА по аренде в ЕУСИ.
    /// </summary>
    public class RentalOSMovingService : TypeObjectService<RentalOSMoving>, IRentalOSMovingService
    {

        private readonly ILogService _logger;
        protected IAccountingMovingService _movingService;

        
        public RentalOSMovingService(IBaseObjectServiceFacade facade, IAccountingMovingService moving, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _movingService = moving;
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
            SecurityService.ThrowIfAccessDenied(uofw, typeof(RentalOSMoving), Base.Enums.TypePermission.Write | Base.Enums.TypePermission.Read | Base.Enums.TypePermission.Create);
            _movingService.Import(uofw, histUofw, table, colsNameMapping, ref count, ref history);
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
            SecurityService.ThrowIfAccessDenied(uow, typeof(RentalOSMoving), Base.Enums.TypePermission.Write | Base.Enums.TypePermission.Read | Base.Enums.TypePermission.Delete);
            _movingService.CancelImport(uow, ref history);
        }

    }




}
