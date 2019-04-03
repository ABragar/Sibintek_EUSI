using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace BaseCatalog.Entities
{
    [EnableFullTextSearch]
    public class Measure : BaseObject, ICategorizedItem
    {
        private static readonly CompiledExpression<Measure, string> _title =
            DefaultTranslationOf<Measure>.Property(x => x.Title).Is(x => x.Symbol + " - " + x.Description);

        [ListView]
        [MaxLength(10)]
        [DetailView(Name = "Код", Required = true)]
        [FullTextSearchProperty]
        public string Code { get; set; }

        [ListView]
        public string Title => _title.Evaluate(this);

        [FullTextSearchProperty]
        [MaxLength(255)]
        [DetailView(Name = "Условное обозначение", Required = true)]
        public string Symbol { get; set; }

        [FullTextSearchProperty]
        [DetailView(Name = "Описание", Visible = true)]
        public string Description { get; set; }

        #region ICategorizedItem
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual MeasureCategory Category { get; set; }
        HCategory ICategorizedItem.Category => this.Category;

        #endregion
    }
}
