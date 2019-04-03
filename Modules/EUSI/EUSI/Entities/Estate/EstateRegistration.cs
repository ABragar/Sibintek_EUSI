using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using EUSI.Entities.NSI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EUSI.Entities.Estate
{
    /// <summary>
    /// Представляет форму заявки на регистрацию ОИ.
    /// </summary>
    [EnableFullTextSearch]
    public class EstateRegistration : TypeObject, IBPObject
    {
        private static readonly CompiledExpression<EstateRegistration, string> _NumberString =
         DefaultTranslationOf<EstateRegistration>
            .Property(x => x.NumberString)
            .Is(x => (x.Number != 0) ? x.Number.ToString() : "");

        /// <summary>
        /// Инициализирует новый экземпляр класса EstateRegistration.
        /// </summary>
        public EstateRegistration() : base()
        {
        }

        /// <summary>
        /// Получает или задает ФИО инициатора.
        /// </summary>
        [SystemProperty]
        [DetailView("ФИО инициатора", Visible = false)]
        [ListView("ФИО инициатора", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContacName { get; set; }

        /// <summary>
        /// Получает или задает e-mail инициатора.
        /// </summary>
        [SystemProperty]
        [DetailView("Email инициатора", Visible = false)]
        [ListView("Email инициатора", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string ContactEmail { get; set; }

        /// <summary>
        /// Получает или задает Телефон инициатора.
        /// </summary>
        [SystemProperty]
        [DetailView("Телефон инициатора", Visible = false)]
        [ListView("Телефон инициатора", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Получает или задает дату заявки.
        /// </summary>
        [DetailView("Дата заявки", Visible = false)]
        [ListView("Дата заявки", Hidden = false)]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Получает или задает номер заявки.
        /// </summary>
        [DetailView("Номер заявки", Visible = false, ReadOnly = true)]
        [ListView("Номер заявки", Visible = false)]
        //[SystemProperty]
        [FullTextSearchProperty]
        public int Number { get; set; }

        /// <summary>
        /// Получает номер заявки (строкой).
        /// </summary>
        [DetailView("Номер заявки (строка)", ReadOnly = true, Visible = false)]
        [ListView("Номер заявки (строка)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberString => _NumberString.Evaluate(this);

        /// <summary>
        /// Получает или задает номер заявки ЦДС.
        /// </summary>
        [DetailView("№ заявки ЦДС", ReadOnly = false, Visible = false)]
        [ListView("№ заявки ЦДС", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberCDS { get; set; }

        /// <summary>
        /// Получает или задает ИД ОГ.
        /// </summary>
        [SystemProperty]
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает общество группы.
        /// </summary>
        [DetailView("Общество группы", Required = false, Visible = false)]
        [ListView("Общество группы", Visible = false)]
        public virtual Society Society { get; set; }

        /// <summary>
        /// Получает или задает ИД вида объекта заявки.
        /// </summary>
        [SystemProperty]
        public int? ERTypeID { get; set; }

        /// <summary>
        /// Получает или задает вид объекта заявки.
        /// </summary>
        [DetailView("Вид объекта заявки", Visible = false)]
        [ListView("Вид объекта заявки", Visible = false)]
        public virtual EstateRegistrationTypeNSI ERType { get; set; }

        /// <summary>
        /// Получает или задает ИД способа поступления.
        /// </summary>
        [SystemProperty]
        public int? ERReceiptReasonID { get; set; }

        /// <summary>
        /// Получает или задает способ поступления.
        /// </summary>
        [DetailView("Способ поступления", Visible = false)]
        [ListView("Способ поступления", Visible = false)]
        public virtual ERReceiptReason ERReceiptReason { get; set; }

        /// <summary>
        /// Получает или задает ИД поставщика.
        /// </summary>
        [SystemProperty]
        public int? ContragentID { get; set; }

        /// <summary>
        /// Получает или задает Контрагента.
        /// </summary>
        [DetailView("Контрагент", Visible = false)]
        [ListView("Контрагент", Visible = false)]
        public virtual Subject Contragent { get; set; }

        /// <summary>
        /// Получает или задает ИД БЕ.
        /// </summary>
        [SystemProperty]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает БЕ.
        /// </summary>
        [DetailView("БЕ", Visible = false)]
        [ListView("БЕ", Visible = false)]
        public virtual Consolidation Consolidation { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса.
        /// </summary>
        [SystemProperty]
        public int? StateID { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        [DetailView("Статус", ReadOnly = true, Visible = false)]
        [ListView("Статус", Visible = false)]
        [ForeignKey("StateID")]
        public virtual EstateRegistrationStateNSI State { get; set; }

        /// <summary>
        /// Получает или задает ИД заявителя.
        /// </summary>
        [SystemProperty]
        public int? OriginatorID { get; set; }

        /// <summary>
        /// Получает или изадает заявителя заявки.
        /// </summary>
        [DetailView("Заявитель", Visible = false)]
        [ListView("Заявитель", Visible = false)]
        public virtual EstateRegistrationOriginator Originator { get; set; }

        /// <summary>
        /// Получает или задает ИД файла заявки.
        /// </summary>
        [SystemProperty]
        public int? FileCardID { get; set; }

        /// <summary>
        /// Карточка файла Excel из которой была создана заявка.
        /// </summary>
        [DetailView("Файл заявки", Visible = false)]
        [ListView("Файл заявки", Visible = false)]
        public virtual FileCard FileCard { get; set; }

        /// <summary>
        /// Получает или задает ИД WF контекста.
        /// </summary>
        [SystemProperty]
        public int? WorkflowContextID { get; set; }

        /// <summary>
        /// Получает или задает WF контекст.
        /// </summary>
        public WorkflowContext WorkflowContext { get; set; }

        /// <summary>
        /// Получает или задает признак срочности исполнения заявки.
        /// </summary>
        [DetailView("Срочная", Visible = false)]
        [ListView("Срочная", Visible = false)]
        [DefaultValue(false)]
        public bool Urgently { get; set; }

        /// <summary>
        /// Получает или задает признак выгрузки ОС заявки в файл БУС.
        /// </summary>
        [DetailView("Выгружено в БУС", Visible = false)]
        [ListView("Выгружено в БУС", Visible = false)]
        [DefaultValue(false)]
        public bool TransferBUS { get; set; }

        /// <summary>
        /// Получает или задает дату выгрузки в БУС.
        /// </summary>
        [ListView("Дата выгрузки в БУС", Visible = false)]
        [DetailView("Дата выгрузки в БУС", Visible = false)]
        public DateTime? TransferBUSDate { get; set; }

        /// <summary>
        /// Получает или задает последний комментарий.
        /// </summary>
        /// <remarks>
        /// Используетя при отклонении заявки как системное поле.
        /// </remarks>
        [ListView("Последний комментарий", Visible = false)]
        [DetailView("Последний комментарий", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string LastComment { get; set; }

        /// <summary>
        /// Получает или задает атрибут "Комментарий".
        /// </summary>
        [DetailView(Name = "Комментарий", Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comment { get; set; }

        /// <summary>
        /// Получает или задает атрибут "Не актуально".
        /// </summary>
        [DetailView("Не актуально", Visible = false)]
        [ListView(Visible = false)]
        public bool NotActual { get; set; }

        /// <summary>
        /// Получает или задает № договора.
        /// </summary>
        [DetailView("№ договора", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string ERContractNumber { get; set; }

        /// <summary>
        /// Получает или задает дату договора.
        /// </summary>
        [DetailView("Дата договора", Visible = false)]
        [SystemProperty]
        public DateTime? ERContractDate { get; set; }

        /// <summary>
        /// Получает или задает № первичного документа.
        /// </summary>
        [DetailView("№ первичного документа", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string PrimaryDocNumber { get; set; }

        /// <summary>
        /// Получает или задает дату первичного документа.
        /// </summary>
        [DetailView("Дата первичного документа", Visible = false)]
        [SystemProperty]
        public DateTime? PrimaryDocDate { get; set; }

        /// <summary>
        /// Получает или задает признак быстрого закрытия.
        /// </summary>
        [DetailView("Заявка обрабатывается со статусом «Быстрое закрытие»", Visible = false), ListView(Visible = false)]
        [DefaultValue(false)]
        [SystemProperty]
        public bool QuickClose { get; set; }

        /// <summary>
        /// Получает или задает атрибуты контрольных дат.
        /// </summary>
        [DetailView("Атрибуты контрольных дат", Visible = false)]
        [ListView("Атрибуты контрольных дат", Visible = false)]
        public virtual ERControlDateAttributes ERControlDateAttributes { get; set; }

        /// <summary>
        /// Получает или задает ИД атрибутов контрольных дат.
        /// </summary>
        [SystemProperty]
        public int? ERControlDateAttributesID { get; set; }

        /// <summary>
        /// Получает или задает ИД планируемого к созданию/обновлению в результате объединения объект заявки.
        /// </summary>
        [DetailView(Visible = false)]
        [ForeignKey("ClaimObject")]
        public int? ClaimObjectID { get; set; }

        /// <summary>
        /// Получает или задает планируемый к созданию/обновлению в результате объединения объект заявки.
        /// </summary>        
        [DetailView("Объект заявки", Visible = false)]
        public virtual EstateRegistrationRow ClaimObject { get; set; }


        [InverseProperty("EstateRegistration")]
        public List<EstateRegistrationRow> EstateRegistrationRows { get; set; }

    }
}