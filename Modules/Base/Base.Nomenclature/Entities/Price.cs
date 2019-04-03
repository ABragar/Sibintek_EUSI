using Base.Attributes;
using Base.Contact.Entities;
using Base.Entities;
using Base.Translations;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Entities
{
    public class Price : BaseObject
    {
        private static readonly CompiledExpression<Price, string> _title =
            DefaultTranslationOf<Price>.Property(x => x.Title).Is(x =>(x.Vender != null? "ООО "+ x.Vender.Title : "") + (x.Product != null ? " / "+ x.Product.Title : "") +(" / "+x.PriceTarif)+ (x.Measure!=null? " / "+ x.Measure.Title : ""));

        [ListView(Visible = false)]
        public string Title => _title.Evaluate(this);

        //[ListView(Name = "Тип перевозки")]
        //[DetailView("Тип перевозки")]
        //public TransportType TransportType { get; set; }

        public int? ProductID { get; set; }

        [DetailView("Категория")]
        [ListView(Hidden = true)]
        public virtual NomenclatureType.Nomenclature Product { get; set; }

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
}

