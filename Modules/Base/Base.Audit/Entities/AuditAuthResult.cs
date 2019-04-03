using Base.Attributes;
using Base.Events.Auth;
using Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Audit.Entities
{
    [ViewModelConfig(Title = "Cобытие авторизации", IsReadOnly = true)]
    public class AuditAuthResult : BaseObject, IAuthResult
    {
        public AuditAuthResult(string login, AuthStatus status, int? userId = null)
        {
            Login = login;
            Status = status;
            UserId = userId;
        }

        [DetailView("Логин пользователя", ReadOnly = true, Required = true, Order = 1)]
        public string Login { get; private set; }

        [DetailView("Результат авторизации в системе", ReadOnly = true, Required = true, Order = 2)]
        public AuthStatus Status { get; private set; }

        public int? UserId { get; private set; }
    }
}
