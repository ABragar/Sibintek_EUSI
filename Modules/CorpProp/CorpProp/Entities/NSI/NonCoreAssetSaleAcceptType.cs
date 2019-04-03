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
    /// Представляет справочник видов одобрений реализаций ННА.
    /// </summary>
    [EnableFullTextSearch]
    public class NonCoreAssetSaleAcceptType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetSaleAcceptType.
        /// </summary>
        public NonCoreAssetSaleAcceptType()
        {

        }
    }
}
