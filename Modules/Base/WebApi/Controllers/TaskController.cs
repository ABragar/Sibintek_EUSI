using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.Service.Log;
using Base.Task.Entities;
using Base.Task.Services;
using Base.Task.Services.Abstract;
using Base.UI;
using Base.UI.Editors.OneToManyExtensions;
using Base.UI.Extensions;
using Base.Utils.Common;
using WebApi.Attributes;
using WebApi.Models.ListView;
using WebApi.Models.Task;
using SystemTask = System.Threading.Tasks;

namespace WebApi.Controllers
{
    [RoutePrefix("task")]
    public class TaskController : BaseApiController
    {
        private readonly IBaseTaskService<BaseTask> _taskService;
        private readonly IBaseObjectService<BaseTaskDependency> _baseTaskDependencyService;
        private readonly IBaseTaskCategoryService _baseTaskCategoryService;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly ILogService _logger;

        public TaskController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IBaseTaskService<BaseTask> taskService, IBaseObjectService<BaseTaskDependency> baseTaskDependencyService, IBaseTaskCategoryService baseTaskCategoryService, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _taskService = taskService;
            _viewModelConfigService = viewModelConfigService;
            _baseTaskDependencyService = baseTaskDependencyService;
            _baseTaskCategoryService = baseTaskCategoryService;
        }

        [Route("mytasks")]
        [HttpGet]
        public async SystemTask.Task<IHttpActionResult> GetMyTasks([FromUri]int maxcount = -1)
        {
            using (var uofw = CreateUnitOfWork())
            {
                var res = _taskService.GetAll(uofw)
                    .Where(x => x.AssignedToID == Base.Ambient.AppContext.SecurityUser.ID)
                    .Where(x => x.Status == TaskStatus.New || x.Status == TaskStatus.Viewed ||
                                x.Status == TaskStatus.InProcess || x.Status == TaskStatus.Rework);

                if (maxcount > 0)
                    res = res.Take(maxcount);

                res = res.OrderByDescending(x => x.Period.Start);

                return Ok(await res.Select(s => new
                {
                    ID = s.ID,
                    Title = s.Title,
                    Status = s.Status,
                    Period = s.Period,
                    AssignedFrom = new
                    {
                        ID = s.AssignedFrom.ID,
                        Image = s.AssignedFrom.Image,
                        FullName = s.AssignedFrom.FullName
                    }
                }).ToListAsync());
            }
        }

        [HttpGet]
        [Route("gantt/categories")]
        public async SystemTask.Task<IHttpActionResult> GetGanttCategory([ModelBinder]ListViewParams lvParams,
            [ModelBinder(Name = "associationParams")]AssociationParams associationParams,
            [FromUri]DateTime start, 
            [FromUri]DateTime end)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var q = _taskService.GetAll(uofw);

                    if (associationParams != null)
                        q = q.Where(x => x.BaseTaskCategory != null && x.BaseTaskCategory.ID == associationParams.Id);

                    var categories = q
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
                return Ok(new
                {
                    error = e.ToStringWithInner()
                });
            }
        }

        [HttpGet]
        [Route("gantt/tasks/{mnemonic}/{id}")]
        public async SystemTask.Task<IHttpActionResult> GetGanttChildTasks(string mnemonic, int id)
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
                            .Where(x => x.CategoryID == id && x.Parent_ == null)
                            .Select(s => new TaskGanttModel
                            {
                                ID = s.ExtraID + "-" + s.ID,
                                ParentId = stringId,
                                Start = s.Period.Start,
                                End = s.Period.End,
                                OrderId = s.SortOrder,
                                Summary = s.Children_.Any(),
                                Title = s.Name,
                                Expanded = false,
                                Priority = (int)s.Priority,
                                PercentComplete = s.PercentComplete,
                                Color = s.Color
                            });

                        tasks = q;
                    }
                    else
                    {
                        if (typeof(BaseTask).IsAssignableFrom(config.TypeEntity))
                        {
                            var q = _taskService.GetAll(uofw)
                                .Where(x => x.ParentID == id)
                                .Select(s => new TaskGanttModel
                                {
                                    ID = s.ExtraID + "-" + s.ID,
                                    ParentId = stringId,
                                    Start = s.Period.Start,
                                    End = s.Period.End,
                                    OrderId = s.SortOrder,
                                    Summary = s.Children_.Any(),
                                    Title = s.Name,
                                    Expanded = false,
                                    Priority = (int)s.Priority,
                                    PercentComplete = s.PercentComplete,
                                    Color = s.Color
                                });
                            tasks = q;
                        }
                    }

                    if (tasks == null)
                        return Ok(new
                        {
                            error = "Неправильный идентификатор родительского объекта"
                        });

                    var dependenciesPred = _baseTaskDependencyService.GetAll(uofw)
                        .Join(tasks, dependency => dependency.PredecessorID, model => model.ID,
                            (dependency, model) => dependency);

                    var dependenciesSuc = _baseTaskDependencyService.GetAll(uofw)
                        .Join(tasks, dependency => dependency.SuccessorID, model => model.ID,
                            (dependency, model) => dependency);

                    return Ok(new
                    {
                        tasks = await tasks.ToListAsync(),
                        dependencies = await dependenciesPred.Union(dependenciesSuc).ToListAsync()
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

        [HttpPost]
        [Route("gantt/tasks/{mnemonic}/{id}")]
        //TODO: реализовать обновление задачи
        public IHttpActionResult GanttTaskUpdate(string mnemonic, int id)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("gantt/tasks/{mnemonic}/{id}")]
        public IHttpActionResult GanttTaskDestroy(string mnemonic, int id)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var config = GetConfig();

                    var stringId = mnemonic + "-" + id;

                    if (typeof(BaseTaskCategory).IsAssignableFrom(config.TypeEntity))
                    {
                        var category = _baseTaskCategoryService.Get(uofw, id);
                        _baseTaskCategoryService.Delete(uofw, category);
                    }
                    else
                    {
                        if (typeof(BaseTask).IsAssignableFrom(config.TypeEntity))
                        {
                            var task = _taskService.Get(uofw, id);
                            _taskService.Delete(uofw, task);
                        }
                    }

                    var dependencies = _baseTaskDependencyService.GetAll(uofw)
                        .Where(x => x.PredecessorID == stringId || x.SuccessorID == stringId);

                    _baseTaskDependencyService.DeleteCollection(uofw, dependencies.ToList());

                    return Ok();
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
        [Route("gantt/dependencies")]
        public async SystemTask.Task<IHttpActionResult> GetGanttCategoryDependencies([FromUri]DateTime start, [FromUri]DateTime end, [FromUri]string searchStr = null)
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

                    var dependenciesPred = _baseTaskDependencyService.GetAll(uofw)
                        .Join(categories, dependency => dependency.PredecessorID, model => model.ID,
                            (dependency, model) => dependency);

                    var dependenciesSuc = _baseTaskDependencyService.GetAll(uofw)
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
        [Route("gantt/dependencies")]
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
                    model = _baseTaskDependencyService.Create(uofw, model);

                    return Ok(model);
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
        [Route("gantt/dependencies")]
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
                    var obj = _baseTaskDependencyService.Get(uofw, model.ID);

                    if (obj == null)
                        return Ok(new
                        {
                            error = "Объект для удаления не найден"
                        });

                    _baseTaskDependencyService.Delete(uofw, obj);

                    return Ok(new [] { model });
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
    }
}