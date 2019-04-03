using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Enums;

namespace Base.Entities.Complex
{    
    public abstract class BaseEmail : BaseObject
    {
        [SystemProperty]
        [DetailView("Тип"), ListView]
        public EmailType Type { get; set; }

        [SystemProperty]
        [DetailView("Email", Required = true), ListView]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        public string Email { get; set; }    
    }
}
