using Base.Attributes;

namespace Base.UI.ViewModal
{
    [UiEnum]
    public enum ListViewType
    {
        Grid = 0,
        GridCategorizedItem = 1,
        Tree = 10,
        Scheduler = 20,
        Gantt = 30,
        TreeListView = 40,
        Pivot = 50,
        Custom = 100,
    }
}