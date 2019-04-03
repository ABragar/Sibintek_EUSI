using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Services.Base;
using EUSI.Entities.Accounting;
using System;
using System.Linq;

namespace EUSI.Services.Accounting
{
    /// <summary>
    /// Предоставляет методы по работе с сервисом историчных версионных записей регистров движений.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccountingMovingHistoryService<T> : ITypeObjectService<T> where T: AccountingMoving
    {

    }
    /// <summary>
    /// Представляет сервис историчных версионных записей регистров движений.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AccountingMovingHistoryService<T> : TypeObjectService<T>, IAccountingMovingHistoryService<T> where T : AccountingMoving
    {
        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingMovingHistoryService.
        /// </summary>
        /// <param name="facade"></param>
        public AccountingMovingHistoryService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Переопределяет метод получения актуальных данных на заданную дату.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="date">Дата.</param>
        /// <returns></returns>
        public override IQueryable<T> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            return base.GetAll(uow, false).Where(f => f.IsHistory);
        }
    }
}
