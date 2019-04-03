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
    /// Представляет справочник ОКПО.
    /// </summary>
    [EnableFullTextSearch]
    public class OKPO : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса OKPO.
        /// </summary>
        public OKPO()
        {

        }
    }
}
