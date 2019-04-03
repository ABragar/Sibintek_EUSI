using Base.Attributes;

namespace Base.UI
{
    public class ListViewColumnBuilder<T> where T : class
    {
        private readonly ColumnViewModel _column;

        public ListViewColumnBuilder(ColumnViewModel column)
        {
            _column = column;
        }

        public ListViewColumnBuilder<T> Title(string val)
        {
            _column.Title = val;
            return this;
        }

        public ListViewColumnBuilder<T> Mnemonic(string val)
        {
            _column.Mnemonic = val;
            return this;
        }

        public ListViewColumnBuilder<T> Visible(bool val)
        {
            _column.Visible = val;
            return this;
        }

        public ListViewColumnBuilder<T> Filterable(bool val)
        {
            _column.Filterable = val;
            return this;
        }

        public ListViewColumnBuilder<T> FilterMulti(bool val)
        {
            _column.FilterMulti = val;
            return this;
        }

        public ListViewColumnBuilder<T> Order(int val)
        {
            _column.SortOrder = val;
            return this;
        }

        public ListViewColumnBuilder<T> Locked(bool val)
        {
            _column.Locked = val;
            return this;
        }

        public ListViewColumnBuilder<T> Lockable(bool val)
        {
            _column.Lockable = val;
            return this;
        }

        public ListViewColumnBuilder<T> DataType(PropertyDataType propertyDataType)
        {
            _column.PropertyDataType = propertyDataType;
            return this;
        }

        public ListViewColumnBuilder<T> OneLine(bool val)
        {
            _column.OneLine = val;
            return this;
        }

        public ListViewColumnBuilder<T> ClientFooterTemplate(string val)
        {
            _column.ClientFooterTemplate = val;
            return this;
        }

        public ListViewColumnBuilder<T> ClientGroupHeaderTemplate(string val)
        {
            _column.ClientGroupHeaderTemplate = val;
            return this;
        }

        public ListViewColumnBuilder<T> Format(string val)
        {
            _column.Format = val;
            return this;
        }

        public ListViewColumnBuilder<T> ClientTemplate(string val)
        {
            _column.ClientTemplate = val;
            return this;
        }
    }
}