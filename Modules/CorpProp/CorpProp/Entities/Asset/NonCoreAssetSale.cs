using Base.Attributes;
using Base.Translations;
using CorpProp.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Asset
{

    /// <summary>
    /// Представляет реализацию ННА.
    /// </summary>
    [EnableFullTextSearch]
    public class NonCoreAssetSale : TypeObject
    {

        #region Вкладки ННА
        /// <summary>
        /// ННА, вкладка Общие данные.
        /// </summary>
        public const string TabName0 = CaptionHelper.DefaultTabName;
        /// <summary>
        /// ННА, вкладка Корп мероприятия в ОГ.
        /// </summary>
        public const string TabName1 = "[1]Корп мероприятия в ОГ";
        /// <summary>
        /// ННА, вкладка Реализация имущества на торговой площадке.
        /// </summary>
        public const string TabName2 = "[2]Реализация имущества на торговой площадке";
        /// <summary>
        /// ННА, вкладка Выбытие имущества.
        /// </summary>
        public const string TabName3 = "[3]Выбытие имущества";
        /// <summary>
        /// ННА, вкладка Ссылки.
        /// </summary>
        public const string TabName5 = "[5]Ссылки";
        #endregion


        private static readonly CompiledExpression<NonCoreAssetSale, string> _NameAssetOwner =
          DefaultTranslationOf<NonCoreAssetSale>.Property(x => x.NameAssetOwner).Is(x => (x.NonCoreAsset != null) ? x.NonCoreAsset.AssetOwnerName : "");

        private static readonly CompiledExpression<NonCoreAssetSale, string> _Name =
          DefaultTranslationOf<NonCoreAssetSale>.Property(x => x.Name).Is(x => (x.NonCoreAsset != null) ? x.NonCoreAsset.NameAsset : "");

        private static readonly CompiledExpression<NonCoreAssetSale, string> _LocationAssetText =
          DefaultTranslationOf<NonCoreAssetSale>.Property(x => x.LocationAssetText).Is(x => (x.NonCoreAsset != null) ? x.NonCoreAsset.LocationAssetText : "");


        #region Конструктор
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetSale.
        /// </summary>
        public NonCoreAssetSale()
        {
        }
        #endregion

        #region Общие данные
        /// <summary>
        ///     Получает или задает ИД типа ННА.
        /// </summary>
        public int? NonCoreAssetID { get; set; }


        /// <summary>
        ///     Получает или задает тип ННА.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("ННА", TabName = TabName0)]
        public virtual NonCoreAsset NonCoreAsset { get; set; }

        [NotMapped]
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Собственник актива", TabName = TabName0)]
        public string NameAssetOwner => _NameAssetOwner.Evaluate(this);

        [NotMapped]
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Наименование имущества", TabName = TabName0)]
        public string Name => _Name.Evaluate(this);

        /// <summary>
        /// Получает или задает Кадастровый/условный номер/номер имущества (указывается по недвижимому имуществу) .
        /// </summary>
        //[FullTextSearchProperty] 
        ////[ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Кадастровый/условный номер/номер имущества", Description = "Кадастровый/условный номер/номер имущества (указывается по недвижимому имуществу)", TabName = TabName0)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralNumber { get; set; }
        #endregion

        #region Корп мероприятия в ОГ
        [NotMapped]
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Местонахождение Актива", TabName = TabName1)]
        public string LocationAssetText => _LocationAssetText.Evaluate(this);

        /// <summary>
        /// Получает или задает Наименование Органа Управления Общества, принявшего решение об отчуждении имущества.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Наименование Органа Управления Общества", Description = "Наименование Органа Управления Общества, принявшего решение об отчуждении имущества", TabName = TabName1)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameManagementCompany { get; set; }
        #endregion

        #region Реализация имущества на торговой площадке
        /// <summary>
        /// Получает или задает Реквизиты договора с организатором торгов.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Реквизиты договора с организатором торгов", Description = "Реквизиты договора(№, дата) с организатором торгов", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RequisitesDealOrganizerTrades { get; set; }

        /// <summary>
        /// Получает или задает Дата размещения имущества на торговых площадках.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Дата размещения имущества на торговых площадках", Description = "Дата размещения имущества на торговых площадках", TabName = TabName2)]
        public DateTime? DatePlacementPropertyInTradingFloors { get; set; }

        /// <summary>
        /// Получает или задает Номер этапа тендера.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Номер этапа тендера", TabName = TabName2)]
        public int? NumberStageTender { get; set; }
        

        /// <summary>
        /// Получает или задает Дата завершения приема заявок.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Дата завершения приема заявок", TabName = TabName2)]
        public DateTime? DateCompletionReceiptApplication { get; set; }


        /// <summary>
        /// Получает или задает Информация о количестве поданных заявок.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Информация о количестве поданных заявок", TabName = TabName2)]
        public int? NumberApplicationsSubmitted { get; set; }
        


        /// <summary>
        /// Получает или задает Дата проведения тендера.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Дата проведения тендера", TabName = TabName2)]
        public DateTime? DateOfPlacementTrading  { get; set; }

        /// <summary>
        /// Получает или задает Начальная цена реализации имущества/актива в руб.
        /// </summary>
        [ListView(Order = 11)]
        [DetailView(Name = "Начальная цена реализации имущества/актива руб.", TabName = TabName2)]
        public decimal? InitialSellingPrice { get; set; }

        /// <summary>
        /// Получает или задает Цена реализации имущества/актива без НДС в руб.
        /// </summary>
        [ListView(Order = 11)]
        [DetailView(Name = "Цена реализации имущества/актива по итогам тендера без НДС руб.", TabName = TabName2)]
        public decimal? SellingPriceWithoutVAT { get; set; }

        /// <summary>
        /// Получает или задает Цена реализации имущества/актива с НДС в руб.
        /// </summary>
        [ListView(Order = 11)]
        [DetailView(Name = "Цена реализации имущества/актива по итогам тендера с НДС руб.", TabName = TabName2)]
        public decimal? SellingPriceIncludingVAT { get; set; }

        /// <summary>
        /// Получает или задает признак Тендер состоялся/не состоялся.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Тендер состоялся/не состоялся", TabName = TabName2)]
        [DefaultValue(false)]
        public bool InConservation { get; set; }
        #endregion


        #region Выбытие имущества
        /// <summary>
        /// Получает или задает Балансовая (остаточная) стоимость отчуждаемого имущества на момент совершения сделки в руб.
        /// </summary>
        
        [ListView(Order = 12)]
        [DetailView(Name = "Балансовая (остаточная) стоимость отчуждаемого имущества на момент совершения сделки, руб.",
        TabName = TabName3)]
        public decimal? ResidualCostOfAlienatedPropertyAtSelling { get; set; }

        /// <summary>
        /// Получает или задает ИД сделки.
        /// </summary>
        [ForeignKey("SibDeal")]
        public int? SibDealID { get; set; }
        /// <summary>
        /// Получает или задает сделку.
        /// </summary>
        [DetailView("Сделка", TabName = TabName3)]
        [ListView(Hidden = true)]
        public virtual SibDeal SibDeal { get; set; }

        /// <summary>
        /// Получает или задает Реквизиты Сделки(№, дата).
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Реквизиты Сделки", Description = "Реквизиты Сделки(№, дата)", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FullNameDeal { get; set; }

        /// <summary>
        /// Получает или задает Дата подписания договора.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Дата подписания договора", TabName = TabName3)]
        public DateTime? DateSignDeal { get; set; }

        /// <summary>
        /// Получает или задает Дата подписания акта приема-передачи.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Дата подписания акта приема-передачи", TabName = TabName3)]
        public DateTime? DateSignActAcceptance { get; set; }

        /// <summary>
        /// Получает или задает Дата перехода права собственности.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Дата перехода права собственности", TabName = TabName3)]
        public DateTime? DateCancelRight { get; set; }

        /// <summary>
        /// Получает или задает Реквизиты Сделки(№, дата).
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "Наименование организации/покупателя", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameBuyerAsset { get; set; }

                /// <summary>
        /// Получает или задает ИД способа реализации.
        /// </summary>
        public int? ImplementationWayID { get; set; }


        /// <summary>
        /// Получает или задает способ реализации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Способ реализации", Required = true,
        TabName = TabName3)]
        public virtual ImplementationWay ImplementationWay { get; set; }

        /// <summary>
        /// Получает или задает дату реализации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Дата реализации", Required = true,
        TabName = TabName3)]
        public DateTime ImplementDate { get; set; }

        /// <summary>
        /// Получает или задает цену реализации без НДС.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Цена реализации, руб, без НДС", Required = true,
        TabName = TabName3)]
        [SibDisplayFormat("c")]
        public decimal PriceWithoutVAT { get; set; }

        /// <summary>
        /// Получает или задает цену реализации с НДС.
        /// </summary>       
        [DetailView("Цена реализации, руб, с НДС", Required = true,
        TabName = TabName3)]
        [ListView(Hidden = true)]
        [SibDisplayFormat("c")]
        public decimal PriceWithVAT { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView("Примечание",
        TabName = TabName3)]
        [ListView(Hidden = true)]
        public String Description { get; set; }

        /// <summary>
        ///     Получает или задает ИД статуса ННА.
        /// </summary>
        public int? NonCoreAssetStatusID { get; set; }

        /// <summary>
        /// Получает или задает Статус.
        /// </summary>
        [ListView(Order = 2)]
        [DetailView(Name = "Статус",
        TabName = TabName1)]
        public virtual NonCoreAssetSaleStatus NonCoreAssetSaleStatus { get; set; }
        #endregion

        /// <summary>
        /// Получает или задает ИД документа.
        /// </summary>      
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает документ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Документ", TabName = TabName3)]
        public virtual FileCard FileCard { get; set; }

        #region Ссылки
        #endregion
    }
}
