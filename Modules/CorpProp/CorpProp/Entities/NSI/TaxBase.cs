using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник налоговой базы.
    /// </summary>
    [EnableFullTextSearch]
    public class TaxBase : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса TaxBase.
        /// </summary>
        public TaxBase()
        {

        }
    }
}
