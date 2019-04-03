using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник бизнес-сфер.
    /// </summary>
    [EnableFullTextSearch]
    public class BusinessArea : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BusinessArea.
        /// </summary>
        public BusinessArea()
        {

        }
    }
}
