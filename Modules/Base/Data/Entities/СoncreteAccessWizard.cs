using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Contact.Entities;
using Base.PBX.Entities;
using Base.Security.Entities.Concrete;
using Base.Security.Service;
using Base.Translations;

namespace Data.Entities
{
    public class СoncreteAccessWizard : AccessUserWizard
    {
        [DetailView("Создать сотрудника")]
        public bool CreateEmployee { get; set; } = true;

        [DetailView("Создать почтовый ящик")]
        public bool CreateMailAccount { get; set; }

        [DetailView("Создать SIP аккаунт")]
        public bool CreateSipAccount { get; set; }

        #region mail_account

        [DetailView("Email/Логин", Required = true)]
        [PropertyDataType("MailAccount")]
        public string MailAccount { get; set; }

        [SystemProperty]
        public string MailDomain { get; set; }

        #endregion

        #region employee

        [DetailView("Компания", Required = true)]
        public Company Company { get; set; }

        [DetailView("Департамент", Required = true)]
        public Department Department { get; set; }

        [DetailView("Должность", Required = true)]
        public EmployeePost Post { get; set; }

        #endregion

        #region SIP

        [DetailView("Сервер телефонии", Required = true)]
        public virtual PBXServer PBXServer { get; set; }

        [DetailView("Добавочный номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string extension { get; set; }

        [DetailView("Номер CallerID")]
        [PropertyDataType(PropertyDataType.Text)]
        public string cidnumber { get; set; }

        [DetailView("Пароль SIP/IAX", Required = true)]
        [PropertyDataType(PropertyDataType.Password)]
        public string secret { get; set; }

        [DetailView("Пароль голосовой почты (0-9)", Required = true)]
        [PropertyDataType(PropertyDataType.Password)]
        public string vmsecret { get; set; } //just numbers

        [DetailView("Пароль", Required = true)]
        [PropertyDataType(PropertyDataType.Password)]
        public string user_password { get; set; }

        [DetailView("Отправлять e-mail")]
        public bool email_to_user { get; set; }

        [DetailView("Использовать WebRTC")]
        public bool enable_webrtc { get; set; } = true;

        [DetailView("Голосовая почта")]
        public bool hasvoicemail { get; set; } = true;

        [DetailView("Аудио запись")]
        public bool auto_record { get; set; } = true;

        #endregion
    }
}