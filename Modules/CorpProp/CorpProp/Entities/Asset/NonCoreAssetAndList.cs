using System;
using System.Collections.Generic;
using System.Linq;
using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Helpers;
using CorpProp.Entities.NSI;
using Base;
using Base.Utils.Common.Attributes;
using Base.Translations;
using System.ComponentModel;
using CorpProp.Entities.Accounting;

namespace CorpProp.Entities.Asset
{
    /// <summary>
    ///     Представляет строку реестра ННА.
    /// </summary>
    /// <remarks>
    ///     Отдельный инвентарный объект в реестре ННА.
    ///     Необходимость введения сущности обусловлена тем, что ряд показателей и этапов,
    ///     представленные далее отдельными классами, могут быть общими для нескольких строк реестра
    ///     (например, одна индикативная оценка на 10 объектов, затем 2 рыночных - на 3 и 7 из них,
    ///     затем мероприятия по реализации - опять общее в отношении всех 10, и т.п.).
    ///     Соответственно, объект "строка реестра ННА" служит для установления таких связей.
    /// </remarks>
    [EnableFullTextSearch]
    public class NonCoreAssetAndList : TypeObject, IManyToManyLeftAssociation<NonCoreAsset>, IManyToManyRightAssociation<NonCoreAssetList>
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса NonCoreAssetAndList.
        /// </summary>
        public NonCoreAssetAndList()
        {
           
        }

        private static readonly CompiledExpression<NonCoreAssetAndList, string> _InventoryNumber =
            DefaultTranslationOf<NonCoreAssetAndList>.Property(x => x.InventoryNumber)
            .Is(x => (x.ObjLeft != null && x.ObjLeft.EstateObject != null) ? x.ObjLeft.EstateObject.InventoryNumber 
            : "");        

        [ListView(Order = 8)]
        [DetailView(Name = "Первоначальная стоимость, руб.", TabName = "Стоимость", ReadOnly = true, Order = 33)]
        [DefaultValue(0)]
        public decimal? InitialCost { get; set; }

        /// <summary>
        /// Получате или задает признак ННА предыдущего периода.
        /// </summary>
        [ListView(Hidden = true)]
        [DefaultValue(false)]
        [DetailView("ННА предыдущего периода", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public bool IsNCAPreviousPeriod { get; set; }

        public int? NonCoreAssetListItemStateID { get; set; }

        
        [ListView]
        [DetailView("Статус", TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]        
        public NonCoreAssetListItemState NonCoreAssetListItemState { get; set; }

        /// <summary>
        ///     Получает или задает ИД предложения по реализации ННА.
        /// </summary>
        public int? NonCoreAssetSaleOfferID { get; set; }

        /// <summary>
        ///     Получает или задает предложение по реализации ННА.
        /// </summary>
        [ListView]
        [DetailView("Предложение по реализации ННА",
            TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public NonCoreAssetSaleOffer Offer { get; set; }
        public int ObjLeftId { get; set; }

        [ListView(Name = "Объект имущества")]
        [DetailView(Visible = false)]
        public NonCoreAsset ObjLeft { get; set; }

        public int ObjRigthId { get; set; }

        [ListView(Name = "Перечень ННА", Hidden = true)]
        [DetailView(Visible = false)]
        public NonCoreAssetList ObjRigth { get; set; }

        public int? NonCoreAssetInventoryID { get; set; }

        [DetailView(Visible = false)]
        public NonCoreAssetInventory NonCoreAssetInventory { get; set; }

        /// <summary>
        /// Остаточная стоимость ННА, руб. (Согласование).
        /// </summary>
        [DetailView(Name = "Остаточная стоимость ННА, руб."
            , TabName = "Стоимость"
            , Group ="Согласовано Куратором"
            , Description = "Стоимость на момент включения Перечня ННА в Реестр ННА")] //Остаточная стоимость ННА, руб. (Согласовано Куратором ДС ЦАУК)
        public decimal? ResidualCostAgreement { get; set; }

        /// <summary>
        /// Дата определения остаточной стоимости (Согласование).
        /// </summary>
        [DetailView(Name = "Дата определения остаточной стоимости"
            , Group = "Согласовано Куратором"
            , TabName = "Стоимость"
            , Description = "Дата определения остаточной стоимости (Согласовано Куратором)")] //Дата определения остаточной стоимости (Согласовано Куратором ДС ЦАУК)
        public DateTime? ResidualCostDateAgreement { get; set; }

        /// <summary>
        /// Остаточная стоимость ННА, руб. (Утверждение).
        /// </summary>
        [DetailView(Name = "Остаточная стоимость ННА, руб."
            , Group = "Утверждение"
            , TabName = "Стоимость"
            , Description = "Стоимость в момент направления Реестра ННА на согласование в орган Управления")]
        public decimal? ResidualCostStatement { get; set; }

        /// <summary>
        /// Дата определения остаточной стоимости (Утверждение).
        /// </summary>
        [DetailView(Name = "Дата определения остаточной стоимости"
            , Group = "Утверждение"
            , TabName = "Стоимость"
        , Description = "Дата определения остаточной стоимости в момент направления Реестра ННА на согласование в орган Управления")]
        public DateTime? ResidualCostDateStatement { get; set; }

        /// <summary>
        /// Стоимость при статусе Реестра ННА в момент направления на согласование Кураторам, руб.
        /// </summary>
        [ListView(Name = "Остаточная стоимость ННА, руб.")]
        [DetailView(Name = "Остаточная стоимость ННА, руб."
            , TabName = "Стоимость"
            , ReadOnly = true
            , Description = "Остаточная стоимость ННА в момент направления на согласование Кураторам, руб.")]
        public decimal? ResidualCostMatching { get; set; }

        /// <summary>
        /// Дата определения остаточной стоимости при статусе Реестра ННА в момент направления на согласование Кураторамю
        /// </summary>
        [ListView(Name = "Дата определения остаточной стоимости")]
        [DetailView(Name = "Дата определения остаточной стоимости", TabName = "Стоимость", ReadOnly = true
            , Description = "Дата определения остаточной стоимости в момент направления на согласование Кураторам")]
        public DateTime? ResidualCostDateMatching { get; set; }

        /// <summary>
        /// Остаточная стоимость ННА, руб.
        /// </summary>
        [ListView(Name = "Остаточная стоимость ННА, руб. (на последнюю отчетную дату)")]
        [DetailView(Name = "Остаточная стоимость ННА, руб."
            , Group = "_"
            , TabName = "Стоимость", ReadOnly = true
            , Description = "Остаточная стоимость ННА, руб. (на последнюю отчетную дату)")]
        public decimal? ResidualCost { get; set; }

        /// <summary>
        /// Дата определения остаточной стоимости.
        /// </summary>
        [ListView(Name = "Дата определения остаточной стоимости")]
        [DetailView(Name = "Дата определения остаточной стоимости", TabName = "Стоимость", ReadOnly = true
            , Description = "Дата определения остаточной стоимости")]
        public DateTime? ResidualCostDate { get; set; }


        /// <summary>
        /// Получает или задает примечание общества группы.
        /// </summary>
        [ListView("Примечание ОГ")]
        [DetailView("Примечание ОГ")]
        [PropertyDataType(PropertyDataType.Text)]
        public String NoticeOG { get; set; }

        /// <summary>
        /// Получает или задает примечание ЦАУК.
        /// </summary>
        [ListView("Примечание ЦАУК")]
        [DetailView("Примечание ЦАУК")]
        [PropertyDataType(PropertyDataType.Text)]
        public String NoticeCauk { get; set; }

        /// <summary>
        /// Остаточная стоимость ННА, руб.
        /// </summary>
        [ListView(Name = "Дата постановки на бухгалтерский учет")]
        [DetailView(Name = "Дата постановки на бухгалтерский учет"
            , Group = "_"
            , TabName = "Стоимость", ReadOnly = false)]        
        public DateTime? BURegDate { get; set; }

        /// <summary>
        /// Инвентарный номер ОИ.
        /// </summary>
        [ListView("Инвентарный номер", Hidden = true)]
        [DetailView("Инвентарный номер", Visible = false)]
        public string InventoryNumber => _InventoryNumber.Evaluate(this);

    }



    public class NonCoreAssetListItemAndNonCoreAssetSaleAccept : ManyToManyAssociation<NonCoreAssetAndList, NonCoreAssetSaleAccept>
    {
    }

 
}
