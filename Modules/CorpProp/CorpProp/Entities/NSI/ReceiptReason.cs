using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник причин поступления объектов БУ.
    /// </summary>
    /// <remarks>
    /// Внутренний справочник: приобретение, правопреемство, строительство и пр.
    /// </remarks>
    [EnableFullTextSearch]
    public class ReceiptReason : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ReceiptReason.
        /// </summary>
        public ReceiptReason()
        {

        }
    }
}
