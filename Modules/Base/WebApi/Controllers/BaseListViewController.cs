using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.UI;
using Base.UI.Extensions;
using Base.UI.Filter;
using Base.UI.Service.Abstract;
using Base.Utils.Common;
using Base.Utils.Common.Caching;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebApi.Models.ListView;
using CorpProp.Common;
using CorpProp.Services.Base;
using System;
using CorpProp.Extentions;
using System.Text.RegularExpressions;
using System.Collections;
using Base.Extensions;
using System.Web;
using System.Linq.Dynamic;
using System.Collections.Generic;
using WebApi.Extensions;
using Base.Service.Log;

namespace WebApi.Controllers
{
    public abstract class BaseListViewController : BaseApiController
    {
        private IViewModelConfigService _viewModelConfigService;
        private readonly IMnemonicFilterService<MnemonicFilter> _mnemonicFilterService;
        private readonly ILogService _logger;

        protected BaseListViewController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService, ISimpleCacheWrapper simpleCache, ILogService logger)
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _mnemonicFilterService = mnemonicFilterService;
            _viewModelConfigService = viewModelConfigService;
            SimpleCache = simpleCache;
        }

        protected ISimpleCacheWrapper SimpleCache { get; }

        protected bool IsChildMnemonicsExist
        {
            get { return GetConfig().ListView.Columns.Any(x => !string.IsNullOrEmpty(x.ChildMnemonic)); }
        }

        protected async Task<IQueryable<T>> Read<T>(IUnitOfWork uofw, ListViewParams lvParams) where T : IBaseObject
        {
            var serv = GetQueryService<T>();
            var q = serv.GetAll(uofw);

            //TODO: историчность
            try
            {
                if (serv is IHistory)
                {
                    var dt = (!string.IsNullOrEmpty(lvParams.Date)) ? lvParams.Date.GetDate() : null;
                    q = ((IHistoryService<T>)serv).GetAllByDate(uofw, dt);
                }
                if (serv is ICustomDS)
                {
                    q = ((ICustomDataSource<T>)serv).GetAllCustom(uofw, lvParams.CustomParams);
                }
            }
            catch { q = serv.GetAll(uofw); }

            return q;
        }

        protected async Task<IQueryable<T>> CategorizedItemRead<T>(IUnitOfWork uofw, ListViewParams lvParams, int categoryId, bool allItems = false) where T : ICategorizedItem
        {
            var serv = GetBaseCategorizedItemService<T>();

            var q = allItems ? serv.GetAllCategorizedItems(uofw, categoryId) : serv.GetCategorizedItems(uofw, categoryId);

            q = await AddFilter(uofw, q, lvParams);

            return q;
        }

        protected async Task<IHttpActionResult> ToResultAsync<T>(IQueryable<T> q, DataSourceRequest request, string[] columns = null)
        {
            return Ok(await q.Select(GetConfig().ListView, columns).ToDataSourceResultAsync(request));
        }

        protected async Task<IHttpActionResult> ToResultAsync(IQueryable q, DataSourceRequest request, string[] columns = null)
        {
            return Ok(await q.Select(GetConfig().ListView, columns).ToDataSourceResultAsync(request));
        }

        protected async Task<IQueryable<T>> AddFilter<T>(IUnitOfWork uofw, IQueryable<T> q, ListViewParams lvParams, string[] columns = null) where T : IBaseObject
        {
            if (lvParams == null) return q;

            var config = GetConfig();

            if (lvParams.MnemonicFilterId != null)
            {
                q = await _mnemonicFilterService.AddMnemonicFilter(uofw, q, config.Mnemonic, lvParams.MnemonicFilterId.Value);
            }

            q = q.ListViewFilterGeneric(config.ListView, lvParams.Extrafilter);

            if (columns == null || (columns != null && columns.Length == 0))
                columns = config.ListView.Columns.Select(s => s.PropertyName).ToArray();

            var lookups = typeof(T).GetProperties().Where(p => columns.Contains(p.Name))
                .Join(_viewModelConfigService.GetAll(), pr => pr.PropertyType, vm => vm.TypeEntity, (pr, vm) =>
                new
                {
                    typename = pr.PropertyType.Name,
                    lookup = vm.LookupProperty.Text
                })
                .GroupBy(g => g.typename)
                .ToDictionary(gr => gr.Key, v => v.FirstOrDefault().lookup);

            q = q.FullTextSearch(lvParams.SearchStr, SimpleCache, columns, lookups);

            return q;
        }

        protected async Task<IQueryable> AddFilter(IUnitOfWork uofw, IQueryable q, ListViewParams lvParams)
        {
            if (lvParams == null) return q;

            var config = GetConfig();

            if (lvParams.MnemonicFilterId != null)
            {
                q = await _mnemonicFilterService.AddMnemonicFilter(uofw, q, config.Mnemonic, lvParams.MnemonicFilterId.Value);
            }

            //q = q.ListViewFilter(config.ListView, lvParams.Extrafilter);

            //q = q.FullTextSearch(lvParams.SearchStr, SimpleCache);

            return q;
        }

        protected async Task<IHttpActionResult> GetStrPropertyForFilter_<T>(string mnemonic, string startswith, string property, string filter = null,
           Func<IQueryable<T>, IUnitOfWork, IQueryable<T>> funcFilter = null)
           where T : BaseObject
        {
            //validation
            CheckProperty(property);

            using (var uofw = this.CreateUnitOfWork())
            {
                var bserv = GetQueryService<T>();

                var query = bserv?.GetAll(uofw);

                if (query == null) return null;

                if (funcFilter != null)
                    query = funcFilter(query, uofw);

                var q = query.Filter(GetConfig(), filter);

                startswith = startswith?.Trim();

                if (!string.IsNullOrEmpty(startswith))
                {
                    var res =
                        await
                            q.Select("it." + property)
                                //sanitization
                                .Where($"it.Contains(\"{startswith.Sanitize()}\")")
                                .Cast<string>()
                                .Distinct()
                                .Take(50)
                                .ToListAsync();

                    string pattern = string.Format(startswith.Length == 1 ? @"\b{0}\S*" : @"\S*{0}\S*", startswith);

                    var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                    var res2 = (from str in res from Match match in regex.Matches(str) select match.Value).ToList();

                    return Ok(res2.Distinct().Take(20));
                }
                else
                {
                    return
                        Ok(
                            await q.Select("it." + property).Cast<string>().Distinct().Take(20).ToListAsync());
                }
            }

        }

        protected async Task<IHttpActionResult> GetUniqueValuesForProperty_<T>(string mnemonic, string property,
            Func<IQueryable<T>, IUnitOfWork, IQueryable<T>> funcFilter = null)
            where T : BaseObject
        {
            //validation
            CheckProperty(property);

            var config = GetConfig();

            using (var uofw = CreateUnitOfWork())
            {
                var bserv = GetQueryService<T>();

                if (bserv == null)
                    return null;

                var query = bserv.GetAll(uofw);

                if (funcFilter != null)
                    query = funcFilter(query, uofw);

                var q = query.Filter(config);

                var propertyVm = config.GetProperty(property);

                if (propertyVm == null)
                    throw new HttpException(400, "property config is null");

                q = q.Where($"it.{property} != null");

                if (propertyVm.IsPrimitive)
                {
                    return
                        Ok(
                            await q.Select($"new (it.{property})").Distinct()/*.Take(200)*/.ToListAsync());
                }
                else
                {
                    if (propertyVm.Relationship == Relationship.One)
                    {
                        var lookup = propertyVm.ViewModelConfig.LookupProperty;

                        string select = $"new (it.{property}.ID as ID, it.{property}.{lookup.Text} as {lookup.Text}";

                        if (lookup.Icon != null)
                            select += $",it.{property}.{lookup.Icon} as {lookup.Icon}";

                        if (lookup.Image != null)
                            select += $",it.{property}.{lookup.Image} as {lookup.Image}";

                        select += ")";

                        return Ok(await q.Select(select).Distinct()/*.Take(200)*/.ToListAsync());
                    }
                }

                return null;
            }
        }

        protected async Task<IHttpActionResult> GetBoPropertyForFilter_<T>(string mnemonic, string startswith, string ids = null, string extrafilter = null,
         Func<IQueryable<T>, IUnitOfWork, IQueryable<T>> funcFilter = null)
         where T : BaseObject
        {
            //validation
            var config = GetConfig();

            using (var uofw = this.CreateUnitOfWork())
            {
                var bserv = GetQueryService<T>();

                var query = bserv.GetAll(uofw);

                if (query == null) return null;

                if (funcFilter != null)
                    query = funcFilter(query, uofw);

                var q = query.Filter(config, extrafilter);

                string property = config.LookupPropertyForFilter;

                IList qIDs = new List<BaseObject>();

                if (!string.IsNullOrEmpty(ids))
                {
                    var arrIDs = ids.Split(';').Select(int.Parse).ToArray();

                    qIDs = await bserv.GetAll(uofw, hidden: null)
                        .Where("@0.Contains(ID)", arrIDs)
                        .Select(config.ListView, new[] { property })
                        .ToListAsync();

                    q = q.Where("!@0.Contains(ID)", arrIDs);
                }

                if (!string.IsNullOrEmpty(startswith))
                {
                    startswith = startswith.Trim();
                    var listSearchWords = startswith.Split(' ');

                    startswith = "";

                    foreach (string word in listSearchWords.Take(3))
                    {
                        startswith += $" it.{property}.Contains(\"{word.Sanitize()}\") &&";
                    }

                    startswith = startswith.Substring(0, startswith.Length - 2);

                    q = q.Where(startswith);

                    //sanitization
                    var list = await q.Select(config.ListView, new[] { property })/*.Take(20)*/.ToListAsync();

                    return Ok(list.Cast<object>().Concat(qIDs.Cast<object>()).Distinct());
                }
                else
                {
                    var list = await q.Select(config.ListView, new[] { property })/*.Take(20)*/.ToListAsync();

                    return Ok(list.Cast<object>().Concat(qIDs.Cast<object>()).Distinct());
                }
            }
        }

        protected void CheckProperty(string property)
        {
            var config = GetConfig();

            if (property == null)
                throw new HttpException(400, "property is null");

            var arr = property.Split('.');

            var propertyInfo = config.TypeEntity.GetProperty(arr[0]);

            if (propertyInfo == null || arr.Length > 2)
                throw new HttpException(400, $"property [{property}] not found");

            //complex property
            if (arr.Length == 2)
            {
                if (propertyInfo.PropertyType.GetProperty(arr[1]) == null)
                    throw new HttpException(400, $"property [{property}] not found");
            }
        }
    }
}
