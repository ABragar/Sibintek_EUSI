using System;
using System.Linq;
using Base.DAL;

namespace CorpProp.Services.DocumentFlow
{
    public interface IDealCurrencyConversionService
    {
        IQueryable GetAll(IUnitOfWork unitOfWork, int targetedCurrencyId, DateTime exchangeDate, bool? hidden = false);
    }
}
