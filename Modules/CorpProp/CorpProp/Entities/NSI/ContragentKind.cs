using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник видов контрагентов.
    /// </summary>
    [EnableFullTextSearch]
    public class ContragentKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ContragentKind.
        /// </summary>
        public ContragentKind()
        {

        }
    }
}
