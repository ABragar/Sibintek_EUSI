using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Base;
using Base.DAL;
using Base.Excel;
using Base.UI;
using Base.UI.Filter;
using Base.UI.Service.Abstract;
using Base.Utils.Common.Caching;
using Kendo.Mvc.UI;
using WebApi.Models.ListView;
using Base.UI.Extensions;
using Kendo.Mvc.Extensions;
using WebApi.Attributes;
using Base.Service.Log;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("excel/{mnemonic}")]
    internal class ExcelController : BaseListViewController
    {
        private readonly IExcelExportService _excelExportService;
        private readonly ILogService _logger;

        public ExcelController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService, ISimpleCacheWrapper simpleCache, IExcelExportService excelExportService, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, mnemonicFilterService, simpleCache, logger)
        {
            _logger = logger;
            _excelExportService = excelExportService;
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("")]
        public HttpResponseMessage Get<T>(string mnemonic, CancellationToken token,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))] DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [FromUri] string[] columns = null)
            where T : IBaseObject
        {

            return _Get(token, request, lvParams, columns,
                (uofw, lvparams) => Read<T>(uofw, lvParams));
            
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("categorized")]
        public HttpResponseMessage GetCategorizedItems<T>(string mnemonic, CancellationToken token,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))] DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            int categoryId = 0, bool allItems = false,
            [FromUri] string[] columns = null)
            where T : ICategorizedItem
        {

            return _Get(token, request, lvParams, columns,
                (uofw, lvparams) => CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems));
            
        }

        private HttpResponseMessage _Get<T>(
            CancellationToken token,
            DataSourceRequest request,
            ListViewParams lvParams,
            string[] columns,
            Func<IUnitOfWork, ListViewParams, Task<IQueryable<T>>> query)
            where T : IBaseObject
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new PushStreamContent(
                async (outputStream, httpContent, transportContext) =>
                {
                    try
                    {
                        using (var uow = CreateUnitOfWork())
                        {
                            var q = await query(uow, lvParams);

                            var select = q.Select(GetConfig().ListView, columns);

                            if (request.Sorts != null)
                            {
                                select = select.Sort(request.Sorts);
                            }

                            if (request.Filters != null)
                            {
                                select = select.Where(request.Filters);
                            }

                            _excelExportService.Export(outputStream, select, GetConfig(), columns, token);
                        }
                    }
                    finally
                    {
                        outputStream.Dispose();
                    }

                });

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = $"{GetConfig().Title}.xlsx", FileNameStar = $"{GetConfig().Title}.xlsx" };

            return response;

        }
    }
}
