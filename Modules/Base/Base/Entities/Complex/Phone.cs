using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Enums;

namespace Base.Entities.Complex
{
    [ComplexType]
    public class Phone
    {
        [DetailView("Тип"), ListView]
        public PhoneType Type { get; set; }

        [DetailView("Код", Required = true), ListView]
        [MaxLength(10)]
        public string Code { get; set; } = "+7";

        [DetailView("Номер", Required = true), ListView]
        public string Number { get; set; }

        public override string ToString()
        {
            return Code + Number;
        }
    }
}
