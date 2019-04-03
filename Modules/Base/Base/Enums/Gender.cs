using Base.Attributes;

namespace Base.Enums
{
    [UiEnum]
    public enum Gender
    {
        [UiEnumValue("Мужской")]
        Male,
        [UiEnumValue("Женский")]
        Female
    }
}
