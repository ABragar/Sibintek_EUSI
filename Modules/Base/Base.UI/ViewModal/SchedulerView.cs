namespace Base.UI.ViewModal
{
    public class SchedulerView: ListView
    {
        public SchedulerView()
        {
            Type = ListViewType.Scheduler;
        }

        public override T Copy<T>(T view = null)
        {
            var scheduler = view as SchedulerView ?? new SchedulerView();
            //copy
            return base.Copy(scheduler as T);
        }
    }
}
