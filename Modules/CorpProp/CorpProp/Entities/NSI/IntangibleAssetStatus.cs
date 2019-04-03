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
    /// Статус НМА.
    /// </summary>
    [EnableFullTextSearch]
    public class IntangibleAssetStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса IntangibleAssetStatus.
        /// </summary>
        public IntangibleAssetStatus()
        {

        }
    }
}
