#if DEBUG
using Base.UI.Presets;
using Base.UI.Service;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{

    public class TestController : BaseController
    {
        private readonly IPresetService<MenuPreset> _menuPresetService;

        public TestController(IBaseControllerServiceFacade serviceFacade, IPresetService<MenuPreset> menuPresetService)
            : base(serviceFacade)
        {
            _menuPresetService = menuPresetService;
        }

        public async Task<ActionResult> Index()
        {
            return View(new BaseViewModel(this));
        }
    }
}
#endif