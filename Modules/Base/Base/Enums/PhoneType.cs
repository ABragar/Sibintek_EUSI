using Base.Attributes;

namespace Base.Enums
{
    [UiEnum]
    public enum PhoneType
    {
        [UiEnumValue("Мобильный", Icon = "mdi mdi-cellphone-iphone tooltipstered")]
        Mobile,
        [UiEnumValue("Домашний", Icon = "mdi mdi-cellphone-iphone tooltipstered")]
        Home,
        [UiEnumValue("Рабочий", Icon = "mdi mdi-phone-voip tooltipstered")]
        Work,
    }
}
