namespace Base.PBX.Entities
{
    public interface IPBXServer
    {
        string Url { get; } 
        string User { get; set; }
        string Password { get; set; }
        int MinNumber { get; set; }
    }
}