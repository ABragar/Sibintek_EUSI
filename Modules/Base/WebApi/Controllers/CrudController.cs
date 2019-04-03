using Base;
using Base.ComplexKeyObjects.Common;
using Base.DAL;
using Base.Links.Service.Abstract;
using Base.Security.ObjectAccess;
using Base.Security.Service.Abstract;
using Base.Service.Crud;
using Base.Service.Log;
using Base.UI;
using Base.UI.ViewModal;
using Base.Utils.Common;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Models.Crud;
using CorpProp.Extentions;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("crud/{mnemonic}")]
    internal class CrudController : BaseApiController
    {
        /// <summary>
        /// Поиск по коду и мнемонике.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mnemonic">Мнемоника</param>
        /// <param name="code">Код элемента</param>
        /// <returns>Объект с моделью полученного элемента или описанием ошибки.</returns>
        [HttpGet]
        [Route("code/{code}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Get<T>(string mnemonic, string code)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateTransactionUnitOfWork())
                {
                    return Ok(new
                    {
                        model = Get<T>(uofw, code),
                        //access = GetObjAccess<T>(uofw, ),
                    });
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Get<T>(string mnemonic, int id)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateTransactionUnitOfWork())
                {
                    return Ok(new
                    {
                        model = Get<T>(uofw, id),
                        access = GetObjAccess<T>(uofw, id),
                    });
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        /// <summary>
        /// Получение данных дочерних областей
        /// </summary>
        /// <typeparam name="T">тип родительского объекта</typeparam>
        /// <param name="mnemonic">мнемоника родительского объекта</param>
        /// <param name="id">идентификатор родительского объекта</param>
        /// <param name="childsysname">sysname дочерней области</param>
        /// <returns></returns>
        [HttpGet]
        [Route("child/{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult GetChild<T>(string mnemonic, int id, string childsysname, string date = null)
            where T : BaseObject
        {
            try
            {
                var childEditor = GetConfig(mnemonic).DetailView.Editors
                    .FirstOrDefault(model => model.SysName == childsysname);
                if (childEditor == null)
                {
                    throw new Exception(
                        $"Запрос получения дочерней области содержит sysname=={childsysname} для которого нет редактора в конфигурации {mnemonic}");
                }

                using (var unitOfWork = CreateTransactionUnitOfWork())
                {
                    var access = GetObjAccess<T>(unitOfWork, id);
                    var model = GetChildDetailView<T>(unitOfWork, childEditor, parentId: id, parentMnemonic: mnemonic, date: date);
                    return Ok(new
                    {
                        model,
                        access,
                        byDate = date,
                    });
                }
            }
            catch (Exception e)
            {
                _logger.Log(e);
                return Ok(new
                {
                    error = e.Message
                });
            }
        }

        private object GetChildDetailView<TParent>(IUnitOfWork unitOfWork, EditorViewModel editor, int parentId,
            string parentMnemonic, string date = null) where TParent : BaseObject
        {
            const string linkKey = "link";
            if (!editor.EditorTemplateParams.TryGetValue(linkKey, out var link))
            {
                //"Не задан link"
                throw new Exception();
            }

            var childMnemonic = editor.Mnemonic;
            var childConfig = _view_model_config_service.Get(childMnemonic);

            var getMethod = GetType().GetMethod(nameof(Get),
                BindingFlags.NonPublic | BindingFlags.Instance,
                Type.DefaultBinder,
                new[] { typeof(IUnitOfWork), typeof(int), typeof(string), typeof(string), typeof(string), typeof(string)},
                null);
            if (getMethod == null)
            {
                //"Не найден метод Get в классе this"
                throw new Exception();
            }

            var genericGetMethod = getMethod.MakeGenericMethod(typeof(TParent), childConfig.TypeEntity);
            var model = genericGetMethod.Invoke(this,
                new object[] { unitOfWork, parentId, parentMnemonic, editor.Mnemonic, link, date });
            return model;
        }

        [HttpGet]
        [Route("preview/{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult GetPreview<T>(string mnemonic, int id)
            where T : BaseObject
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var config = GetConfig();

                    if (!config.Preview.Enable)
                        return Ok(new { error = $"preview is not provided for [{mnemonic}]" });

                    var serv = GetQueryService<T>();

                    if (serv == null)
                        return Ok(new { error = $"service [{mnemonic}] not found" });

                    return Ok(config.Preview.GetData(uofw, serv, id));
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = $"mnemonic: {mnemonic}; id: {id}; error: {e.Message}" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Delete<T>(string mnemonic, int id)
            where T : BaseObject
        {
            int error = 0;
            string message = "";

            try
            {
                using (var uofw = CreateTransactionUnitOfWork())
                {
                    Delete<T>(uofw, id);

                    uofw.Commit();
                }

                message = "Данные успешно удалены!";
            }
            catch (Exception e)
            {
                error = 1;
                message = $"Ошибка удаления записи: {e.ToStringWithInner()}";
            }

            return Ok(new
            {
                error = error,
                message = message
            });
        }

        private object GetObjAccess<T>(IUnitOfWork uofw, int id)
        {
            if (typeof(IAccessibleObject).IsAssignableFrom(typeof(T)))
            {
                var access = _security_service.GetAccessType(uofw, typeof(T), id);

                return new
                {
                    Update = access.HasFlag(AccessType.Update),
                    Delete = access.HasFlag(AccessType.Delete)
                };
            }

            return new
            {
                Update = true,
                Delete = true,
            };
        }

        [HttpPatch]
        [Route("{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Patch<T>(string mnemonic, int id,
            [FromBody] PatchModel<T> patch_model)
            where T : BaseObject
        {
            int intError = 0;
            string strMsg = "";

            object res = null;

            try
            {
                using (var uofw = CreateTransactionUnitOfWork())
                {
                    var jobj = new JObject();

                    var serializer = CreateJsonSerializer();

                    jobj = JObject.FromObject(Get<T>(uofw, id), serializer);

                    //TODO отфильтровать поля по только разрешенным

                    jobj.Merge(patch_model.model, new JsonMergeSettings()
                    {
                        MergeNullValueHandling = MergeNullValueHandling.Merge,
                        MergeArrayHandling = MergeArrayHandling.Replace,
                    });

                    var obj = jobj.ToObject<T>(serializer);
                    obj.ID = id;

                    Save<T>(uofw, obj);
                    uofw.Commit();
                    return Ok(new
                    {
                    });
                }
            }
            catch (Exception e)
            {
                intError = 1;
                strMsg = $"Ошибка сохранения записи: {e.ToStringWithInner()}";
            }

            return Ok(new
            {
                error = intError,
                message = strMsg,
                model = res,
            });
        }

        private void SetLink<TParentModel, TModel>(TParentModel parentModel, TModel childModel, string link)
            where TParentModel : BaseObject
            where TModel : BaseObject
        {
            GetLinkObjectsParams(parentModel, childModel, link,
                out var leftModel, out var leftParam,
                out var rightModel, out var rightparam, out var _);

            var paramLeft = Expression.Parameter(leftModel.GetType(), "Param_0");
            var paramRight = Expression.Parameter(rightModel.GetType(), "Param_1");
            var right = CreateExpression(paramRight, rightparam);
            var left = CreateExpression(paramLeft, leftParam);
            right = Expression.Convert(right, left.Type);
            var assign = Expression.Lambda(Expression.Assign(left, right), paramLeft, paramRight);
            var Delegate = assign.Compile();
            Delegate.DynamicInvoke(leftModel, rightModel);
        }

        private Expression CreateExpression(ParameterExpression param, string propertyName)
        {
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return body;
        }

        private static void GetLinkObjectsParams(
            BaseObject parentModel,
            BaseObject childModel,
            string link,
            out BaseObject leftModel,
            out string leftParam,
            out BaseObject rightModel,
            out string rightparam,
            out bool parentChildReverse)
        {
            ChildViewHelper.GetLinkParams(link, out var childParam, out var parentParam, out parentChildReverse);

            leftModel = parentChildReverse
                ? childModel
                : parentModel;
            leftParam = parentChildReverse
                ? childParam
                : parentParam;

            rightModel = parentChildReverse
                ? parentModel
                : childModel;
            rightparam = parentChildReverse
                ? parentParam
                : childParam;

            if (leftParam.EndsWith(".ID"))
            {
                leftParam = leftParam.Remove(leftParam.Length - ".ID".Length) + "ID";
            }
        }

        private void SaveChild<TParentModel, TModel>(IUnitOfWork uow, TParentModel parentModel, JToken childModel,
            string mnemonic, string link)
            where TParentModel : BaseObject
            where TModel : BaseObject
        {
            TModel model = childModel.ToObject<TModel>();

            GetLinkObjectsParams(parentModel, model, link,
                out var leftModel, out var leftParam,
                out var rightModel, out var rightparam, out var parentChildReverse);

            SetLink(parentModel, model, link);

            var newModel = SaveModel<TModel>(uow, model, mnemonic);

            if (newModel?.ID == 0)
            {
                if (parentChildReverse)
                    Save<TModel>(uow, mnemonic, newModel);
            }
        }

        private void MergeAndSaveModel<TParentModel, TModel>(IUnitOfWork uow, TParentModel parentModel, List<MergeModelConfig> childModels)
            where TParentModel : BaseObject
            where TModel : BaseObject
        {
            TModel baseModel = null;
            foreach (var childModel in childModels)
            {
                TModel model = childModel.JsonModel.ToObject<TModel>();
                if (model?.ID != 0)
                {
                    var service = GetBaseObjectService<TModel>(childModel.Mnemonic);
                    baseModel = service.GetById(uow, model.ID).Single();
                }

                if (baseModel == null)
                {
                    baseModel = model;
                }

                var properties = childModel.VmConfig.DetailView.Editors.Where(x => x.Visible).Select(x => x.PropertyName).Where(x => x != null).ToList();
                foreach (var property in properties)
                {
                    var value = model.GetType().GetProperty(property).GetValue(model);
                    var prop = baseModel.GetType().GetProperty(property);
                    if (prop != null && prop.GetSetMethod() != null)
                        prop.SetValue(baseModel, value);
                }
            }

            var link = childModels.First()?.Link;
            var mnemonic = childModels.First()?.Mnemonic;

            GetLinkObjectsParams(parentModel, baseModel, link,
                out var leftModel, out var leftParam,
                out var rightModel, out var rightparam, out var parentChildReverse);

            SetLink(parentModel, baseModel, link);

            var newModel = SaveModel<TModel>(uow, baseModel, mnemonic);
            if (baseModel?.ID == 0)
            {
                if (parentChildReverse)
                    Save<TModel>(uow, mnemonic, newModel);
            }
        }

        private void SaveChildren<TParent>(IUnitOfWork unitOfWork, TParent parentModel, JToken jChildren)
            where TParent : BaseObject
        {
            const string linkKey = "link";
            const string modelFieldName = "model";
            const string sysnameFieldName = "sysname";

            var entityModels = new Dictionary<Type, List<MergeModelConfig>>();

            foreach (var child in jChildren)
            {
                var sysname = child.SelectToken(sysnameFieldName).ToObject<string>();
                if (string.IsNullOrWhiteSpace(sysname))
                {
                    //"Нет поля sysname"
                    throw new Exception("Нет поля sysname в описании дочерней области");
                }

                var editor = GetConfig().DetailView.Editors.FirstOrDefault(model => model.SysName == sysname);
                if (editor == null)
                {
                    //"Нет редактора c SysName == {sysname}"
                    throw new Exception($"Нет редактора c SysName == {sysname}");
                }

                if (!editor.EditorTemplateParams.TryGetValue(linkKey, out var link))
                {
                    throw new Exception($"Не задан link для редактора c sysname=={sysname}");
                }

                //разделим модели в разрезе сохраняемых сущностей
                var childMnemonic = editor.Mnemonic;
                var childConfig = _view_model_config_service.Get(childMnemonic);
                var entityType = childConfig.TypeEntity;
                var childModel = child.SelectToken(modelFieldName);

                var modelConfig = new MergeModelConfig()
                {
                    Mnemonic = childMnemonic,
                    VmConfig = childConfig,
                    EntityType = entityType,
                    JsonModel = childModel,
                    Link = link
                };

                if (entityModels.ContainsKey(modelConfig.EntityType))
                {
                    List<MergeModelConfig> models = null;
                    entityModels.TryGetValue(entityType, out models);
                    models.Add(modelConfig);
                }
                else
                {
                    entityModels.Add(entityType,
                        new List<MergeModelConfig>()
                        {
                            modelConfig
                        });
                }
            }

            var saveMethod = GetType().GetMethod(nameof(SaveChild), BindingFlags.NonPublic | BindingFlags.Instance);
            var appendSaveMethod = GetType()
                .GetMethod(nameof(MergeAndSaveModel), BindingFlags.NonPublic | BindingFlags.Instance);
            if (saveMethod == null)
            {
                //"Не найден метод сохранения в классе this"
                throw new Exception($"Не найден метод сохранения в {nameof(CrudController)}");
            }
            if (appendSaveMethod == null)
            {
                //"Не найден метод сохранения в классе this"
                throw new Exception($"Не найден метод сохранения в {nameof(CrudController)}");
            }

            foreach (var entry in entityModels)
            {
                var genericMergeAndSaveMethod = appendSaveMethod.MakeGenericMethod(typeof(TParent), entry.Key);

                var childModels = entry.Value;
                {
                    genericMergeAndSaveMethod.Invoke(this,
                        new object[] { unitOfWork, parentModel, childModels });
                }
            }
        }

        [HttpPost]
        [Route()]
        [GenericAction("mnemonic")]
        public IHttpActionResult Save<T>(string mnemonic, [FromBody] JObject data, bool returnEntireModel = false)
            where T : BaseObject
        {
            //var xx = this.Request.Content.
            //                 ReadAsByteArrayAsync().
            //                 GetAwaiter().GetResult();

            //var postData = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(xx.Select(b => Convert.ToChar(b)).ToArray()));

            const string childenFieldName = "childs";
            var serializer = CreateJsonSerializer();
            SaveModel<T> saveModel = data.ToObject<SaveModel<T>>(serializer);
            {
                int intError = 0;
                string strMsg = "";

                object res = null;
                object access = null;

                try
                {
                    int id = 0;

                    using (var uofw = CreateTransactionUnitOfWork())
                    {
                        var newModel = SaveModel<T>(uofw, saveModel.model);
                        if (data.TryGetValue(childenFieldName, out JToken jChildrens))
                        {
                            SaveChildren(uofw, newModel, jChildrens);
                        }

                        id = newModel.ID;

                        uofw.Commit();
                    }

                    using (var uofw = CreateUnitOfWork())
                    {
                        if (returnEntireModel)
                        {
                            res = Get<T>(uofw, id);
                            access = GetObjAccess<T>(uofw, id);
                        }
                        else
                        {
                            res = new { ID = id };
                        }

                        //TODO перенести в транзакцию сохранения
                        if (saveModel.link != null && saveModel.link.LinkSourceID != 0)
                        {
                            var config = _view_model_config_service.Get(saveModel.link.Mnemonic);
                            var service = config.GetService<IQuerySource<BaseObject>>();

                            var dest = GetBaseObjectService<T>().Get(uofw, id);

                            var sourceId = saveModel.link.LinkSourceID;
                            var source = service.GetQuery(uofw).SingleOrDefault(x => x.ID == sourceId);

                            if (source != null)
                            {
                                _link_item_service.SaveLink(uofw, dest, source);
                            }
                        }
                    }

                    strMsg = "Данные успешно сохранены!";
                }
                catch (Exception e)
                {
                    intError = 1;
                    strMsg = $"Ошибка сохранения записи: {e.ToStringWithInner()}";
                }

                return Ok(new
                {
                    error = intError,
                    message = strMsg,
                    model = res,
                    access = access,
                });
            }
        }

        [HttpPost]
        [Route("saveTask")]
        [GenericAction("mnemonic")]
        public IHttpActionResult SaveTask<T>(string mnemonic, [FromBody]SaveModel<T> save_model, bool returnEntireModel = false)
    where T : BaseObject
        {
            int intError = 0;
            string strMsg = "";

            object res = null;
            object access = null;

            try
            {
                int id = 0;

                using (var uofw = CreateTransactionUnitOfWork())
                {
                    id = Save<T>(uofw, save_model.model);

                    uofw.Commit();
                }

                using (var uofw = CreateTransactionUnitOfWork())
                {
                    SibTaskHelper.VeryfyUpdateParentTaskPeriod(uofw, id);
                    uofw.Commit();
                }

                using (var uofw = CreateUnitOfWork())
                {
                    if (returnEntireModel)
                    {
                        res = Get<T>(uofw, id);
                        access = GetObjAccess<T>(uofw, id);
                    }
                    else
                    {
                        res = new { ID = id };
                    }

                    //TODO перенести в транзакцию сохранения
                    if (save_model.link != null && save_model.link.LinkSourceID != 0)
                    {
                        var config = _view_model_config_service.Get(save_model.link.Mnemonic);
                        var service = config.GetService<IQuerySource<BaseObject>>();

                        var dest = GetBaseObjectService<T>().Get(uofw, id);

                        var source_id = save_model.link.LinkSourceID;
                        var source = service.GetQuery(uofw).Where(x => x.ID == source_id).SingleOrDefault();

                        if (source != null)
                        {
                            _link_item_service.SaveLink(uofw, dest, source);
                        }
                    }
                }
                strMsg = "Данные успешно сохранены!";
            }
            catch (Exception e)
            {
                intError = 1;
                strMsg = $"Ошибка сохранения записи: {e.ToStringWithInner()}";
            }

            return Ok(new
            {
                error = intError,
                message = strMsg,
                model = res,
                access = access,
            });
        }

        private readonly ILinkItemService _link_item_service;

        private readonly IViewModelConfigService _view_model_config_service;

        private readonly IUnitOfWorkFactory _unit_of_work_factory;

        private readonly ISecurityService _security_service;

        private readonly ILogService _logger;


        public CrudController(IViewModelConfigService view_model_config_service, IUnitOfWorkFactory unit_of_work_factory, ILinkItemService link_item_service, ISecurityService security_service, ILogService logger)
            : base(view_model_config_service, unit_of_work_factory, logger)
        {
            _logger = logger;
            _view_model_config_service = view_model_config_service;
            _link_item_service = link_item_service;
            _security_service = security_service;
            _unit_of_work_factory = unit_of_work_factory;
        }

        private new JsonSerializer CreateJsonSerializer()
        {
            return this.ControllerContext.Configuration.Formatters.JsonFormatter.CreateJsonSerializer();
        }

        private void Delete<T>(IUnitOfWork uofw, int id)
            where T : BaseObject
        {
            if (id == 0) return;

            var serv = GetBaseObjectService<T>();

            var obj = serv.Get(uofw, id);

            if (obj == null) return;

            serv.Delete(uofw, obj);
        }

        private IBaseObjectCrudService GetService<T>(string mnemonic = null) where T : BaseObject
        {
            return (typeof(T) == typeof(DealCurrencyConversion))
                ? GetBaseObjectService<SibDeal>(mnemonic) as IBaseObjectCrudService
                : GetBaseObjectService<T>(mnemonic);
        }

        private object Get<T>(IUnitOfWork uofw, int id)
            where T : BaseObject
        {
            var config = GetConfig();

            var model = config.DetailView.GetData(uofw, GetService<T>(), id);

            if (model == null)
                throw new Exception("Объект не найден");

            return model;
        }

        private object Get<TParent, TChild>(IUnitOfWork unitOfWork, int parentID, string parentMnemonic,
            string childMnemonic, string oneToOneLink, string date = null)
            where TChild : BaseObject
            where TParent : BaseObject
        {
            var config = GetConfig(childMnemonic);

            var childService = GetService<TChild>(childMnemonic);

            var parentService = GetService<TParent>(parentMnemonic);
            IQueryable query =
                GetSingleByOneToOneLink<TParent, TChild>(unitOfWork, childService, parentService, oneToOneLink,
                    parentID, date);
            var model = config.DetailView.GetDataOrDefault(unitOfWork, query);
            return model;
        }

        private IQueryable GetSingleByOneToOneLink<TParent, TChild>(
            IUnitOfWork unitOfWork
            , IBaseObjectCrudService childService
            , IBaseObjectCrudService parentService
            , string link
            , int parentId
            , string date = null)
        {
            //TODO: доработать выборку с учётом историчности данных.
            bool byOid = false;
            var isHistoryLink = (parentService is CorpProp.Services.Base.IHistoryService<TParent>
                && typeof(TParent).GetProperty(nameof(CorpProp.Entities.Base.TypeObject.Oid)) != null);
           
            if (isHistoryLink && link.Trim().Contains("parent.ID="))
            {                
                var childPropStrOld = link.Replace("parent.ID=","").Replace("child.","");
                var childPropStr = childPropStrOld;
                if (childPropStr.EndsWith("ID"))
                    childPropStr = childPropStr.Remove((childPropStr.Length - 2), 2) + "Oid";                
                if (typeof(TChild).GetProperty(childPropStr) != null)
                {
                    byOid = true;
                    link = "parent.Oid=child." + childPropStr;
                }   
            }
            
            ChildViewHelper.GetLinkParams(link, out var childParamStr, out var parentParamStr, out _);
            DateTime? dt = date.GetDate();

            var childs = (childService is CorpProp.Services.Base.IHistoryService<TChild>) ?
                (((CorpProp.Services.Base.IHistoryService<TChild>)childService).GetAllByDate(unitOfWork, dt) as IQueryable<TChild>)
                : (childService.GetAll(unitOfWork) as IQueryable<TChild>);

            var parents = parentService.GetById(unitOfWork, parentId) as IQueryable<TParent>;

            if (byOid)
            {
                return childs.Join<TChild, TParent, Guid?, TChild>(
                    parents
                    , (GetParameterFromLink(typeof(TChild), childParamStr, byOid) as Expression<Func<TChild, Guid?>>)
                    , (GetParameterFromLink(typeof(TParent), parentParamStr, byOid) as Expression<Func<TParent, Guid?>>)
                    , (child, parent) => child);
            }

            var childParam = 
                GetParameterFromLink(typeof(TChild), childParamStr, byOid) as Expression<Func<TChild, int?>>;
            var parentParam =
                GetParameterFromLink(typeof(TParent), parentParamStr, byOid) as Expression<Func<TParent, int?>>;
            var join = childs.Join<TChild, TParent, int?, TChild>(parents, childParam, parentParam,
                (child, parent) => child);
            return join;
        }


      

        private LambdaExpression GetParameterFromLink(Type parentType, string paramStr, bool byOid, bool fixId = true)
        {
            if (fixId && paramStr.EndsWith(".ID"))
            {
                paramStr = paramStr.Remove(paramStr.Length - ".ID".Length) + "ID";
            }

            var param = Expression.Parameter(parentType, "parenParam");
            var property = CreateExpression(param, paramStr);
            var nullableproperty = Expression.Convert(property, ((byOid) ? typeof(Guid?) : typeof(int?)));

            var lambda = Expression.Lambda(nullableproperty, param);

            return lambda;
        }
       
        private object Get<T>(IUnitOfWork uofw, string code)
                    where T : BaseObject
        {
            var serv = GetBaseObjectService<T>();

            var config = GetConfig();

            var model = config.DetailView.GetData(uofw, serv, code: code);

            if (model == null)
                throw new Exception("Объект не найден");

            return model;
        }

        private int Save<T>(IUnitOfWork uofw, BaseObject model)
            where T : BaseObject
        {
            var res = SaveModel<T>(uofw, model);
            return res.ID;
        }

        private int Save<T>(IUnitOfWork uofw, string mnemonic, BaseObject model)
            where T : BaseObject
        {
            var res = SaveModel<T>(uofw, model, mnemonic);
            return res.ID;
        }

        private BaseObject SaveModel<T>(IUnitOfWork uofw, BaseObject model, string mnemonic = null)
            where T : BaseObject
        {
            var service = GetBaseObjectService<T>(mnemonic);

            if (model is Cadastral)
            {
                CheckDuplCadastral(uofw, model);
            }

            var res = model.ID == 0 ? service.Create(uofw, model) : service.Update(uofw, model);

            return res;
        }

        public void CheckDuplCadastral(IUnitOfWork uow, BaseObject model)
        {
            if (model == null) return;

            Cadastral cad = model as Cadastral;
            if (cad?.PropertyComplex != null)
            {
                if (EstateHelper.CheckDuplInPC(uow, cad, cad.CadastralNumber, new string[] { cad.ID.ToString() }, cad.PropertyComplex.ID))
                {
                    throw new System.Exception(String.Format(EstateHelper.DuplInPcError, cad.CadastralNumber));
                }
            }
        }
    }

    internal class MergeModelConfig
    {
        public string Mnemonic { get; set; }
        public ViewModelConfig VmConfig { get; set; }
        public Type EntityType { get; set; }
        public JToken JsonModel { get; set; }
        public string Link { get; set; }
    }
}