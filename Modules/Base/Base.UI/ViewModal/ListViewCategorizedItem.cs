using System;

namespace Base.UI.ViewModal
{
    public class ListViewCategorizedItem : ListView
    {
        public ListViewCategorizedItem()
        {
            Type = ListViewType.GridCategorizedItem;
        }

        public string MnemonicCategory { get; set; }
        public bool HiddenTree { get; set; }

        public override T Copy<T>(T view = null)
        {
            var lv = view as ListViewCategorizedItem ?? new ListViewCategorizedItem();

            lv.MnemonicCategory = MnemonicCategory;
            lv.HiddenTree = HiddenTree;

            return base.Copy(lv as T);
        }
    }
}