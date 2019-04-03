using Base.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using Base.Settings;

namespace Base.Notification.Entities
{
    [Serializable]
    public class NotificationSetting: SettingItem
    {
        [DetailView("Отображаемое имя отправителя")]
        [MaxLength(255)]
        public string AccountTitle { get; set; }

        [DetailView("Отправлять уведомления на почту", Order = 20)]
        public bool EnableEmail { get; set; }
    }
}
