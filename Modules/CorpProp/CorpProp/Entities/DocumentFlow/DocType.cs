using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.DocumentFlow
{
    /// <summary>
    /// Инициализирует новый экземпляр класса тип регистрируемого документа
    /// </summary>
    [EnableFullTextSearch]
    public class DocType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DocType.
        /// </summary>
        public DocType()
        {

        }
    }
}
