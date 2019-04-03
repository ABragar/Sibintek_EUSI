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
    /// Справочник ракурсов.
    /// </summary>
    [EnableFullTextSearch]
    public class Angle : DictObject
    {
        public Angle() : base() { }
    }
}
