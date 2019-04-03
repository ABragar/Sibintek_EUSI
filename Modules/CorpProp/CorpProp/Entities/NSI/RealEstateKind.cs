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
    /// Представляет справочник видов объектов недвижимого имущества.
    /// </summary>
    [EnableFullTextSearch]
    public class RealEstateKind : DictObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RealEstateKind.
        /// </summary>
        public RealEstateKind()
        {

        }


    }
}
