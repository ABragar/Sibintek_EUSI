using Base;
using Base.Entities.Complex;
using Base.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Base.Entities;
using Base.Helpers;
using Base.Reporting;
using Base.Security;
using Base.Settings;
using Base.UI.Enums;
using Base.UI.Helpers;
using Base.UI.Presets;
using Base.UI.Service;
using Base.UI.ViewModal;
using WebUI.Controllers;

namespace WebUI.Helpers
{
    /// <summary>
    /// Отрисовка полей и задание атрибутов.
    /// </summary>
    public static class HtmlHelpers
    {
        //private static int _increment;

        public static string CreateSystemName(this HtmlHelper htmlHelper, string prefix)
        {
            return UIHelper.CreateSystemName(prefix); //$"{prefix}_sys_{unchecked((uint)Interlocked.Increment(ref _increment))}";
        }

        public static CommonEditorViewModel GetCommonEditor(this HtmlHelper htmlHelper, string mnemonic)
        {
            var baseController = htmlHelper.ViewContext.Controller as IBaseController;

            return baseController?.UiFasade.GetCommonEditor(mnemonic);
        }

        public static ViewModelConfig GetViewModelConfig(this HtmlHelper htmlHelper, string mnemonic)
        {
            var baseController = htmlHelper.ViewContext.Controller as IBaseController;

            return baseController?.GetViewModelConfig(mnemonic);
        }

        public static IEnumerable<ViewModelConfig> GetViewModelConfigs(this HtmlHelper htmlHelper)
        {
            var baseController = htmlHelper.ViewContext.Controller as IBaseController;

            return baseController != null ? baseController.GetViewModelConfigs() : new List<ViewModelConfig>();
        }
        /// <summary>
        /// Создание html - атрибутов для поля из модели
        /// </summary>
        /// <param name="htmlHelper">Хелпер</param>
        /// <param name="eViewModel">Модель</param>
        /// <param name="htmlAttributes">Атрибуты</param>
        /// <param name="textbinding"></param>
        /// <returns>Перечень атрибутов ключ-значение</returns>
        public static IDictionary<string, object> CreateHtmlAttributes(this HtmlHelper htmlHelper, EditorViewModel eViewModel, object htmlAttributes = null, bool textbinding = false)
        {

            if (eViewModel == null)
            {
                throw new Exception("EditorViewModel is null");
            }

            var attributes = InitAttributes(htmlAttributes);

            var type = eViewModel.EditorType;

            string propertyName = eViewModel.PropertyName;

            if (!textbinding)
            {
                if (!attributes.ContainsKey("data-bind"))
                {
                    if (type == typeof(bool) || type == typeof(bool?))
                        attributes.Add("data-bind", String.Format("checked:{0}", propertyName));
                    else
                        attributes.Add("data-bind", String.Format("value:{0}", propertyName));
                }

                if (eViewModel.IsReadOnly)
                {
                    if (attributes.ContainsKey("class"))
                    {
                        attributes["class"] = attributes["class"] + " k-state-disabled";
                    }
                    else
                    {
                        attributes.Add("class", "k-state-disabled");
                    }

                    attributes.Add("disabled", "disabled");
                }
                else
                {
                    //Обработка обязательнх полей.
                    var parentObj = eViewModel.ParentViewModelConfig;
                    string mnemonic = parentObj.Mnemonic;
                    bool isTemplate = (mnemonic.ToLower() == "sibtasktemplate" || mnemonic.ToLower() == "sibprojecttemplate");
                    bool isLookup = parentObj.LookupProperty.Text == eViewModel.PropertyName;

                    if ((eViewModel.IsRequired && !isTemplate) || (eViewModel.IsRequired && isLookup && isTemplate))
                    {
                        attributes.Add("required", true);
                        attributes.Add("validationMessage", "Обязательное поле");
                    }
                }

            }
            else
            {
                if (!attributes.ContainsKey("data-bind"))
                {
                    attributes.Add("data-bind", String.Format("text:{0}", propertyName));
                }

            }

            return attributes;
        }

        public static HtmlString JsonObject(this HtmlHelper htmlHelper, object obj)
        {
            return new HtmlString($"{Json.Encode(obj)};");
        }

        public static HtmlString JsonObjectRaw(this HtmlHelper htmlHelper, object obj)
        {
            return new HtmlString($"{Json.Encode(obj)}");
        }

        public static HtmlString JsonObject(this HtmlHelper htmlHelper, string objectName, object obj)
        {
            return new HtmlString($"var {objectName} = {JsonObject(htmlHelper, obj)}");
        }

        public static HtmlString JsonObjectRaw(this HtmlHelper htmlHelper, string objectName, object obj)
        {
            return new HtmlString($"var {objectName} = {JsonObjectRaw(htmlHelper, obj)}");
        }

        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, EditorViewModel eViewModel, object htmlAttributes = null)
        {
            var tag = new TagBuilder("input");

            var attributes = htmlHelper.CreateHtmlAttributes(eViewModel, htmlAttributes);

            attributes.Add("id", eViewModel.UID);
            attributes.Add("type", "checkbox");
            attributes.Add("class", "k-checkbox");



            tag.MergeAttributes(new RouteValueDictionary(attributes));

            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString TextBox(this HtmlHelper htmlHelper, EditorViewModel eViewModel, object htmlAttributes = null)
        {
            var tag = new TagBuilder("input");

            var attributes = htmlHelper.CreateHtmlAttributes(eViewModel, htmlAttributes);

            if (attributes.ContainsKey("class"))
                attributes["class"] = attributes["class"] + " k-textbox";
            else
                attributes.Add("class", "k-textbox");

            tag.MergeAttributes(new RouteValueDictionary(attributes));

            return tag.ToMvcHtmlString(eViewModel);
        }

        public static MvcHtmlString TextArea(this HtmlHelper htmlHelper, EditorViewModel eViewModel, object htmlAttributes = null)
        {
            var tag = new TagBuilder("textarea");

            var attributes = htmlHelper.CreateHtmlAttributes(eViewModel, htmlAttributes);

            if (attributes.ContainsKey("class"))
                attributes["class"] = attributes["class"] + " k-textbox";
            else
                attributes.Add("class", "k-textbox");


            tag.MergeAttributes(new RouteValueDictionary(attributes));

            return tag.ToMvcHtmlString(eViewModel);
        }

        private static MvcHtmlString ToMvcHtmlString(this TagBuilder tag, EditorViewModel eViewModel)
        {
            if (eViewModel.IsRequired)
            {
                string name = eViewModel.PropertyName;

                tag.MergeAttribute("id", eViewModel.UID);
                tag.MergeAttribute("name", name);

                var validationmsg = new TagBuilder("span");

                validationmsg.AddCssClass("k-invalid-msg");
                validationmsg.MergeAttribute("data-for", name);

                return MvcHtmlString.Create(tag.ToString() + validationmsg.ToString());
            }

            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString Span(this HtmlHelper htmlHelper, EditorViewModel eViewModel, object htmlAttributes = null)
        {
            var attributes = InitAttributes(htmlAttributes);

            if (!attributes.ContainsKey("data-bind"))
            {
                attributes.Add("data-bind", String.Format("text:{0}", eViewModel.PropertyName));
            }

            var tag = new TagBuilder("span");

            tag.MergeAttributes(new RouteValueDictionary(attributes));

            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString Action(this HtmlHelper htmlHelper, EditorViewModel eViewModel, object htmlAttributes = null)
        {
            var attributes = InitAttributes(htmlAttributes);

            string name = eViewModel.PropertyName;

            if (!attributes.ContainsKey("data-bind"))
            {
                attributes.Add("data-bind", "attr: { href: " + name + "}; text: " + name + "");
            }

            attributes.Add("target", "_blank");

            var tag = new TagBuilder("a");

            tag.MergeAttributes(new RouteValueDictionary(attributes));

            return MvcHtmlString.Create(tag.ToString());
        }

        private static MvcHtmlString RenderByJavascript(string jsCode, bool onDocReady = true)
        {
            string scriptid = "injscr_" + Guid.NewGuid().ToString("N");
            string job = string.Format("$('#{0}').replaceWith({1});", scriptid, jsCode);

            if (onDocReady)
            {
                job = string.Format("$(function(){{{0}}});", job);
            }

            return MvcHtmlString.Create($"<script id=\"{scriptid}\">{job}</script>");
        }

        public static MvcHtmlString UserState(this HtmlHelper htmlHelper, int? userID = null, CustomStatus customStatus = CustomStatus.Disconnected, string size = null, bool withDescription = false)
        {
            var jsParams = string.Format("{0}, {{status:'{1}',showDesc:{2},size:'{3}'}}",
                userID?.ToString() ?? "null",
                customStatus,
                withDescription.ToString().ToLower(),
                size ?? ""
            );

            return RenderByJavascript($"pbaAPI.getUserState({jsParams})");
        }

        public static MvcHtmlString MnemonicCounter(this HtmlHelper htmlHelper, string mnemonic)
        {
            return RenderByJavascript($"pbaAPI.getMnemonicCounter('{mnemonic}')");
        }

        public static MvcHtmlString MnemonicCounter(this HtmlHelper htmlHelper, params string[] mnemonics)
        {
            string jsParams = "[" + string.Join(",", mnemonics.Select(mnemonic => "'" + mnemonic + "'")) + "]";

            return RenderByJavascript($"pbaAPI.getCompoundMnemonicCounter({jsParams})");
        }

        private static Dictionary<string, object> InitAttributes(object htmlAttributes)
        {
            var attributes = htmlAttributes as Dictionary<string, object>;

            if (attributes == null)
            {
                attributes = new Dictionary<string, object>();

                if (htmlAttributes != null)
                {
                    foreach (var pr in htmlAttributes.GetType().GetProperties())
                    {
                        attributes.Add(pr.Name.Replace("_", "-"), pr.GetValue(htmlAttributes));
                    }
                }
            }

            return attributes;
        }


        public static string GetImageThumbnailSrc(this HtmlHelper helper, Guid imageID, ImageSize size, string defImage = "NoImage", string type = "",
            string scale = "", string anchor = "")
        {
            return
                $"/Files/GetImageThumbnail/{imageID.ToString("N")}?size={size}&defImage={defImage}&type={type}&anchor={anchor}";
        }

        public static string GetImageThumbnailSrc(this HtmlHelper helper, FileData image, ImageSize size, string defImage = "NoImage", string type = "",
            string scale = "", string anchor = "")
        {
            return GetImageThumbnailSrc(helper, image?.FileID ?? Guid.Empty, size, defImage, type, anchor);
        }

        public static string GetImageThumbnailSrc(this HtmlHelper helper, string id, ImageSize size, string defImage = "NoImage", string type = "",
           string scale = "", string anchor = "")
        {
            Guid guid;

            if (!Guid.TryParse(id, out guid))
            {
                guid = Guid.Empty;
            }

            return GetImageThumbnailSrc(helper, guid, size, defImage, type, anchor);
        }

        public static string RenderBool(this HtmlHelper htmlHelper, bool val)
        {
            return val.ToString().ToLower();
        }

        #region Editor search

        private static readonly EditorViewEngine _engine = new EditorViewEngine();

        public static MvcHtmlString PartialEditor(this HtmlHelper htmlHelper, EditorViewModel editor, ViewType viewType = ViewType.DetailView)
        {
            var url = _engine.FindPartial(htmlHelper.ViewContext, editor, viewType);

            return string.IsNullOrEmpty(url) ? new MvcHtmlString($"<h6 style=\"color: red;\">Cant't find editor for \"{editor.EditorTemplate}\"</h6>")
                : htmlHelper.Partial(url, editor);
        }

        public static void RenderPartialEditor(this HtmlHelper htmlHelper, EditorViewModel editor, ViewType viewType = ViewType.DetailView)
        {
            var url = _engine.FindPartial(htmlHelper.ViewContext, editor, viewType);

            if (!string.IsNullOrEmpty(url))
            {
                htmlHelper.RenderPartial(url, editor);
            }
            else
            {
                htmlHelper.RenderPartial($"<h6 style=\"color: red;\">Cant't find editor for \"{editor.EditorTemplate}\"</h6>");
            }
        }


        public static MvcHtmlString PartialEditor(this HtmlHelper htmlHelper, PreviewField editor, ViewType viewType = ViewType.Preview)
        {
            var url = _engine.FindPartial(htmlHelper.ViewContext, editor, viewType);

            return string.IsNullOrEmpty(url) ? new MvcHtmlString($"<h6 style=\"color: red;\">Cant't find editor for \"{editor.PropertyDataTypeName}\"</h6>")
                : htmlHelper.Partial(url, editor);
        }

        public static void RenderPartialEditor(this HtmlHelper htmlHelper, PreviewField editor, ViewType viewType = ViewType.Preview)
        {
            var url = _engine.FindPartial(htmlHelper.ViewContext, editor, viewType);

            if (!string.IsNullOrEmpty(url))
            {
                htmlHelper.RenderPartial(url, editor);
            }
            else
            {
                htmlHelper.RenderPartial($"<h6 style=\"color: red;\">Cant't find editor for \"{editor.PropertyDataTypeName}\"</h6>");
            }
        }


        public static MvcHtmlString PartialEditor(this HtmlHelper htmlHelper, CommonPreview preview, string templateName = "Form", ViewType viewType = ViewType.Preview)
        {
            var url = _engine.FindPartial(htmlHelper.ViewContext, preview, templateName, viewType);

            return string.IsNullOrEmpty(url) ? new MvcHtmlString($"<h6 style=\"color: red;\">Cant't find editor for \"{templateName}\"</h6>")
                : htmlHelper.Partial(url, preview);
        }

        public static void RenderPartialEditor(this HtmlHelper htmlHelper, CommonPreview preview, string templateName = "Form", ViewType viewType = ViewType.Preview)
        {
            var url = _engine.FindPartial(htmlHelper.ViewContext, preview, templateName, viewType);

            if (!string.IsNullOrEmpty(url))
            {
                htmlHelper.RenderPartial(url, preview);
            }
            else
            {
                htmlHelper.RenderPartial($"<h6 style=\"color: red;\">Cant't find editor for \"{templateName}\"</h6>");
            }
        }

        #endregion

        //TODO: устаревшие хелперы (не использовать)
        #region oldHelpers
        public static string GetImageSrc(this HtmlHelper helper, Guid imageID, int? width = null,
           int? height = null, string defImage = "NoImage", string type = "Crop", string anchor = null)
        {
            return
                $"/Files/GetImage/{imageID.ToString("N")}?width={width}&height={height}&defImage={defImage}&type={type}&anchor={anchor}";
        }
        //public static string GetImageSrc(this HtmlHelper helper, FileData image, int? width = null, int? height = null, string defImage = "NoImage", string type = "Crop", string anchor = null)
        //{
        //    return GetImageSrc(helper, image?.FileID ?? Guid.Empty, width, height, defImage, type, anchor);
        //}
        //public static string GetImageSrc(this HtmlHelper helper, string id, int? width = null, int? height = null,
        //    string defImage = "NoImage", string type = "Crop", string anchor = null)
        //{
        //    Guid guid;

        //    if (!Guid.TryParse(id, out guid))
        //    {
        //        guid = Guid.Empty;
        //    }
        //    return GetImageSrc(helper, guid, width, height, defImage, type, anchor);
        //}
        #endregion


        public static bool IsDebug(this HtmlHelper htmlHelper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        public enum ResourceType
        {
            Js = 0,
            Css = 1
        }

        public static IHtmlString Resource(this HtmlHelper htmlHelper, Func<object, dynamic> template, ResourceType ResType)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[ResType] != null) ((List<Func<object, dynamic>>)htmlHelper.ViewContext.HttpContext.Items[ResType]).Add(template);
            else htmlHelper.ViewContext.HttpContext.Items[ResType] = new List<Func<object, dynamic>>() { template };

            return new HtmlString(string.Empty);
        }

        public static IHtmlString RenderResources(this HtmlHelper htmlHelper, ResourceType ResType)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[ResType] != null)
            {
                var resources = (List<Func<object, dynamic>>)htmlHelper.ViewContext.HttpContext.Items[ResType];

                foreach (var resource in resources)
                {
                    if (resource != null) htmlHelper.ViewContext.Writer.Write(resource(null));
                }
            }

            return new HtmlString(string.Empty);
        }

        public static string GetStandartDetailView(this HtmlHelper htmlHelper, string viewName, bool isEditorView)
        {
            if (viewName.StartsWith("/"))
            {
                viewName = viewName.Substring(1);
            }

            if (!viewName.EndsWith(".cshtml"))
            {
                viewName += ".cshtml";
            }

            return $"~/Views/Standart/DetailView/{(isEditorView ? "Editor" : "Display")}/{viewName}";
        }

        private static AppSetting getAppSetting()
        {
            var service = DependencyResolver.Current.GetService<ISettingService<AppSetting>>();
            return service.Get();
        }

        #region PROJECT SETTING HELPERS (dashboard/logos/project name etc)

        public static Guid? GetDashboardImageGuid(this HtmlHelper htmlHelper)
        {
            return getAppSetting()?.DashboardImage?.FileID;
        }

        public static Guid? GetLogoGuid(this HtmlHelper htmlHelper)
        {
            return getAppSetting()?.Logo?.FileID;
        }

        public static string GetDashboardImageSrc(this HtmlHelper htmlHelper, string type = "crop", string anchor = null)
        {
            var id = GetDashboardImageGuid(htmlHelper);
            if (id == null || id == Guid.Empty)
                return null;
            return GetImageThumbnailSrc(htmlHelper, id.Value, ImageSize.XL, type: type, anchor: anchor);
        }

        public static string GetLogoLogInSrc(this HtmlHelper htmlHelper, string type = "contain", string anchor = null)
        {
            var id = getAppSetting()?.LogoLogIn?.FileID;
            if (id == null || id == Guid.Empty)
                return null;
            return GetImageThumbnailSrc(htmlHelper, id.Value, ImageSize.M, type: type, anchor: anchor);
        }

        public static string GetLogoSrc(this HtmlHelper htmlHelper, string type = "contain", string anchor = null)
        {
            var id = GetLogoGuid(htmlHelper);
            if (id == null || id == Guid.Empty)
                return null;
            return GetImageThumbnailSrc(htmlHelper, id.Value, ImageSize.M, type: type, anchor: anchor);
        }

        public static string GetMenuLogoSrc(this HtmlHelper htmlHelper, string type = "contain", string anchor = null)
        {
            var id = GetLogoGuid(htmlHelper);
            if (id == null || id == Guid.Empty)
                return null;
            return GetImageThumbnailSrc(htmlHelper, id.Value, ImageSize.XXS, type: type, anchor: anchor);
        }

        public static string GetProjectName(this HtmlHelper htmlHelper)
        {
            return getAppSetting()?.AppName;
        }

        public static string GetWelcomeMessage(this HtmlHelper htmlHelper)
        {
            return getAppSetting()?.WelcomeMessage;
        }

        public static string GetTelerikReportingService(this HtmlHelper htmlHelper)
        {
            var service = DependencyResolver.Current.GetService<ISettingService<ReportingSetting>>();
            return service.Get().Url;
        }
        
        //Если настройки не инициализированы, то задаем координаты Москвы
        public static string GetDefaultLatAsString(this HtmlHelper htmlHelper)
        {
            var lat = getAppSetting()?.MapPosition.Latitude;

            if (lat == null || lat.Value == 0)
                lat = 55.75229016000003;

            return lat.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetDefaultLongAsString(this HtmlHelper htmlHelper)
        {
            var lon = getAppSetting()?.MapPosition.Longitude;

            if (lon == null || lon.Value == 0)
                lon = 37.62062072753906;

            return lon.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static int GetDefaultZoom(this HtmlHelper htmlHelper)
        {
            var zoom = getAppSetting()?.MapPosition.Zoom;

            if (zoom == null || zoom.Value == 0)
                zoom = 8;

            return zoom.Value;
        }

        #endregion
    }
}