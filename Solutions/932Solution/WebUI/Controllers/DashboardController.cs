using Base.UI;
using Base.UI.Presets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Base.UI.Dashboard;
using Base.UI.Service;
using WebUI.Authorize;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class DashboardController : BaseController
    {

        private readonly IPresetService<DashboardPreset> _dashboardPresetService;
        private readonly IDashboardService _dashboardService;

        public DashboardController(IBaseControllerServiceFacade baseServiceFacade, IPresetService<DashboardPreset> dashboardPresetService, IDashboardService dashboardService) : base(baseServiceFacade)
        {
            _dashboardPresetService = dashboardPresetService;
            _dashboardService = dashboardService;
        }

        public PartialViewResult GetPanelConfigs()
        {
            return PartialView("QuickAccessBarPanel", new BaseViewModel(this));
        }

        [ProfileFilter]
        public async Task<ActionResult> Index(string module = "Global")
        {

            var vm = new BaseViewModel(this);

            var dashboardVm = new DashboardVm()
            {
                Module = module,
                Widgets = _dashboardService.GetWidgetsForPreset(await _dashboardPresetService.GetAsync(module))
            };

            ViewBag.DashboardVm = dashboardVm;

            Response.SetCookie(new HttpCookie("dashboard", module));


            if (Request.IsAjaxRequest())
            {
                return PartialView(vm);
            }

            return View(vm);
        }

        public async Task<JsonNetResult> GetWidgets(string module)
        {
            if (Base.Ambient.AppContext.SecurityUser.IsAdmin)
                return new JsonNetResult(_dashboardService.GetWidgets());
            else
            {
                return new JsonNetResult(_dashboardService.GetWidgetsForPreset(await _dashboardPresetService.GetUserCategoryPresetAsync(module)));
            }
                
        }
    }
}
