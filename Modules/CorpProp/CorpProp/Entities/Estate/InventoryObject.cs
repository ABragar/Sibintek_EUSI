using Base.Attributes;
using Base.DAL;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет инвентарный объект: материальный объект (ОС, НЗС) в том составе (границах), в которых он отражён в бух. учёте.
    /// </summary>
    [EnableFullTextSearch]
    public class InventoryObject : Estate
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса InventoryObject.
        /// </summary>
        public InventoryObject() : base()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса InventoryObject из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public InventoryObject(IUnitOfWork uofw, AccountingObject obj) : base(uofw, obj)
        {
            this.BuildingLength = obj.BuildingLength;
            this.IsRealEstate = obj.IsRealEstate;
            this.CadastralNumbers = obj.CadastralNumber;
            this.PropertyComplexName = obj.PropertyComplexName;
        }

        private static readonly CompiledExpression<InventoryObject, decimal?> _initialCostNU =
            DefaultTranslationOf<InventoryObject>.Property(x => x.InitialCostNU).Is(x => (x.Calculate != null) ? x.Calculate.InitialCostSumNU : 0m);

        private static readonly CompiledExpression<InventoryObject, decimal?> _initialCostOBU =
            DefaultTranslationOf<InventoryObject>.Property(x => x.InitialCostOBU).Is(x => (x.Calculate != null) ? x.Calculate.InitialCostSumOBU : 0m);

        private static readonly CompiledExpression<InventoryObject, int?> _inventoryObjectsCount =
                                            DefaultTranslationOf<InventoryObject>.Property(x => x.InventoryObjectsCount).Is(x => (x.Calculate != null) ? x.Calculate.ChildObjectsCount : 0);

        private static readonly CompiledExpression<InventoryObject, decimal?> _residualCostNU =
            DefaultTranslationOf<InventoryObject>.Property(x => x.ResidualCostNU).Is(x => (x.Calculate != null) ? x.Calculate.ResidualCostSumNU : 0m);

        private static readonly CompiledExpression<InventoryObject, decimal?> _residualCostOBU =
                    DefaultTranslationOf<InventoryObject>.Property(x => x.ResidualCostOBU).Is(x => (x.Calculate != null) ? x.Calculate.ResidualCostSumOBU : 0m);

        private readonly EstateTaxes _estateTaxes;

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Адрес", Visible = false)]
        public string Address { get; set; }

        [DetailView("Длина линейного сооружения, м.", Visible = false)]
        [ListView("Длина линейного сооружения, м.", Visible = false)]
        public decimal? BuildingLength { get; set; }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Получает кадастровые номера.
        /// </summary>
        /// <remarks>
        /// Кадастровый объект.Кадастровый номер.
        /// В случае, если установлено соответствие кадастровому объекту 1:1 или вхождение в 1 кадастровый объект - номер этого объекта.
        /// В случае вхождения нескольких кадастровых объектов в инвентарный - перечень их номеров
        /// </remarks>
        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public String CadastralNumbers { get; set; }

        /// <summary>
        /// Получает или задает ИД дочернего инв.объекта. (для кол-ии вышестоящих объектов)
        /// </summary>
        [SystemProperty]
        public int? ChildID { get; set; }

        [DetailView("Объем резервуара, куб.м.", Visible = false)]
        [ListView("Объем резервуара, куб.м.", Visible = false)]
        public decimal? ContainmentVolume { get; set; }

        /// <summary>
        /// Получает или задает описание статуса объекта культурного наследия.
        /// </summary>
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CulturalStatus { get; set; }

        [DetailView("Месторождение (номер)", Visible = false)]
        [ListView("Месторождение (номер)", Visible = false)]
        public Deposit Deposit { get; set; }

        [SystemProperty]
        public int? DepositID { get; set; }

        [DetailView("Амортизационная группа НУ", Visible = false)]
        [ListView("Амортизационная группа НУ", Visible = false)]
        public DepreciationGroup DepreciationGroup { get; set; }

        [SystemProperty]
        public int? DepreciationGroupID { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Дата договора", Visible = false,
                   Description = "Пользователем указывается дата договора на основании которого на ОИ наложено ограничение/ обременение или если последнее стало причиной смены статуса ОИ (реализован, передан в аренду, передан на хранение и т.п.)")]
        public DateTime? EncumbranceContractDateSZVD { get; set; }

        [ListView(Visible = false)]
        [DetailView("Номер договора", Visible = false,
                    Description = "Номер договора, на основании которого на ОИ наложено ограничение/ обременение или если последнее стало причиной смены статуса ОИ (реализован, передан на хранение и т.п.)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceContractNumber { get; set; }

        [ListView(Visible = false)]
        [DetailView("Номер договора СЦВД", Visible = false,
                  Description = "Пользователем указывается системный номер договора в ИР СЦВД на основании которого на ОИ наложено ограничение/ обременение или если последнее стало причиной смены статуса ОИ (реализован, передан в аренду, передан на хранение и т.п.)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceContractNumberSZVD { get; set; }

        [ListView(Visible = false)]
        [DetailView("Обременение", Visible = false)]
        [DefaultValue(false)]
        public bool EncumbranceExist { get; set; }

        [DetailView("Признак движимое/недвижимое имущество по данным БУ", Visible = false)]
        [ListView("Признак движимое/недвижимое имущество по данным БУ", Visible = false)]
        public EstateMovableNSI EstateMovableNSI { get; set; }

        [SystemProperty]
        public int? EstateMovableNSIID { get; set; }

        [DetailView("Фактическое местонахождение объекта", Visible = false)]
        [ListView("Фактическое местонахождение объекта", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FactAddress { get; set; }

        /// <summary>
        /// Получает или задает кадастровый объект' (штрих).
        /// </summary>
        /// <remarks>
        /// Создается для объединения нескольких МА с одним кадстровым номером в один фэйковый кадастровый объект.
        /// </remarks>
        [ListView(Hidden = true)]
        [DetailView(Name = "Кадастровый объект'", Visible = false, ReadOnly = true)]
        public Cadastral Fake { get; set; }

        /// <summary>
        /// Получает или задает ИД кадастрового объекта' (штрих).
        /// </summary>
        [SystemProperty]
        public int? FakeID { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Группа учета (вид) ОС/группа консолидации (МСФО)", Visible = false)]
        [ForeignKey("GroupConsolidationMSFOID")]
        public PositionConsolidation GroupConsolidationMSFO { get; set; }

        [SystemProperty]
        public int? GroupConsolidationMSFOID { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Группа учета (вид) ОС/группа консолидации (РСБУ)", Visible = false)]
        [ForeignKey("GroupConsolidationRSBUID")]
        public PositionConsolidation GroupConsolidationRSBU { get; set; }

        [SystemProperty]
        public int? GroupConsolidationRSBUID { get; set; }

        /// <summary>
        /// Первоначальная стоимость ИК по данным НУ
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Первоначальная стоимость ИК по данным НУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? InitialCostNU => _initialCostNU.Evaluate(this);

        /// <summary>
        /// Первоначальная стоимость ИК по данным БУ
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Первоначальная стоимость ИК по данным БУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? InitialCostOBU => _initialCostOBU.Evaluate(this);

        /// <summary>
        /// Количество объектов
        /// </summary>
        [FullTextSearchProperty]
        [ListView("Количество объектов", Visible = false)]
        [DetailView(Name = "Количество объектов", ReadOnly = true, Visible = false)]
        public int? InventoryObjectsCount => _inventoryObjectsCount.Evaluate(this);

        [DetailView("Отнесение к категории памятников истории и культуры", Visible = false)]
        public bool IsCultural { get; set; }

        /// <summary>
        /// Получает или задает признак что это имущество как ИК.
        /// </summary>
        [SystemProperty]
        [DetailView("ОИ как имущественный комплекс", TabName = EstateTabs.GeneralData, ReadOnly = true, Order = 1)]
        [DefaultValue(false)]
        public bool IsPropertyComplex { get; set; }

        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        public bool IsRealEstate { get; set; }

        [DetailView("Признак объекта социально-культурного или бытового назначения", Visible = false)]
        [DefaultValue(false)]
        public bool IsSocial { get; set; }

        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        public LayingType LayingType { get; set; }

        /// <summary>
        /// Получает или задает признак является ли инвентарный объект недвижимым.
        /// </summary>
        /// <summary>
        /// Получает или задает признак социально-культурного или бытового назначения.
        /// </summary>
        /// <summary>
        /// Получает или задает признак культурного наследия.
        /// </summary>
        /// <summary>
        /// Получает или задает ИД типа прокладки лин. сооружения.
        /// </summary>
        public int? LayingTypeID { get; set; }

        [DetailView("Арендатор (Лизингополучатель)/Пользователь по договору", Visible = false)]
        [ListView("Арендатор (Лизингополучатель)/Пользователь по договору", Visible = false)]
        public Entities.Subject.Subject LessorSubject { get; set; }

        [SystemProperty]
        public int? LessorSubjectID { get; set; }

        [ListView(Visible = false)]
        [DetailView("Лицензионный участок", Visible = false)]
        public LicenseArea LicenseArea { get; set; }

        [SystemProperty]
        public int? LicenseAreaID { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Изготовитель", Visible = false)]
        [ListView("Изготовитель", Visible = false)]
        public String Manufacturer { get; set; }

        [DetailView("Форма собственности", Visible = false)]
        [ListView("Форма собственности", Visible = false)]
        public OwnershipType OwnershipType { get; set; }

        [SystemProperty]
        public int? OwnershipTypeID { get; set; }

        /// <summary>
        /// Получает или задает родительский инв. объект.
        /// </summary>
        [ListView(Name = "Вышестоящий объект имущества", Visible = false)]
        [DetailView(Name = "Вышестоящий объект имущества", TabName = EstateTabs.GeneralData, Order = 3)]
        public InventoryObject Parent { get; set; }

        /// <summary>
        /// Получает или задает ИД родительский инв.объекта.
        /// </summary>
        [ListView(Hidden = true, Visible = false)]
        [SystemProperty]
        public int? ParentID { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Номер паспорта объекта", Visible = false)]
        [ListView("Номер паспорта объекта", Visible = false)]
        public String PassportNumber { get; set; }

        [DetailView("Длина трубопровода, м.", Visible = false)]
        [ListView("Длина трубопровода, м.", Visible = false)]
        public decimal? PipelineLength { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Позиция консолидации", Visible = false)]
        public PositionConsolidation PositionConsolidation { get; set; }

        [SystemProperty]
        public int? PositionConsolidationID { get; set; }

        [DetailView("ИК", Visible = false)]
        [ListView(Hidden = true)]
        public PropertyComplex PropertyComplex { get; set; }

        /// <summary>
        /// Получает или задает тип прокладки линейного сооружения.
        /// </summary>
        /// <summary>
        /// Получает или задает ИД имущественного комплекса.
        /// </summary>
        [SystemProperty]
        public int? PropertyComplexID { get; set; }

        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView("Наименование имущественного комплекса", ReadOnly = true,
        //TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String PropertyComplexName { get; set; }

        /// <summary>
        /// Получает или задает имущественный комплекс.
        /// </summary>
        /// <summary>
        /// Получает собственник(-ов).
        /// </summary>
        /// <remarks>
        /// Право.Субъект -> Деловой партнёр.Краткое наименование на русском языке.
        /// В случае, если тип права - собственность и право актуально.
        /// Если прав такого типа несколько - перечень значений.
        /// </remarks>
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Proprietor { get; set; }

        [DetailView("Арендодатель (Лизингодатель)/Собственник по договору", Visible = false)]
        [ListView("Арендодатель (Лизингодатель)/Собственник по договору", Visible = false)]
        public Entities.Subject.Subject ProprietorSubject { get; set; }

        [SystemProperty]
        public int? ProprietorSubjectID { get; set; }

        /// <summary>
        /// Получает наименование имущественного комплекса.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionCode { get; set; }

        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionName { get; set; }

        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionStr { get; set; }

        //[PropertyDataType(PropertyDataType.Text)]
        [ListView(Visible = false)]
        [DetailView("Дата договора аренды", Visible = false)]
        public DateTime? RentContractDate { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Visible = false)]
        [DetailView("Номер договора аренды", Visible = false)]
        public string RentContractNumber { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Visible = false)]
        [DetailView("Номер договора аренды СЦВД", Visible = false)]
        public string RentContractNumberSZVD { get; set; }

        /// <summary>
        /// Получает арендатора(-ов).
        /// </summary>
        /// <remarks>
        /// Право.Субъект -> Деловой партнёр.Краткое наименование на русском языке
        /// В случае, если тип права - аренда и право актуально. Если прав такого типа несколько - перечень значений
        /// </remarks>
        // TODO: добавить логику вычисления арендаторов.
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView("Арендатор(ы)", ReadOnly = true,
        //TabName = TabName4)]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String Renter { get; set; }

        [DetailView("Тип аренды МСФО", Visible = false)]
        [ListView(Visible = false)]
        public RentTypeRSBU RentTypeMSFO { get; set; }

        [SystemProperty]
        public int? RentTypeMSFOID { get; set; }

        [DetailView("Тип аренды РСБУ", Visible = false)]
        [ListView(Visible = false)]
        public RentTypeRSBU RentTypeRSBU { get; set; }

        [SystemProperty]
        public int? RentTypeRSBUID { get; set; }

        /// <summary>
        /// Остаточная стоимость ИК по данным НУ
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Остаточная стоимость ИК по данным НУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? ResidualCostNU => _residualCostNU.Evaluate(this);

        /// <summary>
        /// Остаточная стоимость ИК по данным БУ
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Остаточная стоимость ИК по данным БУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? ResidualCostOBU => _residualCostOBU.Evaluate(this);

        [ListView(Visible = false)]
        [DetailView(Name = "Доля в праве (знаменатель доли)", Visible = false)]
        [DefaultValue(1)]
        public int? ShareRightDenominator { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Доля в праве (числитель доли)", Visible = false)]
        [DefaultValue(1)]
        public int? ShareRightNumerator { get; set; }

        [DetailView("Город", Visible = false)]
        [ListView("Город", Visible = false)]
        public SibCityNSI SibCityNSI { get; set; }

        public int? SibCityNSIID { get; set; }

        [DetailView("Страна", Visible = false)]
        [ListView("Страна", Visible = false)]
        public SibCountry SibCountry { get; set; }

        [SystemProperty]
        public int? SibCountryID { get; set; }

        [DetailView("Федеральный округ", Visible = false)]
        [ListView("Федеральный округ", Visible = false)]
        public SibFederalDistrict SibFederalDistrict { get; set; }

        [SystemProperty]
        public int? SibFederalDistrictID { get; set; }

        [DetailView("Субъект РФ/Регион", Visible = false)]
        public SibRegion SibRegion { get; set; }

        [SystemProperty]
        public int? SibRegionID { get; set; }

        [DetailView("Код СПП из БИП", Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SPPCode { get; set; }

        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        public StageOfCompletion StageOfCompletion { get; set; }

        /// <summary>
        /// Получает или задает ИД стадии готовности.
        /// </summary>
        public int? StageOfCompletionID { get; set; }

        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        public CorpProp.Entities.NSI.StatusConstruction StatusConstruction { get; set; }

        /// <summary>
        /// Получает или задает стадию готовности.
        /// </summary>
        /// <summary>
        /// Получает или задает ИД статуса строительства.
        /// </summary>
        public int? StatusConstructionID { get; set; }
    }
}