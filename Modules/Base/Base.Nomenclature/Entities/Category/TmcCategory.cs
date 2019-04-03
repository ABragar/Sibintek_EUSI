using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.Nomenclature.Entities.Category
{
    [EnableFullTextSearch]
    public class TmcCategory : BaseNomCategory
    {
        //private static readonly CompiledExpression<TmcCategory, string> _title =
        //    DefaultTranslationOf<TmcCategory>.Property(x => x.Title).Is(x => x.Code + ": " + x.Name);

        //[DetailView("Наименование"), ListView]
        //[FullTextSearchProperty]
        //public string Title => _title.Evaluate(this);

        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual TmcCategory Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<TmcCategory> Children_ { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent => this.Parent_;
        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<TmcCategory>();
        #endregion
    }
}
