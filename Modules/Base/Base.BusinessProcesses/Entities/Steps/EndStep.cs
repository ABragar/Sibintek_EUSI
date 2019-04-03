using System.Collections.Generic;

namespace Base.BusinessProcesses.Entities.Steps
{
    public class EndStep : Stage
    {
        public EndStep()
        {
            BaseOutputs = new List<Output>();
            StepType = FlowStepType.EndStep;
        }       
    }
}
