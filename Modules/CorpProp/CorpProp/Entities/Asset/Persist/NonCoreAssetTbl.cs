using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Translations;
using CorpProp.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using Base;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Law;
using Base.DAL;
using System.Linq;

namespace CorpProp.Entities.Asset
{
    /// <summary>
    /// Представляет непрофильный или неэффективный актив.
    /// </summary>
    /// <remarks>
    /// Представляет инвентарный объект, предлагающийся к признанию,
    /// признанный или бывший признанным непрофильным или неэффективным (ННА).
    /// </remarks>
    [EnableFullTextSearch]
    public class NonCoreAssetTbl : TypeObject
    {

        /// <summary>
        ///     Инициализирует новый экземпляр класса NonCoreAsset.
        /// </summary>
        public NonCoreAssetTbl()
        {
            
        }      

        #region Характеристики актива


        public int? EstateObjectID { get; set; }

        /// <summary>
        ///     Связанный объект имущества.
        /// </summary>
        
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Объект имущества")]
        [ForeignKey("EstateObjectID")]
        public Estate.Estate EstateObject { get; set; }

        /// <summary>
        ///     Получает или задает ИД типа ННА.
        /// </summary>
        public int? NonCoreAssetTypeID { get; set; }


        /// <summary>
        ///     Получает или задает тип ННА.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Тип ННА")]
        public NonCoreAssetType NonCoreAssetType { get; set; }

        /// <summary>
        ///     Получает или задает ИД статуса ННА.
        /// </summary>
        public int? NonCoreAssetStatusID { get; set; }

        /// <summary>
        ///     Получает или задает Статус.
        /// </summary>
        [ListView(Order = 2)]
        [DetailView(Name = "Статус")]
        public NonCoreAssetStatus NonCoreAssetStatus { get; set; }

        [SystemProperty]
        public int? AssetOwnerID { get; set; }

        /// <summary>
        ///     Получает или задает балансодержателя Актива.
        /// </summary>
        [ListView(Order = 1)]
        [DetailView("Балансодержатель Актива")]
        public Society AssetOwner { get; set; }

        [SystemProperty]
        public int? AssetMainOwnerID { get; set; }
        /// <summary>
        ///     Получает или задает собственника Актива.
        /// </summary>
        [ListView(Order = 1)]
        [DetailView("Cобственник Актива")]
        public Society AssetMainOwner { get; set; }

        public int? AssetUserID { get; set; }
        /// <summary>
        /// Получает или задает пользователя Актива.
        /// </summary>
        [ListView(Order = 1)]
        [DetailView("Пользователь Актива")]
        public Society AssetUser { get; set; }

        /// <summary>
        ///     Получает или задает наименование собственника Актива.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView("Собственник Актива (Наименование)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string AssetOwnerName { get; set; }

        ///// <summary>
        /////     Получает или задает наименование Актива/Комплекса.
        ///// </summary>
        //[ListView(Order = 2)]
        //[DetailView("Наименование Актива/Комплекса",
        //    Description = "Наименование Актива/Комплекса(производственная база, нефтебаза, АЗС и т.д.)",
        //    TabName = TabName1)]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string NameAssetOrComplex { get; set; }

        /// <summary>
        ///     Получает или задает наименование Актива/Комплекса.
        /// </summary>
        [ListView(Order = 2, Visible = false)]
        [DetailView("Наименование актива",
            Description = "Наименование актива признанного ННА", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NonCoreAssetName { get; set; }

        /// <summary>
        ///     Получает или задает наименование Актива/Комплекса.
        /// </summary>
        [ListView(Order = 3)]
        [DetailView("Наименование комплекса",
            Description = "Наименование имущественного комплекса (производственная база, нефтебаза, АЗС и т.д.)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NonCoreAssetNameComplex { get; set; }

        [SystemProperty]
        public int? ImplementationWayID { get; set; }

        /// <summary>
        /// Получает или задает способ реализации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Способ реализации")]
        public ImplementationWay ImplementationWay { get; set; }


        /// <summary>
        ///     Получает или задает наименование недвижимости, входящей в состав комплекса.
        /// </summary>
        [ListView(Order = 4)]
        [DetailView("Наименование по ЕГРН",
            Description =
                "Наименование недвижимости, входящей в состав комплекса в соответствии с правоустанавливающими документами (в соответствии с утвержденным реестром)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NnamePropertyPartComplex { get; set; }

        /// <summary>
        ///     Получает или задает Наименование ОС.
        /// </summary>
        [ListView(Order = 5)]
        [DetailView("Наименование ОС (по ОБУ)", Description = "Наименование ОС в соответствии с БУ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameAsset { get; set; }

        /// <summary>
        ///     Получает или задает инвентарный номер.
        /// </summary>
        [ListView(Order = 6)]
        [DetailView(Name = "Инвентарный номер по БУ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }
               

        /// <summary>
        ///     Получает или задает Местонахождение Актива.
        /// </summary>
        [ListView(Order = 7)]
        [DetailView(Name = "Местонахождение Актива")]
        public string LocationAssetText { get; set; }

        /// <summary>
        ///     Получает или задает Характеристики объекта (площадь, протяженность).
        /// </summary>
        [ListView(Order = 8)]
        [DetailView(Name = "Характеристики объекта", Description =
                "Характеристики объекта (площадь, протяженность)")]

        public string PropertyCharacteristics { get; set; }


        /// <summary>
        ///     Получает или задает Ограничение / обременение.
        /// </summary>
        [ListView(Order = 9)]
        [DetailView(Name = "Ограничение / обременение")]
        public string EncumbranceText { get; set; }

        public int? NonCoreAssetOwnerCategoryID { get; set; }

        [DetailView(Name = "Категория балансодержателя актива")]
        public NonCoreAssetOwnerCategory NonCoreAssetOwnerCategory { get; set; }

        /// <summary>
        ///     Получает или задает Курирующий ВП.
        /// </summary>
        [ListView(Order = 10)]
        [DetailView(Name = "Курирующий ВП")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Supervising { get; set; }

        #endregion

        #region Финансовая информация

        /// <summary>
        ///     Получает или задает первоначальную стоимость в руб.
        /// </summary>
        
        [ListView(Order = 11)]
        [DetailView(Name = "Первоначальная стоимость, руб.")]
        public decimal? InitialCost { get; set; }

        /// <summary>
        ///     Получает или задает балансовая (остаточная) стоимость в руб.
        /// </summary>
        
        [ListView(Order = 12)]
        [DetailView(Name = "Балансовая (остаточная) стоимость, руб.")]
        public decimal? ResidualCost { get; set; }

        /// <summary>
        ///     Получает или задает годовая выручка от использования без учета НДС.
        /// </summary>
        
        [ListView(Order = 13)]
        [DetailView(Name = "Годовая выручка от использования без учета НДС",
            Description = "Годовая выручка без учета НДС")]
        public decimal? AnnualRevenueFromUseWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает годовые затраты на содержание актива без учета НДС.
        /// </summary>
        
        [ListView(Order = 14)]
        [DetailView(Name = "Годовые затраты без учета НДС",
            Description = "Годовые затраты на содержание актива без учета НДС")]
        public decimal? AnnualCostContentWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает индикативная оценка без учета НДС.
        /// </summary>
        
        [ListView(Order = 15)]
        [DetailView(Name = "Индикативная оценка без учета НДС", Required = true)]
        public decimal? IndicativeValuationWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает индикативная оценка с НДС.
        /// </summary>
        
        [ListView(Order = 16)]
        [DetailView(Name = "Индикативная оценка с НДС", Required = true)]
        public decimal? IndicativeValuationIncludingVAT { get; set; }

        
        [ListView(Order = 17)]
        [DetailView(Name = "Индикативная оценка - НДС", Required = true)]
        public decimal? IndicativeVAT { get; set; }

        /// <summary>
        ///     Получает или задает рыночная оценка Актива без учета НДС.
        ///     <remarc>При заполнении этого поля - Атрибуту "Индикативная оценка без учета НДС" - присвоить значение "0.00"</remarc>
        /// </summary>
        
        [ListView(Order = 18)]
        [DetailView(Name = "Рыночная оценка Актива без учета НДС")]
        public decimal? MarketValuationWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает рыночная оценка Актива с учетом НДС.
        ///     <remarc>При заполнении этого поля - Атрибуту "Индикативная оценка с НДС" - присвоить значение "0.00"</remarc>
        /// </summary>
        
        [ListView(Order = 19)]
        [DetailView(Name = "Рыночная оценка Актива с учетом НДС")]
        public decimal? MarketValuationIncludingVAT { get; set; }

        
        [ListView(Order = 20)]
        [DetailView(Name = "Рыночная оценка Актива - НДС")]
        public decimal? MarketValuationVAT { get; set; }

        #endregion

        #region Предложения и обоснование

        [DetailView(Name = "Иные объекты, помимо указанных в направляемом на согласование Списке ННА, в границах земельного участка отсутствуют"
            , Description = "Рабочей группой по работе с ННА проведен осмотр предлагаемого к отчуждению актива. Иные объекты, помимо указанных в направляемом на согласование Списке ННА, в границах земельного участка на котором расположен актив, отсутствуют", Required = true)]
        public bool NoOtherObjectsOnLand { get; set; }

        /// <summary>
        ///     Получает или задает предлагаемые действия (процедуры) Способ реализации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Предлагаемые действия (процедуры)",
            Description = "Предлагаемые действия (процедуры) Способ реализации")]
        public string ProposedActionsMethodImplementation { get; set; }
              

        #region Бюджет процедуры реализации

        [DetailView(Name = "Расходы на публикацию", Group = "Бюджет процедуры реализации")]
        public decimal? PublicationExpense { get; set; }

        [DetailView(Name = "Расходы по оценке", Group = "Бюджет процедуры реализации")]
        public decimal? AppraisalExpense { get; set; }

        [DetailView(Name = "Вознаграждение организатору торгов", Group = "Бюджет процедуры реализации")]
        public decimal? BiddingOrganizersBenefits { get; set; }

        [DetailView(Name = "Балансовая (ост.) стоимость ННА",
        Description = "Балансовая (остаточная) стоимость ННА на дату реализации",
        Group = "Бюджет процедуры реализации")]
        public decimal? SellingResidualCost { get; set; }

        [DetailView(Name = "Прочие расходы", Group = "Бюджет процедуры реализации")]
        public decimal? OtherExpenses { get; set; }

        #endregion

        /// <summary>
        ///     Получает или задает Прогнозный срок организации мероприятий по реализации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Срок орг. мероприятий по реализации",
            Description = "Прогнозный срок организации мероприятий по реализации")]
        public DateTime? ForecastPeriod { get; set; }

        /// <summary>
        ///     Получает или задает Обоснование предложений.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Обоснование предложений")]
        public string RationaleProposals { get; set; }

        /// <summary>
        ///     Получает или задает примечание.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Примечание")]
        public string Description { get; set; }

        #endregion

        #region Критерии отнесения к ННА

        [DetailView(Name = "Несоответствие стратегии развития Компании")]
        public bool StrategicDiscrepancy { get; set; }

        [DetailView(Name = "Осуществление непрофильных видов деятельности",
            Description =
                "Осуществление объектом КС и/или с использованием объекта КС непрофильных видов деятельности, то есть не связанных с бизнес-направлениями деятельности и основными управленческими процессами Компании"
            )]
        public bool NonCoreBusiness { get; set; }

        [DetailView(Name = "Нецелесообразность инвестиций",
            Description =
                "Нецелесообразность для Компании инвестиций в развитие определенных непрофильных и/или неэффективных видов деятельности"
            )]
        public bool InexpedientInvestment { get; set; }

        [DetailView(Name = "Сохранение конкурентных преимуществ при отсутствии объекта в портфеле активов",
            Description =
                "Сохранение конкурентных преимуществ Компании при отсутствии объекта КС в портфеле активов Компании"
            )]
        public bool PreservationOfCompetitiveAdvantages { get; set; }

        [DetailView(Name = "Отсутствие стратегических интересов",
            Description =
                "Отсутствие стратегических интересов Компании в регионе присутствия/ местонахождения объекта КС")]
        public bool LackOfStrategicGoals { get; set; }

        [DetailView(Name = "Ожидание рыночной цены за объект выше, чем экономический эффект от инвестиций",
            Description =
                "Ожидание рыночной цены за объект КС выше, чем экономический эффект от инвестиций Компании в развитие соответствующего объекта КС"
            )]
        public bool WorthSelling { get; set; }

        [DetailView(Name = "Необходимость отвлечения управленческих ресурсов Компании",
            Description =
                "Необходимость отвлечения управленческих ресурсов Компании для осуществления контроля над деятельностью объекта КС, несопоставимых с уровнем доходности объекта КС"
            )]
        public bool InexpedientMaintence { get; set; }

        [DetailView(Name = "Негативные экономические и репутационные последствиям",
            Description =
                "Сохранение объекта в собственности/ аренде в ОАО «НК «Роснефть» или Обществах Группы может привести к существенным негативным экономическим и репутационным последствиям"
            )]
        public bool ReputationLoss { get; set; }

        /// <summary>
        ///     Получает или задает специальные критерии отнесения к ННА.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Специальные критерии отнесения к ННА", Required = true)]
        public string SpecialNonCoreAssetCriteria { get; set; }


        /// <summary>
        /// Получает или задает номер поручения.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Номер поручения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaskNumber { get; set; }

        /// <summary>
        /// Получает или задает дату поручения.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Дата поручения")]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? TaskDate { get; set; }

        /// <summary>
        /// Получает или задает дату списания.
        /// </summary>        
        [ListView(Order = 100)]
        [DetailView(Name = "Дата списания", ReadOnly = true)]
        public DateTime? LeavingDate { get; set; }

        public int? AppraisalAssignmentID { get; set; }

        /// <summary>
        /// Поручение об оценке.
        /// </summary>
        [DetailView(Name = "Поручение об оценке")]
        public FileCard AppraisalAssignment { get; set; }


        public int? LeavingReasonID { get; set; }

        /// <summary>
        /// Получает или задает причину выбытия.
        /// </summary>
        [ListView(Order = 101)]
        [DetailView(Name = "Причина списания", ReadOnly = true)]
        public LeavingReason LeavingReason { get; set; }
        

        #endregion
        
     

        #region Ссылки       

        public int? FormDocumentID { get; set; }

        [DetailView(Name = "Анкета ННА", Required = true)]
        public FileCard FormDocument { get; set; }

        public int? JustificationOfAuthorityDocumentID { get; set; }

        [DetailView(Name = "Осн. полномочий Комиссии от ОГ", Description = "Cкан-копия документа о назначении комиссии, которая утверждают анкету", Required = true)]
        public FileCard JustificationOfAuthorityDocument { get; set; }

        public int? ApprovedListDocumentID { get; set; }

        [DetailView(Name = "Копия переченя ННА", Description = "Копия переченя ННА, утвержденного Куратором", Required = true)]
        public FileCard ApprovedListDocument { get; set; }

        public int? WorkingGroupConclusionDocumentID { get; set; }

        [DetailView(Name = "Копия заключения рабочей группы", Description = "Копия заключения рабочей группы по проведённому осмотру предпалагаемого к отчуждению актива (отнесение к ННА)", Required = true)]
        public FileCard WorkingGroupConclusionDocument { get; set; }

       
        #endregion

        /// <summary>
        /// Переопределяет OnSaving.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="entry">Запись.</param>
        public override void OnSaving(IUnitOfWork uow, object entry)
        {
            base.OnSaving(uow, entry);

        }
    }


}
