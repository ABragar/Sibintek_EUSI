using System.ComponentModel.DataAnnotations;
using Base.Attributes;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class ColumnEx : BaseObject
    {
        [MaxLength(255)]
        [DetailView(Name = "����", Required = true, Order = -1)]
        [PropertyDataType("ColumnViewModel")]
        public string PropertyName { get; set; }

        [MaxLength(255)]
        [DetailView(Name = "������������", Required = true), ListView]
        public string Title { get; set; }

        [DetailView(Name = "������"), ListView]
        public bool Visible { get; set; }

        [DetailView(Name = "� ���� ������"), ListView]
        public bool OneLine { get; set; }

        [DetailView(Name = "���������� �����"), ListView]
        public int? Order { get; set; }
    }
}