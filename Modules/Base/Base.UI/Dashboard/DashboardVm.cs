using System.Collections.Generic;

namespace Base.UI
{
    public class DashboardVm
    {
        public string Module { get; set; }
        public IEnumerable<DashboardWidget> Widgets { get; set; } = new List<DashboardWidget>();
    }
}
