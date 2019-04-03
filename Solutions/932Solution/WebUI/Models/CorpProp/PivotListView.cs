namespace Base.UI.ViewModal
{
    public class PivotListView : ListView
    {
        public PivotListView()
        {
            this.Type = ListViewType.Pivot;
        }

        public override T Copy<T>(T view = null)
        {
            var gant = view as PivotListView ?? new PivotListView();
            //copy
            return base.Copy(gant as T);
        }
    }
}
