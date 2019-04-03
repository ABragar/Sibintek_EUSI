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
    /// Представляет справочник видов перечня ННА.
    /// </summary>
    [EnableFullTextSearch]
    public class NonCoreAssetListKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetListKind.
        /// </summary>
        public NonCoreAssetListKind()
        {

        }
    }
}
