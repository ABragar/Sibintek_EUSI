using System.Collections.Concurrent;
using Base.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Entities
{
    [JsonObject]
    public class Output : BaseObject
    {
        public Output()
        {
            Color = "#6f5499";
        }

        [ListView]
        [DetailView(Name = "Описание", Order = -100)]
        public string Description { get; set; }

        [PropertyDataType(PropertyDataType.Color)]
        [DetailView(Name = "Цвет", Order = -90)]
        [ListView]
        public string Color { get; set; }

        [DetailView(Visible = false)]
        public string NextStepViewID { get; set; }
        
        public int StepID { get; set; }
        [JsonIgnore, InverseProperty("BaseOutputs")]
        public virtual Step Step { get; set; }

        [DetailView(Visible = false)]
        public int? NextStepID { get; set; }
        [JsonIgnore]
        public virtual Step NextStep { get; set; }

        [ListView]
        [MaxLength(255)]
        [DetailView(Name = "Служебное имя", Order = -80)]
        public string SystemName { get; set; }
    }
}