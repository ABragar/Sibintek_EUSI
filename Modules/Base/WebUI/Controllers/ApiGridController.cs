using Base;
using Base.ComplexKeyObjects.Common;
using Base.DAL;
using Base.Links.Service.Abstract;
using Base.Security.ObjectAccess;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.Service.Crud;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Extensions;
using Base.UI.Filter;
using Base.UI.Presets;
using Base.UI.Service;
using Base.UI.Service.Abstract;
using Base.UI.ViewModal;
using Base.Utils.Common;
using Base.Utils.Common.Caching;
using CorpProp.Entities.Subject;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using CorpProp.Services.DocumentFlow;
using WebUI.Extensions;

namespace WebUI.Controllers
{
    public class ApiGridController : ApiController
    {
        private readonly IBaseControllerServiceFacade _serviceFacade;
        private readonly ILinkItemService _link_item_service;
        private readonly IViewModelConfigService _view_model_config_service;
        private readonly IUnitOfWorkFactory _unit_of_work_factory;
        private readonly ISecurityService _security_service;
        private readonly DealCurrencyConversionService _converisonService;

        private readonly IDvSettingService<DvSettingForType> _dvSettingService;
        private readonly ITypeRelationService _relationService;
        private readonly IDvSettingManager _dvSettingManager;
        private readonly IPresetService<GridPreset> _gridPresetService;
        private readonly IPresetService<MenuPreset> _menuPresetService;
        private readonly IMnemonicFilterService<MnemonicFilter> _mnemonicFilterService;
        public ISimpleCacheWrapper CacheWrapper => _serviceFacade.CacheWrapper;


        public ApiGridController(IBaseControllerServiceFacade baseServiceFacade
            , ITypeRelationService relationService
            , IDvSettingService<DvSettingForType> dvSettingService
            , IDvSettingManager dvSettingManager
            , IPresetService<GridPreset> gridPresetService
            , IPresetService<MenuPreset> menuPresetService
            , IMnemonicFilterService<MnemonicFilter> mnemonicFilterService
            , IViewModelConfigService view_model_config_service
            , IUnitOfWorkFactory unit_of_work_factory
            , ILinkItemService link_item_service
            , ISecurityService security_service
            , IBaseControllerServiceFacade serviceFacade
            , DealCurrencyConversionService converisonService)            
        {
            _relationService = relationService;
            _dvSettingService = dvSettingService;
            _dvSettingManager = dvSettingManager;
            _gridPresetService = gridPresetService;
            _menuPresetService = menuPresetService;
            _mnemonicFilterService = mnemonicFilterService;
            _view_model_config_service = view_model_config_service;
            _unit_of_work_factory = unit_of_work_factory;
            _link_item_service = link_item_service;
            _security_service = security_service;
            _serviceFacade = serviceFacade;
            _converisonService = converisonService;
        }

        public ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            ViewModelConfig _config;             
            _config = _view_model_config_service.Get(mnemonic);
            return _config;
        }
        
        [System.Web.Http.HttpGet]
        public async Task<DataSourceResult> ReadSource(
            [System.Web.Http.ModelBinding.ModelBinder(typeof(WebApiDataSourceRequestModelBinder))] DataSourceRequest request
             , string mnemonic
             , string[] columns
             , int? categoryID = null
             , bool? allItems = null
             , string mnemonicFilterId = null
             , string searchStr = null
             , string extrafilter = null
             , int? targetedCurrencyId = null
             , DateTime? exchangeDate = null
            )
        {
            //TODO: разобраться почему в запросе есть колонки, но в параметре они не приходят
            var serv = this.GetService<IQueryService<object>>(mnemonic);
            try
            {
                using (var uofw = _unit_of_work_factory.CreateTransaction())
                {
                    var config = this.GetViewModelConfig(mnemonic);

                    IQueryable q;

                    if (mnemonic == "DealCurrencyConversion")
                    {
                        if(!targetedCurrencyId.HasValue) throw new ArgumentException("Отсутствует идентификатор валюты.");
                        if(!exchangeDate.HasValue) throw new ArgumentException("Отсутствует дата пересчёта.");
                        q = ((IDealCurrencyConversionService)serv).GetAll(uofw, targetedCurrencyId.Value, exchangeDate.Value);
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
                    q = q.FilterApiGrid(this, uofw, config, mnemonicFilter);

                    q = q.FullTextSearch(searchStr, CacheWrapper);

                    return await q.Select(config.ListView, columns).ToDataSourceResultAsync(request, config);
                }
            }
            catch (Exception e)
            {
                var res = new DataSourceResult()
                {
                    Errors = e.Message
                };

                return res;
            }
        }

       
                
        //[System.Web.Http.HttpGet]
        //public object Get(string mnemonic, int id, string cache = null)
        //{        

        //    try
        //    {
        //        using (var uofw = _unit_of_work_factory.CreateTransaction())
        //        {
        //            var obj = Get(uofw, mnemonic, id);
        //            return Ok(new
        //            {
        //                model = obj,
        //                access = GetObjAccess(uofw, obj.GetType(), id),
        //            });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { error = e.Message });
        //    }
        //}
      




        public T GetService<T>(string mnemonic) where T : IService
        {
            var config = GetViewModelConfig(mnemonic);

            if (config == null)
                throw new Exception($"config [{mnemonic}] not found");

            return config.GetService<T>();
        }

        private object Get(IUnitOfWork uofw, string mnemonic,int id)            
        {
            ViewModelConfig _config = _view_model_config_service.Get(mnemonic);
            var serv = _config.GetService<IBaseObjectCrudService>();            
            var model = serv.Get(uofw, id);

            if (model == null)
                throw new Exception("Объект не найден");

            return model;
        }
        private object GetObjAccess(IUnitOfWork uofw, Type t, int id)
        {
            if (typeof(IAccessibleObject).IsAssignableFrom(t))
            {
                var access = _security_service.GetAccessType(uofw, t, id);

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


    }

}