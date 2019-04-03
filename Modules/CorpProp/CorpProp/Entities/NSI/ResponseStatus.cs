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
    /// Представляет справочник статусов ответа на запрос информации.
    /// </summary>
    [EnableFullTextSearch]
    public class ResponseStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ResponseStatus.
        /// </summary>
        public ResponseStatus()
        {

        }
    }
}
