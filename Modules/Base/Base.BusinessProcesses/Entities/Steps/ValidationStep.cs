using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{
    public class ValidationStep : Step
    {
        public ValidationStep()
        {
            StepType = FlowStepType.ValidationStep;
        }

        [PropertyDataType("StageValidationEdit")]
        [DetailView("Правила валидации", TabName = "Инициализатор объекта")]
        public virtual ICollection<StepValidationItem> ValidatonRules { get; set; }

        [NotMapped]
        [DetailView(Visible = false)]
        public ICollection<Output> Outputs
        {
            get { return BaseOutputs.With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.ToList()); }
        }

        
    }
}
