using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Asset
{
    /// <summary>
    /// Представляет справочник статусов реестра ННА
    /// </summary>   
    [EnableFullTextSearch]
    public class NonCoreAssetInventoryState : DictObject
    {
        public NonCoreAssetInventoryState()
        {
        }
    }
}
