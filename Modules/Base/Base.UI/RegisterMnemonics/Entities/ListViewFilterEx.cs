using Base.Attributes;
using Base.UI.ViewModal;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class ListViewFilterEx : MnemonicEx
    {
        [DetailView("Фильтр", Required = true), ListView]
        [PropertyDataType("QueryBuilderFilter")]
        public string Filter { get; set; }

        public override void Visit(ConfigListViewFilter listConfigListViewFilter)
        {
            listConfigListViewFilter.Filter = Filter;
        }
    }
}