using Base;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class WizardController : BaseController
    {
        public WizardController(IBaseControllerServiceFacade baseServiceFacade) : base(baseServiceFacade) { }


        public ActionResult GetViewModel(string mnemonic, TypeDialog typeDialog, int id = 0)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_BuilderViewModel", new StandartDialogViewModel(this, mnemonic, typeDialog));
            }

            ViewBag.ID = id;
            ViewBag.AutoBind = true;

            return View("_BuilderViewModel", new StandartDialogViewModel(this, mnemonic, typeDialog));
        }


        public ActionResult GetSummary(string mnemonic, string path, BaseObject model)
        {
            return PartialView(path, model);
        }
    }
}
