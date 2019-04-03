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
    /// Представляет формат выписки ЕГРН/ЕГРП.
    /// </summary>
    [EnableFullTextSearch]
    public class ExtractFormat : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractFormat.
        /// </summary>
        public ExtractFormat()
        {
        }
    }
}
