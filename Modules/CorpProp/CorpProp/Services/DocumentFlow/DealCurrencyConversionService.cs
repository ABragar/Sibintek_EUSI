using System;
using System.Linq;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.NSI;

namespace CorpProp.Services.DocumentFlow
{
    public class DealService : BaseObjectService<SibDeal>
    {
        public DealService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }

    public class DealCurrencyConversionService : DealService, IDealCurrencyConversionService
    {
        public DealCurrencyConversionService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public IQueryable GetAll(IUnitOfWork unitOfWork, int targetedCurrencyId, DateTime exchangeDate,
            bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden).ToList().Select(p => new DealCurrencyConversion(p)
            {
                SumDealInTargetedCurrency =
                    Convert(unitOfWork, p.SumDeal, p.CurrencyID.Value, targetedCurrencyId, exchangeDate)
            }).AsQueryable();
        }

        private decimal Convert(IUnitOfWork unitOfWork, decimal sum, int currentCurrencyId, int targetCurrencyId,
            DateTime exchangeDate)
        {
            var currentExchangeRate =
                unitOfWork.GetRepository<ExchangeRate>()
                    .All()
                    .FirstOrDefault(p => p.CurrencyID == currentCurrencyId && p.Date == exchangeDate);

            var targetedExchangeRate =
                unitOfWork.GetRepository<ExchangeRate>()
                    .All()
                    .FirstOrDefault(p => p.CurrencyID == targetCurrencyId && p.Date == exchangeDate);

            if (currentExchangeRate == null) throw new ExchangeRateNotFoundException(exchangeDate, currentCurrencyId);
            if (targetedExchangeRate == null) throw new ExchangeRateNotFoundException(exchangeDate, targetCurrencyId);

            return sum * currentExchangeRate.Rate * targetedExchangeRate.Amount /
                   (currentExchangeRate.Amount * targetedExchangeRate.Rate);
        }
    }
}
