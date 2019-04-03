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
    /// Представляет справочник производственных блоков.
    /// </summary>
    [EnableFullTextSearch]
    public class ProductionBlock : DictObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ProductionBlock.
        /// </summary>
        public ProductionBlock()
        {

        }
    }
}
