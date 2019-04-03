using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Entities.Complex;
using Base.UI.ViewModal;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class TitleEx: MnemonicEx
    {
        [DetailView("Иконка"), ListView]
        public Icon Icon { get; set; } = new Icon();

        [MaxLength(255)]
        [DetailView("Наименование", Required = true), ListView]
        public string Title { get; set; }
        [MaxLength(255)]
        [DetailView("Наименование - Список", Required = true), ListView]
        public string ListViewTitle { get; set; }
        [MaxLength(255)]
        [DetailView("Наименование - Форма", Required = true), ListView]
        public string DetailViewTitle { get; set; }

        public override void Visit(ConfigTitle configTitle)
        {
            configTitle.Title = Title;
            configTitle.ListViewTitle = ListViewTitle;
            configTitle.DetailViewTitle = DetailViewTitle;
            configTitle.Icon = Icon;

        }
    }
}