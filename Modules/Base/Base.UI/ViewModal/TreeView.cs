namespace Base.UI.ViewModal
{
    public class TreeView : ListView
    {
        public TreeView()
        {
            Type = ListViewType.Tree;
        }

        public override T Copy<T>(T view = null)
        {
            var lv = view as TreeView ?? new TreeView();
            //copy
            return base.Copy(lv as T);
        }
    }
}