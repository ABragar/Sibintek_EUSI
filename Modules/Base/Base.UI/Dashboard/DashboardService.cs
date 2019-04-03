using System;
using System.Collections.Generic;
using System.Linq;
using Base.UI.Presets;

namespace Base.UI.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly Dictionary<string, DashboardWidget> _widgets = new Dictionary<string, DashboardWidget>();

        public DashboardWidgetBuilder Register(string name)
        {
            var widget = new DashboardWidget() { Name = name };

            var widgetbuilder = new DashboardWidgetBuilder(widget);

            if (!_widgets.ContainsKey(name))
                _widgets.Add(name, widget);

            return widgetbuilder;
        }

        public IEnumerable<DashboardWidget> GetWidgets()
        {
            return _widgets.Values;
        }

        public IEnumerable<DashboardWidget> GetWidgetsForPreset(DashboardPreset preset)
        {
            if (preset == null)
                throw new ArgumentNullException(nameof(preset));

            return preset.Widgets
                .Where(x => _widgets.ContainsKey(x.Name))
                .Select(x => _widgets[x.Name].Copy(x));
        }
    }
}