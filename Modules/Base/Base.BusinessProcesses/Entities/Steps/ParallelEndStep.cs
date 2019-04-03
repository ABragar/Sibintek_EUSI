using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{
    public class ParallelEndStep : Step
    {
        public ParallelEndStep()
        {
            StepType = FlowStepType.ParallelEndStep;
        }


        [ListView]
        [DetailView(Name = "Ждать завершения всех потоков", Order = 3)]
        public bool WaitAllThreads { get; set; }

        [NotMapped]
        [DetailView(Visible = false)]
        public ICollection<Output> Outputs
        {
            get { return BaseOutputs.With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.ToList()); }
        }
    }
}