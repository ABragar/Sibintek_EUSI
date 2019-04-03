using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Mapping
{
    /// <summary>
    /// Мэппинг кодов ОКОФ с типами ОИ.
    /// </summary>
    [EnableFullTextSearch]
    public class OKOFEstates : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса OKOFEstates.
        /// </summary>
        public OKOFEstates():base() { }

        /// <summary>
        /// Получает или задает ОКОФ 94.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "ОКОФ 2014", Order = 1)]
        public OKOF2014 OKOF2014 { get; set; }

        /// <summary>
        /// Получает или задает тип объекта имущества - мнемонику ОИ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Тип объекта имущества", Order = 2)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string EstateType { get; set; }
    }
}
