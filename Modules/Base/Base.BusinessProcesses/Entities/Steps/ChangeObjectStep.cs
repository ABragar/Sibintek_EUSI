using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Macros.Entities;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{
    public class ChangeObjectStep : Step
    {
        public ChangeObjectStep()
        {
            StepType = FlowStepType.ChangeObjectStep;
        }

        [PropertyDataType(PropertyDataType.BPObjectEditButton)]
        [DetailView("Инициализатор объекта", TabName = "Инициализатор объекта")]
        public virtual ICollection<ChangeObjectStepInitItems> InitItems { get; set; } =
            new List<ChangeObjectStepInitItems>();

        [NotMapped]
        [DetailView(Visible = false)]
        public ICollection<Output> Outputs
        {
            get { return BaseOutputs.With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.ToList()); }
        }
    }

    public class ChangeObjectStepInitItems : InitItem
    {
        public int StepID { get; set; }
        public virtual ChangeObjectStep Step { get; set; }
    }
}