using Base.Attributes;
using Base.Contact.Entities;
using Base.Entities;
using Base.Nomenclature.Entities.Category;
using Base.Translations;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Entities
{
    public class Transportation :BaseObject
    {
        private static readonly CompiledExpression<Transportation, string> _title =
            DefaultTranslationOf<Transportation>.Property(x => x.Title).Is(x => (x.Vender != null ? "ООО " + x.Vender.Title : "") + (x.NomCategory != null ? " / " + x.NomCategory.Name : "") + (" / " + x.PriceTarif) + (x.Measure != null ? " / " + x.Measure.Title : ""));

        [ListView(Visible = false)]
        public string Title => _title.Evaluate(this);

        [ListView(Name = "Тип перевозки")]
        [DetailView("Тип перевозки")]
        public TransportType TransportType { get; set; }

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

    [UiEnum]
    public enum TransportType
    {
        [UiEnumValue("Авто перевозки")]
        Auto = 10,
        [UiEnumValue("ЖД перевозки")]
        JD = 20,
        [UiEnumValue("Авиа перевозки")]
        Avia = 30,
    }
}
