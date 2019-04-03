using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Analyze.Entities.NSI
{
    /// <summary>
    /// Представляет справочник строк бюджета
    /// </summary>
    [EnableFullTextSearch]
    public class BudgetLine: DictObject
    {
        public BudgetLine():base()
        {
        }
    }
}