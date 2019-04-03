using System.Linq;
using Base.DAL;

namespace CorpProp.Services.Response
{
    public class ResponseDynamicQueryService: IResponseDynamicQueryService
    {
        public IQueryable GetAll(IUnitOfWork uofw, bool? hidden, int id)
        {
            //временная заглушка
            //TODO: доработать кастомную логику 
            throw new System.NotImplementedException();
        }
    }
}
