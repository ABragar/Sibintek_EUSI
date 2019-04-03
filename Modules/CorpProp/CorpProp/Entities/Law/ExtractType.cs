using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет тип выписки ЕГРН/ЕГРП
    /// </summary>
    [EnableFullTextSearch]
    public class ExtractType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractType.
        /// </summary>
        public ExtractType()
        {
        }
    }
}
