using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Nomenclature.Entities.Category;
using Base.Translations;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.Nomenclature.Entities
{
    public class OKPD2 : HCategory
    {
        private static readonly CompiledExpression<OKPD2, string> _title =
                DefaultTranslationOf<OKPD2>.Property(x => x.Title).Is(x => x.Code + ": " + x.Name);

        [DetailView("Наименование"), ListView]
        [FullTextSearchProperty]
        public string Title => _title.Evaluate(this);

        [DetailView("Код", Required = true, Order = 0), ListView]
        [MaxLength(255)]
        public string Code { get; set; }

        [DetailView("Описание", Order = 10), ListView]
        public string Description { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent => this.Parent_;
        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<OKPD2>();
        #endregion

        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual OKPD2 Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<OKPD2> Children_ { get; set; }
    }
}
