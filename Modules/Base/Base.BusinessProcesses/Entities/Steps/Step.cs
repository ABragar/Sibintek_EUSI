using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Entities;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.BusinessProcesses.Entities.Steps
{
    [JsonObject]
    public class Step : BaseObject, IRuntimeBindingType
    {       
        [ListView, MaxLength(255), FullTextSearchProperty]
        [DetailView(Name = "Наименование", Required = true, Order = -100)]
        public string Title { get; set; }
        [ListView, DetailView(Name = "Описание", Order = -90)]
        public string Description { get; set; }
        [DetailView(Visible = false)]
        public string ViewID { get; set; }
        public int WorkflowImplementationID { get; set; }
        public virtual WorkflowImplementation WorkflowImplementation { get; set; }
        [DetailView(Visible = false)]
        public FlowStepType StepType { get; set; }
        [DetailView(Visible = false)]
        public string RuntimeType => GetType().GetTypeName();
        [JsonIgnore]
        public virtual ICollection<Output> BaseOutputs { get; set; } // For mapping    
    }
}