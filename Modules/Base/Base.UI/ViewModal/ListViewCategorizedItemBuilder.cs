using System;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class ListViewCategorizedItemBuilder<T> : ListViewBuilder<T> where T : class
    {
        private readonly ListViewCategorizedItem _listView;

        public ListViewCategorizedItemBuilder(ListViewCategorizedItem listView, IInitializerContext context)
                        : base(listView, context)
        {
            if (listView == null)
                throw new ArgumentNullException("Listview is null");
            _listView = listView;
        }

        public ListViewCategorizedItemBuilder<T> MnemonicCategory(string val)
        {
            _listView.MnemonicCategory = val;
            return this;
        }

        public ListViewCategorizedItemBuilder<T> HiddenTree(bool val)
        {
            _listView.HiddenTree = val;
            return this;
        }
    }
}