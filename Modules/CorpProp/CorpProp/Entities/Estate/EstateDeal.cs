using Base.Attributes;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using EstateObject = CorpProp.Entities.Estate;
using DocumentObject = CorpProp.Entities.Document;
using CorpProp.Entities.Base;
using CorpProp.Entities.DocumentFlow;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет предмет сделки.
    /// </summary>
    [EnableFullTextSearch]
    public class EstateDeal : TypeObject
    {
     
        /// <summary>
        /// Инициализирует новый экземпляр класса EstateDeal.
        /// </summary>
        public EstateDeal() { }
       
        /// <summary>
        /// Получает или задает сумму сделки по объекту.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сумма", Order = 0, TabName = CaptionHelper.DefaultTabName)]
        public decimal? SumDealByEstate { get; set; }

        /// <summary>
        /// Получает или задает ИД сделки.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public int? SibDealID { get; set; }

        /// <summary>
        /// Получает или задает сделку.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сделка", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SibDeal SibDeal { get; set; }

        /// <summary>
        /// Получает или задает ИД объекта имущества.
        /// </summary>
        public int? EstateID { get; set; }

        /// <summary>
        /// Получает или задает связь с объектом имущества.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Объект имущества", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual EstateObject.Estate Estate { get; set; }
    }
}
