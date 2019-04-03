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
    /// Представляет справочник типов характеристик.
    /// </summary>
    [EnableFullTextSearch]
    public class FeatureType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса FeatureType.
        /// </summary>
        public FeatureType()
        {

        }
    }
}
