using Base.UI.Service;
using Newtonsoft.Json;
using System;
using Base.UI.ViewModal;

namespace Base.UI
{
    [Serializable]
    [JsonObject]
    public abstract class Preset : BaseObject
    {
        public string For { get; set; }
    }
}
