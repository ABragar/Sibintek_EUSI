namespace Base.UI.Dashboard
{
    public class DashboardWidgetBuilder
    {
        private readonly DashboardWidget _widget;

        public DashboardWidgetBuilder(DashboardWidget widget)
        {
            _widget = widget;
        }

        public DashboardWidgetBuilder Title(string val)
        {
            _widget.Title = val;
            return this;
        }
    }
}