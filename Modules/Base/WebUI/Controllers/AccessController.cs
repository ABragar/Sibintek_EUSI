using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AccessController : BaseController
    {
        public AccessController(IBaseControllerServiceFacade serviceFacade) : base(serviceFacade)
        {
        }

        public async Task<ActionResult> GetAccessEntry(string mnemonic, int objid)
        {
            return JsonNet(new { error = "not implemented" });
        } 
    }
}   