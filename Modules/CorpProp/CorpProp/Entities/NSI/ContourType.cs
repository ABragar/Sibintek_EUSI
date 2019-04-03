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
    /// Представляет справочник типов контура ОНИ.
    /// </summary>
    [EnableFullTextSearch]
    public class ContourType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ContourType.
        /// </summary>
        public ContourType() : base() { }

        public ContourType(string name) : base(name) { }
    }
}
