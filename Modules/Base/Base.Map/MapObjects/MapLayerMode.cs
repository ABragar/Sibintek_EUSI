using Base.Attributes;

namespace Base.Map.MapObjects
{
    [UiEnum]
    public enum MapLayerMode
    {
        [UiEnumValue("Клиентский")]
        Client = 1,

        [UiEnumValue("Серверный")]
        Server = 2
    }
}