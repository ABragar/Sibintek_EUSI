using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник "Наименование налога")
    /// </summary>
    [EnableFullTextSearch]
    public class TaxName : DictObject
    {
        public TaxName() : base()
        {
            
        }
    }
}
