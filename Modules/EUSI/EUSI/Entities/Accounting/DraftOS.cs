using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.NSI;
using EUSI.Entities.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{
    /// <summary>
    /// Представляет проекцию ОС/НМА и заявок на регистрацию для контроля полученных данных по прототипам ОС/НМА из БУС.
    /// </summary>
    public class DraftOS : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DraftOS.
        /// </summary>
        public DraftOS(): base()
        {
        }

        /// <summary>
        /// Получает номер ЕУСИ объекта имущества.
        /// </summary>
        [DetailView("Номер ЕУСИ")]
        [ListView("Номер ЕУСИ")]  
        [FullTextSearchProperty]
        public int? EUSINumber { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>        
        [ListView("Инвентарный номер")]
        [DetailView("Инвентарный номер")]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает балансовую единицу (единицу консолидации).
        /// </summary>
        [DetailView("БЕ")]
        [ListView("БЕ")]
        [FullTextSearchProperty]
        public virtual Consolidation Consolidation { get; set; }

        /// <summary>
        /// Получает или задает ИД балансовой единицы.
        /// </summary>
        [SystemProperty]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает наименование ЕУСИ.
        /// </summary>
        [DetailView("Наименование ЕУСИ")]
        [ListView("Наименование ЕУСИ")]
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameEUSI { get; set; }

        /// <summary>
        /// Получает или задает состояние объекта по РСБУ.
        /// </summary>
        [ListView("Состояние объекта РСБУ")]
        [DetailView("Состояние объекта РСБУ")]
        public virtual StateObjectRSBU StateObjectRSBU { get; set; }

        /// <summary>
        /// Получает или задает ИД состояния объекта по РСБУ.
        /// </summary>
        [SystemProperty]
        public int? StateObjectRSBUID { get; set; }


        /// <summary>
        /// Получает или задает дату выгрузки в БУС.
        /// </summary>
        [ListView("Дата выгрузки в БУС", Visible = false)]
        [DetailView("Дата выгрузки в БУС", Visible = false)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? TransferBUSDate { get; set; }

        /// <summary>
        /// Получает или задает номер заявки.
        /// </summary>
        [ListView("Номер заявки")]
        [DetailView("Номер заявки")]
        public int? ERNumber { get; set; }


        /// <summary>
        /// Получает или задает Инициатора (ФИО) заявки.
        /// </summary>
        [SystemProperty]
        [DetailView("Инициатор (ФИО)")]
        [ListView("Инициатор (ФИО)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ERContactName { get; set; }

        /// <summary>
        /// Получает или задает e-mail инициатора заявки.
        /// </summary>
        [SystemProperty]
        [DetailView("Email")]
        [ListView("Email")]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string ERContactEmail { get; set; }

        /// <summary>
        /// Получает или задает Телефон инициатора заявки.
        /// </summary>
        [SystemProperty]
        [DetailView("Телефон")]
        [ListView("Телефон")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ERContactPhone { get; set; }


        /// <summary>
        /// Дата проверки (выполнения) заявки.
        /// </summary>
        [ListView("Дата проверки (выполнения) заявки")]
        [DetailView("Дата проверки (выполнения) заявки")]
        public DateTime? DateVerification { get; set; }

        /// <summary>
        /// Комментарий.
        /// </summary>
        [SystemProperty]
        [DetailView("Комментарий")]
        [ListView("Комментарий")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comment { get; set; }

        /// <summary>
        /// Получает или задает признак успешной отправки уведомления о получении данных из БУС по e-mail.
        /// </summary>        
        [SystemProperty]
        [DetailView("Отправлено по E-mail инициатору", Visible = false)]
        [ListView(Visible = false)]
        public bool NotifyOriginator { get; set; }

        /// <summary>
        /// Получает или задает дату/время отправки уведомления о получении данных из БУС по e-mail.
        /// </summary>        
        [SystemProperty]
        [DetailView("Дата/Время отправки уведомления инициатору", Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? NotifyOriginatorDate { get; set; }

        /// <summary>
        /// Получает или задает признак успешной отправки уведомления о получении данных из БУС по e-mail.
        /// </summary>        
        [SystemProperty]
        [DetailView("Отправлено по E-mail ответственному в БУС", Visible = false)]
        [ListView(Visible = false)]
        public bool NotifyBUS { get; set; }

        /// <summary>
        /// Получает или задает дату/время отправки уведомления о получении данных из БУС по e-mail.
        /// </summary>        
        [SystemProperty]
        [DetailView("Дата/Время отправки уведомления ответственному БУС", Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? NotifyBUSDate { get; set; }
    }

       
}
