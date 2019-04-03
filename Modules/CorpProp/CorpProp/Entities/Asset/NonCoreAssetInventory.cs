using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;

namespace CorpProp.Entities.Asset
{
    [EnableFullTextSearch]
    public class NonCoreAssetInventory : DictObject
    {
        public const string TabName2 = "[2]Перечни ННА";

        public const string TabName3 = "[3]Объекты ННА";

       

        //private static readonly CompiledExpression<NonCoreAssetInventory, IEnumerable<NonCoreAsset>> _NonCoreAssets =
        //    DefaultTranslationOf<NonCoreAssetInventory>.Property(x => x.NonCoreAssets).Is(x =>
        //        x.NonCoreAssetLists.SelectMany(p => p.NonCoreAssetAndLists.Select(q => q.NonCoreAsset)));

        public NonCoreAssetInventory()
        {
            //NonCoreAssetLists = new List<NonCoreAssetList>();
        }

        /// <summary>
        /// Получает или задает год графика.
        /// </summary>
        [ListView(Width = 100)]
        [DetailView("Год", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType("Sib_Year")]
        [FullTextSearchProperty]
        public int? Year { get; set; }


        public int? SocietyID { get; set; }

        //[DetailView(Name = "Общество Группы", Order = 8)]
        public Society Society { get; set; }

        [DetailView(Name = "Вид реестра ННА", TabName = CaptionHelper.DefaultTabName, Order = 9)]
        public NonCoreAssetInventoryType NonCoreAssetInventoryType { get; set; }

        [DetailView(Name = "Статус реестра ННА", TabName = CaptionHelper.DefaultTabName, Order = 10)]
        public NonCoreAssetInventoryState NonCoreAssetInventoryState { get; set; }

        //[DetailView(Name = "Наименование Органа Управления Общества",
        //    Description = "Наименование Органа Управления Общества, принявшего решение об отчуждении имущества",
        //    Order = 7)]
        //[PropertyDataType(PropertyDataType.Text)]
        public string NameManagementCompany { get; set; }

        [DetailView(Name = "Реквизиты документа (№, дата)", TabName = CaptionHelper.DefaultTabName, Order = 12)]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocumentDetails { get; set; }

        /// <summary>
        /// Получает или задает ИД документа.
        /// </summary>
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает документ.
        /// </summary>
        [DetailView("Документ", Order = 11,
        TabName = CaptionHelper.DefaultTabName)]
        [ListView(Hidden = true)]
        public FileCard FileCard { get; set; }

        //public ICollection<NonCoreAssetList> NonCoreAssetLists { get; set; }

        //[DetailView(HideLabel = true, ReadOnly = true, TabName = TabName3)]
        //public IEnumerable<NonCoreAsset> NonCoreAssets => _NonCoreAssets.Evaluate(this);
    }
}
