using System;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Settings;

namespace Base.MailAdmin.Entities
{


    [Serializable]
    public class MailAdminSettings: SettingItem
    {

        [DetailView]
        public bool Enabled { get; set; } = true;

        [DetailView]
        [MaxLength(50)]
        public string Domain { get; set; } = "mail2.pba.su";

        [DetailView]
        [MaxLength(255)]
        public string BaseAddress { get; set; } = "http://project.pba.su";
    }
}