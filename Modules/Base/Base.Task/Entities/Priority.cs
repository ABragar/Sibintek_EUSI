using Base.Attributes;

namespace Base.Task.Entities
{
    [UiEnum]
    public enum Priority
    {
        [UiEnumValue("Высокий")]
        High = 0,
        [UiEnumValue("Нормальный")]
        Normal = 1,
        [UiEnumValue("Низкий")]
        Low = 2
    }
}