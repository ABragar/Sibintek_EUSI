using Base.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.UI.ViewModal;
using Base.Utils.Common;
using WebUI.Controllers;

namespace WebUI.Models
{
    public class WizardDialogViewModel : DialogViewModel
    {
        public WizardDialogViewModel(IBaseController controller, string mnemonic, TypeDialog type = TypeDialog.Frame) : base(controller, mnemonic, type)
        {
        }

        public WizardDialogViewModel(BaseViewModel baseViewModel, string mnemonic, TypeDialog type = TypeDialog.Frame) : base(baseViewModel, mnemonic, type)
        {
        }
    }

    public class WizardFormModel : StandartViewModel
    {
        private readonly Dictionary<string, EditorViewModel> _editors;
        private readonly Dictionary<string, string> _props_dbl = new Dictionary<string, string>();
        private CommonEditorViewModel CommonEditorViewModel { get; set; }

        public WizardFormModel(DialogViewModel dialogViewModel, CommonEditorViewModel commonEditorViewModel)
            : base(dialogViewModel)
        {
            CommonEditorViewModel = commonEditorViewModel;
            FormName = "f_" + Guid.NewGuid().ToString("N");
            _editors = CommonEditorViewModel.Editors.ToDictionary(x => x.PropertyName, x => x);
        }

        public List<WizzardStep> Steps => ((WizardDetailView)ViewModelConfig.DetailView).Steps;

        public EditorViewModel GetEditor(string stepName, string propertyName)
        {
            var ed = _editors[propertyName];

            if (_props_dbl.ContainsKey(propertyName))
            {
                if (_props_dbl[propertyName] == stepName)
                    return ed;

                string key = $"{stepName}_{propertyName}";

                if (!_editors.ContainsKey(key))
                    _editors.Add(key, ObjectHelper.CreateAndCopyObject<EditorViewModel>(ed));
                    
                return _editors[key];
            }
            else
            {
                _props_dbl.Add(propertyName, stepName);
            }

            return ed;
        }

        public string FormName { get; set; }

        //public string CompleteText
        //{
        //    get { return ((WizardDetailView) ViewModelConfig.DetailView).CompleteText; }
        //}
    }

    public class StepViewModel
    {
        public StepViewModel()
        {
        }

        public StepViewModel(WizardFormModel view, WizzardStep step)
        {
            Wizard = view;
            Step = step;
        }

        public WizardFormModel Wizard { get; set; }
        public WizzardStep Step { get; set; }


      //  public bool IsComplete { get; set; }

        //public int StepIndex
        //{
        //    get { return Step.Index; }
        //}

        public string StepName => Step.Name;

        public string StepDescription => Step.Description;

        public string StepTitle => Step.Title;

        public ViewModelConfig ViewModelConfig => Wizard?.ViewModelConfig;

        public string DialogID => Wizard != null ? Wizard.DialogID : "";

        public string WidgetID => Wizard != null ? Wizard.WidgetID + "_" + StepName : "";

        public TypeDialog Type => Wizard?.Type ?? TypeDialog.Modal;

        public string FormName => Wizard != null ? Wizard.FormName + "_" + StepName : "";

        public string Mnemonic => Wizard != null ? Wizard.Mnemonic : "";

        private List<EditorViewModel> _editors;
        public List<EditorViewModel> Editors
        {
            get
            {
                if (_editors == null && Wizard != null)
                {
                    _editors = new List<EditorViewModel>();

                    foreach (var ed in Step.StepProperties.Select(pr => Wizard.GetEditor(StepName, pr.Name)))
                    {
                        _editors.Add(ed);
                    }
                }

                return _editors;
            }
        }

        public int StepCount => Wizard?.Steps.Count ?? 0;
    }

}