using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Helpers;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник валют.
    /// </summary>
    [EnableFullTextSearch]
    public class Currency : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Currency.
        /// </summary>
        public Currency()
        {

        }

        /// <summary>
        /// Получает или задает буквенный код валюты.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Буквенный код", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public String LetterCode { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Страна", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        public String Description { get; set; }
        /// <summary>
        /// Получает или задает знак.
        /// </summary>
        [DetailView(Name = "Знак", Order = 3, TabName = CaptionHelper.DefaultTabName)]
        public System.String Sign { get; set; }

        /// <summary>
        /// Получает или задает начало действия.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата начала действия", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Получает или задает окончание действия.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата окончание действия", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? EndDate { get; set; }

        
    }
}
