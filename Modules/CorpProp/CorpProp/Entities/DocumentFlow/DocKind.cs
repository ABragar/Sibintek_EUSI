using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.DocumentFlow
{
    /// <summary>
    /// Инициализирует новый экземпляр класса вид регистрируемого документа
    /// </summary>
    [EnableFullTextSearch]
    public class DocKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DocKind.
        /// </summary>
        public DocKind()
        {

        }
    }
}
