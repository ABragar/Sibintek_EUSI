using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using EUSI.Entities.NSI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{
    /// <summary>
    /// Представляет регистр движений (упрощенное внедрение) МСФО.
    /// </summary>
    [EnableFullTextSearch]
    public class AccountingMovingMSFO : AccountingMoving
    {
        /// <summary>
        /// Инифиализирует новы йэкземпляр класса AccountingMovingMSFO.
        /// </summary>
        public AccountingMovingMSFO(): base()
        {

        }

        /// <summary>
        /// Получает или задает тип движения МСФО.
        /// </summary>
        [ListView("Тип", Visible = false)]
        [DetailView("Тип", Visible = false)]          
        public TypeMovingMSFO TypeMovingMSFO { get; set; }

        /// <summary>
        /// Получает или задает ИД амортизационной группы (дебет).
        /// </summary>
        [SystemProperty]
        public int? DepGroupDebitID { get; set; }


        /// <summary>
        /// Получает или заадет амортизационную группу (дебет).
        /// </summary>
        [ListView("Амортизационная группа (Дебет)", Visible = false)]
        [DetailView("Амортизационная группа (Дебет)", Visible = false)]
        [ForeignKey("DepGroupDebitID")]
        public virtual DepreciationGroup DepGroupDebit { get; set; }


        /// <summary>
        /// Получает или задает ИД амортизационной группы (кредит).
        /// </summary>
        [SystemProperty]
        public int? DepGroupCreditID { get; set; }


        /// <summary>
        /// Получает или заадет амортизационную группу (Кредит).
        /// </summary>
        [ListView("Амортизационная группа (Кредит)", Visible = false)]
        [DetailView("Амортизационная группа (Кредит)", Visible = false)]
        [ForeignKey("DepGroupCreditID")]
        public virtual DepreciationGroup DepGroupCredit { get; set; }

              

        /// <summary>
        /// Получает или задает Бизнес – единица (Аналитика 97 ИКСО) (Дебет).
        /// </summary>
        [ListView("Бизнес–единица (Аналитика 97 ИКСО) (Дебет)", Visible = false)]
        [DetailView("Бизнес–единица (Аналитика 97 ИКСО) (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BusinessUnitDebit { get; set; }

        /// <summary>
        /// Получает или задает Бизнес – единица (Аналитика 97 ИКСО) (Кредит).
        /// </summary>
        [ListView("Бизнес–единица (Аналитика 97 ИКСО) (Кредит)", Visible = false)]
        [DetailView("Бизнес–единица (Аналитика 97 ИКСО) (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BusinessUnitCredit { get; set; }


        /// <summary>
        /// Получает или задает ИД Бизнес – сфера (Дебет).
        /// </summary> 
        [SystemProperty]
        public int? BusinessAreaDebitID { get; set; }

        /// <summary>
        /// Получает или задает Бизнес – сфера (Дебет).
        /// </summary>
        [ListView("Бизнес – сфера (Дебет)", Visible = false)]
        [DetailView("Бизнес – сфера (Дебет)", Visible = false)]
        [ForeignKey("BusinessAreaDebitID")]
        public virtual BusinessArea BusinessAreaDebit { get; set; }

        /// <summary>
        /// Получает или задает ИД Бизнес – сфера (Кредит).
        /// </summary> 
        [SystemProperty]
        public int? BusinessAreaCreditID { get; set; }

        /// <summary>
        /// Получает или задает Бизнес – сфера (Кредит).
        /// </summary>
        [ListView("Бизнес – сфера (Кредит)", Visible = false)]
        [DetailView("Бизнес – сфера (Кредит)", Visible = false)]
        [ForeignKey("BusinessAreaCreditID")]
        public virtual BusinessArea BusinessAreaCredit { get; set; }





        /// <summary>
        /// Получает или задает Группа объектов (Аналитика 24 ИКСО) (Дебет).
        /// </summary>
        [ListView("Группа объектов (Аналитика 24 ИКСО) (Дебет)", Visible = false)]
        [DetailView("Группа объектов (Аналитика 24 ИКСО) (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string GroupObjDebit { get; set; }

        /// <summary>
        /// Получает или задает Группа объектов (Аналитика 24 ИКСО) (Кредит).
        /// </summary>
        [ListView("Группа объектов (Аналитика 24 ИКСО) (Кредит)", Visible = false)]
        [DetailView("Группа объектов (Аналитика 24 ИКСО) (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string GroupObjCredit { get; set; }


        /// <summary>
        /// Получает или задает Группа объектов/Вид НМА (Аналитики 11, 103 ИКСО) (Дебет).
        /// </summary>
        [ListView("Группа объектов/Вид НМА (Аналитики 11, 103 ИКСО) (Дебет)", Visible = false)]
        [DetailView("Группа объектов/Вид НМА (Аналитики 11, 103 ИКСО) (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string GroupObjTypeDebit { get; set; }

        /// <summary>
        /// Получает или задает Группа объектов/Вид НМА (Аналитики 11, 103 ИКСО) (Кредит).
        /// </summary>
        [ListView("Группа объектов/Вид НМА (Аналитики 11, 103 ИКСО) (Кредит)", Visible = false)]
        [DetailView("Группа объектов/Вид НМА (Аналитики 11, 103 ИКСО) (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string GroupObjTypeCredit { get; set; }

        /// <summary>
        /// Получает или задает дату документа.
        /// </summary>
        [ListView("Дата документа", Visible = false)]
        [DetailView("Дата документа", Visible = false)]
        public DateTime? DocDate { get; set; }

        
        /// <summary>
        /// Получает или задает Дата оприходования.
        /// </summary>
        [ListView("Дата оприходования", Visible = false)]
        [DetailView("Дата оприходования", Visible = false)]
        public DateTime? DateOfReceipt { get; set; }
               

        /// <summary>
        /// Получает или задает Дополнительная аналитика 1 (Дебет).
        /// </summary>
        [ListView("Дополнительная аналитика 1 (Дебет)", Visible = false)]
        [DetailView("Дополнительная аналитика 1 (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string AnalyticOneDebit { get; set; }

        /// <summary>
        /// Получает или задает Дополнительная аналитика 1 (Кредит).
        /// </summary>
        [ListView("Дополнительная аналитика 1 (Кредит)", Visible = false)]
        [DetailView("Дополнительная аналитика 1 (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string AnalyticOneCredit { get; set; }


        /// <summary>
        /// Получает или задает Дополнительная аналитика 2 (Дебет).
        /// </summary>
        [ListView("Дополнительная аналитика 2 (Дебет)", Visible = false)]
        [DetailView("Дополнительная аналитика 2 (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string AnalyticTwoDebit { get; set; }

        /// <summary>
        /// Получает или задает Дополнительная аналитика 2 (Кредит).
        /// </summary>
        [ListView("Дополнительная аналитика 2 (Кредит)", Visible = false)]
        [DetailView("Дополнительная аналитика 2 (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string AnalyticTwoCredit { get; set; }

        /// <summary>
        /// Получает или задает Инвентарный номер в учетной системе (Дебет).
        /// </summary>
        [ListView("Инвентарный номер в учетной системе (Дебет)", Visible = false)]
        [DetailView("Инвентарный номер в учетной системе (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string InventoryDebit { get; set; }

        /// <summary>
        /// Получает или задает Инвентарный номер в учетной системе (Кредит).
        /// </summary>
        [ListView("Инвентарный номер в учетной системе (Кредит)", Visible = false)]
        [DetailView("Инвентарный номер в учетной системе (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string InventoryCredit { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-2014.
        /// </summary>
        [SystemProperty]
        public int? OKOFDebitID { get; set; }

        /// <summary>
        /// Получает или задает Код ОКОФ (для ОГ РФ) (Дебет).
        /// </summary>       
        [ListView("Код ОКОФ (для ОГ РФ) (Дебет)", Visible = false)]
        [DetailView("Код ОКОФ (для ОГ РФ) (Дебет)", Visible = false)]
        [ForeignKey("OKOFDebitID")]
        public virtual OKOF2014 OKOFDebit { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-2014.
        /// </summary>
        [SystemProperty]
        public int? OKOFCreditID { get; set; }

        /// <summary>
        /// Получает или задает Код ОКОФ (для ОГ РФ) (Кредит).
        /// </summary>       
        [ListView("Код ОКОФ (для ОГ РФ) (Кредит)", Visible = false)]
        [DetailView("Код ОКОФ (для ОГ РФ) (Кредит)", Visible = false)]
        [ForeignKey("OKOFCreditID")]
        public virtual OKOF2014 OKOFCredit { get; set; }

        /// <summary>
        /// Получает или задает Код показателя ИКСО Амортизация (Дебет).
        /// </summary>
        [ListView("Код показателя ИКСО Амортизация (Дебет)", Visible = false)]
        [DetailView("Код показателя ИКСО Амортизация (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string IXODepreciationDebit { get; set; }

        /// <summary>
        /// Получает или задает Код показателя ИКСО Амортизация (Кредит).
        /// </summary>
        [ListView("Код показателя ИКСО Амортизация (Кредит)", Visible = false)]
        [DetailView("Код показателя ИКСО Амортизация (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string IXODepreciationCredit { get; set; }

        /// <summary>
        /// Получает или задает Код показателя ИКСО Первоначальная стоимость (Дебет).
        /// </summary>
        [ListView("Код показателя ИКСО Первоначальная стоимость (Дебет)", Visible = false)]
        [DetailView("Код показателя ИКСО Первоначальная стоимость (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string IXOInitialDebit { get; set; }

        /// <summary>
        /// Получает или задает Код показателя ИКСО Первоначальная стоимость (Кредит).
        /// </summary>
        [ListView("Код показателя ИКСО Первоначальная стоимость (Кредит)", Visible = false)]
        [DetailView("Код показателя ИКСО Первоначальная стоимость (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string IXOInitialCredit { get; set; }

        /// <summary>
        /// Получает или задает Контрагент (Имя кредитора) (Кредит).
        /// </summary>
        [ListView("Контрагент (Имя кредитора) (Кредит)", Visible = false)]
        [DetailView("Контрагент (Имя кредитора) (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string Contragent { get; set; }


        /// <summary>
        /// Получает или задает Наименование объекта (Дебет).
        /// </summary>
        [ListView("Наименование объекта (Дебет)", Visible = false)]
        [DetailView("Наименование объекта (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string NameDebit { get; set; }


        /// <summary>
        /// Получает или задает Номер месторождения (Дебет).
        /// </summary>
        [ListView("Номер месторождения (Дебет)", Visible = false)]
        [DetailView("Номер месторождения (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string DepositDebit { get; set; }

        /// <summary>
        /// Получает или задает Номер месторождения (Кредит).
        /// </summary>
        [ListView("Номер месторождения (Кредит)", Visible = false)]
        [DetailView("Номер месторождения (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string DepositCredit { get; set; }

       
        /// <summary>
        /// Получает или задает Номер партии (Дебет).
        /// </summary>
        [ListView("Номер партии (Дебет)", Visible = false)]
        [DetailView("Номер партии (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BatchNumberDebit { get; set; }

        /// <summary>
        /// Получает или задает Номер партии (Кредит).
        /// </summary>
        [ListView("Номер партии (Кредит)", Visible = false)]
        [DetailView("Номер партии (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BatchNumberCredit { get; set; }

        /// <summary>
        /// Получает или задает Номер СПП (Дебет).
        /// </summary>
        [ListView("Номер СПП (Дебет)", Visible = false)]
        [DetailView("Номер СПП (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string SPPDebit { get; set; }

        /// <summary>
        /// Получает или задает Оставшийся срок службы по бухгалтерскому учету (месяцев) (Дебет).
        /// </summary>
        [ListView("Оставшийся срок службы по БУ (месяцев) (Дебет)", Visible = false)]
        [DetailView("Оставшийся срок службы по БУ (месяцев) (Дебет)", Visible = false)]      
        [FullTextSearchProperty]
        public int? UsefulEndDebit { get; set; }

        
        /// <summary>
        /// Получает или задает Партнер (кому) (Кредит).
        /// </summary>
        [ListView("Партнер (кому) (Кредит)", Visible = false)]
        [DetailView("Партнер (кому) (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string PartnerWhoCredit { get; set; }

        /// <summary>
        /// Получает или задает Партнер (Кредит).
        /// </summary>
        [ListView("Партнер (Кредит)", Visible = false)]
        [DetailView("Партнер (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string PartnerCredit { get; set; }


        /// <summary>
        /// Получает или задает Подпозиция консолидации.
        /// </summary>
        [ListView("Подпозиция консолидации", Visible = false)]
        [DetailView("Подпозиция консолидации", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string SubPosition { get; set; }


        /// <summary>
        /// Получает или задает Позиция консолидации (Дебет).
        /// </summary>
        [ListView("Позиция консолидации (Дебет)", Visible = false)]
        [DetailView("Позиция консолидации (Дебет)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string PositionDebit { get; set; }


        /// <summary>
        /// Получает или задает Позиция консолидации (Кредит).
        /// </summary>
        [ListView("Позиция консолидации (Кредит)", Visible = false)]
        [DetailView("Позиция консолидации (Кредит)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string PositionCredit { get; set; }

        /// <summary>
        /// Получает или задает Позиция сторно.
        /// </summary>
        [ListView("Позиция сторно", Visible = false)]
        [DetailView("Позиция сторно", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string PositionStorno { get; set; }


        /// <summary>
        /// Получает или задает Позиция сторно (признак).
        /// </summary>
        [ListView("Позиция сторно (признак)", Visible = false)]
        [DetailView("Позиция сторно (признак)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string PositionStornoSign { get; set; }

        /// <summary>
        /// Получает или задает пояснение к операции.
        /// </summary>
        [ListView("Пояснение к операции", Visible = false)]
        [DetailView("Пояснение к операции", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Explanation { get; set; }



        /// <summary>
        /// Получает или задает Признак выбытия (полное или частичное) (Дебет).
        /// </summary>
        [ListView("Признак выбытия (полное или частичное) (Дебет)", Visible = false)]
        [DetailView("Признак выбытия (полное или частичное) (Дебет)", Visible = false)]
        [DefaultValue(0)]
        public bool IsLeavingDebit { get; set; }


        /// <summary>
        /// Получает или задает СПП-элемент (Дебет).
        /// </summary>
        [ListView("СПП-элемент (Дебет)", Visible = false)]
        [DetailView("СПП-элемент (Дебет)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string SPPItemDebit { get; set; }

        /// <summary>
        /// Получает или задает СПП-элемент (Кредит).
        /// </summary>
        [ListView("СПП-элемент (Кредит)", Visible = false)]
        [DetailView("СПП-элемент (Кредит)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string SPPItemCredit { get; set; }

        /// <summary>
        /// Получает или задает Срок полезного использования по БУ (мес) (Дебет).
        /// </summary>
        [ListView("Срок полезного использования по БУ (мес) (Дебет)", Visible = false)]
        [DetailView("Срок полезного использования по БУ (мес) (Дебет)", Visible = false)]       
        public int? UsefulDebit { get; set; }

        /// <summary>
        /// Получает или задает Сумма (Стоимость по БУ).
        /// </summary>
        [ListView("Сумма (Стоимость по БУ)", Visible = false)]
        [DetailView("Сумма (Стоимость по БУ)", Visible = false)]
        public decimal? Cost { get; set; }

        /// <summary>
        /// Получает или задает Сумма начисленной амортизации по бухгалтерскому учету (руб.).
        /// </summary>
        [ListView("Сумма начисленной амортизации по БУ (руб.)", Visible = false)]
        [DetailView("Сумма начисленной амортизации по БУ (руб.)", Visible = false)]
        public decimal? CostDepreciation { get; set; }

        /// <summary>
        /// Получает или задает Счет ГК (учетной системы) (Дебет)
        /// </summary>
        [ListView("Счет ГК (учетной системы) (Дебет)", Visible = false)]
        [DetailView("Счет ГК (учетной системы) (Дебет)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountGKDebit { get; set; }

        /// <summary>
        /// Получает или задает Счет ГК (учетной системы) (Кредит).
        /// </summary>
        [ListView("Счет ГК (учетной системы) (Кредит)", Visible = false)]
        [DetailView("Счет ГК (учетной системы) (Кредит)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountGKCredit { get; set; }
        
        
        /// <summary>
        /// Получает или задает Уникальный номер в учетной системе (Дебет).
        /// </summary>
        [ListView("Уникальный номер в учетной системе (Дебет)", Visible = false)]
        [DetailView("Уникальный номер в учетной системе (Дебет)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExternalIDDebit { get; set; }


        /// <summary>
        /// Получает или задает Уникальный номер в учетной системе (Кредит).
        /// </summary>
        [ListView("Уникальный номер в учетной системе (Кредит)", Visible = false)]
        [DetailView("Уникальный номер в учетной системе (Кредит)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExternalIDCredit { get; set; }

        

        /// <summary>
        /// Получает или задает Хозяйственная операция.
        /// </summary>
        [ListView("Хозяйственная операция", Visible = false)]
        [DetailView("Хозяйственная операция", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Operation { get; set; }

    }


    /// <summary>
    /// Тип движения (упрощенное внедрение)
    /// </summary>
    [UiEnum]
    public enum TypeMovingMSFO
    {
        [UiEnumValue("Кредитование 01")]
        Credit01 = 1,
        [UiEnumValue("Кредитование 07")]
        Credit07 = 2,
        [UiEnumValue("Кредитование 08")]
        Credit08 = 3,
        [UiEnumValue("Дебетование 01")]
        Debit01 = 4,
        [UiEnumValue("Дебетование 07")]
        Debit07 = 5,
        [UiEnumValue("Дебетование 08")]
        Debit08 = 6,   
        [UiEnumValue("Амортизация 01")]
        Depreciation01 = 7,
        
    }

}
