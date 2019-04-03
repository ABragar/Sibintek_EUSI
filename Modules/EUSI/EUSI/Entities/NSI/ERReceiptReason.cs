using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.NSI
{
    /// <summary>
    /// Справочник способов поступления для заявки на регистрацию
    /// </summary>
    /// <remarks>
    /// Отличается от ReceiptReason.
    /// </remarks>
    public class ERReceiptReason : DictObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ERReceiptReason.
        /// </summary>
        public ERReceiptReason() : base()
        {

        }
    }
}
