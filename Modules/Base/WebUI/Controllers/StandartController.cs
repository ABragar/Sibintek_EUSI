using Base;
using Base.Security.ObjectAccess;
using Kendo.Mvc.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Base.ComplexKeyObjects;
using Base.ComplexKeyObjects.Common;
using Base.DAL;
using Base.Enums;
using Base.Extensions;
using Base.Service.Crud;
using WebUI.Helpers;
using WebUI.Models;
using static System.String;
using Base.UI.Extensions;
using Base.UI.ViewModal;
using WebUI.Extensions;
using Base.Links.Service.Abstract;
using Base.Map.Services;
using Base.Service;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Filter;
using Base.UI.Presets;
using Base.UI.Service;
using Base.UI.Service.Abstract;
using Base.Utils.Common;
using CorpProp.Services;
using WebUI.Concrete;
using CorpProp.Services.DocumentFlow;
using CorpProp.Services.Response;
using CorpProp.Entities.Security;
using CorpProp.Services.Security;

namespace WebUI.Controllers
{
    public class StandartController : BaseController
    {
        private readonly IDvSettingService<DvSettingForType> _dvSettingService;
        private readonly ITypeRelationService _relationService;
        private readonly IDvSettingManager _dvSettingManager;
        private readonly IPresetService<GridPreset> _gridPresetService;
        private readonly IPresetService<MenuPreset> _menuPresetService;
        private readonly IMnemonicFilterService<MnemonicFilter> _mnemonicFilterService;

        public StandartController(IBaseControllerServiceFacade baseServiceFacade, ITypeRelationService relationService,
            IDvSettingService<DvSettingForType> dvSettingService, IDvSettingManager dvSettingManager,
            IPresetService<GridPreset> gridPresetService, IPresetService<MenuPreset> menuPresetService, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService)
            : base(baseServiceFacade)
        {
            _relationService = relationService;
            _dvSettingService = dvSettingService;
            _dvSettingManager = dvSettingManager;
            _gridPresetService = gridPresetService;
            _menuPresetService = menuPresetService;
            _mnemonicFilterService = mnemonicFilterService;
        }

        public async Task<ActionResult> Index(string mnemonic, int? parentID, int? currentID,
            TypeDialog typeDialog = TypeDialog.Frame, string filter = null, bool multiSelect = false)

        {
            mnemonic = mnemonic.Replace("-", ".");

            var model = new StandartDialogViewModel(this, mnemonic, typeDialog)
            {
                ParentID = parentID,
                CurrentID = currentID,
                SysFilter = filter,
                MultiSelect = multiSelect
            };

            await InitViewModel(model);

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public async Task<ActionResult> Menu()
        {
            return PartialView("_SideBar", await _menuPresetService.GetAsync("Menu"));
        }

        public async Task<PartialViewResult> GetDialog(string mnemonic, int? parentID, int? currentID,
            TypeDialog typeDialog = TypeDialog.Frame, string searchStr = null, string filter = null,
            bool multiSelect = false)
        {
            var model = new StandartDialogViewModel(this, mnemonic, typeDialog)
            {
                ParentID = parentID,
                CurrentID = currentID,
                SearchStr = searchStr,
                SysFilter = filter,
                MultiSelect = multiSelect
            };

            await InitViewModel(model);

            return PartialView("Index", model);
        }

        private async Task InitViewModel(StandartDialogViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (model.ViewModelConfig.ListView.Type == ListViewType.Grid ||
                model.ViewModelConfig.ListView.Type == ListViewType.GridCategorizedItem ||
                model.ViewModelConfig.ListView.Type == ListViewType.TreeListView)
            {
                model.Preset = await _gridPresetService.GetAsync(model.Mnemonic);
            }
        }

        public ActionResult GetEditors(string mnemonic)
        {
            return new JsonNetResult(UiFasade.GetEditors(mnemonic).Select(x => new
            {
                x.PropertyName,
                x.Title
            }));
        }

        public ActionResult GetColumns(string mnemonic)
        {
            return new JsonNetResult(UiFasade.GetColumns(mnemonic).Select(x => new
            {
                x.PropertyName,
                x.Title
            }));
        }

        public ActionResult GetEditorViewModel(string mnemonic, string member)
        {
            var editorVm = UiFasade.GetEditors(mnemonic).FirstOrDefault(x => x.PropertyName == member);

            if (editorVm != null)
                return PartialView("DetailView/Editor/EditorView", editorVm);

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetViewModel(string mnemonic, TypeDialog typeDialog, int id = 0)
        {
            ViewBag.ID = id;
            ViewBag.AutoBind = true;

            var model = new StandartDialogViewModel(this, mnemonic, typeDialog);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Builders/DetailView", model);
            }

            return View("Builders/DetailView", model);
        }

        public PartialViewResult GetPartialViewModel(string mnemonic, TypeDialog typeDialog, int id = 0,
            bool autoBind = false, bool isReadOnly = false)
        {
            ViewBag.ID = id;
            ViewBag.AutoBind = autoBind;

            return PartialView("Builders/DetailView",
                new StandartDialogViewModel(this, mnemonic, typeDialog, isReadOnly));
        }

        public PartialViewResult GetAjaxForm(int id, string mnemonic, bool readOnly = false)
        {
            using (var uofw = CreateUnitOfWork())
            {
                var serv = GetService<IBaseObjectCrudService>(mnemonic);

                var model = id != 0 ? serv.Get(uofw, id) : serv.CreateDefault(uofw);

                var commonEditorViewModel = UiFasade.GetCommonEditor(uofw, mnemonic, model);

                string partialName = Format(readOnly
                        ? "~/Views/Standart/DetailView/Display/Common/{0}.cshtml"
                        : "~/Views/Standart/DetailView/Editor/Common/{0}.cshtml",
                    "AjaxForm");

                return PartialView(partialName, new StandartFormModel(this)
                {
                    CommonEditorViewModel = commonEditorViewModel
                });
            }
        }


        public JsonNetResult GetDvSettings(int? id, string objectType)
        {
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var config = this.GetViewModelConfig(nameof(DvSettingForType));

                    var pifo = config.TypeEntity.GetProperty(config.LookupProperty.Text);
                    var settings = _dvSettingService.GetDvSettings(uow, objectType);

                    var ids = settings.Select(x => x.ID).ToList();

                    var parents = settings
                        .Where(x => x.ParentID != null)
                        .Select(x => x.ParentID.Value)
                        .Where(x => ids.Contains(x))
                        .Distinct().ToDictionary(x => x);

                    settings = id != null
                        ? settings.Where(x => x.ParentID == id)
                        : settings.Where(x => x.ParentID == null);

                    var res = settings.ToList().Select(a =>
                        new
                        {
                            id = a.ID,
                            Title = pifo.GetValue(a),
                            hasChildren = parents.ContainsKey(a.ID),
                            isRoot = a.IsRoot
                        });

                    return new JsonNetResult(res);
                }
            }
            catch (Exception error)
            {
                return new JsonNetResult(new { error = error.Message });
            }
        }

        public JsonNetResult ReadDvSettings(int id, string mnemonic)
        {
            using (var uow = CreateUnitOfWork())
            {
                var config = GetViewModelConfig(mnemonic);

                var serv = GetService<IBaseObjectCrudService>(config.Mnemonic);

                var model = serv.Get(uow, id);

                var settings =
                    _dvSettingManager.GetSettingsForType(uow, config.TypeEntity, model).Select(x => new { x.Name, x.ID });

                return new JsonNetResult(settings);
            }
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult KendoUI_Destroy([DataSourceRequest] DataSourceRequest request, string mnemonic,
            BaseObject model)
        {
            int error = 0;
            string message = "";

            var config = GetViewModelConfig(mnemonic);

            try
            {
                using (var uofw = this.CreateTransactionUnitOfWork())
                {
                    this._Delete(uofw, mnemonic, model.ID);

                    uofw.Commit();
                }

                message = "Данные успешно удалены!";
            }
            catch (Exception e)
            {
                error = 1;
                message = $"Ошибка удаления записи: {e.ToStringWithInner()}";
            }

            if (error == 0)
            {
                return new JsonNetResult(new[] { model }.ToDataSourceResult(request, config));
            }
            else
            {
                var res = new DataSourceResult()
                {
                    Errors = message
                };

                return new JsonNetResult(res);
            }
        }

        public async Task<JsonNetResult> KendoUI_CollectionRead([DataSourceRequest] DataSourceRequest request,
            string mnemonic, int? categoryID, bool? allItems, string searchStr, string extrafilter, string[] columns, string mnemonicFilterId)
        {
            var serv = this.GetService<IQueryService<object>>(mnemonic);

            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    var config = this.GetViewModelConfig(mnemonic);

                    IQueryable q;

                    if (mnemonic == "DealCurrencyConversion")
                    {
                        q = ((IDealCurrencyConversionService)serv).GetAll(uofw, 25, DateTime.Today);
                    }
                    else if (serv is ICategorizedItemCrudService)
                    {
                        if (allItems ?? false)
                        {
                            q = ((ICategorizedItemCrudService)serv).GetAllCategorizedItems(uofw,
                                categoryID ?? 0);
                        }
                        else
                        {
                            q = ((ICategorizedItemCrudService)serv).GetCategorizedItems(uofw,
                                categoryID ?? 0);
                        }
                    }
                    else
                    {
                        q = serv.GetAll(uofw);
                    }

                    string mnemonicFilter = await _mnemonicFilterService.GetMnemonicFilter(mnemonic, mnemonicFilterId, extrafilter);
                    q = q.Filter(this, uofw, config, mnemonicFilter);

                    q = q.FullTextSearch(searchStr, CacheWrapper);

                    return
                        new JsonNetResult(await q.Select(config.ListView, columns)
                            .ToDataSourceResultAsync(request, config));
                }
            }
            catch (Exception e)
            {
                var res = new DataSourceResult()
                {
                    Errors = e.Message
                };

                return new JsonNetResult(res);
            }
        }

        //TODO : переписать
        public JsonNetResult KendoUI_TreeRead([DataSourceRequest] DataSourceRequest request, int? id, string mnemonic,
            int? categoryID, bool? allItems, string searchStr, string extrafilter)
        {
            var serv = this.GetService<IQueryService<object>>(mnemonic);

            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    IQueryable q = null;

                    if (serv is ICategorizedItemCrudService)
                    {
                        if (allItems ?? false)
                        {
                            q = ((ICategorizedItemCrudService)serv).GetAllCategorizedItems(uofw,
                                categoryID ?? 0);
                        }
                        else
                        {
                            q = ((ICategorizedItemCrudService)serv).GetCategorizedItems(uofw,
                                categoryID ?? 0);
                        }
                    }
                    else
                    {
                        q = serv.GetAll(uofw);
                    }

                    var config = this.GetViewModelConfig(mnemonic);

                    q = q.Filter(this, uofw, config, extrafilter);

                    q = id != null ? q.Where($"it.ParentID == {id}") : q.Where("it.ParentID == null");

                    return new JsonNetResult(q.Select(config.ListView).ToTreeResult(request));
                }
            }
            catch (Exception e)
            {
                var res = new DataSourceResult()
                {
                    Errors = e.Message
                };

                return new JsonNetResult(res);
            }
        }

        [HttpGet]
        public JsonNetResult GetDefaultCategory(string mnemonic)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var serv = this.GetService<IBaseObjectCrudService>(mnemonic);

                    if (serv is ICategorizedItemCrudService)
                    {
                        //TODO: изменить реализацию так, чтобы возвращалась категория, а не только айдишник
                        var cat = ((ICategorizedItemCrudService)serv).GetDefaultCategory(uofw);

                        if (cat != null)
                        {
                            return new JsonNetResult(new
                            {
                                Category = new
                                {
                                    ID = cat
                                }
                            });
                        }
                    }

                    return new JsonNetResult(null);
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = e.ToStringWithInner()
                });
            }
        }

        private ViewModelConfig GetCheckedViewModelConfig(string mnemonic)
        {
            if (mnemonic == null)
                throw new HttpException(400, "mnemonic is null");

            var result = GetViewModelConfig(mnemonic);
            if (result == null)
                throw new HttpException(400, $"mnemonic [{mnemonic}] not found");

            return result;
        }

        private void CheckProperty(string mnemonic, string property)
        {
            var config = GetCheckedViewModelConfig(mnemonic);

            if (property == null)
                throw new HttpException(400, "property is null");

            if (config.TypeEntity.GetProperty(property) == null)
                throw new HttpException(400, $"property [{property}] not found");
        }


        public async Task<JsonNetResult> GetStrPropertyForFilter(string startswith, string mnemonic, string property, string filter = null)
        {
            IResponseDynamicQueryService dynamicSrv;

            try
            {
                dynamicSrv = this.GetService<IResponseDynamicQueryService>(mnemonic);
            }
            catch (InvalidCastException)
            {
                dynamicSrv = null;
            }

            //validation
            if (dynamicSrv == null)
                CheckProperty(mnemonic, property);

            using (var uofw = this.CreateUnitOfWork())
            {
                IQueryable q;

                if (dynamicSrv != null)
                {
                    q = dynamicSrv.GetAll(uofw, false, -1).Filter(this, uofw, GetViewModelConfig(mnemonic), filter);
                }
                else
                {
                    var bserv = this.GetService<IQueryService<object>>(mnemonic);

                    q = bserv?.GetAll(uofw).Filter(this, uofw, GetViewModelConfig(mnemonic), filter);
                }

                if (q == null) return null;

                startswith = startswith?.Trim();

                if (!IsNullOrEmpty(startswith))
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

                    string pattern = Format(startswith.Length == 1 ? @"\b{0}\S*" : @"\S*{0}\S*", startswith);

                    var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                    var res2 = (from str in res from Match match in regex.Matches(str) select match.Value).ToList();

                    return new JsonNetResult(res2.Distinct().Take(20));
                }
                else
                {
                    return
                        new JsonNetResult(
                            await q.Select("it." + property).Cast<string>().Distinct().Take(20).ToListAsync());
                }
            }
        }

        public async Task<JsonNetResult> GetUniqueValuesForProperty(string mnemonic, string property)
        {
            //validation
            IResponseDynamicQueryService dynamicSrv;

            try
            {
                dynamicSrv = this.GetService<IResponseDynamicQueryService>(mnemonic);
            }
            catch (InvalidCastException)
            {
                dynamicSrv = null;
            }

            //validation
            if (dynamicSrv == null)
                CheckProperty(mnemonic, property);

            var config = GetCheckedViewModelConfig(mnemonic);

            using (var uofw = CreateUnitOfWork())
            {
                IQueryable q;

                var objectId = -1;//TODO

                if (dynamicSrv != null)
                {
                    q = dynamicSrv.GetAll(uofw, false, objectId).Filter(this, uofw, GetViewModelConfig(mnemonic));
                }
                else
                {
                    var bserv = this.GetService<IQueryService<object>>(mnemonic);

                    q = bserv?.GetAll(uofw).Filter(this, uofw, GetViewModelConfig(mnemonic));
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
                        new JsonNetResult(
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

                        return new JsonNetResult(await q.Select(select).Distinct().Take(200).ToListAsync());
                    }
                }

                return null;
            }
        }

        private void _Delete(IUnitOfWork uofw, string mnemonic, int id)
        {
            if (id == 0) return;

            var serv = this.GetService<IBaseObjectCrudService>(mnemonic);

            var obj = serv.Get(uofw, id);

            if (obj == null) return;

            serv.Delete(uofw, obj);
        }


        public async Task<JsonNetResult> GetBoPropertyForFilter(string startswith, string mnemonic, string ids = null,
            string extrafilter = null)
        {
            //validation
           var config = GetCheckedViewModelConfig(mnemonic);

            using (var uofw = this.CreateUnitOfWork())
            {
                IQueryable q;
                IResponseDynamicQueryService dynamicSrv;
                var objectId = -1;//TODO

                try
                {
                    dynamicSrv = this.GetService<IResponseDynamicQueryService>(mnemonic);
                }
                catch (InvalidCastException)
                {
                    dynamicSrv = null;
                }
                var bserv = this.GetService<IQueryService<object>>(mnemonic);

                if (dynamicSrv != null)
                {
                    q = dynamicSrv.GetAll(uofw, false, objectId).Filter(this, uofw, GetViewModelConfig(mnemonic));
                }
                else
                {
                    q = bserv?.GetAll(uofw).Filter(this, uofw, GetViewModelConfig(mnemonic));
                }

                if (q == null)
                    return null;

                string property = config.LookupPropertyForFilter;

                startswith = startswith?.Trim();
                var listSearchWords = startswith.Split(new char[] { ' ' });

                IList qIDs = new List<BaseObject>();

                if (!IsNullOrEmpty(ids))
                {
                    var arrIDs = ids.Split(';').Select(int.Parse).ToArray();

                    qIDs = await (dynamicSrv != null ? dynamicSrv.GetAll(uofw, false, objectId) : bserv.GetAll(uofw, hidden: null))
                        .Where("@0.Contains(ID)", arrIDs)
                        .Select(config.ListView, new string[] { property })
                        .ToListAsync();

                    q = q.Where("!@0.Contains(ID)", arrIDs);
                }

                if (!IsNullOrEmpty(startswith))
                {
                    startswith = "";

                    foreach (string word in listSearchWords.Take(3))
                    {
                        startswith += $" it.{property}.Contains(\"{word.Sanitize()}\") &&";
                    }

                    startswith = startswith.Substring(0, startswith.Length - 2);

                    q = q.Where(startswith);

                    //sanitization
                    var list = await q.Select(config.ListView, new[] { property }).Take(20).ToListAsync();

                    return new JsonNetResult(list.Cast<object>().Concat(qIDs.Cast<object>()).Distinct());
                }
                else
                {
                    var list = await q.Select(config.ListView, new[] { property }).Take(20).ToListAsync();

                    return new JsonNetResult(list.Cast<object>().Concat(qIDs.Cast<object>()).Distinct());
                }
            }
        }


        public async Task<JsonNetResult> FilterBaseObject_Read(string startswith, string mnemonicCollection,
            string property)
        {
            var config = GetCheckedViewModelConfig(mnemonicCollection);

            using (var uofw = this.CreateUnitOfWork())
            {
                property = property.Split('.')[0];

                var bserv = this.GetService<IQueryService<object>>(mnemonicCollection);
                IResponseDynamicQueryService dynamicSrv;
                var objectId = -1;

                try
                {
                    dynamicSrv = this.GetService<IResponseDynamicQueryService>(mnemonicCollection);
                }
                catch (InvalidCastException)
                {
                    dynamicSrv = null;
                }

                startswith = startswith?.Trim();

                var col =
                    UiFasade.GetColumns(mnemonicCollection)
                        .FirstOrDefault(x => x.PropertyName == property);

                if (col == null)
                    throw new HttpException(400, $"property [{property}] not found");

                string lookupProperty = col.ViewModelConfig.LookupProperty.Text;

                if (col.PropertyType.IsBaseCollection())
                {
                    var q =
                        dynamicSrv != null ? dynamicSrv.GetAll(uofw, false, objectId) : bserv.GetAll(uofw)
                            .Filter(this, uofw, config)
                            .Where("it." + property + ".Any()")
                            .SelectMany("it." + property);

                    if (typeof(IEasyCollectionEntry).IsAssignableFrom(col.PropertyType.GetGenericArguments()[0]))
                    {
                        q = q.Select("it.Object");
                    }

                    if (!IsNullOrEmpty(startswith))
                        //sanitization
                        q = q.Where($"it.{lookupProperty}.Contains(\"{startswith.Sanitize()}\")");

                    return
                        new JsonNetResult(
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
                            .Filter(this, uofw, config)
                            .Where("it." + property + " != null")
                            .Select("it." + property);

                    if (!IsNullOrEmpty(startswith))
                        //sanitization
                        q = q.Where($"it.{lookupProperty}.Contains(\"{startswith.Sanitize()}\")", startswith);

                    return
                        new JsonNetResult(
                            await q.Select(col.ViewModelConfig.ListView, new[] { lookupProperty })
                                .Distinct()
                                .OrderBy("it." + lookupProperty)
                                .Take(20)
                                .ToListAsync());
                }
            }
        }

        public PartialViewResult GetToolbarPreset(string mnemonic)
        {
            return PartialView("Toolbars/Preset");
        }

        [System.Web.Mvc.HttpPost]
        public JsonNetResult ChangeSortOrder(string mnemonic, int id, int posId)
        {
            try
            {
                var serv = this.GetService<IBaseObjectCrudService>(mnemonic);

                using (var unitOfWork = this.CreateTransactionUnitOfWork())
                {
                    var obj = serv.Get(unitOfWork, id);

                    serv.ChangeSortOrder(unitOfWork, obj, posId);

                    unitOfWork.Commit();

                    return new JsonNetResult(new
                    {
                        error = 0,
                        model = obj
                    });
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = 1,
                    message = e.Message
                });
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonNetResult ChangeCategory(string mnemonic, int id, int categoryID)
        {
            try
            {
                var serv = this.GetService<ICategorizedItemCrudService>(mnemonic);

                using (var unitOfWork = this.CreateTransactionUnitOfWork())
                {
                    serv.ChangeCategory(unitOfWork, id, categoryID);

                    unitOfWork.Commit();

                    return new JsonNetResult(new
                    {
                        error = 0,
                    });
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = 1,
                    message = e.Message
                });
            }
        }


        public ActionResult GetAccessObject(string mnemonic, int id)
        {
            try
            {
                using (var unitOfWork = this.CreateUnitOfWork())
                {
                    var config = this.GetViewModelConfig(mnemonic);

                    var item = this.SecurityService.GetObjectAccessItem(unitOfWork, config.TypeEntity, id);

                    if (item == null)
                    {
                        var bServ = this.GetService<IBaseObjectCrudService>(mnemonic);

                        var model = bServ.Get(unitOfWork, id);

                        if (model != null)
                            item = this.SecurityService.CreateAndSaveAccessItem(unitOfWork, model);
                    }

                    return JsonNet(new { model = item });
                }
            }
            catch (Exception e)
            {
                return JsonNet(new { error = e.ToStringWithInner() });
            }
        }


        public async Task<ActionResult> GetObjectCount(string mnemonic, bool allItems = true, int? categoryID = null)
        {
            var serv = this.GetService<IBaseObjectCrudService>(mnemonic);

            try
            {
                using (var unitOfWork = this.CreateUnitOfWork())
                {
                    IQueryable q;

                    if (categoryID.HasValue &&
                        serv.GetType().GetInterfaces().Contains(typeof(ICategorizedItemCrudService)))
                    {
                        q = allItems
                            ? ((ICategorizedItemCrudService)serv).GetAllCategorizedItems(unitOfWork,
                                categoryID.Value)
                            : ((ICategorizedItemCrudService)serv).GetCategorizedItems(unitOfWork,
                                categoryID.Value);
                    }
                    else
                    {
                        q = serv.GetAll(unitOfWork);
                    }

                    var config = this.GetViewModelConfig(mnemonic);

                    q = q.Filter(this, unitOfWork, config);

                    return new JsonNetResult(new { count = await q.CountAsync() });
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new { message = e.ToStringWithInner() });
            }
        }

        public ActionResult GetTypes(string search)
        {
            var configs = GetViewModelConfigs();

            if (!IsNullOrEmpty(search))
                configs = configs.Where(x => x.Title.StartsWith(search));

            return JsonNet(configs.GroupBy(x => x.TypeEntity)
                .Select(x => new
                {
                    ID = x.Key.GetTypeName(),
                    Text = x.First().Title
                }));
        }

        public ActionResult GetMnemonics(string search, string filter)
        {
            var configs = GetViewModelConfigs();

            if (!IsNullOrEmpty(search))
                configs = configs.Where(x => x.Title.StartsWith(search))
                    .Where(x => Base.Ambient.AppContext.SecurityUser.IsPermission(x.TypeEntity, TypePermission.Navigate));

            if (!IsNullOrEmpty(filter))
            {
                var ftype = Type.GetType(filter);

                if (ftype != null)
                    configs = configs.Where(x => x.TypeEntity == ftype || x.TypeEntity.IsSubclassOf(ftype));
            }

            return JsonNet(configs
                .Select(x => new
                {
                    ID = x.Mnemonic,
                    Text = $"{x.Mnemonic} : {x.Title}"
                }));
        }


        [System.Web.Mvc.HttpGet]
        public ActionResult GetPublishViewModelConfig(string mnemonic)
        {
            var config = this.GetViewModelConfig(mnemonic);

            if (config == null)
                return null;

            var res = new
            {
                Mnemonic = config.Mnemonic,
                TypeEntity = config.TypeEntity.GetTypeName(),
                Icon = config.Icon,
                LookupProperty = config.LookupProperty,
                Title = config.Title,
                SystemProperties =
                config.DetailView.Props.Where(x => x.IsSystemPropery).Select(x => new { PropertyName = x.PropertyName }),
                ListView = new
                {
                    Title = config.ListView.Title ?? config.Title,
                    Columns = config.ListView.Columns.Select(c => new
                    {
                        PropertyName = c.PropertyName,
                        Hidden = c.Hidden,
                        DataType = c.PropertyDataTypeName,
                        Type = c.ColumnType.IsEnum || c.ColumnType.IsBaseObject() ? c.ColumnType.GetTypeName() : null
                    })
                },
                DetailView = new
                {
                    Title = config.DetailView.Title ?? config.Title,
                    Width = config.DetailView.Width,
                    Height = config.DetailView.Height,
                    IsMaximaze = config.DetailView.IsMaximaze,
                    WizardName = config.DetailView.WizardName
                },
                Ext = new Dictionary<string, object>() { },
                Preview = config.Preview.Enable
            };

            var listViewCategorizedItem = config.ListView as ListViewCategorizedItem;

            if (listViewCategorizedItem != null)
            {
                res.Ext.Add("MnemonicCategory", listViewCategorizedItem?.MnemonicCategory);
            }

            if (config.Relations.Any())
                res.Ext.Add("Relations", config.Relations);

            return new JsonNetResult(res);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult GetUiEnum(string type)
        {
            try
            {
                var uienum = UiFasade.GetUiEnum(Type.GetType(type));

                return new JsonNetResult(new
                {
                    Type = uienum.Type,
                    Title = uienum.Title,
                    Values = uienum.Values.ToDictionary(x => x.Value, x => new
                    {
                        Value = x.Value,
                        Title = x.Title,
                        Color = x.Icon.Color ?? "#428bca",
                        Icon = x.Icon.Value ?? "mdi mdi-multiplication",
                    })
                });
            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = e.Message
                });
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult KendoUI_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public JsonNetResult GetEditor(string objectType, string propertyName)
        {
            var editor = UiFasade.GetEditors(objectType).Select(x => new
            {
                x.PropertyName,
                x.Title,
                x.Description,
                x.Visible,
                x.IsReadOnly,
                Enable = !x.IsReadOnly,
                x.IsRequired,
                x.IsLabelVisible,
                x.TabName
            }).FirstOrDefault(x => x.PropertyName == propertyName);

            return editor != null
                ? new JsonNetResult(editor)
                : new JsonNetResult(new { error = $"property [{propertyName}] is not found" });
        }

        public JsonNetResult GetColumn(string objectType, string propertyName)
        {
            var column = UiFasade.GetColumns(objectType).Select(x => new
            {
                x.PropertyName,
                x.Title,
                x.Visible,
                x.OneLine
            }).FirstOrDefault(x => x.PropertyName == propertyName);
            return column != null
                ? new JsonNetResult(column)
                : new JsonNetResult(new { error = $"property [{propertyName}] is not found" });
        }


        public JsonNetResult GetRelations(string mnemonic)
        {
            var config = GetViewModelConfig(mnemonic);

            var relations = _relationService.GetRelations(config.TypeEntity);

            return new JsonNetResult(relations);
        }

        public JsonNetResult CorrectMnemonic(string mnemonic, int id)
        {
            var config = GetCheckedViewModelConfig(mnemonic);

            if (!config.Relations.Any())
            {
                return new JsonNetResult(new { Mnemonic = mnemonic });
            }
            if (!typeof(IComplexKeyObject).IsAssignableFrom(config.TypeEntity))
            {
                return new JsonNetResult(new { error = "not supported" });
            }

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    IQueryable q = config.GetService<IQueryService<IComplexKeyObject>>().GetAll(uofw);


                    var m = q.Where("it.ID==@0", id).Select("it.ExtraID").FirstOrDefault();

                    if (m == null)
                    {
                        return new JsonNetResult(new { error = "not found" });
                    }

                    return new JsonNetResult(new { Mnemonic = m });
                }
            }
            catch (Exception)
            {
                return new JsonNetResult(new { error = "error" });
            }
        }

        public ActionResult Clone(string mnemonic, int id)
        {
            GetCheckedViewModelConfig(mnemonic);

            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var serv = GetService<IBaseObjectCrudService>(mnemonic);
                    var source = serv.Get(uow, id);
                    var dest = (BaseObject)Facade.AutoMapperCloner.Copy(source);
                    var clone = serv.Create(uow, dest);
                    return new JsonNetResult(new
                    {
                        model = GetViewModelConfig(mnemonic).DetailView.GetData(uow, serv, clone.ID)
                    });
                }
            }
            catch (Exception error)
            {
                return new JsonNetResult(new
                {
                    error = error.Message,
                    code = -1,
                });
            }
        }

        public async Task<JsonNetResult> KendoUI_SchedulerRead(DataSourceRequest request, string mnemonic,
            DateTime? start, DateTime? end, string filter)
        {
            var config = GetCheckedViewModelConfig(mnemonic);

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    if (!typeof(Base.IScheduler).IsAssignableFrom(config.TypeEntity))
                        return new JsonNetResult(new DataSourceResult()
                        {
                            Errors = "type is not IScheduler"
                        });

                    var serv = this.GetService<IQueryService<object>>(mnemonic);

                    if (serv == null)
                        return new JsonNetResult(new DataSourceResult()
                        {
                            Errors = "service not found"
                        });

                    IQueryable q = serv.GetAll(uofw);

                    if (start != null && end != null)
                    {
                        q = q.Where($"it.End >= @0 and it.Start <= @1", start.Value, end.Value);
                    }

                    q = q.Filter(this, uofw, config, filter);

                    return new JsonNetResult(await q.Select(config.ListView).ToDataSourceResultAsync(request, config));
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = e.Message,
                    code = -1,
                });
            }
        }

        public ActionResult GetPreviewTemplate(string mnemonic)
        {
            return PartialView("Builders/Preview", UiFasade.GetCommonPreview(mnemonic));
        }


        public class Page
        {
            public Page(IQueryable source, int? page, int? page_size, int def_page_size, int max_page_size)
            {
                var checked_page_size = CheckPageSize(page_size, def_page_size, max_page_size);
                var checked_index = CheckPageIndex(page);

                TotalCount = source.Count();

                Items =
                    source.Skip((checked_index - 1) * checked_page_size).Take(checked_page_size).AsEnumerable().ToList();

                PageIndex = checked_index;
                PageSize = checked_page_size;
            }

            public IReadOnlyCollection<object> Items { get; }

            public int TotalCount { get; }

            public int PageIndex { get; }

            public int PageSize { get; }

            public static int CheckPageSize(int? page_size, int def, int max)
            {
                if (page_size.HasValue && page_size > 1 && page_size < max)
                    return page_size.Value;

                return def;
            }

            public static int CheckPageIndex(int? page_index)
            {
                if (page_index.HasValue && page_index > 0)
                    return page_index.Value;

                return 1;
            }
        }

        public JsonNetResult GetPreviewExtendedData(string mnemonic, int id, string extended, int? page, int? pageSize)
        {
            var config = this.GetCheckedViewModelConfig(mnemonic);

            using (var uofw = CreateUnitOfWork())
            {
                if (!config.Preview.Enable)
                    return new JsonNetResult(new { error = $"preview is not provided for [{mnemonic}]" });

                var serv = GetService<IQueryService<object>>(mnemonic);

                if (serv == null)
                    return new JsonNetResult(new { error = $"service [{mnemonic}] not found" });

                try
                {
                    var data = config.Preview.GetPreviewExtendedData(uofw, serv, id, extended);

                    var temp = data.AsEnumerable().Cast<object>().ToList();
                    return new JsonNetResult(new Page(data, page, pageSize, 50, 50));
                }
                catch (Exception e)
                {
                    return
                        new JsonNetResult(
                            new { error = $"mnemonic: {mnemonic}; id: {id}; {extended}; error: {e.Message}" });
                }
            }
        }


        public JsonNetResult GetPreviewData(string mnemonic, int id)
        {
            //var layerConfig = _layerService.GetLayerConfig(mnemonic);
            //if (layerConfig != null)
            //{
            //    mnemonic = layerConfig.Mnemonic;
            //}

            ViewModelConfig config = this.GetCheckedViewModelConfig(mnemonic);

            using (var uofw = CreateUnitOfWork())
            {
                if (!config.Preview.Enable)
                    return new JsonNetResult(new { error = $"preview is not provided for [{mnemonic}]" });

                var serv = GetService<IQueryService<object>>(mnemonic);

                if (serv == null)
                    return new JsonNetResult(new { error = $"service [{mnemonic}] not found" });

                try
                {
                    return new JsonNetResult(config.Preview.GetData(uofw, serv, id));
                }
                catch (Exception e)
                {
                    return new JsonNetResult(new { error = $"mnemonic: {mnemonic}; id: {id}; error: {e.Message}" });
                }
            }
        }

        [HttpPost]
        public JsonNetResult Calculate(string mnemonic, BaseObject model)
        {
            using (var uow = CreateUnitOfWork())
            {
                var service = GetService<IBaseObjectCrudService>(mnemonic);

                var calcMethod = service.GetType().GetMethod("Calculate");
                if (calcMethod == null)
                    return new JsonNetResult(new { error = "Объект не расчитывается" });

                calcMethod.Invoke(service, new object[] { model, uow });

                return new JsonNetResult(model);
            }
        }


        public async Task<JsonNetResult> KendoUI_GanttRead(DataSourceRequest request, string mnemonic, DateTime? start,
            DateTime? end, string searchText, string filter)
        {
            var config = GetCheckedViewModelConfig(mnemonic);

            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    if (!typeof(IGantt).IsAssignableFrom(config.TypeEntity))
                        return new JsonNetResult(new DataSourceResult()
                        {
                            Errors = "type is not IGantt"
                        });

                    var serv = this.GetService<IQueryService<object>>(mnemonic);

                    if (serv == null)
                        return new JsonNetResult(new DataSourceResult()
                        {
                            Errors = "service not found"
                        });

                    IQueryable q = serv.GetAll(uofw);

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        q = q.Where($"it.Title.Contains(@0)", searchText);
                    }

                    if (start != null && end != null)
                    {
                        q = q.Where($"it.End >= @0 and it.Start <= @1", start.Value, end.Value);
                    }

                    q = q.Filter(this, uofw, config, filter);

                    return new JsonNetResult(await q.Select(config.ListView).ToDataSourceResultAsync(request, config));
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = e.Message,
                    code = -1,
                });
            }
        }


        public async Task<JsonNetResult> GetLookupPropertyValue(string mnemonic, int id)
        {
            var config = GetCheckedViewModelConfig(mnemonic);
            using (var uofw = CreateUnitOfWork())
            {
                var serv = this.GetService<IQueryService<object>>(mnemonic);
                var lookupProperty = config.LookupProperty.Text;
                if (serv == null)
                    return new JsonNetResult(new
                    {
                        error = "service not found"
                    });

                var result = await serv.GetAll(uofw)
                    .Where("it.ID =" + id)
                    .Select("it." + lookupProperty)
                    .Cast<string>()
                    .SingleOrDefaultAsync();

                return new JsonNetResult(new
                {
                    value = result
                });
            }
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult GetViewModelConfigByObjectType(string mnemonic, int id = 0)
        {           
            var config = this.GetViewModelConfig(mnemonic);

            if (config == null)
                return null;
            if (mnemonic == "Subject")
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    var serv = this.GetService<IBaseObjectCrudService>(mnemonic);
                    var model = serv.Get(uofw, id);
                    if (model is CorpProp.Entities.Subject.Subject)
                        return GetSocietyConfig(model);
                }

            }
            return StandartConfig(config);


        }

        public ActionResult ObjectIDConfig(ViewModelConfig config, int? id)
        {
            if (config == null)
                return null;
            var res = new
            {
                ObjectID = id,
                Mnemonic = config.Mnemonic,
                TypeEntity = config.TypeEntity.GetTypeName(),
                Icon = config.Icon,
                LookupProperty = config.LookupProperty,
                Title = config.Title,
                SystemProperties =
               config.DetailView.Props.Where(x => x.IsSystemPropery).Select(x => new { PropertyName = x.PropertyName }),
                ListView = new
                {
                    Title = config.ListView.Title ?? config.Title,
                    Columns = config.ListView.Columns.Select(c => new
                    {
                        PropertyName = c.PropertyName,
                        Hidden = c.Hidden,
                        DataType = c.PropertyDataTypeName,
                        Type = c.ColumnType.IsEnum || c.ColumnType.IsBaseObject() ? c.ColumnType.GetTypeName() : null
                    })
                },
                DetailView = new
                {
                    Title = config.DetailView.Title ?? config.Title,
                    Width = config.DetailView.Width,
                    Height = config.DetailView.Height,
                    IsMaximaze = config.DetailView.IsMaximaze,
                    WizardName = config.DetailView.WizardName
                },
                Ext = new Dictionary<string, object>() { },
                Preview = config.Preview.Enable
            };

            var listViewCategorizedItem = config.ListView as ListViewCategorizedItem;

            if (listViewCategorizedItem != null)
            {
                res.Ext.Add("MnemonicCategory", listViewCategorizedItem?.MnemonicCategory);
            }

            if (config.Relations.Any())
                res.Ext.Add("Relations", config.Relations);

            return new JsonNetResult(res);
        }

        public ActionResult StandartConfig(ViewModelConfig config)
        {
            if (config == null)
                return null;
            var res = new
            {
                Mnemonic = config.Mnemonic,
                TypeEntity = config.TypeEntity.GetTypeName(),
                Icon = config.Icon,
                LookupProperty = config.LookupProperty,
                Title = config.Title,
                SystemProperties =
               config.DetailView.Props.Where(x => x.IsSystemPropery).Select(x => new { PropertyName = x.PropertyName }),
                ListView = new
                {
                    Title = config.ListView.Title ?? config.Title,
                    Columns = config.ListView.Columns.Select(c => new
                    {
                        PropertyName = c.PropertyName,
                        Hidden = c.Hidden,
                        DataType = c.PropertyDataTypeName,
                        Type = c.ColumnType.IsEnum || c.ColumnType.IsBaseObject() ? c.ColumnType.GetTypeName() : null
                    })
                },
                DetailView = new
                {
                    Title = config.DetailView.Title ?? config.Title,
                    Width = config.DetailView.Width,
                    Height = config.DetailView.Height,
                    IsMaximaze = config.DetailView.IsMaximaze,
                    WizardName = config.DetailView.WizardName
                },
                Ext = new Dictionary<string, object>() { },
                Preview = config.Preview.Enable
            };

            var listViewCategorizedItem = config.ListView as ListViewCategorizedItem;

            if (listViewCategorizedItem != null)
            {
                res.Ext.Add("MnemonicCategory", listViewCategorizedItem?.MnemonicCategory);
            }

            if (config.Relations.Any())
                res.Ext.Add("Relations", config.Relations);

            return new JsonNetResult(res);
        }

        /// <summary>
        /// Возвращает конфиг для ДП, являющегося актуальным ОГ.
        /// </summary>
        /// <param name="subject">Деловой партнер.</param>
        /// <returns>Конфиг общества группы, если деловой партнер является актуальным ОГ, иначе - конфиг ДП.</returns>
        public ActionResult GetSocietyConfig(BaseObject subject)
        {
            var mnemonic = "Subject";            
            CorpProp.Entities.Subject.Subject subj = null;

            if (subject != null && subject is CorpProp.Entities.Subject.Subject)
                subj = subject as CorpProp.Entities.Subject.Subject;
            mnemonic = (subj != null && (subj.Society != null || subj.SocietyID != null)) ? "Society" : "Subject";
            var config = GetViewModelConfig(mnemonic);
            if (subj != null && subj.Society != null )
                return ObjectIDConfig(config, subj.Society.ID);
            return StandartConfig(config);
        }


        /// <summary>
        /// Возвращает конфиг наследуемой модели.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="model">Модель.</param>
        /// <returns>Конфиг.</returns>
        public Base.UI.ViewModal.ViewModelConfig GetInheritModelConfig(IUnitOfWork uow, BaseObject model)
        {
            Base.UI.ViewModal.ViewModelConfig config = null;
            try
            {
                if (model != null && model.BaseObjectType != null
                   && !String.IsNullOrEmpty(model.BaseObjectType.TypeName))
                {
                    var type = CorpProp.Helpers.TypesHelper.GetTypeByFullName(model.BaseObjectType.TypeName);
                    config = this.GetViewModelConfig(type);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return config;
        }



        [System.Web.Mvc.HttpGet]
        public ActionResult AddInComplex(string objectIds, int complexID)
        {
            string mess = "";   
            if (String.IsNullOrEmpty(objectIds))
                return new JsonNetResult(new { error = $"Ошибка при выборе объектов для добавления в ИК." });
            var oids = objectIds.Split(';');
            if (oids != null && oids.Length > 0 )
            {
                if (complexID != 0)
                {
                    using (var uofw = CreateUnitOfWork())
                    {
                        var serv = this.GetService<CorpProp.Services.Estate.InventoryObjectService>("InventoryObject");
                        foreach (var strId in oids)
                        {
                            int id = 0;
                            int.TryParse(strId, out id);
                            if (id != 0)
                            {
                                var item = serv.Get(uofw, id);
                                if (item != null)
                                {
                                    item.PropertyComplexID = complexID;
                                    serv.Update(uofw, item);
                                }
                                else
                                    return new JsonNetResult(new { error = $"Ошибка добавления в ИК: объекта с идентификатором {id} не существует." });
                            }                                                           
                        }                        
                        mess = "Объекты успешно добавлены в ИК";
                    }
                }
                else
                    return new JsonNetResult(new { error = $"Ошибка добавления в ИК: не передан идентификатор комплекса." });
            }
            else
                return new JsonNetResult(new { error = $"Выберите объекты для добавления в ИК!" });

            var res = new
            {
                message = mess
            };
            return new JsonNetResult(res);
        }


        /// <summary>
        /// Получает данные ОГи подразделения из профиля пользователя.
        /// </summary>
        /// <param name="id">ИД пользователя</param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult GetUserProfile(int? id)
        {
            SibUser sb = null;
            using (var uow = CreateUnitOfWork())
            {
                sb = uow.GetRepository<SibUser>().Filter(x => x.UserID == id).FirstOrDefault();
            }
            return new JsonNetResult(
                    new
                    {
                        SocietyName = (sb != null && !String.IsNullOrEmpty(sb.SocietyName)) ? sb.SocietyName : "Общество группы",
                        DeptName = (sb != null && !String.IsNullOrEmpty(sb.DeptName)) ? sb.DeptName : "Структурное подразделение"
                    });
        }
    }
}