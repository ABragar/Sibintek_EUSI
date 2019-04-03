using Base.Attributes;
using Base.Translations;

namespace Base.Contact.Entities
{
    public class BasePersonAgent : BaseObject
    {
        private static readonly CompiledExpression<BasePersonAgent, string> _title =
            DefaultTranslationOf<BasePersonAgent>.Property(x => x.Title)
                .Is(
                    x => x.Representative == null ? "" : x.Representative.Title);


        [ListView]
        [DetailView("Наименование",ReadOnly = true)]
        public string Title => _title.Evaluate(this);

        [ListView]
        [DetailView("Тип представителя")]
        public AgentType AgentType { get; set; }

        public int? BaseEmployeeID { get; set; }
        [ListView]
        [DetailView("Кого представляет", Visible = false)]
        public virtual BaseEmployee BaseEmployee { get; set; }

        public int? RepresentativeID { get; set; }
        [ListView]
        [DetailView("Представитель", Required = true)]
        public virtual BaseEmployee Representative { get; set; }
    }
}
