using Base.Attributes;

namespace Base.Enums
{
    [UiEnum]
    public enum SystemRole
    {
        [UiEnumValue("Администраторы")]
        Admin = 0,
        [UiEnumValue("Администраторы бизнес-процессов")]
        AdminWF = 1,
        [UiEnumValue("Высшие должностные лица")]
        Ceo = 10,
        [UiEnumValue("Базовая")]
        Base = 20,
    }
}
