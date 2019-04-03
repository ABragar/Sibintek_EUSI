using Base.Attributes;

namespace Base.Task.Entities
{
    [UiEnum]
    public enum TaskType
    {
        [UiEnumValue("Задача")]
        Task = 0,
        [UiEnumValue("Веха")]
        Note = 1
    }
}
