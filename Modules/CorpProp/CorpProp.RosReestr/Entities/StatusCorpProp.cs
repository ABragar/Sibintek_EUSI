using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Статус обновления записи в АИС КС.
    /// </summary>
    [UiEnum]
    public enum StatusCorpProp
    {  
        [UiEnumValue("Не обновлено")]
        NotUpdated = 0,
        [UiEnumValue("Обновлено")]
        Updated = 1,
        [UiEnumValue("Обновлено частично")]
        NotAll = 2,
    }
}
