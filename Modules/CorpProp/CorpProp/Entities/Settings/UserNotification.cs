using Base.Attributes;
using Base.Notification.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Settings
{
    /// <summary>
    /// Представляет уведомление пользователя в Системе и по email.
    /// </summary>
    /// <remarks>
    /// Расширение стандартного Base.Notification.Entities.Notification.
    /// </remarks>
    [Serializable]
    public class UserNotification : Notification
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса UserNotification.
        /// </summary>
        public UserNotification() : base()
        {
            Oid = System.Guid.NewGuid();
        }

        /// <summary>
        /// Получает или задает Oid.
        /// </summary>
        [SystemProperty]
        public Guid Oid { get; private set; }

        /// <summary>
        /// Получает или задает признак, что тело уведомления HTML.
        /// </summary>
        [DetailView("Это HTML"), ListView]
        [DefaultValue(false)]
        public bool IsHTML { get; set; }

        /// <summary>
        /// Получает или задает содержимое HTML уведомления.
        /// </summary>
        [DetailView("Содержимое HTML"), ListView]
        [PropertyDataType(PropertyDataType.Html)]
        public string HtmlBody { get; set; }

        /// <summary>
        /// Получает или задает признак необходимости отправки уведомления по email.
        /// </summary>
        [DetailView("Отправить по E-mail"), ListView]
        [DefaultValue(false)]
        public bool ByEmail { get; set; }

        /// <summary>
        /// Получает или задает признак факта отправки уведомления по email.
        /// </summary>
        [DetailView("Отправлено по E-mail"), ListView]
        [DefaultValue(false)]
        public bool IsSentByEmail { get; set; }

        /// <summary>
        /// Получает или задает признак факта просмотра уведомления в Системе.
        /// </summary>
        [DetailView("Просмотрено в Системе"), ListView]
        [DefaultValue(false)]
        public bool IsReadInSystem { get; set; }

        /// <summary>
        /// Получает или задает адресатов email уведомления.
        /// </summary>
        [DetailView("Адресат (E-mail)"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string EmailRecipient { get; set; }

        /// <summary>
        /// Получает или задает текст ошибки при попытке отправки уведомления по email.
        /// </summary>
        [DetailView("Текст ошибки отправки e-mail"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string EmailSendError { get; set; }

        [DetailView("Шаблон", Visible = false), ListView(Visible = false)]
        public virtual UserNotificationTemplate Template { get; set; }

        [DetailView("ИД шаблона", Visible = false), ListView(Visible = false)]
        public int? TemplateID { get; set; }
    }
}