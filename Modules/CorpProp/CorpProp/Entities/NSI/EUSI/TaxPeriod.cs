using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник "Налоговый период")
    /// </summary>
    [EnableFullTextSearch]
    public class TaxPeriod : DictObject
    {
        public TaxPeriod() : base()
        {

        }
    }
}
