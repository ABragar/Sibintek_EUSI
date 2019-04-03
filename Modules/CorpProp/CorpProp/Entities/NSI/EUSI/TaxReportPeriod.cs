using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник "Отчетный период по налогу")
    /// </summary>
    [EnableFullTextSearch]
    public class TaxReportPeriod : DictObject
    {
        public TaxReportPeriod() : base()
        {

        }
    }
}
