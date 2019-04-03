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
    /// Представляет статус запроса.
    /// </summary>
    [EnableFullTextSearch]
    public class RequestStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса RequestStatus.
        /// </summary>
        public RequestStatus()
        {

        }

    }
}
