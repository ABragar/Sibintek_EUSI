using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class CustomController : BaseController
    {

        public CustomController(IBaseControllerServiceFacade baseServiceFacade) : base(baseServiceFacade)
        {

        }


        public ActionResult GetDraftOSToolbar()
        {
            return PartialView("~/Views/Custom/DraftOS/_DraftOSToolbar.cshtml");
        }
    }
}