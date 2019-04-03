using System.Collections.Generic;
using Base.Attributes;
using Base.Entities;
using Newtonsoft.Json;

namespace Base.UI.Wizard
{
    public abstract class DecoratedWizardObject<TObject> : BaseObject, IWizardObject where TObject : BaseObject, new()
    {
        public abstract TObject GetObject();
        [DetailView(Visible = false)]
        public List<string> PreviousSteps { get; set; } = new List<string>();
        [DetailView(Visible = false)]
        public string Step { get; set; }
        [DetailView(Visible = false)]
        public int StepCount { get; set; }
        [DetailView(Visible = false)]
        public int Index { get; set; }
        [DetailView(Visible = false)]
        public bool IsCompleted { get; set; }
        [DetailView(Visible = false)]
        public bool HasSummary { get; set; }
    }
}