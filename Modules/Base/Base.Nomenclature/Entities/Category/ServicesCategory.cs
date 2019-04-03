using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Base.Nomenclature.Entities.Category
{
    public class ServicesCategory : BaseNomCategory
    {
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual ServicesCategory Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<ServicesCategory> Children_ { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent => this.Parent_;
        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<ServicesCategory>();
        #endregion
    }
}
