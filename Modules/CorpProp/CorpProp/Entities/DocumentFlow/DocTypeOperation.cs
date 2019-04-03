using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.DocumentFlow
{
    /// <summary>
    /// Представляет справочник типов операций отраженных в регистрационных карточеках документов.
    /// </summary>
    [EnableFullTextSearch]
    public class DocTypeOperation : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DocTypeOperation.
        /// </summary>
        public DocTypeOperation()
        {

        }
    }
}
