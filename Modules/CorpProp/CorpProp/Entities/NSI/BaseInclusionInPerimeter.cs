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
    /// Представляет справочник оснований для включения в периметр.
    /// </summary>
    [EnableFullTextSearch]
    public class BaseInclusionInPerimeter : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BaseInclusionInPerimeter.
        /// </summary>
        public BaseInclusionInPerimeter()
        {

        }
        
    }
}
