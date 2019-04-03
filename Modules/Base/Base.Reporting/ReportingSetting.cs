using System;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Settings;

namespace Base.Reporting
{
    [Serializable]
    public class ReportingSetting : SettingItem
    {
        [DetailView("Ссылка на REST-сервис", Required = true)]
        [MaxLength(255)]
        public string Url { get; set; }
    }
}