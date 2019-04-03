using Base.Attributes;
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
    /// Да/Нет.
    /// </summary>  
    [UiEnum]
    public enum YesNo 
    {
        [UiEnumValue("Нет")]
        No = 0,
        [UiEnumValue("Да")]
        Yes = 1
    }
}
