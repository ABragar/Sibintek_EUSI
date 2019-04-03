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
    public enum EstateStatus
    {
        [UiEnumValue("Не определен")]
        Undefined = 0,
        [UiEnumValue("Создан")]
        Create = 1,
        [UiEnumValue("Дообогащён")]
        Update = 2,
        [UiEnumValue("В архиве")]
        Archive = 3
    }


}
