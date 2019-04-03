using Base.Attributes;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Base.UI.Presets
{
    [Serializable]
    [JsonObject]
    public class DashboardPreset: Preset
    {
        [PropertyDataType("DashboardWidgets")]
        [DetailView("Виджеты")]
        public List<DashboardWidgetPreset> Widgets { get; set; } = new List<DashboardWidgetPreset>();
    }

    [Serializable]
    [JsonObject]
    public class DashboardWidgetPreset: BaseObject
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public DashboardPanel Panel { get; set; }
    }
}
