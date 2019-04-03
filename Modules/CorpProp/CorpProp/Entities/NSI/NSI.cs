using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник справочников.
    /// </summary>
    [EnableFullTextSearch]
    public class NSI : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NSI.
        /// </summary>
        public NSI(): base()
        {

        }

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [FullTextSearchProperty]
        [MaxLength(255)]
        [ListView]
        [DetailView("Наименование", Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }  
        
        /// <summary>
        /// Получает или задает ИД типа.
        /// </summary>
        [SystemProperty]
        public int? NSITypeID { get; set; }

        /// <summary>
        /// Получает или задает тип.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Тип")]
        public virtual NSIType NSIType { get; set; }

        /// <summary>
        /// Получает или задает категорию.
        /// </summary>        
        [FullTextSearchProperty]
        [MaxLength(100)]
        [DetailView("Категория", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Category { get; set; }

        /// <summary>
        /// Получает или задает мнемонику.
        /// </summary>
        [FullTextSearchProperty]
        [MaxLength(100)]
        [ListView(Hidden = true)]
        [DetailView("Мнемоника")]
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Получает или задает URL.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView("URL")]
        [MaxLength(255)]
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string URL { get; set; }

    }
}
