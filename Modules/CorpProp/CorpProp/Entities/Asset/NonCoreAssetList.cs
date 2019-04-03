using CorpProp.Entities.Base;
using SubjectObject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using CorpProp.Helpers;
using CorpProp.Entities.Document;
using System.ComponentModel.DataAnnotations.Schema;
using CorpProp.Entities.Subject;
using CorpProp.Entities.NSI;
using CorpProp.Attributes;
using Base.Utils.Common.Attributes;
using Base.Translations;

namespace CorpProp.Entities.Asset
{
    
    /// <summary>
    /// Представляет перечень ННА.
    /// </summary>
    /// <remarks>
    /// Перечень или реестр ННА, проходящий процедуру согласования или реализации. 
    /// Реестры, формируемые исключительно для целей отчётности (сводные, аналитические и пр.), 
    /// создаются в подсистеме "Отчёты" и не порождают экземпляров данной сущности.
    /// </remarks>
    [EnableFullTextSearch]
    public class NonCoreAssetList : TypeObject
    {
        private static readonly CompiledExpression<NonCoreAssetList, string> _Name =
            DefaultTranslationOf<NonCoreAssetList>.Property(x => x.Name)
                .Is(x => x.Society != null ? x.Society.ShortName  : "" );

        [ListView(Visible = false)]
        [DetailView(Visible = false)]
        public string Name => _Name.Evaluate(this);

        /// <summary>
        /// Получает или задает год графика.
        /// </summary>
        [ListView(Width =100)]
        [DetailView("Год", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType("Sib_Year")]
        [FullTextSearchProperty]
        public int? Year { get; set; }

        /// <summary>
        /// Получает или задает ИД Реестра ННА.
        /// </summary>
        public int? NonCoreAssetInventoryID { get; set; }

        /// <summary>
        /// Получает или задает реестр ННА.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Реестр ННА", Order = 2,
            TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public NonCoreAssetInventory NonCoreAssetInventory { get; set; }

        /// <summary>
        /// Получает или задает идентификатор Общества группы.
        /// </summary>       
        [SystemProperty]
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает Общество группы.
        /// </summary>   
        [ListView]
        [DetailView(Name = "Общество группы", Required = true, Order = 3,
        TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public Society Society { get; set; }

        public int? NonCoreAssetListStateID { get; set; }

        [ListView]
        [DetailView(Name = "Статус перечня", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public NonCoreAssetListState NonCoreAssetListState { get; set; }

        /// <summary>
        /// Получает или задает дату реестра.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Дата перечня", Required = true, Order = 5,
        TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public DateTime ListDate { get; set; }

        [FullTextSearchProperty]
        [ListView]
        [DetailView("Срок предоставления", Required = true, Order = 6, TabName = CaptionHelper.DefaultTabName)]
        public DateTime AvailabilityDeadline { get; set; }

        [FullTextSearchProperty]
        [ListView]
        [DetailView("Срок утверждения", Required = true, Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public DateTime ApprovalDeadline { get; set; }

        /// <summary>
        /// Получает или заадает ИД типа ННА.
        /// </summary>
        public int? NonCoreAssetListTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип ННА.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Тип перечня", Required = true, Order = 8,
         TabName = CaptionHelper.DefaultTabName)]
        public NonCoreAssetListType NonCoreAssetListType { get; set; }

        /// <summary>
        /// Получает или задает ИД вида ННА.
        /// </summary>
        public int? NonCoreAssetListKindID { get; set; }

        /// <summary>
        /// Получает или задает вид ННА.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Вид перечня", Required = true, Order = 9,
        TabName = CaptionHelper.DefaultTabName)]
        public NonCoreAssetListKind NonCoreAssetListKind { get; set; }
        
        /// <summary>
        /// Получает или задает ИД документа.
        /// </summary>
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает документ.
        /// </summary>
        [DetailView("Документ", Order = 10,
        TabName = CaptionHelper.DefaultTabName)]
        [ListView(Hidden = true)]
        public FileCard FileCard { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Примечание", Order = 11,
        TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public String Description { get; set; }

        /// <summary>
        /// Коллекция записей строк реестра.
        /// </summary>
        //
        //[DetailView(Name = "Строки перечня", TabName = "[1]Строки перечня", HideLabel = true)]
        //public ICollection<NonCoreAssetAndList> NonCoreAssetAndLists { get; set; }





        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetList.
        /// </summary>
        public NonCoreAssetList()
        {
        }

        /// <summary>
        /// Копирует данные из объекта old в текущий экземпляр.
        /// </summary>
        /// <param name="old"></param>
        public void CopyFrom(NonCoreAssetList old)
        {
            this.ApprovalDeadline = old.ApprovalDeadline;
            this.AvailabilityDeadline = old.AvailabilityDeadline;
            this.Description = old.Description;
            this.FileCardID = old.FileCardID;
            this.ListDate = old.ListDate;
            this.NonCoreAssetInventoryID = old.NonCoreAssetInventoryID;
            this.NonCoreAssetListKindID = old.NonCoreAssetListKindID;
            this.NonCoreAssetListStateID = old.NonCoreAssetListStateID;
            this.NonCoreAssetListTypeID = old.NonCoreAssetListTypeID;
            this.SocietyID = old.SocietyID;
            this.Year = old.Year;
        }
    }
}
