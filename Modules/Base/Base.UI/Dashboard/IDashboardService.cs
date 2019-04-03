using System.Collections.Generic;
using Base.UI.Presets;

namespace Base.UI.Dashboard
{
    public interface IDashboardService
    {
        DashboardWidgetBuilder Register(string name);
        IEnumerable<DashboardWidget> GetWidgets();
        IEnumerable<DashboardWidget> GetWidgetsForPreset(DashboardPreset preset);
    }
}
