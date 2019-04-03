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
using System.ComponentModel;

namespace CorpProp.Entities.Asset
{
    /// <summary>
    ///  Представление для непрофильного или неэффективного актива.
    /// </summary>  
    [EnableFullTextSearch]    
    public class NonCoreAsset : TypeObject
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса NonCoreAsset.
        /// </summary>
        public NonCoreAsset()
        {
            
        }

        #region Вкладки ННА

        /// <summary>
        ///     ННА, вкладка Характеристики актива.
        /// </summary>
        public const string TabName1 = "[1]Характеристики актива";

        /// <summary>
        ///     ННА, вкладка Характеристики актива.
        /// </summary>
        public const string TabName2 = "[2]Финансовая информация";

        /// <summary>
        ///     ННА, вкладка Характеристики актива.
        /// </summary>
        public const string TabName3 = "[3]Предложения и обоснование";

        public const string TabName4 = "[4]Критерии отнесения к ННА";

        /// <summary>
        ///     ННА, вкладка Перечни ННА.
        /// </summary>
        public const string TabName5 = "[5]Перечни ННА";

        /// <summary>
        ///     ННА, вкладка Реализация ННА.
        /// </summary>
        public const string TabName6 = "[6]Реализация ННА";

        public const string TabName7 = "[7]Бюджет процедуры реализации";

        /// <summary>
        ///     ННА, вкладка Ссылки.
        /// </summary>
        public const string TabName8 = "[8]Документы";

        #endregion

        #region Характеристики актива

        [ListView(Visible = false)]
        [DetailView(Visible = false, ReadOnly = true)]
        public string Name { get; set; }

        public int? EstateObjectID { get; set; }

        /// <summary>
        ///     Связанный объект имущества.
        /// </summary>
        
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Объект имущества", TabName = TabName1)]
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
        [DetailView("Тип ННА",
            TabName = TabName1)]
        public NonCoreAssetType NonCoreAssetType { get; set; }

        /// <summary>
        ///     Получает или задает ИД статуса ННА.
        /// </summary>
        public int? NonCoreAssetStatusID { get; set; }

        /// <summary>
        ///     Получает или задает Статус.
        /// </summary>
        [ListView(Order = 2)]
        [DetailView(Name = "Статус",
            TabName = TabName1)]
        public NonCoreAssetStatus NonCoreAssetStatus { get; set; }

        [SystemProperty]
        public int? AssetOwnerID { get; set; }

        /// <summary>
        ///     Получает или задает балансодержателя Актива.
        /// </summary>
        [ListView(Order = 1)]
        [DetailView("Балансодержатель Актива", TabName = TabName1)]
        public Society AssetOwner { get; set; }

        [SystemProperty]
        public int? AssetMainOwnerID { get; set; }
        /// <summary>
        ///     Получает или задает собственника Актива.
        /// </summary>
        [ListView(Order = 1)]
        [DetailView("Cобственник Актива", TabName = TabName1)]
        public Society AssetMainOwner { get; set; }

        public int? AssetUserID { get; set; }
        /// <summary>
        /// Получает или задает пользователя Актива.
        /// </summary>
        [ListView(Order = 1)]
        [DetailView("Пользователь Актива", TabName = TabName1)]
        public Society AssetUser { get; set; }

        /// <summary>
        ///     Получает или задает наименование собственника Актива.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView("Собственник Актива (Наименование)", TabName = TabName1, Visible = false)]
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
        [ListView(Order = 2,Visible = false)]
        [DetailView("Наименование актива",
            Description = "Наименование актива признанного ННА",
            TabName = TabName1, Visible = false)            ]
        [PropertyDataType(PropertyDataType.Text)]
        public string NonCoreAssetName { get; set; }

        /// <summary>
        ///     Получает или задает наименование Актива/Комплекса.
        /// </summary>
        [ListView(Order = 3)]
        [DetailView("Наименование комплекса",
            Description = "Наименование имущественного комплекса (производственная база, нефтебаза, АЗС и т.д.)",
            TabName = TabName1)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NonCoreAssetNameComplex { get; set; }

        [SystemProperty]
        public int? ImplementationWayID { get; set; }

        /// <summary>
        /// Получает или задает способ реализации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Способ реализации", 
        TabName = TabName3)]
        public ImplementationWay ImplementationWay { get; set; }


        /// <summary>
        ///     Получает или задает наименование недвижимости, входящей в состав комплекса.
        /// </summary>
        [ListView(Order = 4)]
        [DetailView("Наименование по ЕГРН",
            Description =
                "Наименование недвижимости, входящей в состав комплекса в соответствии с правоустанавливающими документами (в соответствии с утвержденным реестром)",
            TabName = TabName1,Required =true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NnamePropertyPartComplex { get; set; }

        /// <summary>
        ///     Получает или задает Наименование ОС.
        /// </summary>
        [ListView(Order = 5)]
        [DetailView("Наименование ОС (по ОБУ)", Description = "Наименование ОС в соответствии с БУ",
            TabName = TabName1,Required =true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameAsset { get; set; }

        /// <summary>
        ///     Получает или задает инвентарный номер.
        /// </summary>
        [ListView(Order = 6)]
        [DetailView(Name = "Инвентарный номер по БУ",
            TabName = TabName1,Required =true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        [FullTextSearchProperty]
        [DetailView(Name = "Кадастровый номер", TabName = TabName1, ReadOnly = true)]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает Местонахождение Актива.
        /// </summary>
        [ListView(Order = 7)]
        [DetailView(Name = "Местонахождение Актива", TabName = TabName1,Required =true)]
        public string LocationAssetText { get; set; }

        /// <summary>
        ///     Получает или задает Характеристики объекта (площадь, протяженность).
        /// </summary>
        [ListView(Order = 8)]
        [DetailView(Name = "Характеристики объекта", Description =
                "Характеристики объекта (площадь, протяженность)",
            TabName = TabName1,Required =true)]
        
        public string PropertyCharacteristics { get; set; }


        /// <summary>
        ///     Получает или задает Ограничение / обременение.
        /// </summary>
        [ListView(Order = 9)]
        [DetailView(Name = "Ограничение / обременение",
            TabName = TabName1,Required =true)]
        public string EncumbranceText { get; set; }

        public int? NonCoreAssetOwnerCategoryID { get; set; }

        [DetailView(Name = "Категория балансодержателя актива", TabName = TabName1)]
        public NonCoreAssetOwnerCategory NonCoreAssetOwnerCategory { get; set; }

        /// <summary>
        ///     Получает или задает Курирующий ВП.
        /// </summary>
        [ListView(Order = 10)]
        [DetailView(Name = "Курирующий ВП",
            TabName = TabName1)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Supervising { get; set; }

        #endregion

        #region Финансовая информация

        /// <summary>
        ///     Получает или задает первоначальную стоимость в руб.
        /// </summary>
        
        [ListView(Order = 11)]
        [DetailView(Name = "Первоначальная стоимость, руб.",
            TabName = TabName2)]
        public decimal? InitialCost { get; set; }

        /// <summary>
        ///     Получает или задает балансовая (остаточная) стоимость в руб.
        /// </summary>
        
        [ListView(Order = 12)]
        [DetailView(Name = "Балансовая (остаточная) стоимость, руб.",
            TabName = TabName2)]
        public decimal? ResidualCost { get; set; }

        /// <summary>
        ///     Получает или задает годовая выручка от использования без учета НДС.
        /// </summary>
        
        [ListView(Order = 13)]
        [DetailView(Name = "Годовая выручка от использования без учета НДС, руб.", 
            Description = "Годовая выручка без учета НДС, руб.",
            TabName = TabName2)]
        public decimal? AnnualRevenueFromUseWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает годовые затраты на содержание актива без учета НДС.
        /// </summary>
        
        [ListView(Order = 14)]
        [DetailView(Name = "Годовые затраты без учета НДС, руб.",
            Description = "Годовые затраты на содержание актива без учета НДС, руб.",
            TabName = TabName2)]
        public decimal? AnnualCostContentWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает индикативная оценка без учета НДС.
        /// </summary>
        
        [ListView(Order = 15)]
        [DetailView(Name = "Индикативная оценка без учета НДС, руб.",
            TabName = TabName2, Required = true)]
        public decimal? IndicativeValuationWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает индикативная оценка с НДС.
        /// </summary>
        
        [ListView(Order = 16)]
        [DetailView(Name = "Индикативная оценка с НДС, руб.",
            TabName = TabName2, Required = true)]
        public decimal? IndicativeValuationIncludingVAT { get; set; }

        
        [ListView(Order = 17)]
        [DetailView(Name = "Индикативная оценка - НДС, руб.",
            TabName = TabName2, Required = true)]
        public decimal? IndicativeVAT { get; set; }

        /// <summary>
        ///     Получает или задает рыночная оценка Актива без учета НДС.
        ///     <remarc>При заполнении этого поля - Атрибуту "Индикативная оценка без учета НДС" - присвоить значение "0.00"</remarc>
        /// </summary>
        
        [ListView(Order = 18)]
        [DetailView(Name = "Рыночная оценка Актива без учета НДС, руб.",
            TabName = TabName2)]
        public decimal? MarketValuationWithoutVAT { get; set; }

        /// <summary>
        ///     Получает или задает рыночная оценка Актива с учетом НДС.
        ///     <remarc>При заполнении этого поля - Атрибуту "Индикативная оценка с НДС" - присвоить значение "0.00"</remarc>
        /// </summary>
        
        [ListView(Order = 19)]
        [DetailView(Name = "Рыночная оценка Актива с учетом НДС, руб.",
            TabName = TabName2)]
        public decimal? MarketValuationIncludingVAT { get; set; }

        
        [ListView(Order = 20)]
        [DetailView(Name = "Рыночная оценка Актива - НДС, руб.",
            TabName = TabName2)]
        public decimal? MarketValuationVAT { get; set; }

        #endregion

        #region Предложения и обоснование

        [DetailView(Name = "Иные объекты, помимо указанных в направляемом на согласование Списке ННА, в границах земельного участка отсутствуют"
            , Description = "Рабочей группой по работе с ННА проведен осмотр предлагаемого к отчуждению актива. Иные объекты, помимо указанных в направляемом на согласование Списке ННА, в границах земельного участка на котором расположен актив, отсутствуют"
            , TabName = TabName3)]
        [DefaultValue(false)]
        public bool NoOtherObjectsOnLand { get; set; }

        /// <summary>
        ///     Получает или задает предлагаемые действия (процедуры) Способ реализации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Предлагаемые действия (процедуры)",
            Description = "Предлагаемые действия (процедуры) Способ реализации",
            TabName = TabName3)]
        public string ProposedActionsMethodImplementation { get; set; }

        /// <summary>
        /// Получает или задает Бюджет предполагаемой процедуры.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Бюджет предполагаемой процедуры, руб",
            TabName = TabName3, ReadOnly = true)]
        public decimal? BudgetProposedProcedure { get; set; }

        #region Бюджет процедуры реализации

        [DetailView(Name = "Расходы на публикацию, руб", TabName = TabName3, Group = "Бюджет процедуры реализации")]
        public decimal? PublicationExpense { get; set; }

        [DetailView(Name = "Расходы по оценке, руб", TabName = TabName3, Group = "Бюджет процедуры реализации")]
        public decimal? AppraisalExpense { get; set; }

        [DetailView(Name = "Вознаграждение организатору торгов, руб", TabName = TabName3, Group = "Бюджет процедуры реализации")]
        public decimal? BiddingOrganizersBenefits { get; set; }

        [DetailView(Name = "Балансовая (ост.) стоимость ННА, руб", 
        Description = "Балансовая (остаточная) стоимость ННА на дату реализации",
        TabName = TabName3, Group = "Бюджет процедуры реализации")]
        public decimal? SellingResidualCost { get; set; }

        [DetailView(Name = "Прочие расходы, руб", TabName = TabName3, Group = "Бюджет процедуры реализации")]
        public decimal? OtherExpenses { get; set; }

        #endregion
        
        /// <summary>
        ///     Получает или задает Прогнозный срок организации мероприятий по реализации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Срок орг. мероприятий по реализации",
            Description = "Прогнозный срок организации мероприятий по реализации",
            TabName = TabName3)]
        public DateTime? ForecastPeriod { get; set; }

        /// <summary>
        ///     Получает или задает Обоснование предложений.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Обоснование предложений",
            TabName = TabName3)]
        public string RationaleProposals { get; set; }

        /// <summary>
        ///     Получает или задает примечание.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Примечание",
            TabName = TabName3)]
        public string Description { get; set; }

        #endregion

        #region Критерии отнесения к ННА
        [SystemProperty]
        [DetailView(Name = "Несоответствие стратегии развития Компании", TabName = TabName4)]
        public bool StrategicDiscrepancy { get; set; }
        [SystemProperty]
        [DetailView(Name = "Осуществление непрофильных видов деятельности",
            Description =
                "Осуществление объектом КС и/или с использованием объекта КС непрофильных видов деятельности, то есть не связанных с бизнес-направлениями деятельности и основными управленческими процессами Компании",
            TabName = TabName4)]
        public bool NonCoreBusiness { get; set; }
        [SystemProperty]
        [DetailView(Name = "Нецелесообразность инвестиций",
            Description =
                "Нецелесообразность для Компании инвестиций в развитие определенных непрофильных и/или неэффективных видов деятельности",
            TabName = TabName4)]
        public bool InexpedientInvestment { get; set; }
        [SystemProperty]
        [DetailView(Name = "Сохранение конкурентных преимуществ при отсутствии объекта в портфеле активов",
            Description =
                "Сохранение конкурентных преимуществ Компании при отсутствии объекта КС в портфеле активов Компании",
            TabName = TabName4)]
        public bool PreservationOfCompetitiveAdvantages { get; set; }
        [SystemProperty]
        [DetailView(Name = "Отсутствие стратегических интересов",
            Description =
                "Отсутствие стратегических интересов Компании в регионе присутствия/ местонахождения объекта КС", TabName = TabName4)]
        public bool LackOfStrategicGoals { get; set; }
        [SystemProperty]
        [DetailView(Name = "Ожидание рыночной цены за объект выше, чем экономический эффект от инвестиций",
            Description =
                "Ожидание рыночной цены за объект КС выше, чем экономический эффект от инвестиций Компании в развитие соответствующего объекта КС",
            TabName = TabName4)]
        public bool WorthSelling { get; set; }
        [SystemProperty]
        [DetailView(Name = "Необходимость отвлечения управленческих ресурсов Компании",
            Description =
                "Необходимость отвлечения управленческих ресурсов Компании для осуществления контроля над деятельностью объекта КС, несопоставимых с уровнем доходности объекта КС",
            TabName = TabName4)]
        public bool InexpedientMaintence { get; set; }
        [SystemProperty]
        [DetailView(Name = "Негативные экономические и репутационные последствиям",
            Description =
                "Сохранение объекта в собственности/ аренде в ОАО «НК «Роснефть» или Обществах Группы может привести к существенным негативным экономическим и репутационным последствиям",
            TabName = TabName4)]
        public bool ReputationLoss { get; set; }
        
        /// <summary>
        ///     Получает или задает специальные критерии отнесения к ННА.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Специальные критерии отнесения к ННА",
            TabName = TabName4,Required = true)]
        public string SpecialNonCoreAssetCriteria { get; set; }


        /// <summary>
        /// Получает или задает номер поручения.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Номер поручения (об оценке)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaskNumber { get; set; }

        /// <summary>
        /// Получает или задает дату поручения.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Дата поручения (об оценке)")]
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
        [DetailView(Name = "Причина списания",ReadOnly = true)]
        public LeavingReason LeavingReason { get; set; }

       

        /// <summary>
        /// Получает или задает номер записи ЕГРН.
        /// </summary>
        [ListView(Order = 102)]
        [DetailView(Name = "№ записи ЕГРН", ReadOnly = true)]
        public string EGRNNumber { get; set; }


        #endregion


        #region Ссылки       

        public int? FormDocumentID { get; set; }

        [DetailView(Name = "Анкета ННА", Required = true, TabName = TabName8)]
        public FileCard FormDocument { get; set; }

        public int? JustificationOfAuthorityDocumentID { get; set; }

        [DetailView(Name = "Осн. полномочий Комиссии от ОГ", Description = "Cкан-копия документа о назначении комиссии, которая утверждают анкету", Required = true, TabName = TabName8)]
        public FileCard JustificationOfAuthorityDocument { get; set; }

        public int? ApprovedListDocumentID { get; set; }

        [DetailView(Name = "Копия переченя ННА", Description = "Копия переченя ННА, утвержденного Куратором", Required = true, TabName = TabName8)]
        public FileCard ApprovedListDocument { get; set; }

        public int? WorkingGroupConclusionDocumentID { get; set; }

        [DetailView(Name = "Копия заключения рабочей группы", Description = "Копия заключения рабочей группы по проведённому осмотру предполагаемого к отчуждению актива (отнесение к ННА)", Required = true, TabName = TabName8)]
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

            var estateID = this.EstateObject?.ID;
            if (estateID != null)
            {
                var estateObject = uow.GetRepository<CorpProp.Entities.Estate.Estate>().Filter(x => x.ID == estateID).FirstOrDefault();
                if (estateObject != null)
                {
                    //InitialCost = estateObject.InitialCost;

                    if (!estateObject.IsNonCoreAsset)
                        estateObject.IsNonCoreAsset = true;

                    var ownerID = estateObject?.Owner?.ID;                    
                    this.AssetMainOwnerID = estateObject?.Owner?.ID;

                    if (ownerID != null)
                    {
                        var owner = uow.GetRepository<CorpProp.Entities.Subject.Society>()
                            .Filter(x => x.ID == ownerID)
                            .FirstOrDefault();

                        AssetOwner = owner;
                        AssetOwnerName = owner?.ShortName;                         
                    }

                    if (string.IsNullOrEmpty(NonCoreAssetName))
                        NonCoreAssetName = estateObject.Name;
                    //Получаем наименование ИК
                    if (string.IsNullOrEmpty(NonCoreAssetNameComplex))
                    {
                        var elementInventory = uow.GetRepository<CorpProp.Entities.Estate.InventoryObject>()
                            .Filter(x => x.ID == estateObject.ID).FirstOrDefault();


                        if (elementInventory != null)
                        {
                            if (elementInventory.PropertyComplexID != null || elementInventory.PropertyComplexID != 0)
                            {
                                var estateComplex = uow.GetRepository<CorpProp.Entities.Estate.PropertyComplex>().Filter(x => x.ID == elementInventory.PropertyComplexID).FirstOrDefault();
                                if (estateComplex != null)
                                    NonCoreAssetNameComplex = (estateComplex != null) ? estateComplex.Name : "";
                            }
                        }
                    }

                }
            }

        }

        /// <summary>
        /// Копирует данные из объекта old в текущий экземпляр.
        /// </summary>
        /// <param name="old"></param>
        public void CopyFrom(NonCoreAsset nna)
        {
            AnnualCostContentWithoutVAT = nna.AnnualCostContentWithoutVAT;
            AnnualRevenueFromUseWithoutVAT = nna.AnnualRevenueFromUseWithoutVAT;
            AppraisalAssignmentID = nna.AppraisalAssignmentID;
            AppraisalExpense = nna.AppraisalExpense;
            ApprovedListDocumentID = nna.ApprovedListDocumentID;
            AssetMainOwnerID = nna.AssetMainOwnerID;
            AssetOwnerID = nna.AssetOwnerID;
            AssetOwnerName = nna.AssetOwnerName;
            AssetUserID = nna.AssetUserID;
            BiddingOrganizersBenefits = nna.BiddingOrganizersBenefits;
            BudgetProposedProcedure = nna.BudgetProposedProcedure;
            CadastralNumber = nna.CadastralNumber;
            Description = nna.Description;
            EGRNNumber = nna.EGRNNumber;
            EncumbranceText = nna.EncumbranceText;
            EstateObjectID = nna.EstateObjectID;
            ForecastPeriod = nna.ForecastPeriod;
            FormDocumentID = nna.FormDocumentID;
            ImplementationWayID = nna.ImplementationWayID;
            IndicativeValuationIncludingVAT = nna.IndicativeValuationIncludingVAT;
            IndicativeValuationWithoutVAT = nna.IndicativeValuationWithoutVAT;
            IndicativeVAT = nna.IndicativeVAT;
            InexpedientInvestment = nna.InexpedientInvestment;
            InexpedientMaintence = nna.InexpedientMaintence;
            InitialCost = nna.InitialCost;
            InventoryNumber = nna.InventoryNumber;
            JustificationOfAuthorityDocumentID = nna.JustificationOfAuthorityDocumentID;
            LackOfStrategicGoals = nna.LackOfStrategicGoals;
            LeavingDate = nna.LeavingDate;
            LeavingReasonID = nna.LeavingReasonID;
            LocationAssetText = nna.LocationAssetText;
            MarketValuationIncludingVAT = nna.MarketValuationIncludingVAT;
            MarketValuationVAT = nna.MarketValuationVAT;
            MarketValuationWithoutVAT = nna.MarketValuationWithoutVAT;
            Name = nna.Name;
            NameAsset = nna.NameAsset;
            NnamePropertyPartComplex = nna.NnamePropertyPartComplex;
            NonCoreAssetName = nna.NonCoreAssetName;
            NonCoreAssetNameComplex = nna.NonCoreAssetNameComplex;
            NonCoreAssetOwnerCategoryID = nna.NonCoreAssetOwnerCategoryID;
            NonCoreAssetStatusID = nna.NonCoreAssetStatusID;
            NonCoreAssetTypeID = nna.NonCoreAssetTypeID;
            NonCoreBusiness = nna.NonCoreBusiness;
            NoOtherObjectsOnLand = nna.NoOtherObjectsOnLand;
            OtherExpenses = nna.OtherExpenses;
            PreservationOfCompetitiveAdvantages = nna.PreservationOfCompetitiveAdvantages;
            PropertyCharacteristics = nna.PropertyCharacteristics;
            ProposedActionsMethodImplementation = nna.ProposedActionsMethodImplementation;
            PublicationExpense = nna.PublicationExpense;
            RationaleProposals = nna.RationaleProposals;
            ReputationLoss = nna.ReputationLoss;
            ResidualCost = nna.ResidualCost;
            SellingResidualCost = nna.SellingResidualCost;
            SpecialNonCoreAssetCriteria = nna.SpecialNonCoreAssetCriteria;
            StrategicDiscrepancy = nna.StrategicDiscrepancy;
            Supervising = nna.Supervising;
            TaskDate = nna.TaskDate;
            TaskNumber = nna.TaskNumber;
            WorkingGroupConclusionDocumentID = nna.WorkingGroupConclusionDocumentID;
            WorthSelling = nna.WorthSelling;
        }
    }

   
}
