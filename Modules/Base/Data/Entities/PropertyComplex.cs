using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class PropertyComplex : HCategory
    {
        public string PCName { get; set; }

        public int? PCClassID { get; set; }
        public virtual PCClass PCClass { get; set; }

        #region HCategory
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual PropertyComplex Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertyComplex> Children_ { get; set; }
        public override HCategory Parent => this.Parent_;
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<HCategory>();
        #endregion
      

        public string Discription { get; set; }
    }
}
