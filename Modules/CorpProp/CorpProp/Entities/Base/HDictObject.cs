using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Base
{
    /// <summary>
    /// Предоставляет дополнительные свойства иерархичного справочника Системы.
    /// </summary>
    public interface IHDictObject
    {
        /// <summary>
        /// Код.
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Дата начала действия.
        /// </summary>        
        DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата окончания действия.
        /// </summary>       
        DateTime? DateTo { get; set; }
    }



    /// <summary>
    /// Представляет базовый класс иерархичных справочников Системы.
    /// </summary>
    public abstract class HDictObject : HCategory, IHDictObject
    {
        /// <summary>
        /// Получает или задает код.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Код", Order = 2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Code { get; set; }

        /// <summary>
        /// Получает или задает заголовок.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Visible = false)]
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия.
        /// </summary>
        [DetailView(Name = "Дата начала", Required = true)]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия.
        /// </summary>
        [DetailView(Name = "Дата окончания")]
        public DateTime? DateTo { get; set; }
    }
}
