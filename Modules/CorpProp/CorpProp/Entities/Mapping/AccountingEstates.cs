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
    /// Представляет таблицу мэппинга класса БУ
    /// </summary>
    [EnableFullTextSearch]
    public class AccountingEstates : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingEstates.
        /// </summary>
        public AccountingEstates() : base()
        {

        }
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Класс БУ", Order = 1)]
        public ClassFixedAsset ClassFixedAsset { get; set; }

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Тип объекта имущества", Order = 2)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string EstateType { get; set; }
    }
}
