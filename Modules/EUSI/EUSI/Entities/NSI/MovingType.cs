using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.NSI
{
   
    /// <summary>
    /// Справочник видов движения (операций).
    /// </summary>
    [EnableFullTextSearch]
    public class MovingType : DictObject
    {
        public MovingType() : base() { }
    }
}
