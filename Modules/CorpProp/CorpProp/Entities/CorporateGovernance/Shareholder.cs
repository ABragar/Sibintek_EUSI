using Base.Attributes;
using CorpProp.Helpers;
using SubjectObject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;
using CorpProp.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет сведения об Акционере / Участнике.
    /// </summary>
    [EnableFullTextSearch]
    public class Shareholder : TypeObject
    {
        private static readonly CompiledExpression<Shareholder, string> _IDEUPShareholder =
           DefaultTranslationOf<Shareholder>.Property(x => x.IDEUPShareholder).Is(x => (x.SocietyShareholder != null)? x.SocietyShareholder.IDEUP : "" );

        private static readonly CompiledExpression<Shareholder, string> _IDEUPRecipient =
          DefaultTranslationOf<Shareholder>.Property(x => x.IDEUPRecipient).Is(x => (x.SocietyRecipient != null) ? x.SocietyRecipient.IDEUP : "");


        /// <summary>
        /// Инициализирует новый экземпляр класса Shareholder.
        /// </summary>
        public Shareholder(): base()
        {

        }
        /// <summary>
        /// Получает или задает ИД ЕУП акционера.
        /// </summary>
        [DetailView(Name = "ИД ЕУП акционера", Order = 1, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDEUPShareholder => _IDEUPShareholder.Evaluate(this);

        /// <summary>
        /// Получает или задает дату начала участия.
        /// </summary>
        [DetailView(Name = "Дата начала участия", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [ListView]
        public DateTime DateFrom { get; set; }


        /// <summary>
        /// Получает или задает долю прямого участия.
        /// </summary>
        [ListView()]
        [DetailView(Name = "Доля прямого участия", Order = 3, TabName = CaptionHelper.DefaultTabName)]
        //[PropertyDataType("Sib_Decimal8")]
        [DecimalPrecision(18, 10)]
        public decimal? ShareMarket { get; set; }

        /// <summary>
        /// Получает или задает долю прямого участия в голосующих акциях.
        /// </summary>
        [ListView()]
        [DetailView(Name = "Доля прямого участия в голосующих акциях", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        //[PropertyDataType("Sib_Decimal8")]
        [DecimalPrecision(18, 10)]
        public decimal? ShareMarketInvest { get; set; }

        /// <summary>
        /// Получает или задает дату окончания участия.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата окончания участия", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Получает или задает количество обыкновенных акций / долей.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Количество обыкновенных акций / долей", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        //[PropertyDataType("Sib_Decimal4")]
        [DecimalPrecision(28, 4)]
        public decimal? NumberOrdinaryShares { get; set; }

        /// <summary>
        /// Получает или задает количество привилегированных акций / долей.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Количество привилегированных акций / долей", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        //[PropertyDataType("Sib_Decimal4")]
        public decimal NumberPreferredShares { get; set; }

        /// <summary>
        /// Получает или задает стоимость номинальная обыкновенных акций / долей.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Стоимость номинальная обыкновенных акций / долей", Order = 8, TabName = CaptionHelper.DefaultTabName)]
        [SibDisplayFormat("c")]
        [DecimalPrecision(31, 8)]
        public decimal? CostNominalOrdinaryShares { get; set; }

        /// <summary>
        /// Получает или задает стоимость фактическая обыкновенных акций / долей.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Стоимость фактическая обыкновенных акций / долей", Order = 9, TabName = CaptionHelper.DefaultTabName)]
        [SibDisplayFormat("c")]
        [DecimalPrecision(31, 8)]
        public decimal? CostActualOrdinaryShares { get; set; }

        /// <summary>
        /// Получает или задает стоимость номинальная привилегированных акций / долей.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Стоимость номинальная привилегированных акций / долей", Order = 10, TabName = CaptionHelper.DefaultTabName)]
        [SibDisplayFormat("c")]
        [DecimalPrecision(31, 8)]
        public decimal? CostNominalPreferredShares { get; set; }

        /// <summary>
        /// Получает или задает стоимость фактическая привилегированных акций / долей.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Стоимость фактическая привилегированных акций / долей", Order = 11, TabName = CaptionHelper.DefaultTabName)]
        [SibDisplayFormat("c")]
        [DecimalPrecision(31, 8)]
        public decimal? CostActualPreferredShares { get; set; }

        /// <summary>
        /// Получает или задает ИД инвестора ДФВ.
        /// </summary>
        [SystemProperty]
        public int? SocietyShareholderID { get; set; }

        /// <summary>
        /// Получает или задает инвестора ДФВ.
        /// </summary>
        /// <remarks>
        /// Владелец ДВФ.
        /// </remarks>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Акционер/участник", Order = 12, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SubjectObject.Society SocietyShareholder { get; set; }

        /// <summary>
        /// Получает или задает ИД получателя инвестиций.
        /// </summary>
        [SystemProperty]
        public int? SocietyRecipientID { get; set; }

        /// <summary>
        /// Получает или задает получатель инвестиций.
        /// </summary>
        /// <remarks>
        /// ОГ, в капитал которого внесены ДФВ.
        /// </remarks>  
        [FullTextSearchProperty]
        [ListView]        
        [DetailView(Name = "Юр.лицо", Order = 13, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SubjectObject.Society SocietyRecipient { get; set; }

        /// <summary>
        /// Получает или задает ИД ЕУП юр.лица.
        /// </summary>
        [DetailView(Name = "ИД ЕУП юр.лица", Order = 0, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDEUPRecipient => _IDEUPRecipient.Evaluate(this);
    }
}
