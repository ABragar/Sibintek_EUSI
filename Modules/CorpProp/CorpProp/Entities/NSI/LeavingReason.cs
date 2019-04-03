using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник причин выбытия объектов БУ.
    /// </summary>
    [EnableFullTextSearch]
    public class LeavingReason : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса LeavingReason.
        /// </summary>
        public LeavingReason()
        {

        }
    }
}
