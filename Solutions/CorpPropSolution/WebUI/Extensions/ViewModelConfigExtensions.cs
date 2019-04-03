using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Base.DAL;
using Base.Service;
using Base.UI;
using Base.UI.ViewModal;
using WebUI.Controllers;
using WebUI.Models;

namespace WebUI.Extensions
{
    public static class ViewModelConfigExtensions
    {
        static string GetViewsDirectory()
        {
            return DependencyResolver.Current.GetService<IPathHelper>().GetViewsDirectory();
        }

        public static string GetName(this ViewModelConfig config)
        {
            return (config.Name ?? config.Mnemonic).Replace(".", "_").Replace("-", "_");
        }

        public static string GetDirName(this ViewModelConfig config)
        {
            return config.TypeEntity.Namespace
                .Replace("Base.", "")
                .Replace("Entities.", "")
                .Replace("Concrete.", "")
                .Replace("Steps.", "")
                .Split('.').First();
        }

        public static string GetDetailViewUrl(this ViewModelConfig config, bool readOnly)
        {
            string name = GetName(config);

            string dirName = GetDirName(config);

            string path = Path.Combine(GetViewsDirectory(), $@"{dirName}\_{name}.cshtml");

            //string pathDef = readOnly ? "~/Views/Standart/DetailView/Display/Common/Form.cshtml" : "~/Views/Standart/DetailView/Editor/Common/Form.cshtml";
            string pathDef = "~/Views/Standart/DetailView/Editor/Common/Form.cshtml";

            return File.Exists(path) ? $"~/Views/{dirName}/_{name}.cshtml" : pathDef;
        }

        public static string GetToolbarDetailViewUrl(this ViewModelConfig config)
        {
            string name = GetName(config);

            string dirName = GetDirName(config);

            string suffix = dirName != name ? $"_{name}" : "";

            string path = Path.Combine(GetViewsDirectory(), $@"{dirName}\_ToolbarDetailView{suffix}.cshtml");

            return File.Exists(path) ? $"~/Views/{dirName}/_ToolbarDetailView{suffix}.cshtml"
                : $"~/Views/Standart/Toolbars/DetailView.cshtml";
        }
        

        public static string GetPreviewUrl(this ViewModelConfig config)
        {
            string name = GetName(config);

            string dirName = GetDirName(config);

            string suffix = dirName != name ? $"_{name}" : "";

            string path = Path.Combine(GetViewsDirectory(), $@"{dirName}\_Preview{suffix}.cshtml");

            return File.Exists(path) ? $"~/Views/{dirName}/_Preview{suffix}.cshtml"
                : "~/Views/Shared/_PreviewDefault.cshtml";
        }

        public static string GetToolbarListViewUrl(this ViewModelConfig config)
        {
            string name = GetName(config);

            string dirName = GetDirName(config);

            string suffix = dirName != name ? $"_{name}" : "";

            string lvname = config.ListView.Type.ToString();                

            string path = Path.Combine(GetViewsDirectory(),
                $@"{dirName}\_Toolbar{lvname}{suffix}.cshtml");

            return File.Exists(path) ? $"~/Views/{dirName}/_Toolbar{lvname}{suffix}.cshtml"
                : $"~/Views/Standart/Toolbars/{lvname}.cshtml";
        }

        public static string GetContextMenuListViewUrl(this ViewModelConfig config)
        {
            string name = GetName(config);

            string dirName = GetDirName(config);

            string suffix = dirName != name ? $"_{name}" : "";

            string path = Path.Combine(GetViewsDirectory(),
                $@"{dirName}\_ContextMenuListView{suffix}.cshtml");

            return File.Exists(path) ? $"~/Views/{dirName}/_ContextMenuListView{suffix}.cshtml"
                : "~/Views/Standart/ContextMenus/ListView.cshtml";
        }

        public static string GetListViewUrl(this ViewModelConfig config, DialogViewModel dialogViewModel)
        {
            string lvname = config.ListView.Type.ToString();

            if (config.ListView.Type == ListViewType.GridCategorizedItem)
                lvname = "Grid";

            if (dialogViewModel.Type == TypeDialog.Lookup && config.ListView.Type == ListViewType.TreeListView)
                lvname = "Grid";

            string name = GetName(config);

            string dirName = GetDirName(config);

            string suffix = dirName != name ? $"_{name}" : "";

            string path = Path.Combine(GetViewsDirectory(), $@"{dirName}\_{lvname}{suffix}.cshtml");

            return File.Exists(path) ? $"~/Views/{dirName}/_{lvname}{suffix}.cshtml"
                : $"~/Views/Standart/ListViews/{lvname}.cshtml";
        }

        public static string GetSummaryUrl(this ViewModelConfig config)
        {
            string name = GetName(config);

            string dirName = GetDirName(config);

            string path = Path.Combine(GetViewsDirectory(), $@"{dirName}\_Summary{name}.cshtml");
            

            string pathDef = "~/Views/Wizard/_Summary.cshtml";

            return File.Exists(path) ? $"~/Views/{dirName}/_Summary{name}.cshtml" : pathDef;
        }
    }
}