using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник назначений ОНИ.
    /// </summary>
    [EnableFullTextSearch]
    public class RealEstatePurpose : DictObject
    {
        public RealEstatePurpose() : base() { }
    }
}
