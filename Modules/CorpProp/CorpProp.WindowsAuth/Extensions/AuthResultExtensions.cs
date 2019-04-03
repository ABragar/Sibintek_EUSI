using Base.Entities;
using Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Extensions
{
    public static class AuthResultExtensions
    {
        public static AuthResult FailedNotInRole(this AuthResult authResult, int? userId, string login, params string[] messages)
        {
            return new AuthResult(userId, login, AuthStatus.FailureNotInRole, messages);
        }
    }
}
