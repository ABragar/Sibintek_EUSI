using Base;
using Base.Attributes;
using Newtonsoft.Json;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник классов имущественных комплексов.
    /// </summary>
    [EnableFullTextSearch]
    public class PropertyComplexKind : DictObject
    {              

       
        /// <summary>
        /// Инициализирует новый экземпляр класса PropertyComplexKind.
        /// </summary>
        public PropertyComplexKind()
        {

        }
    }
}
