using System;
using Base.Attributes;
using Base.UI;
using Newtonsoft.Json;

namespace Base.Event.UI.Presets
{
    [Serializable]
    [JsonObject]
    public class SchedulerPreset : Preset
    {
        [DetailView("Тест")]
        public string TestField { get; set; }
    }
}
