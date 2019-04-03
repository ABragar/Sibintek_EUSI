using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CorpProp.Entities.Estate;
using System.Xml.Serialization;

namespace CorpProp.Entities.FIAS
{
    /// <summary>
    /// Страна
    /// </summary>
    [Table("SibCountry")]
    [EnableFullTextSearch]
    public class SibCountry : DictObject//TypeObject
    {     

        /// <summary>
        /// Получает или задает официальное наименование.
        /// </summary>
        [DetailView(Name = "Официальное наименование")]
        public String OffName { get; set; }

        /// <summary>
        /// Получает или задает двубуквенный код.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [MaxLength(2)]
        [DetailView(Name = "2-буквенный код")]
        public string TwoLetterCode { get; set; }

        /// <summary>
        /// Получает или задает трехбуквенный код.
        /// </summary>
        [MaxLength(3)]
        [DetailView(Name = "3-буквенный код")]
        public string ThreeLetterCode { get; set; }

       

        /// <summary>
        /// Инициализирует новый экземпляр класса SibCountry.
        /// </summary>
        public SibCountry()
        {
            
        }
    }
}
