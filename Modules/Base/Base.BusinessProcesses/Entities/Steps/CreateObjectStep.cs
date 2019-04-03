using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Macros.Entities;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{    
    public class CreateObjectStep : Step
    {
        public CreateObjectStep()
        {
            StepType = FlowStepType.CreateObjectTask;
        }

        [ListView, DetailView("Тип объекта", Required = true), PropertyDataType(PropertyDataType.ListBaseObjects)]
        public string ObjectType { get; set; }

        [SystemProperty]
        public string ParentObjectType { get; set; }

        [PropertyDataType(PropertyDataType.BPObjectEditButton)]
        [DetailView("Инициализатор объекта")]
        public virtual ICollection<CreateObjectStepMemberInitItem> InitItems { get; set; } = new List<CreateObjectStepMemberInitItem>();

        [NotMapped]
        [DetailView(Visible = false)]
        public ICollection<Output> Outputs
        {
            get { return BaseOutputs; }
            set { BaseOutputs = value.With(x => x.ToList()); }
        }
        
    }

    public class CreateObjectStepMemberInitItem : InitItem
    {
        public int CreateObjectStepID { get; set; }
        public virtual CreateObjectStep CreateObjectStep { get; set; }
    }
}