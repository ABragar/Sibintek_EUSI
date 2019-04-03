using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник СПП-элементов (структурный план проекта).
    /// </summary>
    [EnableFullTextSearch]
    public class StructurePlan : DictObject
    {
        public StructurePlan() : base()
        {

        }

    }
}
