using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Base.Attributes;

namespace BaseCatalog.Entities
{
    [ViewModelConfig(Title = "Единицы измерения - Категория")]
    public class MeasureCategory : HCategory
    {
        #region HCategory
        [ForeignKey("ParentID")]
        public virtual MeasureCategory Parent_ { get; set; }
        public virtual ICollection<MeasureCategory> Children_ { get; set; }
        public override HCategory Parent => this.Parent_;
        public override IEnumerable<HCategory> Children => Children_ ?? new List<MeasureCategory>();

        #endregion
    }
}
