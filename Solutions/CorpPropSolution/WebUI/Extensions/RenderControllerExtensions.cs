using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace WebUI.Extensions
{
    public static class RenderControllerExtensions
    {

        public static string RenderPartialViewToString(this Controller controller,
            object model,
            ViewDataDictionary view_data = null)
        {
            return controller.RenderPartialViewToString(null, model, view_data);
        }

        public static string RenderPartialViewToString(this Controller controller, string view_name, object model, ViewDataDictionary view_data = null)
        {
            if (string.IsNullOrEmpty(view_name))
                view_name = controller.RouteData.GetRequiredString("action");

            view_data = new ViewDataDictionary(view_data ?? controller.ViewData)
            {
                Model = model
            };


            using (var string_writer = new StringWriter())
            {
                var view_engine_result = controller.ViewEngineCollection.FindPartialView(controller.ControllerContext, view_name);

                var viewContext = new ViewContext(controller.ControllerContext, view_engine_result.View, view_data, controller.TempData, string_writer);

                view_engine_result.View.Render(viewContext, string_writer);

                return string_writer.ToString();
            }


        }
    }
}