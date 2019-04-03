using System.Collections.Generic;

namespace Base.UI.ViewModal
{
    public class WizardDetailView : DetailView
    {
        public WizardDetailView()
        {
            Type = DetailViewType.WizzardView;
            Steps = new List<WizzardStep>();
        }

        public string FirstStep { get; set; }
        public bool HasSummary { get; set; }
        public List<WizzardStep> Steps { get; set; }

        public override T Copy<T>(T view = null)
        {
            var dv = view as WizardDetailView ?? new WizardDetailView();

            dv.FirstStep = this.FirstStep;
            dv.HasSummary = this.HasSummary;

            foreach (var step in Steps)
            {
                dv.Steps.Add(step.Copy());
            }

            return base.Copy(dv as T);
        }
    }
}