namespace Base.UI.ViewModal
{
    public sealed class TreeListView : ListView
    {
        public TreeListView()
        {
            Type = ListViewType.TreeListView;
        }

        public override T Copy<T>(T view = null)
        {
            var tree = view as TreeListView ?? new TreeListView();
            //copy
            return base.Copy(tree as T);
        }
    }
}
