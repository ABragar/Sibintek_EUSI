using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник типов НМА.
    /// </summary>
    [EnableFullTextSearch]
    public class IntangibleAssetType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса IntangibleAssetType.
        /// </summary>
        public IntangibleAssetType()
        {

        }
    }
}
