using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Entities;

namespace Base.BusinessProcesses.Entities.Steps
{
    public class ExtendedStage : Stage
    {
        public ExtendedStage()
        {
            StepType = FlowStepType.ExtendedStage;
        }

        [DetailView("Дополнительно")]
        [PropertyDataType("StageExtender")]
        public virtual  StageExtender Extender { get; set; }
        public int ExtenderID { get; set; }

        public string Mnemonic { get; set; }       
    }

    public class StageExtender : BaseObject, IRuntimeBindingType
    {
        [NotMapped]
        public string RuntimeType => GetType().GetBaseObjectType().FullName;
    }
}