using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WebUI.Areas.Account.Models;
using WebUI.Models;

namespace WebUI.Areas.Account.Helpers
{
    public static class HtmlHelpers
    {
        /// <returns>Класс иконки</returns>
        public static string GetSocialIcon(this HtmlHelper helper, string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                return null;
            }

            switch (providerName)
            {
                case "Vkontakte":
                    return "fa fa-vk";

                case "ESIA":
                    return "gosuslugi-icon";
                default:
                    return "fa fa-" + providerName.ToLower();
            }
        }



        public static IHtmlString InputFieldFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> property,
            string type)
        {
            return helper.EditorFor(property, "InputField", new {type });
        }



        public static IHtmlString Captcha<TModel>(this HtmlHelper<TModel> helper, string name,int width,int height)
        {

            var model = new CaptchaViewModel()
            {
                Name = name,
                ImgSrc = helper.ViewContext.GenerateCaptcha(name, width, height)
            };

            return helper.Partial("Captcha", model);
        }
    }




    public static class ContextExtensions
    {


        public static bool ValidateCaptcha(this Controller controller,string name)
        {

            var session = controller.HttpContext.Session?[name] as string;
            
            var form = controller.HttpContext.Request.Form[name];

            if (form == null)
            {
                controller.ModelState.AddModelError(name, "Поле не заполнено");
                return false;
            }

            if (form != session)
            {
                controller.ModelState.AddModelError(name, "Поле не верно заполнено");
                return false;
            }
            return true;
        }

        public static IHtmlString GenerateCaptcha(this ViewContext view_context, string name,int width, int height)
        {
            var captcha = new CaptchaModel();

            view_context.HttpContext.Session[name] = captcha.Result;

            return captcha.GenerateString(CaptchaModel.CaptchaType.Png, width, height, true);
        }
    }


    public class CaptchaViewModel
    {
        public string Name { get; set; }
        public IHtmlString ImgSrc { get; set; }
    }
}
