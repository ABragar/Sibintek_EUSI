using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.DAL;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CorpProp.Entities.Subject
{
    /// <summary>
    /// Представляет общество группы.
    /// </summary>    
    [EnableFullTextSearch]
    public class SocietyCalculatedField : TypeObject
    {
        private static readonly CompiledExpression<SocietyCalculatedField, string> _Name =
          DefaultTranslationOf<SocietyCalculatedField>.Property(x => x.Name).Is(x => x.IDEUP + " " + x.ShortName);


        private static readonly CompiledExpression<SocietyCalculatedField, string> _contactResponsableForResponse =
                DefaultTranslationOf<SocietyCalculatedField>.Property(x => x.ContactResponsableForResponse).Is(x => x.ResponsableForResponse == null ? null : x.ResponsableForResponse.Phone);
        private static readonly CompiledExpression<SocietyCalculatedField, string> _subdivisionResponsableForResponse =
                DefaultTranslationOf<SocietyCalculatedField>.Property(x => x.SubdivisionResponsableForResponse).Is(x => x.ResponsableForResponse == null ? null : x.ResponsableForResponse.SocietyDeptName);

        private static readonly CompiledExpression<SocietyCalculatedField, bool?> _isCauk = DefaultTranslationOf<SocietyCalculatedField>.Property(x => x.IsCAUK).Is(x => x.IDEUP == "1");
        private static readonly CompiledExpression<SocietyCalculatedField, bool?> _isService = DefaultTranslationOf<SocietyCalculatedField>.Property(x => x.IsService).Is(x => x.IDEUP == "90");


        #region Contstructor
        /// <summary>
        /// Инициализирует новый экземпляр класса SocietyCalculatedField.
        /// </summary>
        public SocietyCalculatedField() : base()
        {
        }

        #endregion

        #region Request

        [DetailView(Name = "Контакты ответственного", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [ListView(Name = "Контакты ответственного", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContactResponsableForResponse => _contactResponsableForResponse.Evaluate(this);

        [DetailView(Name = "Подразделение ответственного", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [ListView(Name = "Подразделение ответственного", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubdivisionResponsableForResponse => _subdivisionResponsableForResponse.Evaluate(this);

        #endregion

        /// <summary>
        /// Получает код ЕУП и краткое наименование ОГ.
        /// </summary>
        [DetailView(Name = "Наименование", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name => _Name.Evaluate(this);


        /// <summary>
        /// Получает или задает краткое наименование.
        /// </summary>

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сокращенное наименование на русском языке", Order = 6,
         TabName = CaptionHelper.DefaultTabName, Required = true, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShortName { get; set; }

        /// <summary>
        /// Получает или задает полное наименование.
        /// </summary>

        [FullTextSearchProperty]
        [DetailView(Name = "Полное наименование на русском языке", Order = 5,
            TabName = CaptionHelper.DefaultTabName, Required = true, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FullName { get; set; }

        /// <summary>
        /// Получает или задает краткое наименование.
        /// </summary>

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сокращенное наименование на английском языке", Order = 8,
         TabName = CaptionHelper.DefaultTabName, Required = false, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShortNameEng { get; set; }

        /// <summary>
        /// Получает или задает полное наименование на английском языке.
        /// </summary>

        [FullTextSearchProperty]
        [DetailView(Name = "Полное наименование на английском языке", Order = 7,
            TabName = CaptionHelper.DefaultTabName, Required = false, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FullNameEng { get; set; }

        /// <summary>
        /// Получает или задает идентификатор СДП.
        /// </summary>
        /// <remarks>
        /// Для установления связи между СДП и ЕУП через КСК.
        /// </remarks>
        [DetailView(Name = "Идентификатор СДП", Order = 3, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SDP { get; set; }

        /// <summary>
        /// Получает или задает идентификатор КСК.
        /// </summary>
        [DetailView(Name = "Код КСК", Order = 3, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string KSK { get; set; }

        /// <summary>
        /// Получает или задает ИД типа делового партнёра.
        /// </summary>
        public int? SubjectTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип делового партнёра.
        /// </summary>
        [DetailView(Name = "Тип делового партнёра", Order = 36, TabName = CaptionHelper.DefaultTabName, Required = false, Visible = false, ReadOnly = true)]
        public SubjectType SubjectType { get; set; }

        /// <summary>
        /// Получает или задает ИД вида делового партнёра.
        /// </summary>
        public int? SubjectKindID { get; set; }

        /// <summary>
        /// Получает или задает вид делового партнёра.
        /// </summary>
        [DetailView(Name = "Вид делового партнёра", Order = 37, TabName = CaptionHelper.DefaultTabName, Required = false, Visible = false, ReadOnly = true)]
        public SubjectKind SubjectKind { get; set; }


        /// <summary>
        /// Получает или задает ИД страны.
        /// </summary>
        public int? CountryID { get; set; }

        /// <summary>
        /// Получает или задает страну.
        /// </summary>
        [DetailView(Name = "Юрисдикция", Order = 12, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public SibCountry Country { get; set; }


        /// <summary>
        /// Получает или задает фактический адрес (строка).
        /// </summary>
        [DetailView(Name = "Адрес фактический", Order = 17, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressActualString { get; set; }

        /// <summary>
        /// Получает или задает юридический адрес (строка).
        /// </summary>
        [DetailView(Name = "Адрес в соответствии с уставом", Order = 16, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLegalString { get; set; }

        /// <summary>
        /// Получает или задает телефон.
        /// </summary>
        [MaxLength(255)]
        [PropertyDataType(PropertyDataType.PhoneNumber)]
        [DetailView(Name = "Контактные телефоны, факс", Order = 18, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string Phone { get; set; }


        /// <summary>
        /// Получает или задает адрес электронной почты.
        /// </summary>
        [MaxLength(255)]
        [DetailView(Name = "Официальный адрес электронной почты", Order = 19, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Получает или задает ОГРН.
        /// </summary>
        [DetailView(Name = "ОГРН", Order = 11, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=OGRN")]
        public string OGRN { get; set; }

        /// <summary>
        /// Получает или задает ОГРНИП.
        /// </summary>
        [DetailView(Name = "ОГРНИП", Order = 38, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=OGRNIP")]
        public string OGRNIP { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКВЕД.
        /// </summary>
        public int? OKVEDID { get; set; }

        /// <summary>
        /// Получает или задает ОКВЕД.
        /// </summary>
        [DetailView(Name = "ОКВЭД", Order = 39, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        public SibOKVED OKVED { get; set; }

        /// <summary> 
        /// Получает или задает ИНН.
        /// </summary>
        [UIHint("Integer")]
        [DetailView(Name = "ИНН", Order = 10, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=INN")]
        public string INN { get; set; }

        /// <summary>
        /// Получает или задает КПП.
        /// </summary>
        [DetailView(Name = "КПП", Order = 40, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=KPP")]
        public string KPP { get; set; }

        /// <summary>
        /// Получает или задает ОКПО.
        /// </summary>
        [DetailView(Name = "ОКПО", Order = 41, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=OKPO")]
        public string OKPO { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКАТО.
        /// </summary>
        public int? OKATOID { get; set; }

        /// <summary>
        /// Получает или задает ОКАТО.
        /// </summary>
        [DetailView(Name = "ОКАТО", Order = 42, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        public OKATO OKATO { get; set; }


        /// <summary>
        /// Получает или задает дату регистрации.
        /// </summary>
        [DetailView(Name = "Дата государственной регистрации", Order = 9, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public DateTime? DateRegistration { get; set; }

        /// <summary>
        /// Получает или задает ФИО руководителя.
        /// </summary>

        [DetailView(Name = "Ф.И.О. руководителя", Order = 35, TabName = "[2]Руководство", ReadOnly = true, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string HeadName { get; set; }

        /// <summary>
        /// Получает или задает должность руководителя.
        /// </summary>

        [DetailView(Name = "Должность руководителя", Order = 35, TabName = "[2]Руководство", ReadOnly = true, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string HeadPosition { get; set; }

        /// <summary>
        /// Получает или задает признак является субъектом МСП.
        /// </summary>
        [DetailView(Name = "Является субъектом МСП", Order = 43, TabName = CaptionHelper.DefaultTabName,
            ReadOnly = true, Visible = false)]
        [DefaultValue(false)]
        public bool IsSubjectMSP { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Примечание", Order = 44, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД ответственного пользователя.
        /// </summary>
        public int? ResponsableForResponseID { get; set; }

        /// <summary>
        /// Получает или задает пользователя.
        /// </summary>
        ///<remarks>
        ///Ответственный пользователь.
        /// </remarks>
        /// 

        [ListView(Name = "Ответственный за ответы", Visible = false)]
        [DetailView(Name = "Ответственный за ответы", Order = 45, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public SibUser ResponsableForResponse { get; set; }

        /// <summary>
        /// Получает или задает ИД (код) ЕУП.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView(Name = "Код ЕУП", Order = 0, TabName = CaptionHelper.DefaultTabName, Required = true,
            ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDEUP { get; set; }

        /// <summary>
        /// Получает или задает признак является ключевым.
        /// </summary>

        [ListView]
        [DetailView(Name = "Ключевое ОГ", Order = 20,
            TabName = "[1]Признаки/описание", ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsSocietyKey { get; set; }

        /// <summary>
        /// Получает или задает признак является совместным.
        /// </summary>

        [ListView]
        [DetailView(Name = "Совместное предприятие", Order = 21,
            TabName = "[1]Признаки/описание", ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsSocietyJoint { get; set; }

        /// <summary>
        /// Получает или задает признак является резидентом.
        /// </summary>
        [ListView]
        [DetailView(Name = "Резидент", Order = 22,
            TabName = "[1]Признаки/описание", ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsSocietyResident { get; set; }

        /// <summary>
        /// Получает или задает  признак является контролируемым.
        /// </summary>

        [ListView]
        [DetailView(Name = "Подконтрольное", Order = 23,
            TabName = "[1]Признаки/описание", ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsSocietyControlled { get; set; }

        /// <summary>
        /// Получает или задает признак ОГ.
        /// </summary>        
        [ListView]
        [DetailView(Name = "Общество Группы", Order = 24,
            TabName = "[1]Признаки/описание", ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsSociety { get; set; }

        /// <summary>
        /// Получает или задает ИД валюты компании.
        /// </summary>
        public int? CurrencyID { get; set; }

        /// <summary>
        /// Получает или задает валюту компании.
        /// </summary>

        [DetailView(Name = "Валюта компании", Order = 48, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public Currency Currency { get; set; }

        /// <summary>
        /// Получает или задает ИД единицы консолидации.
        /// </summary>
        public int? ConsolidationUnitID { get; set; }

        /// <summary>
        /// Получает или задает единицу консолидации.
        /// </summary>

        [ListView]
        [DetailView(Name = "ЕК(БЕ)", Order = 1, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public Consolidation ConsolidationUnit { get; set; }


        ///// <summary>
        ///// Получает или задает ИД бизнес-сегмента.
        ///// </summary>
        //public int? BusinessSegmentID { get; set; }

        ///// <summary>
        ///// Получает или задает бизнес-сегмент.
        ///// </summary>       
        ////[ListView]
        //[DetailView(Name = "Бизнес-сегмент", Order = 32, TabName = "[1]Признаки/описание", ReadOnly = true)]
        //public virtual BusinessSegment BusinessSegment { get; set; }

        /// <summary>
        /// Получает или задает бизнес-сегмент.
        /// </summary>       
        [ListView]
        [DetailView(Name = "Бизнес-сегмент", Order = 32, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public string BusinessSegment { get; set; }

        /// <summary>
        /// Получает или задает бизнес-блок.
        /// </summary>       
//        [ListView]
//        [DetailView(Name = "Бизнес-блок", Order = 31, TabName = "[1]Признаки/описание", ReadOnly = true)]
//        public string BusinessBlock { get; set; }
        
        public int? BusinessBlockID { get; set; }

        /// <summary>
        /// Получает или задает бизнес-блок.
        /// </summary>       
        [ListView]
        [DetailView(Name = "Бизнес-блок", Order = 31, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public BusinessBlock BusinessBlock { get; set; }

        ///// <summary>
        ///// Получает или задает ИД бизнес-направления.
        ///// </summary>
        //public int? BusinessDirectionID { get; set; }

        ///// <summary>
        ///// Получает или задает бизнес-направление.
        ///// </summary>       
        ////[ListView]
        //[DetailView(Name = "Бизнес-блок", Order = 33, TabName = "[1]Признаки/описание", ReadOnly = true)]
        //public virtual BusinessDirection BusinessDirection { get; set; }


        /// <summary>
        /// Получает или задает бизнес-направление.
        /// </summary>       
        [ListView]
        [DetailView(Name = "Бизнес-направление", Order = 33, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public string BusinessDirection { get; set; }

        ///// <summary>
        ///// Получает или задает ИД производственного блока.
        ///// </summary>
        //public int? ProductionBlockID { get; set; }

        ///// <summary>
        ///// Получает или задает производственный блок.
        ///// </summary>        
        ////[ListView]
        //[DetailView(Name = "Производственный блок", Order = 30, TabName = "[1]Признаки/описание", ReadOnly = true)]
        //public virtual ProductionBlock ProductionBlock { get; set; }

        /// <summary>
        /// Получает или задает производственный блок.
        /// </summary>        
        [ListView]
        [DetailView(Name = "Производственный блок", Order = 30, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public string ProductionBlock { get; set; }

        /// <summary>
        /// Получает или задает эффективная доля участия в капитале.
        /// </summary>

        [DetailView(Name = "Эффективная доля участия в УК, %", Order = 41, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        [PropertyDataType("Sib_Decimal8")]
        public decimal ShareInEquity { get; set; }

        /// <summary>
        /// Получает или задает эффективная доля участия в голосующих акциях.
        /// </summary>

        [DetailView(Name = "Эффективная доля в голосующих акциях, %", Order = 42, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        [PropertyDataType("Sib_Decimal8")]
        public decimal ShareInVotingRights { get; set; }

        /// <summary>
        /// Получает или задает размер уставного капитала.
        /// </summary>

        [DetailView(Name = "Размер УК", Order = 49, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? SizeAuthorizedCapital { get; set; }

        /// <summary>
        /// Получает или задает бенефициарную долю участия в капитале.
        /// </summary>

        [DetailView(Name = "Бенефициарная доля участия в капитале, %", Order = 50
            ,Visible = false
            ,TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? BeneficialShareInCapital { get; set; }

        /// <summary>
        /// Получает или задает бенефициарную долю участия в голосующих акциях.
        /// </summary>

        [DetailView(Name = "Бенефициарная доля участия в голосующих акциях, %",
            Visible = false,
            Order = 51, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? BeneficialShareInVotingRights { get; set; }

        /// <summary>
        /// Получает или задает БСА, руб.
        /// </summary>

        [DetailView(Name = "БСА, руб.", Visible = false, Order = 52, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? BCA { get; set; }

        /// <summary>
        /// Получает или задает ЧА, руб.
        /// </summary>

        [DetailView(Name = "ЧА, руб.", Visible = false, Order = 52, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? ChA { get; set; }

        /// <summary>
        /// Получает или задает выручку от продажи, руб.
        /// </summary>

        [DetailView(Name = "Выручка от продажи, руб.", Visible = false, Order = 52, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? SalesRevenue { get; set; }

        /// <summary>
        /// Получает или задает чистую прибыль (убыток), руб.
        /// </summary>

        [DetailView(Name = "Чистая прибыль (убыток), руб.", Visible = false, Order = 52, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? NetProfit { get; set; }

        /// <summary>
        /// Получает или задает совокупный финансовый результат периода, руб.
        /// </summary>

        [DetailView(Name = "Совокупный финансовый результат периода, руб.", Visible = false, Order = 52, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public decimal? AggregateFinancialResultOfThePeriod { get; set; }

        /// <summary>
        /// Получает или задает дату включения в группу.
        /// </summary>

        [DetailView(Name = "Дата включения в группу", Order = 18, TabName = "[1]Признаки/описание", ReadOnly = true, Visible = false)]
        public DateTime? DateInclusionInGroup { get; set; }

        /// <summary>
        /// Получает или задает дату исключения из группы.
        /// </summary>

        [DetailView(Name = "Дата исключения из группы", Order = 19, TabName = "[1]Признаки/описание", ReadOnly = true, Visible = false)]
        public DateTime? DateExclusionFromGroup { get; set; }

        /// <summary>
        /// Получает или задает дату включения в периметр.
        /// </summary>

        [DetailView(Name = "Дата включения в Периметр", Order = 25, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public DateTime? DateInclusionInPerimeter { get; set; }

        /// <summary>
        /// Получает или задает ИД основания включения в периметр.
        /// </summary>
        public int? BaseInclusionInPerimeterID { get; set; }

        /// <summary>
        /// Получает или задает основание включения в периметр.
        /// </summary>

        [DetailView(Name = "Основание для включения в Периметр", Order = 26, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public BaseInclusionInPerimeter BaseInclusionInPerimeter { get; set; }

        /// <summary>
        /// Получает или задает дату исключения из периметра.
        /// </summary>

        [DetailView(Name = "Дата исключения из Периметра", Order = 28, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public DateTime? DateExclusionFromPerimeter { get; set; }

        /// <summary>
        /// Получает или задает признак исключения из периметра.
        /// </summary>
        [DetailView(Name = "Исключен из Периметра", Order = 27, TabName = "[1]Признаки/описание", ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsExclusionFromPerimeter { get; set; }

        /// <summary>
        /// Получает или задает ИД основания исключения из периметра.
        /// </summary>
        public int? BaseExclusionFromPerimeterID { get; set; }

        /// <summary>
        /// Получает или задает основание исключения из периметра.
        /// </summary>

        [DetailView(Name = "Основание для исключения из Периметра", Order = 29, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public BaseExclusionFromPerimeter BaseExclusionFromPerimeter { get; set; }

        /// <summary>
        /// Получает или задает наименование ЕИО.
        /// </summary>

        [DetailView(Name = "Ф.И.О./наименование ЕИО", Order = 35, TabName = "[2]Руководство", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SoleExecutiveBodyName { get; set; }

        /// <summary>
        /// Получает или задает должность ЕИО.
        /// </summary>

        [DetailView(Name = "Должность ЕИО", Order = 36, TabName = "[2]Руководство", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SoleExecutiveBodyPost { get; set; }



        /// <summary>
        /// Получает или задает дату начала полномочий ЕИО.
        /// </summary>

        [DetailView(Name = "Дата начала полномочий ЕИО", Order = 37, TabName = "[2]Руководство", ReadOnly = true)]
        public DateTime? SoleExecutiveBodyDateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания полномочий ЕИО.
        /// </summary>

        [DetailView(Name = "Дата прекращения полномочий ЕИО", Order = 38, TabName = "[2]Руководство", ReadOnly = true)]
        public DateTime? SoleExecutiveBodyDateTo { get; set; }


        /// <summary>
        /// Получает или задает куратора.
        /// </summary>

        [DetailView(Name = "Курирующий ВП", Order = 40, TabName = "[2]Руководство", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Curator { get; set; }
        /// <summary>
        /// Получает или задает ИД фактического вида деятельности.
        /// </summary>
        public int? ActualKindActivityID { get; set; }
        /// <summary>
        /// Получает или задает фактический вид деятельности.
        /// </summary>

        [DetailView(Name = "Фактический вид деятельности", Order = 53, TabName = "[1]Признаки/описание", ReadOnly = true)]
        public ActualKindActivity ActualKindActivity { get; set; }

        /// <summary>
        /// Получает или задает ИД Федерального округа.
        /// </summary>
        public int? FederalDistrictID { get; set; }

        /// <summary>
        /// Получает или задает Федеральный округ.
        /// </summary>

        [DetailView(Name = "Федеральный округ", Order = 13, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public SibFederalDistrict FederalDistrict { get; set; }

        /// <summary>
        /// Получает или задает ИД Региона.
        /// </summary>
        public int? RegionID { get; set; }

        /// <summary>
        /// Получает или задает Регион.
        /// </summary>

        [DetailView(Name = "Субъект РФ", Order = 14, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public SibRegion Region { get; set; }

        /// <summary>
        /// Получает или задает Город.
        /// </summary>

        [DetailView(Name = "Город", Order = 15, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string City { get; set; }

        /// <summary>
        /// Получает или задает признак Подконтрольное (по КОУ).
        /// </summary>
        [DetailView(Name = "Подконтрольное (по КОУ)", Order = 43, TabName = "[1]Признаки/описание", ReadOnly = true, Visible = false)]
        [DefaultValue(false)]
        public bool IsKOUControl { get; set; }

        /// <summary>
        /// Получает или задает признак Подконтрольное (По доле).
        /// </summary>
        [DetailView(Name = "Подконтрольное (По доле)", Order = 43, TabName = "[1]Признаки/описание", ReadOnly = true, Visible = false)]
        [DefaultValue(false)]
        public bool IsShareControl { get; set; }

        /// <summary>
        /// Получает или задает признак Подконтрольное (По ЕИО).
        /// </summary>
        [DetailView(Name = "Подконтрольное (По ЕИО)", Order = 43, TabName = "[1]Признаки/описание", ReadOnly = true, Visible = false)]
        [DefaultValue(false)]
        public bool IsSoleExecutiveBodyControl { get; set; }

        /// <summary>
        /// Получает или задает ИД ОПФ.
        /// </summary>
        public int? OPFID { get; set; }

        /// <summary>
        /// Получает или задает ОПФ.
        /// </summary>

        [DetailView(Name = "ОПФ", Order = 4, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public OPF OPF { get; set; }

        /// <summary>
        /// Получает или задает ИД Курирующие СП.
        /// </summary>
        public int? UnitOfCompanyID { get; set; }

        /// <summary>
        /// Получает или задает Курирующие СП.
        /// </summary>

        [DetailView(Name = "Курирующие СП", Order = 39, TabName = "[2]Руководство", ReadOnly = true)]
        public UnitOfCompany UnitOfCompany { get; set; }

        /// <summary>
        /// Получает или задает количество правопредшественников.
        /// </summary>

        [DetailView(Name = "Количество правопредшественников", Order = 54, TabName = "[1]Признаки/описание", ReadOnly = true)]
        [DefaultValue(0)]
        public int? SocietyPredecessorsCount { get; set; }

        /// <summary>
        /// Получает или задает долю прямого участия.
        /// </summary>

        [DetailView(Name = "Доля прямого участия", Order = 43, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        [DefaultValue(0)]
        public decimal? DirectShare { get; set; }

        /// <summary>
        /// Получает или задает доли прямого участия в голосующих акциях.
        /// </summary>

        [DetailView(Name = "Доля прямого участия в голосующих акциях", Order = 45, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        [DefaultValue(0)]
        public decimal? DirectShareInVotingRights { get; set; }

        /// <summary>
        /// Получает или задает кол-во прямых участников.
        /// </summary>

        [DetailView(Name = "Прямых участников", Order = 46, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public string DirectParticipantCount { get; set; }

        /// <summary>
        /// Получает или задает список прямых участников.
        /// </summary>

        [DetailView(Name = "Список прямых участников", Order = 47, TabName = "[3]Доля владения и УК", ReadOnly = true)]
        public string DirectParticipantList { get; set; }

        /// <summary>
        /// Является ли ОГ ЦАУК.
        /// <remarks>Цаук – это ОГ с кодом 01.</remarks>
        /// </summary>
        public bool? IsCAUK => _isCauk.Evaluate(this);

        /// <summary>
        /// Является ли ОГ Сервисной организацией.
        /// <remarks>Сервис – это ОГ с кодом 90.</remarks>
        /// </summary>
        public bool? IsService => _isService.Evaluate(this);

        public string ExtraID { get; }

        /// <summary>
        /// Получает или задает дату начала действия записи ракурса.
        /// </summary>
        [DetailView("Дата начала действия записи ракурса", Visible = false), ListView(Visible = false)]
        [SystemProperty]
        public DateTime? DataDateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия записи ракурса.
        /// </summary>
        [DetailView("Дата окончания действия записи ракурса", Visible = false), ListView(Visible = false)]
        [SystemProperty]
        public DateTime? DataDateTo { get; set; }

        /// <summary>
        ///Кол-во объектов движимого и недвижимого имущества по БУ, шт.
        ///
        [DetailView(Name = "Кол-во объектов движимого и недвижимого имущества по БУ, шт.", Visible = false, Order = 100, TabName = "[4]Показатели", ReadOnly = true)]
        public int? CountInventoryObject { get; set; }

        /// <summary>
        ///Первоначальная стоимость объектов движимого и недвижимого имущества, руб.
        ///
        [DetailView(Name = "Первоначальная стоимость объектов движимого и недвижимого имущества, руб.", Visible = false, Order = 101, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? InitialCostInventoryObject { get; set; }
        /// <summary>
        ///Остаточная стоимость объектов движимого и недвижимого имущества, руб.
        ///
        [DetailView(Name = "Остаточная стоимость объектов движимого и недвижимого имущества, руб.", Visible = false, Order = 102, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? ResidualCostInventoryObject { get; set; }
        /// <summary>
        ///Кол-во объектов недвижимого имущества по данным БУ, шт.
        ///
        [DetailView(Name = "Кол-во объектов недвижимого имущества по данным БУ, шт.", Visible = false, Order = 103, TabName = "[4]Показатели", ReadOnly = true)]
        public int? CountRealEstate { get; set; }
        /// <summary>
        ///Кол-во объектов недвижимого имущества по данным ПУ, шт.
        ///
        [DetailView(Name = "Кол-во объектов недвижимого имущества по данным ПУ, шт.", Visible = false, Order = 104, TabName = "[4]Показатели", ReadOnly = true)]
        public int? CountRealEstateRight { get; set; }
        /// <summary>
        ///Первоначальная стоимость объектов недвижимого имущества, руб.
        ///
        [DetailView(Name = "Первоначальная стоимость объектов недвижимого имущества, руб.", Visible = false, Order = 105, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? InitialCostRealEstate { get; set; }
        /// <summary>
        ///Остаточная стоимость объектов недвижимого имущества, руб.
        ///
        [DetailView(Name = "Остаточная стоимость объектов недвижимого имущества, руб.", Visible = false, Order = 106, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? ResidualCostRealEstate { get; set; }
        /// <summary>
        ///Кол-во объектов недвижимого имущества по данным БУ, по которым не зарегистрировано право, шт.
        ///
        [DetailView(Name = "Кол-во объектов недвижимого имущества по данным БУ, по которым не зарегистрировано право, шт.", Visible = false, Order = 107, TabName = "[4]Показатели", ReadOnly = true)]
        public int? CountRealEstateNotRight { get; set; }
        /// <summary>
        ///Первоначальная стоимость объектов недвижимого имущества, по которым не зарегистрировано право, руб.
        ///
        [DetailView(Name = "Первоначальная стоимость объектов недвижимого имущества, по которым не зарегистрировано право, руб.", Visible = false, Order = 108, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? InitialCostRealEstateNotRight { get; set; }
        /// <summary>
        ///Остаточная стоимость объектов недвижимого имущества, по которым не зарегистрировано право, руб.
        ///
        [DetailView(Name = "Остаточная стоимость объектов недвижимого имущества, по которым не зарегистрировано право, руб.", Visible = false, Order = 109, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? ResidualCostRealEstateNotRight { get; set; }
        /// <summary>
        ///Кол-во объектов движимого имущества, шт.
        /// <summary>
        ///
        [DetailView(Name = "Кол-во объектов движимого имущества, шт.", Visible = false, Order = 110, TabName = "[4]Показатели", ReadOnly = true)]
        public int? CountMovableEstate { get; set; }
        /// <summary>
        ///Первоначальная стоимость объектов движимого имущества, руб.
        ///
        [DetailView(Name = "Первоначальная стоимость объектов движимого имущества, руб.", Visible = false, Order = 111, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? InitialCostMovableEstate { get; set; }
        /// <summary>
        ///Остаточная стоимость объектов движимого имущества, руб.
        ///
        [DetailView(Name = "Остаточная стоимость объектов движимого имущества, руб.", Visible = false, Order = 112, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? ResidualCostMovableEstate { get; set; }
        /// <summary>
        ///Кол-во земельных участков на балансе ОГ, шт.
        ///
        [DetailView(Name = "Кол-во земельных участков на балансе ОГ, шт.", Visible = false, Order = 113, TabName = "[4]Показатели", ReadOnly = true)]
        public int? CountLandEstate { get; set; }
        /// <summary>
        ///Первоначальная стоимость земельных участков на балансе ОГ, руб.
        ///
        [DetailView(Name = "Первоначальная стоимость земельных участков на балансе ОГ, руб.", Visible = false, Order = 114, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? InitialCostLandEstate { get; set; }
        /// <summary>
        ///Остаточная стоимость земельных участков на балансе ОГ, руб.
        ///
        [DetailView(Name = "Остаточная стоимость земельных участков на балансе ОГ, руб.", Visible = false, Order = 115, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? ResidualCostLandEstate { get; set; }
        /// <summary>
        ///Кадастровая стоимость земельных участков на балансе ОГ, руб.
        ///
        [DetailView(Name = "Кадастровая стоимость земельных участков на балансе ОГ, руб.", Visible = false, Order = 116, TabName = "[4]Показатели", ReadOnly = true)]
        public decimal? CadastralValueLandEstate { get; set; }
        /// <summary>
        ///Кол-во земельных участков, взятых в аренду, шт.
        ///
        [DetailView(Name = "Кол-во земельных участков, взятых в аренду, шт.", Visible = false, Order = 117, TabName = "[4]Показатели", ReadOnly = true)]
        public int? CountRentalLandEstate { get; set; }




        public override void OnSaving(IUnitOfWork uow, object entry)
        {
            return;
        }

    }
}
