using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.UI;
using Base.UI.Filter;
using Base.UI.Service.Abstract;
using Base.Utils.Common.Caching;
using Kendo.Mvc.UI;
using WebApi.Attributes;
using System.Web.Http.ModelBinding;
using Base.Service;
using Base.UI.Editors.OneToManyExtensions;
using Newtonsoft.Json.Linq;
using WebApi.Extensions;
using WebApi.Models.ListView;
using CorpProp.Helpers;
using Base.Service.Log;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("oneToManyAssociation/{mnemonic}")]
    public class OneToManyAssociationController : BaseListViewController
    {
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly ILogService _logger;

        public OneToManyAssociationController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService, ISimpleCacheWrapper simpleCache, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, mnemonicFilterService, simpleCache, logger)
        {
            _logger = logger;
            _viewModelConfigService = viewModelConfigService;
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGrid")]
        public async Task<IHttpActionResult> Grid_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            [FromUri]string[] columns = null)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var oid = System.Guid.Empty;
                    if (!String.IsNullOrEmpty(associationParams.Oid))
                        oid = System.Guid.Parse(associationParams.Oid);

                    if (!String.IsNullOrEmpty(associationParams.Date))
                        lvParams.Date = associationParams.Date;

                    var q = associationParams.AddFilter(await Read<T>(uofw, lvParams), uofw, _viewModelConfigService, oid);

                    return await ToResultAsync(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGrid/categorized")]
        public async Task<IHttpActionResult> Grid_CategorizedItemRead<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            int categoryId = 0, bool allItems = false,
            [FromUri]string[] columns = null)
            where T : ICategorizedItem
        {

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = associationParams.AddFilter(await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems), uofw, _viewModelConfigService, System.Guid.Empty);

                    return await ToResultAsync(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoTreeList")]
        public async Task<IHttpActionResult> TreeList_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            [FromUri]string[] columns = null)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = associationParams.AddFilter(await Read<T>(uofw, lvParams), uofw, _viewModelConfigService, System.Guid.Empty);

                    return await ToResultAsync(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGantt")]
        public async Task<IHttpActionResult> Gantt_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            DateTime? start = null, DateTime? end = null)
            where T : IGantt, IBaseObject
        {

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = associationParams.AddFilter(await Read<T>(uofw, lvParams), uofw, _viewModelConfigService, System.Guid.Empty);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGantt/categorized")]
        public async Task<IHttpActionResult> Gantt_CategorizedItemRead<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            int categoryId = 0, bool allItems = false,
            DateTime? start = null, DateTime? end = null)
            where T : IGantt, ICategorizedItem
        {

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = associationParams.AddFilter(await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems), uofw, _viewModelConfigService, System.Guid.Empty);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoScheduler")]
        public async Task<IHttpActionResult> Scheduler_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            DateTime? start = null, DateTime? end = null)
            where T : Base.IScheduler, IBaseObject
        {

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = associationParams.AddFilter(await Read<T>(uofw, lvParams), uofw, _viewModelConfigService, System.Guid.Empty);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoScheduler/categorized")]
        public async Task<IHttpActionResult> Scheduler_CategorizedItemRead<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            int categoryId = 0, bool allItems = false,
            DateTime? start = null, DateTime? end = null)
            where T : Base.IScheduler, ICategorizedItem
        {

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = associationParams.AddFilter(await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems), uofw, _viewModelConfigService, System.Guid.Empty);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [Route("create_default")]
        [GenericAction("mnemonic")]
        public IHttpActionResult CreateDefault<T>([FromUri]string mnemonic,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams)
            where T : BaseObject
        {

            try
            {
                var config = GetConfig();
                var service = config.GetService<IBaseObjectService<T>>();

                using (var unitOfWork = CreateUnitOfWork())
                {
                    var baseObject = service != null ? service.CreateDefault(unitOfWork) : Activator.CreateInstance(config.TypeEntity);
                    
                    associationParams.AddAssociation(baseObject, unitOfWork, _viewModelConfigService);

                    var model = config.DetailView.SelectObj(baseObject);

                    return Ok(new { model });
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = 1,
                    message = e.Message
                });
            }
        }

        [HttpPost]
        [Route("addAssociation")]
        [GenericAction("mnemonic")]
        public IHttpActionResult AddAssociation<T>([FromUri]string mnemonic,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            [FromUri]int[] ids)
            where T : BaseObject
        {
            try
            {
                var config = GetConfig();
                var service = config.GetService<IBaseObjectService<T>>();

                if(associationParams == null)
                    throw new Exception("AssociationParams is null");

                if(ids == null)
                    throw new Exception("ids is null");

                using (var unitOfWork = CreateTransactionUnitOfWork())
                {
                    var objects = new List<T>();

                    foreach (var id in ids)
                    {
                        //var serializer = CreateJsonSerializer();
                        //var jobj = JObject.FromObject(config.DetailView.GetData(unitOfWork, service, id), serializer);
                        //var obj = jobj.ToObject<T>(serializer);
                        //objects.Add(obj);

                        var obj = unitOfWork.GetRepository<T>().Find(id);
                        associationParams.AddAssociation(obj, unitOfWork, _viewModelConfigService);
                        unitOfWork.SaveChanges();
                    }

                    //service.UpdateCollection(unitOfWork, objects);
                    unitOfWork.Commit();

                    return Ok(new { });
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = e.Message
                });
            }
        }

        [HttpPost]
        [Route("deleteAssociation")]
        [GenericAction("mnemonic")]
        public IHttpActionResult DeleteAssociation<T>([FromUri]string mnemonic,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            [FromUri]int[] ids)
            where T : BaseObject
        {
            try
            {
                var config = GetConfig();
                var service = config.GetService<IBaseObjectService<T>>();

                if (associationParams == null)
                    throw new Exception("AssociationParams is null");

                if (ids == null)
                    throw new Exception("ids is null");

                using (var unitOfWork = CreateTransactionUnitOfWork())
                {
                    foreach (var id in ids)
                    {
                        //var serializer = CreateJsonSerializer();
                        //var jobj = JObject.FromObject(config.DetailView.GetData(unitOfWork, service, id), serializer);
                        //var obj = jobj.ToObject<T>(serializer);
                        //objects.Add(obj);

                        var obj = unitOfWork.GetRepository<T>().Find(id);
                        associationParams.DeleteAssociation(obj, unitOfWork, _viewModelConfigService);
                        unitOfWork.SaveChanges();
                    }

                    //service.UpdateCollection(unitOfWork, objects);
                    unitOfWork.Commit();

                    return Ok(new { });
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = e.Message
                });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("filter/string")]
        public async Task<IHttpActionResult> GetStrPropertyForFilter<T>(string mnemonic, string startswith, string property,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            string filter = null)
            where T : BaseObject
        {
            var oid = Guid.Parse(associationParams.Oid);
            return await GetStrPropertyForFilter_<T>(mnemonic, startswith, property, filter, (q, uofw) => associationParams.AddFilter(q, uofw, _viewModelConfigService, oid));
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("filter/uniqueValues/{property}")]
        public async Task<IHttpActionResult> GetUniqueValuesForProperty<T>(string mnemonic, string property,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams)
            where T : BaseObject
        {
            var oid = Guid.Parse(associationParams.Oid);
            return await GetUniqueValuesForProperty_<T>(mnemonic, property, (q, uofw) => associationParams.AddFilter(q, uofw, _viewModelConfigService, oid));
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("filter/boProperty")]
        public async Task<IHttpActionResult> GetBoPropertyForFilter<T>(string mnemonic, [FromUri]string startswith,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            [FromUri]string ids = null,
            [FromUri]string extrafilter = null)
            where T : BaseObject
        {
            var oid = Guid.Parse(associationParams.Oid);
            return await GetBoPropertyForFilter_<T>(mnemonic, startswith, ids, extrafilter, (q, uofw) => associationParams.AddFilter(q, uofw, _viewModelConfigService, oid));
        }
    }
}
