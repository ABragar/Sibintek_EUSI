using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.UI.DetailViewSetting;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{
    public class WorkflowOwnerStep : Step
    {
        public WorkflowOwnerStep()
        {
            StepType = FlowStepType.WorkflowOwnerStep;
        }

        public int? ChildWorkflowImplementationID { get; set; }

        [DetailView(Name = "Дочерний бизнес-процесс", Order = 3)]
        [PropertyDataType("WorkflowImplementation")]
        [ListView]
        public virtual WorkflowImplementation ChildWorkflowImplementation { get; set; }       

        [NotMapped]
        [DetailView(Visible = false)]
        public ICollection<Output> Outputs
        {
            get { return BaseOutputs.With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.ToList()); }
        }

        public int? DvSettingID { get; set; }

        [DetailView(Name = "Настройка формы объекта", Order = 4)]
        [PropertyDataType(PropertyDataType.DetailViewSetting)]
        public virtual DvSettingForType DvSetting { get; set; }
    }
}
