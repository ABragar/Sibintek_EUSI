using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Translations;

namespace Base.Nomenclature.Entities
{
    public class OkpdHierarchy : HCategory
    {
        private static readonly CompiledExpression<OkpdHierarchy, string> _title =
            DefaultTranslationOf<OkpdHierarchy>.Property(x => x.Title).Is(x => "[" + x.Code + "] " + x.Name);

        [MaxLength(20)]
        [DetailView("Код", Required = true, Order = 2)]
        public string Code { get; set; }

        public string Title => _title.Evaluate(this);

        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual OkpdHierarchy Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<OkpdHierarchy> Children_ { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent => this.Parent_;

        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? new List<OkpdHierarchy>();

        #endregion
    }
}
