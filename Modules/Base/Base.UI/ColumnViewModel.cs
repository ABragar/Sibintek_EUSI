using System;

namespace Base.UI
{
    public class ColumnViewModel : PropertyViewModel
    {
        public ColumnViewModel() : base()
        {
        }

        public ColumnViewModel(string name) : base(name)
        {
        }
        public string ChildMnemonic { get; set; }
        public Type ChildMnemonicType { get; set; }
        public string ParentProperty { get; set; }
        public string ChildProperty { get; set; }
        public string ChildMnemonicLink { get; set; }
        public bool Filterable { get; set; }
        public bool FilterMulti { get; set; }
        public bool Locked { get; set; }
        public bool Lockable { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool Groupable { get; set; }
        public bool Sortable { get; set; }
        public Type ColumnType => ViewModelType;
        public bool OneLine { get; set; }
        //sib
        public string ClientFooterTemplate { get; set; }
        public string ClientGroupHeaderTemplate { get; set; }
        public string Format { get; set; }
        public string ClientTemplate { get; set; }
        //end sib

        public override T Copy<T>(T propertyView = null)
        {
            var col = propertyView as ColumnViewModel ?? new ColumnViewModel();

            col.Filterable = Filterable;
            col.FilterMulti = FilterMulti;
            col.Locked = Locked;
            col.Lockable = Lockable;
            col.Width = Width;
            col.Height = Height;
            col.Groupable = Groupable;
            col.Sortable = Sortable;
            col.OneLine = OneLine;
            col.SortOrder = SortOrder;

            col.ClientFooterTemplate = ClientFooterTemplate;
            col.ClientGroupHeaderTemplate = ClientGroupHeaderTemplate;
            col.Format = Format;
            col.ClientTemplate = ClientTemplate;

            return base.Copy(col as T);
        }
    }
}