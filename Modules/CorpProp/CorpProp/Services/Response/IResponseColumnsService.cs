using System.Linq;
using Base.DAL;
using Base.Service;

namespace CorpProp.Services.Response
{
    interface IResponseColumnsService: IService
    {
        IQueryable GetAll(IUnitOfWork uofw, bool? hidden);
    }
}
