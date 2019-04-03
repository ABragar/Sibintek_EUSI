using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Accounting
{
    /// <summary>
    /// Представляет справочник БИК.
    /// </summary>
    [EnableFullTextSearch]
    public class BIK : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BIK.
        /// </summary>
        public BIK(): base()
        {

        }
    }
}
