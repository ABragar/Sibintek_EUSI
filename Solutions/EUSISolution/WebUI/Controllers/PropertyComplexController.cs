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
using WebUI.Concrete;
using CorpProp.Entities.Estate;
using Kendo.Mvc.Extensions;

namespace WebUI.Controllers
{
    public class PropertyComplexController : BaseController
    {
        private readonly IDvSettingService<DvSettingForType> _dvSettingService;
        private readonly ITypeRelationService _relationService;
        private readonly IDvSettingManager _dvSettingManager;
        private readonly IPresetService<GridPreset> _gridPresetService;
        private readonly IPresetService<MenuPreset> _menuPresetService;
        private readonly IMnemonicFilterService<MnemonicFilter> _mnemonicFilterService;

        public PropertyComplexController(IBaseControllerServiceFacade baseServiceFacade, ITypeRelationService relationService,
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

        public JsonNetResult PropertyComplex_TreeRead([DataSourceRequest] DataSourceRequest request, int? id, string mnemonic,
           int? categoryID, bool? allItems, string searchStr, string extrafilter)
        {
            var serv = this.GetService<IQueryService<PropertyComplex>>(mnemonic);

            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    IQueryable<PropertyComplex> q = null;                    
                    q = serv.GetAll(uofw);   
                    var config = this.GetViewModelConfig(mnemonic);
                    q = q.Filter(this, uofw, config, extrafilter) as IQueryable<PropertyComplex>;
                    //q = id != null ? q.Where($"it.ParentID == {id}") : q.Where("it.ParentID == null");
                    var list = q.Select(
                        obj => new
                        {
                            ID = obj.ID,
                            ParentID = obj.ParentID,
                            Name = obj.Name,
                            PropertyComplexKind = (obj.PropertyComplexKind == null) ? "" : obj.PropertyComplexKind.Name

                        }
                        ).ToList();
                    return new JsonNetResult(
                        list.ToTreeDataSourceResult(request,
                         e => e.ID,
                         e => e.ParentID
                        ));
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
    }
}