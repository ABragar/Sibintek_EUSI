using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Enums
{
    [UiEnum]
    public enum DefaultImage
    {
        [UiEnumValue("NoImage")]
        NoImage = 10,
        [UiEnumValue("NoPhoto")]
        NoPhoto = 20
    }
}
