using System.ComponentModel.DataAnnotations;
using Base.Attributes;

namespace Base.Security
{
    public class PropertyPermission : BaseObject
    {
        [MaxLength(255)]
        [DetailView(Name = "Поле", Required = true), ListView]
        [PropertyDataType("Property")]
        public string PropertyName { get; set; }
        [DetailView(Name = "Чтение"), ListView]
        public bool AllowRead { get; set; }
        [DetailView(Name = "Редактирование"), ListView]
        public bool AllowWrite { get; set; }
    }
}