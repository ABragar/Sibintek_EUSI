using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    public class RSBU : DictObject
    {
        public RSBU()
        {

        }

        [ListView]
        [DetailView(Name = "Номерсчета",TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountNumber { get; set; }
    }
}
