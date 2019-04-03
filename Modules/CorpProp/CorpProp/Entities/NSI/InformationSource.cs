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
    /// Представляет справочник источников информации.
    /// </summary>
    [EnableFullTextSearch]
    public class InformationSource : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса InformationSource.
        /// </summary>
        public InformationSource()
        {

        }
    }
}
