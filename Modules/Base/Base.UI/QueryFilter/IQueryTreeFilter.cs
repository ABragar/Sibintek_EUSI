using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Base.DAL;

namespace Base.UI.QueryFilter
{
    public interface IQueryTreeFilter
    {
        IQueryable BuildQuery(IUnitOfWork unitOfWork, IQueryable source,string key ,Func<JToken> json, string mnemonic);

        IQueryable<T> BuildQuery<T>(IUnitOfWork unitOfWork, IQueryable<T> source, string key, Func<JToken> json, string mnemonic);

        Task<IQueryable> BuildQueryAsync(IUnitOfWork unitOfWork, IQueryable source, string key, Func<Task<JToken>> json, string mnemonic);

        Task<IQueryable<T>> BuildQueryAsync<T>(IUnitOfWork unitOfWork, IQueryable<T> source, string key, Func<Task<JToken>> json, string mnemonic);

        QueryTreeViewModel GetFilter(string mnemonic);        
    }
}