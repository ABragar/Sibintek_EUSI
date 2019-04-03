using Base.Attributes;
using Base.Contact.Entities;
using Base.Entities;
using Base.Nomenclature.Entities.Category;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Entities
{
    public class Tarif : BaseObject
    {
        //TODO убрать формат
        //private static readonly CompiledExpression<Tarif, string> _title =
        //    DefaultTranslationOf<Tarif>.Property(x => x.Title).Is(x => (x.Vender != null ? string.Format("ООО \"{0}\"", x.Vender.Title) : "") + "/" + (x.NomCategory != null ? x.Title : "") + "/" + (x.PriceTarif) + "/" + (x.Measure != null ? x.Title : ""));

        //[SystemProperty]
        //public string Title => _title.Evaluate(this);

        [ListView(Hidden = true)]
        [DetailView(Name = "Тип перевозки")]
        public TransportType TransportType { get; set; }

        [SystemProperty]
        public int? NomCategoryID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Категория")]
        public virtual NomenclatureCategory NomCategory { get; set; }

        [SystemProperty]
        public int? VenderID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Поставщик")]
        public virtual Company Vender { get; set; }

        [SystemProperty]
        public int? MeasureID { get; set; }

        [DetailView("Ед. изм.")]
        public virtual Measure Measure { get; set; }

        [DetailView("Цена")]
        public decimal PriceTarif { get; set; }
    }
}