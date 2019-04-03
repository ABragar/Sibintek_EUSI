﻿using System;

namespace Base.Enums
{
    [Flags]
    public enum TypePermission
    {
        Read = 1,
        Write = 2,
        Create = 4,
        Delete = 8,
        Navigate = 16
    }
}
