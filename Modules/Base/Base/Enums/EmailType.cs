using Base.Attributes;

namespace Base.Enums
{
    [UiEnum]
    public enum EmailType
    {
        [UiEnumValue("Рабочий")]
        Work,
        [UiEnumValue("Личный")]
        Personal,
    }
}
