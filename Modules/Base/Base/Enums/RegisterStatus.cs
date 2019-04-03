using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Enums
{
    [UiEnum]
    public enum RegisterStatus
    {
        [UiEnumValue("Новый пользователь зарегистирован.")]
        AccountRegistered,
        [UiEnumValue("Не удалось зарегистрировать нового пользователя.")]
        Failure
    }
}
