using System.Linq;
using Base.DAL;

namespace CorpProp.Services.Response
{
    public class RequestDynamicQueryService: IRequestDynamicQueryService
    {
        public IQueryable GetAll(IUnitOfWork uofw, bool? hidden)
        {
            throw new System.NotImplementedException();
        }
    }
}
