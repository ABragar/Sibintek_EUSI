using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник бизнес-блоков.
    /// </summary>
    [EnableFullTextSearch]
    public class BusinessBlock : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BusinessBlock.
        /// </summary>
        public BusinessBlock()
        {
        }
    }
}
