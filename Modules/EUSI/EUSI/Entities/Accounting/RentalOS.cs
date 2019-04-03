using Base.Attributes;
using Base.DAL;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using EUSI.Entities.NSI;
using System;
using System.ComponentModel;

namespace EUSI.Entities.Accounting
{
    /// <summary>
    /// Представляет данные об арендованных ОС/НМА (ИР Аренда).
    /// </summary>    
    public class RentalOS : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса RentalOS.
        /// </summary>
        public RentalOS() : base()
        {

        }

        /// <summary>
        /// Получает или задает ОС/НМА.
        /// </summary>
        [DetailView("ОС/НМА"), ListView()]
        public virtual AccountingObject AccountingObject { get; set; }

        /// <summary>
        /// Получает или задает ИД ОС/НМА.
        /// </summary>
        [DetailView("ИД ОС/НМА"), ListView()]
        public int? AccountingObjectID { get; set; }

        /// <summary>
        /// Получает или задает УИД ОС/НМА.
        /// </summary>
        [DetailView("УИД ОС/НМА"), ListView()]
        public Guid? AccountingObjectOid { get; set; }

        /// <summary>
        /// Получает или задает дату получения объекта аренды (дата акта приемки-передачи).
        /// </summary>        
        [DetailView("Дата получения объекта аренды (дата акта приемки-передачи)"), ListView()]
        public DateTime? ActRentDate { get; set; }

        /// <summary>
        /// Получает или задает На чьем балансе учитывается ОС в РСБУ (арендатор/арендодатель).
        /// </summary>
        [DetailView("На чьем балансе учитывается ОС в РСБУ (арендатор/арендодатель)"), ListView()]
        public virtual AssetHolderRSBU AssetHolderRSBU { get; set; }

        /// <summary>
        /// Получает или задает ИД На чьем балансе учитывается ОС в РСБУ (арендатор/арендодатель).
        /// </summary> 
        [DetailView("ИД на чьем балансе учитывается ОС"), ListView()]
        public int? AssetHolderRSBUID { get; set; }

        /// <summary>
        /// Получает или задает кадастровый номер.
        /// </summary>       
        [DetailView("Кадастровый номер"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает актуальную кадастровую стоимость.
        /// </summary>                
        [DetailView("Кадастровая стоимость, руб."), ListView()]
        [DefaultValue(0)]
        public decimal? CadastralValue { get; set; }

        /// <summary>
        /// Получает или задает комментарии.
        /// </summary>
        [DetailView("Комментарии"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comments { get; set; }

        /// <summary>
        /// Получает или задает балансовую единицу (ОГ)-арендополучателя.
        /// </summary>
        [DetailView("Балансовая единица (ОГ)-арендополучатель "), ListView()]
        public virtual Consolidation Consolidation { get; set; }

        /// <summary>
        /// Получает или задает ИД балансовой единицы (ОГ)-арендополучателя.
        /// </summary>
        [DetailView("ИД БЕ"), ListView()]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает Вид затрат в части арендных платежей.
        /// </summary>            
        [DetailView("Вид затрат в части арендных платежей"), ListView()]
        public virtual CostKindRentalPayments CostKindRentalPayments { get; set; }

        /// <summary>
        /// Получает или задает ИД вида затрат в части арендных платежей.
        /// </summary>            
        [DetailView("ИД вида затрат в части арендных платежей"), ListView()]
        public int? CostKindRentalPaymentsID { get; set; }

        /// <summary>
        /// Получает или задает валюту.
        /// </summary>
        [DetailView("Валюта"), ListView()]
        public virtual Currency Currency { get; set; }

        /// <summary>
        /// Получает или задает ИД валюты.
        /// </summary>
        [DetailView("ИД валюты"), ListView()]
        public int? CurrencyID { get; set; }

        /// <summary>
        /// Получает или задает месторождение.
        /// </summary> 
        [DetailView("Месторождение"), ListView()]
        public virtual Deposit Deposit { get; set; }

        /// <summary>
        /// Получает или задает ИД месторождения.
        /// </summary> 
        [DetailView("ИД месторождения"), ListView()]
        public int? DepositID { get; set; }

        /// <summary>
        /// Получает или задает амортизационную группу НУ.
        /// </summary>
        [DetailView("Амортизационная группа"), ListView()]
        public virtual DepreciationGroup DepreciationGroup { get; set; }

        /// <summary>
        /// Получает или задает ИД амортизационной группы.
        /// </summary>
        [DetailView("ИД Амортизационной группы"), ListView()]
        public int? DepreciationGroupID { get; set; }

        /// <summary>
        /// Получает номер ЕУСИ объекта имущества.
        /// </summary>
        [DetailView("Номер ЕУСИ"), ListView()]
        public int? EUSINumber { get; set; }
                
        /// <summary>
        /// Получает или задает есть ли конкретное указание лицензионного участка в договоре аренды земли (да/нет).
        /// </summary>
        [DetailView("Есть ли конкретное указание лицензионного участка в договоре аренды земли (да/нет)"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string IndicationLicenceLandArea { get; set; }

        /// <summary>
        /// Получает или задает признак инфраструктуры.
        /// </summary>
        [DetailView("Признак инфраструктуры"), ListView()]       
        public bool? InfrastructureExist { get; set; }

        /// <summary>
        /// Получает или задает первоначальную стоимость объекта аренды в нац. валюте.
        /// </summary>
        [DetailView("Первоначальная стоимость объекта аренды в нац. валюте"), ListView()]
        [DefaultValue(0)]
        public decimal? InitialCost { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер объекта, если объект, находящийся на данном ЗУ также арендован.
        /// </summary>
        [DetailView("Аренда ЗУ:Инвентарный номер объекта, если объект, находящийся на данном ЗУ также арендован"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryArendaLand { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>        
        [DetailView("Инвентарный номер"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает назначение ЗУ.
        /// </summary>
        [DetailView("Назначение ЗУ"), ListView()]
        public virtual LandPurpose LandPurpose { get; set; }

        /// <summary>
        /// Получает или задает ИД назначения ЗУ.
        /// </summary>  
        [DetailView("ИД назначения ЗУ"), ListView()]
        public int? LandPurposeID { get; set; }

        /// <summary>
        /// Получает или задает наименование объекта (в соответствии с документами).
        /// </summary>
        [DetailView("Наименование объекта (в соответствии с документами)"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameByDoc { get; set; }

        /// <summary>
        /// Получает или задает код местоположения объекта.
        /// </summary> 
        [DetailView("Код местоположения объекта"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectLocationRent { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-2014.
        /// </summary>
        [DetailView("Код ОКОФ"), ListView()]
        public virtual OKOF2014 OKOF2014 { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-2014.
        /// </summary>        
        [DetailView("ИД ОКОФ"), ListView()]
        public int? OKOF2014ID { get; set; }

        /// <summary>
        /// Получает или задает контрагента (арендодателя).
        /// </summary>
        [DetailView("Контрагент (арендодатель)"), ListView()]
        public virtual CorpProp.Entities.Subject.Subject ProprietorSubject { get; set; }

        /// <summary>
        /// Получает или задает ИД контрагента (арендодателя).
        /// </summary>
        [DetailView("ИД контрагента (арендодателя)"), ListView()]
        public int? ProprietorSubjectID { get; set; }

        /// <summary>
        /// Получает или задает выкупную стоимость объекта аренды в валюте договора.
        /// </summary>        
        [DetailView("Выкупная стоимость объекта аренды (в валюте договора)"), ListView()]
        [DefaultValue(0)]
        public decimal? RedemptionCost { get; set; }

        /// <summary>
        /// Получает или задает предполагаемую дату выкупа объекта аренды.
        /// </summary>
        [DetailView("Предполагаемая дата выкупа объекта аренды"), ListView()]
        public DateTime? RedemptionDate { get; set; }

        /// <summary>
        /// Получает или задает номер договора в компании.
        /// </summary>            
        [DetailView("Уникальный номер договора Компании"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string RentContractNumber { get; set; }

        /// <summary>
        /// Получает или задает состояние ОС/НМА (аренда).
        /// </summary> 
        [DetailView("Состояние ОС/НМА (аренда)"), ListView()]
        public virtual StateObjectRent StateObjectRent { get; set; }

        /// <summary>
        /// Получает или задает ИД состояния ОС/НМА (аренда).
        /// </summary> 
        [DetailView("ИД состояния ОС/НМА (аренда)"), ListView()]
        public int? StateObjectRentID { get; set; }

        /// <summary>
        /// Получает или задает код местоположения объекта.
        /// </summary> 
        [DetailView("Код недропользователя ДАО"), ListView()]
        public virtual Consolidation SubsoilUser { get; set; }

        /// <summary>
        /// Получает или задает ИД кода местоположения объекта.
        /// </summary> 
        [DetailView("ИД кода недропользователя ДАО"), ListView()]
        public int? SubsoilUserID { get; set; }

        /// <summary>
        /// Получает или задает признак наличия условия в договоре аренды "бери или плати".
        /// </summary>       
        [DetailView("Условие в договоре аренды \"бери или плати\"" 
        , Description = "Содержится ли условие в договоре аренды \"бери или плати\", когда арендатор независимо от  пользования объектом должен платить по договору")
        , ListView()]
        public bool? TakeOrPay { get; set; }

        /// <summary>
        /// Получает или задает вид операции.
        /// </summary> 
        [DetailView("Вид операции"), ListView()]
        public virtual TransactionKind TransactionKind { get; set; }

        /// <summary>
        /// Получает или задает ИД вида операции.
        /// </summary> 
        [DetailView("ИД вида операции"), ListView()]
        public int? TransactionKindID { get; set; }

        /// <summary>
        /// Получает или задает предусмотрена ли договором передача права собственности (да /нет).
        /// </summary>       
        [DetailView("Предусмотрена ли договором передача права собственности (да/нет)"), ListView()]
        public bool? TransferRight { get; set; }

        /// <summary>
        /// Получает или задает срок полезного использования по РСБУ.
        /// </summary>
        [DetailView("Срок полезного использования"), ListView()]
        [PropertyDataType(PropertyDataType.Text)]
        public string Useful { get; set; }

        /// <summary>
        /// Получает или задает остаточный срок полезного использования.
        /// </summary>       
        [DetailView("Аренда ЗУ: Остаточный срок полезного использования"), ListView()]
        public int? UsefulEndLand { get; set; }

        /// <summary>
        /// Переопределяет метод перед сохранением объекта.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="entry">Текущая запись.</param>
        public override void OnSaving(IUnitOfWork uow, object entry)
        {
            //не ведем историю по этому объекту
            return;
        }
    }
}
