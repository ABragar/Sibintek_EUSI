using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Entities.Complex;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Nomenclature.Entities.Category
{
    public class NomenclatureCategory : HCategory
    {
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual NomenclatureCategory Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<NomenclatureCategory> Children_ { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent => this.Parent_;
        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<NomenclatureCategory>();
        #endregion
    }
}
