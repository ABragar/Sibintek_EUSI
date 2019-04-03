using Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events.Auth
{
    public interface IAuthResult
    {
        int? UserId { get; }
        string Login { get; }
        AuthStatus Status { get; }
    }
}
