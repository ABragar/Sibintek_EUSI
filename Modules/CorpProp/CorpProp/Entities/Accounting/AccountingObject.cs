using Base.Attributes;
using Base.DAL;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Law;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CorpProp.Entities.Accounting
{
    /// <summary>
    /// Представляет карточку объекта бухгалтерского учета.
    /// </summary>
    /// <remarks>
    /// Запись об объекте: основном средстве (счета 01, 03, 001, 011),
    /// незавершённом строительстве (счёт 08), нематериальном активе (счёт 04)
    /// </remarks>
    [EnableFullTextSearch]
    public class AccountingObject : TypeObject, IArchiveObject
    {
       
        private static readonly CompiledExpression<AccountingObject, bool> _InGGR =
        DefaultTranslationOf<AccountingObject>.Property(x => x.InGGR).Is(x => (x.SSR != null || x.SSRTerminate != null));

        private static readonly CompiledExpression<AccountingObject, bool> _IsNonCoreAsset =
         DefaultTranslationOf<AccountingObject>.Property(x => x.IsNonCoreAsset).Is(x => x.Estate != null ? x.Estate.IsNonCoreAsset : false);

        private static readonly CompiledExpression<AccountingObject, string> _whoUseINN =
         DefaultTranslationOf<AccountingObject>.Property(x => x.WhoUseINN).Is(x => (x.WhoUse != null) ? x.WhoUse.INN : "");

        private static readonly CompiledExpression<AccountingObject, string> _whoUseString =
         DefaultTranslationOf<AccountingObject>.Property(x => x.WhoUseString).Is(x => (x.WhoUse != null) ? x.WhoUse.ShortName : "");

        /*private static readonly CompiledExpression<AccountingObject, bool> _outOfBalance =
            DefaultTranslationOf<AccountingObject>
                .Property(x => x.OutOfBalance)
                .Is(x => x.ReceiptReason != null && ReceiptReasonNameForOutOfBalance.Contains(x.ReceiptReason.Name));*/

        private static readonly CompiledExpression<AccountingObject, string> _consolidationCode =
            DefaultTranslationOf<AccountingObject>
                .Property(x => x.ConsolidationCode)
                .Is(x => x.Consolidation != null ? x.Consolidation.Code : "");

        private static readonly CompiledExpression<AccountingObject, string> _consolidationName =
            DefaultTranslationOf<AccountingObject>
                .Property(x => x.ConsolidationName)
                .Is(x => x.Consolidation != null ? x.Consolidation.Name : "");

        private static readonly CompiledExpression<AccountingObject, decimal> _ownerShareInEquity =
       DefaultTranslationOf<AccountingObject>.Property(x => x.Owner3).Is(x => (x.Owner != null) ? (decimal)x.Owner.ShareInEquity : 0);
        private static readonly CompiledExpression<AccountingObject, decimal> _ownerShareInVotingRights =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner4).Is(x => (x.Owner != null) ? (decimal)x.Owner.ShareInVotingRights : 0);
        private static readonly CompiledExpression<AccountingObject, string> _ownerFullName =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner5).Is(x => (x.Owner != null) ? (string)x.Owner.FullName : "");
        private static readonly CompiledExpression<AccountingObject, string> _ownerShortName =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner6).Is(x => (x.Owner != null) ? (string)x.Owner.ShortName : "");
        private static readonly CompiledExpression<AccountingObject, string> _ownerCountry =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner7).Is(x => (x.Owner != null && x.Owner.Country != null) ? (string)x.Owner.Country.Name : "");
        private static readonly CompiledExpression<AccountingObject, string> _ownerFederalDistrict =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner8).Is(x => (x.Owner != null && x.Owner.FederalDistrict != null) ? (string)x.Owner.FederalDistrict.Name : "");
        private static readonly CompiledExpression<AccountingObject, string> _ownerRegion =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner9).Is(x => (x.Owner != null && x.Owner.Region != null) ? (string)x.Owner.Region.Name : "");
        private static readonly CompiledExpression<AccountingObject, string> _ownerCity =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner10).Is(x => (x.Owner != null) ? (string)x.Owner.City : "");
        private static readonly CompiledExpression<AccountingObject, bool> _ownerIsSocietyKey =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner11).Is(x => (x.Owner != null) ? (bool)x.Owner.IsSocietyKey : false);
        private static readonly CompiledExpression<AccountingObject, string> _ownerCurator =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner12).Is(x => (x.Owner != null) ? (string)x.Owner.Curator : "");
        private static readonly CompiledExpression<AccountingObject, string> _ownerUnitOfCompany =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner13).Is(x => (x.Owner != null && x.Owner.UnitOfCompany != null) ? (string)x.Owner.UnitOfCompany.Name : "");
        private static readonly CompiledExpression<AccountingObject, string> _ownerBusinessBlock =
        DefaultTranslationOf<AccountingObject>.Property(x => x.Owner14).Is(x => (x.Owner != null && x.Owner.BusinessBlock != null) ? (string)x.Owner.BusinessBlock.Name : "");



        protected const string TabName1 = "[001]Классификаторы";
        protected const string TabName2 = "[002]Стоимость";
        protected const string TabName3 = "[003]Состояние";
        protected const string TabName4 = "[004]Дополнительные данные";
        protected const string TabName5 = "[005]Право";
        protected const string TabName6 = "[006]Здание/Сооружение";
        protected const string TabName7 = "[007]Земля";
        protected const string TabName8 = "[008]Транспорт";
        protected const string TabName9 = "[009]Речное/Морское судно";
        protected const string TabName10 = "[010]Воздушное судно";
        protected const string TabName11 = "[011]Ссылки";
        protected const string TabName12 = "[012]Изменения";
                

        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingObject.
        /// </summary>
        public AccountingObject() : base()
        {
        }


        ///// <summary>
        ///// Получает Балансодержатель (Код ЕУП).
        ///// </summary>
        //[ListView("Балансодержатель (Код ЕУП)", Order = 13, Hidden = true)]
        //public string Owner1 => _ownerIDEUP.Evaluate(this);

        ///// <summary>
        ///// Получает Балансодержатель (БЕ).
        ///// </summary>
        //[ListView("Балансодержатель (БЕ)", Order = 13, Hidden = true)]
        //public string Owner2 => _ownerConsolidationCode.Evaluate(this);

        //Эффективная доля участия РН, %
        /// <summary>
        /// Получает Балансодержатель (Эффективная доля участия РН, %).
        /// </summary>
        [ListView("Балансодержатель (Эффективная доля участия РН, %)", Order = 13, Hidden = true)]
        public decimal Owner3 => _ownerShareInEquity.Evaluate(this);
        //Эффективная доля участия РН в голосующих акциях, %
        /// <summary>
        /// Получает Балансодержатель (Эффективная доля участия РН в голосующих акциях, %).
        /// </summary>
        [ListView("Балансодержатель (Эффективная доля участия РН в голосующих акциях, %)", Order = 13, Hidden = true)]
        public decimal Owner4 => _ownerShareInVotingRights.Evaluate(this);
        //Полное наименование ОГ
        /// <summary>
        /// Получает Балансодержатель (Полное наименование ОГ).
        /// </summary>
        [ListView("Балансодержатель (Полное наименование ОГ)", Order = 13, Hidden = true)]
        public string Owner5 => _ownerFullName.Evaluate(this);
        //Сокращенное наименование ОГ
        /// <summary>
        /// Получает Балансодержатель (Сокращенное наименование ОГ).
        /// </summary>
        [ListView("Балансодержатель (Сокращенное наименование ОГ)", Order = 13, Hidden = true)]
        public string Owner6 => _ownerShortName.Evaluate(this);
        //Юрисдикция
        /// <summary>
        /// Получает Балансодержатель (Юрисдикция).
        /// </summary>
        [ListView("Балансодержатель (Юрисдикция)", Order = 13, Hidden = true)]
        public string Owner7 => _ownerCountry.Evaluate(this);
        //ФО
        /// <summary>
        /// Получает Балансодержатель (ФО).
        /// </summary>
        [ListView("Балансодержатель (ФО)", Order = 13, Hidden = true)]
        public string Owner8 => _ownerFederalDistrict.Evaluate(this);
        //Субъект РФ
        /// <summary>
        /// Получает Балансодержатель (Субъект РФ).
        /// </summary>
        [ListView("Балансодержатель (Субъект РФ)", Order = 13, Hidden = true)]
        public string Owner9 => _ownerRegion.Evaluate(this);
        //Город
        /// <summary>
        /// Получает Балансодержатель (Город).
        /// </summary>
        [ListView("Балансодержатель (Город)", Order = 13, Hidden = true)]
        public string Owner10 => _ownerCity.Evaluate(this);
        //Признак ключевого
        /// <summary>
        /// Получает Балансодержатель (Признак ключевого).
        /// </summary>
        [ListView("Балансодержатель (Признак ключевого)", Order = 13, Hidden = true)]
        public bool Owner11 => _ownerIsSocietyKey.Evaluate(this);
        //Куратор
        /// <summary>
        /// Получает Балансодержатель (Куратор).
        /// </summary>
        [ListView("Балансодержатель (Куратор)", Order = 13, Hidden = true)]
        public string Owner12 => _ownerCurator.Evaluate(this);
        //Курирующее СП
        /// <summary>
        /// Получает Балансодержатель (Курирующее СП).
        /// </summary>
        [ListView("Балансодержатель (Курирующее СП)", Order = 13, Hidden = true)]
        public string Owner13 => _ownerUnitOfCompany.Evaluate(this);
        //Бизнес-блок
        /// <summary>
        /// Получает Балансодержатель (Бизнес-блок).
        /// </summary>
        [ListView("Балансодержатель (Бизнес-блок)", Order = 13, Hidden = true)]
        public string Owner14 => _ownerBusinessBlock.Evaluate(this);







        /// <summary>
        /// Получает или задает статус ОБУ.
        /// </summary>
        [ListView("Статус", Hidden = true)]
        [DetailView("Статус", TabName = TabName3, Order = 49)]
        public AccountingStatus AccountingStatus { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса ОБУ.
        /// </summary>
        [SystemProperty]
        public int? AccountingStatusID { get; set; }

        /// <summary>
        /// Получает или задает счет главной книги ЛУС.
        /// </summary>
        [ListView("Счет главной книги ЛУС", Hidden = true)]
        [DetailView("Счет главной книги ЛУС", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountLedgerLUS { get; set; }

        /// <summary>
        /// Получает или задает номер счета.
        /// </summary>
        [ListView("Счет", Hidden = true)]
        [DetailView(Name = "Счет", TabName = CaptionHelper.DefaultTabName, Order = 6)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Получает или задает амортизацию (накопленнаю) по РСБУ, руб..
        /// </summary>
        [ListView("Амортизация (накопленная) по РСБУ, руб.", Visible = false)]
        [DetailView("Амортизация (накопленная) по РСБУ, руб.", Visible = false)]
        public decimal? AccumulatedDepreciationRSBU { get; set; }

        /// <summary>
        /// Получает или задает дату получения объекта аренды (дата акта приемки-передачи).
        /// </summary>
        [ListView("Дата получения объекта аренды (дата акта приемки-передачи)", Visible = false)]
        [DetailView("Дата получения объекта аренды (дата акта приемки-передачи)", Visible = false)]
        public DateTime? ActRentDate { get; set; }

        /// <summary>
        /// Получает или задает Доп. признак категории земель.
        /// </summary>
        [DetailView("Доп. признак категории земель", Visible = false)]
        [ListView("Доп. признак категории земель", Visible = false)]
        public AddonAttributeGroundCategory AddonAttributeGroundCategory { get; set; }

        /// <summary>
        /// Получает или задает ИД доп.признака категории земель.
        /// </summary>
        [SystemProperty]
        public int? AddonAttributeGroundCategoryID { get; set; }

        /// <summary>
        /// Получает или задает доп. код ОКОФ.
        /// </summary>
        [DetailView("Доп. код ОКОФ", Visible = false)]
        [ListView("Доп. код ОКОФ", Visible = false)]
        public AddonOKOF AddonOKOF { get; set; }

        /// <summary>
        /// Получает или задает ИД доп. кода ОКОФ.
        /// </summary>
        [SystemProperty]
        public int? AddonOKOFID { get; set; }

        /// <summary>
        /// Получает или задает адрес местонахождения (местоположения).
        /// </summary>       
        [ListView("Адрес местонахождения", Hidden = true)]
        [DetailView("Адрес местонахождения", TabName = TabName4, Order = 55)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address { get; set; }

        /// <summary>
        /// Получает или задает назначение воздушного судна.
        /// </summary>
        [DetailView(Name = "Назначение судна", TabName = TabName10, Order = 136)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AircraftAppointment { get; set; }

        /// <summary>
        /// Получает или задает вид воздушного судна.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Вид судна", TabName = TabName10, Order = 133)]
        public AircraftKind AircraftKind { get; set; }

        /// <summary>
        /// Получает или задает ИД вида воздушного судна.
        /// </summary>
        public int? AircraftKindID { get; set; }

        /// <summary>
        /// Получает или задает тип воздушного судна.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Тип судна", TabName = TabName10, Order = 134)]
        public AircraftType AircraftType { get; set; }

        /// <summary>
        /// Получает или задает ИД типа воздушного судна.
        /// </summary>
        public int? AircraftTypeID { get; set; }

        /// <summary>
        /// Получает или задает адрес места базирования воздушного судна.
        /// </summary>
        [DetailView(Name = "Адрес места базирования", TabName = TabName10, Order = 142)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AirtcraftLocation { get; set; }

        /// <summary>
        /// Получает или задает среднегодовую стоимость имущества.
        /// </summary>
        [ListView("Среднегодовая стоимость имущества", Visible = false)]
        [DetailView("Среднегодовая стоимость имущества", Visible = false)]
        [DefaultValue(0)]
        public decimal? AnnualCostAvg { get; set; }

        /// <summary>
        /// Получает или задает площадь объекта недвижимости, кв.м.
        /// </summary>
        [ListView("Площадь объекта недвижимости, кв.м.", Visible = false)]
        [DetailView("Площадь объекта недвижимости, кв.м.", Visible = false)]
        [DefaultValue(0)]
        public decimal? Area { get; set; }

        /// <summary>
        /// Получает или задает номер партии.
        /// </summary>
        [ListView("Номер партии", Visible = false)]
        [DetailView("Номер партии", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BatchNumber { get; set; }

        /// <summary>
        /// Получает или задает признак льготируемого объекта.
        /// </summary>
        [DetailView("Льготируемый объект", Visible = false)]
        [ListView("Льготируемый объект", Visible = false)]
        [DefaultValue(false)]
        public bool Benefit { get; set; }

        /// <summary>
        /// Получает или задает признак применения льготы.
        /// </summary>
        [DetailView("Применение льготы", Visible = false)]
        [ListView("Применение льготы", Visible = false)]
        [DefaultValue(false)]
        public bool BenefitApply { get; set; }

        /// <summary>
        /// Получает или задает признак применения льготы для энегроэффективного оборудования.
        /// </summary>
        [DetailView("Применение льготы для энегроэффективного оборудования", Visible = false)]
        [ListView("Применение льготы для энегроэффективного оборудования", Visible = false)]
        [DefaultValue(false)]
        public bool BenefitApplyForEnergy { get; set; }

        /// <summary>
        /// Получает или задает признак применения льготы по земельному налогу.
        /// </summary>
        [DetailView("Применение льготы. Земельный налог", Visible = false)]
        [ListView("Применение льготы. Земельный налог", Visible = false)]
        [DefaultValue(false)]
        public bool BenefitApplyLand { get; set; }

        /// <summary>
        /// Получает или задает признак применения льготы по транспортному налогу.
        /// </summary>
        [DetailView("Применение льготы. Транспортный налог", Visible = false)]
        [ListView("Применение льготы. Транспортный налог", Visible = false)]
        [DefaultValue(false)]
        public bool BenefitApplyTS { get; set; }

        /// <summary>
        /// Получает или задает признак наличия док-ов, подтверждающих применение льготы.
        /// </summary>
        [DetailView("Наличие документов, подтверждающих применение льготы", Visible = false)]
        [ListView("Наличие документов, подтверждающих применение льготы", Visible = false)]
        [DefaultValue(false)]
        public bool BenefitDocsExist { get; set; }

        /// <summary>
        /// Получает или задает площадь застройки, кв.м.
        /// </summary>
        [ListView("Площадь застройки, кв.м.", Hidden = true)]
        [DetailView("Площадь застройки, кв.м.", TabName = TabName6, Order = 69)]
        public decimal? BuildingArea { get; set; }

        /// <summary>
        /// Получает или задает кадастровый/условный номер здания.
        /// </summary>
        [ListView("Кадастровый/условный номер", Hidden = true)]
        [DetailView("Кадастровый/условный номер", TabName = TabName6, Order = 68)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BuildingCadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает общую характеристику (описание) здания.
        /// </summary>
        [ListView("Общая характеристика (описание)", Hidden = true)]
        [DetailView("Общая характеристика (описание)", TabName = TabName6, Order = 82)]
        [PropertyDataType(PropertyDataType.Text)]
        public string BuildingDescription { get; set; }

        /// <summary>
        /// Получает или задает этажность.
        /// </summary>
        [ListView("Этажность", Hidden = true)]
        [DetailView("Этажность", TabName = TabName6, Order = 71)]
        public decimal? BuildingFloor { get; set; }

        /// <summary>
        /// Получает или задает общую площадь здания, кв.м.
        /// </summary>
        [ListView("Общая площадь здания, кв.м.", Hidden = true)]
        [DetailView("Общая площадь здания, кв.м.", TabName = TabName6, Order = 70)]
        public decimal? BuildingFullArea { get; set; }

        /// <summary>
        /// Получает или задает длину линейного сооружения.
        /// </summary>
        [ListView("Длина линейного сооружения, м.", Hidden = true)]
        [DetailView("Длина линейного сооружения, м.", TabName = TabName6, Order = 73)]
        public decimal? BuildingLength { get; set; }

        /// <summary>
        /// Получает или задает наименование здания.
        /// </summary>
        [DetailView(Name = "Наименование", TabName = TabName6, Order = 67)]
        [PropertyDataType(PropertyDataType.Text)]
        public string BuildingName { get; set; }

        /// <summary>
        /// Получает или задает кол-во подземных этажей.
        /// </summary>
        [ListView("Подземная этажность", Hidden = true)]
        [DetailView("Подземная этажность", TabName = TabName6, Order = 72)]
        public decimal? BuildingUnderground { get; set; }

        /// <summary>
        /// Получает или задает место постройки.
        /// </summary>
        [ListView("Место постройки", Hidden = true)]
        [DetailView("Место постройки", TabName = TabName9, Order = 112)]
        [PropertyDataType(PropertyDataType.Text)]
        public string BuildPlace { get; set; }

        /// <summary>
        /// Получает или задает год постройки.
        /// </summary>
        [ListView("Год постройки", Hidden = true)]
        [DetailView("Год постройки", TabName = TabName9, Order = 111)]
        public int? BuildYear { get; set; }

        /// <summary>
        /// Получает или задает куст.
        /// </summary>
        [ListView("Куст", Hidden = true)]
        [DetailView("Куст", TabName = TabName4, Order = 57)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Bush { get; set; }

        /// <summary>
        /// Получает или задает бизнес-сферу.
        /// </summary>
        [ListView("Обособленное подразделение/БС", Hidden = true)]
        [DetailView("Обособленное подразделение/БС", TabName = CaptionHelper.DefaultTabName, Order = 11)]
        public virtual BusinessArea BusinessArea { get; set; }

        /// <summary>
        /// Получает или задает ИД бизнес-сферы.
        /// </summary>
        [SystemProperty]
        public int? BusinessAreaID { get; set; }

        /// <summary>
        /// Получает или задает кадастровый номер.
        /// </summary>
        [ListView("Кадастровый номер", Order = 10)]
        [DetailView("Кадастровый номер", TabName = TabName5, Order = 60)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает актуальную кадастровую стоимость.
        /// </summary>        
        [ListView("Кадастровая стоимость, руб.")]
        [DetailView("Кадастровая стоимость, руб.", TabName = TabName2, Visible = false)]
        [DefaultValue(0)]
        public decimal? CadastralValue { get; set; }

        /// <summary>
        /// Получает или задает дату присвоения кадастровой стоимости.
        /// </summary>        
        [ListView("Дата присвоения кадастровой стоимости", Visible = false)]
        [DetailView("Дата присвоения кадастровой стоимости", Visible = false)]
        public DateTime? CadastralValueAppDate { get; set; }

        /// <summary>
        /// Получает или задает дату постановки на гос. кадастровый учет.
        /// </summary>        
        [ListView("Дата постановки на государственный кадастровый учет", Visible = false)]
        [DetailView("Дата постановки на государственный кадастровый учет", Visible = false)]
        public DateTime? CadRegDate { get; set; }

        /// <summary>
        /// Получает или задает класс БУ.
        /// </summary>
        [ListView("Класс БУ", Order = 3)]
        [DetailView("Класс БУ", TabName = CaptionHelper.DefaultTabName, Order = 8)]
        public ClassFixedAsset ClassFixedAsset { get; set; }

        /// <summary>
        /// Получает или задает ИД класса БУ.
        /// </summary>
        [SystemProperty]
        public int? ClassFixedAssetID { get; set; }

        /// <summary>
        /// Получает или задает реквизиты документа о переводе объекта на консервацию.
        /// </summary>
        [ListView("Дата и № документа о переводе объекта на консервацию", Visible = false)]
        [DetailView("Дата и № документа о переводе объекта на консервацию", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ConservationDocInfo { get; set; }

        /// <summary>
        /// Получает или задает дату начала консервации.
        /// </summary>       
        [ListView("Дата начала консервации", Hidden = true)]
        [DetailView("Дата начала консервации", TabName = TabName3, Order = 47)]
        public DateTime? ConservationFrom { get; set; }

        /// <summary>
        /// Получает или задает реквизиты док-та о возвращении объекта из консервации.
        /// </summary>
        [ListView("Дата и № документа о возвращении объекта из консервации", Visible = false)]
        [DetailView("Дата и № документа о возвращении объекта из консервации", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ConservationReturnInfo { get; set; }

        /// <summary>
        /// Получает или задает дату окончания консервации.
        /// </summary>       
        [ListView("Дата окончания консервации", Hidden = true)]
        [DetailView("Дата окончания консервации", TabName = TabName3, Order = 48)]
        public DateTime? ConservationTo { get; set; }

        /// <summary>
        /// Получает или задает балансовую единицу (единицу консолидации).
        /// </summary>
        [DetailView("БЕ", Visible = false)]
        [ListView("БЕ", Visible = false)]
        [FullTextSearchProperty]
        public virtual Consolidation Consolidation { get; set; }

        /// <summary>
        /// Получает или задает ИД балансовой единицы.
        /// </summary>
        [SystemProperty]
        public int? ConsolidationID { get; set; }

        public string ConsolidationCode => _consolidationCode.Evaluate(this);

        public string ConsolidationName => _consolidationName.Evaluate(this);

        /// <summary>
        /// Получает или задает объем резервуара, куб.м.
        /// </summary>
        [ListView("Объем резервуара, куб.м.", Hidden = true)]
        [DetailView("Объем резервуара, куб.м.", TabName = TabName6, Order = 75)]
        public decimal? ContainmentVolume { get; set; }

        /// <summary>
        /// Получает или задает дату договора.
        /// </summary>
        [ListView("Дата договора", Hidden = true)]
        [DetailView("Дата договора", TabName = TabName3, Order = 54)]
        public DateTime? ContractDate { get; set; }

        /// <summary>
        /// Получает или задает № договора (из заявки).
        /// </summary>
        [DetailView("№ договора", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Получает или задает поставщика.
        /// </summary>
        [DetailView("Поставщик", Visible = false)]
        [ListView("Поставщик", Visible = false)]
        public Subject.Subject Contragent { get; set; }

        /// <summary>
        /// Получает или задает ИД поставщика.
        /// </summary>
        [SystemProperty]
        public int? ContragentID { get; set; }

        /// <summary>
        /// Получает или задает атрибут "Комментарий".
        /// </summary>
        [DetailView(Name = "Комментарий", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comment { get; set; }

        /// <summary>
        /// Получает или задает предполагаемые затраты на продажу.
        /// </summary>
        [ListView("Предполагаемые затраты на продажу", Visible = false)]
        [DetailView("Предполагаемые затраты на продажу", Visible = false)]
        [DefaultValue(0)]
        public decimal? CostForSale { get; set; }

        /// <summary>
        /// Получает или задает номер заявки из которой был создан ОС.
        /// </summary>
        [ListView("Номер заявки", Visible = false)]
        [DetailView("Номер заявки", Visible = false)]
        public int? CreatingFromER { get; set; }

        /// <summary>
        /// Получает или задает номер позиции заявки из которой был создан ОС.
        /// </summary>
        [ListView("Номер позиции", Visible = false)]
        [DetailView("Номер позиции", Visible = false)]
        public int? CreatingFromERPosition { get; set; }

        /// <summary>
        /// Получает или задает дату включения в перечень объектов, облагаемых налогом по кадастровой стоимости.
        /// </summary>
        [DetailView("Дата включения в перечень объектов",
        Description = "Дата включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения",
        Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? DateInclusion { get; set; }

        /// <summary>
        /// Получает или задает дату оприходования.
        /// </summary>
        [ListView("Дата оприходования", Order = 5)]
        [DetailView("Дата оприходования", TabName = CaptionHelper.DefaultTabName, Order = 16)]
        public DateTime? DateOfReceipt { get; set; }

        /// <summary>
        /// Получает или задает грузоподъёмность речного/морского судна.
        /// </summary>
        [ListView("Полная грузоподъемность судна", Visible = false)]
        [DetailView("Полная грузоподъемность судна", TabName = TabName9, Order = 128)]
        [DefaultValue(0)]
        public decimal? DeadWeight { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения грузоподъёмности речного/морского судна.
        /// </summary>
        [ListView("Ед. измернения грузоподъемности судна", Visible = false)]
        [DetailView("Ед. измернения", TabName = TabName9, Order = 129)]
        public SibMeasure DeadWeightUnit { get; set; }

        /// <summary>
        /// Получает или задает ИД ед. измерения грузоподъемности речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? DeadWeightUnitID { get; set; }

        /// <summary>
        /// Получает или задает реквизиты договора.
        /// </summary>
        [ListView("Реквизиты договора", Hidden = true)]
        [DetailView("Реквизиты договора", TabName = TabName3, Order = 52)]
        [PropertyDataType(PropertyDataType.Text)]
        public string DealProps { get; set; }

        /// <summary>
        /// Получает или задает реквизиты решения органа субъектов/муниципальных образований по налогу на имущество.
        /// </summary>
        [ListView("Реквизиты решения органа субъектов/муниципальных образований", Visible = false)]
        [DetailView("Реквизиты решения органа субъектов/муниципальных образований", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [ForeignKey("DecisionsDetailsID")]
        public DecisionsDetails DecisionsDetails { get; set; }

        /// <summary>
        /// Получает или задает ИД решения МО по налогу наимущество.
        /// </summary>
        [SystemProperty]
        public int? DecisionsDetailsID { get; set; }

        /// <summary>
        /// Получает или задает реквизиты решения МО по земельному налогу.
        /// </summary>
        [ListView("Реквизиты решения органа субъектов/муниципальных образований  по земельному налогу", Visible = false)]
        [DetailView("Реквизиты решения органа субъектов/муниципальных образований  по земельному налогу", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [ForeignKey("DecisionsDetailsLandID")]
        public DecisionsDetailsLand DecisionsDetailsLand { get; set; }

        /// <summary>
        /// Получает или задает ИД решения МО по земельному налогу.
        /// </summary>
        [SystemProperty]
        public int? DecisionsDetailsLandID { get; set; }

        /// <summary>
        /// Получает или задает решение МО по транспортному налогу.
        /// </summary>
        [ListView("Реквизиты решения органа субъектов/муниципальных образований по транспортному налогу", Visible = false)]
        [DetailView("Реквизиты решения органа субъектов/муниципальных образований по транспортному налогу", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [ForeignKey("DecisionsDetailsTSID")]
        public DecisionsDetailsTS DecisionsDetailsTS { get; set; }

        /// <summary>
        /// Получает или задает ИД решения МО по транспортному налогу.
        /// </summary>
        [SystemProperty]
        public int? DecisionsDetailsTSID { get; set; }

        /// <summary>
        /// Получает или задает подразделение.
        /// </summary>
        [ListView("Подразделение", Visible = false)]
        [DetailView("Подразделение", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Department { get; set; }

        /// <summary>
        /// Получает или задает месторождение.
        /// </summary>        
        [ListView("Месторождение", Hidden = true)]
        [DetailView("Месторождение", TabName = TabName4, Order = 56)]
        public Deposit Deposit { get; set; }

        /// <summary>
        /// Получает или задает ИД месторождения.
        /// </summary>
        [SystemProperty]
        public int? DepositID { get; set; }

        /// <summary>
        /// Получает или задает начисленную амортизацию в руб.
        /// </summary>
        [ListView("Начисленная амортизация, руб.", Hidden = true)]
        [DetailView("Начисленная амортизация, руб.", TabName = TabName2, Order = 35)]
        [DefaultValue(0)]
        public decimal? DepreciationCost { get; set; }

        /// <summary>
        /// Получает или задает накопленную амортизацию по налоговому учету.
        /// </summary>
        [ListView("Начисленная амортизация, руб.", Visible = false)]
        [DetailView("Начисленная амортизация, руб.", Visible = false)]
        [DefaultValue(0)]
        public decimal? DepreciationCostNU { get; set; }

        /// <summary>
        /// Получает или задает амортизационную группу НУ.
        /// </summary>
        [DetailView("Амортизационная группа НУ", Visible = false)]
        [ListView("Амортизационная группа НУ", Visible = false)]
        public virtual DepreciationGroup DepreciationGroup { get; set; }

        /// <summary>
        /// Получает или задает ИД амортизационной группы.
        /// </summary>
        [SystemProperty]
        public int? DepreciationGroupID { get; set; }

        /// <summary>
        /// Получает или здаает метод амортизации (МСФО).
        /// </summary>
        [DetailView("Метод амортизации (МСФО)", Visible = false)]
        [ListView("Метод амортизации (МСФО)", Visible = false)]
        public DepreciationMethodMSFO DepreciationMethodMSFO { get; set; }

        /// <summary>
        /// Получает или задает ИД метода амортизации МСФО.
        /// </summary>
        [SystemProperty]
        public int? DepreciationMethodMSFOID { get; set; }

        /// <summary>
        /// Получает или задает метод амортизации НУ.
        /// </summary>
        [DetailView("Метод амортизации НУ", Visible = false)]
        [ListView("Метод амортизации НУ", Visible = false)]
        public DepreciationMethodNU DepreciationMethodNU { get; set; }

        /// <summary>
        /// Получает или задает ИД метода амортизации НУ.
        /// </summary>
        [SystemProperty]
        public int? DepreciationMethodNUID { get; set; }

        /// <summary>
        /// Получает или задает метод амортизации РСБУ.
        /// </summary>
        [DetailView("Метод амортизации РСБУ", Visible = false)]
        [ListView("Метод амортизации РСБУ", Visible = false)]
        public DepreciationMethodRSBU DepreciationMethodRSBU { get; set; }

        /// <summary>
        /// Получает или задает ИД метода амортизации РСБУ.
        /// </summary>
        [SystemProperty]
        public int? DepreciationMethodRSBUID { get; set; }

        /// <summary>
        /// Получает или задает коэффициент ускоренной амортизации НУ.
        /// </summary>
        [DetailView("Коэффициент ускоренной амортизации для НУ", Visible = false)]
        [ListView("Коэффициент ускоренной амортизации для НУ", Visible = false)]
        [DefaultValue(0)]
        public decimal? DepreciationMultiplierForNU { get; set; }

        /// <summary>
        /// Получает или задает глубину скважины в м.
        /// </summary>
        [ListView("Глубина скважины, м.", Hidden = true)]
        [DetailView("Глубина скважины, м.", TabName = TabName6, Order = 76)]
        [DefaultValue(0)]
        public decimal? DepthWell { get; set; }

        /// <summary>
        /// Получает или задает описание объекта..
        /// </summary>
        [ListView("Описание", Hidden = true)]
        [DetailView("Описание", TabName = CaptionHelper.DefaultTabName, Order = 10)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает признак наличия дизельного двигателя у ТС.
        /// </summary>
        [ListView("Дизельный двигатель", Hidden = true)]
        [DetailView("Дизельный двигатель", TabName = TabName8, Order = 95)]
        [DefaultValue(false)]
        public bool DieselEngine { get; set; }

        /// <summary>
        /// Получает или задает отделяемое/неотделяемое имущество.
        /// </summary>
        [DetailView("Отделимое/неотделимое имущество", Visible = false)]
        [ListView("Отделимое/неотделимое имущество", Visible = false)]
        public DivisibleType DivisibleType { get; set; }

        /// <summary>
        /// Получает или задает ИД отделяемого/неотделяемого имущества.
        /// </summary>
        [SystemProperty]
        public int? DivisibleTypeID { get; set; }

        /// <summary>
        /// Получает или задает номер документа о включении в перечень объектов, облагаемых налогом по кадастровой стоимости.
        /// </summary>
        [DetailView("Номер документа включения в перечень объектов",
        Description = "Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения",
        Visible = false)]
        [ListView("Номер документа включения в перечень объектов", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Получает или задает осадку в полном грузу для речного/морского судна.
        /// </summary>
        [ListView("Осадка в полном грузу судна", Visible = false)]
        [DetailView("Осадка в полном грузу судна", TabName = TabName9, Order = 122)]
        [DefaultValue(0)]
        public decimal? DraughtHard { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения осадки в полном грузу для речного/морского судна.
        /// </summary>
        [ListView("Ед. измернения осадки в полном грузу речного/морского судна", Visible = false)]
        [DetailView("Ед. измернения осадки в полном грузу", TabName = TabName9, Order = 123)]
        public SibMeasure DraughtHardUnit { get; set; }

        /// <summary>
        /// Получает или задает ИД ед. измерения осадки в полном грузу речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? DraughtHardUnitID { get; set; }

        /// <summary>
        /// Получает или задает осадку порожнем для речного/морского судна.
        /// </summary>
        [ListView("Осадка порожнем речного/морского судна", Visible = false)]
        [DetailView("Осадка порожнем судна", TabName = TabName9, Order = 124)]
        [DefaultValue(0)]
        public decimal? DraughtLight { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения осадки порожнем речного/морского судна.
        /// </summary>
        [ListView("Ед. измернения осадки порожнем речного/морского судна", Visible = false)]
        [DetailView("Ед. измернения", TabName = TabName9, Order = 125)]
        public SibMeasure DraughtLightUnit { get; set; }

        /// <summary>
        /// Получает или задает ИД ед. измерения осадки порожнем речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? DraughtLightUnitID { get; set; }

        /// <summary>
        /// Получает или задает экологический класс ТС.
        /// </summary>
        [DetailView(Name = "Экологический класс", Visible = false)]
        [ListView(Name = "Экологический класс", Visible = false)]
        public virtual EcoKlass EcoKlass { get; set; }

        /// <summary>
        /// Получает или задает ИД экологического класса ТС.
        /// </summary>
        [SystemProperty]
        public int? EcoKlassID { get; set; }

        /// <summary>
        /// Получает или задает ЕНАОФ.
        /// </summary>
        [ListView("ЕНАОФ", Visible = false)]
        [DetailView("ЕНАОФ", Visible = false)]
        public ENAOF ENAOF { get; set; }

        /// <summary>
        /// Получает или задает ИД ЕНАОФ.
        /// </summary>
        [SystemProperty]
        public int? ENAOFID { get; set; }

        /// <summary>
        /// Получает или задает признак наличия ограничения/обременения.
        /// </summary>
        [ListView("Обременение", Visible = false)]
        [DetailView("Обременение", Visible = false)]
        [DefaultValue(false)]
        public bool EncumbranceExist { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия записи ОБУ во внешней системе.
        /// </summary>
        [ListView("Дата окончания", Hidden = true)]
        [DetailView("Дата окончания", TabName = TabName3, Order = 51)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Получает или задает признак наличия док-ов, подтверждающих энергоэффективность оборудования.
        /// </summary>
        [DetailView("Наличие документов, подтверждающих энергоэффективность оборудования", Visible = false)]
        [ListView("Наличие документов, подтверждающих энергоэффективность оборудования", Visible = false)]
        [DefaultValue(false)]
        public bool EnergyDocsExist { get; set; }

        /// <summary>
        /// Получает или задает класс энергетической эффективности.
        /// </summary>
        [DetailView("Класс энергетической эффективности", Visible = false)]
        [ListView("Класс энергетической эффективности", Visible = false)]
        [DefaultValue(false)]
        public EnergyLabel EnergyLabel { get; set; }

        /// <summary>
        /// Получает или задает ИД класса энергетической эффективности.
        /// </summary>
        [SystemProperty]
        public int? EnergyLabelID { get; set; }

        /// <summary>
        /// Получает или задает номера двигателей ТС.
        /// </summary>
        [ListView("Номера двигателей ТС", Visible = false)]
        [DetailView("Номера двигателей", TabName = TabName10, Order = 138)]
        [PropertyDataType(PropertyDataType.Text)]
        public string EngineNumber { get; set; }

        /// <summary>
        /// Получает или задает объем двигателя ТС.
        /// </summary>
        [ListView("Объем двигателя ТС, л.", Hidden = true)]
        [DetailView("Объем двигателя, л.", TabName = TabName8, Order = 96)]
        public decimal? EngineSize { get; set; }

        /// <summary>
        /// Получает или задает ссылку на объект имущества.
        /// </summary>       
        [ListView("Объект имущества", Visible = false)]
        [DetailView("Объект имущества", TabName = TabName11, Order = 137)]
        public virtual CorpProp.Entities.Estate.Estate Estate { get; set; }

        /// <summary>
        /// Получает или задает тип объекта имущества.
        /// </summary>
        [ListView("Тип Объекта имущества")]
        [DetailView("Тип Объекта имущества", Required = false, TabName = CaptionHelper.DefaultTabName, Order = -3)]
        public EstateDefinitionType EstateDefinitionType { get; set; }

        /// <summary>
        /// Получает или задает ИД типа объекта имущества.
        /// </summary>
        [SystemProperty]
        public int? EstateDefinitionTypeID { get; set; }

        /// <summary>
        /// Получает или задает ИД объекта имущества.
        /// </summary>
        [SystemProperty]
        public int? EstateID { get; set; }

        /// <summary>
        /// Получает или задает признак движимого/недвижимого имущества.
        /// </summary>
        [DetailView("Признак движимое/недвижимое имущество", Visible = false)]
        [ListView(Visible = false)]
        public EstateMovableNSI EstateMovableNSI { get; set; }

        /// <summary>
        /// Получает или задает ИД признака движимого/недвижимого имущества.
        /// </summary>
        [SystemProperty]
        public int? EstateMovableNSIID { get; set; }

        /// <summary>
        /// Получает или задает класс корпоративной собственности.
        /// </summary>
        [ListView("Класс КС", Order = 6)]
        [DetailView("Класс КС", TabName = TabName1, Order = 18)]
        public EstateType EstateType { get; set; }

        /// <summary>
        /// Получает или задает ИД класса корпоративной собственности.
        /// </summary>
        public int? EstateTypeID { get; set; }

        /// <summary>
        /// Получает или задает сумму оценочного обязательства в стоимости ОС.
        /// </summary>
        [ListView("Сумма оценочного обязательства в стоимости ОС", Visible = false)]
        [DetailView("Сумма оценочного обязательства в стоимости ОС", Visible = false)]
        [DefaultValue(0)]
        public decimal? EstimatedAmount { get; set; }

        /// <summary>
        /// Получает или задает дату начала списания оценочного обязательства в стоимости ОС.
        /// </summary>
        [ListView("Дата начала списания оценочного обязательства в стоимости ОС", Visible = false)]
        [DetailView("Дата начала списания оценочного обязательства в стоимости ОС", Visible = false)]
        public DateTime? EstimatedAmountWriteOffStart { get; set; }

        /// <summary>
        /// Получает или задает срок списания оценочного обязательства в стоимости ОС.
        /// </summary>
        [ListView("Срок списания оценочного обязательства в стоимости ОС", Visible = false)]
        [DetailView("Срок списания оценочного обязательства в стоимости ОС", Visible = false)]
        [DefaultValue(0)]
        public int? EstimatedAmountWriteOffTerm { get; set; }

        /// <summary>
        /// Получает номер ЕУСИ объекта имущества.
        /// </summary>
        [DetailView("Номер ЕУСИ", Visible = false, TabName = CaptionHelper.DefaultTabName)]
        [ListView("Номер ЕУСИ", Visible = false)]
        [FullTextSearchProperty]
        public int? EUSINumber { get; set; }

        /// <summary>
        /// Получает или задает пояснение к операции из регистра движения объекта.
        /// </summary>
        [ListView("Пояснение к операции из регистра движения объекта", Visible = false)]
        [DetailView("Пояснение к операции из регистра движения объекта", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Explanation { get; set; }

        /// <summary>
        /// Получает или задает ИД записи ОБУ/ОС во внешней Системе.
        /// </summary>
        /// <remarks>
        /// В зависимости от структуры данных в учётной системе, выбирается уникальный неизменяющийся номер, идентифицирующий запись.
        /// Им может быть, например, для записей об ОС - инвентарный номер, либо внутренний ID учётной системы.
        /// </remarks>
        [ListView(Hidden = true)]
        [DetailView(Name = "Системный номер ОГ", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExternalID { get; set; }

        /// <summary>
        /// Получает или задает фактическое местонахождение объекта.
        /// </summary>
        [DetailView("Фактическое местонахождение объекта", Visible = false)]
        [ListView("Фактическое местонахождение объекта", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FactAddress { get; set; }

        /// <summary>
        /// Получает или задает признак имущества, предназначенного для продажи.
        /// </summary>
        [ListView("Признак имущества, предназначенного для продажи", Visible = false)]
        [DetailView("Признак имущества, предназначенного для продажи", Visible = false)]
        [DefaultValue(false)]
        public bool ForSale { get; set; }

        /// <summary>
        /// Получает или задает номер планера воздушного судна.
        /// </summary>
        [ListView("Номер планера воздушного судна", Visible = false)]
        [DetailView("Номер планера", TabName = TabName10, Order = 137)]
        [PropertyDataType(PropertyDataType.Text)]
        public string GliderNumber { get; set; }

        /// <summary>
        /// Получает или задает площадь ЗУ в кв.м.
        /// </summary>
        [DetailView(Name = "Площадь, кв.м.", TabName = TabName7, Order = 84)]
        public decimal? GroundArea { get; set; }

        /// <summary>
        /// Получает или задает кадастровый/условный номер гос. учета в лесном реестре для ЗУ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView("Кадастровый/условный номер гос. учета в лесном реестре для ЗУ", Visible = false)]
        [DetailView("Кадастровый/условный номер гос. учета в лесном реестре", TabName = TabName7, Order = 85)]
        [PropertyDataType(PropertyDataType.Text)]
        public string GroundCadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает категорию земель.
        /// </summary>
        [ListView("Категория земель", Visible = false)]
        [DetailView("Категория земель", TabName = TabName7, Order = 88)]
        public virtual GroundCategory GroundCategory { get; set; }

        /// <summary>
        /// Получает или задает ИД категории земель.
        /// </summary>
        [SystemProperty]
        public int? GroundCategoryID { get; set; }

        /// <summary>
        /// Получает или задает общую площадь ЗУ в кв.м.
        /// </summary>
        [ListView("Общая площадь земельного/лесного участка, кв.м.", Visible = false)]
        [DetailView("Общая площадь земельного/лесного участка, кв.м.", TabName = TabName7, Order = 87)]
        public decimal? GroundFullArea { get; set; }

        /// <summary>
        /// Получает или задает наименование земельного участка.
        /// </summary>
        [DetailView("Наименование", TabName = TabName7, Order = 83)]
        [PropertyDataType(PropertyDataType.Text)]
        public string GroundName { get; set; }

        /// <summary>
        /// Получает или задает кадастровый номер вышестоящего земельного участка.
        /// </summary>
        /// <remarks>
        /// Кадастровый номер вышестоящего ЗУ текущего кадастрового объекта.
        /// </remarks>      
        [ListView("Кадастровый номер ЗУ", Hidden = true)]
        [DetailView(Name = "Кадастровый номер ЗУ", TabName = TabName4, Order = 59)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string GroundNumber { get; set; }

        /// <summary>
        /// Получает или задает порт/место регистрации речного/морского судна.
        /// </summary>
        [ListView("Порт (место) регистрации речного/морского судна", Hidden = true)]
        [DetailView("Порт (место) регистрации судна", TabName = TabName9, Order = 131)]
        public string Harbor { get; set; }

        /// <summary>
        /// Полуает или задает высоту речного/морского судна.
        /// </summary>
        [ListView("Высота речного/морского судна (мачт и прочих сооружений, имеющих данную характеристику), м", Hidden = true)]
        [DetailView("Высота (мачт и прочих сооружений, имеющих данную характеристику), м", TabName = TabName6, Order = 77)]
        public decimal? Height { get; set; }

        /// <summary>
        /// Получает или задает код ИФНС.
        /// </summary>
        [ListView("Код ИФНС", Visible = false)]
        [DetailView("Код ИФНС", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string IFNS { get; set; }

        /// <summary>
        /// Получает или задает признак консервации объекта.
        /// </summary>      
        [ListView("Признак консервации", Hidden = true)]
        [DetailView("Признак консервации", TabName = TabName3, Order = 46)]
        [DefaultValue(false)]
        public bool InConservation { get; set; }

        /// <summary>
        /// Получает признак включения ОБУ/ОС в график гос. регистрации.
        /// </summary>
        [ListView("Включен в ГГР", Hidden = true)]
        [DetailView("Включен в ГГР", TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Order = 111)]
        public bool InGGR => _InGGR.Evaluate(this);

        /// <summary>
        /// Получает или задает первоначальную стоимость в руб.
        /// </summary>        
        [ListView("Первоначальная стоимость, руб.", Order = 8)]
        [DetailView("Первоначальная стоимость, руб.", TabName = TabName2, Order = 33)]
        [DefaultValue(0)]
        public decimal? InitialCost { get; set; }

        /// <summary>
        /// Получает или задает первоначальную стоимость по МСФО в руб.
        /// </summary>
        [ListView("Первоначальная стоимость объекта по МСФО, руб.", Visible = false)]
        [DetailView(Name = "Первоначальная стоимость объекта по МСФО, руб.", Visible = false)]
        [DefaultValue(0)]
        public decimal? InitialCostMSFO { get; set; }

        /// <summary>
        /// Получает или задает первоначальную стоимость по НУ в руб.
        /// </summary>
        [ListView("Первоначальная стоимость объекта по НУ, руб. ", Visible = false)]
        [DetailView("Первоначальная стоимость объекта по НУ, руб. ", Visible = false)]
        [DefaultValue(0)]
        public decimal? InitialCostNU { get; set; }

        /// <summary>
        /// Получает или задает признак учета в другой системе (актуально для ТС).
        /// </summary>
        [ListView("Учёт в системе ПЛАТОН (ТС)", Hidden = true)]
        [DetailView("Учёт в системе ПЛАТОН", TabName = TabName8, Order = 104)]
        [DefaultValue(false)]
        public bool InOtherSystem { get; set; }

        /// <summary>
        /// Получает или задает дату ввода в эксплуатацию.
        /// </summary>
        [ListView("Дата ввода в эксплуатацию", Visible = false)]
        [DetailView("Дата ввода в эксплуатацию", Visible = false)]
        public DateTime? InServiceDate { get; set; }

        /// <summary>
        /// Получает или задает дату ввода в эксплуатацию МСФО.
        /// </summary>
        [ListView("Дата ввода в эксплуатацию МСФО", Visible = false)]
        [DetailView("Дата ввода в эксплуатацию МСФО", Visible = false)]
        public DateTime? InServiceDateMSFO { get; set; }

        /// <summary>
        /// Получает или задает вид НМА.
        /// </summary>
        [DetailView("Вид НМА", Visible = false)]
        [ListView("Вид НМА", Visible = false)]
        public IntangibleAssetType IntangibleAssetType { get; set; }

        /// <summary>
        /// Получает или задает ИД вида НМА.
        /// </summary>
        [SystemProperty]
        public int? IntangibleAssetTypeID { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>
        /// <remarks>
        /// Например, инв. № SAP в КИС САП РН.
        /// </remarks>
        [ListView("Инвентарный номер", Order = 1)]
        [DetailView(Name = "Инвентарный номер", TabName = CaptionHelper.DefaultTabName, Order = 2)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер 2.
        /// </summary>
        /// <remarks>
        /// Например, инв. № 1C в КИС САП РН.
        /// </remarks>
        [ListView("Инвентарный номер в старой БУС", Order = 2)]
        [DetailView("Инвентарный номер в старой БУС", TabName = CaptionHelper.DefaultTabName, Order = 3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber2 { get; set; }

        /// <summary>
        /// Получает или задает состояние, находится ли ОC\НМА в архиве.
        /// </summary>
        [ListView("На удаление", Visible = false)]
        [DetailView("На удаление", Visible = false)]
        public bool? IsArchived { get; set; }

        /// <summary>
        /// Получает или задает признак отнесения объекта к категории памятников истории и культуры.
        /// </summary>
        [DetailView("Отнесение к категории памятников истории и культуры", Visible = false)]
        [ListView("Отнесение к категории памятников истории и культуры", Visible = false)]
        [DefaultValue(false)]
        public bool IsCultural { get; set; }

        /// <summary>
        /// Получает или задает признак спорности утверждения, что объект является движимым.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Спорный", TabName = CaptionHelper.DefaultTabName, Order = 110)]
        [DefaultValue(false)]
        public bool IsDispute { get; set; }

        /// <summary>
        /// Получает или задает признак энергоэффективного оборудования.
        /// </summary>
        [DetailView("Энергоэффективное оборудование", Visible = false)]
        [ListView(Visible = false)]
        [DefaultValue(false)]
        public bool IsEnergy { get; set; }

        /// <summary>
        /// Получает или задает признак инвестиционного имущества.
        /// </summary>
        [ListView("Признак инвестиционного имущества", Visible = false)]
        [DetailView(Name = "Признак инвестиционного имущества", Visible = false)]
        [DefaultValue(false)]
        public bool IsInvestment { get; set; }

        /// <summary>
        /// Получает или задает признак имущества, созданного по инвестиционной программе.
        /// </summary>
        [DetailView("Имущество, созданное по инвестиционной программе", Visible = false,
        Description = "Имущество, созданное по инвестиционной программе (в соответствии с программой развития регионов)")]
        [ListView("Имущество, созданное по инвестиционной программе", Visible = false)]
        [DefaultValue(false)]
        public bool IsInvestmentProgramm { get; set; }

        /// <summary>
        /// Получает признак, что объект является ННА.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Является ННА", TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Order = -2)]
        public bool IsNonCoreAsset => _IsNonCoreAsset.Evaluate(this);


        /// <summary>
        /// Получает или задает признак недвижимого имущества.
        /// </summary>
        [ListView("Признаки недвижимого имущества (по БУ, по НУ, по ПУ)", Order = 15)]
        [DetailView("Признаки недвижимого имущества (по БУ, по НУ, по ПУ)", TabName = TabName1, Order = 14)]
        [DefaultValue(false)]
        public bool IsRealEstate { get; set; }

        /// <summary>
        /// Получает или задает признак движимого/недвижимого имущества. Систменое поле.
        /// </summary>
        /// <remarks>
        /// Технич. атрибут, используется при импорте данных ОБУ.
        /// </remarks>
        [SystemProperty]
        public bool? IsRealEstateImpl { get; set; }

        /// <summary>
        /// Получает или задает признак объекта социально-культурного или бытового назначения.
        /// </summary>
        [DetailView("Признак объекта социально-культурного или бытового назначения", Visible = false)]
        [ListView("Признак объекта социально-культурного или бытового назначения", Visible = false)]
        [DefaultValue(false)]
        public bool IsSocial { get; set; }


        /// <summary>
        /// Получает или задает код показателя ИКСО (амортизация).
        /// </summary>
        [ListView("Код показателя ИКСО (амортизация)", Visible = false)]
        [DetailView("Код показателя ИКСО (амортизация)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string IXODepreciation { get; set; }

        /// <summary>
        /// Получает или задает код показателя ИКСО первоначальная стоимость.
        /// </summary>
        [ListView("Код показателя ИКСО ПСт", Visible = false)]
        [DetailView("Код показателя ИКСО ПСт", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string IXOPSt { get; set; }

        /// <summary>
        /// Получает или задает назначение ЗУ.
        /// </summary>
        [DetailView("Назначение ЗУ", Visible = false)]
        [ListView("Назначение ЗУ", Visible = false)]
        public LandPurpose LandPurpose { get; set; }

        /// <summary>
        /// Получает или задает ИД назначения ЗУ.
        /// </summary>
        [SystemProperty]
        public int? LandPurposeID { get; set; }

        /// <summary>
        /// Получает или задает тип ЗУ.
        /// </summary>
        [ListView("Тип ЗУ", Visible = false)]
        [DetailView("Тип ЗУ", Visible = false)]
        public LandType LandType { get; set; }

        /// <summary>
        /// Получает или задает ИД типа ЗУ.
        /// </summary>
        [SystemProperty]
        public int? LandTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип прокладки линейного сооружения.
        /// </summary>
        [ListView("Тип прокладки линейного сооружения", Hidden = true)]
        [DetailView("Тип прокладки линейного сооружения", TabName = TabName6, Order = 74)]
        public LayingType LayingType { get; set; }

        /// <summary>
        /// Получает или задает ИД типа прокладки лин. сооружения.
        /// </summary>
        public int? LayingTypeID { get; set; }

        /// <summary>
        /// Получает или задает стоимость выбытия в руб.
        /// </summary>
        [ListView("Стоимость выбытия, руб.", Hidden = true)]
        [DetailView("Стоимость выбытия, руб.", TabName = TabName2, Order = 38)]
        public decimal? LeavingCost { get; set; }

        /// <summary>
        /// Получает или задает дату списания объекта.
        /// </summary>     
        [ListView("Дата списания", Hidden = true)]
        [DetailView("Дата списания", TabName = CaptionHelper.DefaultTabName, Order = 18)]
        public DateTime? LeavingDate { get; set; }

        /// <summary>
        /// Получает или задает причину выбытия.
        /// </summary>
        [ListView("Причина выбытия", Hidden = true)]
        [DetailView("Причина выбытия", TabName = CaptionHelper.DefaultTabName, Order = 19)]
        public LeavingReason LeavingReason { get; set; }

        /// <summary>
        /// Получает или задает ИД причины выбытия.
        /// </summary>
        [SystemProperty]
        public int? LeavingReasonID { get; set; }

        /// <summary>
        /// Получает или задает длину речного/морского судна.
        /// </summary>
        [ListView("Длина речного/морского судна", Visible = false)]
        [DetailView("Длина судна", TabName = TabName9, Order = 118)]
        [DefaultValue(0)]
        public decimal? Length { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения длины речного/морского судна.
        /// </summary>
        [ListView("Ед. измерения длины речного/морского судна", Visible = false)]
        [DetailView("Ед. измерения длины судна", TabName = TabName9, Order = 119)]
        public SibMeasure LengthUnit { get; set; }

        /// <summary>
        /// Получает или задает ИД ед. измерения длины речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? LengthUnitID { get; set; }

        /// <summary>
        /// Получает или задает арендатора/пользователя по договору.
        /// </summary>
        [DetailView("Арендатор (Лизингополучатель)/Пользователь по договору", Visible = false)]
        [ListView("Арендатор (Лизингополучатель)/Пользователь по договору", Visible = false)]
        public CorpProp.Entities.Subject.Subject LessorSubject { get; set; }

        /// <summary>
        /// Получает или задает ИД арендатора/пользователя.
        /// </summary>
        [SystemProperty]
        public int? LessorSubjectID { get; set; }

        /// <summary>
        /// Получает или задает лицензионный участок.
        /// </summary>
        [ListView("Лицензионный участок", Visible = false)]
        [DetailView("Лицензионный участок", Visible = false)]
        public LicenseArea LicenseArea { get; set; }

        /// <summary>
        /// Получает или задает ИД лицензионного участка.
        /// </summary>
        [SystemProperty]
        public int? LicenseAreaID { get; set; }

        /// <summary>
        /// Получает или задает число главных машин речного/морского судна.
        /// </summary>
        [DetailView("Число главных машин", TabName = TabName9, Order = 115)]
        [DefaultValue(0)]
        public int? MainEngineCount { get; set; }

        /// <summary>
        /// Получает или задает общую мощность главных машин речного/морского судна.
        /// </summary>
        [DetailView("Общая мощность главных машин", TabName = TabName9, Order = 116)]
        [DefaultValue(0)]
        public decimal? MainEnginePower { get; set; }

        /// <summary>
        /// Получает или задает тип главной машины речного/морского судна.
        /// </summary>
        [DetailView("Тип главной машины", TabName = TabName9, Order = 114)]
        [PropertyDataType(PropertyDataType.Text)]
        public string MainEngineType { get; set; }

        /// <summary>
        /// Получает или задает владельца/собственника ОБУ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView("Собственник", Order = 13)]
        [DetailView("Собственник", TabName = CaptionHelper.DefaultTabName, Order = 5)]
        public Society MainOwner { get; set; }

        /// <summary>
        /// Получает или задает идентификатор владельца/собственника ОБУ.
        /// </summary>
        [SystemProperty]
        public int? MainOwnerID { get; set; }

        /// <summary>
        /// Получает или задает наименование изготовителя воздушног осудна.
        /// </summary>
        [DetailView("Наименование изготовителя", TabName = TabName10, Order = 141)]
        [PropertyDataType(PropertyDataType.Text)]
        public string MakerName { get; set; }

        /// <summary>
        /// Получает или задает рыночную стоимость объекта.
        /// </summary>
        /// <remarks>
        /// Последняя актуальная рыночная стоимость объекта оценки.	ОИ.Оценка.Объект оценки.
        /// </remarks>       
        [DetailView("Рыночная стоимость, руб.", TabName = TabName2, Order = 40)]
        public decimal? MarketCost { get; set; }

        /// <summary>
        /// Получает или задает дату рыночной оценки.
        /// </summary>
        /// <remarks>
        /// Последняя актуальная дата оценки (ближайшей к текущей дате)	ОИ.Оценка.
        /// </remarks>       
        [DetailView("Дата рыночной оценки", TabName = TabName2, Order = 41)]
        public DateTime? MarketDate { get; set; }

        /// <summary>
        /// Получает или задает материал конструкций объекта. (Здания, помащения, сооружения).
        /// </summary>
        [ListView("Материал конструкций", Hidden = true)]
        [DetailView("Материал конструкций", TabName = TabName6, Order = 78)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Material { get; set; }


        /// <summary>
        /// Получает или задает марку транспортного средства.
        /// </summary>
        [ListView("Марка ТС", Hidden = true)]
        [DetailView("Марка ТС", TabName = TabName8, Order = 97)]
        [ForeignKey("VehicleModelID")]
        public virtual VehicleModel Model { get; set; }

        /// <summary>
        /// Получает или задает модель ТС.
        /// </summary>
        [ListView("Модель ТС", Hidden = true)]
        [DetailView("Модель ТС", TabName = TabName8, Order = 98)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Model2 { get; set; }

        /// <summary>
        /// Получает или задает материально-ответственное лицо.
        /// </summary>
        [DetailView("МОЛ", TabName = CaptionHelper.DefaultTabName, Order = 15)]
        [PropertyDataType(PropertyDataType.Text)]
        public string MOL { get; set; }

        /// <summary>
        /// Получает или задает максимальную высоту с надстройками (от осадки порожнем) речного/морского судна.
        /// </summary>
        [DetailView("Наибольшая высота с надстройками (от осадки порожнем) судна", TabName = TabName9, Order = 126)]
        public decimal? MostHeight { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения макс высоты с надстройками (от осадки порожнем) речного/морского судна.
        /// </summary>
        [DetailView("Ед. измернения", TabName = TabName9, Order = 127)]
        public SibMeasure MostHeightUnit { get; set; }

        /// <summary>
        /// Получает или задает ИЛД ед. измерения макс высоты с надстройками речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? MostHeightUnitID { get; set; }

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [ListView(Order = 4)]
        [DetailView(Name = "Наименование",
        TabName = CaptionHelper.DefaultTabName, Order = 9)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает наименование объекта (в соответствии с документами).
        /// </summary>
        [DetailView("Наименование объекта (в соответствии с документами)", Visible = false)]
        [ListView("Наименование объекта (в соответствии с документами)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameByDoc { get; set; }

        /// <summary>
        /// Получает или задает наименование ЕУСИ.
        /// </summary>
        [DetailView("Наименование ЕУСИ", Visible = false)]
        [ListView("Наименование ЕУСИ", Visible = false)]
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameEUSI { get; set; }

        /// <summary>
        /// Получает или задает OKATO.
        /// </summary>
        [ListView("ОКАТО", Hidden = true)]
        [DetailView("ОКАТО", TabName = TabName1, Order = 29)]
        public OKATO OKATO { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКАТО.
        /// </summary>
        [SystemProperty]
        public int? OKATOID { get; set; }

        /// <summary>
        /// Получает или задает регион по коду ОКАТО.
        /// </summary>
        [ListView("Регион по ОКАТО", Hidden = true)]
        [DetailView("Регион по ОКАТО", TabName = TabName1, Order = 30)]
        [ForeignKey("OKATORegionID")]
        public SibRegion OKATORegion { get; set; }

        /// <summary>
        /// Получает или задает ИД региона по ОКАТО.
        /// </summary>
        [SystemProperty]
        public int? OKATORegionID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-2014.
        /// </summary>
        [DetailView("ОКОФ-2014", Visible = false, TabName = TabName1, Order = 22)]
        public virtual OKOF2014 OKOF2014 { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-2014.
        /// </summary>
        [SystemProperty]
        public int? OKOF2014ID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-94.
        /// </summary>
        [DetailView(Name = "ОКОФ-94", TabName = TabName1, Visible = false, Order = 19)]
        public OKOF94 OKOF94 { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-94.
        /// </summary>
        [SystemProperty]
        public int? OKOF94ID { get; set; }

        /// <summary>
        /// Получает или задает код ОКОФ.
        /// </summary>
        [ListView("Код ОКОФ", Hidden = true)]
        [DetailView("Код ОКОФ", TabName = TabName1, Order = 21, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKOFCode { get; set; }

        /// <summary>
        /// Получает или задает код ОКОФ 2.
        /// </summary>
        [ListView("Код ОКОФ 2", Hidden = true)]
        [DetailView("Код ОКОФ 2", TabName = TabName1, Order = 24, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKOFCode2 { get; set; }

        /// <summary>
        /// Получает или задает класс ОКОФ.
        /// </summary>
        [ListView("Класс ОКОФ", Hidden = true)]
        [DetailView("Класс ОКОФ", TabName = TabName1, Order = 20, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKOFName { get; set; }

        /// <summary>
        /// Получает или задает клас ОКОФ 2.
        /// </summary>
        [ListView("Класс ОКОФ 2", Hidden = true)]
        [DetailView("Класс ОКОФ 2", TabName = TabName1, Order = 23, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKOFName2 { get; set; }

        /// <summary>
        /// Получает или задает OKTMO.
        /// </summary>
        [DetailView("ОКТМО", Visible = false, TabName = TabName1, Order = 25)]
        public OKTMO OKTMO { get; set; }

        /// <summary>
        /// Получает или задает код ОКТМО.
        /// </summary>
        [ListView("Код ОКТМО", Hidden = true)]
        [DetailView("Код ОКТМО", TabName = TabName1, Order = 26, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKTMOCode { get; set; }

        /// <summary>
        /// Получает или задает ИД OKTMO.
        /// </summary>
        [SystemProperty]
        public int? OKTMOID { get; set; }

        /// <summary>
        /// Получает или задает наименование ОКТМО.
        /// </summary>
        [ListView("ОКТМО", Hidden = true)]
        [DetailView("ОКТМО", TabName = TabName1, Order = 27, Description = "ОКТМО (Наименование)", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKTMOName { get; set; }

        /// <summary>
        /// Получает или задает регион по ОКТМО.
        /// </summary>
        [ListView("Регион по ОКТМО", Order = 7)]
        [DetailView("Регион по ОКТМО", TabName = TabName1, Order = 28)]
        [ForeignKey("OKTMORegionID")]
        public SibRegion OKTMORegion { get; set; }

        /// <summary>
        /// Получает или задает ИД региона по ОКТМО.
        /// </summary>
        [SystemProperty]
        public int? OKTMORegionID { get; set; }

        /// <summary>
        /// Получает или задает прежний порт/место регистрации речного/морского судна.
        /// </summary>
        [DetailView(Name = "Прежний порт (место) регистрации судна", TabName = TabName9, Order = 132)]
        public string OldHarbor { get; set; }

        /// <summary>
        /// Получает или задает прежнее наименование речного/морского судна.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Прежнее название судна", TabName = TabName9, Order = 107)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OldName { get; set; }

        /// <summary>
        /// Получает признак "За балансом"
        /// </summary>
        [ListView("За балансом", Hidden = false, Visible = false)]
        [DetailView("За балансом", TabName = TabName1, ReadOnly = true, Visible = false, Order = 222)]
        [DefaultValue(false)]
        public bool OutOfBalance { get; set; }

        /// <summary>
        /// Получает или задает балансодержателя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView("Балансодержатель", Order = 13)]
        [DetailView("Балансодержатель", TabName = CaptionHelper.DefaultTabName, Order = 4)]
        public Society Owner { get; set; }

        /// <summary>
        /// Получает или задает идентификатор балансодержателя.
        /// </summary>
        [SystemProperty]
        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает или заадет форму собственности.
        /// </summary>
        [ListView("Форма собственности", Visible = false)]
        [DetailView("Форма собственности", Visible = false)]
        public OwnershipType OwnershipType { get; set; }

        /// <summary>
        /// Получает или задает ИД формы собственности.
        /// </summary>
        [SystemProperty]
        public int? OwnershipTypeID { get; set; }

        /// <summary>
        /// Получает или задает разрешенное использование ЗУ (по документам).
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [ListView("Разрешенное использование", Visible = false)]
        [DetailView("Разрешенное использование", Visible = false)]
        public string PermittedByDoc { get; set; }

        /// <summary>
        /// Получает или задает вид разрешенного использования.
        /// </summary>
        [DetailView("Вид разрешенного использования", Visible = false)]
        [ListView("Вид разрешенного использования", Visible = false)]
        public PermittedUseKind PermittedUseKind { get; set; }

        /// <summary>
        /// Получает или задает ИД вида разрешенного использования.
        /// </summary>
        [SystemProperty]
        public int? PermittedUseKindID { get; set; }

        /// <summary>
        /// Получает или задает длину трубопровода.
        /// </summary>
        [DetailView("Длина трубопровода, м.", Visible = false)]
        [ListView("Длина трубопровода, м.", Visible = false)]
        public decimal? PipelineLength { get; set; }

        /// <summary>
        /// Получает или задает позицию консолидации.
        /// </summary>
        [ListView("Позиция консолидации", Visible = false)]
        [DetailView("Позиция консолидации", Visible = false)]
        public PositionConsolidation PositionConsolidation { get; set; }

        /// <summary>
        /// Получает или задает Ид позиции консолидации.
        /// </summary>
        [SystemProperty]
        public int? PositionConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает Мощность ТС.
        /// </summary>
        [ListView("Мощность ТС", Hidden = true)]
        [DetailView("Мощность ТС", TabName = TabName8, Order = 93)]
        public decimal? Power { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения мощности тС.
        /// </summary>
        [DetailView("Ед. измерения мощности", TabName = TabName9, Order = 117)]
        public virtual SibMeasure PowerUnit { get; set; }

        /// <summary>
        /// Получает или задает ИД ед. измерения мощности ТС.
        /// </summary>
        [SystemProperty]
        public int? PowerUnitID { get; set; }

        /// <summary>
        /// Получает или задает дату первичного документа.
        /// </summary>       
        [DetailView("Дата первичного документа", Visible = false)]
        public DateTime? PrimaryDocDate { get; set; }

        /// <summary>
        /// Получает или задает № первичного документа.
        /// </summary> 
        [DetailView("№ первичного документа", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PrimaryDocNumber { get; set; }

        /// <summary>
        /// Получает или задает дату изготовления.
        /// </summary>
        [ListView("Дата изготовления", Hidden = true)]
        [DetailView("Дата изготовления", TabName = TabName10, Order = 140)]
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// Получает или задает наименование ИК.
        /// </summary>
        /// <remarks>
        /// Задается при импорте данных БУС и не вычисляется.
        /// </remarks>
        [ListView("Наименование имущественного комплекса", Visible = false)]
        [DetailView("Наименование имущественного комплекса", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String PropertyComplexName { get; set; }

        /// <summary>
        /// Получает или задает арендодателя/собственника по договору.
        /// </summary>
        [DetailView("Арендодатель (Лизингодатель)/Собственник по договору", Visible = false)]
        [ListView("Арендодатель (Лизингодатель)/Собственник по договору", Visible = false)]
        public CorpProp.Entities.Subject.Subject ProprietorSubject { get; set; }

        /// <summary>
        /// Получает или задает ИД арендодателя/собственника по договору.
        /// </summary>
        [SystemProperty]
        public int? ProprietorSubjectID { get; set; }

        /// <summary>
        /// Получает или задает номера вспомогательных силовых установок воздушного судна.
        /// </summary>
        [DetailView("Номера вспомогательных силовых установок", TabName = TabName10, Order = 139)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PropulsionNumber { get; set; }

        /// <summary>
        /// Получает или задает причину/способ поступления ОС.
        /// </summary>
        [ListView("Причина поступления", Hidden = true)]
        [DetailView("Причина поступления", TabName = CaptionHelper.DefaultTabName, Order = 17)]
        public ReceiptReason ReceiptReason { get; set; }

        /// <summary>
        /// Получает или задает ИД причины/способа поступления ОС.
        /// </summary>
        [SystemProperty]
        public int? ReceiptReasonID { get; set; }

        /// <summary>
        /// Получает или задает выкупную стоимость объекта аренды в валюте договора.
        /// </summary>
        [ListView("Выкупная стоимость объекта аренды (в валюте договора)", Visible = false)]
        [DetailView("Выкупная стоимость объекта аренды (в валюте договора)", Visible = false)]
        [DefaultValue(0)]
        public decimal? RedemptionCost { get; set; }

        /// <summary>
        /// Получает или задает предполагаемую дату выкупа объекта аренды.
        /// </summary>
        [ListView("Предполагаемая дата выкупа объекта аренды", Visible = false)]
        [DetailView("Предполагаемая дата выкупа объекта аренды", Visible = false)]
        public DateTime? RedemptionDate { get; set; }

        /// <summary>
        /// Получает или задает субъект РФ/регион.
        /// </summary>       
        [ListView("Субъект РФ", Hidden = true)]
        [DetailView("Субъект РФ", TabName = TabName4, Order = 54)]
        public SibRegion Region { get; set; }

        /// <summary>
        /// Получает или задает ИД субъекта РФ/региона.
        /// </summary>
        public int? RegionID { get; set; }

        /// <summary>
        /// Получает или задает номер записи гос. регистрации.
        /// </summary>       
        [ListView("Номер записи гос. регистрации", Order = 11)]
        [DetailView("Номер записи гос. регистрации", TabName = TabName5, Order = 64)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegNumber { get; set; }

        /// <summary>
        /// Получает или задает дату договора аренды.
        /// </summary>
        [ListView("Дата договора аренды", Visible = false)]
        [DetailView("Дата договора аренды", Visible = false)]
        public DateTime? RentContractDate { get; set; }

        /// <summary>
        /// Получает или задает номер договора аренды.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Visible = false)]
        [DetailView("Номер договора аренды", Visible = false)]
        public string RentContractNumber { get; set; }

        /// <summary>
        /// Получает или задает номер договора аренды СЦВД.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Visible = false)]
        [DetailView("Номер договора аренды СЦВД", Visible = false)]
        public string RentContractNumberSZVD { get; set; }

        /// <summary>
        /// Получает или задает предполагаемый срок аренды.
        /// </summary>
        /// <remarks>
        /// Предполагаемый срок аренды с учетом дальнейшей пролонгации и бизнес-плана (дата окончания аренды).
        /// </remarks>
        [ListView("Предполагаемый срок аренды", Visible = false)]
        [DetailView("Предполагаемый срок аренды", Visible = false,
         Description = "Предполагаемый срок аренды с учетом дальнейшей пролонгации и бизнес-плана (дата окончания аренды)")]
        public DateTime? RentTerm { get; set; }

        /// <summary>
        /// Получает или задает тип аренды МСФО.
        /// </summary>
        [DetailView("Тип аренды МСФО", Visible = false)]
        [ListView("Тип аренды МСФО", Visible = false)]
        public RentTypeMSFO RentTypeMSFO { get; set; }

        /// <summary>
        /// Получает или задает ИД типа аренды МСФО.
        /// </summary>
        [SystemProperty]
        public int? RentTypeMSFOID { get; set; }

        /// <summary>
        /// Получает или задает тип аренды РСБУ.
        /// </summary>
        [DetailView("Тип аренды РСБУ", Visible = false)]
        [ListView("Тип аренды РСБУ", Visible = false)]
        public RentTypeRSBU RentTypeRSBU { get; set; }

        /// <summary>
        /// Получает или задает ИД типа аренды РСБУ.
        /// </summary>
        [SystemProperty]
        public int? RentTypeRSBUID { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость в руб.
        /// </summary>       
        [ListView("Остаточная стоимость, руб.", Order = 9)]
        [DetailView("Остаточная стоимость, руб.", TabName = TabName2, Order = 34)]
        [DefaultValue(0)]
        public decimal? ResidualCost { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость оценочного обязательства.
        /// </summary>
        [ListView("Остаточная стоимость оценочного обязательства, руб.", Visible = false)]
        [DetailView("ООстаточная стоимость оценочного обязательства, руб.", Visible = false)]
        [DefaultValue(0)]
        public decimal? ResidualCostEstimate { get; set; }

        /// <summary>
        /// Получает или задает оставточную стоимость объекта по МСФО в руб.
        /// </summary>
        [ListView("Остаточная стоимость объекта по МСФО, руб.", Visible = false)]
        [DetailView("Остаточная стоимость объекта по МСФО, руб.", Visible = false)]
        [DefaultValue(0)]
        public decimal? ResidualCostMSFO { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость объекта по НУ в руб.
        /// </summary>
        [ListView("Остаточная стоимость объекта по НУ, руб.", Visible = false)]
        [DetailView("Остаточная стоимость объекта по НУ, руб.", Visible = false)]
        [DefaultValue(0)]
        public decimal? ResidualCostNU { get; set; }

        /// <summary>
        /// Получает или задает адрес по данным  кадастрового учета.
        /// </summary>       
        [DetailView("Адрес по данным кадастрового учета", TabName = TabName5, Order = 62)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightAddress { get; set; }

        /// <summary>
        /// Получает или задает вид права.
        /// </summary>        
        [ListView("Вид права", Hidden = true)]
        [DetailView("Вид права", TabName = TabName5, Order = 63)]
        public RightKind RightKind { get; set; }

        /// <summary>
        /// Получает или задает ИД вида права.
        /// </summary>
        [SystemProperty]
        public int? RightKindID { get; set; }

        /// <summary>
        /// Получает или задает дату записи гос. регистрации.
        /// </summary>        
        [ListView("Дата записи гос. регистрации", Order = 12)]
        [DetailView("Дата записи гос. регистрации", TabName = TabName5, Order = 65)]
        public DateTime? RightRegDate { get; set; }

        /// <summary>
        /// Получает или задает дату гос. регистрации прекращения права.
        /// </summary>        
        [ListView("Дата гос. регистрации прекращения права", Visible = false)]
        [DetailView("Дата гос. регистрации прекращения права", Visible = false)]
        public DateTime? RightRegEndDate { get; set; }

        /// <summary>
        /// Получает или задает код региона по данным кадастрового учета.
        /// </summary>
        /// <remarks>Код кадастрового округа (первые 2 цифры кадастрового номера)</remarks>
        [ListView("Регион по данным кадастрового учета", Hidden = true)]
        [DetailView("Регион по данным кадастрового учета", TabName = TabName5, Order = 61)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightRegion { get; set; }

        /// <summary>
        /// Получает или задает номер счета РСБУ.
        /// </summary>
        [ListView("Счет РСБУ", Hidden = true)]
        [DetailView("Счет РСБУ", TabName = CaptionHelper.DefaultTabName, Order = 7)]
        public RSBU RSBUAccountNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД номера счета РСБУ.
        /// </summary>
        [SystemProperty]
        public int? RSBUAccountNumberID { get; set; }

        /// <summary>
        /// Получает или задает пассажировместимость речного/морского судна.
        /// </summary>
        [DetailView("Пассажировместимость судна", TabName = TabName9, Order = 130)]
        public int? SeatingCapacity { get; set; }

        /// <summary>
        /// Получает или задает серийный (идентификационный, заводской) номер воздушного судна.
        /// </summary>
        [DetailView("Серийный (идентификационный, заводской) номер воздушного судна", TabName = TabName10, Order = 135)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SerialName { get; set; }

        /// <summary>
        /// Получает или задает серийный/заводской/ВИН номер ТС.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Серийный номер/Заводской номер/ВИН", TabName = TabName8, Order = 94)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Получает или задает знаменатель доли в праве.
        /// </summary>
        [ListView("Доля в праве (знаменатель доли)", Visible = false)]
        [DetailView("Доля в праве (знаменатель доли)", Visible = false)]
        [DefaultValue(1)]
        public int? ShareRightDenominator { get; set; }

        /// <summary>
        /// Получает или задает числитель доли в праве.
        /// </summary>
        [ListView("Доля в праве (числитель доли)", Visible = false)]
        [DetailView("Доля в праве (числитель доли)", Visible = false)]
        [DefaultValue(1)]
        public int? ShareRightNumerator { get; set; }

        /// <summary>
        /// Получает или задает материал корпуса речного/морского судна.
        /// </summary>
        [DetailView("Материал корпуса судна", TabName = TabName9, Order = 113)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShellMaterial { get; set; }

        /// <summary>
        /// Получает или задает назначение речного/морского судна.
        /// </summary>
        [ListView("Назначение судна", Hidden = true)]
        [DetailView("Назначение судна", TabName = TabName9, Order = 109)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShipAppointment { get; set; }

        /// <summary>
        /// Получает или задает класс речного-морского судна.
        /// </summary>
        [ListView("Класс судна", Hidden = true)]
        [DetailView("Класс судна", TabName = TabName9, Order = 110)]
        public ShipClass ShipClass { get; set; }

        /// <summary>
        /// Получает или задает ИД класса речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? ShipClassID { get; set; }

        /// <summary>
        /// Получает или задает построечный номер речного/морского судна.
        /// </summary>
        [ListView("Название (построечный номер) судна", Hidden = true)]
        [DetailView("Название (построечный номер) судна", TabName = TabName9, Order = 105)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShipName { get; set; }

        /// <summary>
        /// Получает или задает дату включения в российский международный реестр речных/морских судов.
        /// </summary>
        [ListView("Дата включения в Российский международный реестр судов", Visible = false)]
        [DetailView("Дата включения в Российский международный реестр судов", Visible = false)]
        public DateTime? ShipRegDate { get; set; }

        /// <summary>
        /// Получает или задает номер в речном/морском реестре.
        /// </summary>
        [ListView("Номер в речном/морском реестре", Hidden = true)]
        [DetailView("Номер в речном/морском реестре", TabName = TabName9, Order = 106)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShipRegNumber { get; set; }

        /// <summary>
        /// Получает или задает тип речного/морского судна.
        /// </summary>
        [ListView("Тип судна", Hidden = true)]
        [DetailView("Тип судна", TabName = TabName9, Order = 108)]
        public ShipType ShipType { get; set; }

        /// <summary>
        /// Получает или задает ИД типа речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? ShipTypeID { get; set; }

        /// <summary>
        /// Получает или задает город.
        /// </summary>
        [DetailView("Город", Visible = false)]
        [ListView("Город", Visible = false)]
        public SibCityNSI SibCityNSI { get; set; }

        /// <summary>
        /// Получает или задает ИД города.
        /// </summary>
        [SystemProperty]
        public int? SibCityNSIID { get; set; }

        /// <summary>
        /// Получает или задает страну.
        /// </summary>
        [DetailView("Страна", Visible = false)]
        [ListView("Страна", Visible = false)]
        public SibCountry SibCountry { get; set; }

        /// <summary>
        /// Получает или задает ИД страны.
        /// </summary>
        [SystemProperty]
        public int? SibCountryID { get; set; }

        /// <summary>
        /// Получает или задает федеральный округ.
        /// </summary>
        [DetailView("Федеральный округ", Visible = false)]
        [ListView("Федеральный округ", Visible = false)]
        public SibFederalDistrict SibFederalDistrict { get; set; }

        /// <summary>
        /// Получает или задает ИД федерального округа.
        /// </summary>
        [SystemProperty]
        public int? SibFederalDistrictID { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения мощности ТС.
        /// </summary>
        [ListView("Единица измерения мощности ТС", Hidden = true)]
        [DetailView("Единица измерения мощности ТС", TabName = TabName8, Order = 92)]
        public SibMeasure SibMeasure { get; set; }

        /// <summary>
        /// Получает или задает ИД ед. измерения мощности ТС.
        /// </summary>
        public int? SibMeasureID { get; set; }

        /// <summary>
        /// Получает или задает номерной знак ТС.
        /// </summary>
        [ListView("Номерной знак транспортного средства", Hidden = true)]
        [DetailView("Номерной знак транспортного средства", TabName = TabName8, Order = 101)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SignNumber { get; set; }

        /// <summary>
        /// Получает или задает код СПП из БИП.
        /// </summary>
        [DetailView("Код СПП из БИП", Visible = false)]
        [ListView("Код СПП из БИП", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SPPCode { get; set; }

        /// <summary>
        /// Получает или задает СПП элемент.
        /// </summary>
        [DetailView("СПП элемент", Visible = false)]
        [ListView("СПП элемент", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SPPItem { get; set; }

        /// <summary>
        /// Получает или задает ссылку на ГГР, вводимых в текущем году.
        /// </summary>
        [ListView("Включен в ГГР вводимых в текущем году", Hidden = true)]
        [DetailView("Включен в ГГР вводимых в текущем году", TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Order = 113)]
        public ScheduleStateRegistration SSR { get; set; }

        /// <summary>
        /// Получает или задает ИД ГГР, вводимых в текущем году.
        /// </summary>
        [SystemProperty]
        public int? SSRID { get; set; }

        /// <summary>
        /// Получает или задает ссылку на ГГР прекращения права.
        /// </summary>
        [ListView("Включен в ГГР прекращения права", Hidden = true)]
        [DetailView("Включен в ГГР прекращения права", TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Order = 114)]
        public ScheduleStateTerminate SSRTerminate { get; set; }

        /// <summary>
        /// Получает или задает ИД ГГР, прекращения права.
        /// </summary>
        [SystemProperty]
        public int? SSRTerminateID { get; set; }

        /// <summary>
        /// Получает или задает дату начала записи ОБУ во внешней системе.
        /// </summary>
        [ListView("Дата начала", Hidden = true)]
        [DetailView("Дата начала", TabName = TabName3, Order = 50)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Получает или задает дату начала использования НЗС.
        /// </summary>
        [ListView("Дата начала использования (НКС)", Visible = false)]
        [DetailView("Дата начала использования (НКС)", Visible = false)]
        public DateTime? StartDateUse { get; set; }

        /// <summary>
        /// Получает или задает состояние ОБУ по данным внешней системы.
        /// </summary>
        [ListView("Состояние", Hidden = true)]
        [DetailView("Состояние", TabName = TabName6, Order = 79)]
        [PropertyDataType(PropertyDataType.Text)]
        public string State { get; set; }

        /// <summary>
        /// Получает или задает дату изменения статуса РСБУ.
        /// </summary>
        /// <remarks>
        /// Дата изменения статуса ОС при создании ОС и передаче данных в БУС.
        /// </remarks>
        [ListView(Visible = false)]
        [DetailView(Name = "Дата изменения статуса", Visible = false)]
        public DateTime? StateChangeDate { get; set; }

        /// <summary>
        /// Получает или задает состояние объекта МСФО.
        /// </summary>
        [ListView("Состояние объекта МСФО", Visible = false)]
        [DetailView("Состояние объекта МСФО", Visible = false)]
        public StateObjectMSFO StateObjectMSFO { get; set; }

        /// <summary>
        /// Получает или задает ИД состояния объекта МСФО.
        /// </summary>
        [SystemProperty]
        public int? StateObjectMSFOID { get; set; }

        /// <summary>
        /// Получает или задает состояние объекта по РСБУ.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Состояние объекта РСБУ", Visible = false)]
        public virtual StateObjectRSBU StateObjectRSBU { get; set; }

        /// <summary>
        /// Получает или задает ИД состояния объекта по РСБУ.
        /// </summary>
        [SystemProperty]
        public int? StateObjectRSBUID { get; set; }

        /// <summary>
        /// Получает или задает место хранения.
        /// </summary>
        [ListView("Место хранения", Hidden = true)]
        [DetailView("Место хранения", TabName = CaptionHelper.DefaultTabName, Order = 20)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Storage { get; set; }

        /// <summary>
        /// Получает или задает код СДП контрагента по договору.
        /// </summary>
        [ListView("Контрагент по договору (Код СДП)", Hidden = true)]
        [DetailView("Контрагент по договору (Код СДП)", TabName = TabName3, Order = 55)]
        public string SubjectCode { get; set; }

        /// <summary>
        /// Получает или задает наименование контрагента по договору.
        /// </summary>       
        [ListView("Контрагент по договору", Hidden = true)]
        [DetailView("Контрагент по договору", TabName = TabName3, Order = 53)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubjectName { get; set; }

        /// <summary>
        /// Получает или задает субномер.
        /// </summary>
        [ListView("Субномер", Visible = false)]
        [DetailView("Субномер", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubNumber { get; set; }

        /// <summary>
        /// Получает или задает признак начисления налога с кадастровой стоимости.
        /// </summary>
        [DetailView("Признак начисления налога с кадастровой стоимости", TabName = TabName6, Order = 81)]
        [DefaultValue(false)]
        public bool Tax { get; set; }

        /// <summary>
        /// Получает или задает выбор базы налогооблажения.
        /// </summary>
        [ListView("Выбор базы налогообложения", Visible = false)]
        [DetailView("Выбор базы налогообложения", Visible = false)]
        public virtual TaxBase TaxBase { get; set; }

        /// <summary>
        /// Получает или задает выбор базы налогооблажения (ЕУСИ).
        /// </summary>
        [ListView("Выбор базы налогообложения (ЕУСИ)", Visible = false)]
        [DetailView("Выбор базы налогообложения (ЕУСИ)", Visible = false)]
        [ForeignKey("TaxBaseEstateID")]
        public TaxBase TaxBaseEstate { get; set; }

        /// <summary>
        /// Получает или задает ИД выбора базы налогооблажения (ЕУСИ).
        /// </summary>
        [SystemProperty]
        public int? TaxBaseEstateID { get; set; }

        /// <summary>
        /// Получает или задает ИД базы налогооблажения.
        /// </summary>
        [SystemProperty]
        public int? TaxBaseID { get; set; }

        /// <summary>
        /// Получает или задает дату включения в перечень объектов, учитываемых по кадастровой стоимости.
        /// </summary>
        [ListView("Дата включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false)]
        [DetailView("Дата включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false, Description = "Дата включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения")]
        public DateTime? TaxCadastralIncludeDate { get; set; }

        /// <summary>
        /// Получает или задает номер док-та включения в перечень объектов, учитываемых по кадатсрвоой стоимости.
        /// </summary>
        [ListView("Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false)]
        [DetailView("Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false, Description = "Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxCadastralIncludeDoc { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы. Налог на имущество.
        /// </summary>
        [ListView("Код налоговой льготы", Visible = false)]
        [DetailView("Код налоговой льготы", Visible = false)]
        public virtual TaxExemption TaxExemption { get; set; }


        /// <summary>
        /// Получает или задает код налоговой льготы. Налог на землю.
        /// </summary>
        [ListView("Код налоговой льготы Земля", Visible = false)]
        [DetailView("Код налоговой льготы Земля", Visible = false)]
        public TaxExemptionLand TaxExemptionLand { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы. Налог на транспорт.
        /// </summary>
        [ListView("Код налоговой льготы Транспорт", Visible = false)]
        [DetailView("Код налоговой льготы Транспорт", Visible = false)]
        public TaxExemptionTS TaxExemptionTS { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия льготных условий налооблажения.
        /// </summary>
        [ListView("Дата окончания действия льготных условий налогообложения", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия льготных условий налогооблажения (ЕУСИ).
        /// </summary>
        [ListView("Дата окончания действия льготных условий налогообложения (ЕУСИ)", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения (ЕУСИ)", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDateEstate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия льготных условий налогооблажения ЗУ (ЕУСИ).
        /// </summary>
        [ListView("Дата окончания действия льготных условий налогообложения. Земельный налог (ЕУСИ)", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения. Земельный налог (ЕУСИ)", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDateEstateLand { get; set; }

        /// <summary>
        /// Получае или задает дату окончания действия льготных условий налогооблажения ТС (ЕУСИ).
        /// </summary>
        [ListView("Дата окончания действия льготных условий налогообложения. Транспортный налог (ЕУСИ)", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения. Транспортный налог (ЕУСИ)", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDateEstateTS { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия льготных условий налогооблажения ЗУ.
        /// </summary>
        [ListView("Дата окончания действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDateLand { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия льготных условий налооблажения ТС.
        /// </summary>
        [ListView("Дата окончания действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDateTS { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы. Налог на имущество.
        /// </summary>
        [SystemProperty]
        public int? TaxExemptionID { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы. Налог на землю.
        /// </summary>
        [SystemProperty]
        public int? TaxExemptionLandID { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы. Налог на транспорт.
        /// </summary>
        [SystemProperty]
        public int? TaxExemptionTSID { get; set; }

        /// <summary>
        /// Получает или задает причину налоговой льготы. Налог на имущество.
        /// </summary>
        [DetailView("Причина налоговой льготы", Visible = false)]
        [ListView("Причина налоговой льготы", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReason { get; set; }

        /// <summary>
        /// Получает или задает причину налогвоой льготы (ЕУСИ). Налог на имущество.
        /// </summary>
        [DetailView("Причина налоговой льготы (ЕУСИ)", Visible = false)]
        [ListView("Причина налоговой льготы (ЕУСИ)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReasonEstate { get; set; }

        /// <summary>
        /// Получает или задает причину налоговой льготы ЗУ (ЕУСИ).
        /// </summary>
        [DetailView("Причина налоговой льготы. Земельный налог (ЕУСИ)", Visible = false)]
        [ListView("Причина налоговой льготы. Земельный налог (ЕУСИ)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReasonEstateLand { get; set; }

        /// <summary>
        /// Получает или задает причину налоговой льготы ТС (ЕУСИ).
        /// </summary>
        [DetailView("Причина налоговой льготы. Транспортный налог (ЕУСИ)", Visible = false)]
        [ListView("Причина налоговой льготы. Транспортный налог (ЕУСИ)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReasonEstateTS { get; set; }

        /// <summary>
        /// Получает или задает причину налоговой льготы ЗУ.
        /// </summary>
        [DetailView("Причина налоговой льготы. Земельный налог", Visible = false)]
        [ListView("Причина налоговой льготы. Земельный налог", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReasonLand { get; set; }

        /// <summary>
        /// Получает или задает причину налоговой льготы ТС.
        /// </summary>
        [DetailView("Причина налоговой льготы. Транспортный налог", Visible = false)]
        [ListView("Причина налоговой льготы. Транспортный налог", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReasonTS { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия льготных условий налооблажения. Налог на имущество.
        /// </summary>
        [ListView("Дата начала действия льготных условий налогообложения", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDate { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия льготных условий налогооблажения (ЕУСИ). Налог на имущество.
        /// </summary>
        [ListView("Дата начала действия льготных условий налогообложения (ЕУСИ)", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения (ЕУСИ)", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDateEstate { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия льготных условий налогооблажения ЗУ (ЕУСИ).
        /// </summary>
        [ListView("Дата начала действия льготных условий налогообложения. Земельный налог (ЕУСИ)", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения. Земельный налог (ЕУСИ)", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDateEstateLand { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия льготных условий налогооблажения ТС (ЕУСИ).
        /// </summary>
        [ListView("Дата начала действия льготных условий налогообложения. Транспортный налог (ЕУСИ)", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения. Транспортный налог (ЕУСИ)", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDateEstateTS { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия льготных условий налогооблажения ЗУ.
        /// </summary>
        [ListView("Дата начала действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDateLand { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия льготных условий налогооблажения ТС.
        /// </summary>
        [ListView("Дата начала действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDateTS { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы в виде освобождения от налогообложения. Земельный налог.
        /// </summary>
        [ListView("Код налоговой льготы в виде освобождения от налогообложения. Земельный налог", Visible = false)]
        [DetailView("Код налоговой льготы в виде освобождения от налогообложения. Земельный налог", Visible = false)]
        public virtual TaxFreeLand TaxFreeLand { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде освобождения от налогообложения. Земельный налог.
        /// </summary>
        [SystemProperty]
        public int? TaxFreeLandID { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы в виде освобождения от налогообложения. Транспортный налог.
        /// </summary>
        [ListView("Код налоговой льготы в виде освобождения от налогообложения. Транспортный налог", Visible = false)]
        [DetailView("Код налоговой льготы в виде освобождения от налогообложения. Транспортный налог", Visible = false)]
        public virtual TaxFreeTS TaxFreeTS { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде освобождения от налогообложения. Транспортный налог.
        /// </summary>
        [SystemProperty]
        public int? TaxFreeTSID { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы в виде уменьшения суммы налога. Налог на имущество.
        /// </summary>
        [ListView("Код налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        [DetailView("Код налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        public virtual TaxLower TaxLower { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде уменьшения суммы налога. Налог на имущество.
        /// </summary>
        [SystemProperty]
        public int? TaxLowerID { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы в виде уменьшения суммы налога. Земельный налог.
        /// </summary>
        [ListView("Код налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        [DetailView("Код налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        public virtual TaxLowerLand TaxLowerLand { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде уменьшения суммы налога. Земельный налог.
        /// </summary>
        [SystemProperty]
        public int? TaxLowerLandID { get; set; }

        /// <summary>
        /// Получает или задает процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот). Налог на имущество.
        /// </summary>
        [ListView("Процент, уменьшающий исчисленную сумму налога на имущество", Visible = false)]
        [DetailView("Процент, уменьшающий исчисленную сумму налога на имущество", Visible = false)]
        public decimal? TaxLowerPercent { get; set; }

        /// <summary>
        /// Получает или задает процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот). Налог на землю.
        /// </summary>
        [ListView("Процент, уменьшающий исчисленную сумму налога на имущество (ЗУ)", Visible = false)]
        [DetailView("Процент, уменьшающий исчисленную сумму налога на имущество (ЗУ)", Visible = false)]
        public decimal? TaxLowerPercentLand { get; set; }

        /// <summary>
        /// Получает или задает процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот). Транспортный налог.
        /// </summary>
        [ListView("Процент, уменьшающий исчисленную сумму налога на имущество (TC)", Visible = false)]
        [DetailView("Процент, уменьшающий исчисленную сумму налога на имущество (TC)", Visible = false)]
        public decimal? TaxLowerPercentTS { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы в виде уменьшения суммы налога. Транспортный налог.
        /// </summary>
        [ListView("Код налоговой льготы в виде уменьшения суммы налога. Транспортный налог", Visible = false)]
        [DetailView("Код налоговой льготы в виде уменьшения суммы налога. Транспортный налог", Visible = false)]
        public virtual TaxLowerTS TaxLowerTS { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде уменьшения суммы налога ТС.
        /// </summary>
        [SystemProperty]
        public int? TaxLowerTSID { get; set; }


        /// <summary>
        /// Получает или задает код налоговой льготы в виде понижения налоговой ставки. Налог на имущество.
        /// </summary>
        [ListView("Код налоговой льготы в виде понижения налоговой ставки. Налог на имущество", Visible = false)]
        [DetailView("Код налоговой льготы в виде понижения налоговой ставки. Налог на имущество", Visible = false)]
        public TaxRateLower TaxRateLower { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде понижения налоговой ставки. Налог на имущество.
        /// </summary>
        [SystemProperty]
        public int? TaxRateLowerID { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы в виде снижения налоговой ставки. Земельный налог.
        /// </summary>
        [ListView("Код налоговой льготы в виде снижения налоговой ставки. Земельный налог", Visible = false)]
        [DetailView("Код налоговой льготы в виде снижения налоговой ставки. Земельный налог", Visible = false)]
        public virtual TaxRateLowerLand TaxRateLowerLand { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде понижения налоговой ставки. Земельный налог.
        /// </summary>
        [SystemProperty]
        public int? TaxRateLowerLandID { get; set; }

        /// <summary>
        /// Получает или задает код налоговой льготы в виде снижения налоговой ставки. Транспортный налог.
        /// </summary>
        [ListView("Код налоговой льготы в виде снижения налоговой ставки. Транспортный налог", Visible = false)]
        [DetailView("Код налоговой льготы в виде снижения налоговой ставки. Транспортный налог", Visible = false)]
        public virtual TaxRateLowerTS TaxRateLowerTS { get; set; }

        /// <summary>
        /// Получает или задает ИД кода налоговой льготы в виде снижения налоговой ставки. Транспортный налог.
        /// </summary>
        [SystemProperty]
        public int? TaxRateLowerTSID { get; set; }


        /// <summary>
        /// Получает или задает наименование налога.
        /// </summary>
        [ListView("Наименование налога", Visible = false)]
        [DetailView("Наименование налога", Visible = false)]
        public TaxRateType TaxRateType { get; set; }

        /// <summary>
        /// Получает или задает ИД наименования налога.
        /// </summary>
        [SystemProperty]
        public int? TaxRateTypeID { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку в соответствии с локальным учетом. Налог на имущество.
        /// </summary>
        [ListView("Налоговая ставка в соответствии с локальным учетом", Visible = false)]
        [DetailView("Налоговая ставка в соответствии с локальным учетом", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateValue { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку в соответствии с локальным учетом. Земельный налог.
        /// </summary>
        [ListView("Налоговая ставка в соответствии с локальным учетом. Земельный налог", Visible = false)]
        [DetailView("Налоговая ставка в соответствии с локальным учетом. Земельный налог", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateValueLand { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку в соответствии с локальным учетом. Транспортный налог.
        /// </summary>
        [ListView("Налоговая ставка в соответствии с локальным учетом. Транспортный налог", Visible = false)]
        [DetailView("Налоговая ставка в соответствии с локальным учетом. Транспортный налог", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateValueTS { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку с учетом применяемых льгот. Налог на имущество.
        /// </summary>
        [ListView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemption { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку с учетом применяемых льгот (ЕУСИ). Налог на имущество.
        /// </summary>
        [ListView("Налоговая ставка с учетом применяемых льгот (ЕУСИ), %", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот (ЕУСИ), %", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemptionEstate { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку с учетом применяемых льгот (ЕУСИ). Земельный налог.
        /// </summary>
        [ListView("Налоговая ставка с учетом применяемых льгот (ЕУСИ), %", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот (ЕУСИ), %", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemptionEstateLand { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку с учетом применяемых льгот (ЕУСИ). Транспортный налог.
        /// </summary>
        [ListView("Налоговая ставка с учетом применяемых льгот, % (ЕУСИ)", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот, % (ЕУСИ)", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemptionEstateTS { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку с учетом применяемых льгот. Земельный налог.
        /// </summary>
        [ListView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemptionLand { get; set; }

        /// <summary>
        /// Получает или задает налоговую ставку с учетом применяемых льгот. Транспортный налог.
        /// </summary>
        [ListView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemptionTS { get; set; }

        /// <summary>
        /// Получает или заадет код вида ТС.
        /// </summary>
        [DetailView("Код вида ТС", Visible = false)]
        [ListView("Код вида ТС", Visible = false)]
        public virtual TaxVehicleKindCode TaxVehicleKindCode { get; set; }

        /// <summary>
        /// Получает или задает ИД кода вида ТС.
        /// </summary>
        [SystemProperty]
        public int? TaxVehicleKindCodeID { get; set; }

        /// <summary>
        /// Получает или задает интервал ТО (лет).
        /// </summary>
        [ListView("Интервал ТО (лет)", Hidden = true)]
        [DetailView("Интервал ТО (лет)", TabName = TabName8)]
        public int? TechInspectInterval { get; set; }

        /// <summary>
        /// Получает или задает предусмотрена ли договором передача права собственности (да /нет).
        /// </summary>
        [ListView("Предусмотрена ли договором передача права собственности (да/нет)", Visible = false)]
        [DetailView("Предусмотрена ли договором передача права собственности (да/нет)", Visible = false)]
        public YesNo? TransferRight { get; set; }

        /// <summary>
        /// Получает или задает дату выгрузки в БУС.
        /// </summary>
        [ListView("Дата выгрузки в БУС", Visible = false)]
        [DetailView("Дата выгрузки в БУС", Visible = false)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? TransferBUSDate { get; set; }

        /// <summary>
        /// Получает или задает дату обновления записи.
        /// </summary>
        [ListView("Дата обновления информации", Hidden = true)]
        [DetailView("Дата обновления информации", TabName = TabName2, Order = 32)]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Получает или задает срок полезного использования по РСБУ.
        /// </summary>
        [DetailView("СПИ", TabName = TabName2, Order = 36)]
        [PropertyDataType(PropertyDataType.Text)]
        public int? Useful { get; set; }

        /// <summary>
        /// Получает или задает оставшийся срок полезного использования.
        /// </summary>       
        [DetailView("Оставшийся срок службы по бухгалтерскому учету (месяцев)", TabName = TabName2, Order = 37)]
        public int? UsefulEnd { get; set; }

        /// <summary>
        /// Получает или задает дату окончания срока полезного использования.
        /// </summary>
        [DetailView("Дата окончания СПИ", TabName = TabName2, Order = 39)]
        public DateTime? UsefulEndDate { get; set; }

        /// <summary>
        /// Получает или задает оставшийся срок службы по МСФО (месяцев).
        /// </summary>
        [DetailView("Оставшийся срок службы по МСФО (месяцев)", Visible = false)]
        [ListView("Оставшийся срок службы по МСФО (месяцев)", Visible = false)]
        public int? UsefulEndMSFO { get; set; }

        /// <summary>
        /// Получает или задает оставшийся срок службы по налоговому учету (месяцев).
        /// </summary>
        [DetailView("Оставшийся срок службы по налоговому учету (месяцев) ", Visible = false)]
        [ListView("Оставшийся срок службы по налоговому учету (месяцев) ", Visible = false)]
        public int? UsefulEndNU { get; set; }

        /// <summary>
        /// Получает или задает СПИ по МСФО.
        /// </summary>
        [DetailView("СПИ по МСФО", Visible = false)]
        [ListView("СПИ по МСФО", Visible = false)]
        public int? UsefulForMSFO { get; set; }

        /// <summary>
        /// Получает или задает СПИ по НУ.
        /// </summary>
        [DetailView("СПИ по НУ", Visible = false)]
        [ListView("СПИ по НУ", Visible = false)]
        public int? UsefulForNU { get; set; }

        /// <summary>
        /// Получает или задает разрешенное использование.
        /// </summary>
        [DetailView(Name = "Разрешенное использование", TabName = TabName7, Order = 89)]
        [PropertyDataType(PropertyDataType.Text)]
        public string UsesKind { get; set; }

        /// <summary>
        /// Получает или задает среднюю стоимость ТС.
        /// </summary>
        [DetailView("Средняя стоимость ТС, руб.", Visible = false)]
        [ListView("Средняя стоимость ТС, руб.", Visible = false)]
        public decimal? VehicleAverageCost { get; set; }

        /// <summary>
        /// Получает или задает категорию ТС.
        /// </summary>
        [ListView("Категория ТС", Hidden = true)]
        [DetailView("Категория ТС", TabName = TabName8, Order = 91)]
        public VehicleCategory VehicleCategory { get; set; }

        /// <summary>
        /// Получает или задает ИД категории ТС.
        /// </summary>
        [SystemProperty]
        public int? VehicleCategoryID { get; set; }

        /// <summary>
        /// Получает или задает единый классификатор ТС.
        /// </summary>
        [DetailView("Единый классификатор транспортных средств", Visible = false)]
        [ListView("Единый классификатор транспортных средств", Visible = false)]
        public VehicleClass VehicleClass { get; set; }

        /// <summary>
        /// Получает или задает ИД единого классификатора ТС.
        /// </summary>
        [SystemProperty]
        public int? VehicleClassID { get; set; }

        /// <summary>
        /// Получает или задает дату снятия ТС с учета в гос. органах.
        /// </summary>
        [ListView("Дата снятия с учета ТС в гос.органах", Hidden = true)]
        [DetailView("Дата снятия с учета ТС в гос.органах", TabName = TabName8, Order = 103)]
        public DateTime? VehicleDeRegDate { get; set; }

        /// <summary>
        /// Получает или задает калсс ТС.
        /// </summary>
        [DetailView("Класс ТС", Visible = false)]
        [ListView("Класс ТС", Visible = false)]
        public VehicleLabel VehicleLabel { get; set; }

        /// <summary>
        /// Получает или задает ИД класса ТС.
        /// </summary>
        [SystemProperty]
        public int? VehicleLabelID { get; set; }

        /// <summary>
        /// Получает или задает рыночную стоимость в руб.
        /// </summary>
        [DetailView("Рыночная стоимость, руб.", Visible = false)]
        [ListView("Рыночная стоимость, руб.", Visible = false)]
        public decimal? VehicleMarketCost { get; set; }

        /// <summary>
        /// Получает или задает ИД марки ТС.
        /// </summary>
        [SystemProperty]
        public int? VehicleModelID { get; set; }

        /// <summary>
        /// Получает или задает дату регистрации ТС в гос. органах.
        /// </summary>
        [ListView("Дата регистрации ТС в гос.органах", Hidden = true)]
        [DetailView("Дата регистрации ТС в гос.органах", TabName = TabName8, Order = 102)]
        public DateTime? VehicleRegDate { get; set; }

        /// <summary>
        /// Получает или задает номер гос. регистрации ТС.
        /// </summary>
        [ListView("Номер Госрегистрации ТС", Hidden = true)]
        [DetailView("Номер Госрегистрации ТС", TabName = TabName8, Order = 100)]
        [PropertyDataType(PropertyDataType.Text)]
        public string VehicleRegNumber { get; set; }

        /// <summary>
        /// Получает или задает повышающий/понижающий коэффициент расчета транспортного налога.
        /// </summary>
        [DetailView("Повышающий/понижающий коэффициент расчета транспортного налога", Visible = false)]
        [ListView("Повышающий/понижающий коэффициент расчета транспортного налога", Visible = false)]
        public decimal? VehicleTaxFactor { get; set; }

        /// <summary>
        /// Получает или задает тип ТС.
        /// </summary>
        [ListView("Тип ТС", Hidden = true)]
        [DetailView("Тип ТС", TabName = TabName8, Order = 89)]
        public TenureType VehicleTenureType { get; set; }

        /// <summary>
        /// Получает или задает ИД типа ТС.
        /// </summary>
        [SystemProperty]
        public int? VehicleTenureTypeID { get; set; }

        /// <summary>
        /// Получает ии задает вид ТС.
        /// </summary>
        [ListView("Вид ТС", Hidden = true)]
        [DetailView("Вид ТС", TabName = TabName8, Order = 90)]
        public VehicleType VehicleType { get; set; }

        /// <summary>
        /// Получает или задает ИД вида тС.
        /// </summary>
        [SystemProperty]
        public int? VehicleTypeID { get; set; }

        /// <summary>
        /// Получает или задает скважину.
        /// </summary>      
        [ListView("Скважина", Hidden = true)]
        [DetailView("Скважина", TabName = TabName4, Order = 58)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Well { get; set; }

        /// <summary>
        /// Получает или задает категорию скважины.
        /// </summary>
        [DetailView("Категория скважины", Visible = false)]
        [ListView("Категория скважины", Visible = false)]
        public WellCategory WellCategory { get; set; }

        /// <summary>
        /// Получает или задает ИД категории скважины.
        /// </summary>
        [SystemProperty]
        public int? WellCategoryID { get; set; }

        /// <summary>
        /// Получает или задает ОГ - пользователя ОБУ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView("Пользователь", Order = 14)]
        [DetailView("Пользователь", TabName = CaptionHelper.DefaultTabName, Order = 10, Visible = false)]
        public Society WhoUse { get; set; }

        /// <summary>
        /// Получает или задает идентификатор ОГ - пользователя ОБУ.
        /// </summary>
        [SystemProperty]
        public int? WhoUseID { get; set; }

        /// <summary>
        /// Получает ИНН пользователя ОБУ.
        /// </summary>
        [DetailView("Пользователь (ИНН)", TabName = CaptionHelper.DefaultTabName, Order = 13)]
        public string WhoUseINN => _whoUseINN.Evaluate(this);

        /// <summary>
        /// Получает наименование пользователя ОБУ.
        /// </summary>
        [DetailView("Пользователь", TabName = CaptionHelper.DefaultTabName, Order = 12)]
        public string WhoUseString => _whoUseString.Evaluate(this);

        /// <summary>
        /// Получает или задает ширину речного/морского судна.
        /// </summary>
        [DetailView("Ширина судна", TabName = TabName9, Order = 120)]
        public decimal? Width { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения ширины речного/морского судна.
        /// </summary>
        [DetailView("Ед. измерения ширины судна", TabName = TabName9, Order = 121)]
        public SibMeasure WidthUnit { get; set; }

        /// <summary>
        /// Получает или задает ИД ед. измерения ширины речного/морского судна.
        /// </summary>
        [SystemProperty]
        public int? WidthUnitID { get; set; }

        /// <summary>
        /// Получает или задает принак лесного участка.
        /// </summary>
        [DetailView("Лесной участок", TabName = TabName7, Order = 86)]
        [DefaultValue(false)]
        public bool Wood { get; set; }

        /// <summary>
        /// Получает или задает год постройки объекта.
        /// </summary>
        [ListView("Год постройки", Hidden = true)]
        [DetailView("Год постройки", TabName = TabName6, Order = 80)]
        public int? Year { get; set; }

        /// <summary>
        /// Получает или задает год выпуска ТС.
        /// </summary>
        [ListView("Год выпуска", Hidden = true)]
        [DetailView("Год выпуска", TabName = TabName8, Order = 99)]
        public int? YearOfIssue { get; set; }
                       

        /// <summary>
        /// Переопределяет метод перед сохранением объекта.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="entry">Запись об объекте.</param>
        public override void OnSaving(IUnitOfWork uow, object entry)
        {
            //base.OnSaving(uow, entry);

            if (ID == 0) return;

            var original = uow.GetRepository<AccountingObject>().GetOriginal(ID).StateObjectRSBUID;

            if (StateObjectRSBU?.ID != original)
                this.StateChangeDate = DateTime.Now;
        }

        /// <summary>
        /// Устанавливает ссылки на ГГР-ы.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        public void SetSSR(IUnitOfWork uow)
        {
            try
            {
                //TODO: неадекватность какая-то, разобраться с изменением объекта после сохранения в БД
                var yeahr = DateTime.Now.Year;
                var ids = ID;

                ScheduleStateRegistration rr =
                       uow.GetRepository<ScheduleStateRegistrationRecord>()
                       .Filter(x => x.ScheduleStateRegistration.Year == yeahr
                       && x.AccountingObjectID == ids)
                       .OrderByDescending(s => s.ScheduleStateRegistration.CreateDate)
                       .Select(s => s.ScheduleStateRegistration).FirstOrDefault();

                if (this.SSR != rr)
                {
                    this.SSR = rr;
                }

                ScheduleStateTerminate term = uow.GetRepository<ScheduleStateTerminateRecord>()
                       .Filter(x => x.ScheduleStateTerminate.Year == yeahr
                       && x.AccountingObjectID == ids)
                       .OrderByDescending(s => s.ScheduleStateTerminate.CreateDate)
                       .Select(s => s.ScheduleStateTerminate).FirstOrDefault();
                if (this.SSRTerminate != term)
                {
                    this.SSRTerminate = term;
                }

                uow.SaveChanges();
            }
            catch
            {
            }
        }
    }
}
