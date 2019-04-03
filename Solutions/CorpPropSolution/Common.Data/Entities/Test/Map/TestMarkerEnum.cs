using Base.Attributes;

namespace Common.Data.Entities.Test.Map
{
    [UiEnum]
    public enum TestMarkerEnum
    {
        [UiEnumValue("Marker1")] Marker1,

        [UiEnumValue("Marker2")] Marker2,

        [UiEnumValue("Marker3")] Marker3
    }
}