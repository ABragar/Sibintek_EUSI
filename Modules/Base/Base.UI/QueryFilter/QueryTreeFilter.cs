using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Base.DAL;
using Base.Utils.Common.Caching;
using Newtonsoft.Json.Linq;

namespace Base.UI.QueryFilter
{
    public class QueryTreeFilter : IQueryTreeFilter, IQueryTreeService
    {
        private readonly IQueryTreeService _service;
        private readonly ISimpleCacheWrapper _cache;

        public QueryTreeFilter(IQueryTreeService service, ISimpleCacheWrapper cache)
        {
            _service = service;
            _cache = cache;
        }

        public IQueryable<T> BuildQuery<T>(IUnitOfWork unitOfWork, IQueryable<T> source, string key, Func<JToken> json, string mnemonic)
        {
            return (IQueryable<T>)BuildQuery(unitOfWork, (IQueryable)source, key, json, mnemonic);
        }

        public async Task<IQueryable<T>> BuildQueryAsync<T>(IUnitOfWork unitOfWork, IQueryable<T> source, string key, Func<Task<JToken>> json, string mnemonic)
        {
            return (IQueryable<T>)(await BuildQueryAsync(unitOfWork, (IQueryable)source, key, json, mnemonic));
        }


        private static readonly CacheAccessor<TreeOperatorResult<LambdaExpression>> Filters
            = new CacheAccessor<TreeOperatorResult<LambdaExpression>>(TimeSpan.FromHours(30), true);

        public async Task<IQueryable> BuildQueryAsync(IUnitOfWork unitOfWork, IQueryable source, string key, Func<Task<JToken>> json, string mnemonic)
        {
            using (var context = new QueryTreeBuilderContext(unitOfWork))
            {

                var operation = await GetOperationAsync(key, json, mnemonic, source.ElementType);

                var predicate = operation.GetResult(context);

                if (predicate == null)
                    return source;

                return source.Provider.CreateQuery(Expression.Call(typeof(Queryable),
                    nameof(Queryable.Where),
                    new[]
                    {
                        source.ElementType
                    },
                    source.Expression,
                    Expression.Quote(predicate)
                ));
            }
        }

        public IQueryable BuildQuery(IUnitOfWork unitOfWork, IQueryable source, string key, Func<JToken> json, string mnemonic)
        {

            using (var context = new QueryTreeBuilderContext(unitOfWork))
            {
                var predicate = GetOperation(key, json, mnemonic, source.ElementType).GetResult(context);

                if (predicate == null)
                    return source;

                return source.Provider.CreateQuery(Expression.Call(typeof(Queryable),
                    nameof(Queryable.Where),
                    new[]
                    {
                        source.ElementType
                    },
                    source.Expression,
                    Expression.Quote(predicate)
                ));
            }
        }
        private async Task<TreeOperatorResult<LambdaExpression>> GetOperationAsync(string key, Func<Task<JToken>> json, string mnemonic, Type type)
        {
            //TODO dependency
            //TODO найта способ прокинуть контекст в кеширование
            /*return await _cache.GetOrAddAsync(_filters, key, async () =>
            {
                var builder = new QueryTreeBuilder(this);

                return builder.BuildPridecate(await json(), mnemonic, type);
            });*/
 
            var builder = new QueryTreeBuilder(this);

            return builder.BuildPridecate(await json(), mnemonic, type);
            
        }


        private TreeOperatorResult<LambdaExpression> GetOperation(string key, Func<JToken> json, string mnemonic, Type type)
        {
            //TODO dependency
            //TODO найта способ прокинуть контекст в кеширование
            /*return _cache.GetOrAdd(_filters, key, () =>
            {
                var builder = new QueryTreeBuilder(this);

                return builder.BuildPridecate(json(), mnemonic, type);
            });*/

            var builder = new QueryTreeBuilder(this);

            return builder.BuildPridecate(json(), mnemonic, type);
        }


        private class FilterCacheItem
        {
            public QueryTreeFilterModel Filter { get; set; }

            public QueryTreeViewModel ViewModel { get; set; }
        }

        private static readonly CacheAccessor<FilterCacheItem> FilterModels = new CacheAccessor<FilterCacheItem>(TimeSpan.Zero);

        private FilterCacheItem GetFilterItem(string mnemonic)
        {
            return _cache.GetOrAdd(FilterModels, mnemonic, () =>
            {
                var filters = _service.GetFilters(mnemonic);

                var view = new QueryTreeViewModel()
                {
                    Title = filters.Title,
                    Items = filters.Items.Values.Select(x => new QueryTreeItemViewModel()
                    {
                        Id = x.Id,
                        Label = x.Label,
                        Operators = x.Operators.Keys,
                        PrimitiveType = x.PrimitiveType,
                        SystemData = x.SystemData,
                        Plugins = x.Plugins
                    }).OrderBy(x => x.Label)
                };

                return new FilterCacheItem() { Filter = filters, ViewModel = view };
            });
        }

        public QueryTreeViewModel GetFilter(string mnemonic)
        {
            return GetFilterItem(mnemonic).ViewModel;
        }

        QueryTreeFilterModel IQueryTreeService.GetFilters(string mnemonic)
        {
            return GetFilterItem(mnemonic).Filter;
        }

        IEnumerable<QueryTreeItemViewModel> IQueryTreeService.GetAggregatableProperties(string mnemonic)
        {
            throw new NotImplementedException();
        }
    }
}