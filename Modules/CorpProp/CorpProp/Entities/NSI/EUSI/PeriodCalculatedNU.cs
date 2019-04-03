using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{

    [UiEnum]
    public enum PeriodCalculatedNU
    {
        [UiEnumValue("Год")]
        Year = 4,
        [UiEnumValue("1 квартал")]
        Quarter1 = 1,
        [UiEnumValue("2 квартал")]
        Quarter2 = 2,
        [UiEnumValue("3 квартал")]
        Quarter3 = 3
    }
}
