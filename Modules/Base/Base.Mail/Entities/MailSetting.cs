using System;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Settings;

namespace Base.Mail.Entities
{
    [Serializable]
    public class MailSetting: SettingItem
    {
        [DetailView("SMTP-сервер", TabName = "[0]Отправка почты")]
        [MaxLength(255)]
        public string SmtpServerAddress { get; set; }

        [DetailView("Порт", TabName = "[0]Отправка почты")]
        public int SmtpServerPort { get; set; } = 465;

        [DetailView("Адрес отправителя", TabName = "[0]Отправка почты", Required = true)]
        [MaxLength(255)]
        public string EmailFrom { get; set; }

        [DetailView("Логин", TabName = "[0]Отправка почты")]
        [MaxLength(255)]
        public string SmtpAccountLogin { get; set; }

        [DetailView("Пароль", TabName = "[0]Отправка почты")]
        [MaxLength(255)]
        [PropertyDataType(PropertyDataType.Password)]
        public string SmtpAccountPassword { get; set; }

        [DetailView("Без авторизации", TabName = "[0]Отправка почты")]
        public bool SmtpWithoutCredentials { get; set; }

        [DetailView("Использовать SSL", TabName = "[0]Отправка почты")]
        public bool SmtpUseSsl { get; set; }

        [DetailView("Протокол", TabName = "[1]Получение почты")]
        public Protocol Protocol { get; set; }

        [DetailView("Почт.сервер", TabName = "[1]Получение почты")]
        [MaxLength(255)]
        public string ServerAddress { get; set; }

        [DetailView("Порт", TabName = "[1]Получение почты")]
        public int ServerPort { get; set; } = 995;

        [DetailView("Использовать SSL", TabName = "[1]Получение почты")]
        public bool UseSsl { get; set; }
    }
}