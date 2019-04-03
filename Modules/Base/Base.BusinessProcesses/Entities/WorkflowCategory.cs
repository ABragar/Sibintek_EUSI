using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.BusinessProcesses.Entities
{
    public class WorkflowCategory : HCategory
    {

        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual WorkflowCategory Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<WorkflowCategory> Children_ { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent => Parent_;
        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? new List<WorkflowCategory>();
        #endregion
    }
}
