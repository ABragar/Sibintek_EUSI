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
using Base.Service.Crud;
using Base.UI.Editors;
using WebApi.Extensions;
using WebApi.Models.ListView;
using System.Linq.Dynamic;
using Base.Utils.Common;
using Base.UI.Editors.ManyToManyExtensions;
using Base.Service.Log;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("manyToManyAssociation/{mnemonic}")]
    public class ManyToManyAssociationController : BaseListViewController
    {
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly ILogService _logger;

        public ManyToManyAssociationController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService, ISimpleCacheWrapper simpleCache, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, mnemonicFilterService, simpleCache, logger)
        {
            _viewModelConfigService = viewModelConfigService;
            _logger = logger;
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
                    var q = associationParams.AddFilter(await Read<T>(uofw, lvParams), uofw, _viewModelConfigService);

                    return await ToResultAsync(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.ToStringWithInner() });
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
                    var q = associationParams.AddFilter(await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems), uofw, _viewModelConfigService);

                    return await ToResultAsync(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.ToStringWithInner() });
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
                    var q = associationParams.AddFilter(await Read<T>(uofw, lvParams), uofw, _viewModelConfigService);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.ToStringWithInner() });
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
                    var q = associationParams.AddFilter(await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems), uofw, _viewModelConfigService);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.ToStringWithInner() });
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
                    var q = associationParams.AddFilter(await Read<T>(uofw, lvParams), uofw, _viewModelConfigService);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.ToStringWithInner() });
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
                    var q = associationParams.AddFilter(await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems), uofw, _viewModelConfigService);

                    return await ToResultAsync(q.AddDateFilter(start, end), request);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.ToStringWithInner() });
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
                if (associationParams == null)
                    throw new Exception("AssociationParams is null");

                if (ids == null)
                    throw new Exception("ids is null");

                var manyToManyEditor = GetEditor(associationParams);
                var junctionConfig = _viewModelConfigService.Get(manyToManyEditor.ManyToManyType);
                var junctionService = junctionConfig.GetService<IBaseObjectCrudService>();

                foreach (var id in ids)
                {
                    using (var unitOfWork = CreateUnitOfWork())
                    {
                        int leftId;
                        int rigthId;

                        if (manyToManyEditor.AssociationType == ManyToManyAssociationType.Left)
                        {
                            leftId = associationParams.Id;
                            rigthId = id;
                        }
                        else
                        {
                            leftId = id;
                            rigthId = associationParams.Id;
                        }

                        BaseObject obj = junctionService.GetAll(unitOfWork, hidden: null)
                            .Where($"ObjLeftId == @0 && ObjRigthId == @1", leftId, rigthId).SingleOrDefault();

                        if (obj == null)
                        {
                            obj = junctionService.CreateDefault(unitOfWork);

                            obj.GetType().GetProperty("ObjLeftId")?.SetValue(obj, leftId);
                            obj.GetType().GetProperty("ObjRigthId")?.SetValue(obj, rigthId);

                            junctionService.Create(unitOfWork, obj);
                        }
                        else
                        {
                            if (obj.Hidden)
                            {
                                obj.Hidden = false;
                                junctionService.Update(unitOfWork, obj);
                            }
                        }


                    }
                }

                return Ok(new { });

            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = e.ToStringWithInner()
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
                if (associationParams == null)
                    throw new Exception("AssociationParams is null");

                if (ids == null)
                    throw new Exception("ids is null");

                var manyToManyEditor = GetEditor(associationParams);
                var junctionConfig = _viewModelConfigService.Get(manyToManyEditor.ManyToManyType);
                var junctionService = junctionConfig.GetService<IBaseObjectCrudService>();

                foreach (var id in ids)
                {
                    using (var unitOfWork = CreateUnitOfWork())
                    {

                        int leftId;
                        int rigthId;

                        if (manyToManyEditor.AssociationType == ManyToManyAssociationType.Left)
                        {
                            leftId = associationParams.Id;
                            rigthId = id;
                        }
                        else
                        {
                            leftId = id;
                            rigthId = associationParams.Id;
                        }

                        var obj = junctionService.GetAll(unitOfWork)
                            .Where($"ObjLeftId == @0 && ObjRigthId == @1", leftId, rigthId).SingleOrDefault();

                        if (obj != null)
                            junctionService.Delete(unitOfWork, (BaseObject)obj);
                    }
                }

                return Ok(new { });

            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = e.ToStringWithInner()
                });
            }
        }

        private ManyToManyAssociationEditor GetEditor(AssociationParams associationParams)
        {
            var config = _viewModelConfigService.Get(associationParams.Mnemonic);
            return (ManyToManyAssociationEditor)config.DetailView.Editors.Single(x => x.SysName == associationParams.SysName);
        }
    }
}
