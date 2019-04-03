using Base.UI.ViewModal;

namespace Base.UI
{
    public class TreeViewBuilder<T> : ListViewBuilder<T> where T : class
    {
        private readonly TreeView _treeView;

        public TreeViewBuilder(TreeView treeView, IInitializerContext context)
            : base(treeView, context)
        {
            _treeView = treeView;
        }
    }
}