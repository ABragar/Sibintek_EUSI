using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Analyze.Entities.NSI
{
    /// <summary>
    /// Представляет справочник финансовых показателей
    /// </summary>
    [EnableFullTextSearch]
    public class FinancialIndicator:DictObject
    {
        public FinancialIndicator():base()
        {
        }
    }
}