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
    /// Представляет справочник организационно-правовая форма.
    /// </summary>
    [EnableFullTextSearch]
    public class OPF : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса OPF.
        /// </summary>
        public OPF()
        {

        }
    }
}
