using Base.Attributes;

namespace Base.Contact.Entities
{
    [UiEnum]
    public enum Kinship
    {
        [UiEnumValue("Муж")]
        Husband,
        [UiEnumValue("Жена")]
        Wife,
        [UiEnumValue("Сын")]
        Son,
        [UiEnumValue("Дочь")]
        Daughter,
        [UiEnumValue("Брат")]
        Brother,
        [UiEnumValue("Сестра")]
        Sister,
        [UiEnumValue("Отец")]
        Father,
        [UiEnumValue("Мать")]
        Mother,
        [UiEnumValue("Дедушка")]
        Grandfather,
        [UiEnumValue("Бабушка")]
        Grandmother,
    }
}