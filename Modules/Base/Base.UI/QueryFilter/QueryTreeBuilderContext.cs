using System.Linq;
using Base.Ambient;
using Base.DAL;
using Base.Security;
using Base.Service;

namespace Base.UI.QueryFilter
{
    class QueryTreeBuilderContext : IQueryTreeBuilderContext
    {
        private readonly IUnitOfWork _unitOfWork;

        public QueryTreeBuilderContext(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable Get(IQueryService<object> service)
        {
            IQueryable query = service.GetAll(_unitOfWork);

            return query.GetInnerQuery();
        }

        public int? GetUserId()
        {
            return AppContext.SecurityUser?.ID;
        }

        public IUnitOfWork GetUow()
        {
            return _unitOfWork;
        }

        public void Dispose()
        {

        }
    }
}