using Base.Attributes;
using Base.Macros.Entities;
using Base.Utils.Common.Maybe;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Base.BusinessProcesses.Entities.Steps
{

    public class ConditionalStep : Step
    {
        public ConditionalStep()
        {
            StepType = FlowStepType.ConditionalStep;
        }
        [NotMapped]
        [DetailView(Name = "Действия", HideLabel = true, TabName = "[1]Действия")]
        public ICollection<ConditionalBranch> Outputs
        {
            get { return BaseOutputs.With(x => x.OfType<ConditionalBranch>()).With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.OfType<Output>()).With(x => x.ToList()); }
        }
    }


    public class ConditionalBranch : Output
    {
        [PropertyDataType(PropertyDataType.ConditionalBranch)]
        [DetailView("Условия перехода")]
        public virtual ICollection<ConditionalMacroItem> InitItems { get; set; } = new List<ConditionalMacroItem>();

        [DetailView("По умолчанию"), ListView]
        public bool IsDefaultBranch { get; set; } = false;
    }

    public class ConditionalMacroItem : ConditionItem
    {
        public int ConditionalBranchID { get; set; }
        public virtual ConditionalBranch ConditionalBranch { get; set; }
    }
}
