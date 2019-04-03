using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Contact.Entities;
using Base.Translations;

namespace Base.Event.Entities
{
    public class Call : Event
    {
        private static readonly CompiledExpression<Call, string> _phone =
            DefaultTranslationOf<Call>.Property(x => x.CodeAndPhone).Is(x => x.Code + " " + x.Phone);

        [ListView]
        [DetailView("Тип звонка", Order = 10)]
        public CallType CallType { get; set; }

        public int? ContactID { get; set; }
        [ListView]
        [DetailView("Контакт", Required = true, Order = 11)]
        public virtual BaseContact Contact { get; set; }

        [ListView("Телефон"), DetailView(ReadOnly = true, Order = 1, Name = "Телефон")]
        [NotMapped]
        public string CodeAndPhone => _phone.Evaluate(this);

        [DetailView("Код", ReadOnly = true, Visible = false, Order = 13)]
        [MaxLength(3)]
        public string Code { get; set; } = "+7";

        [DetailView("Телефон", ReadOnly = true, Visible = false, Order = 14)]
        [MaxLength(15)]
        public string Phone { get; set; }
    }

    [UiEnum]
    public enum CallType
    {
        [UiEnumValue("Исходящий")]
        Out = 0,
        [UiEnumValue("Входящий")]
        In = 1,
    }
}