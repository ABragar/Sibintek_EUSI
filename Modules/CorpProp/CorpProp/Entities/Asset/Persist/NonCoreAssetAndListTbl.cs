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
    public class NonCoreAssetAndListTbl : TypeObject, IManyToManyLeftAssociation<NonCoreAsset>, IManyToManyRightAssociation<NonCoreAssetList>
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса NonCoreAssetAndList.
        /// </summary>
        public NonCoreAssetAndListTbl()
        {
            
        }       
       

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
        [DetailView(Name = "Остаточная стоимость ННА, руб. (Согласование)", TabName = "Стоимость")]
        public decimal? ResidualCostAgreement { get; set; }

        /// <summary>
        /// Дата определения остаточной стоимости (Согласование).
        /// </summary>
        [DetailView(Name = "Дата определения остаточной стоимости (Согласование)", TabName = "Стоимость")]
        public DateTime? ResidualCostDateAgreement { get; set; }

        /// <summary>
        /// Остаточная стоимость ННА, руб. (Утверждение).
        /// </summary>
        [DetailView(Name = "Остаточная стоимость ННА, руб. (Утверждение)", TabName = "Стоимость")]
        public decimal? ResidualCostStatement { get; set; }

        /// <summary>
        /// Дата определения остаточной стоимости (Утверждение).
        /// </summary>
        [DetailView(Name = "Дата определения остаточной стоимости (Утверждение)", TabName = "Стоимость")]
        public DateTime? ResidualCostDateStatement { get; set; }
        
        /// <summary>
        /// Стоимость при статусе Реестра ННА в момент направления на согласование Кураторам, руб.
        /// </summary>
        [ListView(Name = "Стоимость при статусе Реестра ННА в момент направления на согласование Кураторам, руб.")]
        [DetailView(Name = "Стоимость при статусе Реестра ННА в момент направления на согласование Кураторам, руб.", TabName = "Стоимость", ReadOnly = true)]
        public decimal? ResidualCostMatching { get; set; }

        /// <summary>
        /// Дата определения остаточной стоимости при статусе Реестра ННА в момент направления на согласование Кураторамю
        /// </summary>
        [ListView(Name = "Дата определения остаточной стоимости при статусе Реестра ННА в момент направления на согласование Кураторам")]
        [DetailView(Name = "Дата определения остаточной стоимости при статусе Реестра ННА в момент направления на согласование Кураторам", TabName = "Стоимость", ReadOnly = true)]
        public DateTime? ResidualCostDateMatching { get; set; }

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
            , TabName = "Стоимость", ReadOnly = true)]
        public DateTime? BURegDate { get; set; }
    }

   

   
}
