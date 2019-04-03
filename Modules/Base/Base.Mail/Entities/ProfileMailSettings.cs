using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Security;
using Base.UI;
using Newtonsoft.Json;

namespace Base.Mail.Entities
{
    [Serializable]
    [JsonObject]
    public class ProfileMailSettings : Preset
    {
        [DetailView("Логин", TabName = "[0]Основное", Required = true)]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        [MaxLength(255)]
        public string AccountLogin { get; set; }

        [DetailView("Пароль", TabName = "[0]Основное", Required = true)]
        [MaxLength(255)]
        [PropertyDataType(PropertyDataType.Password)]
        public string AccountPassword { get; set; }

        [DetailView("SMTP-сервер", TabName = "[1]Отправка почты")]
        [MaxLength(255)]
        public string SmtpServerAddress { get; set; }

        [DetailView("Порт", TabName = "[1]Отправка почты")]
        public int SmtpServerPort { get; set; } = 465;

        [DetailView("Использовать SSL", TabName = "[1]Отправка почты")]
        public bool SmtpUseSsl { get; set; }

        [DetailView("Почт.сервер", TabName = "[2]Получение почты")]
        [MaxLength(255)]
        public string ServerAddress { get; set; }

        [DetailView("Порт", TabName = "[2]Получение почты")]
        public int ServerPort { get; set; } = 995;

        [DetailView("Использовать SSL", TabName = "[2]Получение почты")]
        public bool UseSsl { get; set; }
    }
}
