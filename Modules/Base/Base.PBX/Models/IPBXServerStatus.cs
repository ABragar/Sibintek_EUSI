namespace Base.PBX.Models
{
    public interface IPBXServerStatus
    {
        bool need_apply { get; set; }
        bool need_reboot { get; set; }
    }
}