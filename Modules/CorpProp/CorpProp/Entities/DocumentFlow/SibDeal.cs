using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.NSI;
using System.ComponentModel.DataAnnotations.Schema;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using CorpProp.Entities.Document;
using Base.Attributes;
using CorpProp.Entities.CorporateGovernance;
using System.Collections.Generic;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Estate;
using System.ComponentModel;
using System;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.ProjectActivity;

namespace CorpProp.Entities.DocumentFlow
{
   
    /// <summary>
    /// Представляет сделку.
    /// </summary>   
    public class SibDeal : Doc
    {
      
        /// <summary>
        /// Инициализирует новый экземпляр класса SibDeal.
        /// </summary>
        public SibDeal(): base()
        {
            
        }
        

        /// <summary>
        /// Получает или задает ИД типа сделки.
        /// </summary>
        public int? DealTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип сделки.
        /// </summary>      
        
        [DetailView(Name = "Тип сделки", Order = 1, TabName = CaptionHelper.DealTabName, Visible = false)]
        public virtual DealType DealType { get; set; }

       

        /// <summary>
        /// Получает или задает полный номер у контрагента.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Полный номер у контрагента", Order = 3, TabName = CaptionHelper.DealTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberContragent { get; set; }

        /// <summary>
        /// Получает или задает дату начала сделки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата С", Order = 4, TabName = CaptionHelper.DealTabName, Required = true)]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания сделки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата По", Order = 5, TabName = CaptionHelper.DealTabName, Required = true)]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Получает или задает ИД валюты сделки.
        /// </summary>
        public int? CurrencyID { get; set; }

        /// <summary>
        /// Получает или задает валюту сделки.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Валюта", Order = 6, TabName = CaptionHelper.DealTabName)]
        public virtual Currency Currency { get; set; }

        /// <summary>
        /// Получает или задает сумму сделки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сумма", Order = 7, TabName = CaptionHelper.DealTabName)]
        public decimal SumDeal { get; set; }

        /// <summary>
        /// Получает или задает сумму сделки, не разбитая пообъектно.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Сумма сделки, не разбитая пообъектно", Order = 8, TabName = CaptionHelper.DealTabName)]
        //public decimal SumDealNotBroken { get; set; }

        /// <summary>
        /// Получает или задает требование корпоративного одобрения.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Требует корп.одобрения", Order = 9, TabName = CaptionHelper.DealTabName)]
        //[DefaultValue(false)]
        //public bool IsRequiresCorporateApproval { get; set; }

        /// <summary>
        /// Получает или задает дату корпоративного одобрения.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Дата корп. одобрения", Order = 10, TabName = CaptionHelper.DealTabName)]
        //public DateTime? DateCorporateApproval { get; set; }

        /// <summary>
        /// Получает или задает орган, одобривший сделку.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Орган, одобривший сделку", Order = 11, TabName = CaptionHelper.DealTabName)]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string AuthorizedApproveDeal { get; set; }

        /// <summary>
        /// Получает или задает требование государственной регистрации
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Требует гос. регистрации", Order = 12, TabName = CaptionHelper.DealTabName)]
        [DefaultValue(false)]
        public bool IsRequiresStateRegistration { get; set; }

        /// <summary>
        /// Получает или задает дату государственной регистрации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата гос.регистрации", Order = 13, TabName = CaptionHelper.DealTabName)]
        public DateTime? DateStateRegistration { get; set; }
     
                

        

        /// <summary>
        /// Получает или задает Группу полномочий.
        /// </summary>
        [DetailView(Name = "Группа полномочий", HideLabel = false, TabName = "Заголовок договора")]
        public int? PermissionGroup { get; set; }

        /// <summary>
        /// Получает или задает Номер вышестоящего документа в Компании
        /// </summary>
        [DetailView(Name = "Номер вышестоящего документа в Компании", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ParentContractNumber { get; set; }

        /// <summary>
        /// Получает или задает Системный номер
        /// </summary>
        [DetailView(Name = "Системный номер", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SystemNumber { get; set; }

        /// <summary>
        /// Получает или задает Предмет договора
        /// </summary>
        [DetailView(Name = "Предмет договора", HideLabel = false, TabName = "Заголовок договора")]
        public string ContractSubject { get; set; }

        /// <summary>
        /// Код ЕУП (основная сторона договора, ОГ)
        /// </summary>
        [DetailView(Name = "Код ЕУП", HideLabel = false, TabName = "Заголовок договора")]
        public int? EUPCode { get; set; }

        /// <summary>
        /// Код СДП (основная сторона договора, ОГ))
        /// </summary>
        [DetailView(Name = "Код СДП", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SDPCode  { get; set; }

        /// <summary>
        /// Получает или задает строковое значение контрагентк
        /// </summary>
        [DetailView(Name = "Контрагент", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string StrContragent { get; set; }

        /// <summary>
        /// Получает или задает Граппу консолидации
        /// </summary>
        [DetailView(Name = "Группа консолидации", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string StrConsolidationGroup { get; set; }

        /// <summary>
        /// Получает или задает Подразделение
        /// </summary>
        [DetailView(Name = "Подразделение", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Department { get; set; }


        /// <summary>
        /// Получает или задает Подписанта от Компании
        /// </summary>
        [DetailView(Name = "Подписант от Компании", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string CompanySigner { get; set; }

        /// <summary>
        /// Контрагент (ДП, ОГ)
        /// </summary>
        [DetailView(Name = "Контрагент (ДП, ОГ)", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Contragent { get; set; }

        /// <summary>
        /// Получает или задает Подписанта от Контрагента
        /// </summary>
        [DetailView(Name = "Подписант от Контрагента", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContragentSigner { get; set; }

        /// <summary>
        /// Получает или задает Сумму сделки, руб. с НДС
        /// </summary>
        [DetailView(Name = "Сумма сделки, руб. с НДС", HideLabel = false, TabName = "Заголовок договора")]
        public decimal? SumWithTax { get; set; }

        /// <summary>
        /// Получает или задает Ставку НДС
        /// </summary>
        [DetailView(Name = "Ставка НДС", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Percent)]
        public decimal? TaxPercent { get; set; }

        /// <summary>
        /// Получает или задает Сумму НДС, в руб.
        /// </summary>
        [DetailView(Name = "Сумма НДС, в руб.", HideLabel = false, TabName = "Заголовок договора")]
        public decimal? TaxSum { get; set; }

        /// <summary>
        /// Получает или задает является ли сделка является долгосрочной
        /// </summary>
        [DetailView(Name = "Сделка является долгосрочной", HideLabel = false, TabName = "Аренда/Лизинг")]
        public bool? LongDeal { get; set; }

        /// <summary>
        /// Получает или задает Оставшейся срок действия"
        /// </summary>
        [DetailView(Name = "Оставшейся срок действия", HideLabel = false, TabName = "Аренда/Лизинг", ReadOnly = true)]
        public int? LeftToEnd { get; set; }
        //public virtual int? LeftToEnd { get { return CalculateLeftToEnd(); }}

        /// <summary>
        /// Получает или задает Сумму платежа, с НДС
        /// </summary>
        [DetailView(Name = "Сумма платежа, с НДС", HideLabel = false, TabName = "Аренда/Лизинг")]
        public decimal? PaymentSum { get; set; }

        /// <summary>
        /// Получает или задает Сумму НДС платежа, в руб.
        /// </summary>
        [DetailView(Name = "Сумма НДС платежа, в руб.", HideLabel = false, TabName = "Аренда/Лизинг")]
        public decimal? PaymentTaxSum { get; set; }

        /// <summary>
        /// Получает или задает Заключен ли договор субаренды
        /// </summary>
        [DetailView(Name = "Заключен договор субаренды", HideLabel = false, TabName = "Аренда/Лизинг")]
        public bool? HasSubleaseContract { get; set; }

        /// <summary> 
        /// Получает или задает День платежа
        /// </summary>
        [DetailView(Name = "День платежа", HideLabel = false, TabName = "Права/Переход права")]
        public int? PaymentDay { get; set; }

        /// <summary>
        /// Получает или задает Месяц платежа
        /// </summary>
        [DetailView(Name = "Месяц платежа", HideLabel = false, TabName = "Права/Переход права")]
        public int? PaymentMonth { get; set; }

        /// <summary>
        /// Получает или задает Год платежа
        /// </summary>
        [DetailView(Name = "Год платежа", HideLabel = false, TabName = "Права/Переход права")]
        public int? PaymentYear { get; set; }

        /// <summary>
        /// Получает или задает Количество дней по условию платежа
        /// </summary>
        [DetailView(Name = "Количество дней по условию платежа", HideLabel = false, TabName = "Права/Переход права")]
        public int? DaysCountByPayment { get; set; }

        /// <summary>
        /// Получает или задает Дату следующего платежа
        /// </summary>
        [DetailView(Name = "Дата следующего платежа", HideLabel = false, TabName = "Права/Переход права")]
        public DateTime? PaymentNextDate { get; set; }

        /// <summary>
        /// Получает или задает № записи гос. регистрации договора
        /// </summary>
        [DetailView(Name = "№ записи гос. регистрации договора", HideLabel = false, TabName = "Права/Переход права")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberStateRegistartion { get; set; }

        /// <summary>
        /// Получает или задает Требование регистрации права
        /// </summary>
        [DetailView(Name = "Требует регистрации права", HideLabel = false, TabName = "Заголовок договора")]
        public bool? IsRequiresRightRegistration { get; set; }

        /// <summary>
        /// Получает или задает № Передаточного акта
        /// </summary>
        [DetailView(Name = "№ Передаточного акта", HideLabel = false, TabName = "Заголовок договора")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TransferActNumber { get; set; }

        /// <summary>
        /// Получает или задает Дата передаточного акта
        /// </summary>
        [DetailView(Name = "Дата передаточного акта", HideLabel = false, TabName = "Права/Переход права")]
        [PropertyDataType(PropertyDataType.Text)]
        public DateTime? TransferActDate { get; set; }

        /// <summary>
        /// Получает или задает Инвентарный номер
        /// </summary>
        [DetailView(Name = "Инвентарный номер", HideLabel = false, TabName = "Спецификация")]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает Название ОС
        /// </summary>
        [DetailView(Name = "Название ОС", HideLabel = false, TabName = "Спецификация")]
        [PropertyDataType(PropertyDataType.Text)]
        public string FixedAssetsTitle { get; set; }

        /// <summary>
        /// Получает или задает Первоначальная ст. (в договорах с объектами БУ системы)
        /// </summary>
        [DetailView(Name = "Первоначальная ст. (в договорах с объектами БУ системы)", HideLabel = false, TabName = "Спецификация")]
        public decimal? InitialCost { get; set; }

        /// <summary>
        /// Получает или задает Остаточная ст. (в договорах с объектами БУ системы)
        /// </summary>
        [DetailView(Name = "Остаточная ст. (в договорах с объектами БУ системы)", HideLabel = false, TabName = "Спецификация")]
        public decimal? ResidualCost { get; set; }

        /// <summary>
        /// Получает или задает
        /// </summary>
        [DetailView(Name = "Стоимость аренды (в дог. аренды)", HideLabel = false, TabName = "Спецификация")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RentCost { get; set; }

        /// <summary>
        /// Получает или задает Стоимость реализации (в дог. реализации)
        /// </summary>
        [DetailView(Name = "Стоимость реализации (в дог. реализации)", HideLabel = false, TabName = "Спецификация")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SellCost { get; set; }

        /// <summary>
        /// Получает или задает Стоимость покупки (в дог. покупки)
        /// </summary>
        [DetailView(Name = "Стоимость покупки (в дог. покупки)", HideLabel = false, TabName = "Спецификация")]
        [PropertyDataType(PropertyDataType.Text)]
        public string BuyCost { get; set; }

        /// <summary>
        /// Получает или задает ИД Вида контрагента
        /// </summary>
        public int? ContragentKindID { get; set; }

        /// <summary>
        /// Получает или задает Вид контрагента
        /// </summary>
        [DetailView(Name = "Вид контрагента", HideLabel = false, TabName = "Заголовок договора")]
        public virtual ContragentKind ContragentKind { get; set; }

        /// <summary>
        /// Получает или задает ИД Тип операции
        /// </summary>
        public int? DocTypeOperationID { get; set; }

        /// <summary>
        /// Получает или задает Тип операции
        /// </summary>
        /// 
        [ListView (Visible =true, Order = 11)]
        [DetailView(Name = "Тип операции", HideLabel = false, TabName = CaptionHelper.DealTabName)]
        public virtual DocTypeOperation DocTypeOperation { get; set; }

        /// <summary>
        /// Получает или задает ИД Статуса
        /// </summary>
        public int? SibDealStatusID { get; set; }

        /// <summary>
        /// Получает или задает Статус
        /// </summary>
        [DetailView(Name = "Статус", HideLabel = false, TabName = "Заголовок договора")]
        public virtual SibDealStatus SibDealStatus { get; set; }

        /// <summary>
        /// Получает или задает ИД Источника информации
        /// </summary>
        public int? InformationSourceID { get; set; }

        /// <summary>
        /// Получает или задает Источник информации
        /// </summary>
        [DetailView(Name = "Источник информации", HideLabel = false, TabName = "Заголовок договора")]
        public virtual InformationSource InformationSource { get; set; }


        /// <summary>
        /// Получает или задает ИД Вышестоящего документа
        /// </summary>
        public int? ParentDealID { get; set; }

        /// <summary>
        /// Получает или задает Вышестоящий документ
        /// </summary>
        [DetailView(Name = "Вышестоящий документ", HideLabel = false, TabName = "Заголовок договора")]
        public SibDeal ParentDeal { get; set; }


        /// <summary>
        /// Получает или задает ИД БЕ.
        /// </summary>
        public int? ConsolidationUnitID { get; set; }

        /// <summary>
        /// Получает или задает БЕ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "ЕК(БЕ)", Order = 1, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual Consolidation ConsolidationUnit { get; set; }



    }
}
