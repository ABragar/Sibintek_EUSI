using Base;
using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.BusinessProcesses.Services.Concrete;
using Base.DAL;
using Base.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.BusinessProcesses.Entities.Steps;
using Base.UI.ViewModal;
using WebUI.Models.BusinessProcess;
using Base.Extensions;
using Base.Macros.Entities;
using Base.Security;
using Base.Service;
using Base.Service.Crud;
using Base.Task.Services.Abstract;
using Base.Utils.Common;
using Base.Utils.Common.Maybe;
using WebUI.Helpers;
using Kendo.Mvc.UI;
using WebUI.Extensions;
using TreeView = Base.UI.ViewModal.TreeView;
using Base.UI.Extensions;
using Kendo.Mvc.Extensions;
using WebUI.Models.Base;

namespace WebUI.Controllers
{
    public class BusinessProcessesController : BaseController
    {
        private readonly IWorkflowService _workflowService;
        private readonly IWorkflowContextService _workflowContextService;
        private readonly IBaseTaskService<BPTask> _taskService;
        private readonly IWorkflowCacheService _cashService;
        private readonly IChangeHistoryService _changeHistoryService;
        private readonly IWorkflowStrategyService _strategyService;
        private readonly IStageUserService _stageUserService;

        public BusinessProcessesController(
            IBaseControllerServiceFacade baseServiceFacade,
            IWorkflowService workflowService,
            IBaseTaskService<BPTask> taskService,
            IWorkflowCacheService cashService,
            IWorkflowContextService workflowContextService,
            IChangeHistoryService changeHistoryService,
            IWorkflowStrategyService strategyService, IStageUserService stageUserService)
            : base(baseServiceFacade)
        {
            _workflowService = workflowService;
            _taskService = taskService;
            _cashService = cashService;
            _workflowContextService = workflowContextService;
            _changeHistoryService = changeHistoryService;
            _strategyService = strategyService;
            _stageUserService = stageUserService;
        }


        public ActionResult UsersList(int objectID, string objectType, int stageID)
        {
            return PartialView("_UserList", new InvokeStageVm() { ObjectID = objectID, ObjectType = objectType, StageID = stageID });
        }

        public async Task<ActionResult> GetPermittedUsers([DataSourceRequest]DataSourceRequest request, int objectID, string objectType, int stageID, string searchStr)
        {
            using (var uow = CreateUnitOfWork())
            {
                var objService = this.GetService<IBaseObjectCrudService>(objectType);
                var obj = (IBPObject)objService.Get(uow, objectID);

                var stage = uow.GetRepository<Stage>().Find(x => x.ID == stageID);

                if (stage == null)
                    throw new Exception("Этап не найден");

                var users = _stageUserService.GetStakeholders(uow, stage, obj);


                users = users.FullTextSearch(searchStr, CacheWrapper);

                return new JsonNetResult(await users.Select(x => new
                {
                    x.ID,
                    x.FullName,
                    x.SortOrder,
                    x.Hidden,
                    x.Image,

                }).OrderBy(x => x.FullName).ToDataSourceResultAsync(request));
            }
        }

        public ActionResult CanSelectPerformer(int stageID)
        {
            using (var uow = CreateUnitOfWork())
            {
                var result = _workflowContextService.CanSelectPreformer(uow, stageID);
                return new JsonNetResult(result);
            }
        }

        public ActionResult CheckNextStage(int actionID, string objectType, int objectID)
        {
            using (var uow = CreateUnitOfWork())
            {
                var intError = 0;
                var strMsg = "";

                try
                {
                    var objService = this.GetService<IBaseObjectCrudService>(objectType);
                    var obj = (IBPObject)objService.Get(uow, objectID);
                    if (obj == null) throw new Exception("Объект не найден");

                    var nextStages = _workflowService.GetNextStage(uow, obj, actionID);

                    if (nextStages.Count() == 1 && nextStages.First().Stage.IsCustomPerformer)
                    {
                        return new JsonNetResult(new { IsCustomPerformer = true, StageID = nextStages.First().Stage.ID });
                    }
                    else
                    {
                        return new JsonNetResult(new { Success = "Ok" });
                    }
                }
                catch (Exception e)
                {
                    intError = 1;

                    strMsg = $"Ошибка выполнения этапа бизнес процесса: {e.ToStringWithInner()}";
                }
                return new JsonNetResult(new { error = intError, message = strMsg });
            }
        }

        public ActionResult ExecuteAction(int actionID, int objectID, string objectType, ActionComment comment, int? userID = null)
        {
            using (var uow = CreateTransactionUnitOfWork())
            {
                var action = uow.GetRepository<StageAction>().All().FirstOrDefault(x => x.ID == actionID);
                if (action == null)
                    throw new ArgumentException("Не удалось найти действие");

                var objectService = this.GetService<IBaseObjectCrudService>(objectType);

                if (action.RequiredComment)
                {
                    if (String.IsNullOrEmpty(comment.Message))
                        return new JsonNetResult(new { error = 1, message = "Введите комментарий" });
                }
                else
                {
                    if (comment != null && String.IsNullOrEmpty(comment.Message))
                        comment.Message = "";
                }

                var intError = 0;
                var strMsg = "";

                try
                {
                    var obj = (IBPObject)objectService.Get(uow, objectID);

                    _workflowService.InvokeStage(uow, objectService, obj, action, comment, userID);

                    ClearCash(objectID, objectType);

                    uow.Commit();
                }
                catch (Exception e)
                {
                    intError = 1;

                    strMsg = e.ToStringWithInner();

                    uow.Rollback();
                }
                return new JsonNetResult(new { error = intError, message = strMsg });
            }
        }

        private ICollection<StageVM> GetCurrentStages(IUnitOfWork uow, IBPObject obj)
        {
            var currentStages = _workflowContextService.GetCurrentStages(uow, obj).Where(x => x.Stage.StepType == FlowStepType.Stage);

            return (from currentStage in currentStages
                    let stageContext = new InvokeStageContext { BPObject = obj }
                    select new StageVM(uow, currentStage)
                    {
                        PerformerType = _workflowContextService.GetPerformerType(uow, currentStage, stageContext),
                        ObjectID = obj.ID,
                        ObjectType = obj.WorkflowContext.WorkflowImplementation.Workflow.ObjectType,
                        PermittedUsers = _stageUserService.GetStakeholders(uow, currentStage.Stage, obj).ToList(),
                        CurrentUser = SecurityUser
                    }).ToList();
        }

        public ActionResult GetToolbar(string mnemonic, int objectID)
        {
            using (var uow = CreateUnitOfWork())
            {
                try
                {
                    if (objectID != 0)
                    {
                        var obj = (IBPObject)GetService<IBaseObjectCrudService>(mnemonic).Get(uow, objectID);

                        WorkflowToolbarVm toolbar = new WorkflowToolbarVm(this.SecurityUser, objectID, obj.GetType().GetBaseObjectType().GetTypeName())
                        {
                            Stages = GetCurrentStages(uow, obj)
                        };
                        return PartialView("~/Views/BusinessProcesses/_Toolbar.cshtml", toolbar);
                    }
                    return null;
                }
                catch (Exception e)
                {
                    return PartialView("_Error", new ToolbarErrorModel() { Mnemonic = mnemonic, Error = e.Message, ObjectID = objectID });
                }
            }
        }

        public ActionResult TakeForPerform(int objectID, string objectType, int? performID, int? userID)
        {
            using (var uow = CreateTransactionUnitOfWork())
            {
                var intError = 0;
                var strMsg = "";

                BaseObject result = null;

                try
                {
                    var objService = this.GetService<IBaseObjectCrudService>(objectType);
                    _workflowContextService.TakeForPerform(uow, objService, userID, performID, objectID);
                    ClearCash(objectID, objectType);
                    uow.Commit();
                }
                catch (Exception e)
                {
                    uow.Rollback();
                    intError = 1;
                    strMsg = String.Format("Ошибка выполнения этапа бизнес процесса: {0}", e.ToStringWithInner());
                }

                return new JsonNetResult(new
                {
                    error = intError,
                    message = strMsg,
                    model = result
                });

            }
        }

        private void ClearCash(int objectID, string objectType)
        {
            string key = _getCashKey(objectType, objectID, WorkflowCacheType.Toolbar);
            _cashService.Clear(key);

            key = _getCashKey(objectType, objectID, WorkflowCacheType.TimeLine);
            _cashService.Clear(key);
        }

        private string _getCashKey(string type, int objectId, WorkflowCacheType cashType)
        {
            string key = String.Format("A5EC7C63F62848DE8CE09CBF49D1B7C4-[{0}|{1}|{2}]", GetViewModelConfig(type).TypeEntity, objectId, cashType);
            return key;
        }

        public ActionResult ReleasePerform(int objectID, string objectType, int performID)
        {
            using (var uow = CreateTransactionUnitOfWork())
            {
                var intError = 0;
                var strMsg = "";

                BaseObject result = null;

                try
                {
                    var objService = this.GetService<IBaseObjectCrudService>(objectType);

                    _workflowContextService.ReleasePerform(uow, objService, performID, objectID);
                    ClearCash(objectID, objectType);
                    uow.Commit();
                }
                catch (Exception e)
                {
                    uow.Rollback();
                    intError = 1;
                    strMsg = String.Format("Ошибка выполнения этапа бизнес процесса: {0}", e.ToStringWithInner());
                }

                return new JsonNetResult(new
                {
                    error = intError,
                    message = strMsg,
                    model = result
                });

            }
        }

        public ActionResult TimeLine(string objectType, int objectID, int? implID = null, bool showCurrentStages = true)
        {
            using (var uow = CreateUnitOfWork())
            {
                try
                {
                    var config = GetViewModelConfig(objectType);
                    var serv = GetService<IBaseObjectCrudService>(config.Entity);
                    var obj = (IBPObject)serv.Get(uow, objectID);

                    var history = _changeHistoryService.GetChangeHistory(uow, obj, implID).ToList();

                    var model = new TimeLineVm();

                    model.InitCollection(history);

                    if (showCurrentStages)
                    {

                        model.ShowCurrentStages = true;
                        model.CurrnetStages = GetCurrentStages(uow, obj);
                    }

                    var partialView = this.RenderPartialViewToString("~/Views/BusinessProcesses/_TimeLine.cshtml", model, ViewData);
                    return Content(partialView);
                }
                catch (Exception e)
                {
                    return PartialView("_Error", e.ToStringWithInner());
                }
            }
        }

        public ActionResult GetBPHistory(string objectType, int objectID, int? implID = null)
        {
            using (var uow = CreateUnitOfWork())
            {
                var config = GetViewModelConfig(objectType);
                var serv = GetService<IBaseObjectCrudService>(config.Entity);
                var obj = (IBPObject)serv.Get(uow, objectID);

                var history = _changeHistoryService.GetChangeHistory(uow, obj, implID).Where(x => x.Step.StepType == FlowStepType.Stage).ToList();

                var result = history.Where(x => x.AgreementItem != null).Select(x => new AgreementItemVm()
                {
                    User = new UserVm()
                    {
                        Image = x.AgreementItem.User.Image,
                        ID = x.AgreementItem.User.ID,
                        FullName = x.AgreementItem.User.FullName
                    },
                    Comment = x.AgreementItem.Comment,
                    ShortDate = x.AgreementItem.Date.Value.ToShortDateString(),
                    ShortTime = x.AgreementItem.Date.Value.ToShortTimeString(),
                    File = x.AgreementItem.File,
                    Action = new StageAction()
                    {
                        ID = x.AgreementItem.Action.ID,
                        Title = x.AgreementItem.Action.Title,
                        Color = x.AgreementItem.Action.Color,
                    }
                }).ToList();
                return new JsonNetResult(result);
            }
        }

        public ActionResult GetActions(string objType)
        {
            var type = this.GetType(objType);

            if (type == null) return null;

            var config = this.GetViewModelConfig(type);

            if (config == null) return null;

            return new JsonNetResult(config.TypeEntity.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.PropertyType.IsEnum)
                    .ToDictionary(x => x.Name, x => new BPActionBuilderVm
                    {
                        Member = x.Name,
                        Title = x.GetCustomAttribute<DetailViewAttribute>().With(d => d.Name) ?? x.Name,
                        Values = UiFasade.GetUiEnum(x.PropertyType).Values.Select(e => new BPActionValueVm
                        {
                            Title = e.Title,
                            Value = e.Value
                        })
                    }));
        }

        public ActionResult CheckTypes(string testType, string baseType)
        {
            var type1 = GetViewModelConfig(testType).With(x => x.TypeEntity);
            var type2 = GetViewModelConfig(baseType).With(x => x.TypeEntity);

            return new JsonNetResult(new { result = type1 != null && type2 != null && (type2.IsAssignableFrom(type1) || type1 == type2) });
        }

        public ActionResult TaskToolbar(int? taskID)
        {
            if (taskID.HasValue && taskID.Value != 0)
            {
                using (var uow = CreateUnitOfWork())
                {
                    var task = _taskService.GetAll(uow).FirstOrDefault(x => x.ID == taskID.Value);

                    var bpTask = task as BPTask;
                    if (bpTask != null)
                    {
                        var type = GetType(bpTask.ObjectType);

                        string mnemonic = bpTask.ObjectType;

                        if (type != null)
                        {
                            var obj = GetService<IBaseObjectCrudService>(bpTask.ObjectType).Get(uow, bpTask.ObjectID);

                            if (obj != null)
                            {
                                return PartialView("_TaskToolbar",
                                    new TaskToolbarViewModel(GetViewModelConfig(mnemonic), obj));

                            }
                        }
                    }
                }
            }

            return RedirectToAction("Toolbar", "Task", new
            {
                taskID
            });
        }

        public ActionResult GetHelp(string objectType)
        {
            var type = this.GetType(objectType);

            if (type != null)
            {
                var types = new[] { typeof(String), typeof(Decimal), typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan), typeof(Guid) };

                var items = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.PropertyType.IsValueType || type.IsPrimitive || types.Contains(x.PropertyType))
                    .Select(x => new { Attr = x.GetCustomAttribute<DetailViewAttribute>(), Property = x })
                    .Where(x => x.Attr != null)
                    .Select(x => new { Title = x.Attr.Name, Text = String.Format("[{0}] или [{1}]", x.Property.Name, x.Attr.Name) })
                    .Concat(new[] { new { Title = "Предыдущий комментарий", Text = "[Comment]" } });

                return JsonNet(items);
            }

            return null;
        }

        public ActionResult GetConditionalList(string objectType)
        {
            var type = this.GetType(objectType);

            if (type != null)
            {
                var props = this.GetEditors(type).ToList();
                var vm = new ObjectInitializerVm()
                {
                    Editors = props,
                    Type = type,
                    Mnemonic = this.GetViewModelConfig(type).Mnemonic
                };

                return PartialView("_ConditionalList", vm);
            }

            return null;
        }

        public ActionResult GetCondition(string mnemonic, string property, string parenttype, string objectType)
        {
            var type = Type.GetType(objectType) ?? this.GetType(objectType);
            var parentType = this.GetType(parenttype);

            if (type != null)
            {
                var model = new WithCustomEditorVm
                {
                    Config = this.GetViewModelConfig(mnemonic),
                    Property = property,
                    Nullable = !type.IsValueType || Nullable.GetUnderlyingType(type) != null
                };

                if (type.IsNumericType() && parentType != null)
                {
                    model.Editors = this.GetEditors(parentType, x => x.IsNumericType()).Where(x => x.Member != property);

                    return View("Conditional/Int", model);
                }

                if (typeof(BaseObject).IsAssignableFrom(type))
                {
                    ViewBag.Config = this.GetViewModelConfig(type);

                    model.Editors = this.GetEditors(parentType, x => x == type).Where(x => x.Member != property);

                    return View("Conditional/BaseObject", model);
                }

                if (type == typeof(String))
                {
                    model.Editors = this.GetEditors(parentType, x => x == typeof(String)).Where(x => x.Member != property);

                    return View("Conditional/String", model);
                }

                if (type.IsEnum)
                {
                    model.Editors = Enum.GetValues(type).OfType<Enum>().Select(x => new EditorVm(x.GetTitle(), x.GetValue().ToString())).Where(x => x.Member != property);

                    return View("Conditional/Enum", model);
                }

                if (type == typeof(bool) || type == typeof(bool?))
                {
                    model.Editors = model.Editors = this.GetEditors(parentType, x => x == typeof(bool) || x == typeof(bool?)).Where(x => x.Member != property);

                    return View("Conditional/Bool", model);
                }

                if (type == typeof(DateTime) || type == typeof(DateTime?))
                {
                    model.Editors = model.Editors = this.GetEditors(parentType, x => x == typeof(DateTime) || x == typeof(DateTime?)).Where(x => x.Member != property);

                    return View("Conditional/DateTime", model);
                }
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    model.Editors = model.Editors = this.GetEditors(parentType, x => x == typeof(IEnumerable)).Where(x => x.Member != property);
                    var t = type.GenericTypeArguments.FirstOrDefault();
                    ViewBag.Config = GetViewModelConfig(t);
                    return View("Conditional/Collection", model);
                }
            }

            return null;
        }

        public ActionResult GetEditorList(string objectType, bool allprops = false)
        {
            var type = this.GetType(objectType);

            if (type != null)
            {
                var props = this.GetEditors(type, null, allprops).ToList();

                return PartialView("_GetEditorList", new ObjectInitializerVm
                {
                    Editors = props,
                    Type = type,
                    Mnemonic = this.GetViewModelConfig(type).Mnemonic
                });
            }

            return null;
        }

        public ActionResult GetEditor(string mnemonic, string property, string parenttype, string objectType)
        {
            var type = Type.GetType(objectType) ?? this.GetType(objectType);
            var parentType = this.GetType(parenttype);

            if (type != null)
            {
                var model = new WithCustomEditorVm
                {
                    Config = this.GetViewModelConfig(mnemonic),
                    Property = property,
                    Nullable = !type.IsValueType || Nullable.GetUnderlyingType(type) != null
                };

                if (type.IsNumericType() && parentType != null)
                {
                    model.Editors = this.GetEditors(parentType, x => x.IsNumericType());

                    return View("Editors/Int", model);
                }

                if (typeof(BaseObject).IsAssignableFrom(type))
                {
                    ViewBag.Config = this.GetViewModelConfig(type);

                    model.Editors = this.GetEditors(parentType, x => x == type);

                    return View("Editors/BaseObject", model);
                }

                if (type == typeof(String))
                {
                    model.Editors = this.GetEditors(parentType, x => x == typeof(String));

                    return View("Editors/String", model);
                }

                if (type.IsEnum)
                {
                    model.Editors = Enum.GetValues(type).OfType<Enum>().Select(x => new EditorVm(x.GetTitle(), x.GetValue().ToString()));

                    return View("Editors/Enum", model);
                }

                if (type == typeof(bool) || type == typeof(bool?))
                {
                    model.Editors = model.Editors = this.GetEditors(parentType, x => x == typeof(bool) || x == typeof(bool?));

                    return View("Editors/Bool", model);
                }

                if (type == typeof(DateTime) || type == typeof(DateTime?))
                {
                    model.Editors = model.Editors = this.GetEditors(parentType, x => x == typeof(DateTime) || x == typeof(DateTime?));

                    return View("Editors/DateTime", model);
                }
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    model.Editors = model.Editors = this.GetEditors(parentType, x => x == typeof(IEnumerable));
                    var t = type.GenericTypeArguments.FirstOrDefault();
                    ViewBag.Config = GetViewModelConfig(t);
                    return View("Editors/Collection", model);
                }
            }

            return null;
        }

        public ActionResult ChangeValidationStep(string objectType)
        {
            var type = GetType(objectType);
            if (type != null)
            {
                var props = this.GetEditors(type, x =>
                {
                    if (x != typeof(string) && x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        return false;
                    }
                    return true;
                }).ToList();


            }
            return null;
        }

        private IEnumerable<EditorVm> GetEditors(Type type, Func<Type, bool> predicate = null, bool allprops = false)
        {
            var props = new List<EditorVm>();

            var allProps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty).ToList();

            if (!allprops)
                allProps = allProps.Where(x => x.CanWrite).ToList();

            if (typeof(ICategorizedItem).IsAssignableFrom(type))
            {
                var hCategoryProperty = allProps.FirstOrDefault(x => Attribute.IsDefined(x, typeof(ForeignKeyAttribute)));

                if (hCategoryProperty != null)
                    props.Add(new EditorVm("Категория", hCategoryProperty));
            }

            if (predicate != null)
                allProps = allProps.Where(x => predicate(x.PropertyType)).ToList();
            return allProps.Select(x => new { Prop = x, PropName = x.GetCustomAttribute<DetailViewAttribute>()?.Name ?? x.Name })
                .Select(x => new EditorVm(x.PropName, x.Prop)).Union(props);
        }

        private Type GetType(string objectType)
        {
            return !String.IsNullOrEmpty(objectType) ? GetViewModelConfig(objectType).With(x => x.TypeEntity) : null;
        }

        public ActionResult GetObject(string type, int? id = null)
        {
            using (var uow = CreateUnitOfWork())
            {
                var viewModelConfig = GetViewModelConfig(type);

                if (viewModelConfig != null)
                {
                    BaseObject baseObject = null;

                    if (id != null)
                    {
                        var serv = this.GetService<IBaseObjectCrudService>(viewModelConfig.Mnemonic);

                        baseObject = serv.Get(uow, id.Value);
                    }

                    return JsonNet(new
                    {
                        Config = new { viewModelConfig.LookupProperty, ObjectType = viewModelConfig.TypeEntity.FullName },
                        Object = baseObject.IfNotNullReturn(x => new { x.ID, Title = x.GetType().GetProperty(viewModelConfig.LookupProperty.Text).GetValue(x) })
                    });
                }

                throw new NullReferenceException($"Конифиг для типа {type} не найден");
            }
        }

        public ActionResult TestMacros(IEnumerable<InitItem> items, string type, string parentType)
        {
            using (var uow = CreateUnitOfWork())
            {
                var objType = this.GetType(type);
                var parentObjType = this.GetType(parentType);

                Exception exception;
                var result = _workflowService.TestMacros(uow, items, objType, parentObjType, out exception);

                return JsonNet(new
                {
                    result,
                    error = !result ? exception.ToStringWithInner() : String.Empty
                });
            }
        }

        public ActionResult TestBranch(IEnumerable<ConditionItem> items, string type, string parentType)
        {
            using (var uow = CreateUnitOfWork())
            {
                var objType = this.GetType(type);
                var parentObjType = this.GetType(parentType);

                Exception exception;
                var result = _workflowService.TestBranch(uow, items, objType, parentObjType, out exception);

                return JsonNet(new
                {
                    result,
                    error = !result ? exception.ToStringWithInner() : String.Empty
                });
            }
        }

        //TODO зачем мнемонический биндер
        //public ActionResult WorkflowList(string mnemonic, BaseObject model)
        //{
        //    using (var uow = CreateUnitOfWork())
        //    {
        //        return JsonNet(
        //            _workflowService.GetWorkflowList(uow, GetViewModelConfig(mnemonic).TypeEntity, model)
        //                .Select(x => new
        //                {
        //                    x.Title,
        //                    x.ID,
        //                    x.Description,
        //                    x.CreatedDate,
        //                    x.Creator
        //                }));
        //    }
        //}

        public ActionResult GetCollectionItems(string mnemonic, string property)
        {
            using (var uow = CreateUnitOfWork())
            {
                var editorVm = UiFasade.GetEditors(mnemonic).FirstOrDefault(x => x.PropertyName == property);
                var service = GetService<IQueryService<object>>(editorVm.Mnemonic);
                return JsonNet(service.GetAll(uow));
            }
        }

        public ActionResult GetStakeHoldersStrategies(string type)
        {
            Type objType = Type.GetType(type);
            var strategies = _strategyService.GetStakeholdersSelectionStrategies(objType);

            var values = strategies.Select(
                x => new StrategyVm()
                {
                    Text = x.Name,
                    Value = x.GetType().FullName
                });

            return new JsonNetResult(values);
        }

        [HttpPost]
        public ActionResult ReStartWorkflow(string mnemonic, int id)
        {

            using (var uow = CreateSystemTransactionUnitOfWork())
            {
                try
                {
                    var config = GetViewModelConfig(mnemonic);

                    var objectService = GetService<IBaseObjectCrudService>(config.Mnemonic);

                    var bpObject = (IBPObject)objectService.Get(uow, id);

                    if (bpObject == null)
                        throw new Exception("Не надйен объект бп");

                    //if(bpObject.WorkflowContext != null)
                    //    throw new Exception("БП существует, не возможно перезапустить");

                    _workflowService.ReStartWorkflow(uow, bpObject, objectService);

                    uow.Commit();

                    return new JsonNetResult(new { OK = true });
                }
                catch (Exception error)
                {
                    uow.Rollback();
                    return new JsonNetResult(new { error = error.Message });
                }
            }


        }

        public async Task<JsonNetResult> GetWorkflowVersions(string objectType, string startswith)
        {
            using (var uow = CreateUnitOfWork())
            {
                var wfConfig = GetViewModelConfig("BPWorkflow");
                var objConfig = GetViewModelConfig(objectType);
                var type = objConfig.TypeEntity;
                var res = await _workflowService.GetWorkflowList(uow, type).Select(wfConfig.ListView).ToListAsync();
                return new JsonNetResult(res);
            }
        }
    }
}
