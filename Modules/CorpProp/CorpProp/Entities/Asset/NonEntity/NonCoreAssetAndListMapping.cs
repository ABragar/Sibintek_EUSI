using System;
using System.ComponentModel;
using System.Reflection;
using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;

namespace CorpProp.Entities.Asset.NonEntity
{
    ///// <summary>
    /////     Представляет строку реестра ННА (проекция от <see cref="NonCoreAssetAndList"/>).
    ///// </summary>
    ///// <remarks>
    /////     Отдельный инвентарный объект в реестре ННА.
    /////     Необходимость введения сущности обусловлена тем, что ряд показателей и этапов,
    /////     представленные далее отдельными классами, могут быть общими для нескольких строк реестра
    /////     (например, одна индикативная оценка на 10 объектов, затем 2 рыночных - на 3 и 7 из них,
    /////     затем мероприятия по реализации - опять общее в отношении всех 10, и т.п.).
    /////     Соответственно, объект "строка реестра ННА" служит для установления таких связей.
    ///// </remarks>
    //[EnableFullTextSearch]
    //public class NonCoreAssetAndListMapping: TypeObject, IManyToManyLeftAssociation<NonCoreAsset>, IManyToManyRightAssociation<NonCoreAssetList>
    //{
    //    /// <summary>
    //    ///     Инициализирует новый экземпляр класса NonCoreAssetAndListMapping.
    //    /// </summary>
    //    public NonCoreAssetAndListMapping()
    //    {
    //    }


    //    #region Поля проецируемые из NonCoreAssetAndList


    //    ///// <summary>
    //    /////     Получает или задает ИД ННА.
    //    ///// </summary>
    //    //[System.ComponentModel.DataAnnotations.Schema.Index("IX_UniquNonCoreAsset", 1, IsUnique = true)]
    //    //public int? NonCoreAssetID { get; set; }

    //    ///// <summary>
    //    /////     Получает или задает ННА.
    //    ///// </summary>
    //    //[DetailView("Непрофильный / неэффективный актив", Required = true,
    //    //    TabName = CaptionHelper.DefaultTabName)]
    //    //[FullTextSearchProperty]
    //    //[ListView]
    //    //public virtual NonCoreAsset NonCoreAsset { get; set; }

    //    ///// <summary>
    //    /////     Получает или задает ИД реестра ННА.
    //    ///// </summary>
    //    //[System.ComponentModel.DataAnnotations.Schema.Index("IX_UniquNonCoreAsset", 2, IsUnique = true)]
    //    //public int? NonCoreAssetListID { get; set; }

    //    ///// <summary>
    //    /////     Получает или задает реестр ННА.
    //    ///// </summary>
    //    //[ListView(Hidden = true)]
    //    //[DetailView("Перечень ННА",
    //    //    TabName = CaptionHelper.DefaultTabName)]
    //    //public virtual NonCoreAssetList NonCoreAssetList { get; set; }


    //    [ListView(Hidden = true)]
    //    [DefaultValue(false)]
    //    [DetailView("ННА предыдущего периода", TabName = CaptionHelper.DefaultTabName, Visible = false)]
    //    public bool IsNCAPreviousPeriod { get; set; }

    //    public int? NonCoreAssetListItemStateID { get; set; }


    //    [ListView]
    //    [DetailView("Статус", TabName = CaptionHelper.DefaultTabName)]
    //    [FullTextSearchProperty]
    //    public virtual NonCoreAssetListItemState NonCoreAssetListItemState { get; set; }

    //    /// <summary>
    //    ///     Получает или задает ИД предложения по реализации ННА.
    //    /// </summary>
    //    public int? NonCoreAssetSaleOfferID { get; set; }

    //    /// <summary>
    //    ///     Получает или задает предложение по реализации ННА.
    //    /// </summary>
    //    [ListView]
    //    [DetailView("Предложение по реализации ННА",
    //        TabName = CaptionHelper.DefaultTabName)]
    //    [FullTextSearchProperty]
    //    public virtual NonCoreAssetSaleOffer Offer { get; set; }
    //    public int ObjLeftId { get; set; }

    //    [ListView(Name = "Объект имущества")]
    //    [DetailView(Visible = false)]
    //    public NonCoreAsset ObjLeft { get; set; }

    //    public int ObjRigthId { get; set; }

    //    [ListView(Name = "Перечень ННА", Hidden = true)]
    //    [DetailView(Visible = false)]
    //    public NonCoreAssetList ObjRigth { get; set; }

    //    public int? NonCoreAssetInventoryID { get; set; }

    //    [DetailView(Visible = false)]
    //    public NonCoreAssetInventory NonCoreAssetInventory { get; set; }

    //    /// <summary>
    //    /// Остаточная стоимость ННА, руб. (Согласование).
    //    /// </summary>
    //    [DetailView(Name = "Остаточная стоимость ННА, руб. (Согласование)", TabName = AccountingObject.TabName2)]
    //    public decimal? ResidualCostAgreement { get; set; }

    //    /// <summary>
    //    /// Дата определения остаточной стоимости (Согласование).
    //    /// </summary>
    //    [DetailView(Name = "Дата определения остаточной стоимости (Согласование)", TabName = AccountingObject.TabName2)]
    //    public DateTime? ResidualCostDateAgreement { get; set; }

    //    /// <summary>
    //    /// Остаточная стоимость ННА, руб. (Утверждение).
    //    /// </summary>
    //    [DetailView(Name = "Остаточная стоимость ННА, руб. (Утверждение)", TabName = AccountingObject.TabName2)]
    //    public decimal? ResidualCostStatement { get; set; }

    //    /// <summary>
    //    /// Дата определения остаточной стоимости (Утверждение).
    //    /// </summary>
    //    [DetailView(Name = "Дата определения остаточной стоимости (Утверждение)", TabName = AccountingObject.TabName2)]
    //    public DateTime? ResidualCostDateStatement { get; set; }

    //    /// <summary>
    //    /// Остаточная стоимость ННА, руб.
    //    /// </summary>
    //    [ListView(Name = "Остаточная стоимость ННА, руб.")]
    //    [DetailView(Name = "Остаточная стоимость ННА, руб.", TabName = AccountingObject.TabName2)]
    //    public decimal? ResidualCost { get; set; }

    //    /// <summary>
    //    /// Дата определения остаточной стоимости.
    //    /// </summary>
    //    [ListView(Name = "Дата определения остаточной стоимости")]
    //    [DetailView(Name = "Дата определения остаточной стоимости", TabName = AccountingObject.TabName2)]
    //    public DateTime? ResidualCostDate { get; set; }

    //    /// <summary>
    //    ///     Получает или задает связанные одобрения реализации ННА.
    //    /// </summary>
    //    //[DetailView(Name = "Одобрения реализации ННА", TabName = "[1]Одобрения реализации ННА", HideLabel = true)]
    //    //public virtual ICollection<NonCoreAssetSaleAccept> Accepts { get; set; }

    //    /// <summary>
    //    ///     Получает или задает связанные оценки ННА.
    //    /// </summary>
    //    //[DetailView(Name = "Оценки ННА", TabName = "[2]Оценки ННА", HideLabel = true)]
    //    //public virtual ICollection<NonCoreAssetAppraisal> NonCoreAssetAppraisals { get; set; }

    //    ///// <summary>
    //    /////     Получает или задает ИД реализации ННА.
    //    ///// </summary>
    //    //public int? NonCoreAssetSaleID { get; set; }

    //    ///// <summary>
    //    /////     Получает или задает реализацию ННА.
    //    ///// </summary>
    //    //[FullTextSearchProperty]
    //    //[ListView]
    //    //[DetailView(Name = "Реализация ННА", TabName = CaptionHelper.DefaultTabName)]
    //    //public virtual NonCoreAssetSale Implementation { get; set; }
    //    #endregion

    //    #region Поля проецируемые из AccountingObject
    //    /// <summary>
    //    /// Получает или задает первоначальную стоимость в руб.
    //    /// </summary>
    //    //
    //    [ListView(Order = 8)]
    //    [DetailView(Name = "Первоначальная стоимость, руб.",
    //    TabName = AccountingObject.TabName2, ReadOnly = !SibiAssemblyInfo.FullAccess, Order = 33)]
    //    [DefaultValue(0)]
    //    public decimal? InitialCost { get; set; }
    //    #endregion

    //}

    ////public class NonCoreAssetListItemAndNonCoreAssetAppraisal : ManyToManyAssociation<NonCoreAssetAndList, NonCoreAssetAppraisal>
    ////{
    ////}

    ////public class NonCoreAssetListItemAndNonCoreAssetSaleAccept : ManyToManyAssociation<NonCoreAssetAndList, NonCoreAssetSaleAccept>
    ////{
    ////}

}
