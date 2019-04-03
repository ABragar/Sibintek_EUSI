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
    /// Представляет справочник оснований для исключения из периметра.
    /// </summary>
    [EnableFullTextSearch]
    public class BaseExclusionFromPerimeter : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BaseExclusionFromPerimeter.
        /// </summary>
        public BaseExclusionFromPerimeter()
        {

        }
        
    }
}
