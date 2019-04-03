using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Mapping
{
    public class RosReestrTypeEstate : TypeObject
    {
        public RosReestrTypeEstate() : base() { }

        [ListView]
        [DetailView(Name = "Код вида ОНИ", Order = 1)]
        public string ObjectTypeCode { get; set; }

        [ListView]
        [DetailView(Name = "Тип объекта имущества", Order = 2)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string EstateMnemonic { get; set; }
    }
}
