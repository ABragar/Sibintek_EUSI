using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;
using Base.Attributes;
using Base.UI;
using Base.UI.ViewModal;
using WebUI.Extensions;

namespace WebUI.Helpers
{
    public class EditorViewEngine : RazorViewEngine
    {
        public EditorViewEngine()
        {

            ViewLocationFormats = new[]
            {
                "~/Views/{0}.cshtml"
            };

            PartialViewLocationFormats = ViewLocationFormats;
            FileExtensions = new[] { "cshtml" };
        }

        private readonly ConcurrentDictionary<string, string> routes = new ConcurrentDictionary<string, string>();

        public string FindPartial(ControllerContext controllerContext, EditorViewModel editor, ViewType vType)
        {

            var dirName = (editor.ViewModelConfig ?? editor.ParentViewModelConfig).GetDirName();
            var editorMode = editor.IsReadOnly ? "Display" : "Editor";
            var viewType = vType.ToString();
            var templateName = editor.EditorTemplate;

            var template = FindPartial(controllerContext, dirName, editorMode, viewType, templateName);

            return template;
        }
        public string FindPartial(ControllerContext controllerContext, CommonPreview preview, string templateName, ViewType vType)
        {
            var dirName = preview.ViewModelConfig.GetDirName();
            var suffix = preview.ViewModelConfig.GetName();

            var viewType = vType.ToString();
            var template = FindPartial(controllerContext, dirName, "Display", viewType, templateName, suffix);

            return template;
        }
        public string FindPartial(ControllerContext controllerContext, PreviewField editor, ViewType vType)
        {

            var dirName = (editor.ViewModelConfig ?? editor.ParentViewModelConfig).GetDirName();
            var viewType = vType.ToString();
            var templateName = editor.PropertyDataTypeName;

            var template = FindPartial(controllerContext, dirName, "Display", viewType, templateName);

            return template;
        }

        public string FindPartial(ControllerContext controllerContext, string dirName, string editorMode, string viewType, string templateName, string suffix = "")
        {
            if (templateName.StartsWith("Sib_"))
            {
                templateName = templateName.Split('_')[1];
                editorMode += "/SibEditors";
            }
            return routes.GetOrAdd($"{dirName}/{viewType}/{editorMode}/{templateName}/{suffix}",
                x => GetTemplateName(controllerContext, dirName, editorMode, viewType, templateName, suffix));
        }

        private string GetTemplateName(ControllerContext controllerContext, string dirName, string editorMode, string viewType, string templateName, string suffix = "")
        {
            var suffixViewLocations = new[]
            {
                $"{dirName}/{viewType}/{editorMode}/Common/{templateName}_{suffix}",
                $"{dirName}/{viewType}/{editorMode}/{templateName}_{suffix}",
                $"Standart/{viewType}/{editorMode}/Common/{templateName}_{suffix}",
                $"Standart/{viewType}/{editorMode}/{templateName}_{suffix}"
            };

            var viewLocations = new[] {
                $"{dirName}/{viewType}/{editorMode}/Common/{templateName}",
                $"{dirName}/{viewType}/{editorMode}/{templateName}",
                $"Standart/{viewType}/{editorMode}/Common/{templateName}",
                $"Standart/{viewType}/{editorMode}/{templateName}",
            };

            if (!string.IsNullOrEmpty(suffix))
            {
                viewLocations = suffixViewLocations.Concat(viewLocations).ToArray();
            }

            try
            {
                foreach (var url in viewLocations)
                {
                    var viewResult = FindPartialView(controllerContext, url, false);

                    if (viewResult.View != null)
                    {
                        return ((RazorView)viewResult.View).ViewPath;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

    }

    public enum ViewType
    {
        DetailView,
        ListView,
        Preview
    }
}