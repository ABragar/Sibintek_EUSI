using Base.Attributes;
using CorpProp.Helpers;
using SubjectObject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Document;
using CorpProp.Entities.Base;
using Base.Entities.Complex.KLADR;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Asset;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Subject;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Security;
using CorpProp.Entities.NSI;
using Base.Translations;
using CorpProp.Common;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет оценку.
    /// </summary>
    [EnableFullTextSearch]
    public class Appraisal : TypeObject, ISocietal
    {
        private static readonly CompiledExpression<Appraisal, string> _ExecutorFullName =
         DefaultTranslationOf<Appraisal>
            .Property(x => x.ExecutorFullName)
            .Is(x => x.ExecutorLastName + " " + x.ExecutorFirstName + " " + x.ExecutorMiddleName);

        private static readonly CompiledExpression<Appraisal, string> _INNOfSubj =
        DefaultTranslationOf<Appraisal>
           .Property(x => x.INNOfSubj)
           .Is(x => x.Appraiser != null ? x.Appraiser.INN : "");

        private static readonly CompiledExpression<Appraisal, string> _CustomerKindActivity =
        DefaultTranslationOf<Appraisal>
           .Property(x => x.CustomerKindActivity)
           .Is(x => (x.Customer != null && x.Customer.ActualKindActivity != null) ? x.Customer.ActualKindActivity.Name : "");

        private static readonly CompiledExpression<Appraisal, string> _CustomerCountry =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.CustomerCountry)
          .Is(x => (x.Customer != null && x.Customer.Country != null) ? x.Customer.Country.Name : "");

        private static readonly CompiledExpression<Appraisal, string> _CustomerEK =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.CustomerEK)
          .Is(x => (x.Customer != null && x.Customer.ConsolidationUnit != null) ? x.Customer.ConsolidationUnit.Name : "");


        private static readonly CompiledExpression<Appraisal, string> _CustomerDistrict =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.CustomerDistrict)
          .Is(x => (x.Customer != null && x.Customer.FederalDistrict != null) ? x.Customer.FederalDistrict.Name : "");

        private static readonly CompiledExpression<Appraisal, string> _CustomerRegion =
      DefaultTranslationOf<Appraisal>
         .Property(x => x.CustomerRegion)
         .Is(x => (x.Customer != null && x.Customer.Region != null) ? x.Customer.Region.Name : "");


        private static readonly CompiledExpression<Appraisal, bool> _IsSocietyKey =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.IsSocietyKey)
          .Is(x => (x.Customer != null) ? x.Customer.IsSocietyKey : false);


        private static readonly CompiledExpression<Appraisal, bool> _IsSocietyJoint =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.IsSocietyJoint)
          .Is(x => (x.Customer != null) ? x.Customer.IsSocietyJoint : false);



        private static readonly CompiledExpression<Appraisal, bool> _IsSocietyResident =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.IsSocietyResident)
          .Is(x => (x.Customer != null) ? x.Customer.IsSocietyResident : false);



        private static readonly CompiledExpression<Appraisal, bool> _IsSocietyControlled =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.IsSocietyControlled)
          .Is(x => (x.Customer != null) ? x.Customer.IsSocietyControlled : false);

        private static readonly CompiledExpression<Appraisal, SibRegion> _AppRegion =
       DefaultTranslationOf<Appraisal>
          .Property(x => x.AppRegion)
          .Is(x => (x.Appraiser != null) ? x.Appraiser.Region : null);

        /// <summary>
        /// Инициализирует новый экземпляр класса Appraisal.
        /// </summary>
        public Appraisal(): base()
        {
          
            
           
        }

        /// <summary>
        /// Получает субъект РФ оценочной организации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView("Субъект РФ", Hidden = true)]
        [DetailView("Субъект РФ", Visible = false, Description ="Субъект РФ (оценочная организация)")]       
        public SibRegion AppRegion => _AppRegion.Evaluate(this);

        /// <summary>
        /// Получает или задает фамилию исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView(Name = "Фамилия", Order = 1, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorLastName { get; set; }
      
        /// <summary>
        /// Получает или задает идентификатор общества группы.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }

        /// <summary>
        /// Получает или задает общества группы.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "ОГ заказчик оценки", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Исполнитель")]      
        public virtual SubjectObject.Society Customer { get; set; }
         
        /// <summary>
        /// Получает или задает наименование структурного подразделения
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView(Name = "Структурное подразделение", Order = 3, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorDeptName { get; set; }
      
        /// <summary>
        /// Получает или задает имя исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView(Name = "Имя", Order = 4, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorFirstName { get; set; }

        /// <summary>
        /// Получает или задает моб.телефон исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Моб. телефон", Order = 5, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorMobile { get; set; }


        /// <summary>
        /// Получает или задает телефон исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Телефон", Order = 6, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorPhone { get; set; }

        /// <summary>
        /// Получает или задает отчество исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView(Name = "Отчество", Order = 7, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorMiddleName { get; set; }
         
        /// <summary>
        /// Получает или задает ИД исполнителя.
        /// </summary>
        [SystemProperty]
        public int? ExecutorID { get; set; }

        /// <summary>
        /// Получает или задает профиль исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Ф.И.О. исполнителя", Order = 8, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        public virtual SibUser Executor { get; set; }

        
        /// <summary>
        /// Получает или задает Email исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Email", Order = 9, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorEmail { get; set; }

        /// <summary>
        /// Получает или задает должность исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Должность", Order = 10, TabName = CaptionHelper.DefaultTabName, Group = "Исполнитель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorPostName { get; set; }

        /// <summary>
        /// Получает ФИО исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Ф.И.О. исполнителя (строка)", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorFullName => _ExecutorFullName.Evaluate(this);
            
       
        /// <summary>
        /// Получает или задает  объект оценки.
        /// </summary>
        
        [ListView]
        [DetailView(Name = "Объект оценки", Order = 10, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Объект оценки")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AprObject { get; set; }
        
        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает регион оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Субъект РФ (оценки)", Order = 11, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Объект оценки")]
        public SibRegion SibRegion { get; set; }

        /// <summary>
        /// Получает или задает дата оценки.
        /// </summary>
        [ListView (Order =1)]
        [DetailView(Name = "Дата оценки", Order = 12, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Объект оценки")]        
        public DateTime AppraisalDate { get; set; }



        /// <summary>
        /// Получает или задает ИД типа оценки.
        /// </summary>
        /// 

        public int? AppTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип объекта оценки.
        /// </summary>
        [ListView(Visible = true)]
        [DetailView(Name = "Тип объекта оценки", Order = 13, TabName = CaptionHelper.DefaultTabName, Visible = true, Group = "Объект оценки")]
        public virtual AppType AppType { get; set; }


        /// <summary>
        /// Получает остаточная стоимость по РСБУ, руб.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Балансовая стоимость по РСБУ, руб.", Order = 14, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Group = "Объект оценки")]
        [PropertyDataType("Sib_Decimal2")]
        public decimal MarketAppraisalCost { get; set; }

        

                
        /// <summary>
        /// Получает или задает идентификатор общества группы.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [ForeignKey("Owner")]
        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает или задает ДП.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Балансодержатель актива", Order = 16, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Объект оценки")]
        public virtual SubjectObject.Subject Owner { get; set; }

        /// <summary>
        /// Получает стоимость
        /// </summary>
        [ListView]
        [DetailView(Name = "Стоимость объекта оценки", Order = 17, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Group = "Объект оценки")]
        [PropertyDataType("Sib_Decimal2")]
        public decimal EstateAppraisalCost { get; set; }

        /// <summary>
        /// Получает или задает Балансовую стоимость.
        /// </summary>
        [ListView]
        [DetailView(Name = "Балансовая стоимость", Order = 18, TabName = CaptionHelper.DefaultTabName, Group = "Объект оценки")]
        [PropertyDataType("Sib_Decimal2")]
        public decimal BalanceCost { get; set; }


        /// <summary>
        /// Получает или задает краткое описание всех объектов оценки.
        /// </summary>

        [ListView (Visible =false)]
        [DetailView(Name = "Краткое описание всех объектов оценки", TabName = CaptionHelper.DefaultTabName, Required = false, Visible = false)]
        public string ShortDescriptionObjects { get; set; }
        
        /// <summary>
        /// Получает или задает номер отчёта.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Номер отчёта", Order = 21, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Отчет")]
        public string ReportNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД валюты  стоимости объекта оценки..
        /// </summary>
        public int? CurrencyID { get; set; }

        /// <summary>
        /// Получает или задает валюту стоимости объекта оценки.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Валюта", TabName = CaptionHelper.DefaultTabName, Group = "Объект оценки",Visible = false)]
        public virtual Currency Currency { get; set; }
  

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Стоимость услуг, с НДС, руб.", Order = 19, TabName = CaptionHelper.DefaultTabName, Group = "Отчет")]
        [PropertyDataType("Sib_Decimal2")]
        public decimal CostWithVAT { get; set; }

        /// <summary>
        /// Получает или задает цель оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Цель оценки", Order = 20, TabName = CaptionHelper.DefaultTabName, Group = "Отчет")]
        public virtual AppraisalGoal AppraisalGoal { get; set; }

        public int? AppraisalPurposeID { get; set; }
        /// <summary>
        /// Получает или задает назначение оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Назначение оценки", Order = 25, TabName = CaptionHelper.DefaultTabName, Group = "Отчет")]
        public virtual AppraisalPurpose AppraisalPurpose { get; set; }


        /// <summary>
        /// Получает или задает  НДС, руб. стоимости услуг
        /// </summary>

        [FullTextSearchProperty]
        [DetailView(Name = "НДС, руб.", Order = 22, TabName = CaptionHelper.DefaultTabName, Group = "Отчет")]
        [PropertyDataType("Sib_Decimal2")]
        public decimal CostVAT { get; set; }

        /// <summary>
        /// Получает или задает ИД делового партнера (оценщика).
        /// </summary>
        public int? AppraiserID { get; set; }

        /// <summary>
        /// Получает или задает делового партнера (оценщика).
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Оценочная организация", Order = 23, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Отчет")]
        public virtual Appraiser Appraiser { get; set; }

        /// <summary>
        /// Получает или задает оценщика.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Ф.И.О. оценщика", Order = 27, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Отчет")]
        [PropertyDataType(PropertyDataType.Text)]
        public string АppraiserPerson { get; set; }

        /// <summary>
        /// Получает или задает дата отчёта.
        /// </summary>
        [ListView(Order = 2)]
        [DetailView(Name = "Дата отчёта", Order = 24, TabName = CaptionHelper.DefaultTabName, Required = true, Group = "Отчет")]        
        public DateTime ReportDate { get; set; }
      
        /// <summary>
        /// Получает ИНН оценочной оргинизации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "ИНН оценочной организации", Order = 26, TabName = CaptionHelper.DefaultTabName, Group = "Отчет")]
        [PropertyDataType(PropertyDataType.Text)]
        public string INNOfSubj => _INNOfSubj.Evaluate(this);
       
        
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Фактический вид деятельности",TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CustomerKindActivity => _CustomerKindActivity.Evaluate(this);

        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Страна", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CustomerCountry => _CustomerCountry.Evaluate(this);

        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "ЕК", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CustomerEK => _CustomerEK.Evaluate(this);

        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Федеральный округ", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CustomerDistrict => _CustomerDistrict.Evaluate(this);

        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Регион", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CustomerRegion => _CustomerRegion.Evaluate(this);

               

        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Ключевое ОГ", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public bool IsSocietyKey => _IsSocietyKey.Evaluate(this);

        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Совместное предприятие", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public bool IsSocietyJoint => _IsSocietyJoint.Evaluate(this);

        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Резидент", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public bool IsSocietyResident => _IsSocietyResident.Evaluate(this);


        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Подконтрольное", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public bool IsSocietyControlled => _IsSocietyControlled.Evaluate(this);



        
        #region Подходы к оценке. Должен быть выбран хотябы один подход
        //Бывает три подхода: доходный, затратный, сравнительный. В каждой отдельной оценке могут использоваться любые 1, 2 или все 3 вместе.
        //Предлагается сделать самым простым способом: присвоить каждому подходу значения 0x1, 0x10 и 0x100 и суммировать использованные.
        //Можно вместо этого три boolean атрибута "использован ли подход ___" или как-то ещё - это уже техника.
        /// <summary>
        /// Получает или задает подход к оценке (кодовое обозначение).
        /// </summary>
        /// <remarks>
        /// Хранится вычесленное значение кода комбинации выбранных подходов:
        ///доходный      (Д): 100
        ///затратный     (З): 010
        ///сравнительный (С): 001
        ///Комбинации:
        ///Д   :100
        ///ДЗ  :110
        ///ДЗС :111
        ///ДС  :101
        ///З   :010
        ///ЗС  :011
        ///С   :001
        /// </remarks>
        [ListView(Name = "Подход (Кодовое обозначение)",Hidden = true, Visible = false)]        
        public string ApproachCode { get; set; }

        [ListView(Name = "Подход", Hidden = true)]       
        public string Approach { get; set; }

        ///// <summary>
        ///// Получает или задает подход к оценке: доходный.
        ///// </summary>
        //[ListView]
        //[DetailView(Name = "Подход: доходный", Order = 10, TabName = CaptionHelper.DefaultTabName)]
        //[DefaultValue(false)]
        //public bool ApproachProfitable { get; set; }

        ///// <summary>
        ///// Получает или задает подход к оценке: затратный.
        ///// </summary>
        //[ListView]
        //[DetailView(Name = "Подход: затратный", Order = 11, TabName = CaptionHelper.DefaultTabName)]
        //[DefaultValue(false)]
        //public bool ApproachCostly { get; set; }

        ///// <summary>
        ///// Получает или задает подход к оценке: сравнительный.
        ///// </summary>
        //[ListView]
        //[DetailView(Name = "Подход: сравнительный", Order = 12, TabName = CaptionHelper.DefaultTabName)]
        //[DefaultValue(false)]
        //public bool ApproachComparative { get; set; }
        #endregion



        /// <summary>
        /// Получает ИД сделки.
        /// </summary>
        public int? DealID { get; set; }

        /// <summary>
        /// Получает сделку.
        /// </summary>
        [DetailView(Name = "Договор оценки", Order = 29, TabName = CaptionHelper.DefaultTabName/*, Required = true*/, Group = "Отчет")]
        public virtual SibDeal Deal { get; set; }

      
        /// <summary>
        /// Получает или задает признак согласования платежа для проведения оценки.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Платёж за проведенную оценку", TabName = CaptionHelper.DefaultTabName, Group = "Стоимость услуг за проведение оценки",Visible =false)]     
        [DefaultValue(false)]
        public bool AgreedPay { get; set; }

        /// <summary>
        /// Получает или задает признак прокверки отчета.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Акцепт ДС", Order = 31, TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета")]
        [DefaultValue(false)]
        public bool ReportCheck { get; set; }

       

       /// <summary>
        ///  Получает или задает Комментарий к оценке.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Примечание к оценке", Order = 34, TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета")]
        public string Description { get; set; }
      
       

       
        

        public int? SocietyID { get; set; }


        /// <summary>
        /// Получает или задает признак оценки ННА.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Оценка ННА", Order = 18, TabName = CaptionHelper.DefaultTabName, Group = "Отчет")]
        [DefaultValue(false)]
        public bool AppraisalNNA { get; set; }

        /// <summary>
        /// Получает или задает номер поручения об оценке.
        /// </summary>
        [ListView(Order =100)]
        [DetailView(Name = "Номер поручения об оценке", Order = 32, TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaskNumber { get; set; }

        /// <summary>
        /// Получает или задает дату поручения об оценке.
        /// </summary>
        [ListView(Order = 101)]
        [DetailView(Name = "Дата поручения об оценке", Order = 36, TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета")]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? TaskDate { get; set; }

        /// <summary>
        /// Получает или задает дату поручения об оценке.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Дата согласования курирующим КСП", Order = 40, TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета")]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? AcceptDeptDate { get; set; }

        /// <summary>
        /// Получает или задает дату поручения об оценке.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Дата согласования ДС ЦАУК", Order = 44, TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета")]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? AcceptCAUKDate { get; set; }

        

        /// <summary>
        /// Получает или задает Лист согласования в обществе группы.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView(Name = "Лист согласования в обществе группы", Order = 33
            , TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета", Required = true)]
        public virtual FileCard FileAcceptInSociety { get; set; }

        

        /// <summary>
        /// Получает или задает Подтверждающий документ о согласовании с КСП.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView(Name = "Документ о согласовании с КСП"
            ,Description = "Подтверждающий документ о согласовании с КСП"
            ,Order = 37, TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета")]
        public virtual FileCard FileAcceptDept { get; set; }
       

        /// <summary>
        /// Получает или задает ИД карточки документа.
        /// </summary>
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает карточку документа.
        /// </summary>
        /// <remarks>
        /// Подтверждающий документ.
        /// </remarks>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Документ о согласовании ДС ЦАУК", Order = 41
            ,Description = "Подтверждающий документ о согласовании ДС ЦАУК"
            , TabName = CaptionHelper.DefaultTabName, Group = "{4}Проверка отчета", Required = true)]
        public virtual FileCard FileCard { get; set; }

       
    }
}
