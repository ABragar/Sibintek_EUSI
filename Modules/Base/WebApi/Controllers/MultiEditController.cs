using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Base;
using Base.DAL;
using Base.Extensions;
using Base.Service.Log;
using Base.UI;
using Base.UI.Extensions;
using Base.UI.Filter;
using Base.UI.Service.Abstract;
using Base.Utils.Common.Caching;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebApi.Attributes;
using WebApi.Helper;
using WebApi.Models.ListView;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("multi_edit/{mnemonic}")]
    internal class MultiEditController : BaseListViewController
    {
        private readonly ILogService _logger;
        public MultiEditController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService, ISimpleCacheWrapper simpleCache, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, mnemonicFilterService, simpleCache, logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("")]
        public async Task<IHttpActionResult> Get<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            ListViewParams lvParams,
            [FromUri]string[] columns = null)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = await Read<T>(uofw, lvParams);
                    if (IsChildMnemonicsExist)
                    {
                        var joinedQuery = JoinMnemonicsHelper.LeftJoin2(q, uofw, GetConfig());
                        joinedQuery = await AddFilter(uofw, joinedQuery, lvParams);
                        return await ToResult(joinedQuery, request, columns);
                    }
                    q = await AddFilter(uofw, q, lvParams);
                    return await ToResult(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("categorized")]
        public async Task<IHttpActionResult> GetCategorizedItems<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            ListViewParams lvParams,
            int categoryId = 0, bool allItems = false,
            [FromUri]string[] columns = null)
            where T : ICategorizedItem
        {

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems);

                    return await ToResult(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        private async Task<IHttpActionResult> ToResult(IQueryable q, DataSourceRequest request, string[] columns)
        {
            var res = q.Select(GetConfig().ListView, columns);

            if (request.Sorts != null)
            {
                res = res.Sort(request.Sorts);
            }

            if (request.Filters != null)
            {
                res = res.Where(request.Filters);
            }

            res = res.Select("new (it.ID as ID)");

            return Ok(new DataSourceResult()
            {
                Data = await res.ToListAsync(),
                Total = await res.CountAsync()
            });
        }
    }
}
