using System;
using System.Linq;
using System.Linq.Expressions;
using Base.DAL;
using Base.Security;
using Base.Service;
using Newtonsoft.Json.Linq;

namespace Base.UI.QueryFilter
{
    public interface IQueryTreeBuilderContext : IDisposable
    {
        IQueryable Get(IQueryService<object> service);
        int? GetUserId();
        IUnitOfWork GetUow();
    }
}