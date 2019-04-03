using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Service.Crud;
using WebUI.Models;
using static System.String;
using Base.UI.ViewModal;
using Base.UI.Presets;
using Base.UI.Service;

namespace WebUI.Controllers
{
    public class ViewController : BaseController
    {
        private readonly IPresetService<GridPreset> _gridPresetService;
        private readonly IPresetService<MenuPreset> _menuPresetService;

        public ViewController(IBaseControllerServiceFacade baseServiceFacade,
            IPresetService<GridPreset> gridPresetService, 
            IPresetService<MenuPreset> menuPresetService)
            : base(baseServiceFacade)
        {
            _gridPresetService = gridPresetService;
            _menuPresetService = menuPresetService;
        }

        public async Task<ActionResult> Index(string mnemonic, int? parentID, int? currentID,
            TypeDialog typeDialog = TypeDialog.Frame, string filter = null, bool multiSelect = false)

        {
            mnemonic = mnemonic.Replace("-", ".");

            var model = new StandartDialogViewModel(this, mnemonic, typeDialog)
            {
                ParentID = parentID,
                CurrentID = currentID,
                SysFilter = filter,
                MultiSelect = multiSelect
            };

            await InitViewModel(model);

            if (Request.IsAjaxRequest())
            {
                return PartialView("/Views/Standart/Index.cshtml", model);
            }

            return View("/Views/Standart/Index.cshtml", model);
        }

        public async Task<ActionResult> Menu()
        {
            return PartialView("_SideBar", await _menuPresetService.GetAsync("Menu"));
        }

        public async Task<PartialViewResult> GetDialog(string mnemonic, int? parentID, int? currentID,
            TypeDialog typeDialog = TypeDialog.Frame, string searchStr = null, string filter = null,
            bool multiSelect = false, bool isReadOnly = false)
        {
            var model = new StandartDialogViewModel(this, mnemonic, typeDialog, isReadOnly)
            {
                ParentID = parentID,
                CurrentID = currentID,
                SearchStr = searchStr,
                SysFilter = filter,
                MultiSelect = multiSelect
            };

            await InitViewModel(model);

            return PartialView("/Views/Standart/Index.cshtml", model);
        }

        private async Task InitViewModel(StandartDialogViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (model.ViewModelConfig.ListView.Type == ListViewType.Grid ||
                model.ViewModelConfig.ListView.Type == ListViewType.GridCategorizedItem ||
                model.ViewModelConfig.ListView.Type == ListViewType.TreeListView)
            {
                model.Preset = await _gridPresetService.GetAsync(model.Mnemonic);
            }
        }

        public ActionResult GetEditorViewModel(string mnemonic, string member)
        {
            var editorVm = UiFasade.GetEditors(mnemonic).FirstOrDefault(x => x.PropertyName == member);

            if (editorVm != null)
                return PartialView("/Views/Standart/DetailView/Editor/EditorView.cshtml", editorVm);

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetDisplayViewModel(string mnemonic, string member)
        {
            var editorVm = UiFasade.GetEditors(mnemonic).FirstOrDefault(x => x.PropertyName == member);

            if (editorVm != null)
                return PartialView("/Views/Standart/DetailView/Display/DisplayView.cshtml", editorVm);

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetViewModel(string mnemonic, TypeDialog typeDialog, int id = 0)
        {
            ViewBag.ID = id;
            ViewBag.AutoBind = true;

            var model = new StandartDialogViewModel(this, mnemonic, typeDialog);

            if (Request.IsAjaxRequest())
            {
                return PartialView("/Views/Standart/Builders/DetailView.cshtml", model);
            }

            return View("/Views/Standart/Builders/DetailView.cshtml", model);
        }

        public PartialViewResult GetPartialViewModel(string mnemonic, TypeDialog typeDialog, int id = 0,
            bool autoBind = false, bool isReadOnly = false)
        {
            ViewBag.ID = id;
            ViewBag.AutoBind = autoBind;

            return PartialView("/Views/Standart/Builders/DetailView.cshtml",
                new StandartDialogViewModel(this, mnemonic, typeDialog, isReadOnly));
        }

        public PartialViewResult GetAjaxForm(int id, string mnemonic, bool readOnly = false)
        {
            using (var uofw = CreateUnitOfWork())
            {
                var serv = GetService<IBaseObjectCrudService>(mnemonic);

                var model = id != 0 ? serv.Get(uofw, id) : serv.CreateDefault(uofw);

                var commonEditorViewModel = UiFasade.GetCommonEditor(uofw, mnemonic, model);

                string partialName = Format(readOnly
                        ? "~/Views/Standart/DetailView/Display/Common/{0}.cshtml"
                        : "~/Views/Standart/DetailView/Editor/Common/{0}.cshtml",
                    "AjaxForm");

                return PartialView(partialName, new StandartFormModel(this)
                {
                    CommonEditorViewModel = commonEditorViewModel
                });
            }
        }

        public PartialViewResult GetToolbarPreset(string mnemonic)
        {
            return PartialView("/Views/Standart/Toolbars/Preset.cshtml");
        }

        public ActionResult GetPreviewTemplate(string mnemonic)
        {
            return PartialView("/Views/Standart/Builders/Preview.cshtml", UiFasade.GetCommonPreview(mnemonic));
        }

    }
}