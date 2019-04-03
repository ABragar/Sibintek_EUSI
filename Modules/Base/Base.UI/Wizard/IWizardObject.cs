using System;
using System.Collections.Generic;

namespace Base.UI.Wizard
{
    public interface IWizardObject: IBaseObject
    {
        List<string> PreviousSteps { get; set; }
        string Step { get; set; }
        int StepCount { get; set; }
        bool IsCompleted { get; set; }

        bool HasSummary { get; set; }
    }
}
