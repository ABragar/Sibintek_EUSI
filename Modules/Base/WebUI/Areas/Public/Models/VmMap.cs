using Base.Entities;

namespace WebUI.Areas.Public.Models
{
    public class VmMap
    {
        public string UID { get; set; }
        public MapViewType ViewType { get; set; }
        public AppSetting Settings { get; set; }
    }
}