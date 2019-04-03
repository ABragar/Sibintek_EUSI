using System.Linq;
using Base.DAL;
using Base.Service;

namespace CorpProp.Services.Response
{
    public interface IRequestDynamicQueryService: IService
    {
        IQueryable GetAll(IUnitOfWork uofw, bool? hidden);
    }
}
