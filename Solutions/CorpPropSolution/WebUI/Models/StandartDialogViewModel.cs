using Base;
using Base.DAL;
using Base.UI;
using Base.UI.Presets;
using Base.UI.ViewModal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using Base.Helpers;
using Base.UI.Service;
using WebUI.Controllers;
using WebUI.Helpers;

namespace WebUI.Models
{
    public class StandartDialogViewModel : DialogViewModel
    {
        public StandartDialogViewModel(IBaseController controller)
            : base(controller)
        {
        }

        public StandartDialogViewModel(IBaseController controller, string mnemonic, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(controller, mnemonic, type, isReadOnly)
        {
        }

        public StandartDialogViewModel(BaseViewModel baseViewModel, string mnemonic, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(baseViewModel, mnemonic, type, isReadOnly)
        {
        }
    }

    public class StandartViewModel : Dialog_WidgetViewModel
    {
        public StandartViewModel(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(baseViewModel, mnemonic, dialogID, type, isReadOnly)
        {
        }

        public StandartViewModel(DialogViewModel dialogViewModel)
            : base(dialogViewModel)
        {
        }

        public StandartViewModel(IBaseController controller)
            : base(controller)
        {
        }
    }

    public class StandartFormModel : StandartViewModel
    {
        private static int _increment;

        public CommonEditorViewModel CommonEditorViewModel { get; set; }

        public StandartFormModel(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame, bool isReadOnly = false)
            : base(baseViewModel, mnemonic, dialogID, type, isReadOnly)
        {
        }

        public StandartFormModel(DialogViewModel dialogViewModel, CommonEditorViewModel commonEditorViewModel)
            : base(dialogViewModel)
        {
            this.CommonEditorViewModel = commonEditorViewModel;
            this.FormName = $"form_{unchecked((uint)Interlocked.Increment(ref _increment))}";
            
        }

        public StandartFormModel(IBaseController controller)
            : this(controller, controller.HttpContext.Request["mnemonic"])
        {
        }

        public StandartFormModel(IBaseController controller, string mnemonic)
            : base(controller)
        {
            this.CommonEditorViewModel = controller.UiFasade.GetCommonEditor(mnemonic);
            this.FormName = $"form_{unchecked((uint)Interlocked.Increment(ref _increment))}";
        }

        public StandartFormModel(IBaseController controller, CommonEditorViewModel commonEditorViewModel)
            : base(controller)
        {
            this.CommonEditorViewModel = commonEditorViewModel;
            this.FormName = $"form_{unchecked((uint)Interlocked.Increment(ref _increment))}";
        }

        public int TabsCount => this.CommonEditorViewModel.Tabs.Count;
        public List<TabVm> Tabs => this.CommonEditorViewModel.Tabs;


        public int GroupCount => CommonEditorViewModel.Groups.Count;
        public List<GroupVm> Groups => CommonEditorViewModel.Groups;

        public string FormName { get; set; }

        public BaseObject Model { get; set; }
    }

    public class StandartTreeView : Dialog_WidgetViewModel
    {
        public StandartTreeView(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(baseViewModel, mnemonic, dialogID, type)
        {
        }

        public StandartTreeView(DialogViewModel dialogViewModel)
            : base(dialogViewModel)
        {
        }
    }

    public class StandartGridView : Dialog_WidgetViewModel
    {
        public StandartGridView(IBaseController controller, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(controller, mnemonic, dialogID, type)
        {
        }

        public StandartGridView(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(baseViewModel, mnemonic, dialogID, type)
        {
        }

        public StandartGridView(DialogViewModel dialogViewModel)
            : base(dialogViewModel)
        {
        }

        public StandartGridView(IBaseController controller)
            : base(controller)
        {
        }
    }

    public class CustomDialogView : Dialog_WidgetViewModel
    {
        public CustomDialogView(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(baseViewModel, mnemonic, dialogID, type)
        {
        }

        public CustomDialogView(IBaseController controller, string mnemonic, string dialogID,
            TypeDialog type = TypeDialog.Frame)
            : base(controller, mnemonic, dialogID, type)
        {
        }

        public CustomDialogView(DialogViewModel dialogViewModel)
            : base(dialogViewModel)
        {
        }

        public CustomDialogView(IBaseController controller)
            : base(controller)
        {
        }
    }

    public class StandartScheduler : Dialog_WidgetViewModel
    {
        public StandartScheduler(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(baseViewModel, mnemonic, dialogID, type)
        {
        }

        public StandartScheduler(DialogViewModel dialogViewModel)
            : base(dialogViewModel)
        {
        }
    }

    public class StandartGantt : Dialog_WidgetViewModel
    {
        public StandartGantt(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(baseViewModel, mnemonic, dialogID, type)
        {
        }

        public StandartGantt(DialogViewModel dialogViewModel)
            : base(dialogViewModel)
        {
        }
    }

    public class StandartPivot : Dialog_WidgetViewModel
    {
        public StandartPivot(IBaseController controller, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(controller, mnemonic, dialogID, type)
        {
        }

        public StandartPivot(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame)
            : base(baseViewModel, mnemonic, dialogID, type)
        {
        }

        public StandartPivot(DialogViewModel dialogViewModel)
            : base(dialogViewModel)
        {
        }

        public StandartPivot(IBaseController controller)
            : base(controller)
        {
        }
    }

    public class StandartTreeListView : StandartGridView
    {
        public StandartTreeListView(DialogViewModel model)
            : base(model)
        {
        }
    }

}