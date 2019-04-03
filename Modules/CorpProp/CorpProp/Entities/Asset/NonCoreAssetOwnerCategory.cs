using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Asset
{
    [EnableFullTextSearch]
    public class NonCoreAssetOwnerCategory : DictObject
    {
        public NonCoreAssetOwnerCategory()
        {
        }
    }
}
