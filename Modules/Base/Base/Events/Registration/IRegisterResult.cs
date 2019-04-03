using Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Registration
{
    public interface IRegisterResult
    {
        string Login { get; }
        RegisterStatus Status { get; }
        int? UserId { get; }
    }
}
