using Base.Attributes;
using Base.Translations;
using CorpProp.Attributes;
using CorpProp.Entities.Base;
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
    /// Представляет предложение по реализации одного или группы ННА.
    /// </summary>   
    [EnableFullTextSearch]
    public class NonCoreAssetSaleOffer : TypeObject
    {
        private static readonly CompiledExpression<NonCoreAssetSaleOffer, string> _Name =
         DefaultTranslationOf<NonCoreAssetSaleOffer>.Property(x => x.Name).Is(x => (x.ImplementationWay != null)? x.ImplementationWay.Name : "");

        /// <summary>
        /// Получает или задает ИД способа реализации.
        /// </summary>
        public int? ImplementationWayID { get; set; }

        [ListView(Visible = false)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name => _Name.Evaluate(this);


        /// <summary>
        /// Получает или задает способ реализации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Предполагаемый способ реализации", Required = true,
        TabName = CaptionHelper.DefaultTabName)]
        public virtual ImplementationWay ImplementationWay { get; set; }


        /// <summary>
        /// Получает или задает бюджет.
        /// </summary>
        [DetailView("Предполагаемый бюджет", Required = true,
        TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [ListView]
        [SibDisplayFormat("c")]
        public decimal Budget { get; set; }

        /// <summary>
        /// Получает или задает дату реализации.
        /// </summary>
        [DetailView("Предполагаемый срок", Required = true,
        TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [ListView]
        public DateTime ImplementDate { get; set; }

        /// <summary>
        /// Получает или задает обоснование.
        /// </summary>
        [DetailView("Обоснование", Required = true,
        TabName = CaptionHelper.DefaultTabName)]
        [ListView(Hidden = true)]
        public String Justification { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView("Примечание", Required = true,
        TabName = CaptionHelper.DefaultTabName)]
        [ListView(Hidden = true)]
        public String Description { get; set; }


        /// <summary>
        /// Получает или задает связанные строки реестров ННА.
        /// </summary>
        //[DetailView(Name = "Строки перечней ННА", Required = true,
        //TabName = "[1]Строки перечней ННА", HideLabel = true)]
        //public virtual ICollection<NonCoreAssetAndList> NonCoreAssetListItems { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetSaleOffer.
        /// </summary>
        public NonCoreAssetSaleOffer()
        {
            //NonCoreAssetListItems = new List<NonCoreAssetAndList>();
        }
    }
}
