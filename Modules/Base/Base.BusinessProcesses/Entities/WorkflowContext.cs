using System.Collections.Generic;
using Newtonsoft.Json;

namespace Base.BusinessProcesses.Entities
{
    public class WorkflowContext : BaseObject
    {

        public int? WorkflowImplementationID { get; set; }
        public virtual WorkflowImplementation WorkflowImplementation { get; set; }

        [JsonIgnore]
        public virtual ICollection<StagePerform> CurrentStages { get; set; } = new List<StagePerform>();

        public int? ObjectID { get; set; }
        public string ObjectType { get; set; }
    }
}
