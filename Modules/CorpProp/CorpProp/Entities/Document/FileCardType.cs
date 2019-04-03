using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Document
{
    /// <summary>
    /// Инициализирует новый экземпляр класса тип документа
    /// </summary>
    [EnableFullTextSearch]
    public class FileCardType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса FileCardType
        /// </summary>
        public FileCardType(): base()
        {

        }

        public FileCardType(string name) : base()
        {
            Name = name;
        }
    }
}
