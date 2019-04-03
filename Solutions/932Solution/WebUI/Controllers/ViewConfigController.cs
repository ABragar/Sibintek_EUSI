using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.UI.DetailViewSetting;
using Base.UI.Service;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class ViewConfigController : BaseController
    {
        private readonly IDvSettingService<DvSettingForType> _dvSettingService;

        public ViewConfigController(IBaseControllerServiceFacade serviceFacade, IDvSettingService<DvSettingForType> dvSettingService) : base(serviceFacade)
        {
            _dvSettingService = dvSettingService;
        }

        public JsonNetResult CheckDvSetting(DvSettingForType setting)
        {
            var result = _dvSettingService.ValidateSetting(setting);

            return new JsonNetResult(new {errors = result});
        }

        public PartialViewResult GetDvSettingForTypeToolbar()
        {
            return PartialView("~/Views/UI/DetailView/_Toolbar_DvSettingForType.cshtml");
        }
    }
}