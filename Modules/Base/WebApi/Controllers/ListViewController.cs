using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.Service;
using Base.UI;
using Base.UI.Filter;
using Base.UI.Service.Abstract;
using Base.Utils.Common;
using Base.Utils.Common.Caching;
using Kendo.Mvc.UI;
using WebApi.Attributes;
using Base.Extensions;
using System.Web.Http.ModelBinding;
using WebMvc = System.Web.Mvc;
using WebApi.Extensions;
using WebApi.Models.ListView;
using System.Linq.Dynamic;
using System.Reflection;
using Base.UI.Extensions;
using Base.UI.Service;
using CorpProp.Services.Response;
using CorpProp.Entities.ProjectActivity;
using Base.Task.Entities;
using WebApi.Models.Task;
using Base.Task.Services.Abstract;
using Base.UI.ViewModal;
using CorpProp.Entities.Base;
using Kendo.Mvc;
using WebApi.Helper;
using Base.Service.Log;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("listview/{mnemonic}")]
    internal class ListViewController : BaseListViewController
    {
        private readonly IUiFasade _uiFasade;
        private readonly IBaseObjectService<SibTask> _taskService;
        private readonly IBaseObjectService<SibTaskGanttDependency> _taskDependencyService;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly ILogService _logger;

        public ListViewController(
            IViewModelConfigService viewModelConfigService
            , IUnitOfWorkFactory unitOfWorkFactory
            , IMnemonicFilterService<MnemonicFilter> mnemonicFilterService
            , ISimpleCacheWrapper simpleCache
            , IUiFasade uiFasade
            , IBaseObjectService<SibTask> taskService
            , CorpProp.Services.ProjectActivity.SibTaskGanttDependencyService taskDependencyService
            , ILogService logger)
            : base(viewModelConfigService, unitOfWorkFactory, mnemonicFilterService, simpleCache, logger)
        {
            _logger = logger;
            _uiFasade = uiFasade;
            _taskService = taskService;
            _taskDependencyService = taskDependencyService;
            _viewModelConfigService = viewModelConfigService;
        }

        [WebMvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGrid")]
        public async Task<IHttpActionResult> Grid_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [FromUri]string[] columns = null)
            where T : BaseObject
        {
            try
            {
                //13600 для DictObject обеспечить сортировку по по полю указанному в LookupProperty (или Name)
                if (typeof(DictObject).IsAssignableFrom(typeof(T)))
                {
                    var sorpProperty = GetConfig(mnemonic)?.LookupProperty?.Text;
                    if (string.IsNullOrEmpty(sorpProperty))
                    {
                        sorpProperty = nameof(DictObject.Name);
                    }
                    if (request.Sorts.Count == 0)
                    {
                        request.Sorts.Add(new SortDescriptor(sorpProperty, ListSortDirection.Ascending));
                    }
                }
                using (var uofw = CreateUnitOfWork())
                {
                    var query = await Read<T>(uofw, lvParams);
                    query = await AddFilter(uofw, query, lvParams, columns);

                    if (IsChildMnemonicsExist)
                    {
                        var joinedQuery = JoinMnemonicsHelper.LeftJoin2(query, uofw, GetConfig());
                        joinedQuery = await AddFilter(uofw, joinedQuery, lvParams);
                        return await ToResultAsync(joinedQuery, request, columns);
                    }
                    return await ToResultAsync(query, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpPost]
        [GenericAction("mnemonic")]
        [Route("kendoGrid/changeSortOrder/{id}/{posId}")]
        public IHttpActionResult Grid_ChangeSortOrder<T>(string mnemonic, int id, int posId)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateTransactionUnitOfWork())
                {
                    var serv = GetBaseObjectService<T>();

                    var obj = serv.Get(uofw, id);

                    serv.ChangeSortOrder(uofw, obj, posId);

                    return Ok(new
                    {
                        error = 0,
                        model = obj
                    });
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

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGrid/categorized")]
        public async Task<IHttpActionResult> Grid_CategorizedItemRead<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            int? categoryId = 0, bool allItems = false,
            [FromUri]string[] columns = null)
            where T : ICategorizedItem
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = await CategorizedItemRead<T>(uofw, lvParams, categoryId ?? 0, allItems);

                    return await ToResultAsync(q, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [Route("mytasks")]
        public async Task<IHttpActionResult> GetMyTasks([FromUri]int maxcount = -1)
        {
            using (var uofw = CreateUnitOfWork())
            {
                var res = _taskService.GetAll(uofw)
                    .Where(x => (x.Initiator != null && x.Responsible != null)
                            && (x.Initiator.UserID == Base.Ambient.AppContext.SecurityUser.ID || x.Responsible.UserID == Base.Ambient.AppContext.SecurityUser.ID))
                        .Where(x => x.SibStatus != null && (x.SibStatus.Code.ToLower() != "draft" && x.SibStatus.Code.ToLower() != "completed"));

                if (maxcount > 0)
                    res = res.Take(maxcount);

                res = res.OrderByDescending(x => x.Period.Start);

                return Ok(await res.Select(s => new
                {
                    ID = s.ID,
                    Title = s.Title,
                    Status = s.SibStatus,
                    Period = s.Period,
                    AssignedFrom = new
                    {
                        ID = s.Initiator.ID,
                        FullName = s.Initiator.FullName
                    }
                }).ToListAsync());
            }
        }

        //TODO найти замену ExtraID во всех блоках Gantt
        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGanttDependency")]
        public async Task<IHttpActionResult> GanttDependency_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            DateTime? start = null, DateTime? end = null)
            where T : IGanttDependency, IBaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = await Read<T>(uofw, lvParams);
                    var e = await ToResultAsync(q, request);

                    return e;
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
        public async Task<IHttpActionResult> Gantt_Read(string mnemonic, [ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            [FromUri]DateTime start,
            [FromUri]DateTime end)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    //var q = uofw.GetRepository<BaseTask>().All();
                    var q = _taskService.GetAll(uofw);

                    if (associationParams != null)
                        q = q.Where(x => x.BaseTaskCategory != null && x.BaseTaskCategory.ID == associationParams.Id && !x.IsTemplate && !x.Hidden);

                    var categories = q
                        .Where(x => x.BaseTaskCategory != null)
                        .GroupBy(x => x.BaseTaskCategory)
                        .Select(s => new TaskGanttModel
                        {
                            ID = "SibProjectMenuList-" + s.Key.ID,
                            ParentId = (string)null,
                            Start = s.Min(w => w.Period.Start),
                            End = s.Max(w => w.Period.End),
                            OrderId = s.Key.SortOrder,
                            Summary = true,
                            Title = s.Key.Title,
                            Expanded = false,
                            Priority = -1,
                            PercentComplete = s.Sum(w => w.PercentComplete) / s.Count()
                        });

                    if (!string.IsNullOrEmpty(lvParams?.SearchStr))
                        categories = categories.Where(x => x.Title.Trim()
                            .ToLower()
                            .Contains(lvParams.SearchStr.Trim().ToLower()));

                    categories = categories.Where(x => x.Start >= start && x.End <= end);

                    return Ok(await categories.ToListAsync());
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGantt/parent/{id}")]
        public async Task<IHttpActionResult> Gantt_Read_Child(string mnemonic, int id)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    IQueryable<TaskGanttModel> tasks = null;

                    var config = GetConfig();

                    var stringId = mnemonic + "-" + id;

                    if (typeof(BaseTaskCategory).IsAssignableFrom(config.TypeEntity))
                    {
                        var q = _taskService.GetAll(uofw)
                            .Where(x => x.CategoryID == id && x.Parent_ == null && !x.IsTemplate && !x.Hidden)
                            .Select(s => new TaskGanttModel
                            {
                                ID = "SibTaskMenuList" + "-" + s.ID,
                                ParentId = stringId,
                                Start = s.Period.Start,
                                End = s.Period.End,
                                OrderId = s.SortOrder,
                                Summary = s.Children_.Any(),
                                Title = s.Name,
                                Expanded = false,
                                Priority = (int)s.Priority,
                                PercentComplete = s.PercentComplete,
                                Color = s.Color,
                                Status = s.SibStatus != null ? s.SibStatus.Name : ""
                            });

                        tasks = q;
                    }
                    else
                    {
                        if (typeof(BaseTask).IsAssignableFrom(config.TypeEntity))
                        {
                            var q = _taskService.GetAll(uofw)
                                .Where(x => x.ParentID == id && !x.IsTemplate && !x.Hidden)
                                .Select(s => new TaskGanttModel
                                {
                                    ID = "SibTaskMenuList" + "-" + s.ID,
                                    ParentId = stringId,
                                    Start = s.Period.Start,
                                    End = s.Period.End,
                                    OrderId = s.SortOrder,
                                    Summary = s.Children_.Any(),
                                    Title = s.Name,
                                    Expanded = false,
                                    Priority = (int)s.Priority,
                                    PercentComplete = s.PercentComplete,
                                    Color = s.Color,
                                    Status = s.SibStatus != null ? s.SibStatus.Name : ""
                                });
                            tasks = q;
                        }
                    }

                    if (tasks == null)
                        return Ok(new
                        {
                            error = "Неправильный идентификатор родительского объекта"
                        });

                    var dependenciesPred = _taskDependencyService.GetAll(uofw)
                        .Join(tasks, dependency => dependency.PredecessorID, model => model.ID,
                            (dependency, model) => dependency);

                    var dependenciesSuc = _taskDependencyService.GetAll(uofw)
                        .Join(tasks, dependency => dependency.SuccessorID, model => model.ID,
                            (dependency, model) => dependency);
                    var dep = await dependenciesPred.Union(dependenciesSuc).Select(s =>
                        new
                        {
                            s.ID,
                            s.PredecessorID,
                            s.SuccessorID,
                            s.Type
                        }
                    ).ToListAsync();

                    return Ok(new
                    {
                        tasks = await tasks.ToListAsync(),
                        dependencies = dep
                    });
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = e.ToStringWithInner()
                });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoGantt/dependencies")]
        public async Task<IHttpActionResult> Gantt_Read_Dependencies(string mnemonic, [FromUri]DateTime start, [FromUri]DateTime end, [FromUri]string searchStr = null)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var categories = _taskService.GetAll(uofw)
                        .Where(x => x.BaseTaskCategory != null)
                        .GroupBy(x => x.BaseTaskCategory)
                        .Select(s => new TaskGanttModel
                        {
                            ID = s.Key.ExtraID + "-" + s.Key.ID,
                            ParentId = (string)null,
                            Start = s.Min(w => w.Period.Start),
                            End = s.Max(w => w.Period.End),
                            OrderId = s.Key.SortOrder,
                            Summary = true,
                            Title = s.Key.Title,
                            Expanded = false
                        });

                    if (!string.IsNullOrEmpty(searchStr))
                        categories = categories.Where(x => x.Title.Trim()
                            .ToLower()
                            .Contains(searchStr.Trim().ToLower()));

                    categories = categories.Where(x => x.Start >= start && x.End <= end);

                    var dependenciesPred = _taskDependencyService.GetAll(uofw)
                        .Join(categories, dependency => dependency.PredecessorID, model => model.ID,
                            (dependency, model) => dependency);

                    var dependenciesSuc = _taskDependencyService.GetAll(uofw)
                        .Join(categories, dependency => dependency.SuccessorID, model => model.ID,
                            (dependency, model) => dependency);

                    return Ok(await dependenciesSuc.Union(dependenciesPred).ToListAsync());
                }
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
        [Route("kendoGantt/dependencies")]
        public IHttpActionResult CreateGanttDependency([FromBody]BaseTaskDependency model)
        {
            if (model == null)
                return Ok(new
                {
                    error = "Пустой объект"
                });

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    //на сервере Build TFS версия build tools ниже нашей,
                    //синтаксис int.TryParse(model.PredecessorID.Split('-')[1], out int predecessorTaskID);
                    //не поддерживается
                    int predecessorTaskID = 0;
                    int successorTaskID = 0;

                    string pr = model.PredecessorID.Split('-')[1];
                    string sus = model.SuccessorID.Split('-')[1];

                    int.TryParse(pr, out predecessorTaskID);
                    int.TryParse(sus, out successorTaskID);

                    if (successorTaskID != 0 && predecessorTaskID != 0)
                    {
                        SibTask predecessorTask = uofw.GetRepository<SibTask>().Find(f => f.ID == predecessorTaskID);
                        SibTask successorTask = uofw.GetRepository<SibTask>().Find(f => f.ID == successorTaskID);

                        SibTaskGanttDependency dependencyModel = new SibTaskGanttDependency()
                        {
                            ID = model.ID,
                            PredecessorTaskID = predecessorTask.ID,
                            SuccessorTaskID = successorTask.ID,
                            Type = model.Type,
                            PredecessorID = model.PredecessorID,
                            SuccessorID = model.SuccessorID
                        };
                        model = _taskDependencyService.Create(uofw, dependencyModel);

                        return Ok(model);
                    }
                    else
                        throw new Exception("error");
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = e.ToStringWithInner()
                });
            }
        }

        [HttpDelete]
        [Route("kendoGantt/dependencies")]
        public IHttpActionResult DeleteGanttDependency([FromBody]BaseTaskDependency model)
        {
            if (model == null)
                return Ok(new
                {
                    error = "Пустой объект"
                });

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var obj = _taskDependencyService.Get(uofw, model.ID);

                    if (obj == null)
                        return Ok(new
                        {
                            error = "Объект для удаления не найден"
                        });

                    _taskDependencyService.Delete(uofw, obj);

                    return Ok(new[] { model });
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = e.ToStringWithInner()
                });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoScheduler")]
        public async Task<IHttpActionResult> Scheduler_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            DateTime? start = null, DateTime? end = null)
            where T : Base.IScheduler, IBaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    if (mnemonic == "SibTaskScheduler")
                    {
                        var q = _taskService.GetAll(uofw)
                        .Where(x => (x.Initiator != null && x.Responsible != null)
                            && (x.Initiator.UserID == Base.Ambient.AppContext.SecurityUser.ID || x.Responsible.UserID == Base.Ambient.AppContext.SecurityUser.ID))
                        .Where(x => x.SibStatus != null && (x.SibStatus.Code.ToLower() != "draft" && x.SibStatus.Code.ToLower() != "completed"));

                        return await ToResultAsync(q.AddDateFilter(start, end), request);
                    }
                    else
                    {
                        var q = await Read<T>(uofw, lvParams);

                        return await ToResultAsync(q.AddDateFilter(start, end), request);
                    }
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
            int categoryId = 0, bool allItems = false,
            DateTime? start = null, DateTime? end = null)
            where T : Base.IScheduler, ICategorizedItem
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = await CategorizedItemRead<T>(uofw, lvParams, categoryId, allItems);

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
        [Route("kendoTreeList")]
        public async Task<IHttpActionResult> TreeList_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [FromUri]string[] columns = null)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var query = await Read<T>(uofw, lvParams);
                    query = await AddFilter(uofw, query, lvParams, columns);

                    if (IsChildMnemonicsExist)
                    {
                        var joinedQuery = JoinMnemonicsHelper.LeftJoin2(query, uofw, GetConfig());
                        joinedQuery = await AddFilter(uofw, joinedQuery, lvParams);
                        return await ToResultAsync(joinedQuery, request, columns);
                    }
                    return await ToResultAsync(query, request, columns);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("kendoTreeView")]
        public async Task<IHttpActionResult> TreeView_Read<T>(string mnemonic, int? id = null,
            [ModelBinder]ListViewParams lvParams = null)
            where T : HCategory
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var config = GetConfig();

                    var pifo = config.TypeEntity.GetProperty(config.LookupProperty.Text);

                    var serv = config.GetService<IBaseCategoryService<T>>();

                    IQueryable<T> query;

                    if (!string.IsNullOrEmpty(lvParams?.SearchStr))
                    {
                        query = serv.GetAll(uofw).FullTextSearch(lvParams.SearchStr, SimpleCache).Take(500);
                    }
                    else
                    {
                        query = id == null ? serv.GetRoots(uofw) : serv.GetChildren(uofw, (int)id);
                    }

                    query = await AddFilter(uofw, query, lvParams);

                    var list = await query.ToListAsync();

                    var parents = new Dictionary<int, int>();

                    if (string.IsNullOrEmpty(lvParams?.SearchStr))
                    {
                        var ids = list.Select(x => x.ID).ToArray();

                        parents =
                            serv.GetAll(uofw)
                                .Where(x => x.ParentID != null)
                                .Select(x => x.ParentID ?? 0)
                                .Where(x => ids.Contains(x))
                                .Distinct().ToDictionary(x => x);
                    }

                    var res = list.Select(
                        a =>
                            new
                            {
                                id = a.ID,
                                Title = pifo.GetValue(a),
                                (a as ITreeNodeImage)?.Image,
                                (a as ITreeNodeIcon)?.Icon,
                                hasChildren = parents.ContainsKey(a.ID),
                                isRoot = a.IsRoot
                            });

                    return Ok(res);
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpPost]
        [GenericAction("mnemonic")]
        [Route("kendoPivot")]
        public async Task<IHttpActionResult> Pivot_Read<T>(string mnemonic,
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [ModelBinder]ListViewParams lvParams,
            [FromUri]string[] columns = null)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = await Read<T>(uofw, lvParams);

                    var e = await ToResultAsync(q, request, columns);
                    return e;
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpPost]
        [GenericAction("mnemonic")]
        [Route("{id}/{categoryId}")]
        public IHttpActionResult ChangeCategory<T>(string mnemonic, int id, int categoryId)
            where T : ICategorizedItem
        {
            try
            {
                var serv = GetBaseCategorizedItemService<T>();

                using (var unitOfWork = CreateTransactionUnitOfWork())
                {
                    serv.ChangeCategory(unitOfWork, id, categoryId);

                    unitOfWork.Commit();

                    return Ok(new { error = 0 });
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("filter/string")]
        public async Task<IHttpActionResult> GetStrPropertyForFilter<T>(string mnemonic, string startswith, string property,
            string filter = null)
            where T : BaseObject
        {
            var isChild = ModifyWhenChild(ref property, ref mnemonic);
            var fConf = isChild ? GetConfig(mnemonic) : GetConfig();
            IResponseDynamicQueryService dynamicSrv;
            try
            {
                dynamicSrv = fConf.GetService<IResponseDynamicQueryService>();
            }
            catch (InvalidCastException)
            {
                dynamicSrv = null;
            }

            //validation
            if (dynamicSrv == null)
                CheckProperty(property, mnemonic);

            using (var uofw = this.CreateUnitOfWork())
            {
                IQueryable q;
                if (dynamicSrv != null)
                {
                    q = dynamicSrv.GetAll(uofw, false, -1).Filter(fConf, filter);
                }
                else
                {
                    q = isChild ? GetFilterQuery(uofw, filter, fConf) : GetFilterQuery<T>(uofw, filter, fConf);
                }

                if (q == null) return null;

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

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("filter/uniqueValues/{property}")]
        public async Task<IHttpActionResult> GetUniqueValuesForProperty<T>(string mnemonic, string property)
            where T : BaseObject
        {
            var fConf = GetConfig();
            IResponseDynamicQueryService dynamicSrv;
            try
            {
                dynamicSrv = fConf.GetService<IResponseDynamicQueryService>();
            }
            catch (InvalidCastException)
            {
                dynamicSrv = null;
            }

            //validation
            if (dynamicSrv == null)
                CheckProperty(property);

            var config = GetConfig();

            using (var uofw = CreateUnitOfWork())
            {
                IQueryable q;
                var objectId = -1;//TODO
                if (dynamicSrv != null)
                {
                    q = dynamicSrv.GetAll(uofw, false, objectId).Filter(fConf);
                }
                else
                {
                    var bserv = GetQueryService<T>();
                    q = bserv?.GetAll(uofw).Filter(fConf);
                }
                if (q == null)
                    return null;
                var property_vm = config.GetProperty(property);

                if (property_vm == null)
                    throw new HttpException(400, "property config is null");

                q = q.Where($"it.{property} != null");

                if (property_vm.IsPrimitive)
                {
                    return
                        Ok(
                            await q.Select($"new (it.{property})").Distinct().Take(200).ToListAsync());
                }
                else
                {
                    if (property_vm.Relationship == Relationship.One)
                    {
                        var lookup = property_vm.ViewModelConfig.LookupProperty;

                        string select = $"new (it.{property}.ID as ID, it.{property}.{lookup.Text} as {lookup.Text}";

                        if (lookup.Icon != null)
                            select += $",it.{property}.{lookup.Icon} as {lookup.Icon}";

                        if (lookup.Image != null)
                            select += $",it.{property}.{lookup.Image} as {lookup.Image}";

                        select += ")";

                        return Ok(await q.Select(select).Distinct().Take(200).ToListAsync());
                    }
                }

                return null;
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("filter/boProperty")]
        public async Task<IHttpActionResult> GetBoPropertyForFilter<T>(string mnemonic
            , [FromUri]string startswith
            , [FromUri]string ids = null
            , [FromUri]string extrafilter = null
            , [FromUri]string customParams = null)
            where T : BaseObject
        {
            var fConf = GetConfig();
            IResponseDynamicQueryService dynamicSrv;
            try
            {
                dynamicSrv = fConf.GetService<IResponseDynamicQueryService>();
            }
            catch (InvalidCastException)
            {
                dynamicSrv = null;
            }

            //validation
            var config = GetConfig();

            using (var uofw = this.CreateUnitOfWork())
            {
                IQueryable q;
                var objectId = -1;//TODO
                var bserv = GetQueryService<T>();
                if (bserv is CorpProp.Common.ICustomDS)
                {
                    q = ((CorpProp.Common.ICustomDataSource<T>)bserv).GetAllCustom(uofw, customParams);
                }
                else if (dynamicSrv != null)
                {
                    q = dynamicSrv.GetAll(uofw, false, objectId).Filter(fConf, extrafilter);
                }
                else
                {
                    q = bserv?.GetAll(uofw).Filter(fConf, extrafilter);
                }

                if (typeof(DictObject).IsAssignableFrom(typeof(T)))
                {
                    var sorpProperty = GetConfig(mnemonic)?.LookupProperty?.Text;
                    q = q.OrderBy(sorpProperty);
                }

                if (q == null) return null;

                string property = config.LookupPropertyForFilter;

                IList qIDs = new List<BaseObject>();

                if (!string.IsNullOrEmpty(ids))
                {
                    var arrIDs = ids.Split(';').Select(int.Parse).ToArray();

                    qIDs = await (dynamicSrv != null ? dynamicSrv.GetAll(uofw, false, objectId) : bserv.GetAll(uofw, hidden: null))
                        .Where("@0.Contains(ID)", arrIDs)
                        .Select(config.ListView, new string[] { property })
                        .ToListAsync();

                    q = q.Where("!@0.Contains(ID)", arrIDs);
                }

                if (!string.IsNullOrEmpty(startswith))
                {
                    startswith = startswith.Trim();
                    var listSearchWords = startswith.Split(new char[] { ' ' });

                    startswith = "";

                    foreach (string word in listSearchWords.Take(3))
                    {
                        startswith += $" it.{property}.Contains(\"{word.Sanitize()}\") &&";
                    }

                    startswith = startswith.Substring(0, startswith.Length - 2);

                    q = q.Where(startswith);
                }

                //sanitization
                var list = await q.Select(config.ListView, new[] { property }).Take(20).ToListAsync();
                return Ok(list.Cast<object>().Concat(qIDs.Cast<object>()).Distinct());
            }
        }

        [HttpGet]
        [Route("filter/baseObject")]
        [GenericAction("mnemonic")]
        public async Task<IHttpActionResult> FilterBaseObject_Read<T>(string mnemonic, [FromUri]string startswith, [FromUri]string property)
            where T : BaseObject
        {
            var config = GetConfig();

            using (var uofw = this.CreateUnitOfWork())
            {
                property = property.Split('.')[0];

                var bserv = GetQueryService<T>();
                IResponseDynamicQueryService dynamicSrv;
                var objectId = -1; //TODO
                try
                {
                    dynamicSrv = config.GetService<IResponseDynamicQueryService>();
                }
                catch (InvalidCastException)
                {
                    dynamicSrv = null;
                }

                startswith = startswith?.Trim();

                var col =
                    _uiFasade.GetColumns(mnemonic)
                        .FirstOrDefault(x => x.PropertyName == property);

                if (col == null)
                    throw new HttpException(400, $"property [{property}] not found");

                string lookupProperty = col.ViewModelConfig.LookupProperty.Text;

                if (col.PropertyType.IsBaseCollection())
                {
                    var q =
                        dynamicSrv != null ? dynamicSrv.GetAll(uofw, false, objectId) : bserv.GetAll(uofw)
                            .Filter(config)
                            .Where("it." + property + ".Any()")
                            .SelectMany("it." + property);

                    if (typeof(IEasyCollectionEntry).IsAssignableFrom(col.PropertyType.GetGenericArguments()[0]))
                    {
                        q = q.Select("it.Object");
                    }

                    if (!string.IsNullOrEmpty(startswith))
                        //sanitization
                        q = q.Where($"it.{lookupProperty}.Contains(\"{startswith.Sanitize()}\")");

                    return
                        Ok(
                            await q.Select(col.ViewModelConfig.ListView, new[] { lookupProperty })
                                .Distinct()
                                .OrderBy("it." + lookupProperty)
                                .Take(20)
                                .ToListAsync());
                }
                else
                {
                    var q =
                        dynamicSrv != null ? dynamicSrv.GetAll(uofw, false, objectId) : bserv.GetAll(uofw)
                            .Filter(config)
                            .Where("it." + property + " != null")
                            .Select("it." + property);

                    if (!string.IsNullOrEmpty(startswith))
                        //sanitization
                        q = q.Where($"it.{lookupProperty}.Contains(\"{startswith.Sanitize()}\")", startswith);

                    return
                        Ok(
                            await q.Select(col.ViewModelConfig.ListView, new[] { lookupProperty })
                                .Distinct()
                                .OrderBy("it." + lookupProperty)
                                .Take(20)
                                .ToListAsync());
                }
            }
        }

        private void CheckProperty(string property, string mnemonic = null)
        {
            var config = GetConfig(mnemonic);

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

        private IQueryable GetFilterQuery(IUnitOfWork uofw, string filter, ViewModelConfig fConfig)
        {
            var getMethod = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(info => info.IsGenericMethod && info.Name == nameof(GetFilterQuery));

            if (getMethod == null)
            {
                throw new Exception($"Не найден метод {nameof(GetFilterQuery)} в классе {GetType().GetTypeName()}");
            }

            var genericGetMethod = getMethod.MakeGenericMethod(fConfig.TypeEntity);

            var q = genericGetMethod.Invoke(this,
                new object[]
                {
                    uofw,
                    filter,
                    fConfig,
                }) as IQueryable;
            return q;
        }

        private IQueryable GetFilterQuery<T>(IUnitOfWork uofw, string filter, ViewModelConfig fConf)
            where T : BaseObject
        {
            IQueryable q;
            var bserv = GetQueryService<T>(fConf.Mnemonic);

            q = bserv?.GetAll(uofw).Filter(fConf, filter);
            return q;
        }

        /// <summary>
        /// Корректировка своиства и мнемоники, если своиство дочернего объекта.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="mnemonic"></param>
        /// <returns>Является ли своиство дочерним</returns>
        private bool ModifyWhenChild(ref string property, ref string mnemonic)
        {
            var childSplit = property.Split('_');
            if (childSplit.Length > 1)
            {
                var childMnemonic = childSplit[0];
                var childProperty = childSplit[1];
                var prop = property;
                var isChildProperty = GetConfig().ListView.Columns.Any(model =>
                    model.ChildMnemonic == childMnemonic && model.PropertyName == prop);
                if (isChildProperty)
                {
                    mnemonic = childMnemonic;
                    property = childProperty;
                    return true;
                }
            }

            return false;
        }
    }
}