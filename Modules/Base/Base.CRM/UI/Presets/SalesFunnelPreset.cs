using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Enums;
using Base.UI;
using Newtonsoft.Json;

namespace Base.CRM.UI.Presets
{
    [Serializable]
    [JsonObject]
    public class SalesFunnelPreset : Preset
    {
        [DetailView("Период")]
        public DatePeriod DatePeriod { get; set; }

        [DetailView(Name = "Мнемоника", Required = true)]
        [PropertyDataType("IDealService")]
        public string EntityMnemonic { get; set; }

    }
}
