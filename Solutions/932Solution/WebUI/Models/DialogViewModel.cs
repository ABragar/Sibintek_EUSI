using Base;
using System;
using System.Threading;
using Base.Enums;
using Base.UI;
using Base.UI.ViewModal;
using WebUI.Controllers;
using WebUI.Extensions;

namespace WebUI.Models
{
    public abstract class DialogViewModel : BaseViewModel
    {
        private static int _increment;

        protected bool ReadOnly { get; set; }
        public string Mnemonic { get; protected set; }
        public TypeDialog Type { get; protected set; }
        public string DialogID { get; protected set; }

        public int? ParentID { get; set; }
        public int? CurrentID { get; set; }
        public string SysFilter { get; set; }
        public string SearchStr { get; set; }

        public bool MultiSelect { get; set; }

        public ViewModelConfig ViewModelConfig
        {
            get
            {
                var cfg =  UiFasade.GetViewModelConfig(Mnemonic);
                if(cfg == null)
                    throw new ArgumentNullException();
                return cfg;
            }
        }


        private string _nameDir = null;

        public string NameDir => _nameDir ?? (_nameDir = ViewModelConfig.GetDirName());

        public string GetDetailViewUrl(bool readOnly)
        {
            return ViewModelConfig.GetDetailViewUrl(readOnly);
        }

        public string GetPreviewUrl()
        {
            return ViewModelConfig.GetPreviewUrl();
        }

        public string GetToolbarListViewUrl()
        {
            return ViewModelConfig.GetToolbarListViewUrl();
        }

        public string GetContextMenuListViewUrl()
        {
            return ViewModelConfig.GetContextMenuListViewUrl();
        }

        public string GetListViewUrl()
        {
            return ViewModelConfig.GetListViewUrl(this);
        }

        public string GetSummaryUrl()
        {
            return ViewModelConfig.GetSummaryUrl();
        }

        public bool HasWizard => !string.IsNullOrEmpty(ViewModelConfig.DetailView.WizardName);

        public bool IsPermission(TypePermission typePermission)
        {
            return this.SecurityUser.IsPermission(this.ViewModelConfig.TypeEntity, typePermission);
        }

        public bool IsReadOnly => ReadOnly || ViewModelConfig.IsReadOnly || typeof(IReadOnly).IsAssignableFrom(ViewModelConfig.ServiceType);

        protected DialogViewModel(IBaseController controller)
            : base(controller)
        {
            var request = controller.HttpContext.Request;

            Mnemonic = request["mnemonic"];

            DialogID = request["_dialogid"];

            if (!String.IsNullOrEmpty(request["_dialogtype"]))
                Type = (TypeDialog)Enum.Parse(typeof(TypeDialog), request["_dialogtype"]);

            if (!String.IsNullOrEmpty(request["_parentid"]))
                ParentID = Int32.Parse(request["_parentid"]);

            if (!String.IsNullOrEmpty(request["_currentid"]))
                CurrentID = Int32.Parse(request["_currentid"]);
        }

        protected DialogViewModel(IBaseController controller, string mnemonic, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(controller)
        {
            Mnemonic = mnemonic;
            Type = type;

            ReadOnly = isReadOnly;

            DialogID = controller.HttpContext.Request["_dialogid"] ?? "dialog_" + Guid.NewGuid().ToString("N");
        }

        public DialogViewModel(BaseViewModel baseViewModel, string mnemonic, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(baseViewModel)
        {
            Mnemonic = mnemonic;
            Type = type;

            ReadOnly = isReadOnly;

            DialogID = $"dialog_{unchecked((uint)Interlocked.Increment(ref _increment))}";
        }
    }

    public abstract class Dialog_WidgetViewModel : DialogViewModel
    {
        private static int _increment;

        public string WidgetID { get; protected set; }

        protected Dialog_WidgetViewModel(IBaseController controller, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(controller, mnemonic, type, isReadOnly)
        {
            WidgetID = $"widget_{unchecked((uint)Interlocked.Increment(ref _increment))}";
            DialogID = dialogID;
        }

        protected Dialog_WidgetViewModel(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(baseViewModel, mnemonic, type, isReadOnly)
        {
            WidgetID = $"widget_{unchecked((uint)Interlocked.Increment(ref _increment))}";
            DialogID = dialogID;
        }

        protected Dialog_WidgetViewModel(DialogViewModel dialogViewModel)
            : base(dialogViewModel, dialogViewModel.Mnemonic, dialogViewModel.Type)
        {
            this.WidgetID = $"widget_{unchecked((uint)Interlocked.Increment(ref _increment))}";
            this.DialogID = dialogViewModel.DialogID;
            this.ParentID = dialogViewModel.ParentID;
            this.CurrentID = dialogViewModel.CurrentID;
            this.SysFilter = dialogViewModel.SysFilter;
            this.SearchStr = dialogViewModel.SearchStr;
            this.MultiSelect = dialogViewModel.MultiSelect;
            this.ReadOnly = dialogViewModel.IsReadOnly;
        }

        protected Dialog_WidgetViewModel(IBaseController controller)
            : base(controller)
        {
            var request = controller.HttpContext.Request;
            this.WidgetID = request["_widgetid"];
        }
    }

    public enum TypeDialog
    {
        Frame = 0,
        Modal = 1,
        Lookup = 2,
        Custom = 3,
    }
}