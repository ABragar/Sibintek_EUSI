using Base.Attributes;
using Base.Contact.Entities;
using Base.Entities;
using Base.Nomenclature.Entities.Category;
using Base.Translations;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Entities
{
    public class Stowage: BaseObject
    {
        private static readonly CompiledExpression<Stowage, string> _title =
            DefaultTranslationOf<Stowage>.Property(x => x.Title).Is(x => (x.Vender != null ? "ООО " + x.Vender.Title : "") + (x.NomCategory != null ? " / " + x.NomCategory.Name : "") + (" / " + x.PriceTarif) + (x.Measure != null ? " / " + x.Measure.Title : ""));

        [ListView(Visible = false)]
        public string Title => _title.Evaluate(this);

        //[ListView(Name = "Тип перевозки")]
        //[DetailView("Тип перевозки")]
        //public TransportType TransportType { get; set; }

        public int? StowageTypeID { get; set; }

        [DetailView("Вид хранения")]
        [ListView(Hidden = true)]
        public virtual StowageType StowageType { get; set; }

        public int? NomCategoryID { get; set; }

        [DetailView("Категория")]
        [ListView(Hidden = true)]
        public virtual NomenclatureCategory NomCategory { get; set; }

        public int? VenderID { get; set; }

        [ListView(Name = "Поставщик")]
        [DetailView("Поставщик")]
        public virtual Company Vender { get; set; }

        public int? MeasureID { get; set; }

        [ListView(Name = "Ед. изм.")]
        [DetailView("Ед. изм.")]
        public virtual Measure Measure { get; set; }

        [ListView(Name = "Цена")]
        [DetailView("Цена")]
        public decimal PriceTarif { get; set; }
    }

    public class StowageType:BaseObject
    {
        [DetailView("Название")]
        [ListView]
        public string Title { get; set; }
    }
}
