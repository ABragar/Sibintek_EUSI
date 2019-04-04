using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.CorpProp.ImportExport;
using WebUI.Models.Social;

namespace WebUI.Controllers
{
    public class ToolbarController : BaseController
    {
        /// <summary>
        /// возвращает тулбар с кнопкой экпорт в формате ЛДН
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        //[Route("ExportLnd")]
        public ActionResult GetExportLndToolbar(string mnemonic, int objectId)
        {
            var model = new ExportExcelVm() { Mnemonic = mnemonic, Id = objectId };
            return PartialView("~/Views/Toolbar/_ExportLndToolbar.cshtml", model);
        }

        public ActionResult GetExportSocietyGraphsToolbar()
        {
            return PartialView("~/Views/Toolbar/_ExportSocietyGraphsToolbar.cshtml");
        }

        public ToolbarController(IBaseControllerServiceFacade serviceFacade) : base(serviceFacade)
        {

        }
    }
}