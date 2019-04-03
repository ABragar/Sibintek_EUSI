using Base.Security.ObjectAccess;
using Base.Security.ObjectAccess.Policies;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.FileStorage
{
    [AccessPolicy(typeof(CreatorOnlyPolicy))]
    public class FileStorageCategory : HCategory, IAccessibleObject
    {
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual FileStorageCategory Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<FileStorageCategory> Children_ { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent
        {
            get { return this.Parent_; }
        }
        [NotMapped]
        public override IEnumerable<HCategory> Children
        {
            get { return Children_ ?? new List<FileStorageCategory>(); }
        }
        #endregion
    }
}
