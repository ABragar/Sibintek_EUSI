using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Base.DAL;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.NSI;
using Base.ComplexKeyObjects.Superb;
using CorpProp.Entities.Subject;
using CorpProp.Entities.FIAS;
using Base;
using Base.Translations;
using CorpProp.Entities.Common;
using CorpProp.Common;

namespace CorpProp.Entities.Estate
{

    /// <summary>
    /// Представляет объект имущества.
    /// </summary>
    [EnableFullTextSearch]
    public class Estate : TypeObject, ISuperObject<Estate>, IHasAdditionalFeature, IArchiveObject
    {

        private static readonly CompiledExpression<Estate, int?> _EUSINumber =
            DefaultTranslationOf<Estate>.Property(x => x.EUSINumber).Is(x =>
                (x.Number == null) ? (x.PCNumber == null ? null : x.PCNumber) : x.Number);

        private static readonly CompiledExpression<Estate, Subject.Society> _owner =
           DefaultTranslationOf<Estate>.Property(x => x.Owner).Is(x => (x.Calculate != null) ? x.Calculate.Owner : null);

        private static readonly CompiledExpression<Estate, Subject.Society> _mainOwner =
           DefaultTranslationOf<Estate>.Property(x => x.MainOwner).Is(x => (x.Calculate != null) ? x.Calculate.MainOwner : null);

        private static readonly CompiledExpression<Estate, Subject.Society> _whoUse =
          DefaultTranslationOf<Estate>.Property(x => x.WhoUse).Is(x => (x.Calculate != null) ? x.Calculate.WhoUse : null);

        private static readonly CompiledExpression<Estate, string> _dealProps =
          DefaultTranslationOf<Estate>.Property(x => x.DealProps).Is(x => (x.Calculate != null) ? x.Calculate.DealProps : null);


        #region Constructor

        /// <summary>
        /// Инициализирует новый экземпляр класса Estate.
        /// </summary>
        public Estate()
        {
            //Images = new List<EstateImage>();
            EstateStatus = EstateStatus.Undefined;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса Estate из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public Estate(IUnitOfWork uofw, AccountingObject obj) 
        {
            //Images = new List<EstateImage>();
           
            this.Name = obj.Name;
            //this.AccountingName = obj.Name;
            //this.Description = obj.Description;

            //this.InventoryNumber = obj.InventoryNumber;
            //this.InventoryNumber2 = obj.InventoryNumber2;
            //this.AccountingName = obj.Name;
            //this.ClassFixedAsset = obj.ClassFixedAsset;
            //this.GroundNumber = obj.GroundNumber;
            //this.Owner = obj.Owner;
            //this.AccountNumber = obj.AccountNumber;
            //this.BusinessArea = obj.BusinessArea;
            //this.WhoUse = obj.WhoUse;
            //this.MOL = obj.MOL;
            //this.DateOfReceipt = obj.DateOfReceipt;
            //this.ReceiptReason = obj.ReceiptReason;
            //this.LeavingDate = obj.LeavingDate;
            //this.LeavingReason = obj.LeavingReason;
            //this.OKOF94 = obj.OKOF94;
            //this.OKOFName = obj.OKOF94?.Name;
            //this.OKOFCode = obj.OKOF94?.Code;
            //this.OKOF2014 = obj.OKOF2014;
            //this.OKOFName2 = obj.OKOF2014?.Name;
            //this.OKOFCode2 = obj.OKOF2014?.Code;
            //this.OKTMO = obj.OKTMO;
            //this.OKTMOCode = obj.OKTMO?.Code;
            //this.OKTMOName = obj.OKTMO?.Name;
            //this.OKATORegionCode = ((obj.OKATO != null && obj.OKATO.Code.Length > 2) ? obj.OKATO.Code.Substring(0, 2) : "");
            //this.OKTMORegion = ImportHelper.FindRegionByCode(uofw, obj.OKTMO?.Code);
            //this.OKATO = obj.OKATO;
            //this.OKATORegion = ImportHelper.FindRegionOKATO(uofw, obj.OKATO);
            //this.UpdateDate = obj.UpdateDate;
            //this.InitialCost = obj.InitialCost;
            //this.ResidualCost = obj.ResidualCost;
            //this.DepreciationCost = obj.DepreciationCost;
            //this.Useful = obj.Useful;
            //this.UsefulEnd = obj.UsefulEnd;
            //this.LeavingCost = obj.LeavingCost;
            //this.UsefulEndDate = obj.UsefulEndDate;
            //this.MarketCost = obj.MarketCost;
            //this.MarketDate = obj.MarketDate;          
            //this.InConservation = obj.InConservation;
            //this.ConservationFrom = obj.ConservationFrom;
            //this.ConservationTo = obj.ConservationTo;
            //this.Status = obj.Status;
            //this.StartDate = obj.StartDate;
            //this.EndDate = obj.EndDate;
            //this.DealProps = obj.DealProps;
            //this.SubjectName = obj.SubjectName;
            //this.ExternalID = obj.ExternalID;
            //this.IsDispute = obj.IsDispute;
        }

        #endregion

        /// <summary>
        /// Доп. Характеристики
        /// </summary>
        public int? AdditionalFeaturesID { get; set; }

        public AdditionalFeatures AdditionalFeature { get; set; }

        /// <summary>
        /// Получает или задает ИД выч. полей
        /// </summary>
        [SystemProperty]
        public int? CalculateID { get; set; }

        /// <summary>
        /// Получает или задает ссылку на объект с выч. полями
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Выч. значения", ReadOnly = true, Visible = false)]
        public EstateCalculatedField Calculate { get; set; }



        #region EUSI TODO Перенести в ЕУСИ


        /// <summary>
        /// Получает или задает номер ЕУСИ.
        /// </summary>
        [DetailView("Номер ЕУСИ", Visible = false, ReadOnly = true)]
        
        [SystemProperty]
        [FullTextSearchProperty]
        public int? Number { get; set; }

        ///<summary>Номер</summary> 
        ///<remarks>Номер</remarks>
        [DetailView("Номер", Visible = false)]
        [ListView("Номер", Visible = false)]
        [FullTextSearchProperty]
        public int? EUSINumber => _EUSINumber.Evaluate(this);

        /// <summary>
        /// Номер ИК
        /// </summary>
        [DetailView(Name = "Номер ИК", Visible = false)]
        [ListView("Номер ИК", Visible = false)]
        public int? PCNumber { get; set; }


        /// <summary>
        /// Получает или задает статус ОИ по заявке ЗР.
        /// </summary>
        [DetailView("Статус", Visible = false)]
        [ListView("Статус", Visible = false)]
        [SystemProperty]
        public EstateStatus EstateStatus { get; set; }

        /// <summary>
        /// Получает или задает наименование ЕУСИ.
        /// </summary>
        [DetailView("Наименование ЕУСИ", Visible = false)]
        [ListView("Наименование ЕУСИ", Visible = false)]
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameEUSI { get; set; }

        [SystemProperty]
        public int? DepreciationMethodRSBUID { get; set; }

        [DetailView("Метод амортизации (РСБУ)", Visible = false)]
        [ListView("Метод амортизации (РСБУ)", Visible = false)]
        public DepreciationMethodRSBU DepreciationMethodRSBU { get; set; }

        [SystemProperty]
        public int? DepreciationMethodNUID { get; set; }

        [DetailView("Метод амортизации (НУ)", Visible = false)]
        [ListView("Метод амортизации (НУ)", Visible = false)]
        public DepreciationMethodNU DepreciationMethodNU { get; set; }

        [SystemProperty]
        public int? DepreciationMethodMSFOID { get; set; }

        [DetailView("Метод амортизации (МСФО)", Visible = false)]
        [ListView("Метод амортизации (МСФО)", Visible = false)]
        public DepreciationMethodMSFO DepreciationMethodMSFO { get; set; }

       
        [DetailView("СПИ по РСБУ, мес.", Visible = false)]
        [ListView("СПИ по РСБУ, мес.", Visible = false)]
        public int? UsefulForRSBU { get; set; }

        
        [DetailView("СПИ по НУ, мес.", Visible = false)]
        [ListView("СПИ по НУ, мес.", Visible = false)]
        public int? UsefulForNU { get; set; }

                
        [DetailView("Коэффициент ускоренной амортизации для НУ", Visible = false)]
        [ListView(Visible = false)]
        public int? DepreciationMultiplierForNU { get; set; }
                       

        [SystemProperty]
        public int? AddonOKOFID { get; set; }

        [DetailView("Доп. код ОКОФ", Visible = false)]
        [ListView("Доп. код ОКОФ", Visible = false)]
        public AddonOKOF AddonOKOF { get; set; }

        
        [SystemProperty]
        public int? DivisibleTypeID { get; set; }

        [DetailView("Отделимое/неотделимое имущество", Visible = false)]
        [ListView("Отделимое/неотделимое имущество", Visible = false)]
        public DivisibleType DivisibleType { get; set; }

        /// <summary>
        /// Получает или задает состояние, находится ли ОИ в архиве (помечен на удаление).
        /// </summary>
        [ListView("На удаление", Visible = true)]
        [DetailView("На удаление", Visible = false)]
        public bool? IsArchived { get; set; }

        /// <summary>
        /// Получает или задает комментарий для ОИ.
        /// </summary>
        [ListView("Комментарий", Hidden = true)]
        [DetailView("Комментарий")]
        public string Comment { get; set; }

        #endregion//end EUSI
        //----------------------------------------------------------------------------------------------



        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; } = null;

        /// <summary>
        /// Получает или задает ИД класса объекта.
        /// </summary>
        public int? EstateTypeID { get; set; }

        /// <summary>
        /// Получает или задает класс объекта.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView("Класс КС",Visible = false)]
        public EstateType EstateType { get; set; }

        [SystemProperty]
        public int? EstateDefinitionTypeID { get; set; }

            
        [ListView("Тип Объекта имущества", Visible = false)]        
        [DetailView(Name = "Тип Объекта имущества", Visible = false)]
        public EstateDefinitionType EstateDefinitionType { get; set; }

        /// <summary>
        ///Получает или задает наименование.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Name { get; set; }

        /// <summary>
        /// Наименование объекта (в соответствии с документами).
        /// </summary>
        [DetailView("Наименование объекта (в соответствии с документами)", Visible = false)]
        [ListView("Наименование объекта (в соответствии с документами)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameByDoc { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        //[DetailView(Name = "Примечание", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Description { get; set; }

        /// <summary>
        /// Получает признак, является ли объект непрофильным или неэффективным.
        /// </summary>
        /// <remarks>
        /// "Да", если существует ННА, связанный с данным инвентарным объектом, 
        /// и этот ННА актуален (в соотв. с его статусом). "Нет" по умолчанию.
        /// </remarks>
        // TODO: добавить логику вычисления признака ННА.       
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView("Является ННА", Visible = false)]
        [DefaultValue(false)]
        public bool IsNonCoreAsset { get; set; } = false;

        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер 2.
        /// </summary>
        /// <remarks>
        /// Например, инв. № 1C в КИС САП РН.
        /// </remarks>  
        //[ListView(Order = 2)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber2 { get; set; }

        //#region Для ОБУ

        ///// <summary>
        ///// Получает или задает ИД класса ОС.
        ///// </summary>        
        //[DetailView(Visible = false)]
        //public int? ClassFixedAssetID { get; set; }

        ///// <summary>
        ///// Получает или задает класс БУ.
        ///// </summary>
        ////[ListView(Order = 3)]
        //[DetailView(Visible = false)]
        //public  ClassFixedAsset ClassFixedAsset { get; set; }

        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string AccountingName { get; set; }

        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string AccountingDescription { get; set; }

        ///// <summary>
        ///// Кад. номер ЗУ
        ///// </summary>
        ///// <remarks>Кадастровый номер вышестоящего ЗУ текущего кадастрового объекта.	ОИ.Кадастровый объект.Вышестоящие объекты</remarks>
        ////TODO: Добавить в сервис наполнение атрибута
        ////[ListView(Hidden = true)]
        //[DetailView(Visible = false)]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string GroundNumber { get; set; }


        ///// <summary>
        ///// Получает или задает идентификатор балансодержателя.
        ///// </summary> 
        //[SystemProperty]
        //public int? OwnerID { get; set; }

        /// <summary>
        /// Получает  балансодержателя.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Балансодержатель",Visible = false)]
        public Society Owner => _owner.Evaluate(this);

        /// <summary>
        /// Получает  Собственника.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Собственник по БУ", Visible = false)]
        public Society MainOwner => _mainOwner.Evaluate(this);

        ///// <summary>
        ///// Получает или задает номер счета.
        ///// </summary>
        ////[ListView(Hidden = true)]
        //[DetailView(Visible = false)]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string AccountNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД бизнес-сферы.
        /// </summary>       
        public int? BusinessAreaID { get; set; }

        /// <summary>
        /// Получает или задает бизнес-сферу.
        /// </summary>
        [ListView("Бизнес-сфера", Hidden = true)]
        [DetailView("Бизнес-сфера",Visible = false)]
        public BusinessArea BusinessArea { get; set; }

        ///// <summary>
        ///// Получает или задает идентификатор ОГ - пользователя ОБУ.
        ///// </summary> 
        //[SystemProperty]
        //public int? WhoUseID { get; set; }

        /// <summary>
        /// Получает или задает ОГ - пользователя ОБУ.
        /// </summary>  
        [ListView(Hidden = true)]
        [DetailView("Пользователь", Visible = false)]
        public Society WhoUse => _whoUse.Evaluate(this);

        ///// <summary>
        ///// Получает или задает материально-ответственное лицо.
        ///// </summary>       
        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string MOL { get; set; }

        ///// <summary>
        ///// Получает или задает дату оприходования.
        ///// </summary>   
        //[DetailView(Visible = false)]
        //public DateTime? DateOfReceipt { get; set; }
        ///// <summary>
        ///// Получает или задает ИД причины поступления.
        ///// </summary>       
        //public int? ReceiptReasonID { get; set; }

        ///// <summary>
        ///// Получает или задает причину поступления.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public  ReceiptReason ReceiptReason { get; set; }

        ///// <summary>
        ///// Получает или задает дату списания.
        ///// </summary>     
        //[DetailView(Visible = false)]
        //public DateTime? LeavingDate { get; set; }
        ///// <summary>
        ///// Получает или задает ИД причины выбытия.
        ///// </summary>       
        //public int? LeavingReasonID { get; set; }

        ///// <summary>
        ///// Получает или задает причину выбытия.
        ///// </summary>        
        //[DetailView(Visible = false)]
        //public  LeavingReason LeavingReason { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-94.
        /// </summary>
        [SystemProperty]
        public int? OKOF94ID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-94.
        /// </summary>              
        [DetailView("ОКОФ", Visible = false)]
        public OKOF94 OKOF94 { get; set; }

        ///// <summary>
        ///// Получает или задает ОКОФ-94 наименование
        ///// </summary>
        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string OKOFName { get; set; }

        ///// <summary>
        ///// Получает или задает ОКОФ-94 код.
        ///// </summary>
        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string OKOFCode { get; set; }
        /// <summary>
        /// Получает или задает ИД ОКОФ-2014.
        /// </summary>   
        [SystemProperty]
        public int? OKOF2014ID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-2014.
        /// </summary>             
        [DetailView(Visible = false)]
        public OKOF2014 OKOF2014 { get; set; }


        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string OKOFName2 { get; set; }


        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string OKOFCode2 { get; set; }

        /// <summary>
        /// Получает или задает ИД OKTMO.
        /// </summary>       
        [SystemProperty]
        public int? OKTMOID { get; set; }

        /// <summary>
        /// Получает или задает OKTMO.
        /// </summary>        
        [DetailView(Visible = false)]
        public OKTMO OKTMO { get; set; }


        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string OKTMOCode { get; set; }


        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string OKTMOName { get; set; }

        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string OKATORegionCode { get; set; }

        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string KLADRRegionCode { get; set; }

        //[DetailView(Visible = false)]
        //public SibRegion OKTMORegion { get; set; }

        ///// <summary>
        ///// Получает или задает ИД ОКАТО.
        ///// </summary>        
        //[SystemProperty]
        //public int? OKATOID { get; set; }

        ///// <summary>
        ///// Получает или задает OKATO.
        ///// </summary>        
        //[DetailView(Visible = false)]
        //public  OKATO OKATO { get; set; }

        //[DetailView(Visible = false)]
        //public SibRegion OKATORegion { get; set; }





        ///// <summary>
        ///// Получает или задает дату обновления информации о стоимости.
        ///// </summary>      
        //[DetailView(Visible = false)]
        //public DateTime? UpdateDate { get; set; }


        ///// <summary>
        ///// Получаеет или задает ИД ЗУ, на котором расположен объект.
        ///// </summary>
        //[SystemProperty]
        //public int? LandID { get; set; }

        ///// <summary>
        ///// Получает или задает ЗУ, на котором расположен объект.
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Земельный участок",
        //Description = "Земельный участок, на котором расположен объект", Visible =false)]
        //public  Land Land { get; set; }

        //#region Вычесляемые поля из Изменение AccountingObjectMoving
        ///// <summary>
        ///// Получает или задает первоначальную стоимость в руб.
        ///// </summary>      
        //[DetailView(Visible = false)]
        //public decimal? InitialCost { get; set; }

        ///// <summary>
        ///// Получает или задает остаточную стоимость в руб.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public decimal? ResidualCost { get; set; }

        ///// <summary>
        ///// Получает или задает начисленную амортизацию в руб.
        ///// </summary>        
        //[DetailView(Visible = false)]
        //public decimal? DepreciationCost { get; set; }

        ///// <summary>
        ///// Получает или задает срок полезного использования.
        ///// </summary>       

        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string Useful { get; set; }

        ///// <summary>
        ///// Получает или задает оставшийся срок полезного использования.
        ///// </summary>
        ///// <remarks> Вычисляемое поле	Изменение</remarks>       
        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string UsefulEnd { get; set; }


        ///// <summary>
        ///// Получает или задает стоимость выбытия в руб.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public decimal? LeavingCost { get; set; }

        ///// <summary>
        ///// Получает или задает дату окончания срока полезного использования.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public DateTime? UsefulEndDate { get; set; }

        /// <summary>
        /// Получает или задает рыночную стоимость.
        /// </summary>
        /// <remarks>Последняя актуальная рыночная стоимость объекта оценки.	ОИ.Оценка.Объект оценки</remarks>        
        [DetailView(Visible = false)]
        public decimal? MarketCost { get; set; }


        ///// <summary>
        ///// Получает или задает дату рыночной оценки.
        ///// </summary>
        ///// <remarks>Последняя актуальная дата оценки (ближайшей к текущей дате)	ОИ.Оценка</remarks>       
        //[DetailView(Visible = false)]
        //public DateTime? MarketDate { get; set; }


        /////// <summary>
        /////// Получает или задает реквизиты отчета об оценке.
        /////// </summary>
        /////// <remarks>Коллекция документов оценки (ближайшей к текущей дате)	ОИ.Оценка</remarks>
        //////TODO: Добавить в сервис наполнение атрибута       
        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string AppraisalFileCard { get; set; }

        //#endregion

        //#region Состояние 
        ///// <summary>
        ///// Получает или задает признак консервации.
        ///// </summary>    
        //[DefaultValue(false)]
        //[DetailView(Visible = false)]
        //public bool InConservation { get; set; } = false;

        ///// <summary>
        ///// Получает или задает дату начала консервации.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public DateTime? ConservationFrom { get; set; }


        ///// <summary>
        ///// Получает или задает дату окончания консервации.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public DateTime? ConservationTo { get; set; }

        //[SystemProperty]
        //public int? AccountingStatusID { get; set; }

        ///// <summary>
        ///// Получает или задает статус.
        ///// </summary> 
        //[DetailView(Visible = false)]
        //public  AccountingStatus Status { get; set; }

        ///// <summary>
        ///// Получает или задает дату начала.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public DateTime? StartDate { get; set; }

        ///// <summary>
        ///// Получает или задает дату окончания.
        ///// </summary>       
        //[DetailView(Visible = false)]
        //public DateTime? EndDate { get; set; }

        /// <summary>
        /// Получает или задает атрибут "Дата дообогащения ОИ"
        /// Используется в ЕУСИ
        /// </summary> 
        [ListView(Visible = false)]
        [DetailView("Дата дообогащения ОИ", Visible = false, ReadOnly = true)]
        public DateTime? EnrichmentDate { get; set; }

        /// <summary>
        /// Получает или задает реквизиты договора.
        /// </summary>        
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        [ListView(Visible = false)]
        public string DealProps => _dealProps.Evaluate(this);

        /// <summary>
        /// Получает или задает атрибут "За балансом"
        /// Используется в ЕУСИ
        /// </summary> 
        [ListView(Visible = false)]
        [DetailView("За балансом", Visible = false, ReadOnly = true)]
        [DefaultValue(false)]
        public bool OutOfBalance { get; set; }

        /// <summary>
        /// Получает или задает № договора (из заявки).
        /// </summary>
        [DetailView("№ договора", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ERContractNumber { get; set; }

        /// <summary>
        /// Получает или задает дату договора (из заявки).
        /// </summary>       
        [DetailView("Дата договора", Visible = false)]
        public DateTime? ERContractDate { get; set; }

        /// <summary>
        /// Получает или задает № первичного документа (из заявки).
        /// </summary> 
        [DetailView("№ первичного документа", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PrimaryDocNumber { get; set; }

        /// <summary>
        /// Получает или задает дату первичного документа (из заявки).
        /// </summary>       
        [DetailView("Дата первичного документа", Visible = false)]
        public DateTime? PrimaryDocDate { get; set; }

        /// <summary>
        /// Получает или задает ИД доп.кода ОКОФ-2.
        /// </summary>
        [DetailView(Visible = false)]
        public int? AddonOKOF2014ID { get; set; }

        /// <summary>
        /// Получает или задает доп код ОКОФ-2.
        /// </summary>
        [DetailView("Доп. код ОКОФ-2", Visible = false)]
        [ListView("Доп. код ОКОФ-2", Visible = false)]
        public virtual AddonOKOF2014 AddonOKOF2014 { get; set; }
        
        ///// <summary>
        ///// Контрагент по договору.
        ///// </summary>       
        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string SubjectName { get; set; }

        //#endregion

        ///// <summary>
        ///// Получает или задает внутренний ИД ОБУ в БУС ОГ.
        ///// </summary>
        //[PropertyDataType(PropertyDataType.Text)]
        //[DetailView(Visible = false)]
        //public string ExternalID { get; set; }

        //#endregion



        //#region Списки/ссылки

        ////[DetailView(Name = "Фотография", TabName = "[7]Фотографии", HideLabel = true), ListView]       
        //[PropertyDataType(PropertyDataType.Gallery)]       
        //public virtual ICollection<EstateImage> Images { get; set; }

        //#endregion        


        public void UpdateByAccounting(IUnitOfWork uofw, AccountingObject obj)
        {
            if (obj == null) return;

            this.Name = obj.Name;
            //this.AccountingName = obj.Name;
            this.Description = obj.Description;

            this.InventoryNumber = obj.InventoryNumber;
            this.InventoryNumber2 = obj.InventoryNumber2;
            //this.AccountingName = obj.Name;
            //this.ClassFixedAsset = obj.ClassFixedAsset;
            //this.GroundNumber = obj.GroundNumber;
            //this.Owner = obj.Owner;
            //this.AccountNumber = obj.AccountNumber;
            //this.BusinessArea = obj.BusinessArea;
            //this.WhoUse = obj.WhoUse;
            //this.MOL = obj.MOL;
            //this.DateOfReceipt = obj.DateOfReceipt;
            //this.ReceiptReason = obj.ReceiptReason;
            //this.LeavingDate = obj.LeavingDate;
            //this.LeavingReason = obj.LeavingReason;
            //this.OKOF94 = obj.OKOF94;
            //this.OKOFName = obj.OKOF94?.Name;
            //this.OKOFCode = obj.OKOF94?.Code;
            //this.OKOF2014 = obj.OKOF2014;
            //this.OKOFName2 = obj.OKOF2014?.Name;
            //this.OKOFCode2 = obj.OKOF2014?.Code;
            //this.OKTMO = obj.OKTMO;
            //this.OKTMOCode = obj.OKTMO?.Code;
            //this.OKTMOName = obj.OKTMO?.Name;
            //this.OKATORegionCode = ((obj.OKATO != null && obj.OKATO.Code.Length > 2) ? obj.OKATO.Code.Substring(0, 2) : "");
            //this.OKTMORegion = ImportHelper.FindRegionByCode(uofw, obj.OKTMO?.Code);
            //this.OKATO = obj.OKATO;
            //this.OKATORegion = ImportHelper.FindRegionOKATO(uofw, obj.OKATO);
            //this.UpdateDate = obj.UpdateDate;
            //this.InitialCost = obj.InitialCost;
            //this.ResidualCost = obj.ResidualCost;
            //this.DepreciationCost = obj.DepreciationCost;
            //this.Useful = obj.Useful;
            //this.UsefulEnd = obj.UsefulEnd;
            //this.LeavingCost = obj.LeavingCost;
            //this.UsefulEndDate = obj.UsefulEndDate;
            //this.MarketCost = obj.MarketCost;
            //this.MarketDate = obj.MarketDate;
            //this.InConservation = obj.InConservation;
            //this.ConservationFrom = obj.ConservationFrom;
            //this.ConservationTo = obj.ConservationTo;
            //this.Status = obj.Status;
            //this.StartDate = obj.StartDate;
            //this.EndDate = obj.EndDate;
            //this.DealProps = obj.DealProps;
            //this.SubjectName = obj.SubjectName;
            //this.ExternalID = obj.ExternalID;

        }

    }
}

