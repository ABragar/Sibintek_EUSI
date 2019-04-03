using Base;
using Base.Attributes;
using CorpProp.Entities.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Law
{
    [EnableFullTextSearch]
    public class ScheduleStateYear : Base.DictObject
    {
        public ScheduleStateYear()
        {
        }
    }
}
