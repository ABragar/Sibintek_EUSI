using Base.Attributes;

namespace Base.Task.Entities
{
    [UiEnum]
    public enum TaskStatus
    {
        [UiEnumValue("Новая", Color = "#5cb85c")]
        New = 0,

        [UiEnumValue("Исполняется", Color = "#5cb85c")]
        InProcess = 1,

        [UiEnumValue("Завершена", Color = "#5bc0de")]
        Complete = 2,

        [UiEnumValue("Неактуально", Color = "#f0ad4e")]
        NotRelevant = 3,

        [UiEnumValue("Просмотрено", Color = "#5bc0de")]
        Viewed = 4,

        [UiEnumValue("Уточнение", Color = "#5bc0de")]
        Refinement = 5,

        [UiEnumValue("Проверка", Color = "#d9534f")]
        Revise = 6,

        [UiEnumValue("Доработка", Color = "#5bc0de")]
        Rework = 7,

        [UiEnumValue("Переадресация", Color = "#f0ad4e")]
        Redirection = 8,

        [UiEnumValue("Отменена", Color = "#f0ad4e")]
        Abolished = 9,
    }
}
