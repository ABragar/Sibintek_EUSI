using System.Linq;
using Base.DAL;
using Base.Service;

namespace CorpProp.Services.Response
{
    public interface IResponseDynamicQueryService: IService
    {
        IQueryable GetAll(IUnitOfWork uofw, bool? hidden, int id);
    }
}
