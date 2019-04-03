using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет строку выписку.
    /// </summary>
    [EnableFullTextSearch]
    public class ExtractItem : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractItem.
        /// </summary>
        public ExtractItem()
        {
        }

        /// <summary>
        /// Получает или задает ИД выписки ЕГРП/ЕГРН.
        /// </summary>
        public int? ExtractID { get; set; }

        /// <summary>
        /// Получает или задает выписку ЕГРП/ЕГРН.
        /// </summary>      
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Выписка ЕГРН", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual Extract Extract { get; set; }
    }
}
