using System;
using Base.Attributes;
using Base.Settings;

namespace Base.Conference.Entities
{
    [Serializable]
    public class ConferenceSetting : SettingItem
    {
        [DetailView("Время хранения (дней)")]
        public int StorageTime { get; set; }

        [DetailView("Объем хранения (МБ)")]
        public int StorageSize { get; set; }
    }
}