namespace Base.PBX.Models
{
    public class PBXServerStatus : IPBXServerStatus
    {
        public bool need_apply { get; set; }
        public bool need_reboot { get; set; }
    }
}