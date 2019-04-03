using Base;
using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Asset
{
    
    /// <summary>
    /// Представляет одобрение реализации ННА.
    /// </summary>
    [EnableFullTextSearch]
    public class NonCoreAssetSaleAccept : TypeObject
    {

        /// <summary>
        /// Получает или задает ИД вида одобрения.
        /// </summary>
        [ForeignKey("AcceptType")]
        public int? AcceptTypeID { get; set; }

        /// <summary>
        /// Получает или задает вид одобрения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Вид одобрения", Required = true, TabName = CaptionHelper.DefaultTabName)]
        public virtual NonCoreAssetSaleAcceptType AcceptType { get; set; }

        /// <summary>
        /// Получает или задает дату решения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Дата решения", Required = true, TabName = CaptionHelper.DefaultTabName, Order =2)]
        public DateTime? AcceptDate { get; set; }

        /// <summary>
        /// Получает или задает описание органа, принявшего решение.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Орган, принявший решение", Required = true, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Governance { get; set; }


        /// <summary>
        /// Получает или задает наименование решения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Наименование решения", Required = true, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Name { get; set; }

        /// <summary>
        /// Получает или задает номер решения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Номер решения", TabName = CaptionHelper.DefaultTabName, Order = 2)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Number { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>   
        [ListView(Hidden = true)]
        [DetailView("Примечание", TabName = CaptionHelper.DefaultTabName)]
        public String Description { get; set; }

        /// <summary>
        /// Получает или задает ИД документа.
        /// </summary>      
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает документ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Документ", Required = true, TabName = CaptionHelper.DefaultTabName)]
        public virtual FileCard FileCard { get; set; }


        /// <summary>
        /// Получает или задает связанные строки реестров ННА.
        /// </summary>
        //[DetailView(Name = "Строки перечней ННА", TabName = "[1]Строки перечней ННА", HideLabel = true)]
        //public virtual ICollection<NonCoreAssetAndList> NonCoreAssetListItems { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetSaleAccept.
        /// </summary>
        public NonCoreAssetSaleAccept()
        {
            //NonCoreAssetListItems = new List<NonCoreAssetAndList>();
        }
    }
}
