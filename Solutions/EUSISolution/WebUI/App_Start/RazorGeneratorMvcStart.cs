using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(WebUI.RazorGeneratorMvcStart), "Start")]

namespace WebUI {
    public static class RazorGeneratorMvcStart {
        public static void Start() {
            var engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly) {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
            };
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);            

            // StartPage lookups are done by WebPages. 
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}
