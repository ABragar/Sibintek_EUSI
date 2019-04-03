using Base.Attributes;

namespace Base.Notification.Entities
{
    [UiEnum]
    public enum NotificationStatus
    {
        [UiEnumValue("Новое")]
        New = 0,
        [UiEnumValue("Просмотрено")]
        Viewed = 1
    }
}
