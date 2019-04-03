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
    /// Представляет плоский справочник единиц измерения.
    /// </summary>
    [EnableFullTextSearch]
    public class SibMeasure : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibMeasure.
        /// </summary>
        public SibMeasure() : base() { }

        /// <summary>
        /// Получает или задает Группу.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Получает или задает Группу.
        /// </summary>
        public string Measure { get; set; }
        
        /// <summary>
        /// Получает или задает Группу.
        /// </summary>
        public string SubCodeNation { get; set; }

        /// <summary>
        /// Получает или задает Группу.
        /// </summary>
        public string SubCodeInter { get; set; }

        /// <summary>
        /// Получает или задает Группу.
        /// </summary>
        public string CodeNation { get; set; }

        /// <summary>
        /// Получает или задает Группу.
        /// </summary>
        public string CodeInter { get; set; }
    }
}
