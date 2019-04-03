using System;
using System.Linq;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Services.Accounting;
using CorpProp.Services.Asset;
using CorpProp.Services.Base;
using CorpProp.Services.Estate;

namespace EUSI.Services.Estate
{
    public interface IArchivedEstateService<T> : ITypeObjectService<T>, IArchiveService where T : CorpProp.Entities.Estate.Estate
    {
        
    }

    public class ArchivedEstateService<T> : TypeObjectService<T>, IArchivedEstateService<T> where T : CorpProp.Entities.Estate.Estate
    {
        private readonly ILogService _logger;
        public ArchivedEstateService(
            IBaseObjectServiceFacade facade, ILogService logger)
            : base(facade, logger)
        {
            _logger = logger;
        
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden)
                .Where(e => e.IsArchived.HasValue && e.IsArchived.Value);
        }
        public override IQueryable<T> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            return base.GetAllByDate(uow, date)
                .Where(e => e.IsArchived.HasValue && e.IsArchived.Value);
        }
    }
}
