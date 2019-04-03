using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.DocumentFlow
{
    /// <summary>
    /// Представляет справочник статусов сделок.
    /// </summary>
    [EnableFullTextSearch]
    public class SibDealStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibDealStatus.
        /// </summary>
        public SibDealStatus()
        {

        }
    }
}
