using System;
using Base.UI.Presets;
using Base.UI.ViewModal;
using Base.Utils.Common;
using Base.Utils.Common.Maybe;
using Newtonsoft.Json;

namespace Base.UI
{
    [Serializable]
    [JsonObject]
    public class DashboardWidget
    {
        public DashboardWidget()
        {
            Panel = DashboardPanel.Main;
        }

        public string Name { get; set; }
        public double SortOrder { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public DashboardPanel Panel { get; set; }
        public string Preset { get; set; }


        public DashboardWidget Copy(DashboardWidgetPreset preset)
        {
            return new DashboardWidget()
            {
                Name = Name,
                Icon = Icon,
                Title = Title,
                Preset = Preset,
                SortOrder = preset.SortOrder,
                Panel = preset.Panel
            };
        }
    }

    public enum DashboardPanel
    {
        Main,
        Sidebar
    }
}
