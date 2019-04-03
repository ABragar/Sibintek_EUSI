namespace Base.UI.ViewModal
{
    public class PreviewFieldBuilder<T> where T : class
    {
        private readonly PreviewField _field;

        public PreviewFieldBuilder(PreviewField field)
        {
            _field = field;
        }

        public PreviewFieldBuilder<T> Title(string title)
        {
            _field.Title = title;
            return this;
        }

        public PreviewFieldBuilder<T> TabName(string tabName)
        {
            _field.TabName = tabName;
            return this;
        }

        public PreviewFieldBuilder<T> Order(int order)
        {
            _field.SortOrder = order;
            return this;
        }
    }
}