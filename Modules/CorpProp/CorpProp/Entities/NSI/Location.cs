using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник местоположения.
    /// </summary>
    /// <remarks>
    /// Внешний справочник (КИС САП РН).
    /// </remarks>    
    [EnableFullTextSearch]
    public class SibLocation : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibLocation.
        /// </summary>
        public SibLocation()
        {

        }
    }
}
