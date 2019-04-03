namespace Base.UI.Presets
{
    public class DefaultPresetFactory<T> : IPresetFactory<T> where T: Preset, new ()
    {
        public virtual T Create(string ownerName)
        {
            return new T() { For = ownerName };
        }
    }
}
