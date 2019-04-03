using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{   
    public class ParallelizationStep : Step
    {
        public ParallelizationStep()
        {
            StepType = FlowStepType.ParalleizationStep;
        }

        [NotMapped]
        [DetailView(Name = "Действия", HideLabel = true, TabName = "[1]Действия")]
        public ICollection<Output> Outputs
        {
            get { return BaseOutputs.With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.ToList()); }
        }
    }
}

