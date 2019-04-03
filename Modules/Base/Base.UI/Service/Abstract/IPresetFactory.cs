using Base.DAL;

namespace Base.UI
{
    public interface IPresetFactory<out T> where T : Preset
    {
        T Create(string ownerName);
    }
}
