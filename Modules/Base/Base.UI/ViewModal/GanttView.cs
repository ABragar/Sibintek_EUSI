namespace Base.UI.ViewModal
{
    public class GanttView : ListView
    {
        public GanttView()
        {
            this.Type = ListViewType.Gantt;
        }

        public override T Copy<T>(T view = null)
        {
            var gant = view as GanttView ?? new GanttView();
            //copy
            return base.Copy(gant as T);
        }
    }
}
