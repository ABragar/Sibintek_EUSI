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
    /// Представляет вид движения/изменения объекта БУ.
    /// </summary>
    [EnableFullTextSearch]
    public class AccountingMovingType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingMovingType.
        /// </summary>
        public AccountingMovingType()
        {

        }
    }
}
