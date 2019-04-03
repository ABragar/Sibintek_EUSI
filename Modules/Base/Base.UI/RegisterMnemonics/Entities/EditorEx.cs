using System.ComponentModel.DataAnnotations;
using Base.Attributes;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class EditorEx : BaseObject
    {
        [MaxLength(255)]
        [DetailView(Name = "Поле", Required = true, Order = -1)]
        [PropertyDataType("EditorViewModel")]
        public string PropertyName { get; set; }

        [MaxLength(255)]
        [DetailView(Name = "Наименование", Required = true), ListView]
        public string Title { get; set; }

        [DetailView(Name = "Скрыть наименование")]
        public bool HideLabel { get; set; }

        [DetailView(Name = "Описание"), ListView]
        public string Description { get; set; }

        [MaxLength(255)]
        [DetailView(Name = "Наименование вкладки"), ListView]
        public string TabName { get; set; }

        [DetailView(Name = "Обязательно"), ListView]
        public bool IsRequired { get; set; }

        [DetailView(Name = "Видимо"), ListView]
        public bool Visible { get; set; }

        [DetailView(Name = "Только чтение"), ListView]
        public bool IsReadOnly { get; set; }

        [DetailView(Name = "Порядковый номер"), ListView]
        public int? Order { get; set; }
    }
}