using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectObject = CorpProp.Entities.Subject;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет сводную аналитику индикации оценки активов.
    /// </summary>    
    [EnableFullTextSearch]
    public class IndicateEstateAppraisalView : TypeObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса IndicateEstateAppraisalView.
        /// </summary>
        public IndicateEstateAppraisalView()
        {

        }

        #region Appraisal
        public int? ExecutorID { get; set; }
        /// <summary>
        /// Получает или задает ФИО исполнителя в обществе группы.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Ф.И.О. исполнителя в ОГ", Order = 0, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual SibUser Executor { get; set; }

        /// <summary>
        /// Получает или задает Контакты исполнителя в обществе группы.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Контакты исполнителя в ОГ", Order = 1, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string ExecutorContacts { get; set; }

        /// <summary>
        /// Получает или задает номер отчёта.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Номер отчёта", Order = 2, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string ReportNumber { get; set; }

        /// <summary>
        /// Получает или задает дата отчёта.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата отчёта", Order = 3, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// Получает или задает дата оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата оценки", Order = 4, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public DateTime? AppraisalDate { get; set; }

        /// <summary>
        /// Получает или задает краткое описание всех объектов оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Краткое описание всех объектов оценки", Order = 11, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string ShortDescriptionObjects { get; set; }

        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает регион оценки.
        /// </summary>
        //[ListView]
        [DetailView(Name = "Субъект РФ (оценки)", Order = 5, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public SibRegion SibRegion { get; set; }

        /// <summary>
        /// Получает или задает регион оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Регион оценки", Order = 5, TabName = CaptionHelper.DefaultTabName, Visible = false, ReadOnly = true)]
        public string SibRegionTitle { get; set; }

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
        [ListView(Name = "Подход (Кодовое обозначение)", Visible =false)]
        public string ApproachCode { get; set; }

        [ListView(Name = "Подход")]
        public string Approach { get; set; }

        ///// <summary>
        ///// Получает или задает подход к оценке: доходный.
        ///// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        //[DetailView(Name = "Подход: доходный", Order = 7, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //[DefaultValue(false)]
        //public bool ApproachProfitable { get; set; }

        ///// <summary>
        ///// Получает или задает подход к оценке: затратный.
        ///// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        //[DetailView(Name = "Подход: затратный", Order = 7, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //[DefaultValue(false)]
        //public bool ApproachCostly { get; set; }

        ///// <summary>
        ///// Получает или задает подход к оценке: сравнительный.
        ///// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        //[DetailView(Name = "Подход: сравнительный", Order = 7, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //[DefaultValue(false)]
        //public bool ApproachComparative { get; set; }
        #endregion

        ///// <summary>
        ///// Получает или задает стоимость услуг оценщика.
        ///// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Стоимость услуг оценщика", Order = 7, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //public decimal? Cost { get; set; }

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сумма НДС", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType("Sib_Decimal2")]
        public decimal CostVAT { get; set; }
       

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сумма с НДС", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType("Sib_Decimal2")]
        public decimal CostWithVAT { get; set; }

      
        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Примечание", Order = 9, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string Description { get; set; }

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
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Заказчик", Order = 10, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual SubjectObject.Society Customer { get; set; }


        /// <summary>
        /// Получает или задает общества группы.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Заказчик", Order = 10, TabName = CaptionHelper.DefaultTabName, Visible = false, ReadOnly = true)]
        public string CustomerShortName { get; set; }


        /// <summary>
        /// Получает или задает идентификатор общества группы.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Visible = false, ReadOnly = true)]
        [ForeignKey("Owner")]
        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает или задает общества группы.
        /// </summary>       
        [DetailView(Name = "Владелец объектов", Order = 11, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual SubjectObject.Society Owner { get; set; }

        /// <summary>
        /// Получает или задает общества группы.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Владелец объектов", Order = 11, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        public string OwnerShortName { get; set; }

        /// <summary>
        /// Получает или задает ИД делового партнера (оценщика).
        /// </summary>
        public int? AppraiserID { get; set; }

        /// <summary>
        /// Получает или задает делового партнера (Оценивающую организацию).
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Оценивающая организация", Order = 12, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual Appraiser Appraiser { get; set; }

        /// <summary>
        /// Получает или задает делового партнера (оценщика).
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Оценщик", Order = 12, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public string AppraiserShortName { get; set; }

        #endregion

        #region Estate
        /// <summary>
        ///Получает или задает наименование.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", ReadOnly = true, Order = -1, TabName = CaptionHelper.DefaultTabName)]
        public string EstateNames { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Примечание", Order = 2, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [FullTextSearchProperty]
        public string EstateDescriptions { get; set; }
              

        /// <summary>
        /// Получает или задает класс объекта.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Класс ОИ", ReadOnly = true,
         TabName = CaptionHelper.DefaultTabName)]
        public string EstateTypeNames { get; set; }
        #endregion

        #region EstateAppraisal

        public int? AppraisalTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип оценки.
        /// </summary>
        /// <remarks>
        /// Внутренний справочник: рыночная стоимость объекта, рыночная стоимость права аренды и пр.
        /// </remarks>      
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Тип оценки", Order = 0, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual AppraisalType AppraisalType { get; set; }

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Тип оценки", Order = 0, TabName = CaptionHelper.DefaultTabName, Visible = false, ReadOnly = true)]
        public string AppraisalTypeName { get; set; }

        /// <summary>
        /// Получает или задает краткое описание объекта оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Краткое описание объекта оценки", Order = 1, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string ShortDescriptionObjectAppraisal { get; set; }

        /// <summary>
        /// Получает или задает рыночную стоимость, без НДС.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Рыночная стоимость, без НДС", Order = 2, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public decimal? MarketPriceWithoutVAT { get; set; }


        #endregion




    }
}
