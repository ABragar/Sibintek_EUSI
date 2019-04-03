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
    /// Период НУ
    /// </summary>
    [EnableFullTextSearch]
    public class PeriodNU : DictObject
    {

        public PeriodNU() : base(){ }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
