using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Macros.Entities;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{
    [Obsolete]
    public class Branch : Output
    {
        public Branch()
        {
            Color = "#6f5499";
        }

        [DetailView(Name = "Построитель макросов", HideLabel = true, TabName = "[1]Конструктор действий")]
        [PropertyDataType("Macro")]
        public virtual ICollection<BranchConditionItem> BranchConditions { get; set; }

        [DetailView("По умолчанию"), ListView]
        public bool IsDefaultBranch { get; set; } = false;
    }

    [Obsolete]
    public class BranchConditionItem : InitItem
    {
        public int BrunchID { get; set; }
        public virtual Branch Brunch { get; set; }
    }

    [Obsolete]
    public class BranchingStep : Step
    {
        public BranchingStep()
        {
            StepType = FlowStepType.BranchingStep;
        }

        [NotMapped]
        [DetailView(Name = "Действия", HideLabel = true, TabName = "[1]Действия")]
        public ICollection<Branch> Outputs
        {
            get { return BaseOutputs.With(x => Enumerable.OfType<Branch>(x)).With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.OfType<Output>()).With(x => x.ToList()); }
        }
    }
}