using System;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Settings;

namespace Base.Task.Entities
{
    [Serializable]
    public class TaskSetting : SettingItem
    {
        public int? TaskCategoryID { get; set; }

        [DetailView("Категория по умолчанию", Required = true)]
        public virtual TaskCategory TaskCategory { get; set; }
    }
}