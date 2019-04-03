using System.Linq;
using CorpProp.Entities.Request;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;

namespace CorpProp.Services.Response.Fasade
{
    public class TableData
    {
        public IQueryable<RequestColumn> ResponseColumns { get; set; }
        public IQueryable<ResponseRow> ResponseRow { get; set; }
        public IQueryable<Entities.Request.Request> Request { get; set; }
        public IQueryable<Entities.Request.Response> Response { get; set; }
        public IQueryable<SibUser> SibUser { get; set; }
        public IQueryable<Society> Society { get; set; }

    }
}
