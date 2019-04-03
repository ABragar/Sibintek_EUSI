using Base.Attributes;
using Base.Entities.Complex;
using CorpProp.Entities.Base;
using CorpProp.Entities.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpProp.Entities.Settings
{
    /// <summary>
    /// Настройки условий отправки уведомлений по расписанию.
    /// </summary>
    public class SibNotification : TypeObject
    {
        /// <summary>
        /// Мнемоника.
        /// </summary>
        [ListView("Объект")]
        [DetailView("Объект", Required = true)]
        //[PropertyDataType(PropertyDataType.Mnemonic)]
        [PropertyDataType("Sib_NotificationMnemonic")]
        public string Mnemonic { get; set; }

        [ListView("Напомнить до")]
        [DetailView("Напомнить до", Description = "Напоминание сработает за указанный период до наступления даты в выбранном поле.")]
        //[PropertyDataType(PropertyDataType.Mnemonic)]
        [PropertyDataType("Sib_NotificationProperty")]
        public string PropertyName { get; set; }

        /// <summary>
        /// Напоминание. Состоит из Типа (Неделя, день, час) и срока.
        /// </summary>
        [DetailView("Напоминание")]
        public RemindPeriod RemindPeriod { get; set; } = new RemindPeriod();

        /// <summary>
        /// Тема.
        /// </summary>
        [ListView("Тема")]
        [DetailView("Тема")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Subject { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        [ListView]
        [DetailView("Сообщение")]
        public string Message { get; set; }

        /// <summary>
        /// Получатель.
        /// </summary>
        [ListView]
        [ForeignKey("Reciever")]
        public int? RecieverID { get; set; }
        
        [DetailView("Получатель")]
        public virtual SibUser Reciever { get; set; }

        /// <summary>
        /// Признак отправки соисполнителям по задаче.
        /// </summary>
        [ListView]
        [DetailView("Отправить соисполнителям задачи")]
        public bool SendToResponsibles { get; set; }

        /// <summary>
        /// Признак отправки уведомления во все ОГ.
        /// </summary>
        [ListView]
        [DetailView("Отправить всем сотрудникам ОГ")]
        public bool SendToAllSocieties { get; set; }

        /// <summary>
        /// Признак активности уведомления.
        /// </summary>
        [ListView("Активно")]
        [DetailView("Уведомление включено")]
        public bool IsEnabled { get; set; }

        [DetailView(Name = "Дополнительные параметры", Description = "Что-то, где-то делают.", Visible = false)]
        public string FilterExpression { get; set; }

        public int? ItemID { get; set; }
    }

    /// <summary>
    /// Объект уведомления.
    /// </summary>
    [NotMapped]
    public class SibNotificationObject
    {
        public string Message { get; set; }
        public string Subject { get; set; }
        public List<int> Recipients { get; set; }
        public LinkBaseObject LinkBaseObject { get; set; }
    }
}
